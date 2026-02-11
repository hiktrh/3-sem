#include "stdafx.h"
#include <fstream>

namespace LT
{
    // Конструктор класса LexTable
    LT::LexTable::LexTable()
    {
        maxsize = LT_MAXSIZE; // Установка максимального размера
        size = 0; // Инициализация текущего размера
        table = new Entry[LT_MAXSIZE]; // Создание массива для хранения записей
    }

    // Функция для добавления записи в таблицу лексем
    void Add(LexTable& lextable, Entry entry)
    {
        if (lextable.size < lextable.maxsize)
            lextable.table[lextable.size++] = entry; // Добавление записи и увеличение размера
        else
            throw ERROR_THROW(120); // Ошибка при превышении максимального размера
    }

    // Функция для получения записи по индексу
    Entry LexTable::GetEntry(int n)
    {
        if (n < maxsize && n >= 0)
            return table[n]; // Возвращаем запись по индексу
    }

    // Функция для удаления таблицы лексем
    void Delete(LexTable& lextable)
    {
        delete[] lextable.table; // Освобождение памяти
        lextable.table = nullptr; // Обнуление указателя
    }

    // Функция для печати таблицы лексем в файл
    void LexTable::PrintLexTable(const wchar_t* in)
    {
        ofstream* streamToLexem = new ofstream; // Создание потока для записи
        streamToLexem->open(in); // Открытие файла
        if (streamToLexem->is_open())
        {
            (*streamToLexem) << "--------- Таблица лексем ---------"; // Заголовок таблицы
            int num_string = 0; // Номер строки
            for (int i = 0; i < size; i++)
            {
                if (num_string == table[i].numberOfString)
                    (*streamToLexem) << table[i].lexema; // Записываем лексему в ту же строку
                else
                {
                    (*streamToLexem) << '\n' << num_string << ".\t"; // Переход на новую строку
                    while (num_string != table[i].numberOfString)
                        num_string++; // Увеличиваем номер строки
                    (*streamToLexem) << table[i].lexema; // Записываем лексему
                }
            }
        }
        else
            throw ERROR_THROW(128); // Ошибка при открытии файла
        streamToLexem->close(); // Закрытие файла
        delete streamToLexem; // Освобождение памяти
    }

    // Конструктор класса Entry
    LT::Entry::Entry()
    {
        lexema = '\0'; // Инициализация лексемы
        numberOfString = LT_TI_NULLIDX; // Инициализация номера строки
        idInTI = LT_TI_NULLIDX; // Инициализация ID в таблице идентификаторов
    }

    // Конструктор класса Entry с параметрами
    LT::Entry::Entry(const char lex, int stringNumber, int idInTI)
    {
        lexema = lex; // Установка лексемы
        numberOfString = stringNumber; // Установка номера строки
        this->idInTI = idInTI; // Установка ID в таблице идентификаторов
    }
}