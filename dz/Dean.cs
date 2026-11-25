namespace dz;

public class Dean : Employee
{
    public Dean() : base()
    {
    }

    public Dean(string id, string name, string email)
        : base(id, name, email)
    {
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"[Декан] {Name} (ID: {Id}, Email: {Email})");
    }
}
