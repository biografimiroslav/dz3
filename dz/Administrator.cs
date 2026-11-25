namespace dz;

public class Administrator : Employee
{
    public string Department { get; set; } = string.Empty;

    public Administrator() : base()
    {
    }

    public Administrator(string id, string name, string email, string department = "")
        : base(id, name, email)
    {
        Department = department;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"[Адміністратор] {Name} (ID: {Id}, Email: {Email}, Відділ: {Department})");
    }
}
