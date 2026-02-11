#pragma once

#define ID_MAXSIZE		5					// максимальная количество имен в идентификаторе 
#define TI_MAXSIZE		4096				// максимальная количество строк в таблице идентификаторов
#define TI_INT_DEFAULT  0x00000000			// значение по умолчанию для типа integer
#define TI_STR_DEFAULT  0x00				// значение по умолчанию для типа string
#define TI_NULLIDX		0xffffffff			// нет элемента таблицы идентификаторов
#define TI_STR_MAXSIZE  255					// максимальная длина строки

#define PARM_ID L".id.txt"
namespace IT								// таблица идентификаторов 
{
	enum IDDATATYPE { DEF = 0, INT = 1, STR = 2 };				// типы данных идентификаторов: integer, string
	enum IDTYPE { D = 0, V = 1, F = 2, P = 3, L = 4 };			// типы идентификаторов: переменная, функция, параметр, литерал

	struct Entry							// структура таблицы идентификаторов
	{
		char parrent_function[ID_MAXSIZE + 5];
		int firstApi;
		int idOfFirstLetter;				// индекс первой строки в таблице ячейка
		char id[ID_MAXSIZE + 5];			// идентификатор (возможно ограничен до ID_MAXSIZE)
		IDDATATYPE iddatatype;				// тип данных
		IDTYPE idtype;						// тип идентификатора
		union
		{
			int vint;						// значение integer
			char operation = '\0';
			struct
			{
				unsigned char len;			// количество символов в string
				char str[TI_STR_MAXSIZE];	// строки string
			} vstr;							// значение string
		} value;							// значение идентификатора
		int parmQuantity;
		Entry();
		Entry(const char* parrentFunc, const char* id, IDDATATYPE iddatatype, IDTYPE idtype, int first);
		Entry(const char* parrentFunc, const char* id, IDDATATYPE iddatatype, IDTYPE idtype, int first, int it);
		Entry(const char* parrentFunc, const char* id, IDDATATYPE iddatatype, IDTYPE idtype, int first, char* str);
		Entry(const char* parrentFunc, const char* id, IDDATATYPE iddatatype, IDTYPE idtype, int first, const char* str);
		Entry(char* parrentFunc, char* id, IDDATATYPE iddatatype, IDTYPE idtype);
	};

	struct IdTable				// класс таблицы идентификаторов
	{
		int noname_lexema_count;
		int maxsize;						// максимальный размер таблицы идентификаторов < TI_MAXSIZE
		int size;							// текущий размер таблицы идентификаторов < maxsize
		Entry* table;						// массив строк таблицы идентификаторов
		Entry GetEntry(						// получить строку таблицы идентификаторов
			int n							// индекс получаемой строки
		);
		int IsId(				// проверить: индекс строки (если есть), TI_NULLIDX(если нет)
			const char id[ID_MAXSIZE]		// идентификатор
		);
		int IsId(const char id[ID_MAXSIZE],
			const char parrent_function[ID_MAXSIZE + 5]);

		void Add(				// добавить строку в таблицу идентификаторов 
			Entry entry						// строка таблицы идентификаторов
		);
		void PrintIdTable(const wchar_t* in);
		IdTable();
		char* GetLexemaName();
	};


	void Delete(IdTable& idtable);			// удалить таблицу ячеек (освободить память)
};