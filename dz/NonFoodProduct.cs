namespace dz;

public class NonFoodProduct : MarketItem
{
    public string Manufacturer { get; set; } = string.Empty;
    public int WarrantyMonths { get; set; }

    public NonFoodProduct() : base()
    {
    }

    public NonFoodProduct(string name, string category, decimal price, int quantity, string manufacturer, int warrantyMonths)
        : base(name, category, price, quantity)
    {
        Manufacturer = manufacturer;
        WarrantyMonths = warrantyMonths;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"[Непродовольчий товар] {Name}");
        Console.WriteLine($"  Категорія: {Category}");
        Console.WriteLine($"  Виробник: {Manufacturer}");
        Console.WriteLine($"  Ціна: {Price:F2} грн | Кількість: {Quantity} шт.");
        Console.WriteLine($"  Гарантія: {WarrantyMonths} міс.");
        Console.WriteLine($"  Загальна вартість залишку: {GetTotalPrice():F2} грн");
    }

    public override string GetShortInfo()
    {
        return $"{base.GetShortInfo()} | Виробник: {Manufacturer} | Гарантія: {WarrantyMonths} міс.";
    }
}
