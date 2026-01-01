using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Domain.Entities.Service
{
    public  class ServiceType
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public  ICollection<ApplicationUser.ApplicationUser>? Users { get; set; }

    }
}
