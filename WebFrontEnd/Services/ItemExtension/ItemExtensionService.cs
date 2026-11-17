using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using WebFrontEnd.Services.ItemExtension.Dto;

namespace WebFrontEnd.Services.ItemExtension
{
    public class ItemExtensionService : IItemExtensionService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/ItemExtension";
       
        private List<ViewItemExtensionDto>? _cache;
        public ItemExtensionService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }

        public async Task<ServiceResponseGeneralPost<ViewItemExtensionDto>> CreateAsync(
                CreateItemExtensionDto request,
                IBrowserFile? file)
        {
            try
            {
                var content = new MultipartFormDataContent();

                // Add DTO fields
                content.Add(new StringContent(request.ItemCode ?? ""), "ItemCode");
                content.Add(new StringContent(request.ItemName ?? ""), "ItemName");
                content.Add(new StringContent(request.UserId ?? ""), "UserId");

                

                // Add file (if exists)
                if (file != null)
                {
                    var fileContent = new StreamContent(file.OpenReadStream(5 * 1024 * 1024));

                    fileContent.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                    content.Add(fileContent, "file", file.Name);
                }

                var response = await _httpClient.PostAsync(BaseUrl, content);

                var result = CheckResponsePost(response);

                if (!result.Flag)
                {
                    return new ServiceResponseGeneralPost<ViewItemExtensionDto>
                    {
                        Flag = false,
                        Message = result.Message
                    };
                }

                var apiResponse = await RedContent(response);

                var created = General.DeserialzeJsonString<ServiceResponseGeneralPost<ViewItemExtensionDto>>(apiResponse);

                return created;
            }
            catch (Exception ex)
            {
                return new ServiceResponseGeneralPost<ViewItemExtensionDto>
                {
                    Flag = false,
                    Message = ex.Message
                };
            }
        }

        private ServiceResponsePost CheckResponsePost(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                return new ServiceResponsePost()
                {
                    Flag = false,
                    Message = "Error occured. Try again later..."
                };
            }

            return new ServiceResponsePost()
            {
                Flag = true,
                Message = null!
            };
        }

        private static async Task<string> RedContent(HttpResponseMessage response) => await response.Content.ReadAsStringAsync();

        public async Task<bool> CheckByItemCodeAsync(string ItemCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/GetByItemCode?itemCode={ItemCode}");
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
                var apiResponse = await RedContent(response);
                var result = General.DeserialzeJsonString<ServiceResponseGeneralPost<ViewItemExtensionDto>>(apiResponse);
                if (!result.Flag || result.Data == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<ViewItemExtensionDto>> GetAll()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/list");

                if (!response.IsSuccessStatusCode)
                    return new List<ViewItemExtensionDto>();

                _cache = await response.Content.ReadFromJsonAsync<List<ViewItemExtensionDto>>()
                         ?? new List<ViewItemExtensionDto>();

                return _cache;
            }
            catch
            {
                return new List<ViewItemExtensionDto>();
            }
        }

        public Task<ServiceResponseGeneralPost<object>> RemoveAsyncLast(List<string> ItemId)
        {
            try
            {
                var response = _httpClient.DeleteAsync($"{BaseUrl}?itemId={ItemId}").Result;
                if (!response.IsSuccessStatusCode)
                {
                    return Task.FromResult(new ServiceResponseGeneralPost<object>
                    {
                        Flag = false,
                        Message = "Error occured. Try again later..."
                    });
                }
                var apiResponse = response.Content.ReadAsStringAsync().Result;
                var result = General.DeserialzeJsonString<ServiceResponseGeneralPost<object>>(apiResponse);

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ServiceResponseGeneralPost<object>
                {
                    Flag = false,
                    Message = ex.Message
                });

            }
        }

        public async Task<ServiceResponseGeneralPost<object>> RemoveAsync(List<string> ItemCode)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}/delete-many", General.GennerateStringContent(General.SerializeObject(ItemCode)));

                if (!response.IsSuccessStatusCode)
                {
                    return new ServiceResponseGeneralPost<object>
                    {
                        Flag = false,
                        Message = "Error occurred. Try again later..."
                    };
                }

                var apiResponse = await RedContent(response);

                return General.DeserialzeJsonString<ServiceResponseGeneralPost<object>>(apiResponse)
                       ?? new ServiceResponseGeneralPost<object> { Flag = false, Message = "Invalid response" };
            }
            catch (Exception ex)
            {
                return new ServiceResponseGeneralPost<object>
                {
                    Flag = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ViewItemExtensionDto> GetByItemId(string ItemId)
        {
            try
            {
                try
                {
                    var response = await _httpClient.GetAsync($"{BaseUrl}/GetByItemId?ItemId={ItemId}");

                    if (!response.IsSuccessStatusCode)
                        return new ViewItemExtensionDto();

                    var result = await response.Content.ReadFromJsonAsync<ViewItemExtensionDto>()
                             ?? new ViewItemExtensionDto();

                    return result;
                }
                catch
                {
                    return new ViewItemExtensionDto();
                }
            }
            catch (Exception ex)
            {
                return null!;
            }
        }

        public async Task<ServiceResponseGeneralPost<ViewItemExtensionDto>> EditAsync(
            EditItemExtensionDto request, 
            IBrowserFile? file)
        {
            try
            {
                var content = new MultipartFormDataContent();

                // Add DTO fields
                content.Add(new StringContent(request.ItemCode ?? ""), "ItemCode");
                content.Add(new StringContent(request.ItemName ?? ""), "ItemName");
                content.Add(new StringContent(request.UserId ?? ""), "UserId");
                content.Add(new StringContent(request.ImageUrl ?? ""), "ImageUrl");
                content.Add(new StringContent(request.ItemId.ToString() ?? ""), "ItemId");



                // Add file (if exists)
                if (file != null)
                {
                    var fileContent = new StreamContent(file.OpenReadStream(5 * 1024 * 1024));

                    fileContent.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                    content.Add(fileContent, "file", file.Name);
                }

                var response = await _httpClient.PutAsync(BaseUrl, content);

                var result = CheckResponsePost(response);

                if (!result.Flag)
                {
                    return new ServiceResponseGeneralPost<ViewItemExtensionDto>
                    {
                        Flag = false,
                        Message = result.Message
                    };
                }

                var apiResponse = await RedContent(response);

                var created = General.DeserialzeJsonString<ServiceResponseGeneralPost<ViewItemExtensionDto>>(apiResponse);

                return created;
            }
            catch (Exception ex)
            {
                return new ServiceResponseGeneralPost<ViewItemExtensionDto>
                {
                    Flag = false,
                    Message = ex.Message
                };
            }
        }
    }
}
