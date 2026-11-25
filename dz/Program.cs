using System.Text;

namespace dz;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var university = new University("Національний університет");

        var deanCs = new Dean("EMP-001", "Коваленко Ігор Петрович", "kovalenko@univ.edu.ua");
        var facultyCs = new Faculty("Факультет комп'ютерних наук", deanCs);

        var instSoftware = new Institute("Інститут інженерії програмного забезпечення", "вул. Університетська, 1, корп. 3");
        var instAi = new Institute("Інститут штучного інтелекту", "вул. Наукова, 5, корп. 1");
        facultyCs.AddInstitute(instSoftware);
        facultyCs.AddInstitute(instAi);

        var admin = new Administrator("EMP-010", "Сидоренко Оксана Валеріївна", "admin.cs@univ.edu.ua", "Деканат ФКН");

        var courseOop = new Course("CS-101", "Об'єктно-орієнтоване програмування", 4);
        var courseAlgos = new Course("CS-102", "Алгоритми та структури даних", 3);
        var courseMl = new Course("CS-201", "Машинне навчання", 4);

        var projAi = new Project("Розпізнавання медичних зображень за допомогою ШІ", new DateTime(2026, 1, 10), new DateTime(2027, 6, 30));
        var projCyber = new Project("Безпека хмарних систем", new DateTime(2025, 9, 1), new DateTime(2026, 12, 31));

        var researcher = new Researcher("EMP-005", "Мельник Василь Васильович", "melnyk@univ.edu.ua", "Квантові обчислення");
        researcher.AddInstitute(instAi);
        researcher.AssignToProject(projCyber, 120);

        var lecturer = new Lecturer("EMP-003", "Дмитренко Сергій Миколайович", "dmytrenko@univ.edu.ua", "Глибинне навчання");
        lecturer.AddInstitute(instSoftware);
        lecturer.AddInstitute(instAi);
        lecturer.AssignToProject(projAi, 200);
        lecturer.AddCourse(courseOop);
        lecturer.AddCourse(courseMl);

        var student1 = new Student("STD-2024-01", "Шевченко Мирослав", "shevchenko.m@student.univ.edu.ua");
        student1.EnrollInCourse(courseOop);
        student1.EnrollInCourse(courseAlgos);
        facultyCs.AddStudent(student1);

        var student2 = new Student("STD-2024-02", "Бондаренко Олена", "bondarenko.o@student.univ.edu.ua");
        student2.EnrollInCourse(courseOop);
        student2.EnrollInCourse(courseMl);
        facultyCs.AddStudent(student2);

        university.AddFaculty(facultyCs);

        university.DisplayStructure();

        Console.WriteLine("\n--- Детальна інформація про персонал ---");
        admin.DisplayInfo();
        Console.WriteLine();
        researcher.DisplayInfo();
        Console.WriteLine();
        lecturer.DisplayInfo();

        Console.WriteLine("\n--- Детальна інформація про студентів ---");
        student1.DisplayInfo();
        Console.WriteLine();
        student2.DisplayInfo();
    }
}
