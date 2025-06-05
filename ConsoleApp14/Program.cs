using System;
using System.Globalization;

class Program
{
    const string DataFile = "products2.txt";
    const string ExpiredReportFile = "expired_products2_report.txt";
    
    static List<Product2> products = new List<Product2>();
    
    static void Main()
    {
        while (true)
        { 
            Console.WriteLine("1: Базовый уровень");    
            Console.WriteLine("2: Средний уровень");
            Console.WriteLine("3: Высокий уровень");
            Console.Write("Введите номер задания - ");
            
            string input = Console.ReadLine();
            Console.Clear();
            switch (input)
            {
                case "1":
                    Z1();
                    break;
                case "2":
                    Z2();
                    break;
                case "3":
                    Z3();
                    break;
            }
        }
        
    }

    static void Z1()
    {
       try
       {
            
           Console.Write("Введите дату в формате ДДММГГГГ: ");
           string inputDate = Console.ReadLine();

           DateTime date = DateTime.ParseExact(inputDate, "ddMMyyyy", CultureInfo.InvariantCulture);
           Console.WriteLine("Введённое значение является корректной датой: " + date.ToString("dd.MM.yyyy"));
       }
       catch (FormatException)
       {
           Console.WriteLine("Ошибка: введённое значение не является корректной датой.");
       }

       Console.WriteLine("\n--- Расчёт времени проведения процедур ---");

       try
       {
           Console.Write("Введите время первой процедуры (ЧЧ:ММ:СС): ");
           string firstTimeInput = Console.ReadLine();

           Console.Write("Введите время следующей процедуры (ЧЧ:ММ:СС): ");
           string secondTimeInput = Console.ReadLine();

           TimeSpan firstTime = TimeSpan.Parse(firstTimeInput);
           TimeSpan secondTime = TimeSpan.Parse(secondTimeInput);

           Console.Write("Введите общее количество процедур: ");
           int count = int.Parse(Console.ReadLine());

           if (count <= 0)
           {
               Console.WriteLine("Ошибка: количество процедур должно быть больше нуля.");
               return;
           }

           TimeSpan interval = secondTime - firstTime;

           if (interval <= TimeSpan.Zero)
           {
               Console.WriteLine("Ошибка: время следующей процедуры должно быть позже первой.");
               return;
           }

           
           DateTime startDateTime = DateTime.Today.Add(firstTime);

           Console.WriteLine("\nНазначенное время процедур:");
           for (int i = 0; i < count; i++)
           {
               DateTime procedureTime = startDateTime + interval * i;
               Console.WriteLine($"{i + 1}-я процедура: {procedureTime:HH:mm:ss}");
           }
       }
       catch (FormatException)
       {
           Console.WriteLine("Ошибка: введённые значения времени или количества процедур имеют неверный формат.");
       }
       catch (Exception ex)
       {
           Console.WriteLine("Произошла непредвиденная ошибка: " + ex.Message);
       }
    }

    static void Z2()
    {
         List<Product> products = new List<Product>();

        Console.Write("Введите количество товаров: ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Ошибка: введите корректное число товаров.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"\n--- Ввод информации о товаре #{i + 1} ---");

            Product p = new Product();

            Console.Write("Наименование товара: ");
            p.Name = Console.ReadLine();

            Console.Write("Фирма-производитель: ");
            p.Manufacturer = Console.ReadLine();

            Console.Write("Дата производства (ДДММГГГГ): ");
            try
            {
                p.ProductionDate = DateTime.ParseExact(Console.ReadLine(), "ddMMyyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
                Console.WriteLine("Ошибка: некорректный формат даты.");
                return;
            }

            Console.Write("Срок годности (в сутках): ");
            if (!int.TryParse(Console.ReadLine(), out p.ShelfLifeDays) || p.ShelfLifeDays < 0)
            {
                Console.WriteLine("Ошибка: некорректный срок годности.");
                return;
            }

            Console.Write("Цена (в рублях): ");
            if (!decimal.TryParse(Console.ReadLine(), out p.Price) || p.Price < 0)
            {
                Console.WriteLine("Ошибка: некорректная цена.");
                return;
            }

            products.Add(p);
        }

        Console.WriteLine("\n=== Список товаров с датой окончания срока годности ===");
        foreach (var product in products)
        {
            product.PrintInfo();
        }

        Console.Write("\nВведите номер месяца (1-12) для поиска по дате производства: ");
        if (!int.TryParse(Console.ReadLine(), out int searchMonth) || searchMonth < 1 || searchMonth > 12)
        {
            Console.WriteLine("Ошибка: введите корректный номер месяца.");
            return;
        }

        Console.WriteLine($"\n=== Товары, произведённые в месяце №{searchMonth} ===");
        bool found = false;
        foreach (var product in products)
        {
            if (product.ProductionDate.Month == searchMonth)
            {
                product.PrintInfo();
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("Нет товаров, произведённых в этом месяце.");
        }
    }

    static void Z3()
    {
        if (File.Exists(DataFile))
            products = File.ReadAllLines(DataFile).Select(Product2.FromString).ToList();

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Добавить новые товары");
            Console.WriteLine("2. Показать товары, срок которых истекает через 2 дня");
            Console.WriteLine("3. Найти самый свежий товар по названию");
            Console.WriteLine("4. Сформировать отчёт для защиты прав потребителей");
            Console.WriteLine("5. Выйти");

            Console.Write("Выберите пункт меню (1-5): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddProducts();
                    break;
                case "2":
                    ShowExpiringInTwoDays();
                    break;
                case "3":
                    ShowFreshestProduct();
                    break;
                case "4":
                    GenerateExpiredReport();
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Повторите.");
                    break;
            }
        }

        Console.WriteLine("Завершено.");
    }

    static void AddProducts()
    {
        Console.Write("Сколько товаров добавить? ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Ошибка: неверное количество.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"\n--- Товар #{i + 1} ---");
            Product2 p = new Product2();

            Console.Write("Наименование товара: ");
            p.Name = Console.ReadLine();

            Console.Write("Фирма-производитель: ");
            p.Manufacturer = Console.ReadLine();

            Console.Write("Дата производства (ДДММГГГГ): ");
            p.ProductionDate = DateTime.ParseExact(Console.ReadLine(), "ddMMyyyy", CultureInfo.InvariantCulture);

            Console.Write("Срок годности (в днях): ");
            p.ShelfLifeDays = int.Parse(Console.ReadLine());

            Console.Write("Цена: ");
            p.Price = decimal.Parse(Console.ReadLine());

            products.Add(p);
        }

        SaveProductsToFile();
        Console.WriteLine("Товары успешно добавлены и сохранены.");
    }

    static void ShowExpiringInTwoDays()
    {
        DateTime target = DateTime.Today.AddDays(2);
        var result = products.Where(p => p.ExpirationDate.Date == target).ToList();

        Console.WriteLine($"\nТовары, срок годности которых истекает через 2 дня ({target:dd.MM.yyyy}):");
        if (result.Count == 0)
            Console.WriteLine("Таких товаров нет.");
        else
            result.ForEach(p => Console.WriteLine(p.ToReadableString()));
        Console.WriteLine($"Всего: {result.Count}");
    }

    static void ShowFreshestProduct()
    {
        Console.Write("\nВведите наименование товара: ");
        string name = Console.ReadLine();

        var found = products.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();

        if (found.Count == 0)
        {
            Console.WriteLine("Такой товар не найден.");
            return;
        }

        var freshest = found.OrderByDescending(p => p.ProductionDate).First();
        Console.WriteLine("Самый свежий товар:");
        Console.WriteLine(freshest.ToReadableString());
    }

    static void GenerateExpiredReport()
    {
        DateTime today = DateTime.Today;
        var expired = products.Where(p => p.ExpirationDate < today).ToList();

        using (StreamWriter sw = new StreamWriter(ExpiredReportFile))
        {
            sw.WriteLine($"Отчёт о просроченных товарах на {today:dd.MM.yyyy}:\n");
            foreach (var p in expired)
                sw.WriteLine(p.ToReadableString());
            sw.WriteLine($"\nВсего просроченных: {expired.Count}");
        }

        Console.WriteLine($"Отчёт сохранён в файл: {ExpiredReportFile}");
    }

    static void SaveProductsToFile()
    {
        File.WriteAllLines(DataFile, products.Select(p => p.ToString()));
    }
}
