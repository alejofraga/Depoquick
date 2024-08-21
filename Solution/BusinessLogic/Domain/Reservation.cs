namespace BusinessLogic.Domain;

public class Reservation
{
    private string? _rejectedMessage;

    public string? RejectedMessage
    {
        get => _rejectedMessage;
        set
        {
            const int maxLength = 300;
            if (value.Length > maxLength)
            {
                throw new ArgumentException("El mensaje de rechazo no debe superar los 300 caracteres");
            }

            _rejectedMessage = value;
        }
    }

    public bool IsReviewed { get; set; }

    private DateTime _startDate;

    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            if (IsAfterThanActualDate(value))
            {
                _startDate = value;
            }
            else
            {
                throw new ArgumentException("La fecha de inicio debe ser posterior a la fecha actual");
            }
        }
    }

    private static bool IsAfterThanActualDate(DateTime value)
    {
        return value.Date >= DateTimeProvider.GetCurrentDateTime();
    }

    private DateTime _endDate;

    public DateTime EndDate
    {
        get => _endDate;
        set { _endDate = value; }
    }
    public bool IsConfirmed { get; set; }

    public bool IsRejected { get; set; }

    public Deposit? Deposit { get; set; }
    public int DepositId { get; set; }
    
    public Client Client { get; set; }
    public string ClientEmail { get; set; }

    public int Id { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var reservation = (Reservation)obj;
        return reservation.Id == Id;
    }
    
    private static bool ReservationTimeIsAtLeastOneDay(DateTime startDate, DateTime endDate)
    {
        return (endDate - startDate).Days >= 1;
    }
    
    public Reservation(DateTime startDate, DateTime endDate, bool isConfirmed, bool isReviewed)
    {
        if (!ReservationTimeIsAtLeastOneDay(startDate, endDate))
        {
            throw new ArgumentException("La reserva debe ser de al menos un dia de duración");
        }

        StartDate = startDate;
        EndDate = endDate;
        IsConfirmed = isConfirmed;
        IsRejected = false;
        IsReviewed = isReviewed;
        PaymentStatus = PaymentStatus.NoPayment;
    }

    public PaymentStatus PaymentStatus { get; set; }
    
    public Reservation()
    {
    }
}