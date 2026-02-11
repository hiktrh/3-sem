#include "stdafx.h"

namespace IT
{
    // Конструктор для класса IdTable
    IdTable::IdTable()
    {
        noname_lexema_count = 0; // Счетчик неименованных лексем
        maxsize = TI_MAXSIZE; // Максимальный размер таблицы
        size = 0; // Текущий размер таблицы
        table = new Entry[TI_MAXSIZE]; // Создание массива записей
    }

    // Конструктор по умолчанию для класса Entry
    Entry::Entry()
    {
        parrent_function[0] = '\0'; // Инициализация родительской функции
        id[0] = '\0'; // Инициализация идентификатора
        firstApi = 0; // Первое вхождение
        iddatatype = IT::IDDATATYPE::DEF; // Тип данных
        idtype = IT::IDTYPE::D; // Тип идентификатора
        parmQuantity = 0; // Количество параметров
    }

    // Конструктор класса Entry с параметрами
    Entry::Entry(const char* parrent_function, const char* id, IDDATATYPE iddatatype, IDTYPE idtype, int first)
    {
        int i = 0;
        if (parrent_function)
            for (i = 0; parrent_function[i] != '\0'; i++)
                this->parrent_function[i] = parrent_function[i];
        this->parrent_function[i] = '\0'; // Завершение строки
        i = 0;
        if (id)
            for (i = 0; id[i] != '\0'; i++)
                this->id[i] = id[i];

        firstApi = first; // Сохранение первого вхождения
        this->id[i] = '\0'; // Завершение строки
        this->iddatatype = iddatatype; // Сохранение типа данных
        this->idtype = idtype; // Сохранение типа идентификатора
        parmQuantity = 0; // Инициализация количества параметров
    }

    // Другие конструкторы класса Entry (по аналогии с предыдущим)
    Entry::Entry(const char* parrent_function, const char* id, IDDATATYPE iddatatype, IDTYPE idtype, int first, int it)
    {
        int i = 0;
        if (parrent_function)
            for (i; parrent_function[i] != '\0'; i++)
                this->parrent_function[i] = parrent_function[i];
        this->parrent_function[i] = '\0';
        if (id)
            for (i = 0; id[i] != '\0'; i++)
                this->id[i] = id[i];

        firstApi = first;
        this->id[i] = '\0';
        this->iddatatype = iddatatype;
        this->idtype = idtype;
        parmQuantity = 0;
        value.vint = it; // Сохранение целочисленного значения
    }

    Entry::Entry(const char* parrent_function, const char* id, IDDATATYPE iddatatype, IDTYPE idtype, int first, char* ch)
    {
        int i = 0;
        if (parrent_function)
            for (i; parrent_function[i] != '\0'; i++)
                this->parrent_function[i] = parrent_function[i];
        this->parrent_function[i] = '\0';
        if (id)
            for (i = 0; id[i] != '\0'; i++)
                this->id[i] = id[i];

        firstApi = first;
        this->id[i] = '\0';
        this->iddatatype = iddatatype;
        this->idtype = idtype;
        parmQuantity = 0;
        strcpy_s(value.vstr.str, 255, ch); // Копирование строки
        value.vstr.len = strlen(ch); // Длина строки
    }

    Entry::Entry(const char* parrent_function, const char* id, IDDATATYPE iddatatype, IDTYPE idtype, int first, const char* ch)
    {
        int i = 0;
        if (parrent_function)
            for (i; parrent_function[i] != '\0'; i++)
                this->parrent_function[i] = parrent_function[i];
        this->parrent_function[i] = '\0';
        if (id)
            for (i = 0; id[i] != '\0'; i++)
                this->id[i] = id[i];

        firstApi = first;
        this->id[i] = '\0';
        this->iddatatype = iddatatype;
        this->idtype = idtype;
        parmQuantity = 0;
        strcpy_s(value.vstr.str, 255, ch); // Копирование строки
        value.vstr.len = strlen(ch); // Длина строки
    }

    Entry::Entry(char* parrent_function, char* id, IDDATATYPE iddatatype, IDTYPE idtype)
    {
        int i = 0;
        if (parrent_function)
            for (i; parrent_function[i] != '\0'; i++)
                this->parrent_function[i] = parrent_function[i];
        this->parrent_function[i] = '\0';
        if (id)
            for (i = 0; id[i] != '\0'; i++)
                this->id[i] = id[i];

        this->id[i] = '\0';
        this->iddatatype = iddatatype;
        this->idtype = idtype;
        parmQuantity = 0;
    }

    // Функция для создания таблицы идентификаторов
    IdTable Create(int size)
    {
        IdTable idTable;
        idTable.size = size;
        idTable.maxsize = TI_MAXSIZE;
        idTable.table = new Entry[TI_MAXSIZE]; // Создание массива записей
        return idTable;
    }

    // Метод для добавления записи в таблицу идентификаторов
    void IdTable::Add(Entry entry)
    {
        if (strlen(entry.id) > ID_MAXSIZE && entry.idtype != IDTYPE::F)
            throw ERROR_THROW(121); // Ошибка: идентификатор слишком длинный

        if (size < maxsize)
        {
            if (entry.idtype != IDTYPE::F)
                entry.id[5] = '\0'; // Обрезка идентификатора
            table[size] = entry; // Добавление записи

            switch (entry.iddatatype)
            {
            case IDDATATYPE::INT:
                table[size].value.vint = TI_INT_DEFAULT; // Значение по умолчанию для INT
                break;
            case IDDATATYPE::STR:
                table[size].value.vstr.str[0] = TI_STR_DEFAULT; // Значение по умолчанию для STR
                table[size].value.vstr.len = 0; // Длина строки по умолчанию
                break;
            }
            size++; // Увеличение размера таблицы
        }
        else
            throw ERROR_THROW(122); // Ошибка: превышен максимальный размер
    }

    // Метод для получения записи по индексу
    Entry IdTable::GetEntry(int n)
    {
        if (n < size && n >= 0)
            return table[n]; // Возвращаем запись
    }

    // Метод для проверки существования идентификатора
    int IdTable::IsId(const char id[ID_MAXSIZE])
    {
        for (int i = 0; i < size; i++)
        {
            if (strcmp(table[i].id, id) == 0)
                return i; // Возвращаем индекс
        }
        return TI_NULLIDX; // Не найден
    }

    // Метод для проверки существования идентификатора с родительской функцией
    int IdTable::IsId(const char id[ID_MAXSIZE], const char parrent_function[ID_MAXSIZE + 5])
    {
        for (int i = 0; i < size; i++)
        {
            if ((strcmp(table[i].id, id) == 0) &&
                (strcmp(table[i].parrent_function, parrent_function) == 0))
                return i; // Возвращаем индекс
        }
        return TI_NULLIDX; // Не найден
    }

    // Функция для удаления таблицы идентификаторов
    void Delete(IdTable& idtable)
    {
        delete[] idtable.table; // Освобождение памяти
        idtable.table = nullptr; // Обнуление указателя
    }

    // Метод для получения имени лексемы без имени
    char* IdTable::GetLexemaName()
    {
        char buffer[5];
        _itoa_s(noname_lexema_count, buffer, 10); // Преобразование числа в строку
        strcat_s(buffer, 5, "_l"); // Добавление суффикса
        noname_lexema_count++; // Увеличение счетчика
        return buffer; // Возвращаем имя
    }

    // Метод для печати таблицы идентификаторов в файл
    void IdTable::PrintIdTable(const wchar_t* in)
    {
        ofstream* idStream = new ofstream;
        idStream->open(in); // Открытие файла

        if (idStream->is_open())
        {
            bool flagForFirst = false;

            // Заголовок таблицы лексем
            *(idStream) << "##########################################################################################################################\n";
            *(idStream) << "------------------ Лексемы ------------------\n";

            *(idStream) << setw(15) << "Идентификатор:" << setw(17) << "Тип данных:" << setw(15) << "Значение:" << setw(27) << "Длина строки:" << setw(20) << "Первое вхождение:\n";

            for (int i = 0; i < size; i++)
            {
                if (table[i].idtype == IT::IDTYPE::L)
                {
                    cout.width(25);
                    switch (table[i].iddatatype)
                    {
                    case 1:
                        *(idStream) << "   " << table[i].id << "\t\t\t" << "INT " << "\t\t" << table[i].value.vint << "\t\t\t" << "-\t\t" << table[i].firstApi << endl;
                        break;
                    case 2:
                        *(idStream) << "   " << table[i].id << "\t\t\t" << "STR " << "\t    " << table[i].value.vstr.str << setw(30 - strlen(table[i].value.vstr.str)) << (int)table[i].value.vstr.len << "\t\t" << table[i].firstApi << endl;
                        break;
                    }
                }
            }
            *(idStream) << "\n\n\n";

            // Заголовок таблицы функций
            *(idStream) << "##########################################################################################################################\n";
            *(idStream) << "------------------ Функции ------------------\n";

            *(idStream) << setw(15) << "Идентификатор:" << setw(26) << "Тип данных входящих:" << setw(36) << "Количество передаваемых параметров:" << setw(22) << "Первое вхождение:\n";

            for (int i = 0; i < size; i++)
            {
                if (table[i].idtype == IT::IDTYPE::F)
                {
                    switch (table[i].iddatatype)
                    {
                    case 1:
                        *(idStream) << "   " << table[i].id << setw(28 - strlen(table[i].id)) << "INT " << "\t\t\t\t" << table[i].parmQuantity << "\t\t\t\t" << table[i].firstApi << endl;
                        break;
                    case 2:
                        *(idStream) << "   " << table[i].id << setw(28 - strlen(table[i].id)) << "STR " << "\t\t\t\t" << table[i].parmQuantity << "\t\t\t\t" << table[i].firstApi << endl;
                        break;
                    }
                }
            }

            *(idStream) << "\n\n\n";

            // Заголовок таблицы переменных
            *(idStream) << "##########################################################################################################################\n";
            *(idStream) << "------------------ Переменные ------------------\n";
            *(idStream) << setw(25) << "Имя родительской ячейки:" << setw(20) << "Идентификатор:" << setw(16) << "Тип данных:" << setw(24) << "Тип идентификатора:" << setw(21) << "Первое вхождение:\n";

            for (int i = 0; i < size; i++)
            {
                if (table[i].idtype == IT::IDTYPE::V)
                {
                    switch (table[i].iddatatype)
                    {
                    case 1:
                        *(idStream) << "   " << table[i].parrent_function << setw(35 - strlen(table[i].parrent_function)) << table[i].id << setw(20) << "INT " << setw(15) << "V" << "\t\t\t" << table[i].firstApi << endl;
                        break;
                    case 2:
                        *(idStream) << "   " << table[i].parrent_function << setw(35 - strlen(table[i].parrent_function)) << table[i].id << setw(20) << "STR " << setw(15) << "V" << "\t\t\t" << table[i].firstApi << endl;
                        break;
                    }

                    flagForFirst = true;
                }

                if (table[i].idtype == IT::IDTYPE::P)
                {
                    switch (table[i].iddatatype)
                    {
                    case 1:
                        *(idStream) << "   " << table[i].parrent_function << setw(35 - strlen(table[i].parrent_function)) << table[i].id << setw(20) << "INT " << setw(15) << "P" << "\t\t\t" << table[i].firstApi << endl;
                        break;
                    case 2:
                        *(idStream) << "   " << table[i].parrent_function << setw(35 - strlen(table[i].parrent_function)) << table[i].id << setw(20) << "STR " << setw(15) << "P" << "\t\t\t" << table[i].firstApi << endl;
                        break;
                    }

                    flagForFirst = true;
                }
            }
            *(idStream) << "\n\n\n";
        }
        else
            throw ERROR_THROW(125); // Ошибка: не удалось открыть файл

        idStream->close(); // Закрытие файла
        delete idStream; // Освобождение памяти
    }
}