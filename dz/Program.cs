using System.Globalization;
using System.Text;

namespace dz;

internal class Program
{
    private static readonly Supermarket Supermarket = new();
    private const string DefaultFileName = "supermarket_data.json";

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Supermarket.NotificationReceived += OnNotification;
        Supermarket.ItemSold += OnItemSold;
        Supermarket.LowStockWarning += OnLowStockWarning;

        if (File.Exists(DefaultFileName))
        {
            Supermarket.LoadFromFile(DefaultFileName);
        }

        bool running = true;
        while (running)
        {
            PrintMenu();
            string choice = Console.ReadLine()?.Trim() ?? string.Empty;

            Console.WriteLine();
            switch (choice)
            {
                case "1":
                    AddNewItemMenu();
                    break;
                case "2":
                    ShowAllGroupedByCategory();
                    break;
                case "3":
                    SearchItemMenu();
                    break;
                case "4":
                    SellItemMenu();
                    break;
                case "5":
                    DeleteItemMenu();
                    break;
                case "6":
                    SaveToFileMenu();
                    break;
                case "7":
                    LoadFromFileMenu();
                    break;
                case "8":
                    SeedInitialData();
                    break;
                case "0":
                    running = false;
                    Console.WriteLine("Завершення роботи програми.");
                    break;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }

            if (running)
            {
                Console.WriteLine("\nНатисніть Enter для продовження...");
                Console.ReadLine();
            }
        }
    }

    private static void PrintMenu()
    {
        try
        {
            Console.Clear();
        }
        catch
        {
        }
        Console.WriteLine("========================================");
        Console.WriteLine("      СИСТЕМА КЕРУВАННЯ СУПЕРМАРКЕТОМ   ");
        Console.WriteLine("========================================");
        Console.WriteLine("1. Додати товар / продукт");
        Console.WriteLine("2. Переглянути товари за категоріями");
        Console.WriteLine("3. Пошук товару");
        Console.WriteLine("4. Продаж товару (друк чеку)");
        Console.WriteLine("5. Видалити товар з бази");
        Console.WriteLine("6. Зберегти базу у файл");
        Console.WriteLine("7. Завантажити базу з файлу");
        Console.WriteLine("8. Заповнити тестовими даними");
        Console.WriteLine("0. Вихід");
        Console.WriteLine("========================================");
        Console.Write("Оберіть дію: ");
    }

    private static void AddNewItemMenu()
    {
        Console.WriteLine("--- Додавання товару ---");
        Console.WriteLine("1. Продукт харчування (з терміном придатності)");
        Console.WriteLine("2. Непродовольчий товар");
        Console.Write("Тип позиції (1 або 2): ");
        string typeChoice = Console.ReadLine()?.Trim() ?? "1";

        string name = ReadString("Введіть назву: ");
        var existing = Supermarket.FindByName(name);

        if (existing != null)
        {
            Console.WriteLine($"\n[УВАГА] Товар '{existing.Name}' вже є в базі!");
            Console.WriteLine($"Поточна кількість: {existing.Quantity} шт., Ціна: {existing.Price:F2} грн.");
            Console.Write("Бажаєте поповнити кількість цього товару? (т/н): ");
            string answer = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (answer == "т" || answer == "y" || answer == "так")
            {
                int addQty = ReadInt("Введіть кількість для додавання: ", min: 1);
                Console.Write("Бажаєте змінити ціну? (т/н): ");
                string priceAnswer = Console.ReadLine()?.Trim().ToLower() ?? "";
                decimal? newPrice = null;
                if (priceAnswer == "т" || priceAnswer == "y" || priceAnswer == "так")
                {
                    newPrice = ReadDecimal("Нова ціна: ", min: 0.01m);
                }

                Supermarket.UpdateExistingQuantity(existing.Name, addQty, newPrice);
            }
            return;
        }

        string category = ReadString("Введіть категорію (напр. Молочні, Хлібобулочні, Побутова хімія): ");
        decimal price = ReadDecimal("Введіть ціну (грн): ", min: 0.01m);
        int quantity = ReadInt("Введіть початкову кількість: ", min: 0);

        if (typeChoice == "2")
        {
            string manufacturer = ReadString("Введіть виробника: ");
            int warranty = ReadInt("Введіть термін гарантії (міс.): ", min: 0);
            var nonFood = new NonFoodProduct(name, category, price, quantity, manufacturer, warranty);
            Supermarket.AddItem(nonFood);
        }
        else
        {
            DateTime expDate = ReadDate("Введіть термін придатності (рррр-мм-дд): ");
            var food = new FoodProduct(name, category, price, quantity, expDate);
            Supermarket.AddItem(food);
        }
    }

    private static void ShowAllGroupedByCategory()
    {
        Console.WriteLine("--- Список товарів за категоріями ---");
        var groups = Supermarket.GetItemsGroupedByCategory();

        if (groups.Count == 0)
        {
            Console.WriteLine("База супермаркету порожня.");
            return;
        }

        foreach (var group in groups)
        {
            Console.WriteLine($"\nКАТЕГОРІЯ: [{group.Key.ToUpper()}] (Позицій: {group.Value.Count})");
            Console.WriteLine(new string('-', 50));
            foreach (var item in group.Value)
            {
                Console.WriteLine(item.GetShortInfo());
            }
        }
    }

    private static void SearchItemMenu()
    {
        Console.WriteLine("--- Пошук товару ---");
        string query = ReadString("Введіть назву або категорію для пошуку: ");
        var results = Supermarket.Search(query);

        if (results.Count == 0)
        {
            Console.WriteLine("За вашим запитом нічого не знайдено.");
            return;
        }

        Console.WriteLine($"\nЗнайдено позицій: {results.Count}\n");
        foreach (var item in results)
        {
            item.DisplayInfo();
            Console.WriteLine(new string('-', 40));
        }
    }

    private static void SellItemMenu()
    {
        Console.WriteLine("--- Оформлення продажу ---");
        string name = ReadString("Введіть назву товару: ");

        var item = Supermarket.FindByName(name);
        if (item == null)
        {
            Console.WriteLine($"Товар '{name}' відсутній в базі.");
            return;
        }

        Console.WriteLine($"В наявності: {item.Quantity} шт. за ціною {item.Price:F2} грн.");
        if (item.Quantity == 0)
        {
            Console.WriteLine("Цей товар закінчився на складі.");
            return;
        }

        int qty = ReadInt("Введіть кількість для продажу: ", min: 1);
        var receipt = Supermarket.Sell(name, qty);
        if (receipt != null)
        {
            receipt.Print();
        }
    }

    private static void DeleteItemMenu()
    {
        Console.WriteLine("--- Видалення товару ---");
        string name = ReadString("Введіть назву товару для видалення: ");

        var item = Supermarket.FindByName(name);
        if (item == null)
        {
            Console.WriteLine($"Товар '{name}' не знайдено.");
            return;
        }

        Console.Write($"Ви дійсно бажаєте видалити '{item.Name}'? (т/н): ");
        string answer = Console.ReadLine()?.Trim().ToLower() ?? "";
        if (answer == "т" || answer == "y" || answer == "так")
        {
            Supermarket.RemoveItem(name);
        }
        else
        {
            Console.WriteLine("Видалення скасовано.");
        }
    }

    private static void SaveToFileMenu()
    {
        Console.Write($"Введіть назву файлу для збереження (Enter для '{DefaultFileName}'): ");
        string fileName = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrEmpty(fileName))
        {
            fileName = DefaultFileName;
        }

        try
        {
            Supermarket.SaveToFile(fileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка збереження файлу: {ex.Message}");
        }
    }

    private static void LoadFromFileMenu()
    {
        Console.Write($"Введіть назву файлу для завантаження (Enter для '{DefaultFileName}'): ");
        string fileName = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrEmpty(fileName))
        {
            fileName = DefaultFileName;
        }

        try
        {
            Supermarket.LoadFromFile(fileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка завантаження файлу: {ex.Message}");
        }
    }

    private static void SeedInitialData()
    {
        Supermarket.AddItem(new FoodProduct("Молоко 'Яготинське' 2.6%", "Молочні продукти", 42.50m, 15, DateTime.Now.AddDays(7)));
        Supermarket.AddItem(new FoodProduct("Сир твердий 'Гауда'", "Молочні продукти", 89.00m, 8, DateTime.Now.AddDays(30)));
        Supermarket.AddItem(new FoodProduct("Батон 'Київський'", "Хлібобулочні", 24.00m, 20, DateTime.Now.AddDays(2)));
        Supermarket.AddItem(new FoodProduct("Круасан шоколадний", "Хлібобулочні", 32.00m, 12, DateTime.Now.AddDays(4)));
        Supermarket.AddItem(new NonFoodProduct("Порошок Tide Автомат 3кг", "Побутова хімія", 340.00m, 5, "Procter & Gamble", 24));
        Supermarket.AddItem(new NonFoodProduct("Засіб для посуду Fairy 500мл", "Побутова хімія", 65.50m, 14, "Procter & Gamble", 36));
        Supermarket.AddItem(new NonFoodProduct("Рушник кухонний вафельний", "Текстиль", 55.00m, 10, "Текстиль-Контакт", 12));

        Console.WriteLine("Тестові дані успішно додано!");
    }

    private static string ReadString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            Console.WriteLine("Поле не може бути порожнім.");
        }
    }

    private static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int result) && result >= min && result <= max)
            {
                return result;
            }
            Console.WriteLine($"Введіть коректне ціле число від {min} до {max}.");
        }
    }

    private static decimal ReadDecimal(string prompt, decimal min = 0)
    {
        while (true)
        {
            Console.Write(prompt);
            string? raw = Console.ReadLine()?.Replace(',', '.');
            if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result) && result >= min)
            {
                return result;
            }
            Console.WriteLine($"Введіть додатне число (не менше {min}).");
        }
    }

    private static DateTime ReadDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine()?.Trim();
            if (DateTime.TryParse(input, out DateTime date))
            {
                return date;
            }
            Console.WriteLine("Невірний формат дати. Приклад: 2026-12-31 або 31.12.2026");
        }
    }

    private static void OnNotification(string message)
    {
        Console.WriteLine($"[ПОВІДОМЛЕННЯ]: {message}");
    }

    private static void OnItemSold(MarketItem item, int quantity, decimal totalSum)
    {
        Console.WriteLine($"[ПОДІЯ ПРОДАЖУ]: Продано '{item.Name}' x{quantity} на суму {totalSum:F2} грн.");
    }

    private static void OnLowStockWarning(MarketItem item)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[ПОПЕРЕДЖЕННЯ]: Закінчується товар '{item.Name}'! Залишилось всього {item.Quantity} шт.");
        Console.ResetColor();
    }
}
