namespace dz;

public class ReceiptItem
{
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
}

public class Receipt
{
    public int Number { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public List<ReceiptItem> Items { get; set; } = new();
    public decimal TotalAmount => Items.Sum(x => x.TotalPrice);

    public void Print()
    {
        Console.WriteLine("\n================= ФІСКАЛЬНИЙ ЧЕК =================");
        Console.WriteLine($"Чек №: {Number}");
        Console.WriteLine($"Дата та час: {Date:dd.MM.yyyy HH:mm:ss}");
        Console.WriteLine($"Супермаркет \"Світанок\"");
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("{0,-20} {1,8} {2,8} {3,10}", "Назва", "К-сть", "Ціна", "Сума");
        Console.WriteLine("--------------------------------------------------");

        foreach (var item in Items)
        {
            string shortName = item.Name.Length > 19 ? item.Name.Substring(0, 16) + "..." : item.Name;
            Console.WriteLine("{0,-20} {1,8} {2,8:F2} {3,10:F2}", shortName, item.Quantity, item.UnitPrice, item.TotalPrice);
        }

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"ДО СПЛАТИ: {TotalAmount:F2} грн");
        Console.WriteLine("Дякуємо за покупку! Гарного дня!");
        Console.WriteLine("==================================================\n");
    }
}
