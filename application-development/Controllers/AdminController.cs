using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using application_development.Models;
using Microsoft.AspNet.Identity.Owin;
using application_development.ViewModel;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace application_development.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;

        public AdminController()

        {
            _context = new ApplicationDbContext();
        }
        public AdminController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        //Manage Trainer
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AllTrainer()
        {
            
            return View(_context.Trainers.ToList());
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateTrainer()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateTrainer(CreateTrainer model)
        {
            if (ModelState.IsValid)
            {
                var user = new Trainer { UserName = model.Email, Email = model.Email, WorkPlace = model.WorkPlace, Type = model.Type, TrainerName = model.TrainerName };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Trainer");
                    return RedirectToAction("AllTrainer");
                }
                AddErrors(result);
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditTrainer(string Id)
        {
            return View(_context.Trainers.SingleOrDefault(t => t.Id == Id));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditTrainer(Trainer model)
        {
            Trainer trainer = _context.Trainers.SingleOrDefault(t => t.Id == model.Id);
            trainer.TrainerName = model.TrainerName;
            trainer.Type = model.Type;
            trainer.WorkPlace = model.WorkPlace;
            _context.SaveChanges();
            return RedirectToAction("AllTrainer");
        }
        [Authorize(Roles ="Admin")]
        public ActionResult DeleteTrainer(string Id)
        {
            _context.Trainers.Remove(_context.Trainers.SingleOrDefault(t => t.Id == Id));
            _context.SaveChanges();
            return RedirectToAction("AllTrainer");
        }

        //Manage Staff
        [Authorize(Roles = "Admin")]
        public ActionResult AllStaff()
        {
            return View(_context.Staffs.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateStaff()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateStaff(CreateStaff model)
        {
            if (ModelState.IsValid)
            {
                var user = new Staff { UserName = model.Email, Email = model.Email,  StaffName = model.StaffName};

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Staff");
                    return RedirectToAction("AllStaff");
                }
                AddErrors(result);
            }

            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditStaff(string Id)
        {
            return View(_context.Staffs.SingleOrDefault(t => t.Id == Id));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditStaff(Staff model)
        {
            var staff = _context.Staffs.SingleOrDefault(t => t.Id == model.Id);
            staff.StaffName = model.StaffName;
            _context.SaveChanges();
            return RedirectToAction("AllStaff");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteStaff(string Id)
        {
            _context.Staffs.Remove(_context.Staffs.SingleOrDefault(t => t.Id == Id));
            _context.SaveChanges();
            return RedirectToAction("AllStaff");
        }

        [Authorize(Roles ="Admin")]
        public ActionResult ChangeUserPassword(string Id)
        {
            EditUserPassword model = new EditUserPassword()
            {
                Id = Id,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeUserPassword(EditUserPassword model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(model.Id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
               if(_context.Trainers.SingleOrDefault(t => t.Id == model.Id) != null)
                {
                    return RedirectToAction("AllTrainer", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
               else {
                    return RedirectToAction("AllStaff", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
            }
            AddErrors(result);
            return View(model);
        }
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error
        }

    }
}