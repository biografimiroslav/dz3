namespace dz;

public abstract class Person
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    protected Person()
    {
    }

    protected Person(string id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public abstract void DisplayInfo();
}
