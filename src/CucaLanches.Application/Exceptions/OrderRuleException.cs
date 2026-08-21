namespace CucaLanches.Application.Exceptions;

public class OrderRuleException:Exception
{
    public OrderRuleException(string message):base(message)
    {
    }
}