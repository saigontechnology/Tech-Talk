/**
 * Calculates the nth Fibonacci number using memoization.
 *
 * @param {number} n - The position in the Fibonacci sequence.
 * @param {Object} [memo={}] - An object to store previously calculated Fibonacci numbers.
 * @returns {number} - The nth Fibonacci number.
 */
function fibonacci(n, memo = {0: 0, 1: 1}) {
    if (memo[n] !== undefined) return memo[n];
    for (let i = 2; i <= n; i++) {
        memo[i] = memo[i - 1] + memo[i - 2];
    }
    return memo[n];
}
