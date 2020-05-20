using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace SoftSolution.Models
{
    public class KrijoKerkesRequest : KerkesRequest
    {



        public KrijoKerkesRequest(MultipartFormDataStreamProvider provider)
            :base(provider)
        {
        }

    

        public Kerkese MerrKerkesen()
        {
            int statusId = Int32.Parse(StatusId);
            return new Kerkese
            {
                StatusId = statusId,
                Titulli = Titulli,
                Pershkrimi = Pershkrimi,
                DataKerkeses = Convert.ToDateTime(DataKerkeses),
                DataRegjistrimit = DateTime.Now,
                DataPerfundimit = statusId == (int)StatusCode.Perfunduar 
                    ?(DateTime?)Convert.ToDateTime(DataPerfundimit) 
                    : null,
                EmriDokumentit = Dokumenti == null ? null : Dokumenti.Headers.ContentDisposition.FileName.Trim('"'),
            };
        }
    }
}