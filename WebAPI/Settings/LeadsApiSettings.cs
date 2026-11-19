namespace WebAPI.Settings
{
    /// <summary>
    /// Regras do trial dos registos públicos (secção "LeadsApi" do appsettings).
    /// A comunicação com o LicenseKey faz-se pelo ILeadApiClient (secção "LicensingApi").
    /// </summary>
    public class LeadsApiSettings
    {
        /// <summary>Length of the trial of a new account. Decided here, never by the visitor.</summary>
        public int TrialDays { get; set; } = 30;

        /// <summary>Status given to a new account (0 = New). Decided here, never by the visitor.</summary>
        public int TrialStatus { get; set; } = 0;
    }
}
