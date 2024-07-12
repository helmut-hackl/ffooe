using System.ComponentModel.DataAnnotations;

namespace ffooe.rest.api.Models
{
    public class RefreshModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;
    }
}
