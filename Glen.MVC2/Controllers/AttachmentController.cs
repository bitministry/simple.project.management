using System.Web.Mvc;
using BitMinistry.Common;
using Glen.Domain.Entities;
using Glen.MVC.Helpers;

namespace Glen.MVC.Controllers
{
    public class AttachmentController : BaseController
    {
        

        

        [GlenAuthorize]
        public ActionResult Retrive(int id)
        {
            id.ThrowIfArgumentNull(nameof(id));
            
            var stream = AttachmentFileRepo.Retrive(id);
            var att = NhSession.Get<Attachment>(id);

            return File(stream, "application/octet-stream", att.FileName );
        }

        [GlenAuthorize(Roles = "Boss,ProjectManager,Sales")]
        public ActionResult Delete( int id, int customerId)
        {
            id.ThrowIfArgumentNull(nameof(id));
            id.ThrowIfArgumentNull(nameof(customerId));

            NhSession.Delete(NhSession.Get<Attachment>(id));
            NhSession.Flush();
            AttachmentFileRepo.Delete(id);

            return RedirectToAction("Edit", "Customer", new { id = customerId });
        }


    }
}
