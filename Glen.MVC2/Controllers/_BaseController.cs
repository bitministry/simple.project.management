using System;
using System.Configuration;
using System.Web.Mvc;
using BitMinistry.Common;
using Glen.Domain.Entities;
using Glen.IO.Repositories;
using Glen.NHiber;
using Glen.NHiber.Repositories;
using NHibernate;

namespace Glen.MVC.Controllers
{
    public class BaseController : Controller
    {
        protected static  AttachmentRepository AttachmentFileRepo;


        protected readonly InstallationService InstallationService;
        protected readonly CustomerService CustomerService;

        protected readonly IGenericRepository<Login> LoginRepo;
        protected readonly IGenericRepository<Employee> EmployeeRepository;

        protected ISession NhSession;
        public BaseController()
        {
            AttachmentFileRepo = AttachmentFileRepo ??
                                   new AttachmentRepository();

            NhSession = SessionFactory.Object.OpenSession();

            InstallationService = new InstallationService(NhSession);
            CustomerService = new CustomerService(NhSession);
            EmployeeRepository = new GenericRepository<Employee>(NhSession);
            LoginRepo = new GenericRepository<Login>(NhSession);

            

        }

        protected override void OnResultExecuted(System.Web.Mvc.ResultExecutedContext filterContext)
        {
            NhSession.Dispose();
        }


        public Employee Login => System.Web.HttpContext.Current.Session["Login"] as Employee;
    }
}
