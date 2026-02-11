#include <iostream>
#include <winsock2.h>
#include <ws2tcpip.h>

#pragma comment(lib, "ws2_32.lib")

int main() {
    setlocale(LC_CTYPE, "rus");
    WSADATA wsaData;
    WSAStartup(MAKEWORD(2, 2), &wsaData);

    SOCKET clientSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

    sockaddr_in serverAddr{};
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(12345);
    inet_pton(AF_INET, "127.0.0.1", &serverAddr.sin_addr);

    connect(clientSocket, (SOCKADDR*)&serverAddr, sizeof(serverAddr));

    int m, n;
    std::cout << "Введите два числа (m и n): ";
    std::cin >> m >> n;

    char buffer[64];
    sprintf_s(buffer, "%d %d", m, n);
    send(clientSocket, buffer, strlen(buffer), 0);

    char response[64] = { 0 };
    recv(clientSocket, response, sizeof(response), 0);
    std::cout << "Ответ от сервера: " << response << "\n";

    closesocket(clientSocket);
    WSACleanup();
    return 0;
}
