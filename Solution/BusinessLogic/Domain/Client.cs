using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BusinessLogic.Domain;

public class Client
{
    private bool IsValidPassword(string password)
    {
        return PasswordIsMoreThan8Characters(password) &&
               PasswordHaveAtLeastOneSymbol(password) &&
               PasswordHaveAtLeastOneLowercaseLetter(password) &&
               PasswordHaveAtLeastOneUppercaseLetter(password) &&
               PasswordHaveAtLeastOneNumber(password);
    }

    private string? _password;

    public string? Password
    {
        get => _password;
        set
        {
            if (!IsValidPassword(value))
            {
                throw new ArgumentException(
                    "La contraseña debe cumplir: mínimo 8 caracteres, un símbolo, una letra minúscula, una letra mayúscula y un número");
            }

            _password = value;
        }
    }

    private string? _name;

    public string? Name
    {
        get => _name;
        set
        {
            if (!NameIsUpTo100Characters(value))
            {
                throw new ArgumentException("El largo del nombre no debe ser mayor a 100 caracteres");
            }

            _name = value;
        }
    }


    public bool NameIsUpTo100Characters(string name)
    {
        return name.Length <= 100;
    }

    public bool PasswordIsMoreThan8Characters(string password)
    {
        return password.Length >= 8;
    }

    public bool PasswordHaveAtLeastOneSymbol(string password)
    {
        char[] symbols = { '#', '@', '$', '.', ',', '%' };

        foreach (char symbol in symbols)
        {
            if (password.Contains(symbol))
            {
                return true;
            }
        }

        return false;
    }

    public bool PasswordHaveAtLeastOneLowercaseLetter(string password)
    {
        foreach (char letter in password)
        {
            if (char.IsLower(letter))
            {
                return true;
            }
        }

        return false;
    }

    public bool PasswordHaveAtLeastOneUppercaseLetter(string password)
    {
        foreach (var letter in password)
        {
            if (char.IsUpper(letter))
            {
                return true;
            }
        }

        return false;
    }

    public bool PasswordHaveAtLeastOneNumber(string password)
    {
        foreach (var letter in password)
        {
            if (char.IsDigit(letter))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasValidEmail(string email)
    {
        const string emailPattern = @"^(?!.*\.\.)(?!.*\.$)[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        const int suggestedEmailLength = 254;

        return email.Contains('@')
               && email.Length <= suggestedEmailLength
               && Regex.IsMatch(email, emailPattern);
    }

    public bool IsAdmin { get; set; }

    private string _email;
    
    public List<Log> ActionsInLog { get; set; }

    [Key]
    public string Email
    {
        get => _email;
        set
        {
            if (!HasValidEmail(value))
            {
                throw new ArgumentException("Formato de email inválido, debe ser del tipo ej. nombre@dominio");
            }

            _email = value;
        }
    }
    
    public Client()
    {
        _reservations = new List<Reservation>();
        ActionsInLog = new List<Log>();
        Notifications = new List<Notification>();
    }
    public List<Notification> Notifications { get; set; }

    private bool PasswordsAreNotEqual(string? password1, string password2)
    {
        if (password1 != password2)
        {
            return true;
        }

        return false;
    }

    public List<Reservation> _reservations { get; set; }
    
    public Client(string? name, string? email, string? password1, string password2, bool isAdmin)
    {
        if (PasswordsAreNotEqual(password1, password2))
        {
            throw new ArgumentException("Las contraseñas no coinciden");
        }

        Name = name;
        Password = password1;
        Email = email;
        IsAdmin = isAdmin;
        _reservations = new List<Reservation>();
        ActionsInLog = new List<Log>();
        Notifications = new List<Notification>();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return Email == ((Client)obj).Email;
    }
}