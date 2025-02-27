namespace KPO_minidz;

public class Car
{
    public IEngine Engine { get; }
    public int SerialNumber { get; }
    public bool IsSold { get; set; }

    public Car(IEngine engine, int serialNumber)
    {
        Engine = engine;
        SerialNumber = serialNumber;
    }

    public bool IsCompatibleWith(Customer customer)
    {
        return Engine.CheckCompatibility(customer);
    }
}