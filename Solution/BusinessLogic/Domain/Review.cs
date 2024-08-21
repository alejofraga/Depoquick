namespace BusinessLogic.Domain;

public class Review
{
    public int Id { get; set; }

    private int _valoration;

    private bool ValorationIsBetween1And5(int valoration)
     {
        return (valoration <= 5 && valoration >= 1);
    }

    public int Valoration
    {
        get => _valoration;
        set
        {
            if (!ValorationIsBetween1And5(value))
            {
                throw new ArgumentException("La valoración debe estar entre 1 y 5");
            }

            _valoration = value;
        }
    }
    
    public Deposit Deposit { get; set; }
    
    public int DepositId { get; set; }

    private string? _comment;
    private bool CommentLengthIsGreaterThan500Chars(string? inputString)
    {
        const int maxLength = 500;
        return inputString.Length > maxLength;
    }
    public string? Comment
    {
        get => _comment;
        set
        {
            if (CommentLengthIsGreaterThan500Chars(value))
            {
                throw new ArgumentException("El comentario no puede tener más de 500 caracteres");
            }
            _comment = value;
        }
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Review)obj;
        return Id == other.Id;
    }

    public Review(int valoration, string? comment)
    {
        Valoration = valoration;
        Comment = comment;
    }

    public Review()
    {
    }
}