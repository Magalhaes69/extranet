using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public sealed record UserDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Nuit { get; set; }
        public string CardCode { get; set; }
    }
}
