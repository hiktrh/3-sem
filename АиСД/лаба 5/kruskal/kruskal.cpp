#include <iostream>
#define V 8
using namespace std;

int G[V][V] = {
    {0, 2, 0, 8, 2, 0, 0, 0},
    {2, 0, 3, 10, 5, 0, 0, 0},
    {0, 3, 0, 0, 12, 0, 0, 7},
    {8, 10, 0, 0, 14, 3, 1, 0},
    {2, 5, 12, 14, 0, 11, 4, 8},
    {0, 0, 0, 3, 11, 0, 6, 0},
    {0, 0, 0, 1, 4, 6, 0, 9},
    {0, 0, 7, 0, 8, 0, 9, 0}
};

int main() {
    setlocale(LC_ALL, "rus");
    int edgeCount = 0;
    int visited[V];

    for (int i = 0; i < V; i++) {
        visited[i] = i;
    }

    cout << "Алгоритм Крускала:\n";
    while (edgeCount < V - 1) {
        int min = INT_MAX;
        int a = -1, b = -1;

        for (int i = 0; i < V; i++) {
            for (int j = 0; j < V; j++) {

                int rootI = i;
                while (visited[rootI] != rootI) {
                    rootI = visited[rootI];
                }

                int rootJ = j;
                while (visited[rootJ] != rootJ) {
                    rootJ = visited[rootJ];
                }

                if (rootI != rootJ && G[i][j] < min && G[i][j] != 0) {
                    min = G[i][j];
                    a = i;
                    b = j;
                }
            }
        }

        if (a != -1 && b != -1) {

            int rootA = a;
            while (visited[rootA] != rootA) {
                rootA = visited[rootA];
            }

            int rootB = b;
            while (visited[rootB] != rootB) {
                rootB = visited[rootB];
            }

            cout << a + 1 << " to " << b + 1 << " == " << min << endl;
            visited[rootA] = rootB;
            edgeCount++;
        }
    }
}