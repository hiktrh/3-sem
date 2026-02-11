#include <iostream>
using namespace std;
void hanoi(int n, int from, int to, int aux)
{
	if (n == 1)
	{
		cout << "Переместить диск 1 с " << from << " на " << to << " стержень\n";
		return;
	}
	hanoi(n - 1, from,to, aux);
	cout << "Переместить диск " << n << " c " << from << " на " << to << " стержень\n";
	hanoi(n - 1,aux, to,from);
}
int main()
{
	setlocale(LC_CTYPE, "rus");
	int N, k;
	cout << "Введите количество дисков (N): ";
	cin >> N;
	cout << "Введите номер конечного стержня (k): ";
	cin >> k;
	int i = 1;
	int aux = 6 - i - k;
	hanoi(N, i, k, aux);
	return 0;
}