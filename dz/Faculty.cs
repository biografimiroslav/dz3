namespace dz;

public class Faculty
{
    public string Name { get; set; } = string.Empty;
    public Dean? Dean { get; set; }
    public List<Institute> Institutes { get; set; } = new();
    public List<Student> Students { get; set; } = new();

    public Faculty()
    {
    }

    public Faculty(string name, Dean? dean = null)
    {
        Name = name;
        Dean = dean;
    }

    public void AddInstitute(Institute institute)
    {
        if (!Institutes.Contains(institute))
        {
            Institutes.Add(institute);
        }
    }

    public void AddStudent(Student student)
    {
        if (!Students.Contains(student))
        {
            Students.Add(student);
            student.Faculty = this;
        }
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"=== Факультет: {Name} ===");
        if (Dean != null)
        {
            Console.WriteLine($"Декан: {Dean.Name} ({Dean.Email})");
        }
        Console.WriteLine($"Кафедри/Інститути ({Institutes.Count}):");
        foreach (var inst in Institutes)
        {
            Console.WriteLine($"  - {inst.Name} ({inst.Address})");
        }
        Console.WriteLine($"Студенти ({Students.Count}):");
        foreach (var st in Students)
        {
            Console.WriteLine($"  - {st.Name} (ID: {st.Id})");
        }
    }
}
