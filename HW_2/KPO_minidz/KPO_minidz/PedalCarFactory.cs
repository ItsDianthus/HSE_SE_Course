namespace KPO_minidz;

public class PedalCarFactory : ICarFactory<PedalEngineParams>
{
    public Car CreateCar(PedalEngineParams parameters, int serialNumber)
    {
        return new Car(new PedalEngine(parameters.PedalSize), serialNumber);
    }
}

public class PedalEngineParams
{
    public double PedalSize { get; set; }
}