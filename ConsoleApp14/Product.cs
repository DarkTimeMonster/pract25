using System;
using System.Collections.Generic;
using System.Globalization;

struct Product
{
    public string Name;
    public string Manufacturer;
    public DateTime ProductionDate;
    public int ShelfLifeDays;
    public decimal Price;

    // Метод для получения даты окончания срока годности
    public DateTime GetExpirationDate()
    {
        return ProductionDate.AddDays(ShelfLifeDays);
    }

    // Метод для вывода информации
    public void PrintInfo()
    {
        Console.WriteLine($"Товар: {Name}");
        Console.WriteLine($"Производитель: {Manufacturer}");
        Console.WriteLine($"Дата производства: {ProductionDate:dd.MM.yyyy}");
        Console.WriteLine($"Срок годности: {ShelfLifeDays} дней");
        Console.WriteLine($"Дата окончания срока: {GetExpirationDate():dd.MM.yyyy}");
        Console.WriteLine($"Цена: {Price} руб.");
        Console.WriteLine("-----------------------------");
    }
}
