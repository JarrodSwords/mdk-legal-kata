namespace MdkLegal.HelpDesk.Support.Domain;

public class User(string email, string name) : Entity
{
    public string Email { get; } = email;
    public string Name { get; } = name;
}
