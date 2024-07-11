using System.ComponentModel.DataAnnotations;
namespace ffooe.rest.api.Models
{
    public class VerifyModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "VerifyCode is required.")]
        public string VerifyCode { get; set; } = string.Empty;
    }
}
