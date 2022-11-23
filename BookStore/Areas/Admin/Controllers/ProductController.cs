using BookStore.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        //dependency injection
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {

           
            return View();
        }
       
        //get
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            }; 

            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;

                return View(productVM);
            }
            else
            {
                //update product
            }
            
           
            return View(productVM); 
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj ,IFormFile? file)
        {
           
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName=Guid.NewGuid().ToString();
                    var uploads=Path.Combine(wwwRootPath, @"images\products");
                    var extension=Path.GetExtension(file.FileName);
                    //to update and save in db
                    using(var filestreams=new FileStream(Path.Combine(uploads,fileName+extension),FileMode.Create))
                    {
                        file.CopyTo(filestreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product added successfully";
                return RedirectToAction("Index");

            }
            return View(obj);
        }

        //get
        public IActionResult Delete(int? id)
        {


            if (id == null || id == 0)
            {
                return NotFound();
            }
            var CoverTypeFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (CoverTypeFromDb == null)
            {
                return NotFound();
            }
            return View(CoverTypeFromDb);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {

                _unitOfWork.CoverType.Remove(obj);
                _unitOfWork.Save();
                TempData["success"] = "CoverType Deleted successfully";
                return RedirectToAction("Index");

            }
            return View(obj);
        }


    }
    //#region API CALLS
    //[HttpGet]
    //public IActionResult GetAll()
    //{
    //    var productList = _unitOfWork.Product.GetAll();
    //    return Json(new { data = productList });
    //}

}


