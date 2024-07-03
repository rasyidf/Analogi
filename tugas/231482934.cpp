#include <iostream>
using namespace std;

int fibonacci(int n) {
    if (n <= 1)
        return n;
    int prev = 1, curr = 0;
    for (int i = 2; i <= n; ++i) {
        int next = curr + prev;
        curr = prev;
        prev = next;
    }
    return prev;
}

int main() {
    int n = 10; // Example value
    cout << "Fibonacci number " << n << " is " << fibonacci(n) << endl;
    return 0;
}