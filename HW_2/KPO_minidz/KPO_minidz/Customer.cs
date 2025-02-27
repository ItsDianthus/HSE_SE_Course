namespace KPO_minidz;

public class Customer
{
    public string Name { get; }
    public int LegStrength { get; }
    public int HandStrength { get; }
    public Car PurchasedCar { get; set; }

    public Customer(string name, int legStrength, int handStrength)
    {
        Name = name;
        LegStrength = legStrength;
        HandStrength = handStrength;
    }
}