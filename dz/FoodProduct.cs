namespace dz;

public class FoodProduct : MarketItem
{
    public DateTime ExpirationDate { get; set; }

    public FoodProduct() : base()
    {
    }

    public FoodProduct(string name, string category, decimal price, int quantity, DateTime expirationDate)
        : base(name, category, price, quantity)
    {
        ExpirationDate = expirationDate;
    }

    public bool IsExpired => DateTime.Now > ExpirationDate;

    public override void DisplayInfo()
    {
        string status = IsExpired ? "ТЕРМІН МИНУВ!" : "Придатний";
        Console.WriteLine($"[Продукт харчування] {Name}");
        Console.WriteLine($"  Категорія: {Category}");
        Console.WriteLine($"  Ціна: {Price:F2} грн | Кількість: {Quantity} шт.");
        Console.WriteLine($"  Термін споживання до: {ExpirationDate:yyyy-MM-dd} ({status})");
        Console.WriteLine($"  Загальна вартість залишку: {GetTotalPrice():F2} грн");
    }

    public override string GetShortInfo()
    {
        return $"{base.GetShortInfo()} | Вжити до: {ExpirationDate:yyyy-MM-dd}";
    }
}
