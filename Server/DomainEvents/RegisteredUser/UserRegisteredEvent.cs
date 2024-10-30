namespace SycappsWeb.Server;

public class UserRegisteredEvent : DomainEvent
{
    public string Email { get; }
    
    public string Name { get; }

    public string ConfirmationLink { get; }

    public UserRegisteredEvent(string email, string name, string confirmationLink)    {
        Email = email;
        Name = name;
        ConfirmationLink = confirmationLink;
    }
}

