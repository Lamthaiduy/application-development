using application_development.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using application_development.ViewModel;
using System.Threading.Tasks;
using System.Data.Entity;

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
        public ActionResult AllTrainee(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return View(_context.Trainees.ToList());
            }
            return View(_context.Trainees.Where(t => t.TraineeName.Contains(name)).ToList());
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
            ChangePasswordSuccess,
            Error
        }

        // manage trainer profile
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult AllTrainer()
        {
            return View(_context.Trainers.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult EditTrainerProfile(string Id)
        {
            return View(_context.Trainers.SingleOrDefault(t =>  t.Id == Id));
        }
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult EditTrainerProfile(Trainer model)
        {
            var trainer = _context.Trainers.SingleOrDefault(t => t.Id == model.Id);
            trainer.TrainerName = model.TrainerName;
            trainer.Type = model.Type;
            trainer.WorkPlace = model.WorkPlace;
            trainer.PhoneNumber = model.PhoneNumber;
            trainer.Email = model.Email;
            _context.SaveChanges();
            return RedirectToAction("AllTrainer");
        }

        //manage categories
        [Authorize(Roles = "Staff")]
        public ActionResult AllCategories(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return View(_context.Categories.ToList());
            }
            return View(_context.Categories.Where(t => t.CategoryName.Contains(name)).ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult CreateCategory(Category model)
        {
            _context.Categories.Add(model);
            _context.SaveChanges();
            return RedirectToAction("AllCategories");
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult EditCategory(int Id)
        {
            return View(_context.Categories.SingleOrDefault(t => t.Id == Id));
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult EditCategory(Category model)
        {
            var cateogry = _context.Categories.SingleOrDefault(t => t.Id == model.Id);
            cateogry.CategoryName = model.CategoryName;
            cateogry.Description = model.Description;
            _context.SaveChanges();
            return RedirectToAction("AllCategories");
        }
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult DeleteCategory(int Id)
        {
            _context.Categories.Remove(_context.Categories.SingleOrDefault(t => t.Id == Id));
            _context.SaveChanges();
            return RedirectToAction("AllCategories");
        }
        //manage courses
        [Authorize(Roles = "Staff")]
        public ActionResult AllCourses(string name)
        {
           if(string.IsNullOrWhiteSpace(name))
            {
                return View(_context.Courses.Include(t => t.Category).ToList());
            }
            return View(_context.Courses.Where(t => t.CourseName.Contains(name)).Include(t => t.Category).ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult CreateCourse()
        {
            var model = new CourseCategory()
            {
                Categories = _context.Categories.ToList()
            };
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult CreateCourse(CourseCategory model)
        {
            _context.Courses.Add(model.Course);
            _context.SaveChanges();
            return RedirectToAction("AllCourses");
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult EditCourse(int Id)
        {
            var course = _context.Courses.Include(t => t.Category).SingleOrDefault(t => t.Id == Id);
            var model = new CourseCategory()
            {
                Categories = _context.Categories.ToList(),
                Course = course,
            };
            return View(model);
        }

        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult EditCourse(CourseCategory model)
        {
            var course = _context.Courses.SingleOrDefault(t => t.Id == model.Course.Id);
            course.CategoryId = model.Course.CategoryId;
            course.CourseName = model.Course.CourseName;
            course.CourseDescription = model.Course.CourseDescription;
            _context.SaveChanges();
            return RedirectToAction("AllCourses");
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult DeleteCourse(int Id)
        {
            _context.Courses.Remove(_context.Courses.SingleOrDefault(t => t.Id == Id));
            _context.SaveChanges();
            return RedirectToAction("AllCourses");
        }

        //manage trainee - course relationship
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult AllTraineeCourse(string Id)
        {
            ViewBag.TraineeId = Id;
            return View(_context.TraineeCourses.Where(t => t.TraineeId == Id).Include(t => t.Trainee).Include(t => t.Course).ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult AssignCourseToTrainee(string Id)
        {
            ViewBag.TraineeId = Id;
            var model = new AssignTraineeCourse()
            {
                TraineeId = Id,
                Courses = _context.Courses.ToList()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult AssignCourseToTrainee(AssignTraineeCourse model)
        {
            var traineeCourse = new TraineeCourse();
            traineeCourse.TraineeId = model.TraineeId;
            traineeCourse.CourseId = model.CourseId;
            _context.TraineeCourses.Add(traineeCourse);
            _context.SaveChanges();
            return RedirectToAction("AllTraineeCourse", new { Id = model.TraineeId });
        }
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult ReassignTraineeCourse(int Id)
        {
            var traineeCourse = _context.TraineeCourses.Include(t => t.Trainee).Include(t => t.Course).SingleOrDefault(t => t.Id == Id);
            var model = new AssignTraineeCourse()
            {
                TraineeId = traineeCourse.TraineeId,
                Courses = _context.Courses.ToList(),
                TraineeCourse = traineeCourse,
            };
            ViewBag.TraineeId = traineeCourse.TraineeId;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult ReassignTraineeCourse(AssignTraineeCourse model)
        {
            var traineeCourse = _context.TraineeCourses.SingleOrDefault(t => t.Id == model.TraineeCourse.Id);
            traineeCourse.CourseId = model.CourseId;
            _context.SaveChanges();
            return RedirectToAction("AllTraineeCourse", new { Id = traineeCourse.TraineeId });
        }
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult RemoveTraineeCourse(int Id)
        {
            var traineeCourse = _context.TraineeCourses.SingleOrDefault(t => t.Id == Id);
            var traineeId = traineeCourse.TraineeId;
            _context.TraineeCourses.Remove(traineeCourse);
            _context.SaveChanges();
            return RedirectToAction("AllTraineeCourse", new { Id = traineeId });
        }

        //manage trainer-course

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult AllTrainerCourse(string Id)
        {
            ViewBag.TrainerId = Id;
            return View(_context.TrainerCourses.Where(t => t.TrainerId == Id).Include(t => t.Trainer).Include(t => t.Course).ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult AssignCourseToTrainer(string Id)
        {
            ViewBag.TrainerId = Id;
            var model = new AssignTrainerCourse()
            {
                TrainerId = Id,
                Courses = _context.Courses.ToList()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult AssignCourseToTrainer(AssignTrainerCourse model)
        {
            var trainerCourse = new TrainerCourse();
            trainerCourse.TrainerId = model.TrainerId;
            trainerCourse.CourseId = model.CourseId;
            _context.TrainerCourses.Add(trainerCourse);
            _context.SaveChanges();
            return RedirectToAction("AllTrainerCourse", new { Id = model.TrainerId });
        }
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult ReassignTrainerCourse(int Id)
        {
            var traineeCourse = _context.TrainerCourses.Include(t => t.Trainer).Include(t => t.Course).SingleOrDefault(t => t.Id == Id);
            var model = new AssignTrainerCourse()
            {
                TrainerId = traineeCourse.TrainerId,
                Courses = _context.Courses.ToList(),
                TrainerCourse = traineeCourse,
            };
            ViewBag.TrainerId = traineeCourse.TrainerId;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult ReassignTrainerCourse(AssignTrainerCourse model)
        {
            var trainerCourse = _context.TrainerCourses.SingleOrDefault(t => t.Id == model.TrainerCourse.Id);
            trainerCourse.CourseId = model.CourseId;
            _context.SaveChanges();
            return RedirectToAction("AllTrainerCourse", new { Id = trainerCourse.TrainerId });
        }
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult RemoveTrainerCourse(int Id)
        {
            var trainerCourse = _context.TrainerCourses.SingleOrDefault(t => t.Id == Id);
            var trainerId = trainerCourse.TrainerId;
            _context.TrainerCourses.Remove(trainerCourse);
            _context.SaveChanges();
            return RedirectToAction("AllTrainerCourse", new { Id = trainerId });
        }
    }
}