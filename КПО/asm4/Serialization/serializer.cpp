#include "serializer.h"

void Serializer::Serialize(int intVal, ofstream& ofile)
{
	uint8_t dataType = TYPE_INT;
	uint32_t dataLength = sizeof(intVal);
	
	ofile.write(reinterpret_cast<const char*> (&dataType), sizeof(dataType));
	ofile.write(reinterpret_cast<const char*> (&dataLength), sizeof(dataLength));
	ofile.write(reinterpret_cast<const char*> (&intVal), sizeof(intVal));
}

void Serializer::Serialize(unsigned int uintLit, ofstream& ofile)
{

	uint8_t dataType = TYPE_UINT;
	uint32_t dataLength = sizeof(uintLit);

	ofile.write(reinterpret_cast<const char*>(&dataType), sizeof(dataType));
	ofile.write(reinterpret_cast<const char*> (&dataLength), sizeof(dataLength));
	ofile.write(reinterpret_cast<const char*> (&uintLit), dataLength);
}
