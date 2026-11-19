namespace WebFrontEnd.Services.CultureServices
{
    public static class LocalizerSettings
    {
        public static CultureWithName NeutralCulture = new CultureWithName("English", "en-US");
        public static readonly List<CultureWithName> SupportedCultureWithName =
            new List<CultureWithName>()
            {
                new CultureWithName("English", "en-US"),
                new CultureWithName("Português(PT)", "pt-PT"),
                new CultureWithName("Português(BR)", "pt-BR"),
            };
    }
}
