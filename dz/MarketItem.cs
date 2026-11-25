using System.Text.Json.Serialization;

namespace dz;

[JsonDerivedType(typeof(FoodProduct), typeDiscriminator: "Food")]
[JsonDerivedType(typeof(NonFoodProduct), typeDiscriminator: "NonFood")]
public abstract class MarketItem : IMarketItem
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    protected MarketItem()
    {
    }

    protected MarketItem(string name, string category, decimal price, int quantity)
    {
        Name = name;
        Category = category;
        Price = price;
        Quantity = quantity;
    }

    public virtual decimal GetTotalPrice()
    {
        return Price * Quantity;
    }

    public abstract void DisplayInfo();

    public virtual string GetShortInfo()
    {
        return $"{Name} | Категорія: {Category} | Ціна: {Price:C2} | Кількість: {Quantity}";
    }
}
