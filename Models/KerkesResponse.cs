using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace SoftSolution.Models
{
    public class KerkesResponse
    {
       public bool MeSukses { get; set; }
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();


        public KerkesResponse() { }

        public KerkesResponse(KerkesResult kerkesResult)
        {
            this.MeSukses = kerkesResult.IsSuccess;
            this.Errors.Add(kerkesResult.Code,kerkesResult.Message);
        
        
        }



    }
}