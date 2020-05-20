using SoftSolution.Database;
using SoftSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftSolution.Controllers
{
    [RoutePrefix("listakerkesave")]
    public class ListaKerkesaveController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {

            return View("listakerkesave");
        }



        [HttpGet]
        [Route("")]
        [Route("MerrListenMeKerkesa")]
        public ActionResult MerrListenMeKerkesa()
        {
         
            return View("listakerkesave");
        }

    }
}