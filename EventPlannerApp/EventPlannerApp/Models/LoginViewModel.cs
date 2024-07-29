using System.ComponentModel.DataAnnotations;

namespace EventPlannerApp.Models
{
    public class LoginViewModel
    {

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display (Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
