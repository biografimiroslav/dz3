using System.Text;

namespace dz;

internal class Program
{
    private static Dialog? _dialog;

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        bool running = true;
        while (running)
        {
            Console.WriteLine("=============================================");
            Console.WriteLine(" ДЕМОНСТРАЦІЯ ПАТЕРНУ \"ФАБРИЧНИЙ МЕТОД\"");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. Створити діалогове вікно Windows");
            Console.WriteLine("2. Створити діалогове вікно Web (HTML)");
            Console.WriteLine("3. Запустити обидві платформи для порівняння");
            Console.WriteLine("0. Вихід");
            Console.WriteLine("=============================================");
            Console.Write("Оберіть дію: ");

            string choice = Console.ReadLine()?.Trim() ?? "";
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ConfigurePlatform("windows");
                    RunBusinessLogic();
                    break;
                case "2":
                    ConfigurePlatform("web");
                    RunBusinessLogic();
                    break;
                case "3":
                    Console.WriteLine(">>> Тестування під Windows:");
                    ConfigurePlatform("windows");
                    RunBusinessLogic();

                    Console.WriteLine(">>> Тестування під Web:");
                    ConfigurePlatform("web");
                    RunBusinessLogic();
                    break;
                case "0":
                    running = false;
                    Console.WriteLine("Роботу завершено.");
                    break;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.\n");
                    break;
            }

            if (running)
            {
                Console.WriteLine("Натисніть Enter для продовження...");
                Console.ReadLine();
                try { Console.Clear(); } catch { }
            }
        }
    }

    private static void ConfigurePlatform(string platform)
    {
        if (platform.Equals("windows", StringComparison.OrdinalIgnoreCase))
        {
            _dialog = new WindowsDialog();
        }
        else if (platform.Equals("web", StringComparison.OrdinalIgnoreCase))
        {
            _dialog = new WebDialog();
        }
        else
        {
            throw new Exception("Невідома операційна система або середовище.");
        }
    }

    private static void RunBusinessLogic()
    {
        if (_dialog == null)
        {
            Console.WriteLine("Помилка: діалог не ініціалізовано.");
            return;
        }

        _dialog.Render();
    }
}
