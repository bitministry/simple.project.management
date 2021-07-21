using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BitMinistry.Common;
using Glen.Domain.Entities;
using NHibernate;
using NHibernate.Linq;

namespace Glen.NHiber.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public readonly ISession Session;

        public GenericRepository(ISession session)
        {
            if (session.IsOpen == false)
                throw new SessionException("Session not open!");
            Session = session;
        }

        public IQueryable<TEntity> All => Session.Query<TEntity>();
   
        public IList<TEntity> Find(Expression<Func<TEntity, bool>> criteria)
        {
            return Session.Query<TEntity>().Where(criteria).ToList();
        }

        public virtual TEntity Get(object id)
        {
            return Session.Get<TEntity>(id);
        }

        public virtual void SaveOrUpdate(TEntity obj)
        {
            if ((obj.GetType().GetProperty("Address")?.GetValue(obj, null) as Address)?.Id == 0)
            {
                var addr = obj.GetType().GetProperty("Address")?.GetValue(obj, null) as Address;
                Session.SaveOrUpdate( addr );
            }

            Session.SaveOrUpdate(obj);
            Session.Flush();
        }

    
        public void Dispose()
        {
            Session.Dispose();
        }

        public void Refresh(TEntity obj)
        {
            throw new NotImplementedException();
        }
        public void DeleteById(object id)
        {
            Delete( Get(id));
        }

        public void Delete(TEntity entity)
        {
            Session.Delete(entity);
            Session.Flush();
        }

    }
}
