#include <iostream>
using namespace std;

int fibonacci(int n) {
    if (n <= 1)
        return n;
    int x = 0, y = 1, z;
    for (int i = 2; i <= n; ++i) {
        z = x + y;
        x = y;
        y = z;
    }
    return y;
}

int main() {
    int n = 10; // Example value
    cout << "Fibonacci number " << n << " is " << fibonacci(n) << endl;
    return 0;
}