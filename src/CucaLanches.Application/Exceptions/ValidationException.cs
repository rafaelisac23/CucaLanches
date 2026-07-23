using CucaLanches.Application.Validators;

namespace CucaLanches.Application.Exceptions;

public class ValidationException:Exception
{
    
    public readonly List<ValidationError> Errors;
    
    public ValidationException(List<ValidationError> errors) : base("Have some Validation Error")
    {
        Errors = errors;
    }
    
}