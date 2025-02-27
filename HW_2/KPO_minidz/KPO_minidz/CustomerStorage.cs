namespace KPO_minidz;

public class CustomerStorage : ICustomersProvider
{
    private readonly List<Customer> _customers = new();

    public void AddCustomer(Customer customer)
    {
        _customers.Add(customer);
    }

    public IEnumerable<Customer> GetCustomers()
    {
        return _customers.AsReadOnly();
    }
}