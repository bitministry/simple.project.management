using Glen.Domain.Entities;
using Glen.MVC.Helpers;
using Glen.MVC.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Glen.MVC.Controllers
{

    public class InstallationController : BaseController
    {


        [GlenAuthorize]
        public ActionResult Index(string id = "Inquiry")
        {

            var status = (Installation.StatusEnum)Enum.Parse(typeof(Installation.StatusEnum), id, true);

            return View("List" + (status > Installation.StatusEnum.Order ? "Final" : status.ToString()),
                InstallationService.Find(p => p.Status == status));


        }

        //
        [HttpPost]
        [GlenAuthorize(Roles = "Boss,ProjectManager,Sales")]
        public ActionResult Create(Installation installation)
        {
            installation.Id = 0;
            installation.InquiryDate = DateTime.Now;
            
            InstallationService.SaveOrUpdate( installation );
            
            return RedirectToAction("Edit", "Customer", new { id = installation.Customer.Id });
            
        }


        //
        [GlenAuthorize]
        public ActionResult Edit(int id)
        {
            var project = InstallationService.Get(id);
            if (project == null)
                throw new NullReferenceException("There is no such project");

   
            ViewBag.topMenu = project.Status.ToString();

            if (Login.Position == PositionEnum.Installer)
                ViewBag.readOnly = "disabled=true";

            return View("Edit", project);
        }


        //
        [HttpPost]
        [GlenAuthorize(Roles = "Boss,ProjectManager,Sales")]
        public ActionResult Edit(Installation installation)
        {

            InstallationService.SaveOrUpdate(installation);
            if (installation.InquiryDate == null)
            {
                installation.InquiryDate = DateTime.Now;
                InstallationService.SaveOrUpdate(installation);

            }

            return RedirectToAction("Edit", new {id = installation.Id});

        }



        [GlenAuthorize(Roles = "Boss,ProjectManager,Sales")]
        public ActionResult Search(string id)
        {
            var term = id.ToLower();
            var projects = InstallationService.All.Where(p => (p.Title + p.Customer.Name).ToLower().Contains(term)).ToList();

            var customers =
                CustomerService.All.Where(c => c.Attachments.Any(a => a.FileName.ToLower().Contains(term)) 
                                                ||
                                               (c.Name + c.Phone + c.Phone2 + c.Email).ToLower().Contains(term)
                                               ||
                                               (c.Address != null && c.Address.formatted_address.ToLower().Contains(term)));

            return View("ListSearchResults", new object[] { projects , customers });
        }

        //
        [GlenAuthorize(Roles = "Boss")]
        public ActionResult Delete(int id)
        {
            var project = InstallationService.Get(id);
            if (project == null)
                throw new NullReferenceException("There is no such project");

            var prjStatus = project.Status;
            InstallationService.Delete(project);

            return Redirect("~/Project/Index/" + prjStatus);
        }


        [GlenAuthorize]
        public ActionResult ForwardToNextStatus(int id)
        {
            InstallationService.ForwardToNextStatus(id);

            return RedirectToAction("Edit", new { id });
        }

        [GlenAuthorize]
        public ActionResult EventsForCalendar()
        {

            int colorLooper = 0;

            Func<string> colorFunc = () =>
            {
                if (colorLooper == Statics.Colors.Length)
                    colorLooper = 0;
                else
                    colorLooper++;
                return Statics.Colors[colorLooper];
            };

            var eventsLinq = InstallationService.Find(x => x.Status == Installation.StatusEnum.Order || x.Status == Installation.StatusEnum.InStock )
                .Select(x => new
                {
                    title = x.Title + ", " + x.Customer.Name,
                    start = $"{x.Start:yyyy-MM-dd}",
                    end = $"{x.Deadline:yyyy-MM-dd}",
                    color = colorFunc(),
                    allDay = true,
                    url = Url.Action("Edit", new { id = x.Id })
                });

            return Json(eventsLinq, JsonRequestBehavior.AllowGet);
        }

        [GlenAuthorize]
        public ActionResult Calendar()
        {
            return View();
        }

    

      
    }

}