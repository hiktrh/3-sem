#include "deserializer.h"
using namespace std;
void Deserializer::DeserializeData()
{

	ifstream ifile = ifstream("../Serialization/serialization.bin");

	if (ifile.is_open()) {

		uint8_t dataType;
		uint32_t dataLength;

		while (true) {
			if (!ifile.read(reinterpret_cast<char*>(&dataType), sizeof(dataType)))
				break;

			if (!ifile.read(reinterpret_cast<char*>(&dataLength), sizeof(dataLength)))
				break;

			switch (dataType) {
			case TYPE_INT:
				ifile.read(reinterpret_cast<char*>(&intVal), dataLength);
				break;

			case TYPE_UINT:
				ifile.read(reinterpret_cast<char*>(&uintLit), dataLength);
				break;

			default:
				cout << "Неизвестный тип!\n";
				return;
			}
		}


		ifile.close();

		cout << "Результат десереализации: " << "\nInt переменная: " << intVal << "\t" << "\nUnsigned int литерал: " << uintLit << endl;

	}

	else {
		cout << "Не удалось открыть файл для чтения!";
	}
}

void Deserializer::ConvertToAssembler()
{

	ofstream ofile = ofstream("../asm04/asm04.asm");

	if (ofile.is_open()) {

		ofile ASM_HEAD;
		
		ofile << "INT_VAL \t SDWORD " << intVal << endl;
		ofile << "UINT_LIT \t DWORD " << uintLit << endl;

		ofile ASM_FOOTER;

		ofile.close();
	}

	else {
		cout << "Не удалось открыть файл для записи!";
	}
}

