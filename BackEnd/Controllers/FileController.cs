using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers {
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class FileController : ControllerBase {
    private readonly IHostingEnvironment _hostingEnvironment;
    public FileController (IHostingEnvironment hostingEnvironment) {
      this._hostingEnvironment = hostingEnvironment;
    }
    [HttpPost("uploadFile")]
    public async Task<IActionResult> UploadFile ([FromForm] FileManagerModel model) {
      try {
        var fileName = ContentDispositionHeaderValue.Parse(model.file.ContentDisposition)
                .FileName
                .Trim('"');
        string filePath = "\\" + model.FilePath + "\\" + DateTime.Now.Millisecond + "-" + fileName;
        var fullFilePath = _hostingEnvironment.WebRootPath + "\\asset\\" + filePath;
        using (var stream = System.IO.File.Create(fullFilePath)) {
          await model.file.CopyToAsync(stream);
        }
        return Ok(filePath);
      } catch (Exception error) {
        Console.WriteLine(error);
        return Ok("");
      }
    }
  }
}