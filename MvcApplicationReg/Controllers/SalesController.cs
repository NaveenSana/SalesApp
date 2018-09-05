using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplicationReg.Models;

namespace MvcApplicationReg.Controllers
{
    public class SalesController : Controller
    {
        //
        // GET: /Sales/

        SalesModel objModel = new SalesModel();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSalesData(int Searchtext)
        {
            List<SalesData> salesdata = new List<SalesData>();
            salesdata = objModel.getSalesData(Searchtext);
            return Json(salesdata);            
        }

        [HttpPost]
        public ActionResult GetUserSalesData(int Searchtext)
        {
            List<SalesData> salesdata = new List<SalesData>();
            salesdata = objModel.getUserSalesData(Searchtext);
            return Json(salesdata);
        }

        [HttpPost]
        public ActionResult GetAllUserss()
        {
            List<SalesData> userslist = new List<SalesData>();
            userslist = objModel.getallusers();
            Session["count"] = userslist.Count();
            return Json(userslist);
        }

    }
}
