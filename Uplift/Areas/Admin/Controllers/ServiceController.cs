using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostEnvironment;

        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API Calls

        public IActionResult GetAll()
        {
            return Json(new {data = _unitOfWork.Service.GetAll(includeProperties: "Category,Frequency")});
        }

        #endregion

        public IActionResult Upsert()
        {
            return View();
        }
    }
}