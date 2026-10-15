namespace Application.UseCases.VatGroups.GetAllVatGroups
{
    public sealed record GetAllIVatGroupsResult
    {
        public List<GetAllIVatGroupsResponse> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
