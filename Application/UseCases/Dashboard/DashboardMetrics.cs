namespace Application.UseCases.Dashboard
{
    //public static class DashboardMetrics
    //{
    //    private const string Open = "DocumentStatus eq 'bost_Open'";
    //    private const string NotCancelled = "Cancelled eq 'tNO'";

    //    private static string Customer(string cardCode) => $"CardCode eq '{cardCode.Replace("'", "''")}'";
    //    private static string Today => DateTime.Today.ToString("yyyy-MM-dd");
    //    private static string YearStart => new DateTime(DateTime.Today.Year, 1, 1).ToString("yyyy-MM-dd");

    //    private static readonly (string, int)[] DocTotal = { ("DocTotal", 1) };
    //    private static readonly (string, int)[] OpenBalance = { ("DocTotal", 1), ("PaidToDate", -1) };

    //    public static readonly IReadOnlyDictionary<string, DashboardMetricDefinition> All =
    //        new Dictionary<string, DashboardMetricDefinition>(StringComparer.OrdinalIgnoreCase)
    //        {
    //            ["orders-open"] = new("Orders", c => $"{Customer(c)} and {Open}", DocTotal),
    //            ["quotations-open"] = new("Quotations", c => $"{Customer(c)} and {Open}", DocTotal),
    //            ["deliveries-open"] = new("DeliveryNotes", c => $"{Customer(c)} and {Open}", DocTotal),
    //            ["invoices-open"] = new("Invoices", c => $"{Customer(c)} and {Open}", OpenBalance),
    //            ["invoices-overdue"] = new("Invoices", c => $"{Customer(c)} and {Open} and DocDueDate lt '{Today}'", OpenBalance),
    //            ["creditnotes-year"] = new("CreditNotes", c => $"{Customer(c)} and {NotCancelled} and DocDate ge '{YearStart}'", DocTotal),
    //            ["payments-year"] = new("IncomingPayments",
    //                                       c => $"{Customer(c)} and {NotCancelled} and DocDate ge '{YearStart}'",
    //                                       new[] { ("CashSum", 1), ("TransferSum", 1) },
    //                                       IncludeChecks: true),
    //        };
    //}

    public static class DashboardMetrics
    {
        private const string Open = "DocumentStatus eq 'bost_Open'";
        private const string NotCancelled = "Cancelled eq 'tNO'";

        private static string Customer(string cardCode) => $"CardCode eq '{cardCode.Replace("'", "''")}'";
        private static string Today => DateTime.Today.ToString("yyyy-MM-dd");
        private static string YearStart => new DateTime(DateTime.Today.Year, 1, 1).ToString("yyyy-MM-dd");

        private static readonly (string, int)[] NoAmount = Array.Empty<(string, int)>();
        private static readonly (string, int)[] OpenBalance = { ("DocTotal", 1), ("PaidToDate", -1) };

        public static readonly IReadOnlyDictionary<string, DashboardMetricDefinition> All =
            new Dictionary<string, DashboardMetricDefinition>(StringComparer.OrdinalIgnoreCase)
            {
                // Só contagem
                ["orders-open"] = new("Orders", c => $"{Customer(c)} and {Open}", NoAmount),
                ["quotations-open"] = new("Quotations", c => $"{Customer(c)} and {Open}", NoAmount),
                ["deliveries-open"] = new("DeliveryNotes", c => $"{Customer(c)} and {Open}", NoAmount),
                ["creditnotes-year"] = new("CreditNotes", c => $"{Customer(c)} and {NotCancelled} and DocDate ge '{YearStart}'", NoAmount),
                ["payments-year"] = new("IncomingPayments", c => $"{Customer(c)} and {NotCancelled} and DocDate ge '{YearStart}'", NoAmount),

                // Contagem + total
                ["invoices-open"] = new("Invoices", c => $"{Customer(c)} and {Open}", OpenBalance),
                ["invoices-overdue"] = new("Invoices", c => $"{Customer(c)} and {Open} and DocDueDate lt '{Today}'", OpenBalance),
            };
    }
}
