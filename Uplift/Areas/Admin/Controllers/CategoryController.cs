using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert()
        {
            throw new System.NotImplementedException();
        }
    }
}