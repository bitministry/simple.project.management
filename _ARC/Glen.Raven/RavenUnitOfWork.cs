using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glen.Domain.Entities;
using Glen.Domain.IData;
using Glen.Raven.Repository;

namespace Glen.Raven
{
    public class RavenUnitOfWork : IUnitOfWork
    {
        private readonly RavenSessionFactory sessionFactory;
        public RavenUnitOfWork( RavenSessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public object GetRepo(Type t)
        {
            if (t == typeof(IGenericRepository<Project>))
                return new GenericRepository<Project>(sessionFactory);
            if (t == typeof(IGenericRepository<Task>))
                return new GenericRepository<Task>(sessionFactory);
            if (t == typeof(IGenericRepository<Employee>))
                return new GenericRepository<Employee>(sessionFactory);
            return null;
        }

        public void SaveOrUpdate(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
