using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.DTOs.APIResponse
{
    public class APIResponseDto
    {
        public object? ResponseObject { get; set; } = null;
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set;  }=string.Empty;
    }
}
