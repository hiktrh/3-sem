#include <iostream>
#pragma comment(lib, "..\\Debug\\SE_asm01a.lib")

extern "C"
{
	int __stdcall getmax(int*, int);
	int __stdcall getmin(int*, int);
}

int main()
{
	int array[10] = { -25, 5, 23, -10, 25, 9, -10, 22, 9, 0 };
	int max = getmax(array, sizeof(array) / sizeof(int));
	int min = getmin(array, sizeof(array) / sizeof(int));
	std::cout << "getmax + getmin = " << max + min << std::endl;
}
