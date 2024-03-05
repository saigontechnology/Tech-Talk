namespace C_11_Demo.Pieces;
internal interface IMyService
{
    static abstract double FeeRate { get; }

    double GetCost();
}

internal class MyCarService : IMyService
{
    public static double FeeRate => 0.05;

    public double GetCost()
    {
        return 10;
    }
}

internal class MyBoatService : IMyService
{
    public static double FeeRate => 0.1;

    public double GetCost()
    {
        return 20;
    }
}

internal class MyServiceDemo
{
    public double Total()
    {
        var carService = new MyCarService();
        var carCost = carService.GetCost() * (1 + GetFeeRate<MyCarService>());

        var boatService = new MyBoatService();
        var boatCost = boatService.GetCost() * (1 + GetFeeRate<MyBoatService>());

        return carCost + boatCost;
    }

    public double GetFeeRate<T>() where T : class, IMyService
    {
        return T.FeeRate;
    }
}