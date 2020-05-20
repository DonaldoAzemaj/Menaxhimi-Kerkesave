using SoftSolution.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SoftSolution.Models;

namespace SoftSolution.Controllers
{


    [RoutePrefix("MenaxhimiKerkesave")]
    public class MenaxhimiKerkesaveController : Controller
    {


        [HttpGet]
        [Route("")]
        public ActionResult kryefaqa()
        {
            return View("menaxhimkerkesave", new Kerkese { });
        }





        //MenaxhimiKerkesave/modifiko/{id}
        [HttpGet]
        [Route("modifiko/{id}")]
        public ActionResult Modifiko(int id)
        {

            return View("menaxhimkerkesave");
        }

    }
}