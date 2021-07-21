using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Glen.MVC.Helpers;
using System.Web.Mvc;
using Glen.Domain.Entities;

namespace Glen.MVC.Controllers
{
    public class CustomerController  : BaseController
    {
        
        
        [GlenAuthorize(Roles = "Sales,Boss")]
        public ActionResult Index()
        {
            return View("Index", CustomerService.All );
        }

        [GlenAuthorize(Roles = "Sales,ProjectManager,Boss")]
        public ActionResult Edit(int id)
        {
            var customer = CustomerService.Get( id);
            
            return View( "Edit", customer ?? new Customer() );
        }

        [HttpPost]
        [GlenAuthorize(Roles = "Sales,ProjectManager,Boss")]

        public ActionResult Edit(Customer customer)
        {

            CustomerService.SaveOrUpdate( customer );

            AddAttachments(customer);


            return Redirect("~/Customer/Edit/"+ customer.Id );
        }

        [GlenAuthorize(Roles = "Boss")]
        public ActionResult Delete(int id)
        {
            CustomerService.DeleteById(id);

            return Index();
        }




        [GlenAuthorize(Roles = "Boss,ProjectManager,Sales")]
        private void AddAttachments(Customer customer)
        {
            if (Request.Files.Count == 0) return;

            for (int xi = 0; xi < Request.Files.Count; xi++)
            {
                HttpPostedFileBase file = Request.Files[xi];
                if (!String.IsNullOrEmpty(file.FileName))
                    CustomerService.AddAttachment(customer, file.InputStream, file.FileName);
            }
        }

        public ActionResult AddPayment(int id, decimal amount, DateTime xwhen)
        {
            NhSession.SaveOrUpdate( new Payment()
            {
                Customer = new Customer() { Id = id , },
                Amount = amount,
                XWhen = xwhen
            });
            NhSession.Flush();
            return RedirectToAction("Edit", new {id});
        }

        public ActionResult RemovePayment(int id)
        {
            var pay = NhSession.Get<Payment>(id);
            NhSession.Delete(pay);
            NhSession.Flush();
            return RedirectToAction("Edit", new { id = pay.Customer.Id });
        }
    }
}
