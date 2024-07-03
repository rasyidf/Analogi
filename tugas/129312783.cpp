#include <iostream>
#include <vector>
using namespace std;

vector<int> memo(1000, -1);

int fibonacci_helper(int n, vector<int>& memo) {
    if (n <= 1)
        return n;
    if (memo[n] != -1)
        return memo[n];
    memo[n] = fibonacci_helper(n - 1, memo) + fibonacci_helper(n - 2, memo);
    return memo[n];
}

int fibonacci(int n) {
    return fibonacci_helper(n, memo);
}

int main() {
    int n = 10; // Example value
    cout << "Fibonacci number " << n << " is " << fibonacci(n) << endl;
    return 0;
}