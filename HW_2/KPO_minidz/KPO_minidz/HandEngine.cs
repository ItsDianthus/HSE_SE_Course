namespace KPO_minidz;


public class HandEngine : IEngine
{
    public bool CheckCompatibility(Customer customer)
    {
        return customer.HandStrength > 5;
    }

    public override string ToString()
    {
        return "Ручной двигатель";
    }
}