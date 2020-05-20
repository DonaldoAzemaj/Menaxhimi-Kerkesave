using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace SoftSolution.Models
{
    public class NdryshoKerkesRequest : KerkesRequest
    {
         public NdryshoKerkesRequest (MultipartFormDataStreamProvider provider)
            :base(provider)
        {

        }


        public override void ValidoId()
        {
            if (String.IsNullOrEmpty(Id))
                Errors.Add(nameof(Id), "Id nuk mund te jete null.");

        }

        public Kerkese MerrKerkesen()
        {
            int statusId = Int32.Parse(StatusId);
            return new Kerkese
            {
                Id = Int32.Parse(Id),
                StatusId = Int32.Parse(StatusId),
                Titulli = Titulli,
                Pershkrimi = Pershkrimi,
                DataKerkeses = DateTime.Parse(DataKerkeses),
                DataRegjistrimit = DateTime.Now,
                DataPerfundimit = statusId == (int)StatusCode.Perfunduar
                    ? (DateTime?)Convert.ToDateTime(DataPerfundimit)
                    : null,
                EmriDokumentit = Dokumenti == null ? null : Dokumenti.Headers.ContentDisposition.FileName.Trim('"'),
            };
        }


       

    }
}