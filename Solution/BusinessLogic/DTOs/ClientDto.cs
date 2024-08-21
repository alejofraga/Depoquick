namespace BusinessLogic;

public class ClientDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    
    public ClientDto(string name, string email, string password, bool isAdmin)
    {
        Name = name;
        Email = email;
        Password = password;
        IsAdmin = isAdmin;
    }
    
    public ClientDto()
    {
        
    }
}