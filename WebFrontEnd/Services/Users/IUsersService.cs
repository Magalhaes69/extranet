using WebFrontEnd.Services.Users.Dto;

namespace WebFrontEnd.Services.Users
{
    public interface IUsersService
    {
        /// <summary>Returns null when the list could not be loaded.</summary>
        Task<List<UserListItemDto>?> GetAllAsync();

        Task<ServiceResponsePost> CreateAsync(UserFormDto user);

        Task<ServiceResponsePost> UpdateAsync(string id, UserFormDto user);

        Task<ServiceResponsePost> SetStatusAsync(string id, bool isActive);
    }
}
