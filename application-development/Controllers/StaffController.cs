using application_development.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using application_development.ViewModel;
using System.Threading.Tasks;

namespace application_development.Controllers
{
    public class StaffController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public StaffController()

        {
            _context = new ApplicationDbContext();
        }
        public StaffController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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
        // Staff manage trainee

        [Authorize(Roles ="Staff")]
        public ActionResult AllTrainee()
        {
            return View(_context.Trainees.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult CreateTrainee()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> CreateTrainee(CreateTrainee model)
        {
            if (ModelState.IsValid)
            {
                var user = new Trainee { UserName = model.Email, Email = model.Email, TraineeName = model.TraineeName, Age = model.Age, DateOfBirth = model.DateOfBirth, Education = model.Education, TOEICscore = model.TOEICscore, Programming = model.Programming, Department = model.Department, Exprience_details = model.Exprience_details };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Trainee");
                    return RedirectToAction("AllTrainee");
                }
                AddErrors(result);
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult EditTrainee(string Id)
        {
            return View(_context.Trainees.SingleOrDefault(t => t.Id == Id));
        }
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult EditTrainee(Trainee model)
        {
            Trainee trainee = _context.Trainees.SingleOrDefault(t => t.Id == model.Id);
            trainee.TraineeName = model.TraineeName;
            trainee.Age = model.Age;
            trainee.DateOfBirth = model.DateOfBirth;
            trainee.Education = model.Education;
            trainee.Programming = model.Programming;
            trainee.TOEICscore = model.TOEICscore;
            trainee.Department = model.Department;
            trainee.Exprience_details = model.Exprience_details;
            _context.SaveChanges();
            return RedirectToAction("AllTrainee");
        }

        [Authorize(Roles = "Staff")]
        public ActionResult DeleteTrainee(string Id)
        {
            _context.Trainees.Remove(_context.Trainees.SingleOrDefault(t => t.Id == Id));
            _context.SaveChanges();
            return RedirectToAction("AllTrainee");
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult ChangeTraineePwd(string Id)
        {
            EditUserPassword model = new EditUserPassword()
            {
                Id = Id,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> ChangeTraineePwd(EditUserPassword model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(model.Id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("AllTrainee", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }
        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }
    }
}