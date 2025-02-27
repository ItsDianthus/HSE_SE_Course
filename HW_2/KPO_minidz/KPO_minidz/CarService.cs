namespace KPO_minidz;

public class CarService : ICarProvider
{
    private readonly List<Car> _cars = new();
    private int _nextSerialNumber = 1;

    public void AddCars<TParams>(ICarFactory<TParams> factory, TParams parameters, int count)
    {
        for (int i = 0; i < count; i++)
        {
            _cars.Add(factory.CreateCar(parameters, _nextSerialNumber++));
        }
    }

    public Car FindSuitableCar(Customer customer)
    {
        var car = _cars.FirstOrDefault(c => !c.IsSold && c.IsCompatibleWith(customer));
        if (car != null) car.IsSold = true;
        return car;
    }
}