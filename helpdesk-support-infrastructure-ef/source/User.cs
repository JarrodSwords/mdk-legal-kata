namespace MdkLegal.HelpDesk.Support.Infrastructure.Ef;

public class User : Entity
{
    public User()
    {
    }

    public User(Domain.User source) : base(source.Id)
    {
        Email = source.Email;
        Name = source.Name;
    }

    public string Email { get; set; }
    public string Name { get; set; }

    public static implicit operator User(Domain.User source) => new(source);
}
