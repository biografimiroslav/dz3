namespace dz;

public class University
{
    public string Name { get; set; } = string.Empty;
    public List<Faculty> Faculties { get; set; } = new();

    public University()
    {
    }

    public University(string name)
    {
        Name = name;
    }

    public void AddFaculty(Faculty faculty)
    {
        if (!Faculties.Contains(faculty))
        {
            Faculties.Add(faculty);
        }
    }

    public void DisplayStructure()
    {
        Console.WriteLine($"****************************************");
        Console.WriteLine($"       УНІВЕРСИТЕТ: {Name}");
        Console.WriteLine($"****************************************\n");

        foreach (var faculty in Faculties)
        {
            faculty.DisplayInfo();
            Console.WriteLine();
        }
    }
}
