namespace dz;

public abstract class Employee : Person
{
    protected Employee() : base()
    {
    }

    protected Employee(string id, string name, string email)
        : base(id, name, email)
    {
    }
}
