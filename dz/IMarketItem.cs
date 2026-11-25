namespace dz;

public interface IMarketItem
{
    string Name { get; set; }
    string Category { get; set; }
    decimal Price { get; set; }
    int Quantity { get; set; }

    void DisplayInfo();
    decimal GetTotalPrice();
}
