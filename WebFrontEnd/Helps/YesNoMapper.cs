namespace WebFrontEnd.Helps
{
    public static class YesNoMapper
    {
        public static string ToDisplay(string value)
        {
            return value switch
            {
                "tYES" => "Sim",
                "tNO" => "Não",
                _ => ""
            };
        }
    }
}
