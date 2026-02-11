#include <iostream>
#include <string>
#include <vector>
#include <sstream>
#include <algorithm>

#ifdef _WIN32
#include <winsock2.h>
#include <ws2tcpip.h>
#pragma comment(lib, "Ws2_32.lib")
using socklen_t = int;
#else
#include <sys/socket.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <netinet/in.h>
#define INVALID_SOCKET (-1)
#define SOCKET int
#define closesocket close
#endif

bool sendAll(SOCKET s, const char* data, int length) {
    int sent = 0;
    while (sent < length) {
        int n = send(s, data + sent, length - sent, 0);
        if (n <= 0) return false;
        sent += n;
    }
    return true;
}

bool sendMessage(SOCKET s, const std::string& msg) {
    uint32_t len = (uint32_t)msg.size();
    uint32_t netlen = htonl(len);
    if (!sendAll(s, reinterpret_cast<const char*>(&netlen), 4)) return false;
    if (len > 0 && !sendAll(s, msg.data(), (int)len)) return false;
    return true;
}

bool recvAll(SOCKET s, char* buf, int length) {
    int recvd = 0;
    while (recvd < length) {
        int n = recv(s, buf + recvd, length - recvd, 0);
        if (n <= 0) return false;
        recvd += n;
    }
    return true;
}

bool recvMessage(SOCKET s, std::string& out) {
    uint32_t netlen;
    if (!recvAll(s, reinterpret_cast<char*>(&netlen), 4)) return false;
    uint32_t len = ntohl(netlen);
    if (len == 0) { out.clear(); return true; }
    out.resize(len);
    if (!recvAll(s, &out[0], (int)len)) return false;
    return true;
}

std::string readLineTrim() {
    std::string s;
    std::getline(std::cin, s);
    while (!s.empty() && (s.back() == '\r' || s.back() == '\n')) s.pop_back();
    return s;
}

void printMenu() {
    std::cout << "\n--- MENU ---\n";
    std::cout << "1. Показать всех студентов\n";
    std::cout << "2. Показать студентов без оценки 3\n";
    std::cout << "3. Добавить студента\n";
    std::cout << "4. Редактировать студента (по индексу)\n";
    std::cout << "5. Удалить студента (по индексу)\n";
    std::cout << "0. Выйти\n";
    std::cout << "Выбор: ";
}

// Утилиты сериализации (такие же, как на сервере)
std::string serializeStudentLocal(const std::string& fio, const std::string& group, double stipend, const std::vector<int>& grades) {
    std::ostringstream oss;
    oss << fio << "|" << group << "|" << stipend << "|";
    for (size_t i = 0;i < grades.size();++i) {
        if (i) oss << ",";
        oss << grades[i];
    }
    return oss.str();
}

void printListResponse(const std::string& resp) {
    // первая строка — количество
    std::istringstream iss(resp);
    std::string line;
    if (!std::getline(iss, line)) return;
    int n = std::stoi(line);
    std::cout << "Найдено: " << n << "\n";
    for (int i = 0;i < n;i++) {
        if (!std::getline(iss, line)) break;
        // формат: idx|fio|group|stipend|g1,g2,...
        std::vector<std::string> parts;
        std::istringstream tss(line);
        std::string cur;
        while (std::getline(tss, cur, '|')) parts.push_back(cur);
        if (parts.size() >= 5) {
            std::cout << "Индекс (сервер): " << parts[0] << "\n";
            std::cout << "  ФИО: " << parts[1] << "\n";
            std::cout << "  Группа: " << parts[2] << "\n";
            std::cout << "  Стипендия: " << parts[3] << "\n";
            std::cout << "  Оценки: " << parts[4] << "\n";
        }
        else {
            std::cout << "Строка: " << line << "\n";
        }
    }
}

int main() {
    setlocale(LC_CTYPE, "rus");
#ifdef _WIN32
    WSADATA wsa;
    WSAStartup(MAKEWORD(2, 2), &wsa);
#endif

    std::string serverIp = "127.0.0.1";
    int port = 54000;

    SOCKET sock = socket(AF_INET, SOCK_STREAM, 0);
    if (sock == INVALID_SOCKET) { std::cerr << "socket error\n"; return 1; }

    sockaddr_in srv{};
    srv.sin_family = AF_INET;
    srv.sin_port = htons(port);
    inet_pton(AF_INET, serverIp.c_str(), &srv.sin_addr);

    if (connect(sock, (sockaddr*)&srv, sizeof(srv)) < 0) {
        std::cerr << "connect failed\n"; return 1;
    }

    std::cout << "Connected to server " << serverIp << ":" << port << "\n";

    while (true) {
        printMenu();
        std::string choice;
        std::getline(std::cin, choice);
        if (choice == "0") break;
        if (choice == "1") {
            sendMessage(sock, "GET_ALL");
            std::string resp;
            if (!recvMessage(sock, resp)) { std::cout << "Connection closed\n"; break; }
            printListResponse(resp);
        }
        else if (choice == "2") {
            sendMessage(sock, "GET_NO_3");
            std::string resp;
            if (!recvMessage(sock, resp)) { std::cout << "Connection closed\n"; break; }
            printListResponse(resp);
        }
        else if (choice == "3") {
            std::cout << "ФИО: "; std::string fio = readLineTrim();
            std::cout << "Группа: "; std::string group = readLineTrim();
            std::cout << "Стипендия: "; std::string st; std::getline(std::cin, st);
            double stipend = std::stod(st);
            std::cout << "Оценки через пробел (например: 5 4 5 4): ";
            std::string gradesLine; std::getline(std::cin, gradesLine);
            std::istringstream gss(gradesLine);
            std::vector<int> grades; int g;
            while (gss >> g) grades.push_back(g);
            std::string payload = serializeStudentLocal(fio, group, stipend, grades);
            std::string cmd = std::string("ADD\n") + payload;
            sendMessage(sock, cmd);
            std::string resp; if (!recvMessage(sock, resp)) break;
            std::cout << resp << "\n";
        }
        else if (choice == "4") {
            std::cout << "Индекс студента (серверный индекс): ";
            std::string idxs; std::getline(std::cin, idxs);
            int idx = std::stoi(idxs);
            std::cout << "Новое ФИО: "; std::string fio = readLineTrim();
            std::cout << "Новая группа: "; std::string group = readLineTrim();
            std::cout << "Новая стипендия: "; std::string st; std::getline(std::cin, st);
            double stipend = std::stod(st);
            std::cout << "Новые оценки через пробел: ";
            std::string gradesLine; std::getline(std::cin, gradesLine);
            std::istringstream gss(gradesLine);
            std::vector<int> grades; int g;
            while (gss >> g) grades.push_back(g);
            std::string payload = serializeStudentLocal(fio, group, stipend, grades);
            std::ostringstream oss;
            oss << "EDIT\n" << idx << "\n" << payload;
            sendMessage(sock, oss.str());
            std::string resp; if (!recvMessage(sock, resp)) break;
            std::cout << resp << "\n";
        }
        else if (choice == "5") {
            std::cout << "Индекс студента (серверный индекс): ";
            std::string idxs; std::getline(std::cin, idxs);
            std::ostringstream oss;
            oss << "DELETE\n" << idxs;
            sendMessage(sock, oss.str());
            std::string resp; if (!recvMessage(sock, resp)) break;
            std::cout << resp << "\n";
        }
        else {
            std::cout << "Неверный выбор\n";
        }
    }

    // Можно послать SHUTDOWN (если хочется остановить сервер)
    // sendMessage(sock, "SHUTDOWN");

    closesocket(sock);
#ifdef _WIN32
    WSACleanup();
#endif
    return 0;
}
