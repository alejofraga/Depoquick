namespace BusinessLogic;

public class LogDto
{

    public LogDto()
    {
        ActionDescriptions = new List<string>();
    }
    
    public LogDto(List<string> actionDescriptions)
    {
        ActionDescriptions = actionDescriptions;
    }

    public List<string> ActionDescriptions { get; set; }

}