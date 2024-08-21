namespace BusinessLogic.Domain;

public class Promotion
{
    private string? _tag;

    private bool TagIsAlphaNumericAndAllowSpaces(string? tag)
    {
        return tag.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c));
    }

    private bool TagHasAMaximumLengthOf20(string? tag)
    {
        const int maxLength = 20;
        return tag.Length <= maxLength;
    }
    private bool TagIsValid(string? tag)
    {
        return TagIsAlphaNumericAndAllowSpaces(tag) && TagHasAMaximumLengthOf20(tag);
    }
    public string? Tag
    {
        get => _tag;
        set
        {
            if (TagIsValid(value))
            {
                _tag = value;
            }
            else
            {
                throw new ArgumentException(
                    "Etiqueta inválida, debe ser alfanumérica y tener un máximo de 20 caracteres");
            }
        }
    }

    private bool DiscountPercentageIsValid(int discountPercentage)
    {
        return DiscountPercentageIsUpTo75(discountPercentage) &&
               DiscountPercentageIsGreaterThanOrEqualTo5(discountPercentage);
    }
    private int _discountPercentage;
    public bool DiscountPercentageIsUpTo75(int discountPercentage)
    {
        const int maxDiscountPercentage = 75;
        return discountPercentage <= maxDiscountPercentage;
    }

    public bool DiscountPercentageIsGreaterThanOrEqualTo5(int discountPercentage)
    {
        const int minDiscountPercentage = 5;
        return discountPercentage >= minDiscountPercentage;
    }

    public int DiscountPercentage
    {
        get => _discountPercentage;
        set
        {
            if (DiscountPercentageIsValid(value))
            {
                _discountPercentage = value;
            }
            else
            {
                throw new ArgumentException("Porcentaje de descuento inválido, debe ser entre 5 y 75%");
            }
        }
    }

    
    public List<Deposit> Deposits { get; set; }
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    
    public int Id { get; set; }

    
    public bool StartDateIsBeforeEndDate(DateTime startDate, DateTime endDate)
    {
        return startDate < endDate;
    }

    public bool DateIsTodayOrInTheFuture(DateTime date)
    {
        return date >= DateTime.Today;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Promotion)obj;
        return Id == other.Id;
    }

    public Promotion(string? tag, int discountPercentage, DateTime startDate, DateTime endDate)
    {
        if (!StartDateIsBeforeEndDate(startDate, endDate))
        {
            throw new ArgumentException("La fecha de inicio no puede ser mayor a la de finalización");
        }

        if (!DateIsTodayOrInTheFuture(startDate))
        {
            throw new ArgumentException("La fecha de inicio está en el pasado");
        }
        Tag = tag;
        DiscountPercentage = discountPercentage;
        StartDate = startDate;
        EndDate = endDate;
    }

    public Promotion()
    {
    }
}