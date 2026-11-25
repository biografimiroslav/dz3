namespace dz;

public class Lecturer : Researcher
{
    public List<Course> CoursesTaught { get; set; } = new();

    public Lecturer() : base()
    {
    }

    public Lecturer(string id, string name, string email, string researchField)
        : base(id, name, email, researchField)
    {
    }

    public void AddCourse(Course course)
    {
        if (!CoursesTaught.Contains(course))
        {
            CoursesTaught.Add(course);
        }
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"[Лектор] {Name} (ID: {Id}, Email: {Email})");
        Console.WriteLine($"  Предмет дослідження: {ResearchField}");
        Console.WriteLine($"  Інститути роботи ({Institutes.Count}): {string.Join(", ", Institutes.Select(i => i.Name))}");
        Console.WriteLine("  Проекти:");
        foreach (var p in Projects)
        {
            Console.WriteLine($"    - {p.Project.Name} ({p.Hours} год.)");
        }
        Console.WriteLine("  Викладає курси:");
        foreach (var c in CoursesTaught)
        {
            Console.WriteLine($"    - #{c.CourseId} {c.Name} ({c.HoursPerWeek} год./тижд.)");
        }
    }
}
