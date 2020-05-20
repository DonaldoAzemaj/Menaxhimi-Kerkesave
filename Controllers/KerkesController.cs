
using SoftSolution.Database;
using SoftSolution.Menaxheret;
using SoftSolution.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Services.Description;

namespace SoftSolution.Controllers
{
    [RoutePrefix("api/kerkesat")]
    public class KerkesController : ApiController
    {
        HttpContext _httpContext { get; }
        public string TempPath => _httpContext.Server.MapPath("~/App_Data/temp/");
        public string DokumentetPath => _httpContext.Server.MapPath("~/App_Data/Dokumentet/");

        public KerkesMenaxher KerkesMenaxher { get; }


        public KerkesController(KerkesMenaxher kerkesMenaxher)
        {
            KerkesMenaxher = kerkesMenaxher;
            _httpContext = HttpContext.Current;
        }


        // api/kerkesat/faqa/{numriFaqes}
        [HttpGet]
        [Route("faqa/{numriFaqes}")]

        public async Task<IList<Kerkese>> MerrListMeKerkesa(int numriFaqes)
        {
            if (numriFaqes < 1)
                numriFaqes = 1;

            var kerkesat = await KerkesMenaxher.MerrListMeKerkesatAsync(numriFaqes);

            return kerkesat;

        }


        // api/kerkesat/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<Kerkese> MerrKerkesMeId(int id) {


            var kerkesa = await KerkesMenaxher.MerrKerkesMeIdAsync(id);

            return kerkesa;
        
        }


        // api/kerkesat/search/{name}/numriFaqes
        [HttpGet]
        [Route("search/{filter}/{numriFaqes}")]
        public async Task<IList<Kerkese>> KerkoKerkes(string filter,int numriFaqes)
        {
            if (numriFaqes < 1)
                numriFaqes = 1;

            var Kerkese = await KerkesMenaxher.KerkoKerkes(filter,numriFaqes);

            return Kerkese;
        }



        // api/kerkesat/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<KerkesResponse> DeleteKerkesen(int id)
        {

            var result = await KerkesMenaxher.FshiKerkesenMeIdAsync(id);

            if (!result.IsSuccess)
            {
                return new KerkesResponse(result);
            }

            DeleteFileMeId(id);

            return new KerkesResponse { MeSukses = result.IsSuccess};
        }






        // api/kerkesat/
        [Route("")]
        [HttpPut]
        public async Task<KerkesResponse> NdryshoKerkesen()
        {
            var provider = new MultipartFormDataStreamProvider(TempPath);
            await Request.Content.ReadAsMultipartAsync(provider);

            var kerkesRequest = new NdryshoKerkesRequest(provider);
            kerkesRequest.Valido();

            if (kerkesRequest.Errors.Count()>0)
            {
                DeleteTempFile(provider);
                return new KerkesResponse
                {
                    MeSukses = false,
                    Errors = kerkesRequest.Errors
                };
            }

            var kerkesa = kerkesRequest.MerrKerkesen();

            var result = await KerkesMenaxher.NdryshoKerkesenAsync(kerkesa);


            if (!result.IsSuccess)
            {
                DeleteTempFile(provider);
                return new KerkesResponse(result);
            }


            SaveFile(provider, kerkesa);

            return new KerkesResponse
            {
                MeSukses = result.IsSuccess
            };

        }


        // api/kerkesat/
        [Route("")]
        [HttpPost]
        public async Task<KerkesResponse> KrijoKerkesen()
        {

            var provider = new MultipartFormDataStreamProvider(TempPath);
            await Request.Content.ReadAsMultipartAsync(provider);

            var kerkesRequest = new KrijoKerkesRequest(provider);

            kerkesRequest.Valido();

            if(kerkesRequest.Errors.Count() > 0)
            {
                DeleteTempFile(provider);
                return new KerkesResponse
                {
                    MeSukses = false,
                    Errors = kerkesRequest.Errors
                };
            }

            var kerkesa = kerkesRequest.MerrKerkesen();

            var result = await KerkesMenaxher.KrijoKerkeseAsync(kerkesa);


            if (!result.IsSuccess)
            {
                DeleteTempFile(provider);
                return new KerkesResponse(result);
            }


            SaveFile(provider,kerkesa);

            return new KerkesResponse
            {
                MeSukses = result.IsSuccess
            };


        }


        //api/kerkesat/{id}/dokument
        [Route("{id}/dokument")]
        [HttpGet]
        public async Task<HttpResponseMessage> ShkarkoDokumentin(int id)
        {

            var kerkesa = await KerkesMenaxher.MerrKerkesMeIdAsync(id);
            if(kerkesa == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var result = new HttpResponseMessage(HttpStatusCode.OK);


            var filePath = _httpContext.Server.MapPath($"~/App_Data/Dokumentet/{id}");



            if (File.Exists(filePath))
            {

                var fileBytes = File.ReadAllBytes(filePath);
                FileInfo fileinfo = new FileInfo(filePath);

                var fileStream = new MemoryStream(fileBytes);
                result.Content = new StreamContent(fileStream);

                var headers = result.Content.Headers;

                headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                headers.ContentDisposition.FileName = kerkesa.EmriDokumentit;
                var ext = (new FileInfo(kerkesa.EmriDokumentit)).Extension.Trim('.');
                headers.ContentType = new MediaTypeHeaderValue($"application/{ext}");
                headers.ContentLength = fileStream.Length;

            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }


            return result;

        }



        public void DeleteTempFile(MultipartFormDataStreamProvider provider)
        {

            if (provider.FileData.Count() < 1)
                return;

            var tempPath = provider.FileData.First().LocalFileName;

            FileInfo file = new FileInfo(tempPath);
            file.Delete();

        }

        public void DeleteFileMeId(int id)
        {

            string documentPath = Path.Combine(DokumentetPath, id.ToString());
            FileInfo fi = new FileInfo(documentPath);
            if (fi.Exists)
                fi.Delete();
        }


        private void SaveFile(MultipartFormDataStreamProvider provider, Kerkese kerkesa)
        {
            if (provider.FileData.Count() < 1)
                return;

            var tempPath = provider.FileData.First().LocalFileName;


            string documentPath = Path.Combine(DokumentetPath, kerkesa.Id.ToString());
            FileInfo fi = new FileInfo(documentPath);
            if (fi.Exists)
                fi.Delete();
            File.Move(tempPath, documentPath);

        }



    }
}