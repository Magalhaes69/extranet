
namespace Persistence.Seeders
{
    public static class SapSqlQueries
    {
        public const string IncomingPaymentsCheckTotal = "IncomingPaymentsCheckTotal";
        public const string InactiveUserContacts = "InactiveUserContacts";
        public static readonly IReadOnlyList<SapSqlQueryDefinition> All = new[]
       {
           new SapSqlQueryDefinition(
                IncomingPaymentsCheckTotal,
                "Incoming payments - total of checks",
                "SELECT SUM(\"CheckSum\") AS \"TotalCheckSum\" FROM \"ORCT\" " +
                "WHERE (\"CardCode\" LIKE :cardCode OR (\"CardCode\" IS NULL AND \"DocEntry\" >= :cardCodeNull)) " +
                "AND (\"CardName\" LIKE :cardName OR (\"CardName\" IS NULL AND \"DocEntry\" >= :cardNameNull)) " +
                "AND (\"PrjCode\" LIKE :project OR (\"PrjCode\" IS NULL AND \"DocEntry\" >= :projectNull)) " +
                "AND \"Canceled\" LIKE :cancelled " +
                "AND \"DocType\" LIKE :docType " +
                "AND \"DocCurr\" LIKE :currency " +
                "AND \"DocDate\" >= :dateFrom AND \"DocDate\" <= :dateTo " +
                "AND \"DocDueDate\" >= :dueFrom AND \"DocDueDate\" <= :dueTo " +
                "AND \"TaxDate\" >= :taxFrom AND \"TaxDate\" <= :taxTo " +
                "AND \"DocNum\" >= :docNumFrom AND \"DocNum\" <= :docNumTo"),
            new SapSqlQueryDefinition(
                "GetSeriesDynamic",
                "Series Dynamic",
                "SELECT \"Series\", \"SeriesName\" FROM \"NNM1\" WHERE \"ObjectCode\" = :ObjectCode"
                ),
              new SapSqlQueryDefinition(
                InactiveUserContacts,
                "USCT - Select new users",
                "SELECT \"Code\", \"Name\", \"U_Email\", \"U_IsActive\", \"U_CardCode\" " +
                "FROM \"@USCT\" " +
                "WHERE \"U_IsActive\" = 'N'")
        };
    }
}
