#define WIN32_LEAN_AND_MEAN
#include <iostream>
#include <string>      
#include <winsock2.h>
#include <ws2tcpip.h>   
#pragma comment(lib, "ws2_32.lib")

constexpr uint16_t PORT = 8080;
constexpr int BUFFER_SIZE = 1024;

int main() {
    setlocale(LC_CTYPE, "rus");
    WSADATA wsaData;
    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
        std::cerr << "Ошибка WSAStartup\n";
        return 1;
    }

    SOCKET clientSocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
    if (clientSocket == INVALID_SOCKET) {
        std::cerr << "Ошибка создания сокета\n";
        WSACleanup();
        return 1;
    }

    sockaddr_in serverAddr{};
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(PORT);
    if (inet_pton(AF_INET, "127.0.0.1", &serverAddr.sin_addr) <= 0) {
        std::cerr << "Неверный IP-адрес\n";
        closesocket(clientSocket);
        WSACleanup();
        return 1;
    }

    std::cout << "Введите строку: ";
    std::string input;
    std::getline(std::cin, input);   

    int sendResult = sendto(clientSocket,
        input.c_str(),
        static_cast<int>(input.size()),
        0,
        reinterpret_cast<sockaddr*>(&serverAddr),
        sizeof(serverAddr));
    if (sendResult == SOCKET_ERROR) {
        std::cerr << "Ошибка отправки\n";
        closesocket(clientSocket);
        WSACleanup();
        return 1;
    }

    char buffer[BUFFER_SIZE]{};
    sockaddr_in fromAddr{};
    int fromLen = sizeof(fromAddr);

    int recvLen = recvfrom(clientSocket,
        buffer,
        BUFFER_SIZE - 1, // оставим место для терминирующего нуля
        0,
        reinterpret_cast<sockaddr*>(&fromAddr),
        &fromLen);
    if (recvLen == SOCKET_ERROR) {
        std::cerr << "Ошибка получения ответа\n";
    }
    else {
        buffer[recvLen] = '\0';
        std::cout << "Ответ от сервера:\n" << buffer << std::endl;
    }

    closesocket(clientSocket);
    WSACleanup();
    return 0;
}
