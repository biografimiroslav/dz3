namespace dz;

public class Project
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Project()
    {
    }

    public Project(string name, DateTime startDate, DateTime endDate)
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Проект '{Name}' ({StartDate:yyyy-MM-dd} - {EndDate:yyyy-MM-dd})");
    }
}
