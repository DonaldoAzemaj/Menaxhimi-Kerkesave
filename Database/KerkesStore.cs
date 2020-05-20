using SoftSolution.Controllers;
using SoftSolution.Models;
using SoftSolution.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SoftSolution.Database
{
    public class KerkesStore
    {
        private int kerkesaPerFaqe = 20;
        public SSDBContext DBContext { get; }

        public KerkesStore(SSDBContext dbContext)
        {
            DBContext = dbContext;
        }



        public async Task<KerkesResult> KontrolloPerTitullDheDate(string titulli, DateTime dataKerkeses) {


            string query = "SELECT * FROM [dbo].[Kerkesat] Where Titulli=@titulli AND DataKerkeses=@dataKerkeses;";
            var titull = new SqlParameter("titulli", titulli);
            var data = new SqlParameter("dataKerkeses", dataKerkeses);
            var results = await DBContext.Kerkesat.SqlQuery(query, titull, data).ToListAsync();

            if (results.Count() < 1)
                return KerkesResult.Success;


            return new KerkesResult() {
                IsSuccess = false,
                Message = "Nuk lejohen dy tituj me date te njejte!",
                Code = "Titulli",
            };
                

        }


        public async Task<KerkesResult> KrijoKerkesAsync(Kerkese kerkes )
        {


            var kerkesat = new Kerkesat
            {
                Titulli = kerkes.Titulli,
                StatusId = kerkes.StatusId,
                Pershkrimi = kerkes.Pershkrimi,
                DataKerkeses = kerkes.DataKerkeses,
                DataPerfundimit = kerkes.DataPerfundimit,
                DataRegjistrimit = kerkes.DataRegjistrimit,
                DokumentPath = kerkes.EmriDokumentit
            };


            DBContext.Kerkesat.Add(kerkesat);

            int rowsAf = await DBContext.SaveChangesAsync();
            if (rowsAf < 1)
            {
                return new KerkesResult
                {
                    IsSuccess = false,
                    Message = "Problem me ruajtjen e kerkeses ne databaze!",
                    Code = "error",
                };
            }
            else{
                kerkes.Id = kerkesat.Id;
                return KerkesResult.Success;            
            }


        }

        public async Task<IList<Kerkese>> KerkoKerkese(string filter, int numriFaqes)
        {
            int skip = (numriFaqes - 1) * kerkesaPerFaqe;
            List<Kerkese> kerkesat = new List<Kerkese>();
            string query = "SELECT * FROM dbo.Kerkesat WHERE Titulli LIKE  CONCAT( @filter ,'%') ORDER BY Id DESC OFFSET @skip ROWS FETCH NEXT @kerkesaPerFaqe ROWS ONLY";
            var filterP = new SqlParameter("filter", filter);
            var skipP = new SqlParameter("skip", skip);
            var kerkesaPerFaqeP = new SqlParameter("kerkesaPerFaqe", kerkesaPerFaqe);
            var results = await DBContext.Kerkesat.SqlQuery(query,filterP,skipP,kerkesaPerFaqeP ).ToListAsync();

            foreach (var result in results)
            {
                kerkesat.Add(new Kerkese(result));

            }

            return kerkesat;

        }

        public async Task<Kerkese> MerrKerkesMeIdAsync(int id)
        {


            var result = await DBContext.Kerkesat.FindAsync(id);

            if (result == null)
                return null;


            return new Kerkese(result);

        }



        public async Task<IList<Kerkese>> MerrListMeKerkesatAsync(int page)
        {

            int skip = (page - 1) * kerkesaPerFaqe;
            List<Kerkese> kerkesat = new List<Kerkese>();
            string query = "SELECT * FROM dbo.Kerkesat ORDER BY Id DESC OFFSET @skip ROWS FETCH NEXT @kerkesaFaqe ROWS ONLY";
            var paramSkip = new SqlParameter("skip",skip);
            var paramKerkesaPerFaqe = new SqlParameter("kerkesaFaqe", kerkesaPerFaqe);
            var results =  await DBContext.Kerkesat.SqlQuery(query, paramSkip, paramKerkesaPerFaqe).ToListAsync();



            foreach (var result in results)
            {
                kerkesat.Add(new Kerkese
                {
                    Id = result.Id,
                    StatusId = result.StatusId,
                    Titulli = result.Titulli.TrimEnd(),
                    Pershkrimi = result.Pershkrimi,
                    DataKerkeses = result.DataKerkeses,
                    DataRegjistrimit = result.DataRegjistrimit,
                    DataPerfundimit = result.DataPerfundimit,
                    EmriDokumentit = result.DokumentPath,
                    Status = new Status
                    {
                        Id = result.Statuset.Id_,
                        Emri = result.Statuset.Emri,
                        Pershkrimi = result.Statuset.Pershkrimi
                    }

                });


            }

            return kerkesat;

        }



        public async Task<KerkesResult> NdryshoKerkesenAsync(Kerkese kerkese)
        {


            var resultKerkes = await DBContext.Kerkesat.FindAsync(kerkese.Id);

            if(resultKerkes==null)
                return new KerkesResult
                {
                    IsSuccess = false,
                    Message = "Kjo kerkes nuk Ekziston!",
                    Code = "error",
                };



            resultKerkes.StatusId = kerkese.StatusId;
            resultKerkes.Titulli = kerkese.Titulli;
            resultKerkes.Pershkrimi = kerkese.Pershkrimi;
            resultKerkes.DataKerkeses = kerkese.DataKerkeses;
            resultKerkes.DataRegjistrimit = kerkese.DataRegjistrimit;
            resultKerkes.DataPerfundimit = kerkese.DataPerfundimit;

            if (!String.IsNullOrEmpty(kerkese.EmriDokumentit))
            {
                resultKerkes.DokumentPath = kerkese.EmriDokumentit;
            }

             DBContext.SaveChanges();
          
            return KerkesResult.Success;

        }









        public async Task<KerkesResult> FshijKerkesenMeIdAsync(int id)
        {

            var Kerkesat = DBContext.Kerkesat.Attach(new Kerkesat { Id=id});
            DBContext.Kerkesat.Remove(Kerkesat);
            var result = await DBContext.SaveChangesAsync();

            if(result>0)
                 return new KerkesResult { IsSuccess = true };


            return new KerkesResult {
                IsSuccess = false,
                Message="Kjo kerkes nuk Ekziston!",
            };

        }


    }
}