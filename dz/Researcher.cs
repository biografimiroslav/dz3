namespace dz;

public class Researcher : Employee
{
    public string ResearchField { get; set; } = string.Empty;
    public List<Institute> Institutes { get; set; } = new();
    public List<ProjectParticipation> Projects { get; set; } = new();

    public Researcher() : base()
    {
    }

    public Researcher(string id, string name, string email, string researchField)
        : base(id, name, email)
    {
        ResearchField = researchField;
    }

    public void AddInstitute(Institute institute)
    {
        if (!Institutes.Contains(institute))
        {
            Institutes.Add(institute);
        }
    }

    public void AssignToProject(Project project, int hours)
    {
        Projects.Add(new ProjectParticipation(project, hours));
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"[Дослідник] {Name} (ID: {Id}, Email: {Email})");
        Console.WriteLine($"  Предмет дослідження: {ResearchField}");
        Console.WriteLine($"  Інститути роботи ({Institutes.Count}): {string.Join(", ", Institutes.Select(i => i.Name))}");
        Console.WriteLine("  Проекти:");
        foreach (var p in Projects)
        {
            Console.WriteLine($"    - {p.Project.Name} ({p.Hours} год.)");
        }
    }
}
