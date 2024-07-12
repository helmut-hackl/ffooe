using Swashbuckle.AspNetCore.Annotations;

namespace ffooe.db.entities
{
    public partial class M_Client
    {
        public virtual int Id { get; set; }
        public virtual string ClientName { get; set; } = string.Empty;
        public virtual string ClientStreet { get; set; } = string.Empty;
        public virtual string ClientStreetNumber { get; set; } = string.Empty;
        public virtual int ClientPostalCode { get; set; }
        public virtual string ClientCity { get; set; } = string.Empty;
        public virtual int ClientSirenCode { get; set; }
        public virtual string ClientRegisterToken { get; set; } = string.Empty;
        [SwaggerSchema(ReadOnly = true)]
        public virtual string? ClientRefrehsToken { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public virtual string? ClientAccessToken { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public virtual DateTime? ClientAccessExpireDate { get; set; }
        public virtual string? ClientPushGroup { get; set; }
        public virtual string ClientAdminEmail { get; set; } = string.Empty;
    }
}
