using SoftSolution.Database;
using SoftSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SoftSolution.Menaxheret
{
    public class KerkesMenaxher
    {
        public KerkesStore Store { get; }
        public KerkesMenaxher(KerkesStore store)
        {
            Store = store;
        }
        public async Task<KerkesResult> KrijoKerkeseAsync(Kerkese kerkes)
        {

            var ekzistonRespons = await Store.KontrolloPerTitullDheDate(kerkes.Titulli,kerkes.DataKerkeses);
            if (!ekzistonRespons.IsSuccess)
            {
                return ekzistonRespons;
            }

            var result = await Store.KrijoKerkesAsync(kerkes);
            return result;
        }

        public async Task<Kerkese> MerrKerkesMeIdAsync(int id)
        {
            var result = await Store.MerrKerkesMeIdAsync(id);
            return result;
        }


        public async Task<IList<Kerkese>> MerrListMeKerkesatAsync(int page)
        {
            var result = await Store.MerrListMeKerkesatAsync(page);
            return result;
        }



        public async Task<KerkesResult> NdryshoKerkesenAsync(Kerkese kerkese)
        {
            var result = await Store.NdryshoKerkesenAsync(kerkese);
            return result;
        }


        public async Task<KerkesResult> FshiKerkesenMeIdAsync(int id)
        {
            var result = await Store.FshijKerkesenMeIdAsync(id);
            return result;
        }


        public async Task<IList<Kerkese>> KerkoKerkes(string filter, int numriFaqes) {

            return await Store.KerkoKerkese(filter, numriFaqes);
        
        }


    }
}