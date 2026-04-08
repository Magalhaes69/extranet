using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class GetAttachments2
    {
        public Stream Stream { get; set; } = default!;
        public string? ContentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
