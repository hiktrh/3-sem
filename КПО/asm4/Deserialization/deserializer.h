#pragma once
#include <iostream>
#include <fstream>
#include "generateAsm.h"
using namespace std;

class Deserializer {

public:

	void DeserializeData();
	void ConvertToAssembler();

	int intVal;
	unsigned int uintLit;

	Deserializer() {

		this->intVal = 0;
		this->uintLit = 0;
	}


private:

	enum DataType : uint8_t {
		TYPE_INT = 0x01,
		TYPE_UINT = 0x02
	};

};
