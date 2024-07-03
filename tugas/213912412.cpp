#include <iostream>
using namespace std;

int fibonacci(int n) {
    if (n <= 1)
        return n;
    int prev2 = 0, prev1 = 1;
    for (int i = 2; i <= n; ++i) {
        int curr = prev1 + prev2;
        prev2 = prev1;
        prev1 = curr;
    }
    return prev1;
}

int main() {
    int n = 10; // Example value
    cout << "Fibonacci number " << n << " is " << fibonacci(n) << endl;
    return 0;
}