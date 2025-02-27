
namespace KPO_minidz
{
    public class Program
    {
        static void Main()
        {
            var carService = new CarService();
            var customerStorage = new CustomerStorage();
            var pedalFactory = new PedalCarFactory();
            var handCarFactory = new HandCarFactory();
            var hseService = new HseCarService(carService, customerStorage);
            

            customerStorage.AddCustomer(new Customer("Андрей В.", 6, 4));
            customerStorage.AddCustomer(new Customer("Александр Б.", 4, 6));
            customerStorage.AddCustomer(new Customer("Алёна К.", 6, 6));
            customerStorage.AddCustomer(new Customer("Евгений Н.", 4, 4));

            carService.AddCars(pedalFactory, new PedalEngineParams { PedalSize = 15.5 }, 2);
            carService.AddCars(handCarFactory, EmptyEngineParams.Default, 2);

            Console.WriteLine("* * * * * * * В начале * * * * * * *");
            PrintCustStatus(customerStorage.GetCustomers());

            hseService.SellCars();

            Console.WriteLine("\n* * * * * После продажи * * * * * * *");
            PrintCustStatus(customerStorage.GetCustomers());

        }

        static void PrintCustStatus(IEnumerable<Customer> customers)
        {
            foreach (var customer in customers)
            {
                string carInfo;

                if (customer.PurchasedCar is not null)
                {
                    carInfo = $"Имеется {customer.PurchasedCar.Engine} (№{customer.PurchasedCar.SerialNumber})";
                }
                else
                {
                    carInfo = "Авто отсутствует";
                }

                Console.WriteLine($"{customer.Name} в статусе: {carInfo}");
            }
        }
    }
}
