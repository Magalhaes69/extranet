using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Theme { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Language { get; set; }
        public bool TwoFactorEnabled { get; set; } = false;
    }
}
