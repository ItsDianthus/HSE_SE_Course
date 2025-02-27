namespace KPO_minidz;

public class HandCarFactory : ICarFactory<EmptyEngineParams>
{
    public Car CreateCar(EmptyEngineParams parameters, int serialNumber)
    {
        return new Car(new HandEngine(), serialNumber);
    }
}

public struct EmptyEngineParams
{
    public static readonly EmptyEngineParams Default = new();
}