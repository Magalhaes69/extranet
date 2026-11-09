namespace WebFrontEnd.Services.CultureServices
{
    public static class LocalizerSettings
    {
        public static CultureWithName NeutralCulture = new CultureWithName("Português", "pt-PT");
        public static readonly List<CultureWithName> SupportedCultureWithName =
            new List<CultureWithName>()
            {
                new CultureWithName("Português", "pt-PT"),
                new CultureWithName("Português (Brasil)", "pt-BR"),
                new CultureWithName("English", "en-US"),
            };
    }
}
