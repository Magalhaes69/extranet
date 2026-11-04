namespace WebAPI.Settings
{
    /// <summary>Where the "create account" requests are forwarded to (section "LeadsApi" of appsettings).</summary>
    public class LeadsApiSettings
    {
        /// <summary>Address of the leads API, without the path. Example: https://localhost:7032</summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>Path of the "create lead" endpoint.</summary>
        public string CreatePath { get; set; } = "api/Leads";

        /// <summary>Optional API key sent to the leads API. Keep the value out of the repository (user-secrets / environment).</summary>
        public string? ApiKey { get; set; }

        /// <summary>Header that carries <see cref="ApiKey"/>.</summary>
        public string ApiKeyHeader { get; set; } = "X-Api-Key";

        /// <summary>Length of the trial of a new account. Decided here, never by the visitor.</summary>
        public int TrialDays { get; set; } = 30;

        /// <summary>Status given to a new account (0 = trial). Decided here, never by the visitor.</summary>
        public int TrialStatus { get; set; } = 0;
    }
}
