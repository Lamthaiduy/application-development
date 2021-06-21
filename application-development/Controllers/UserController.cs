using application_development.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace application_development.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;

        // GET: User

        public UserController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public UserController()
        {
            _context = new ApplicationDbContext();
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
        [Authorize(Roles = "Trainee")]
        public ActionResult AllTraineeCourses()
        {
            var userId = User.Identity.GetUserId();
            return View(_context.TraineeCourses.Where(t => t.TraineeId == userId).Include(t => t.Trainee).Include(t => t.Course).Include(t => t.Course.Category).ToList());
        }

        [Authorize(Roles = "Trainee")]
        public ActionResult AllCourses()
        {
            return View(_context.Courses.Include(t => t.Category).ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Trainee")]
        public ActionResult UpdateTraineeAccount()
        {
            var userId = User.Identity.GetUserId();
            return View(_context.Trainees.SingleOrDefault(t => t.Id == userId));
        }

        [HttpPost]
        [Authorize(Roles = "Trainee")]
        public ActionResult UpdateTraineeAccount(Trainee model)
        {
            var trainee = _context.Trainees.SingleOrDefault(t => t.Id == model.Id);
            trainee.TraineeName = model.TraineeName;
            trainee.Age = model.Age;
            trainee.DateOfBirth = model.DateOfBirth;
            trainee.Education = model.Education;
            trainee.Programming = model.Programming;
            trainee.TOEICscore = model.TOEICscore;
            trainee.Department = model.Department;
            trainee.Exprience_details = model.Exprience_details;
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Trainer")]
        public ActionResult AllTrainerCourses()
        {
            var userId = User.Identity.GetUserId();
            return View(_context.TrainerCourses.Where(t => t.TrainerId == userId).Include(t => t.Trainer).Include(t => t.Course).Include(t => t.Course.Category).ToList());
        }
        [HttpGet]
        [Authorize(Roles = "Trainer")]
        public ActionResult UpdateTrainerAccount()
        {
            var userId = User.Identity.GetUserId();
            return View(_context.Trainers.SingleOrDefault(t => t.Id == userId));
        }

        [HttpPost]
        [Authorize(Roles = "Trainer")]
        public ActionResult UpdateTrainerAccount(Trainer model)
        {
            var trainer = _context.Trainers.SingleOrDefault(t => t.Id == model.Id);
            trainer.TrainerName = model.TrainerName;
            trainer.Type = model.Type;
            trainer.WorkPlace = model.WorkPlace;
            trainer.PhoneNumber = model.PhoneNumber;
            trainer.Email = model.Email;
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}