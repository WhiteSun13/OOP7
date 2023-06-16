using System;
using System.Collections.Generic;

namespace OOP7
{
    // Класс продукта с основным свойством - Название продукта
    public class Product
    {
        public string Name { get; set; }
    }
    
    // Класс электронного продукта, наследующий от базового класса Продукт
    public class ElectronicsProduct : Product { }

    // Класс одежды, наследующий от базового класса Продукт
    public class ClothingProduct : Product { }

    // Абстрактный класс фабрики продуктов
    public abstract class ProductFactory
    {
        public abstract Product CreateProduct(string name);
    }

    // Фабрика электронных продуктов
    public class ElectronicsProductFactory : ProductFactory
    {
        public override Product CreateProduct(string name)
        {
            return new ElectronicsProduct { Name = name };
        }
    }

    // Фабрика одежды
    public class ClothingProductFactory : ProductFactory
    {
        public override Product CreateProduct(string name)
        {
            return new ClothingProduct { Name = name };
        }
    }

    // Интерфейс наблюдателя
    public interface IObserver
    {
        void Update(Product product);
    }

    // Интерфейс наблюдаемого
    public interface IObservable
    {
        void AddObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers(Product product);
    }

    // Класс базы данных
    public class Database : IObservable
    {
        private List<IObserver> _observers;

        // Олицетворение паттерна Одиночка (Singleton)
        private static Database _instance;

        private Database()
        {
            _observers = new List<IObserver>();
        }

        public static Database Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Database();
                }
                return _instance;
            }
        }

        public void SaveProduct(Product product)
        {
            Console.WriteLine($"Продукт '{product.Name}' типа {product.GetType().Name} сохранён.");
            NotifyObservers(product);
        }

        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers(Product product)
        {
            foreach (IObserver observer in _observers)
            {
                observer.Update(product);
            }
        }
    }

    public class EmailNotifier : IObserver
    {
        public void Update(Product product)
        {
            Console.WriteLine($"Сообщение на электронную почу: Продукт '{product.Name}' типа {product.GetType().Name} сохранён.");
        }
    }

    // Декоратор продуктов
    public abstract class ProductDetailsDecorator : Product
    {
        protected Product _product;

        public ProductDetailsDecorator(Product product)
        {
            _product = product;
        }

        public abstract string GetDetails();
    }


    public class ElectronicsDetailsDecorator : ProductDetailsDecorator
    {
        public ElectronicsDetailsDecorator(Product product) : base(product)
        {
        }

        public override string GetDetails()
        {
            return $"Цифровая и бытовая техника: {_product.Name}";
        }
    }

    // Декоратор одежды
    public class ClothingDetailsDecorator : ProductDetailsDecorator
    {
        public ClothingDetailsDecorator(Product product) : base(product)
        {
        }

        public override string GetDetails()
        {
            return $"Одежда: {_product.Name}";
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в Интернет магазин!");

            // Создание фабрик для электронных товаров и одежды
            ProductFactory electronicsFactory = new ElectronicsProductFactory();
            ProductFactory clothingFactory = new ClothingProductFactory();

            // Создание продуктов с использованием фабрик
            Product smartphone = electronicsFactory.CreateProduct("Смартфон");
            Product tShirt = clothingFactory.CreateProduct("Футболка");

            // Применение декораторов для добавления детальной информации о продуктах
            ProductDetailsDecorator smartphoneDetails = new ElectronicsDetailsDecorator(smartphone);
            ProductDetailsDecorator tShirtDetails = new ClothingDetailsDecorator(tShirt);

            // Вывод детальной информации о продуктах на экран
            Console.WriteLine(smartphoneDetails.GetDetails());
            Console.WriteLine(tShirtDetails.GetDetails());

            // Создание наблюдателя для оповещения клиентов по электронной почте
            IObserver emailNotifier = new EmailNotifier();
            
            // Регистрация наблюдателя в базе данных
            Database.Instance.AddObserver(emailNotifier);
            
            // Добавление продуктов в базу данных
            Database.Instance.SaveProduct(smartphone);
            Database.Instance.SaveProduct(tShirt);

            Console.ReadLine();
        }
    }
}