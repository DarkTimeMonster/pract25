using System.Globalization;

    struct Product2
    {
        public string Name;
        public string Manufacturer;
        public DateTime ProductionDate;
        public int ShelfLifeDays;
        public decimal Price;

        public DateTime ExpirationDate => ProductionDate.AddDays(ShelfLifeDays);

        public override string ToString()
        {
            return $"{Name}|{Manufacturer}|{ProductionDate:ddMMyyyy}|{ShelfLifeDays}|{Price}";
        }

        public static Product2 FromString(string line)
        {
            string[] parts = line.Split('|');
            return new Product2
            {
                Name = parts[0],
                Manufacturer = parts[1],
                ProductionDate = DateTime.ParseExact(parts[2], "ddMMyyyy", CultureInfo.InvariantCulture),
                ShelfLifeDays = int.Parse(parts[3]),
                Price = decimal.Parse(parts[4])
            };
        }

        public string ToReadableString()
        {
            return $"Товар: {Name}, Производитель: {Manufacturer}, Дата производства: {ProductionDate:dd.MM.yyyy}, Срок годности: {ShelfLifeDays} дней, Дата окончания: {ExpirationDate:dd.MM.yyyy}, Цена: {Price} руб.";
        }
    }