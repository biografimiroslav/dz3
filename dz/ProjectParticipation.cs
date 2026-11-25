namespace dz;

public class ProjectParticipation
{
    public Project Project { get; set; } = null!;
    public int Hours { get; set; }

    public ProjectParticipation()
    {
    }

    public ProjectParticipation(Project project, int hours)
    {
        Project = project;
        Hours = hours;
    }
}
