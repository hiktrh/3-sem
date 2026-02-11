#include <iostream>
#include <string>
#include <vector>
#include <thread>
#include <mutex>
#include <sstream>
#include <algorithm>
#include <cstring>
#include <atomic>

#ifdef _WIN32
#define _WINSOCK_DEPRECATED_NO_WARNINGS
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

std::mutex db_mutex;

struct Student {
    std::string fio;
    std::string group;
    double stipend;
    std::vector<int> grades; // оценки
};

std::vector<Student> students;
std::atomic<bool> running{ true };

std::string serializeStudent(const Student& s) {
    // поля разделены '|', оценки через ','
    std::ostringstream oss;
    oss << s.fio << "|" << s.group << "|" << s.stipend << "|";
    for (size_t i = 0;i < s.grades.size();++i) {
        if (i) oss << ",";
        oss << s.grades[i];
    }
    return oss.str();
}

Student deserializeStudent(const std::string& line) {
    Student s;
    std::vector<std::string> parts;
    std::string cur;
    std::istringstream iss(line);
    while (std::getline(iss, cur, '|')) parts.push_back(cur);
    if (parts.size() >= 4) {
        s.fio = parts[0];
        s.group = parts[1];
        s.stipend = std::stod(parts[2]);
        std::istringstream gss(parts[3]);
        std::string g;
        while (std::getline(gss, g, ',')) {
            if (!g.empty()) s.grades.push_back(std::stoi(g));
        }
    }
    return s;
}

// framing: 4 bytes network-order length, then payload
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

// Business logic
std::string handleCommand(const std::string& cmd) {
    // Команды:
    // GET_ALL
    // GET_NO_3
    // ADD\n<student_serialized>
    // EDIT\n<index>\n<student_serialized>
    // DELETE\n<index>
    // Индексация с 0 на сервере (клиент может использовать 1 если захотим, но в примере 0)
    std::istringstream iss(cmd);
    std::string line;
    std::getline(iss, line);
    if (line == "GET_ALL") {
        std::ostringstream oss;
        std::lock_guard<std::mutex> lk(db_mutex);
        oss << students.size() << "\n";
        for (size_t i = 0;i < students.size();++i) {
            oss << i << "|" << serializeStudent(students[i]) << "\n";
        }
        return oss.str();
    }
    else if (line == "GET_NO_3") {
        std::ostringstream oss;
        std::lock_guard<std::mutex> lk(db_mutex);
        std::vector<std::pair<int, Student>> res;
        for (size_t i = 0;i < students.size();++i) {
            bool has3 = false;
            for (int g : students[i].grades) if (g == 3) { has3 = true; break; }
            if (!has3) res.push_back({ (int)i, students[i] });
        }
        oss << res.size() << "\n";
        for (auto& p : res) {
            oss << p.first << "|" << serializeStudent(p.second) << "\n";
        }
        return oss.str();
    }
    else if (line == "ADD") {
        std::string payload;
        std::getline(iss, payload);
        Student s = deserializeStudent(payload);
        {
            std::lock_guard<std::mutex> lk(db_mutex);
            students.push_back(s);
        }
        return std::string("OK");
    }
    else if (line == "EDIT") {
        std::string idxs;
        std::getline(iss, idxs);
        std::string payload;
        std::getline(iss, payload);
        int idx = std::stoi(idxs);
        Student s = deserializeStudent(payload);
        {
            std::lock_guard<std::mutex> lk(db_mutex);
            if (idx < 0 || idx >= (int)students.size()) return std::string("ERROR: index");
            students[idx] = s;
        }
        return std::string("OK");
    }
    else if (line == "DELETE") {
        std::string idxs;
        std::getline(iss, idxs);
        int idx = std::stoi(idxs);
        {
            std::lock_guard<std::mutex> lk(db_mutex);
            if (idx < 0 || idx >= (int)students.size()) return std::string("ERROR: index");
            students.erase(students.begin() + idx);
        }
        return std::string("OK");
    }
    else if (line == "SHUTDOWN") {
        running = false;
        return std::string("OK");
    }
    return std::string("ERROR: unknown");
}

void clientHandler(SOCKET clientSock, std::string clientAddr) {
    std::cout << "Client connected: " << clientAddr << "\n";
    std::string req;
    while (running) {
        if (!recvMessage(clientSock, req)) break;
        // обработать
        std::string resp = handleCommand(req);
        if (!sendMessage(clientSock, resp)) break;
    }
    std::cout << "Client disconnected: " << clientAddr << "\n";
    closesocket(clientSock);
}

int main() {
#ifdef _WIN32
    WSADATA wsa;
    WSAStartup(MAKEWORD(2, 2), &wsa);
#endif

    // Инициализируем БД примером >=5 записей
    {
        std::lock_guard<std::mutex> lk(db_mutex);
        students = {
            {"Иванов Иван Иванович", "ПИ-21", 1200.0, {5,4,4,5}},
            {"Петров Петр Петрович", "ПИ-21", 1000.0, {4,4,4,5}},
            {"Сидорова Мария Сергеевна", "ПИ-20", 900.0, {5,5,5,5}},
            {"Козлов Андрей Анатольевич", "ПИ-22", 1100.0, {4,3,5,4}},
            {"Михайлова Ольга Ивановна", "ПИ-20", 950.0, {5,4,4,5}},
        };
    }

    SOCKET listenSock = socket(AF_INET, SOCK_STREAM, 0);
    if (listenSock == INVALID_SOCKET) {
        std::cerr << "socket error\n"; return 1;
    }

    int port = 54000;
    sockaddr_in srv{};
    srv.sin_family = AF_INET;
    srv.sin_addr.s_addr = INADDR_ANY;
    srv.sin_port = htons(port);

    int opt = 1;
    setsockopt(listenSock, SOL_SOCKET, SO_REUSEADDR,
        reinterpret_cast<char*>(&opt), sizeof(opt));

    if (bind(listenSock, (sockaddr*)&srv, sizeof(srv)) < 0) {
        std::cerr << "bind failed\n"; return 1;
    }

    if (listen(listenSock, SOMAXCONN) < 0) {
        std::cerr << "listen failed\n"; return 1;
    }

    std::cout << "Server listening on port " << port << "\n";

    std::vector<std::thread> threads;
    while (running) {
        sockaddr_in client{};
        socklen_t clientSize = sizeof(client);
        SOCKET clientSock = accept(listenSock, (sockaddr*)&client, &clientSize);
        if (clientSock == INVALID_SOCKET) {
            if (!running) break;
            std::cerr << "accept failed\n"; continue;
        }
        char ip[64];
        inet_ntop(AF_INET, &client.sin_addr, ip, sizeof(ip));
        std::string addr = std::string(ip) + ":" + std::to_string(ntohs(client.sin_port));
        threads.emplace_back(std::thread(clientHandler, clientSock, addr));
        // detach or keep joinable — мы храним и в конце join'им
    }

    // закрыть listening socket
    closesocket(listenSock);
    std::cout << "Server shutting down, waiting for threads...\n";
    for (auto& t : threads) if (t.joinable()) t.join();

#ifdef _WIN32
    WSACleanup();
#endif
    return 0;
}
