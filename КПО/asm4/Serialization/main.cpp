#include "serializer.h"

int main() {

	setlocale(LC_ALL, "rus");
	int intVar = 10;
	unsigned int uintLiteral = 13;
	Serializer Serializer;

	ofstream output;

	output.open("serialization.bin", ios::binary);

	if (output.is_open()) {

		Serializer.Serialize(intVar, output);
		Serializer.Serialize(uintLiteral, output);
		cout << "Сериализация произшла успешно!";
	}

	else {

		cout << "Не удалось открывать файл для записи";
	}

	output.close();

	return 0;
}