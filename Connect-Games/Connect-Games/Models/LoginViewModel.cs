
using System.ComponentModel.DataAnnotations;


namespace Connect_Games.Models
{
    /// <summary>
    /// Container used to hold the user’s login/registration information.
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
