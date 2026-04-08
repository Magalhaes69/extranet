using System.Net.Http.Json;
using WebFrontEnd.Services.Invoices.Dto;
using WebFrontEnd.Shared.Dtos;


namespace WebFrontEnd.Services.Invoices
{
    public class InvoicesService : IInvoicesService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Invoices";
        private List<GetAllInvoicesDto>? _cache;
        private GetAllInvoicesDto invoices;
        public Action InvoicesAction { get; set; }

        public InvoicesService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<List<GetAllInvoicesDto>> GetInvoiceAsync(
            string? CardCode,
            int? DocNum = null,
            string? DocumentStatus = null,
            List<string>? DocumentStatuses = null,
            List<string>? Cancelled = null,
            List<FilterRow>? filterRows = null,
            bool forceRefresh = false)
        {
            try
            {
                if (_cache is not null && !forceRefresh)
                    return _cache;


                
                var queryParams = new List<string>();

                if (!string.IsNullOrEmpty(CardCode))
                    queryParams.Add($"CardCode={CardCode}");

                if(DocumentStatuses?.Any() == true)
                {
                    if (DocumentStatuses.Count == 1)
                        queryParams.Add($"DocumentStatus={DocumentStatuses[0].ToString()}");
                    else
                    {
                        foreach (var status in DocumentStatuses)
                            queryParams.Add($"DocumentStatuses={status}");
                    }
                }

                if (Cancelled?.Any() == true)
                {
                    if (Cancelled.Count == 1)
                        queryParams.Add($"Cancelled={Cancelled[0].ToString()}");
                    else
                    {
                        foreach (var status in Cancelled)
                            queryParams.Add($"CancelledList={status}");
                    }
                }

                if (DocNum.HasValue)
                    queryParams.Add($"DocNum={DocNum}");
                

                if (Cancelled?.Any() == true)
                {
                    foreach (var status in Cancelled)
                        queryParams.Add($"Cancelled = {status}");
                }


                if(filterRows?.Any() == true)
                {
                    for (int i = 0; i < filterRows!.Count; i++)
                    {
                        var f = filterRows[i];

                        if (f.StartDate.HasValue)
                            queryParams.Add($"filterRows[{i}].StartDate={f.StartDate:yyyy-MM-dd}");
                        if (f.EndDate.HasValue)
                            queryParams.Add($"filterRows[{i}].EndDate={f.EndDate:yyyy-MM-dd}");
                        if (f.Today.HasValue)
                            queryParams.Add($"filterRows[{i}].Today={f.Today:yyyy-MM-dd}");
                        queryParams.Add($"filterRows[{i}].Type={f.Type}");
                        if (!string.IsNullOrEmpty(f.Value))
                            queryParams.Add($"filterRows[{i}].Value={f.Value}");
                    }

                }
                var query = string.Join("&", queryParams);
                var response = await _httpClient.GetAsync($"{BaseUrl}/list?{query}");




                if (!response.IsSuccessStatusCode)
                    return new List<GetAllInvoicesDto>();

                _cache = await response.Content.ReadFromJsonAsync<List<GetAllInvoicesDto>>()
                         ?? new List<GetAllInvoicesDto>();

                return _cache;
            }
            catch (Exception ex)
            {
                _httpClient.Dispose();
                throw null!;
            }
        }

        public async Task<List<GetAllInvoicesDto>> GetInvoicesAsync(
            InvoiceRequest request,
            bool forceRefresh = false)
        {
            try
            {
                if (_cache is not null && !forceRefresh)
                    return _cache;

                var serialized = General.GennerateStringContent(General.SerializeObject(request));
                var response = await _httpClient.PostAsync($"{BaseUrl}/list", serialized);

                if (!response.IsSuccessStatusCode)
                    return new List<GetAllInvoicesDto>();

                _cache = await response.Content.ReadFromJsonAsync<List<GetAllInvoicesDto>>()
                         ?? new List<GetAllInvoicesDto>();

                return _cache;
            }
            catch
            {
                return new List<GetAllInvoicesDto>();
            }
        }
        public async Task<GetAllInvoicesDto> GetByDocEntryInvoicesAsync(string cardCode, int DocEntry, bool forceRefresh = false)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/details?CardCode={cardCode}&DocEntry={DocEntry}");

            if ((!response.IsSuccessStatusCode))
            {
                return null!;
            }

            invoices = await response.Content.ReadFromJsonAsync<GetAllInvoicesDto>()
                     ?? new GetAllInvoicesDto();

            return invoices;
        }
    }
}
