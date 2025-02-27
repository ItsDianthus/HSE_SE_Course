namespace KPO_minidz;

public class HseCarService
{
    private readonly ICarProvider _carProvider;
    private readonly ICustomersProvider _customersProvider;

    public HseCarService(ICarProvider carProvider, ICustomersProvider customersProvider)
    {
        _carProvider = carProvider;
        _customersProvider = customersProvider;
    }

    public void SellCars()
    {
        foreach (var customer in _customersProvider.GetCustomers().Where(c => c.PurchasedCar == null))
        {
            var car = _carProvider.FindSuitableCar(customer);
            if (car != null)
            {
                customer.PurchasedCar = car;
            }
        }
    }
}