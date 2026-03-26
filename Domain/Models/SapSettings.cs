using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SapSettings
    {
        public string BaseUrl { get; set; }
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
