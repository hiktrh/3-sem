#include <iostream>
#include <vector>
using namespace std;

struct element {
    int value;
    int length;

    element(int el) {
        value = el;
        length = 1;
    }
};

vector<element> subsequence(vector<element>& sequence, int N) {
    for (int i = 1; i < N; i++) {
        for (int j = i - 1; j >= 0; j--) {
            if (sequence[j].value < sequence[i].value && sequence[j].length >= sequence[i].length) {
                sequence[i].length = sequence[j].length + 1;
            }
        }
    }

    int max_length_index = 0;
    for (int i = 0; i < N; i++) {
        if (sequence[i].length > sequence[max_length_index].length) {
            max_length_index = i;
        }
    }

    vector<int> result_sequence;
    int current_length = sequence[max_length_index].length;
    for (int i = max_length_index; i >= 0; i--) {
        if (sequence[i].length == current_length && sequence[i].value <= sequence[max_length_index].value) {
            result_sequence.push_back(sequence[i].value);
            current_length--;
        }
    }
    cout << endl;

    cout << "Длина максимальной подпоследовательности: " << sequence[max_length_index].length << endl;
    cout << "Максимальная возрастающая подпоследовательность: ";
    for (int i = result_sequence.size() - 1; i >= 0; i--) {
        cout << result_sequence[i] << " ";
    }
    cout << endl;

    return sequence;
}

int main()
{
    setlocale(LC_ALL, "Rus");
    int N, el;
    cout << "Введите количество элементов в последовательности: ";
    cin >> N;
    vector<element> sequence;

    cout << "Введите элементы последовательности: ";
    for (int i = 0; i < N; i++) {
        cin >> el;
        sequence.push_back(element(el));
    }

    subsequence(sequence, N);
}