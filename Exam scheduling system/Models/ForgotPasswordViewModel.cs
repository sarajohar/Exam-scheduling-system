using System.ComponentModel.DataAnnotations;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "The Email field is required.")]
    [RegularExpression(@"^[a-z]+@hu\.edu\.jo$|^[0-9]+@std\.hu\.edu\.jo$",
        ErrorMessage = "Invalid Email Format.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "The Code field is required.")]
    public int Code { get; set; }
}