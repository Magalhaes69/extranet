using WebFrontEnd.Services.Leads.Dto;

namespace WebFrontEnd.Services.Leads
{
    public interface ILeadsService
    {
        /// <summary>Creates the lead. The message of the result comes from the API when there is one.</summary>
        Task<ServiceResponsePost> CreateAsync(LeadFormDto lead);
    }
}
