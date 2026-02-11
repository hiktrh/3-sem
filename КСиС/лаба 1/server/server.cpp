#include <iostream>
#include <winsock2.h>
#include <ws2tcpip.h>

#pragma comment(lib, "ws2_32.lib")

long long factorial(int x) {
    long long result = 1;
    for (int i = 2; i <= x; ++i) result *= i;
    return result;
}

int main() {
    setlocale(LC_CTYPE, "rus");
    WSADATA wsaData;
    WSAStartup(MAKEWORD(2, 2), &wsaData);

    SOCKET serverSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
        
    sockaddr_in serverAddr{};
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(12345);
    serverAddr.sin_addr.s_addr = INADDR_ANY;

    bind(serverSocket, (SOCKADDR*)&serverAddr, sizeof(serverAddr));
    listen(serverSocket, SOMAXCONN);

    std::cout << "Сервер запущен. Ожидание клиента...\n";

    SOCKET clientSocket = accept(serverSocket, nullptr, nullptr);

    char buffer[64] = { 0 };
    recv(clientSocket, buffer, sizeof(buffer), 0);

    int m, n;
    sscanf_s(buffer, "%d %d", &m, &n);
    long long result = factorial(m) + factorial(n);

    char response[64];
    sprintf_s(response, "%lld", result);
    send(clientSocket, response, strlen(response), 0);

    closesocket(clientSocket);
    closesocket(serverSocket);
    WSACleanup();
    return 0;
}
