using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace QwackX.Blazor.Models.Forms
{
    public class RegisterUserForm
    {
        [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom d'utilisateur doit contenir entre 3 et 50 caractères.")]
        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
        [PasswordValidation(ErrorMessage = "Le mot de passe doit contenir au moins une majuscule, une minuscule, un chiffre et un caractère spécial.")]
        public string Password { get; set; } = default!;

        [Required(ErrorMessage = "Veuillez confirmer votre mot de passe.")]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; } = default!;
    }
    
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string password)
            {
                var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");

                if (!passwordRegex.IsMatch(password))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}