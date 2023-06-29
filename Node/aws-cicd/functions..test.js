const { multiply, sum } = require("./functions");

test("Multiplying two numbers", async () => {
  expect(multiply(10, 10)).toStrictEqual(100);
  expect(multiply(200, 100)).toStrictEqual(20000);
});

test("Summing two numbers", async () => {
  expect(sum(10, 10)).toStrictEqual(20);
  expect(sum(200, 100)).toStrictEqual(300);
});
