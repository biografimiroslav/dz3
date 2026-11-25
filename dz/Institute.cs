namespace dz;

public class Institute
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public Institute()
    {
    }

    public Institute(string name, string address)
    {
        Name = name;
        Address = address;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Інститут/кафедра: {Name}, адреса: {Address}");
    }
}
