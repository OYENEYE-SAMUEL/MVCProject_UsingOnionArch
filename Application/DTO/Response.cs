using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class Response<T>
    {
        public string? Message { get; set; }
        public bool Status { get; set; }
        public T? Value { get; set; } = default!;
    }
}
