#include <iostream>
#include <vector>
using namespace std;

vector<int> memo(1000, -1);

int fibonacci(int n) {
    if (n <= 1)
        return n;
    if (memo[n] != -1)
        return memo[n];
    memo[n] = fibonacci(n - 1) + fibonacci(n - 2);
    return memo[n];
}

int main() {
    int n = 10; // Example value
    cout << "Fibonacci number " << n << " is " << fibonacci(n) << endl;
    return 0;
}