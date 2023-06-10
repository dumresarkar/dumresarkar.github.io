using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompanySite.Controllers
{
    public class PolicyController : BaseController
    {
        // GET: Policy
        public ActionResult Privacy()
        {
            MEB_BREADCRUMB.Add("", "Privacy Policy");
            return View();
        }
    }
}