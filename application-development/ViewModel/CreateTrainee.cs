using System.ComponentModel.DataAnnotations;

namespace application_development.ViewModel
{
    public class CreateTrainee
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string TraineeName { get; set; }
        public int Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Education { get; set; }
        public string Programming { get; set; }
        public string TOEICscore { get; set; }
        public string Exprience_details { get; set; }
        public string Department { get; set; }
    }
}