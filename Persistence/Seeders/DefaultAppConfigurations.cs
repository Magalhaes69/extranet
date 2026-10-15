
using Domain.Helps.Enum;
using Domain.Models;

namespace Persistence.Seeders
{
    public static class DefaultAppConfigurations
    {
        public static List<AppSetting> GetAppSettings()
        {
            return new()
            {
                new()
                {
                    Key="Company.Name",
                    Value = "My company",
                    Category = "General",
                    Description = "Company name to be used in the application",
                    DataType = ConfigDataType.Text,
                    IsEncrypted = false,
                    IsVisible = true,
                    UserId = string.Empty,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                },
                new()
                {
                    Key="Company.Image",
                    Value = "",
                    Category = "General",
                    Description = "Image to be used in the application",
                    DataType = ConfigDataType.Text,
                    IsEncrypted = false,
                    IsVisible = true,
                    UserId = string.Empty,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                },
                new()
                {
                    Key="Company.Icon",
                    Value = "",
                    Category = "General",
                    Description = "Image to be used in the application",
                    DataType = ConfigDataType.Text,
                    IsEncrypted = false,
                    IsVisible = true,
                    UserId = string.Empty,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                },
                new()
                {
                    Key="Email.Host",
                    Value = "",
                    Category = "Email",
                    Description = "SMTP host for sending emails",
                    DataType= ConfigDataType.Text,
                    UserId = string.Empty,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                },
                new()
                {
                    Key="Email.Port",
                    Value = "",
                    Category = "Email",
                    Description = "SMTP port for sending emails",
                    DataType= ConfigDataType.Number,
                    UserId = string.Empty,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                },
                new()
                {
                    Key="Email.Password",
                    Value = "",
                    Category = "Email",
                    Description = "Password",
                    DataType= ConfigDataType.Text,
                    IsEncrypted=true,
                    UserId = string.Empty,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                },
                new()
                {
                    Key="Sales.UseCatalog",
                    Value = "",
                    Category = "Sales",
                    Description = "",
                    DataType= ConfigDataType.Boolean,
                    IsEncrypted=false,
                    UserId = string.Empty,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                },

                new()
                {
                    Key = "Security.SessionTimeout",
                    Value = "30",
                    Category = "Security",
                    Description = "Session timeout in minutes",
                    DataType = ConfigDataType.Number,
                    UserId = string.Empty,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                }
            };
        }
    }
}
