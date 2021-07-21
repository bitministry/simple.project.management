using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glen.Domain.Entities;
using Glen.Domain.IData;
using Glen.Raven.Repository;
using Raven.Client;

namespace Glen.Raven.Repositories
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(IDocumentSession session)
            : base( session )
        {}


        public override void SaveOrUpdate( Contact obj )
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> AllEmployees {
            get { return Session.Query<Employee>().AsEnumerable(); }
        }

        public IEnumerable<Customer> AllCustomers
        {
            get { return Session.Query<Customer>().AsEnumerable(); }
        }
    }
}
