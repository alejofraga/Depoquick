namespace BusinessLogic;

public class DateTimeProvider
{
    
    private static DateTime _currentDateTime = DateTime.Today;
    public static DateTime GetCurrentDateTime()
    {
        return _currentDateTime;
    }

    public static void SetCurrentDateTime(DateTime date)
    {
        _currentDateTime = date;
    }
     
}