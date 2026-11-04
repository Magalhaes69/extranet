namespace WebFrontEnd.Services.Licenses.Dto
{
    /// <summary>One license as returned by GET api/Licenses.</summary>
    public class LicenseItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public string? LicenseKey { get; set; }

        /// <summary>Active, Pending (requested, waiting for approval), Expired or Suspended.</summary>
        public string Status { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        /// <summary>Number of users covered by the license, when the product has that limit.</summary>
        public int? Users { get; set; }
    }

    /// <summary>What the customer fills in to renew a license.</summary>
    public class LicenseRenewFormDto
    {
        public int Months { get; set; } = 12;
        public string? Notes { get; set; }
    }

    /// <summary>What the customer fills in to request a new license.</summary>
    public class LicenseRequestFormDto
    {
        public string Product { get; set; } = string.Empty;
        public int Users { get; set; } = 1;
        public string? Notes { get; set; }
    }
}
