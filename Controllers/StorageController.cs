using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using msStorage.Interceptor;
using System.Collections.Generic;
using System.Threading.Tasks;
using msStorage.Entities;
using Microsoft.Extensions.Configuration;

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

        [HttpPost("ImagenGetId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public  ActionResult<string> ImagenGetId(string id)
        {
            try
            {
                if (id == null) return BadRequest(ModelState);
                StorageService miStorageService = new StorageService(_iConfig);
                var salida = miStorageService.ImagenGetId(id);
                if (salida == null) return NotFound();
                return Ok(salida);
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        [HttpPost("ImagenGetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public ActionResult<string> ImagenGetAll()
        {
            try
            {
                StorageService miStorageService = new StorageService(_iConfig);
                var salida = miStorageService.ImagenGetAll();
                if (salida == null) return NotFound();
                return Ok(salida);
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        [HttpPost("ImagenSave")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public ActionResult<Task<Azure.Response<Azure.Storage.Blobs.Models.BlobContentInfo>>> ImagenSave(string filePath)
        {
            try
            {
        
                if (filePath == null) return BadRequest(ModelState);

                StorageService miStorageService = new StorageService(_iConfig);
                var salida = miStorageService.ImagenSave(filePath);
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
