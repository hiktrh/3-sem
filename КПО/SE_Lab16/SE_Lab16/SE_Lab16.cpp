#include "FST.h"
#include <tchar.h>
#include <iostream>
#include <string>
using namespace std;

int _tmain(int argc, _TCHAR* argv[]) {
    setlocale(LC_ALL, "rus");
    FST::NODE fstNodes[] = {
        FST::NODE(1, FST::RELATION('a', 1)),
        FST::NODE(2, FST::RELATION('b', 2), FST::RELATION('s', 2)),
        FST::NODE(2, FST::RELATION('b', 2), FST::RELATION('c', 3)),
        FST::NODE(2, FST::RELATION('b', 4), FST::RELATION('s', 4)),
        FST::NODE(3, FST::RELATION('b', 4), FST::RELATION('k', 5), FST::RELATION('l', 5)),
        FST::NODE(4, FST::RELATION('k', 5), FST::RELATION('l', 5), FST::RELATION('b', 6), FST::RELATION('s', 6)),
        FST::NODE(2, FST::RELATION('b', 6), FST::RELATION('j', 7)),
        FST::NODE()  
    };

    const char* inputStrings[] = {
        "abcbkbj",
        "abbcbblbj",
        "ascbbkklbj",
        "ascbblsj",
        "ascslsj",
        "ascslbbbj",
        "ascskbj",
        "abcbkb",
        "abocbkbj"
    };

    // Выполнение каждого автомата
    for (int i = 0; i < sizeof(inputStrings) / sizeof(inputStrings[0]); ++i) {
        // Создание конечного автомата с первым узлом и остальными узлами
        FST::FST fst(inputStrings[i], 8, fstNodes[0], fstNodes[1], fstNodes[2], fstNodes[3], fstNodes[4], fstNodes[5], fstNodes[6], fstNodes[7]);

        if (FST::execute(fst))
            cout << "Цепочка " << fst.strin << " распознана" << endl;
        else
            cout << "Цепочка " << fst.strin << " не распознана" << endl;
    }
}