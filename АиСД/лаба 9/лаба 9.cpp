#include <iostream>
#include <vector>
#include <algorithm>
#include <cstdlib>
#include <ctime>
#include <climits>
using namespace std;

struct Individ {
    vector<int> route;
    int routeSum;

    Individ(int v) : route(v), routeSum(0) {}

    void calculateRouteSum(const vector<vector<int>>& matList) {
        routeSum = 0;
        for (int i = 0; i < route.size() - 1; i++) {
            routeSum += matList[route[i] - 1][route[i + 1] - 1];    
        }
        routeSum += matList[route.back() - 1][route[0] - 1];
    }

    void generateFirstPopulationIndivid(const vector<vector<int>>& matList) {
        for (int i = 0; i < route.size(); i++) {
            route[i] = i + 1;
        }
        random_shuffle(route.begin(), route.end());
        calculateRouteSum(matList);
    }

    void crossGenerateIndivid(const Individ& a, const Individ& b, int mutationChance, const vector<vector<int>>& matList) {
        vector<bool> used(route.size(), false);
        int chromAmount = rand() % a.route.size();

        for (int i = 0; i < chromAmount; i++) {
            route[i] = a.route[i];
            used[route[i] - 1] = true;
        }

        int index = chromAmount;
        for (int i = chromAmount - 1; i < b.route.size(); i++) {
            if (!used[b.route[i] - 1]) {
                route[index++] = b.route[i];
                used[b.route[i] - 1] = true;
            }
        }

        for (int i = 0; i < a.route.size(); i++) {
            if (!used[a.route[i] - 1]) {
                route[index++] = a.route[i];
            }
        }

        if ((rand() % 100) < mutationChance) {
            swap(route[rand() % route.size()], route[rand() % route.size()]);
        }
        calculateRouteSum(matList);
    }   

    bool operator==(const Individ& other) const {
        return route == other.route;
    }
};

void sortPopulation(vector<Individ>& population) {
    sort(population.begin(), population.end(), [](const Individ& a, const Individ& b) {
        return a.routeSum < b.routeSum;
        });
}

class Graph {
public:
    int verticles;
    vector<vector<int>> matList;

    Graph(int v) : verticles(v), matList(v, vector<int>(v, INT_MAX)) {}

    void addEdge(int x, int y, int value) {
        matList[x - 1][y - 1] = value;
    }

    void addCity() {
        verticles++;
        int city = verticles;
        for (auto& row : matList) {
            row.push_back(INT_MAX);
        }
        matList.push_back(vector<int>(verticles, INT_MAX));

        for (int i = 1; i < verticles; i++) {

            while (true) {
                cout << "Введите стоимость маршрута из города " << city << " в город " << i << ": ";
                int length;
                cin >> length;
                addEdge(city, i, length);
                break;
            }

            while (true) {
                cout << "Введите стоимость маршрута из города " << i << " в город " << city << ": ";
                int length;
                cin >> length;
                addEdge(i, city, length);
                break;
            }
        }

        cout << "Город добавлен. Теперь их " << verticles << ".\n";
    }

    void deleteCity(int x) {
        x--;
        matList.erase(matList.begin() + x);
        for (auto& row : matList) {
            row.erase(row.begin() + x);
        }
        verticles--;
        cout << "Город удалён. Осталось " << verticles << " городов.\n";
    }
};

int main() {
    setlocale(LC_ALL, "Ru");
    srand(time(nullptr));
    Graph routes(8);

    routes.addEdge(1, 2, 17);
    routes.addEdge(1, 3, 10);
    routes.addEdge(1, 4, 11);
    routes.addEdge(1, 5, 30);
    routes.addEdge(1, 6, 3);
    routes.addEdge(1, 7, 10);
    routes.addEdge(1, 8, 15);

    routes.addEdge(2, 1, 14);
    routes.addEdge(2, 3, 19);
    routes.addEdge(2, 4, 12);
    routes.addEdge(2, 5, 17);
    routes.addEdge(2, 6, 15);
    routes.addEdge(2, 7, 22);
    routes.addEdge(2, 8, 26);

    routes.addEdge(3, 1, 15);
    routes.addEdge(3, 2, 12);
    routes.addEdge(3, 4, 17);
    routes.addEdge(3, 5, 10);
    routes.addEdge(3, 6, 1);
    routes.addEdge(3, 7, 19);
    routes.addEdge(3, 8, 21);


    routes.addEdge(4, 1, 20);
    routes.addEdge(4, 2, 18);
    routes.addEdge(4, 3, 14);
    routes.addEdge(4, 5, 12);
    routes.addEdge(4, 6, 11);
    routes.addEdge(4, 7, 5);
    routes.addEdge(4, 8, 2);

    routes.addEdge(5, 1, 25);
    routes.addEdge(5, 2, 22);
    routes.addEdge(5, 3, 19);
    routes.addEdge(5, 4, 17);
    routes.addEdge(5, 6, 20);
    routes.addEdge(5, 7, 19);
    routes.addEdge(5, 8, 23);

    routes.addEdge(6, 1, 12);
    routes.addEdge(6, 2, 3);
    routes.addEdge(6, 3, 9);
    routes.addEdge(6, 4, 17);
    routes.addEdge(6, 5, 20);
    routes.addEdge(6, 7, 10);
    routes.addEdge(6, 8, 2);

    routes.addEdge(7, 1, 22);
    routes.addEdge(7, 2, 31);
    routes.addEdge(7, 3, 19);
    routes.addEdge(7, 4, 7);
    routes.addEdge(7, 5, 2);
    routes.addEdge(7, 6, 20);
    routes.addEdge(7, 8, 23);

    routes.addEdge(8, 1, 12);
    routes.addEdge(8, 2, 14);
    routes.addEdge(8, 3, 30);
    routes.addEdge(8, 4, 24);
    routes.addEdge(8, 5, 13);
    routes.addEdge(8, 6, 2);
    routes.addEdge(8, 7, 33);

    bool going = true;
    while (going) {
        cout << "Добавить город? (1 - да, 2 - нет): ";
        int ans;
        cin >> ans;
        if (ans == 1) {
            int cityNumber;

            routes.addCity();
            break;
        }
        else if (ans == 2) {
            going = false;
        }
        else {
            cout << "Введите корректный ответ.\n";
        }
    }

    going = true;
    while (going) {
        cout << "Удалить город? (1 - да, 2 - нет): ";
        int ans;
        cin >> ans;
        if (ans == 1) {
            int city;
            cout << "Введите номер города для удаления: ";
            cin >> city;
            if (city >= 1 && city <= routes.verticles) {
                routes.deleteCity(city);
            }
            else {
                cout << "Некорректный номер города.\n";
            }
        }
        else if (ans == 2) {
            going = false;
        }
        else {
            cout << "Введите корректный ответ.\n";
        }
    }

    int startPopulation, mutationChance, crossAmount, evolAmount;

    do {
        cout << "\nВведите количество особей в начальной популяции (>= 2): ";
        cin >> startPopulation;
    } while (startPopulation < 2);

    do {
        cout << "Введите показатель по мутации (>= 0): ";
        cin >> mutationChance;
    } while (mutationChance < 0);

    do {
        cout << "Введите количество скрещиваний на каждой эволюции: ";
        cin >> crossAmount;
    } while (crossAmount < 1);

    do {
        cout << "Введите количество эволюций: ";
        cin >> evolAmount;
        if (cin.fail()) {
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
            evolAmount = 0;
        }
    } while (evolAmount < 1);


    vector<Individ> population;
    while (population.size() < startPopulation) {
        Individ individual(routes.verticles);   
        individual.generateFirstPopulationIndivid(routes.matList);  
        if (find(population.begin(), population.end(), individual) == population.end()) {
            population.push_back(individual);
        }
    }

    sortPopulation(population);
    cout << "\n\nНачальная популяция:\n";
    for (size_t i = 0; i < population.size(); i++) {
        cout << "Длина маршрута = " << population[i].routeSum << ", маршрут: ";
        for (auto city : population[i].route) {
            cout << city << " ";
        }
        cout << endl;
    }

    int populationNumber = 1;
    for (int i = 0; i < evolAmount; i++, populationNumber++) {

        for (int j = 0; j < crossAmount; j++) {
            Individ son(routes.verticles);
            Individ daughter(routes.verticles);

            int dad = rand() % population.size();
            int mom;
            do {
                mom = rand() % population.size();
            } while (mom == dad);

            son.crossGenerateIndivid(population[dad], population[mom], mutationChance, routes.matList);
            daughter.crossGenerateIndivid(population[mom], population[dad], mutationChance, routes.matList);

            if (find(population.begin(), population.end(), son) == population.end()) {
                population.push_back(son);
            }
            if (find(population.begin(), population.end(), daughter) == population.end()) {
                population.push_back(daughter);
            }
        }

        sortPopulation(population);
        while (population.size() > startPopulation) {
            population.pop_back();
        }

        cout << "\n\nПопуляция после эволюции " << populationNumber << ":\n";
        for (size_t i = 0; i < population.size(); i++) {
            cout << "Длина маршрута = " << population[i].routeSum << ", маршрут: ";
            for (auto city : population[i].route) {
                cout << city << " ";
            }
            cout << endl;
        }
    }

    cout << "\n\nДлина маршрута: " << population[0].routeSum << "\nМинимальный маршрут коммивояжера: ";
    for (auto city : population[0].route) {
        cout << city << " ";
    }
    cout << endl;
}