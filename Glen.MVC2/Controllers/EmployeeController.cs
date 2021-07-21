using System.Linq;
using System.Web.Mvc;
using BitMinistry.Common;
using Glen.Domain.Entities;
using Glen.MVC.Helpers;
using Glen.MVC.Models;


namespace Glen.MVC.Controllers
{
    
    public class EmployeeController : BaseController
    {
        

        
        [GlenAuthorize(Roles=("Boss"))]
        public ActionResult Index()
        {
            return View("Index", EmployeeRepository.All);
        }

        [GlenAuthorize]
        public ActionResult Edit(int id)
        {
            var employee = EmployeeRepository.Get(id);
            
            return View("Edit", employee ?? new Employee());
        }

        [HttpPost]
        [GlenAuthorize]
        public ActionResult Edit(Employee employee)
        {

            EmployeeRepository.SaveOrUpdate( employee );

            return Redirect("~/Employee/Edit/" + employee.Id);
            
        }

        [GlenAuthorize(Roles="Boss")]
        public ActionResult Delete(int id)
        {
            EmployeeRepository.DeleteById(id);

            return Index();
        }

        public ActionResult LogIn()
        {
            var rememberMe = LoginRepo.Find(l => l.IpAddress == Request.UserHostAddress ).FirstOrDefault();
            var model = new LoginViewModel(); 
            if (rememberMe != null)
            {
                if (rememberMe.KeepMeLoggedIn)
                    return DoLogin(rememberMe.Employee);
                
                model.Employee = rememberMe.Employee;
            }

            return View( model );
        }
        private ActionResult DoLogin(Employee employee)
        {
            Employee login = null;
            employee.InjectInto(ref login);

            Session["Login"] = login;

            if (Request["returnUrl"] != null)
                return new RedirectResult(Request["returnUrl"]);

            return RedirectToAction("Index", "Installation");
        }

        [HttpPost]
        public ActionResult LogIn(LoginViewModel model)
        {
            var incomingEmployee = EmployeeRepository.Find(e => e.Email == model.Email).FirstOrDefault();

            if (incomingEmployee == null )
            {
                model.ErrorMessage = "No such user";
                return View(model);
            }

            if (model.PasswordReminder)
            {
//                KMail.Send(ConfigurationManager.AppSettings["SystemEmail"] ?? model.Email, model.Email, login.Password, "password reminder");
                model.ErrorMessage = "Password is sent to "+ model.Email;
                return View(model);
            }

            if (incomingEmployee.Password != model.Password )
            {
                model.ErrorMessage = "Incorrect password";
                return View(model);
            }
            if (model.KeepMeLoggedIn)
            {
                var saved = LoginRepo.Find(l => l.IpAddress == Request.UserHostAddress).FirstOrDefault();
                if (saved != null)
                {
                    saved.Employee = incomingEmployee;
                    saved.KeepMeLoggedIn = model.KeepMeLoggedIn;
                }
                else
                    saved = new Login()
                    {
                        Employee = incomingEmployee,
                        IpAddress = Request.UserHostAddress, 
                        KeepMeLoggedIn = model.KeepMeLoggedIn
                    };

                LoginRepo.SaveOrUpdate(saved);
            }
            return DoLogin( incomingEmployee );
        }

        public ActionResult LogOff()
        {
            Session["Login"] = null;

            var rememberMe = LoginRepo.Find(l => l.IpAddress == Request.UserHostAddress ).FirstOrDefault();

            if (rememberMe != null)
                if (rememberMe.KeepMeLoggedIn)
                {
                    rememberMe.KeepMeLoggedIn = false; 
                    LoginRepo.SaveOrUpdate( rememberMe );
                }

            return RedirectToAction("Index", "Installation");
        }



    }
}