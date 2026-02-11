#pragma once
#define IN_MAX_LEN_TEXT 1024*1024 //макс размер исх кода
#define IN_CODE_ENDL '\n'
// символ конца строки
// таблица проверки входной информации, индекс = код (Windows-1251) символа
// значения IN::F -заперщенный символ, IN::T - разрешенный символ, IN::I - игнорировать(не вводить),
// если 0 <= значение < 256 - то вводится данное значение

namespace In
{
    struct IN  // исходные данные
    {
        enum { T = 1024, F = 2048, I = 4096 }; // T - допустимый символ, F - недопустимый, I - игнорировать
        int size;                     // размер исходного кода
        int lines;                    // количество строк
        int ignor;                    // количество проигнорированных символов
        unsigned char* text;          // исходный код (Windows - 1251)
        /* int code[256] = { F, F, F, F, F, F, F, F, F,   F,   T,   F,   F,   I,   F,   F, \
             F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
             T,   T,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
             F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
             F,   F,   F,   F,   F,   F,   F,   F,   T,   F,   F,   F,   F,   F,   F,   F, \
             F,   F,   F,   F,   F,   F,   F,   T,   I,  '!',   F,   F,   F,   F,   F,   F, \
             F,   F,   F,   F,   T,   T,   F,   F,   F,   F,   F,   F,   T,   F,   F,   T, \
             F,   F,   T,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
             \
             F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
             F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
             F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
             F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
             F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   T,   F,   F,   T, \
             F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
             F,   F,   T,   F,   F,   T,   F,   F,   T,   T,   F,   F,   F,   F,   F,   F, \
             T,   F,   T,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F }; */
        int code[256] = { F, F, F, F, F, F, F, F, F,   F,  '|',   F,   F,   I,   F,   F, \
            F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
            T,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
            T,   F,   T,   F,   F,   T,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \

            F,   F,   F,   F,   T,   F,   F,   F,   F,   F,   F,   F,   T,   F,    F,   F, \
            F,   F,   F,   F,   F,   F,   F,   F,   I,   F,   F,   F,   F,   F,   F,   F, \
            F,   T,   F,   T,   F,   F,   F,   F,   T,   T,   F,   T,   F,   F,   T,   F, \
            F,   F,   T,   F,   T,   T,   T,   F,   F,   T,   F,   F,   F,   F,   F,   F, \
            
            F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
            F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
            F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
            F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \

           '-',  F,   F,  F,   T,   F,   F,   F,   F,   F,   F,   T,   F,   F,   F,   F, \
            F,   F,   F,  F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F,   F, \
            T,   F,   T,  F,   F,   F,   F,   F,   T,   F,   T,   F,   F,   T,   F,   F, \
            T,   F,   T,  T,   F,   F,   F,   T,   F,   F,   F,   F,   T,   F,   F,   T }; // таблица проверки: T, F, I новое значение
    };
    IN getin(wchar_t infile[]);   // ввести и проверить входной поток
};