using System.IO;

namespace Glen.Domain.IData
{
    public interface IAttachmentRepository
    {
        void Save(Stream stream, int id);
        Stream Retrive(int aid);

        void Delete(int id);
    }
}
