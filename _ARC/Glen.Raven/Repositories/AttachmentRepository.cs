using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glen.Domain.Entities;
using Glen.Domain.IData;
using Raven.Json.Linq;

namespace Glen.Raven.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly MyDocumentStore _documentStore;
        public AttachmentRepository( MyDocumentStore documentStore )
        {
            _documentStore = documentStore;
        }

        public void Save( Stream stream, string attachmentId)
        {
            _documentStore.DocumentStore.DatabaseCommands.PutAttachment(attachmentId, null, stream, null);

        }

        public Stream Retrive(string attachmentId)
        {
            return _documentStore.DocumentStore.DatabaseCommands.GetAttachment(attachmentId).Data(); 
        }

        public void Delete(string attachmentId)
        {
            throw new NotImplementedException();
        }
    }
}
