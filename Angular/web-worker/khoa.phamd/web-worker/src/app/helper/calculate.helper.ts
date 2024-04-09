export class CalculateHelper {
  static  async calculateComplexTask(number: number) {
    // Simulate the complex calculation, take much time to finish: 10s
    await CalculateHelper.customDelay(10000);
    return number*number;
  }

  static customDelay(ms:number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}