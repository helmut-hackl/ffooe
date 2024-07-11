using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ffooe.db.entities
{
    public partial class M_User
    {
        [Key]
        public virtual string UserName { get; set; } = string.Empty;
        public virtual string MailAddress { get; set; } = string.Empty;
        [JsonIgnore] 
        public virtual string PasswordHash { get; set; } = string.Empty;
        public int VerifyCode { get; set; }
        public virtual bool LockOut { get; set; } 
    }
}
