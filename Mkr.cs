
using System;
using System.Collections.Generic;

namespace Mkr1
{
    public class Order
    {
        public int Number { get; }
        public string Description { get; }
        public decimal Weight { get; }
        public decimal Distance { get; }
        public Transport AssignedTransport { get; private set; }
        public bool IsDelivered { get; private set; }
        public string PaymentMethod { get; private set; }

        public Order(int number, string description, decimal weight, decimal distance)
        {
            Number = number;
            Description = description;
            Weight = weight;
            Distance = distance;
        }

        public void AssignTransport(Transport transport)
        {
            AssignedTransport = transport;
        }

        public void MarkDelivered(string paymentMethod)
        {
            IsDelivered = true;
            PaymentMethod = paymentMethod;
        }
    }

    public abstract class Transport
    {
        public string Name { get; }
        public decimal MaxWeight { get; }
        public bool IsBusy { get; private set; }

        public Transport(string name, decimal maxWeight)
        {
            Name = name;
            MaxWeight = maxWeight;
        }

        public abstract decimal CalculateCost(Order order);

        public virtual bool CanTake(Order order)
        {
            return order.Weight <= MaxWeight;
        }

        public void Assign()
        {
            IsBusy = true;
        }

        public void Release()
        {
            IsBusy = false;
        }
    }

    public class Bike : Transport
    {
        public Bike(string name, decimal maxWeight) : base(name, maxWeight)
        {
        }

        public override decimal CalculateCost(Order order)
        {
            return 20 + 5 * order.Distance;
        }

        public override bool CanTake(Order order)
        {
            return base.CanTake(order) && order.Distance <= 10;
        }
    }

    public class Car : Transport
    {
        public Car(string name, decimal maxWeight) : base(name, maxWeight)
        {
        }

        public override decimal CalculateCost(Order order)
        {
            return 50 + 8 * order.Distance + order.Weight;
        }
    }

    public class Truck : Transport
    {
        public Truck(string name, decimal maxWeight) : base(name, maxWeight)
        {
        }

        public override decimal CalculateCost(Order order)
        {
            return 200 + 15 * order.Distance + 0.5m * order.Weight;
        }
    }

    public interface IPaymentMethod
    {
        string Name { get; }
        void Pay(decimal amount);
    }

    public class CardPayment : IPaymentMethod
    {
        public string Name => "Картка";

        public void Pay(decimal amount)
        {
            Console.WriteLine($"Оплата карткою: {amount} грн");
        }
    }

    public class CashPayment : IPaymentMethod
    {
        public string Name => "Готівка";

        public void Pay(decimal amount)
        {
            Console.WriteLine($"Оплата готівкою: {amount} грн");
        }
    }

    public class DeliveryService
    {
        private List<Transport> _transports = new List<Transport>();
        private List<Order> _activeOrders = new List<Order>();
        private List<Order> _history = new List<Order>();

        public void AddTransport(Transport transport)
        {
            _transports.Add(transport);
        }

        public void AddOrder(Order order)
        {
            _activeOrders.Add(order);
        }

        public void ShowCosts(Order order)
        {
            Console.WriteLine($"\nЗамовлення №{order.Number}");

            foreach (Transport transport in _transports)
            {
                if (transport.CanTake(order))
                {
                    Console.WriteLine($"{transport.Name}: {transport.CalculateCost(order)} грн");
                }
            }
        }

        public bool AssignOrder(Order order, Transport transport)
        {
            if (!_activeOrders.Contains(order) || transport.IsBusy || !transport.CanTake(order))
                return false;

            transport.Assign();
            order.AssignTransport(transport);

            return true;
        }

        public bool DeliverOrder(Order order, IPaymentMethod payment)
        {
            if (!_activeOrders.Contains(order) || order.AssignedTransport == null)
                return false;

            decimal cost = order.AssignedTransport.CalculateCost(order);

            payment.Pay(cost);
            order.MarkDelivered(payment.Name);
            order.AssignedTransport.Release();

            _activeOrders.Remove(order);
            _history.Add(order);

            return true;
        }

        public void ShowActiveOrders()
        {
            Console.WriteLine("\nАктивні замовлення:");

            foreach (Order order in _activeOrders)
            {
                Console.WriteLine($"№{order.Number}, {order.Description}, {order.Weight} кг");
            }
        }

        public void ShowHistory()
        {
            Console.WriteLine("\nІсторія доставок:");

            foreach (Order order in _history)
            {
                Console.WriteLine($"№{order.Number}, {order.Description}, оплата: {order.PaymentMethod}");
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            DeliveryService service = new DeliveryService();

            Bike bike = new Bike("Велосипед", 5);
            Car car = new Car("Автомобіль", 500);
            Truck truck = new Truck("Вантажівка", 5000);

            service.AddTransport(bike);
            service.AddTransport(car);
            service.AddTransport(truck);

            Order order1 = new Order(1, "Документи", 2, 4);
            Order order2 = new Order(2, "Коробки", 20, 15);
            Order order3 = new Order(3, "Меблі", 600, 30);
            Order order4 = new Order(4, "Посилка", 1, 3);

            service.AddOrder(order1);
            service.AddOrder(order2);
            service.AddOrder(order3);
            service.AddOrder(order4);

            service.ShowCosts(order1);
            service.ShowCosts(order2);
            service.ShowCosts(order3);

            Console.WriteLine("\nПризначення замовлень:");

            Console.WriteLine($"Замовлення 1 на велосипед: {service.AssignOrder(order1, bike)}");
            Console.WriteLine($"Замовлення 4 на зайнятий велосипед: {service.AssignOrder(order4, bike)}");
            Console.WriteLine($"Замовлення 2 на велосипед: {service.AssignOrder(order2, bike)}");
            Console.WriteLine($"Замовлення 2 на автомобіль: {service.AssignOrder(order2, car)}");
            Console.WriteLine($"Замовлення 3 на автомобіль: {service.AssignOrder(order3, car)}");
            Console.WriteLine($"Замовлення 3 на вантажівку: {service.AssignOrder(order3, truck)}");

            service.ShowActiveOrders();

            Console.WriteLine("\nДоставка:");

            service.DeliverOrder(order1, new CardPayment());
            service.DeliverOrder(order2, new CashPayment());
            service.DeliverOrder(order3, new CardPayment());

            service.ShowActiveOrders();
            service.ShowHistory();
        }
    }
}
