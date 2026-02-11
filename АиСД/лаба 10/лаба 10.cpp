#include <iostream>
#include <vector>
#include <random>
#include <string>
#include <ctime>
#include <algorithm>
#include <climits>
#include <cmath>

using namespace std;
#define MAX_DIST 99

typedef vector<vector<int>> Matrix;
typedef vector<vector<double>> MatrixP;
typedef vector<int> Path;

struct Way {
    Path way;
    size_t dist;
};

string to_string(Way way) {
    string text = "Путь: ";
    for (auto i : way.way) {
        text += to_string(i) + " -> ";
    }
    text += "\nДлина пути: " + to_string(way.dist) + "\n";
    return text;
}

void fillAll(vector<bool>& path, size_t N) {
    path.clear();
    for (size_t i = 0; i < N; i++) {
        path.push_back(true);
    }
}

void fillPropabs(
    Matrix& graph,
    MatrixP& pheramons,
    vector<double>& propabs,
    vector<bool> notv,
    double alpha,
    double beta,
    size_t current)
{
    double sum = 0.0;
    propabs.resize(graph.size(), 0.0);

    for (size_t i = 0; i < notv.size(); i++) {
        if (notv[i] && i != current && graph[current][i] > 0) {
            double pheromone = max(0.01, pheramons[current][i]);
            double distance = max(1.0, (double)graph[current][i]);
            sum += pow(pheromone, alpha) * pow(1.0 / distance, beta);
        }
    }

    if (sum < 0.00001) {
        for (size_t i = 0; i < propabs.size(); i++) {
            if (notv[i] && i != current) {
                propabs[i] = 100.0 / (graph.size() - 1);
            }
            else {
                propabs[i] = 0;
            }
        }
        return;
    }

    for (size_t i = 0; i < propabs.size(); i++) {
        if (notv[i] && i != current && graph[current][i] > 0) {
            double pheromone = max(0.01, pheramons[current][i]);
            double distance = max(1.0, (double)graph[current][i]);
            propabs[i] = 100.0 * pow(pheromone, alpha) * pow(1.0 / distance, beta) / sum;
        }
        else {
            propabs[i] = 0;
        }
    }
}

size_t makeChoise(vector<double> propabs) {
    double total = 0.0;
    for (double p : propabs) total += p;

    if (total <= 0.0) {
        for (size_t i = 0; i < propabs.size(); i++) {
            if (propabs[i] > 0) return i;
        }
        return 0;
    }

    double random = (rand() % 10000) / 10000.0 * total;
    double sum = 0.0;

    for (size_t i = 0; i < propabs.size(); i++) {
        sum += propabs[i];
        if (sum >= random) {
            return i;
        }
    }

    return propabs.size() - 1;
}

void fillPheramons(MatrixP& pheromones, vector<Way> ways) {
    for (auto& el : pheromones) {
        for (auto& i : el) {
            i *= 0.9;
            if (i < 0.01) i = 0.01;
        }
    }

    for (auto& way : ways) {
        if (way.dist > 0 && way.way.size() > 1) {
            double change = 100.0 / way.dist;
            for (size_t i = 0; i < way.way.size() - 1; i++) {
                int from = way.way[i];
                int to = way.way[i + 1];
                if (from < pheromones.size() && to < pheromones.size()) {
                    pheromones[from][to] += change;
                    pheromones[to][from] += change;
                }
            }
        }
    }
}

int main() {
    setlocale(LC_ALL, "Ru");
    srand(time(NULL));

    int N, nIters;
    double alpha, beta, pher;
    cout << "Введите кол-во городов: "; cin >> N;
    cout << "Введите кол-во итераций: "; cin >> nIters;
    cout << "Введите альфа: "; cin >> alpha;
    cout << "Введите бета: "; cin >> beta;
    cout << "Введите начальное значение феромонов(<1.0): "; cin >> pher;

    Matrix cities;
    cities.resize(N);
    for (auto& i : cities) {
        i.resize(N);
    }

    for (int i = 0; i < N; i++) {
        cities[i][i] = 0;
        for (int j = i + 1; j < N; j++) {
            cities[i][j] = cities[j][i] = rand() % MAX_DIST + 1;
        }
    }

    cout << "\nМатрица расстояний:\n";
    for (auto i : cities) {
        for (auto el : i) {
            cout << el << "\t";
        }
        cout << endl;
    }

    MatrixP pheromons;
    pheromons.resize(N);
    for (auto& i : pheromons) {
        i.resize(N);
    }

    for (int i = 0; i < N; i++) {
        pheromons[i][i] = 0;
        for (int j = i + 1; j < N; j++) {
            pheromons[i][j] = pheromons[j][i] = pher;
        }
    }

    Way best;
    best.dist = INT_MAX;

    int n = 0;

    while (n < nIters) {
        vector<Way> ways;

        for (int ant = 0; ant < N * 5; ant++) {
            Way way;
            way.dist = 0;

            vector<bool> notv;
            fillAll(notv, N);

            int current = rand() % N;
            int start = current;
            way.way.push_back(current);
            notv[current] = false;

            int visited_count = 1;

            while (visited_count < N) {
                vector<double> propabs;
                fillPropabs(cities, pheromons, propabs, notv, alpha, beta, current);

                int to = makeChoise(propabs);
                if (to >= N || !notv[to] || to == current) {
                    for (int i = 0; i < N; i++) {
                        if (notv[i] && i != current) {
                            to = i;
                            break;
                        }
                    }
                }

                way.dist += cities[current][to];
                current = to;
                way.way.push_back(current);
                notv[current] = false;
                visited_count++;
            }

            way.dist += cities[current][start];
            way.way.push_back(start);

            if (way.dist < best.dist) {
                best = way;
                cout << "\nНовый лучший путь на итерации " << n + 1 << ": ";
                cout << to_string(best);
            }

            ways.push_back(way);
        }

        fillPheramons(pheromons, ways);
        n++;
    }

    cout << "\nИтоговый лучший маршрут: " << to_string(best);

    return 0;
}