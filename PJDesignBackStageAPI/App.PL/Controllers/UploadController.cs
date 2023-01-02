using App.Common;
using App.Model;
using App.PL.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace App.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        /// <summary>
        /// 圖片上傳
        /// </summary>
        [HttpPost]
        [Route("UploadPhoto")]
        [JwtFilter]
        public async Task<ResponseBase<string>> UploadPhoto([FromForm] IFormFile image)
        {
            if (image == null)
            {
                throw new Exception("請求錯誤");
            }

            var response = new ResponseBase<string>();
            var hostUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}"; ;
            var fileLength = image.Length;
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Images");

            if (fileLength > 0)
            {
                var fileName = DateHelper.GetNowDate().ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid() + Path.GetExtension(image.FileName);
                var fullPath = Path.Combine(pathToSave, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                response.Entries = hostUrl + "/Images/" + fileName;
            }
            else
            {
                throw new Exception("File length is 0.");
            }

            return response;
        }
    }
}
