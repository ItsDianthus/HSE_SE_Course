namespace KPO_minidz;

public interface ICarProvider
{
    Car FindSuitableCar(Customer customer);
}

public interface IEngine
{
    bool CheckCompatibility(Customer customer);
    string ToString();
}
public interface ICarFactory<TParams>
{
    Car CreateCar(TParams parameters, int serialNumber);
}
public interface ICustomersProvider
{
    IEnumerable<Customer> GetCustomers();
}


