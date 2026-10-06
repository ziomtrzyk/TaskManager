using System.ComponentModel.DataAnnotations;

namespace TaskManager.Validation;

public class NotInPastAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if(value is DateTime dateTime)
        {
            if(dateTime < DateTime.UtcNow )
                return false;
        }
        return true;
    }
}