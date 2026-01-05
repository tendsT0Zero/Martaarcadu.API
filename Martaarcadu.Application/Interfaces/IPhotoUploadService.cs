using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.Interfaces
{
    public interface IPhotoUploadService
    {
        Task<string> UploadPhotoAsync(IFormFile file);
    }
}
