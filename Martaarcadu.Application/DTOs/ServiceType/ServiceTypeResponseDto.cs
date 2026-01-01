using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.DTOs.ServiceType
{
    public class ServiceTypeResponseDto
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
    }
}
