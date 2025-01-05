using ExamSchedulingSystem.Models;
using System.ComponentModel.DataAnnotations;



public class CustomIdValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var user = (User)validationContext.ObjectInstance; 
        var id = value as string; 

        

        if (user.Role == UserRole.Student && id.Length != 7)
        {
            return new ValidationResult("Student ID must be exactly 7 characters.");
        }
        if (user.Role == UserRole.Faculty && id.Length != 5)
        {
            return new ValidationResult("Faculty ID must be exactly 5 characters.");
        }
     
        
        return ValidationResult .Success; 
    }
}
