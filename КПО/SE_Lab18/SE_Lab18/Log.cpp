#include "stdafx.h"
#include "Log.h"
#pragma warning(disable:4996)

namespace Log
{
    // Функция для получения лог-файла
    LOG getlog(wchar_t logfile[]) {
        LOG log;
        log.stream = new ofstream; // Создаем поток для записи в файл
        log.stream->open(logfile); // Открываем файл
        if (!log.stream->is_open()) {
            throw ERROR_THROW(112); // Ошибка, если файл не открыт
        }
        wcscpy_s(log.logfile, logfile); // Копируем имя файла в лог
        return log; // Возвращаем лог
    }

    // Функция для записи строки в лог
    void WriteLine(LOG log, const char* c, ...)
    {
        const char** ptr = &c;
        int i = 0;
        while (ptr[i] != "") { // Пока не достигли конца массива
            *log.stream << ptr[i++]; // Записываем в лог
        }
    }

    // Перегрузка функции для записи строки в лог в формате wchar_t
    void WriteLine(LOG log, const wchar_t* c, ...)
    {
        const wchar_t** ptr = &c;
        char temp[50];
        int i = 0;
        while (ptr[i] != L"") { // Пока не достигли конца массива
            wcstombs(temp, ptr[i++], sizeof(temp)); // Конвертация wide char в char
            *log.stream << temp; // Записываем в лог
        }
    }

    // Функция для записи метаданных лога
    void WriteLog(LOG log)
    {
        char date[50];
        tm local;
        time_t currentTime;
        currentTime = time(NULL); // Получаем текущее время
        localtime_s(&local, &currentTime); // Преобразуем в локальное время
        strftime(date, 100, "%d.%m.%Y %H:%M:%S ----", &local); // Форматируем строку даты
        *log.stream << " ----	Протокол	---- " << date << endl; // Записываем заголовок лога
    }

    // Функция для записи параметров
    void WriteParm(LOG log, Parm::PARM parm)
    {
        char buff[PARM_MAX_SIZE];
        size_t tsize = 0;

        *log.stream << " ----	Параметры	---- " << endl; // Заголовок параметров
        wcstombs_s(&tsize, buff, parm.log, PARM_MAX_SIZE); // Конвертация параметра лог
        *log.stream << "-log: " << buff << endl; // Запись лога
        wcstombs_s(&tsize, buff, parm.out, PARM_MAX_SIZE); // Конвертация параметра out
        *log.stream << "-out: " << buff << endl; // Запись output
        wcstombs_s(&tsize, buff, parm.in, PARM_MAX_SIZE); // Конвертация параметра in
        *log.stream << "-in : " << buff << endl; // Запись input
    }

    // Функция для записи информации о входных данных
    void WriteIn(LOG log, In::IN in) {
        *log.stream << " ----	Входные данные	---- " << endl; // Заголовок входных данных
        *log.stream << "Количество семейств : " << in.size << endl; // Запись количества семейств
        *log.stream << "Количество строк    : " << in.lines << endl; // Запись количества строк
        *log.stream << "Пропущенные        : " << in.ignor << endl; // Запись пропущенных
    }

    // Функция для записи ошибок
    void WriteError(LOG log, Error::ERROR error)
    {
        if (log.stream)
        {
            *log.stream << " ----	Ошибка	---- " << endl; // Заголовок ошибки
            *log.stream << "Ошибка ";
            *log.stream << error.id; // Запись ID ошибки
            *log.stream << ": ";
            *log.stream << error.message << endl; // Запись сообщения об ошибке
            if (error.inext.line != -1)
            {
                *log.stream << "Строка: " << error.inext.line << endl << "Столбец: " << error.inext.col << endl << endl; // Запись информации о строке и столбце
            }
        }
        else
            cout << "Ошибка " << error.id << ": " << error.message << ", строка " << error.inext.line << ", столбец " << error.inext.col << endl << endl; // Запись в стандартный вывод
    }

    // Функция для закрытия лог-файла
    void Close(LOG log) {
        log.stream->close(); // Закрываем поток
        delete log.stream; // Освобождаем память
    }
}