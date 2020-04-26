using Microsoft.AspNetCore.Mvc;
using UserManagementLiteDb.Models;
using UserManagementLiteDb.Services;
using UserManagementLiteDb.ViewModels;

namespace UserManagementLiteDb.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Index()
        {
            var users = _userService.GetAll();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userToCreate = new User
                {
                    Email = model.Email,
                    FullName = model.FullName,
                };

                var result = _userService.Insert(userToCreate, model.Password);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //[HttpPost]
        //public ActionResult<WeatherForecast> Insert(WeatherForecast dto)
        //{
        //    var id = _forecastDbService.Insert(dto);
        //    if (id != default)
        //        return CreatedAtRoute("FindOne", new { id = id }, dto);
        //    else
        //        return BadRequest();
        //}
    }
}