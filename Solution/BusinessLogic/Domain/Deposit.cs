using System.Text.RegularExpressions;

namespace BusinessLogic.Domain;

public class Deposit
{
    private string _name;

    private static bool IsValidName(string name)
    {
        if (Regex.IsMatch(name, "^[a-zA-Z ]+$"))
        {
            return true;
        }
        return false;
    }

    public string Name
    {
        get => _name;
        set
        {
            if (IsValidName(value))
            {
                _name = value.ToUpper();
            }
            else
            {
                throw new ArgumentException("El nombre solo puede contener letras y espacios");
            }
        }
    }

    private char _area;

    private static bool IsValidArea(char area)
    {
        char[] validAreas = { 'A', 'B', 'C', 'D', 'E' };
        return validAreas.Contains(area);
    }

    public char Area
    {
        get => _area;
        set
        {
            if (!IsValidArea(value))
            {
                throw new ArgumentException("El área debe ser A, B, C, D o E");
            }

            _area = value;
        }
    }

    private string? _size;

    private static bool IsValidSize(string? size)
    {
        string[] validSizes = { "Pequeño", "Mediano", "Grande" };

        return validSizes.Contains(size);
    }

    public string? Size
    {
        get => _size;
        set
        {
            if (!IsValidSize(value))

            {
                throw new ArgumentException("Tamaño inválido. Los tamaños válidos son Pequeño, Mediano y Grande");
            }

            _size = value;
        }
    }


    public bool Conditioning { get; set; }

    public int Id { get; set; }

    public List<Promotion?> Promotions { get; set; }
    
    public List<Review> Reviews { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Deposit)obj;
        return Id == other.Id;
    }

    public List<DateRange> Disponibility { get; set; }
    
    public Deposit()
    {
        Disponibility = new List<DateRange>();
        Promotions = new List<Promotion?>();
        Reviews = new List<Review>();
    }

}