#include <iostream>
#include <queue>

using namespace std;

const int n = 10;
bool* visited = new bool[n];

int graph[n][n] =
{
{0, 1, 0, 0, 1, 0, 0, 0, 0, 0},
{1, 0, 0, 0, 0, 0, 1, 1, 0, 0},
{0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
{0, 0, 0, 0, 0, 1, 0, 0, 1, 0},
{1, 0, 0, 0, 0, 1, 0, 0, 0, 0},
{0, 0, 0, 1, 1, 0, 0, 0, 1, 0},
{0, 1, 0, 0, 0, 0, 0, 1, 0, 0},
{0, 1, 1, 0, 0, 0, 1, 0, 0, 0},
{0, 0, 0, 1, 0, 1, 0, 0, 0, 1},
{0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
};

void DFS(int st)
{
	cout << st + 1 << " ";
	visited[st] = true;

	for (int r = 0; r <= n; r++) {
		if ((graph[st][r] != 0) && (!visited[r]))
			DFS(r);
	}
}

void BFS(int start) 
{
	queue <int> q; 
	bool visited[n];
	bool inqueue[n];
	int view_cell;

	for (int i = 0; i < n; i++)
	{
		visited[i] = inqueue[i] = false;
	}

	q.push(start);
	visited[start] = inqueue[start] = true; 

	while (!q.empty())
	{
		view_cell = q.front(); 
		cout << view_cell + 1 << " ";

		visited[view_cell] = true;
		q.pop();

		for (int i = 0; i < n; i++)
		{
			if (!inqueue[i] && graph[view_cell][i])
			{
				q.push(i);
				inqueue[i] = true;
			}
		}
	}
}

int main()
{
	setlocale(LC_ALL, "Rus");
	int start;
	cout << "Матрица смежности графа: " << endl;
	for (int i = 0; i < n; i++)
	{
		visited[i] = false;

		for (int j = 0; j < n; j++)
			cout << " " << graph[i][j];
		cout << endl;
	}

	cout << "\n-----------------------------" << endl;
	cout << "\nСписок ребер: " << endl;
	int arr_1[11] = { 1,1,2,2,3,4,4,5,6,7,9 };
	int arr_2[11] = { 2,5,7,8,8,6,9,6,9,8,10 };

	for (int i = 0; i < 11; i++)
	{
		cout << '{' << arr_1[i] << ", " << arr_2[i] << '}';
		cout << " {" << arr_2[i] << ", " << arr_1[i] << '}' << endl;
	}

	cout << "\n-----------------------------" << endl;
	cout << "\nСписок смежности: " << endl;
	int arrEdges[10][10] =
	{ {2,5},
		{7,8},
		{8},
		{6,9},
		{1,6},
		{4,5,9},
		{2,8},
		{2,3,7},
		{4,6,10},
		{9}
	};

	for (int i = 0; i < n; i++)
	{
		cout << i + 1 << "->";

		for (int j = 0; j < n; j++)
		{
			if (arrEdges[i][j] == 0)
			{
				break;
			}
			cout << arrEdges[i][j] << " ";
		}
		cout << endl;
	}
	cout << endl;

	// Поиск в глубину
	cout << "\n" << endl;
	cout << "Поиск в глубину" << endl;
	cout << "Стартовая вершина >> "; cin >> start;
	for (int i = 0; i < n; i++) visited[i] = false;
	cout << "Порядок обхода: ";
	DFS(start - 1);

	// Поиск в ширину
	cout << "\n" << endl;
	int start_2;
	cout << "\nПоиск в ширину" << endl;
	cout << "Стартовая вершина >> "; cin >> start_2;
	cout << "Порядок обхода: ";
	BFS(start_2 - 1);

	delete[] visited;
	cout << endl;
	return 0;
}