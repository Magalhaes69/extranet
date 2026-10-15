
namespace Persistence.SAP_Helps
{
    public sealed class UserContactResponse
    {
        public string Code { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? U_Email { get; set; }
        public string? U_IsActive { get; set; }
        public string? U_CardCode { get; set; }
    }
}
