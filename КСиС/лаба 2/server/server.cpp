#include <iostream>
#include <winsock2.h>
#include <windows.h>
#include <unordered_map>
#include <string>

#pragma comment(lib, "ws2_32.lib")
#define PORT 8080
#define BUFFER_SIZE 1024

int main() {
    setlocale(LC_CTYPE, "rus");
    WSADATA wsaData;
    SOCKET serverSocket;
    sockaddr_in serverAddr, clientAddr;
    char buffer[BUFFER_SIZE];
    int clientAddrSize = sizeof(clientAddr);

    
    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
        std::cerr << "Ошибка WSAStartup\n";
        return 1;
    }

    serverSocket = socket(AF_INET, SOCK_DGRAM, 0);
    if (serverSocket == INVALID_SOCKET) {
        std::cerr << "Ошибка создания сокета\n";
        WSACleanup();
        return 1;
    }

    
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(PORT);
    serverAddr.sin_addr.s_addr = INADDR_ANY;

   
    if (bind(serverSocket, (SOCKADDR*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR) {
        std::cerr << "Ошибка привязки\n";
        closesocket(serverSocket);
        WSACleanup();
        return 1;
    }

    std::cout << "Сервер запущен. Ожидание сообщений...\n";

    while (true) {
        memset(buffer, 0, BUFFER_SIZE);
        int bytesReceived = recvfrom(serverSocket, buffer, BUFFER_SIZE, 0, (SOCKADDR*)&clientAddr, &clientAddrSize);
        if (bytesReceived == SOCKET_ERROR) {
            std::cerr << "Ошибка получения данных\n";
            continue;
        }

        std::string input(buffer);
        std::string target = "WINDOWS";
        std::unordered_map<char, int> counts;

        for (char c : input) {
            c = toupper(c);
            if (target.find(c) != std::string::npos) {
                counts[c]++;
            }
        }

        std::string response;
        for (char c : target) {
            response += c;
            response += ": ";
            response += std::to_string(counts[c]);
            response += "\n";
        }

        sendto(serverSocket, response.c_str(), response.size(), 0, (SOCKADDR*)&clientAddr, clientAddrSize);
    }

    closesocket(serverSocket);
    WSACleanup();
    return 0;
}
