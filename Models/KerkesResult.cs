using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftSolution.Models
{
    public class KerkesResult
    {
        public static KerkesResult Success { get; } = new KerkesResult
        {
            IsSuccess = true
        };
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string Code { get; set; }
    }
}