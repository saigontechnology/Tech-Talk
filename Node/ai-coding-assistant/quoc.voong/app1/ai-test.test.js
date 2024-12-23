const fibonacci = require('./ai-test');

test('calculates the 0th Fibonacci number', () => {
    expect(fibonacci(0)).toBe(0);
});

test('calculates the 1st Fibonacci number', () => {
    expect(fibonacci(1)).toBe(1);
});

test('calculates the 2nd Fibonacci number', () => {
    expect(fibonacci(2)).toBe(1);
});

test('calculates the 3rd Fibonacci number', () => {
    expect(fibonacci(3)).toBe(2);
});

test('calculates the 4th Fibonacci number', () => {
    expect(fibonacci(4)).toBe(3);
});

test('calculates the 5th Fibonacci number', () => {
    expect(fibonacci(5)).toBe(5);
});

test('calculates the 10th Fibonacci number', () => {
    expect(fibonacci(10)).toBe(55);
});
test('calculates the 0th Fibonacci number', () => {
    expect(fibonacci(0)).toBe(0);
});

test('calculates the 1st Fibonacci number', () => {
    expect(fibonacci(1)).toBe(1);
});

test('calculates the 2nd Fibonacci number', () => {
    expect(fibonacci(2)).toBe(1);
});

test('calculates the 3rd Fibonacci number', () => {
    expect(fibonacci(3)).toBe(2);
});

test('calculates the 4th Fibonacci number', () => {
    expect(fibonacci(4)).toBe(3);
});

test('calculates the 5th Fibonacci number', () => {
    expect(fibonacci(5)).toBe(5);
});

test('calculates the 10th Fibonacci number', () => {
    expect(fibonacci(10)).toBe(55);
});

test('calculates the 15th Fibonacci number', () => {
    expect(fibonacci(15)).toBe(610);
});

test('calculates the 20th Fibonacci number', () => {
    expect(fibonacci(20)).toBe(6765);
});

test('calculates the 30th Fibonacci number', () => {
    expect(fibonacci(30)).toBe(832040);
});
