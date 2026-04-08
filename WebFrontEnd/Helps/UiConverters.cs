using Microsoft.Extensions.Localization;

namespace WebFrontEnd.Helps
{
    public class UiConverters
    {
        private readonly IStringLocalizer<App> _localizer;
        public UiConverters(IStringLocalizer<App> localizer)
        {
            this._localizer = localizer;
        }
        public static bool ToBool(string value)
      => value == "tYES";


        public static string ToBoolText(string value)
            => value == "tYES" ? "Sim" : "Não";


        public static string ToDate(DateTime? date)
            => date?.ToString("dd/MM/yyyy") ?? "-";

        public static string ToCurrency(decimal value, string currency)
            => $"{value:N2} {new System.Globalization.CultureInfo("pt-PT")} {currency}";

        public static string ToFormat(decimal value)
            => value.ToString("N2", new System.Globalization.CultureInfo("pt-PT"));
        public static string ToPaymentType(string value)
            => value switch
            {
                "bopt_None" => "Nenhum",
                "bopt_Cash" => "Dinheiro",
                "bopt_Transfer" => "Transferência",
                "bopt_Check" => "Cheque",
                _ => value ?? "-"
            };

        public static string ToDocType(string value)
            => value switch
            {
                "rCustomer" => "Cliente",
                "rSupplier" => "Fornecedor",
                _ => value ?? "-"
            };
        public string ToDocStatus(string documentStatus, string cancelled)
        {
            if (cancelled == "tYES")
                return _localizer["DocStatus_Cancelled"];

            return documentStatus switch
            {
                "bost_Open" => _localizer["DocStatus_Open"],
                "bost_Close" => _localizer["DocStatus_Closed"],
                _ => documentStatus ?? "-"
            };
        }
        public static string ToDocStatusClass(string documentStatus, string cancelled)
        => cancelled == "tYES" ? "text-danger" :
           documentStatus switch
           {
               "bost_Open" => "text-success",
               "bost_Close" => "text-secondary",
               _ => "badge bg-light text-dark"
           };
    }
}
