
using SoftSolution;
using SoftSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.WebPages;

namespace SoftSolution.Models
{
    public class Kerkese
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string Titulli { get; set; }
        public string Pershkrimi { get; set; }
        public DateTime DataKerkeses { get; set; }
        public DateTime DataRegjistrimit { get; set; }
        public DateTime? DataPerfundimit { get; set; }
        public string EmriDokumentit { get; set; }

        public virtual Status Status { get; set; }

        public Kerkese()
        {

        }

        public Kerkese(Kerkesat kerkesat)
        {
            Id = kerkesat.Id;
            StatusId = kerkesat.StatusId;
            Titulli = kerkesat.Titulli.TrimEnd();
            Pershkrimi = kerkesat.Pershkrimi;
            DataKerkeses = kerkesat.DataKerkeses;
            DataRegjistrimit = kerkesat.DataRegjistrimit;
            DataPerfundimit = kerkesat.DataPerfundimit;
            EmriDokumentit = kerkesat.DokumentPath;
            Status = new Status
            {
                Id = kerkesat.Statuset.Id_,
                Emri = kerkesat.Statuset.Emri,
                Pershkrimi = kerkesat.Statuset.Pershkrimi
            };
        }

        public Kerkese(MultipartFormDataStreamProvider formData)
        {



            string dokumentName = null;

            MultipartFileData file = null;

            if (formData.FileData.Count > 0)
            {
                file = formData.FileData.First();
                dokumentName = file.Headers.ContentDisposition.FileName;
                dokumentName = dokumentName.Trim('"');
            }

            // remove " from name


            var data = formData.FormData;

            Id = Int32.Parse(data.GetValues("Id").First());

            Titulli = data.GetValues("Titulli").First();
            StatusId = Int32.Parse(data.GetValues("StatusId").First());
            Pershkrimi = data.GetValues("Pershkrimi").First();
            if (Pershkrimi == "") Pershkrimi = null;
            DataRegjistrimit = DateTime.Now;
            EmriDokumentit = dokumentName;

            var d = data.GetValues("DataKerkeses").First();
            DataKerkeses = DateTime.Parse(d);

            var p = data.GetValues("DataPerfundimit").First();

            DataPerfundimit = Convert.ToDateTime(p);

        }

    }

}