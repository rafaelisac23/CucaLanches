using CucaLanches.Application.Validators;

namespace CucaLanches.Application.Exceptions;

public class ValidationException:Exception
{
    
    public readonly List<ValidationError> Errors;
    public readonly int? CodeStatus = 400;
    
    public ValidationException(List<ValidationError> errors) : base("Have some Validation Error")
    {
        Errors = errors;
    }

    public ValidationException(List<ValidationError> errors,int codeStatus) : base("Have some Validation Error")
    {
        Errors = errors;
        CodeStatus = codeStatus;
    }
    
}