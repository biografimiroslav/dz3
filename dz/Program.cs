using System.Text;
using Microsoft.Data.Sqlite;

namespace dz;

internal class Program
{
    private const string DbPath = "../restaurant.db";

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        string targetDb = File.Exists(DbPath) ? DbPath : "restaurant.db";

        if (!File.Exists(targetDb))
        {
            Console.WriteLine("Створення та наповнення бази даних 'restaurant.db'...");
            InitializeDatabase(targetDb);
        }

        Console.WriteLine("================================================================================");
        Console.WriteLine("          СТРУКТУРА БАЗИ ДАНИХ СИСТЕМИ \"РЕСТОРАН\" (restaurant.db)");
        Console.WriteLine("================================================================================\n");

        using var connection = new SqliteConnection($"Data Source={targetDb}");
        connection.Open();

        var tables = GetTableNames(connection);

        foreach (var table in tables)
        {
            PrintTableStructure(connection, table);
            Console.WriteLine();
        }

        Console.WriteLine("================================================================================");
        Console.WriteLine("                ПРИКЛАД ЗАПИТУ: ЗАМОВЛЕННЯ ТА ЇХ СТАТУСИ");
        Console.WriteLine("================================================================================");
        PrintOrdersSummary(connection);
    }

    private static void InitializeDatabase(string dbPath)
    {
        string schemaFile = File.Exists("../schema.sql") ? "../schema.sql" : "schema.sql";
        string seedFile = File.Exists("../seed.sql") ? "../seed.sql" : "seed.sql";

        if (!File.Exists(schemaFile) || !File.Exists(seedFile))
        {
            Console.WriteLine("Файли schema.sql або seed.sql не знайдено!");
            return;
        }

        using var connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = File.ReadAllText(schemaFile);
            cmd.ExecuteNonQuery();
        }

        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = File.ReadAllText(seedFile);
            cmd.ExecuteNonQuery();
        }
    }

    private static List<string> GetTableNames(SqliteConnection connection)
    {
        var tables = new List<string>();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%' ORDER BY name;";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            tables.Add(reader.GetString(0));
        }
        return tables;
    }

    private static void PrintTableStructure(SqliteConnection connection, string tableName)
    {
        Console.WriteLine($"[ТАБЛИЦЯ: {tableName.ToUpper()}]");
        Console.WriteLine(new string('-', 85));
        Console.WriteLine("{0,-4} | {1,-20} | {2,-16} | {3,-10} | {4,-10} | {5,-12}", 
            "#", "Назва поля", "Тип даних", "Primary Key", "Not Null", "Default");
        Console.WriteLine(new string('-', 85));

        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"PRAGMA table_info({tableName});";
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int cid = reader.GetInt32(0) + 1;
            string name = reader.GetString(1);
            string type = reader.GetString(2);
            bool notNull = reader.GetInt32(3) == 1;
            string dflt = reader.IsDBNull(4) ? "NULL" : reader.GetString(4);
            bool pk = reader.GetInt32(5) == 1;

            Console.WriteLine("{0,-4} | {1,-20} | {2,-16} | {3,-10} | {4,-10} | {5,-12}",
                cid, name, type, pk ? "ТАК (PK)" : "НІ", notNull ? "ТАК" : "НІ", dflt);
        }

        PrintForeignKeys(connection, tableName);
    }

    private static void PrintForeignKeys(SqliteConnection connection, string tableName)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"PRAGMA foreign_key_list({tableName});";
        using var reader = cmd.ExecuteReader();

        bool hasFk = false;
        while (reader.Read())
        {
            if (!hasFk)
            {
                Console.WriteLine("Зовнішні ключі (Foreign Keys):");
                hasFk = true;
            }
            string from = reader.GetString(3);
            string toTable = reader.GetString(2);
            string toCol = reader.GetString(4);
            string onUpdate = reader.GetString(5);
            string onDelete = reader.GetString(6);

            Console.WriteLine($"  -> Поле '{from}' посилається на '{toTable}({toCol})' [ON DELETE {onDelete}, ON UPDATE {onUpdate}]");
        }

        if (!hasFk)
        {
            Console.WriteLine("Зовнішні ключі: відсутні");
        }
    }

    private static void PrintOrdersSummary(SqliteConnection connection)
    {
        string query = @"
            SELECT 
                o.order_id,
                IFNULL(c.full_name, 'Гість закладу') AS customer,
                o.table_number,
                o.order_date,
                o.total_price,
                o.status,
                o.payment_method
            FROM orders o
            LEFT JOIN customers c ON o.customer_id = c.customer_id
            ORDER BY o.order_id;";

        using var cmd = connection.CreateCommand();
        cmd.CommandText = query;
        using var reader = cmd.ExecuteReader();

        Console.WriteLine("{0,-4} | {1,-28} | {2,-6} | {3,-10} | {4,-12} | {5,-12}",
            "№", "Клієнт", "Стіл", "Сума (грн)", "Статус", "Оплата");
        Console.WriteLine(new string('-', 85));

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string customer = reader.GetString(1);
            int table = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
            decimal price = reader.GetDecimal(4);
            string status = reader.GetString(5);
            string payment = reader.GetString(6);

            Console.WriteLine("{0,-4} | {1,-28} | {2,-6} | {3,-10:F2} | {4,-12} | {5,-12}",
                id, customer, table == 0 ? "-" : table.ToString(), price, status, payment);
        }
        Console.WriteLine();
    }
}
