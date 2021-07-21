using System;
using System.Configuration;
using System.IO;

namespace Glen.IO.Repositories 
{
    public class AttachmentRepository 
    {
        private readonly string _path;

        public AttachmentRepository(string attachmentRepositoryPath = null )
        {
            _path = attachmentRepositoryPath ?? 
                ConfigurationManager.AppSettings["attachmentRepositoryPath"] ??
                    AppDomain.CurrentDomain.BaseDirectory + @"\attachments\";
        }

        public void Save(Stream stream, int id)
        {
            var memstream = new MemoryStream();
            stream.CopyTo(memstream);
            if (memstream == null || memstream.Length == 0)
                throw new ArgumentNullException(nameof(stream), "Stream cant be empty!");
            memstream.Position = 0;
            
            var fileinfo = new FileInfo(_path + id);
            if (! fileinfo.Directory.Exists )
                fileinfo.Directory.Create();

            if ( fileinfo.Exists ) fileinfo.Delete();

            using (var fileStream = File.Create(_path + id ))
                memstream.CopyTo(fileStream);
        }

        public Stream Retrive(int id)
        {
            if (! File.Exists(_path + id) )
                throw new InvalidOperationException("No such file!");

            var filebytes = File.ReadAllBytes(_path + id);
            return new MemoryStream(filebytes);
        }

        public void Delete(int id)
        {
            if (! File.Exists(_path + id))
                throw new InvalidOperationException("No such file!");

            File.Delete(_path + id);
        }
    }
}
