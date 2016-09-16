using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisVest.DomainModel.Abstract;

namespace SisVest.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IAdimRepository _adimRepository;

        public AdminController(IAdimRepository adimRepository)
        {
            _adimRepository = adimRepository;
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(_adimRepository.Admins.ToList());
        }
    }
}