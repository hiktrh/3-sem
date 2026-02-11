#pragma once
#include "IT.h"
#define LEXEMA_FIXSIZE  1			// фиксированный размер лексемы
#define	LT_MAXSIZE		4096		// максимальная количество строк в таблице лексем	
#define	LT_TI_NULLIDX	-1			// нет элемента таблицы идентификаторов				
#define	LEX_INTEGER		't'			
#define	LEX_STRING		't'			
#define	LEX_ID			'i'			
#define	LEX_LITERAL		'l'			
#define	LEX_FUNCTION	'f'			
#define	LEX_DECLARE		'd'			
#define	LEX_RETURN		'r'			
#define	LEX_PRINT		'p'			
#define	LEX_STRLEN		'e'			
#define	LEX_SEMICOLON	';'			
#define	LEX_COMMA		','			
#define	LEX_LEFTBRACE	'{'			
#define	LEX_RIGHTBRACE	'}'			
#define	LEX_LEFTHESIS	'('			
#define	LEX_RIGHTHESIS	')'			
#define	LEX_PLUS		'v'			
#define	LEX_MINUS		'v'			
#define	LEX_STAR		'v'			
#define	LEX_DIRSLASH	'v'			
#define	LEX_EQUAL_SIGN	'='			

#define PLUS '+'
#define MINUS '-'
#define STAR '*'
#define DIRSLASH '/'


namespace LT									// таблица лексем
{
	struct Entry								// структура таблицы лексем
	{
		char lexema;							// лексема
		int numberOfString;						// индекс строки в исходном тексте
		int idInTI;								// индекс в таблице идентификаторов или LT_TI_NULLIDX
		Entry();
		Entry(const char lex, int stringNumber, int idInTI);
	};

	struct LexTable								// класс таблицы лексем
	{
		int maxsize;							// максимальный размер таблицы лексем < LT_MAXSIZE
		int size;								// текущий размер таблицы лексем < maxsize
		Entry* table;							// массив строк таблицы лексем


		Entry GetEntry(								// получить строку таблицы лексем
			int n									// индекс получаемой строки
		);

		void PrintLexTable(const wchar_t* in);
		LexTable();
	};

	void Add(									// добавить строку в таблицу лексем
		LexTable& lextable,						// класс таблицы лексем
		Entry entry								// строка таблицы лексем
	);

	void Delete(LexTable& lextable);			// удалить таблицу лексем (освободить память)
};