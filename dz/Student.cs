namespace dz;

public class Student : Person
{
    public Faculty? Faculty { get; set; }
    public List<Course> Courses { get; set; } = new();

    public Student() : base()
    {
    }

    public Student(string id, string name, string email, Faculty? faculty = null)
        : base(id, name, email)
    {
        Faculty = faculty;
    }

    public void EnrollInCourse(Course course)
    {
        if (!Courses.Contains(course))
        {
            Courses.Add(course);
        }
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"[Студент] {Name} (ID: {Id}, Email: {Email})");
        Console.WriteLine($"  Факультет: {Faculty?.Name ?? "Не призначено"}");
        Console.WriteLine("  Вивчає предмети:");
        foreach (var c in Courses)
        {
            Console.WriteLine($"    - #{c.CourseId} {c.Name} ({c.HoursPerWeek} год./тижд.)");
        }
    }
}
