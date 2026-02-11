#include <iostream>
#include <string>
#include <unordered_map>
#include <queue>
#include <iomanip>

using namespace std;

struct Node {
    char ch;
    int freq;
    Node* left;
    Node* right;
};

struct Compare {
    bool operator()(Node* a, Node* b) {
        return a->freq > b->freq;
    }
};

void generateCodes(Node* root, string code, unordered_map<char, string>& huffmanCode) {
    if (!root) return;

    if (!root->left && !root->right) {
        huffmanCode[root->ch] = code;
    }

    generateCodes(root->left, code + "0", huffmanCode);
    generateCodes(root->right, code + "1", huffmanCode);
}

int main() {
    system("chcp 1251");
    system("pause");
    setlocale(LC_ALL, "RU");
    cout << "Введите текст для кодирования (например, ФИО): ";
    string text;
    getline(cin, text);

    unordered_map<char, int> freq;
    for (int i = 0; i < text.length(); ++i) {
        freq[text[i]]++;
    }

   
    priority_queue<Node*, deque<Node*>, Compare> pq;

    for (auto it = freq.begin(); it != freq.end(); ++it) {
        Node* node = new Node{ it->first, it->second, nullptr, nullptr };
        pq.push(node);
    }

    while (pq.size() > 1) {
        Node* left = pq.top(); pq.pop();
        Node* right = pq.top(); pq.pop();

        Node* parent = new Node{ '\0', left->freq + right->freq, left, right };
        pq.push(parent);
    }

    Node* root = pq.top();

    
    unordered_map<char, string> huffmanCode;
    generateCodes(root, "", huffmanCode);


    cout << "\nA) Частота символов:\n";
    for (auto it = freq.begin(); it != freq.end(); ++it) {
        double percent = (double)it->second / text.length() * 100;
        cout << "'" << it->first << "' : " << it->second << " раз (" << fixed << setprecision(2) << percent << "%)\n";
    }


    cout << "\nБ) Таблица кодов Хаффмана:\n";
    for (auto it = huffmanCode.begin(); it != huffmanCode.end(); ++it) {
        cout << "'" << it->first << "' : " << it->second << "\n";
    }



    cout << "\nВ) Закодированная строка:\n";
    string encoded;
    for (int i = 0; i < text.length(); ++i) {
        encoded += huffmanCode[text[i]];
    }
    cout << encoded << "\n";

    cout << "\nДлина исходной строки: " << text.length() * 8 << " бит\n";
    cout << "Длина закодированной строки: " << encoded.length() << " бит\n";

    return 0;
}