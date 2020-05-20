using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.ModelBinding;

namespace SoftSolution.Models
{
    public class KerkesRequest
    {


        public string Id { get; set; }
        public string StatusId { get; set; }
        public string Titulli { get; set; }
        public string Pershkrimi { get; set; }
        public string DataKerkeses { get; set; }
        public string DataPerfundimit { get; set; }

        public MultipartFileData Dokumenti { get; set; }

        public Dictionary<string, string> Errors = new Dictionary<string, string>();

        public KerkesRequest(MultipartFormDataStreamProvider provider)
        {

            Id = provider.FormData.GetValues("Id").FirstOrDefault();
            Titulli = provider.FormData.GetValues("Titulli").FirstOrDefault();
            StatusId = provider.FormData.GetValues("StatusId").FirstOrDefault();
            Pershkrimi = provider.FormData.GetValues("Pershkrimi").FirstOrDefault();
            Dokumenti = provider.FileData.FirstOrDefault();
            DataKerkeses = provider.FormData.GetValues("DataKerkeses").FirstOrDefault();
            DataPerfundimit = provider.FormData.GetValues("DataPerfundimit").FirstOrDefault();

        }

        public void Valido()
        {

            ValidoId();

            ValidoTitulli();

            ValidoStatusId();

            ValidoDataKerkeses();

            ValidoDataPerfundimit();

            ValidoDokument();
        }

        public virtual void ValidoId(){}

        public void ValidoTitulli()
        {
            if (String.IsNullOrEmpty(Titulli))
                Errors.Add(nameof(Titulli), "Titulli nuk mund te jet bosh.");
        }


        public void ValidoStatusId()
        {
            string errorMessage = null;
            if (!Int32.TryParse(StatusId, out int id))
                errorMessage= "Statusi nuk eshte valid";
            else if (id < 1 || id > 3)
                errorMessage ="Duhet te zgjidhni statusin.";

            if (errorMessage != null)
                Errors.Add(nameof(StatusId), errorMessage);
           
        }


        public void ValidoDataKerkeses()
        {
            string errorMessage = null;
            if (DataKerkeses == null)
                errorMessage = "Data e kerkeses nuk mund te jet bosh.";


            if (!DateTime.TryParse(DataKerkeses, out DateTime date))
                errorMessage = "Data e kerkese nuk eshte e sakte.";

            if (errorMessage != null)
                Errors.Add(nameof(DataKerkeses), errorMessage);

        }



        public void ValidoDataPerfundimit()
        {
            string errorMessage = null;

            if (StatusCode.TryParse(StatusId, out StatusCode statusId)
                && statusId == StatusCode.Perfunduar)
            {
                if (DataPerfundimit == null)
                    errorMessage = "Data e perfundimit nuk mund te jet bosh.";
                else if (DateTime.TryParse(DataKerkeses, out DateTime dataKerkeses))
                {
                    if (!DateTime.TryParse(DataPerfundimit, out DateTime dataPerfundimit))
                        errorMessage = "Data e perfundimit nuk eshte e sakte.";
                    else if (dataPerfundimit <= dataKerkeses)
                        errorMessage = "Data e perfundimit duhet te jet me e madhe se data kerkeses.";

                }
            }
            


            if (errorMessage != null)
                Errors.Add(nameof(DataPerfundimit), errorMessage);
        }

        public void ValidoDokument()
        {
            if(Dokumenti !=null)
            {
                string errorMessage = null;
                var fileName = Dokumenti.Headers.ContentDisposition.FileName.Replace("\"","");

                if (String.IsNullOrEmpty(fileName))
                    return;

                FileInfo fi = new FileInfo(fileName);
                string ext = fi.Extension.ToLowerInvariant();


                switch (ext)
                {
                    case ".pdf":
                    case ".docx":
                    case ".doc":
                        break;
                    default:
                        errorMessage = "Tip file i pa suportuar";
                        break;
                }
                if (errorMessage != null)
                    Errors.Add(nameof(Dokumenti), errorMessage);
            }
            
            
        }

    }
}