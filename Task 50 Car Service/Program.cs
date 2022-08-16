using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_50_Car_Service
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CarService carService = new CarService();
            carService.StartGame();
        }
    }

    class Detal
    {
        public string Name { get; private set; }
        public bool IsBroken { get; private set; }
        public int Price { get; private set; }
        public int PriceRepairs { get; private set; }

        public Detal(string name, bool isBroke = false, int cost = 0, int priceRepairs = 0)
        {
            Name = name;
            IsBroken = isBroke;
            Price = cost;
            PriceRepairs = priceRepairs;
        }

        public void ShowInfo()
        {
            Console.Write($"{Name} - ");

            if (IsBroken)
            {
                ColorText("сломано");
            }
            else
            {
                ColorText("", "в порядке");
            }
        }

        public void Repair()
        {
            IsBroken = false;
        }

        private void ColorText(string redText = "", string greenText = "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(redText);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(greenText);
            Console.WriteLine();
            Console.ResetColor();
        }

    }

    class Car
    {
        private List<Detal> _detals = new List<Detal>();

        public Car(Random random)
        {
            AddDetals(random);
        }

        public void RepairDetal(int index)
        {
            _detals[index - 1].Repair();
        }

        public void ShowDetal(int index)
        {
            _detals[index].ShowInfo();
        }

        public string GetNameDetal(int index)
        {
            return _detals[index].Name;
        }

        public int GetNumberDetals()
        {
            return _detals.Count;
        }

        private bool BrokeRandomDetal(Random random, int chanceBreakdown)
        {
            int minLimit = 0;
            int maxLimit = 100;
            bool isBroken;

            if (random.Next(minLimit, maxLimit) < chanceBreakdown)
            {
                isBroken = true;
            }
            else
            {
                isBroken = false;
            }

            return isBroken;
        }

        private void AddDetals(Random random)
        {
            _detals.Add(new Detal("Лобовое стекло", BrokeRandomDetal(random, 50)));
            _detals.Add(new Detal("Подвеска", BrokeRandomDetal(random, 60)));
            _detals.Add(new Detal("Топливный насос", BrokeRandomDetal(random, 15)));
            _detals.Add(new Detal("Сигнализация", BrokeRandomDetal(random, 30)));
            _detals.Add(new Detal("Карбюратор", BrokeRandomDetal(random, 20)));
            _detals.Add(new Detal("Свечи зажигания", BrokeRandomDetal(random, 90)));
        }
    }

    class CarService
    {
        private Random _random = new Random();
        private List<Detal> _detalsWarehouse = new List<Detal>();
        private List<Detal> _priceListDetals = new List<Detal>();
        private int _balanace = 5000;
        private Car _car;

        public CarService()
        {
            _car = new Car(_random);
            AddPositionPriceList();
            FillRandomWarehouse();
        }

        public void StartGame()
        {
            bool isWork = true;

            while (isWork == true)
            {
                Console.WriteLine("\n1. Принять машину на ремонт  \n2. Проверить склад \n3. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        AcceptCar();
                        break;

                    case "2":
                        ShowWareHouseInfo();
                        break;

                    case "3":
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("\nНеккоректный ввод\n");
                        break;
                }
            }
        }

        public void ShowCarInfo()
        {
            Console.WriteLine("\nСостояние: ");

            for (int i = 0; i < _car.GetNumberDetals(); i++)
            {
                Console.Write($"{i + 1}. ");
                _car.ShowDetal(i);
            }
        }

        private void AcceptCar()
        {
            bool isWork = true;

            while (isWork)
            {
                ShowCarInfo();
                Console.WriteLine("\nНапишите номер детали, которую необходимо заменить или 0 для выхода");
                bool isNumber = int.TryParse(Console.ReadLine(), out int index);

                if (isNumber == true & index <= _priceListDetals.Count & index > 0)
                {
                    RepairDetalCar(index, FindDetalOnStock(_car.GetNameDetal(index - 1)));
                }
            }
        }

        private void RepairDetalCar(int index, Detal detal)
        {
            if (detal != null)
            {
                _car.RepairDetal(index);
                _balanace += detal.PriceRepairs;
                _detalsWarehouse.Remove(detal);
                Console.WriteLine("Ремонт выполнен");
            }
        }

        private Detal FindDetalOnStock(string name)
        {
            Detal detal = null;

            for (int i = 0; i < _detalsWarehouse.Count; i++)
            {
                if (_detalsWarehouse[i].Name == name)
                {
                    detal = _detalsWarehouse[i];
                    Console.WriteLine("Деталь имеется на складе!");
                    return detal;
                }
            }

            if (detal == null)
            {
                Console.WriteLine("Данной детали нет на складе!");
            }

            return detal;
        }

        private void ShowWareHouseInfo()
        {
            Console.WriteLine("Наличие на складе: \n");
            for (int i = 0; i < _detalsWarehouse.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_detalsWarehouse[i].Name} - {_detalsWarehouse[i].Price} руб. Стоимость замены - {_detalsWarehouse[i].PriceRepairs} руб.");
            }
        }

        private void FillRandomWarehouse()
        {
            int minNumberDetals = 0;
            int maxNumberDetals = 10;

            for (int i = 0; i < _priceListDetals.Count; i++)
            {
                for (int j = 0; j < _random.Next(minNumberDetals, maxNumberDetals); j++)
                {
                    _detalsWarehouse.Add(_priceListDetals[i]);
                }
            }
        }

        private void AddPositionPriceList()
        {
            _priceListDetals.Add(new Detal("Лобовое стекло", false, 1500, 400));
            _priceListDetals.Add(new Detal("Подвеска", false, 3430, 1200));
            _priceListDetals.Add(new Detal("Топливный насос", false, 250, 600));
            _priceListDetals.Add(new Detal("Сигнализация", false, 300, 250));
            _priceListDetals.Add(new Detal("Карбюратор", false, 2570, 1250));
            _priceListDetals.Add(new Detal("Свечи зажигания", false, 120, 300));
        }
    }
}
