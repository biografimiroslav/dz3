using System.Text.Encodings.Web;
using System.Text.Json;

namespace dz;

public delegate void NotificationHandler(string message);
public delegate void ItemSoldHandler(MarketItem item, int quantity, decimal totalSum);

public class Supermarket
{
    private readonly List<MarketItem> _items = new();
    private int _receiptCounter = 1;

    public event NotificationHandler? NotificationReceived;
    public event ItemSoldHandler? ItemSold;
    public event Action<MarketItem>? LowStockWarning;

    public IReadOnlyList<MarketItem> Items => _items.AsReadOnly();

    public MarketItem? FindByName(string name)
    {
        return _items.FirstOrDefault(x => x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public List<MarketItem> Search(string query)
    {
        string term = query.Trim();
        return _items
            .Where(x => x.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        x.Category.Contains(term, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public bool AddItem(MarketItem item)
    {
        var existing = FindByName(item.Name);
        if (existing != null)
        {
            NotificationReceived?.Invoke($"Товар '{item.Name}' вже існує в базі (поточна кількість: {existing.Quantity}).");
            return false;
        }

        _items.Add(item);
        NotificationReceived?.Invoke($"Товар '{item.Name}' успішно додано до бази.");
        return true;
    }

    public void UpdateExistingQuantity(string name, int addedQuantity, decimal? newPrice = null)
    {
        var existing = FindByName(name);
        if (existing == null)
        {
            NotificationReceived?.Invoke($"Товар '{name}' не знайдено для оновлення.");
            return;
        }

        existing.Quantity += addedQuantity;
        if (newPrice.HasValue && newPrice.Value > 0)
        {
            existing.Price = newPrice.Value;
        }

        NotificationReceived?.Invoke($"Оновлено залишок для '{existing.Name}': тепер {existing.Quantity} шт. (Ціна: {existing.Price:F2} грн).");
    }

    public bool RemoveItem(string name)
    {
        var item = FindByName(name);
        if (item == null)
        {
            NotificationReceived?.Invoke($"Товар '{name}' не знайдено.");
            return false;
        }

        _items.Remove(item);
        NotificationReceived?.Invoke($"Товар '{item.Name}' видалено з бази.");
        return true;
    }

    public Receipt? Sell(string name, int quantity)
    {
        var item = FindByName(name);
        if (item == null)
        {
            NotificationReceived?.Invoke($"Помилка продажу: товар '{name}' не знайдено.");
            return null;
        }

        if (quantity <= 0)
        {
            NotificationReceived?.Invoke("Помилка продажу: кількість має бути більшою за нуль.");
            return null;
        }

        if (item.Quantity < quantity)
        {
            NotificationReceived?.Invoke($"Помилка продажу: недостатньо товару '{item.Name}' на складі (в наявності: {item.Quantity} шт.).");
            return null;
        }

        item.Quantity -= quantity;
        decimal sum = item.Price * quantity;

        ItemSold?.Invoke(item, quantity, sum);

        if (item.Quantity <= 3)
        {
            LowStockWarning?.Invoke(item);
        }

        var receipt = new Receipt
        {
            Number = _receiptCounter++,
            Date = DateTime.Now,
            Items = new List<ReceiptItem>
            {
                new ReceiptItem
                {
                    Name = item.Name,
                    UnitPrice = item.Price,
                    Quantity = quantity
                }
            }
        };

        return receipt;
    }

    public Dictionary<string, List<MarketItem>> GetItemsGroupedByCategory()
    {
        return _items
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Name)
            .GroupBy(x => x.Category, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);
    }

    public void SaveToFile(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        string json = JsonSerializer.Serialize(_items, options);
        File.WriteAllText(filePath, json);
        NotificationReceived?.Invoke($"Базу збережено у файл '{filePath}' ({_items.Count} записів).");
    }

    public void LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            NotificationReceived?.Invoke($"Файл '{filePath}' не знайдено.");
            return;
        }

        string json = File.ReadAllText(filePath);
        var loaded = JsonSerializer.Deserialize<List<MarketItem>>(json);

        if (loaded != null)
        {
            _items.Clear();
            _items.AddRange(loaded);
            NotificationReceived?.Invoke($"Завантажено {_items.Count} позицій з файлу '{filePath}'.");
        }
    }
}
