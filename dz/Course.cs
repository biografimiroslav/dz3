namespace dz;

public class Course
{
    public string CourseId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int HoursPerWeek { get; set; }

    public Course()
    {
    }

    public Course(string courseId, string name, int hoursPerWeek)
    {
        CourseId = courseId;
        Name = name;
        HoursPerWeek = hoursPerWeek;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Курс #{CourseId}: {Name} ({HoursPerWeek} год./тижд.)");
    }
}
