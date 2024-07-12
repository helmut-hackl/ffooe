using System.ComponentModel.DataAnnotations;

namespace ffooe.rest.api.Models
{
    public class RegisterModel : LoginModel
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;
    }
}
