using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_50_Car_Service
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CarSercive carSercive = new CarSercive();
            carSercive.StartGame();
        }
    }

    class Detal
    {
        public string Name { get; private set; }
        public bool IsBroken { get; private set; }
        public int Price { get; private set; }
        public int ChanceBreakdown { get; private set; }

        public Detal(string name, bool isBroke = false, int cost = 0, int chanceBreakdown = 0)
        {
            Name = name;
            IsBroken = isBroke;
            Price = cost;
            ChanceBreakdown = chanceBreakdown;
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

    class TypesDetal
    {
        List<Detal> _types = new List<Detal>();

        public TypesDetal()
        {
            Add();
        }

        public int GetQuantity()
        {
            return _types.Count;
        }

        public Detal ReturnType(int index)
        {
            return _types[index];
        }

        private void Add()
        {
            _types.Add(new Detal("Топливный насос", false, 1300, 30));
            _types.Add(new Detal("Свечи зажигания", false, 750, 90));
            _types.Add(new Detal("Аккумулятор", false, 4000, 40));
            _types.Add(new Detal("Проводка", false, 950, 80));
            _types.Add(new Detal("Воздушный фильтр", false, 950, 60));
            _types.Add(new Detal("Редуктор", false, 3276, 20));
            _types.Add(new Detal("Коробка передач", false, 7540, 5));
        }
    }

    class Warehouse
    {
        private TypesDetal _typesDetal = new TypesDetal();
        private List<Detal> _detals = new List<Detal>();

        public Warehouse(Random random)
        {
            FillRandomDetals(random);
        }

        public void ShowInfo()
        {
            Console.WriteLine("Наличие на складе: \n");

            for (int i = 0; i < _typesDetal.GetQuantity(); i++)
            {
                int Quantity = ReturnQuantityDetal(i);
                Console.WriteLine($"{i + 1}. {_typesDetal.ReturnType(i).Name} - {_typesDetal.ReturnType(i).Price} руб - {Quantity} шт.");
            }
        }

        public void DeleteDetal(int index)
        {
            string name = _typesDetal.ReturnType(index).Name;

            for (int i = 0; i < _detals.Count; i++)
            {
                if (name == _detals[i].Name)
                {
                    _detals.RemoveAt(i);
                    return;
                }
            }
        }

        public void AddDetal(int index)
        {
            _detals.Add(_typesDetal.ReturnType(index));
        }

        public int ReturnCostDetal(string name = "", int index = 0)
        {
            int price = 0;

            if (name == "")
            {
                price = _typesDetal.ReturnType(index).Price;
            }
            else
            {
                for (int i = 0; i < _typesDetal.GetQuantity(); i++)
                {
                    if (name == _typesDetal.ReturnType(i).Name)
                    {
                        price = _typesDetal.ReturnType(i).Price;
                        return price;
                    }
                }
            }

            return price;
        }

        public int ReturnQuantityDetal(int index)
        {
            int quantity = 0;
            string name = _typesDetal.ReturnType(index).Name;

            for (int j = 0; j < _detals.Count; j++)
            {
                if (name == _detals[j].Name)
                {
                    quantity++;
                }
            }

            return quantity;
        }

        public int ReturnQuantityTypes()
        {
            return _typesDetal.GetQuantity();
        }

        private void FillRandomDetals(Random random)
        {
            int minQuantityDetals = 0;
            int maxQuantityDetals = 10;

            for (int i = 0; i < _typesDetal.GetQuantity(); i++)
            {
                for (int j = 0; j < random.Next(minQuantityDetals, maxQuantityDetals); j++)
                {
                    _detals.Add(_typesDetal.ReturnType(i));
                }
            }
        }
    }

    class Car
    {
        private TypesDetal _typesDetal = new TypesDetal();
        private List<Detal> _detals = new List<Detal>();

        public Car(Random random)
        {
            AddDetals(random);
        }

        public void ShowInfo()
        {
            Console.WriteLine("\nСостояние: ");

            for (int i = 0; i < _detals.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                _detals[i].ShowInfo();
            }
        }

        public int ReturnQuantityDetals()
        {
            return _detals.Count();
        }

        public void RepairDetal(int index)
        {
            _detals[index].Repair();
        }

        public List<Detal> ReturnListBrokenDetals()
        {
            List<Detal> detals = new List<Detal>();

            for (int i = 0; i < _detals.Count; i++)
            {
                if (_detals[i].IsBroken == true)
                {
                    detals.Add(_detals[i]);
                }
            }

            return detals;
        }

        private void AddDetals(Random random)
        {
            for (int i = 0; i < _typesDetal.GetQuantity(); i++)
            {
                _detals.Add(new Detal(_typesDetal.ReturnType(i).Name, BrokeRandomDetal(random, _typesDetal.ReturnType(i).ChanceBreakdown)));
            }
        }

        private bool BrokeRandomDetal(Random random, int chanceBreakdown)
        {
            int minLimit = 0;
            int maxLimit = 100;

            return random.Next(minLimit, maxLimit) < chanceBreakdown;
        }
    }

    class CarSercive
    {
        private Random _random = new Random();
        private int _balance = 10000;
        private int _costRepair = 1000;
        private int _fineForBrokenDetal = 2000;
        private Warehouse _warehouse;
        private Car _car;

        public CarSercive()
        {
            _warehouse = new Warehouse(_random);
        }

        public void StartGame()
        {
            bool isWork = true;

            while (isWork == true)
            {
                ShowBalance();
                Console.WriteLine("\n1. Принять машину на ремонт  \n2. Проверить склад \n3. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        StartCarRepair();
                        break;

                    case "2":
                        ShowWarehouse();
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

        private void StartCarRepair()
        {
            _car = new Car(_random);
            bool isWork = true;
            ShowRepairPrice();

            while (isWork)
            {
                _car.ShowInfo();
                Console.WriteLine("\n\n1. Заменить детали \n2. Закончить ремонт");

                switch (Console.ReadLine())
                {
                    case "1":
                        RepairCar();
                        break;

                    case "2":
                        FinishRepair();
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("\nНеккоректный ввод\n");
                        break;
                }
            }
        }

        private void ShowWarehouse()
        {
            bool isWork = true;

            while (isWork == true)
            {
                ShowBalance();
                _warehouse.ShowInfo();
                Console.WriteLine("\n1. Купить комплектующие \n2. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        BuyAccessories();
                        break;

                    case "2":
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("\nНеккоректный ввод\n");
                        break;
                }
            }
        }

        private void RepairCar()
        {
            bool isWork = true;

            while (isWork)
            {
                Console.WriteLine("Введите номер детали, которую необходимо заменить: ");
                bool isNumber = int.TryParse(Console.ReadLine(), out int index);

                if (isNumber == true & index - 1 < _car.ReturnQuantityDetals() & index > 0)
                {
                    if (FindDetalInStock(index - 1))
                    {
                        ReplaceDetal(index - 1);
                        isWork = false;
                    }
                    else
                    {
                        Console.WriteLine("Данной детали нет на складе");
                        isWork = false;
                    }
                }
                else
                {
                    Console.WriteLine("Неккоректный ввод");
                    isWork = false;
                }
            }
        }

        private void FinishRepair()
        {
            int quantityBrokeDetals = _car.ReturnListBrokenDetals().Count;

            if (quantityBrokeDetals == 0)
            {
                Console.WriteLine("Ремонт закончен! Клиент остался доволен!");
            }
            else
            {
                PayFine(quantityBrokeDetals);
            }
        }

        private void PayFine(int numberBrokeDetals)
        {
            int fine = 0;

            for (int i = 0; i < numberBrokeDetals; i++)
            {
                fine += _fineForBrokenDetal;
            }

            _balance -= fine;

            Console.WriteLine($"У машины остались сломанные детали! Вы заплатили штраф в размере {fine} руб. {_fineForBrokenDetal} за каждую оставленную деталь");
        }

        private void ReplaceDetal(int index)
        {
            _balance += _costRepair + _warehouse.ReturnCostDetal("", index);
            _car.RepairDetal(index);
            _warehouse.DeleteDetal(index);
            Console.WriteLine("Выполнено!");
        }

        private bool FindDetalInStock(int index)
        {
            return (_warehouse.ReturnQuantityDetal(index) > 0);
        }

        private void BuyAccessories()
        {
            bool isWork = true;

            while (isWork)
            {
                Console.WriteLine("Введите номер детали, которую необходимо купить: ");
                bool isNumber = int.TryParse(Console.ReadLine(), out int index);

                if (isNumber == true & index - 1 < _warehouse.ReturnQuantityTypes() & index > 0)
                {
                    if (_warehouse.ReturnCostDetal("", index - 1) <= _balance)
                    {
                        _warehouse.AddDetal(index - 1);
                        _balance -= _warehouse.ReturnCostDetal("", index - 1);
                        Console.WriteLine("Куплено!");
                        isWork = false;
                    }
                    else
                    {
                        Console.WriteLine("Недостаточно средств, чтобы купить данную деталь");
                        isWork = false;
                    }
                }
                else
                {
                    Console.WriteLine("Неккоректный ввод");
                    isWork = false;
                }
            }
        }

        private void ShowBalance()
        {
            Console.WriteLine($"Ваши финансы: {_balance} руб.");
        }

        private void ShowRepairPrice()
        {
            int costRepair = 0;

            List<Detal> detals = _car.ReturnListBrokenDetals();
            for (int i = 0; i < detals.Count; i++)
            {
                costRepair += _costRepair + _warehouse.ReturnCostDetal(detals[i].Name);
            }

            Console.WriteLine($"\nСтоимость ремонта {costRepair} руб.\n");
        }
    }
}
