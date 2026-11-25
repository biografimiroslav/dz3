# Завдання 1: UML-діаграма класів та об'єктна модель "Університет"

## 1. Обґрунтування абстрактних класів

У даній предметній області визначено два класи, які доцільно зробити **абстрактними (`abstract`)**:

1. **`Person` (Людина / Особа)** — базовий абстрактний клас для всіх фізичних осіб в університеті (`Employee` та `Student`). В реальній системі не існує просто "особи" без певної ролі — кожен є або співробітником, або студентом. Клас містить спільні поля: `Id`, `Name`, `Email` та абстрактний метод `DisplayInfo()`.
2. **`Employee` (Працівник)** — базовий абстрактний клас для всього персоналу університету. Співробітник завжди належить до конкретної посадової категорії: дослідник/пошукувач (`Researcher`), адміністратор (`Administrator`) або декан (`Dean`).

---

## 2. UML-діаграма класів (Mermaid)

```mermaid
classDiagram
    direction TB

    class Person {
        <<abstract>>
        +string Id
        +string Name
        +string Email
        +DisplayInfo()* void
    }

    class Employee {
        <<abstract>>
    }

    class Administrator {
        +string Department
        +DisplayInfo() void
    }

    class Dean {
        +DisplayInfo() void
    }

    class Researcher {
        +string ResearchField
        +List~Institute~ Institutes
        +List~ProjectParticipation~ Projects
        +AddInstitute(Institute institute) void
        +AssignToProject(Project project, int hours) void
        +DisplayInfo() void
    }

    class Lecturer {
        +List~Course~ CoursesTaught
        +AddCourse(Course course) void
        +DisplayInfo() void
    }

    class Student {
        +Faculty Faculty
        +List~Course~ Courses
        +EnrollInCourse(Course course) void
        +DisplayInfo() void
    }

    class University {
        +string Name
        +List~Faculty~ Faculties
        +AddFaculty(Faculty faculty) void
        +DisplayStructure() void
    }

    class Faculty {
        +string Name
        +Dean Dean
        +List~Institute~ Institutes
        +List~Student~ Students
        +AddInstitute(Institute institute) void
        +AddStudent(Student student) void
        +DisplayInfo() void
    }

    class Institute {
        +string Name
        +string Address
        +DisplayInfo() void
    }

    class Course {
        +string CourseId
        +string Name
        +int HoursPerWeek
        +DisplayInfo() void
    }

    class Project {
        +string Name
        +DateTime StartDate
        +DateTime EndDate
        +DisplayInfo() void
    }

    class ProjectParticipation {
        +Project Project
        +int Hours
    }

    %% Наслідування
    Person <|-- Employee
    Person <|-- Student
    Employee <|-- Administrator
    Employee <|-- Dean
    Employee <|-- Researcher
    Researcher <|-- Lecturer

    %% Зв'язки володіння та асоціації
    University "1" *-- "*" Faculty : складається з
    Faculty "1" *-- "*" Institute : складається з
    Faculty "1" o-- "1" Dean : очолює
    Faculty "1" o-- "*" Student : навчаються

    Researcher "*" o-- "*" Institute : працює в
    Researcher "1" *-- "*" ProjectParticipation : бере участь
    ProjectParticipation "*" --> "1" Project : посилається

    Lecturer "1" o-- "*" Course : викладає
    Student "*" o-- "*" Course : вивчає
```

---

## 3. PlantUML код діаграми (для PlantUML / draw.io)

```plantuml
@startuml
skinparam classAttributeIconSize 0

abstract class Person {
  +Id: string
  +Name: string
  +Email: string
  +{abstract} DisplayInfo(): void
}

abstract class Employee {
}

class Administrator {
  +Department: string
  +DisplayInfo(): void
}

class Dean {
  +DisplayInfo(): void
}

class Researcher {
  +ResearchField: string
  +Institutes: List<Institute>
  +Projects: List<ProjectParticipation>
  +AddInstitute(Institute): void
  +AssignToProject(Project, int): void
  +DisplayInfo(): void
}

class Lecturer {
  +CoursesTaught: List<Course>
  +AddCourse(Course): void
  +DisplayInfo(): void
}

class Student {
  +Faculty: Faculty
  +Courses: List<Course>
  +EnrollInCourse(Course): void
  +DisplayInfo(): void
}

class University {
  +Name: string
  +Faculties: List<Faculty>
  +AddFaculty(Faculty): void
  +DisplayStructure(): void
}

class Faculty {
  +Name: string
  +Dean: Dean
  +Institutes: List<Institute>
  +Students: List<Student>
  +AddInstitute(Institute): void
  +AddStudent(Student): void
  +DisplayInfo(): void
}

class Institute {
  +Name: string
  +Address: string
  +DisplayInfo(): void
}

class Course {
  +CourseId: string
  +Name: string
  +HoursPerWeek: int
  +DisplayInfo(): void
}

class Project {
  +Name: string
  +StartDate: DateTime
  +EndDate: DateTime
  +DisplayInfo(): void
}

class ProjectParticipation {
  +Project: Project
  +Hours: int
}

Person <|-- Employee
Person <|-- Student
Employee <|-- Administrator
Employee <|-- Dean
Employee <|-- Researcher
Researcher <|-- Lecturer

University *-- Faculty
Faculty *-- Institute
Faculty o-- Dean
Faculty o-- Student

Researcher o-- Institute
Researcher *-- ProjectParticipation
ProjectParticipation --> Project

Lecturer o-- Course
Student o-- Course

@enduml
```

---

## 4. Опис ієрархії та зв'язків між класами

- **Наслідування (Inheritance):**
  - `Person` -> `Employee`, `Student`
  - `Employee` -> `Administrator`, `Dean`, `Researcher`
  - `Researcher` -> `Lecturer` (лектор є дослідником, який додатково викладає курси)
- **Композиція (Composition):**
  - `University` містить `Faculty` (факультет не існує окремо від університету).
  - `Faculty` містить `Institute` (кафедри/інститути належать факультету).
  - `Researcher` володіє записами участі в проектах `ProjectParticipation`.
- **Агрегація / Асоціація (Aggregation / Association):**
  - `Faculty` асоціюється з `Dean` (декан призначається на факультет).
  - `Researcher` асоціюється з `Institute` (пошукувач може працювати в кількох інститутах).
  - `Lecturer` веде перелік курсів `Course`.
  - `Student` вивчає перелік курсів `Course` та прикріплений до `Faculty`.
