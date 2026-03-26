using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.User.Auth
{
    public sealed record LoginResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string CardCode { get; set; }
        public string UniqueName { get; set; }
    }
}
