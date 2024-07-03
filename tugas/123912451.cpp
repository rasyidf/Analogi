#include <iostream>
using namespace std;

inline int fibonacci(int n) {
    return (n <= 1) ? n : fibonacci(n - 1) + fibonacci(n - 2);
}

int main() {
    int n = 10; // Example value
    cout << "Fibonacci number " << n << " is " << fibonacci(n) << endl;
    return 0;
}