using System.Globalization;
using Microsoft.Extensions.Localization;

namespace WebFrontEnd.Helps
{
    public class UiConverters
    {
        private readonly IStringLocalizer<App> _localizer;
        private readonly string _lang = string.Empty;
        public UiConverters(IStringLocalizer<App> localizer)
        {
            this._localizer = localizer;
            this._lang = CultureInfo.CurrentCulture.Name;
        }
        public static bool ToBool(string value)
      => value == "tYES";


        public static string ToBoolText(string value)
            => value == "tYES" ? "Sim" : "Não";

        /// <summary>
        /// Converts a nullable date to a short date string using the current culture, or returns a placeholder if the
        /// date is null.
        /// </summary>
        /// <param name="date">The nullable date to format. If null, a placeholder string is returned.</param>
        /// <returns>A string representation of the date in short date format according to the current culture, or "-" if the
        /// date is null.</returns>
        public static string ToDate(DateTime? date)
            => date?.ToString("d", CultureInfo.CurrentCulture) ?? "-";

        /// <summary>
        /// Stringifies a decimal value as a currency string with two decimal places, using the current culture for formatting, and appending the specified currency code.
        /// </summary>
        /// <param name="value">The decimal value to format.</param>
        /// <param name="currency">The currency code to append.</param>
        /// <returns>A string representation of the decimal value as a currency string.</returns>
        public string ToCurrency(decimal value, string currency)
        {
            var culture = new CultureInfo(_lang);
            return $"{value.ToString("N2", culture)} {currency}";
        }

        /// <summary>
        /// Convert a decimal value to a string with two decimal places, formatted according to the current culture. If the value is null, return a placeholder string.
        /// </summary>
        /// <param name="value">The decimal value to format.</param>
        /// <returns>A string representation of the decimal value with two decimal places, or a placeholder if the value is null.</returns>
        public string ToFormat(decimal value)
            => value.ToString("N2", new CultureInfo(_lang));
        public static string ToPaymentType(string value)
            => value switch
            {
                "bopt_None" => "Nenhum",
                "bopt_Cash" => "Dinheiro",
                "bopt_Transfer" => "Transferência",
                "bopt_Check" => "Cheque",
                _ => value ?? "-"
            };

        public string ToDocType(string value)
            => value switch
            {
                "rCustomer" => _localizer["rCustomer"],
                "rSupplier" => _localizer["rSupplier"],
                "dDocument_Items" => _localizer["dDocument_Items"],
                "dDocument_Service" => _localizer["dDocument_Service"],
                "it_Invoice" => _localizer["it_Invoice"],
                "it_JournalEntry" => _localizer["it_JournalEntry"],
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
