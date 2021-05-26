using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using msStorage.Interceptor;
using System.Collections.Generic;
using System.Threading.Tasks;
using msStorage.Entities;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;

using Azure.Storage.Blobs.Models;
using ProblemDetails = msStorage.Entities.ProblemDetails;
using NotFoundResult = msStorage.Entities.NotFoundResult;

namespace msStorage.Controllers
{
    [TypeFilter(typeof(InterceptorLogAttribute))]
    [ApiController]
    [Route("/api/v1/[controller]")]

    public class StorageController : Controller
    {
        public readonly IConfiguration _iConfig;
        public StorageController(IConfiguration _Configuration)
        {
            _iConfig = _Configuration;
        }

        [HttpPost("PdfDownloadGetNombre")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Azure.Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public  async Task<ActionResult<Azure.Response>> PdfDownloadGetNombre(string nombreArchivo, string downloadPath)
        {
            try
            {
                if (nombreArchivo == null || downloadPath == null) return BadRequest(ModelState);
                StorageService miStorageService = new StorageService(_iConfig);
                var salida = await miStorageService.PdfGetNombre( nombreArchivo,  downloadPath);
                if (salida == null) return NotFound();
                return Ok(salida);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        [HttpPost("PdfGetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Azure.Page<BlobItem>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<IEnumerable<Azure.Page<BlobItem>>>> PdfGetAll()
        {
            try
            {
                StorageService miStorageService = new StorageService(_iConfig);
                var salida =  await miStorageService.PdfGetAll();
                if (salida == null) return NotFound();
                return Ok(salida);
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        [HttpPost("PdfSave")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Azure.Response<Azure.Storage.Blobs.Models.BlobContentInfo>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<Task<Azure.Response<Azure.Storage.Blobs.Models.BlobContentInfo>>>> PdfSave(string filePath)
        {
            try
            {
                if (filePath == null) return BadRequest(ModelState);

                StorageService miStorageService = new StorageService(_iConfig);
                var salida = await miStorageService.PdfSave(filePath);
                if (salida == null) return NotFound();
                return Ok(salida);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /* 

           [HttpDelete("ImagenDelete")]
           [ProducesResponseType(StatusCodes.Status200OK)]
           [ProducesResponseType(StatusCodes.Status400BadRequest)]
           [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
           public async Task<ActionResult> ImagenDelete(int id)
           {
               if (id <= 0) return BadRequest(ModelState);
               //await _repository.Delete(new TipoComercio { IdTipoComercio = id });
               return NoContent();
           }*/
    }
}
