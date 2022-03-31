using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    using Infrastructure;

    public class HomeController : Controller
    {
        public ActionResult Index(int company_id = 1)
        {
            this.ViewBag.Title = "Home Page";
            var orders = OrderService.GetOrdersForCompany(company_id);

            return View(orders);
        }
    }
}
