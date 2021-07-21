using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using Autofac;
using Autofac.Util;
using Glen.Domain.Entities;
using Glen.Domain.IData;
using Raven.Abstractions.Exceptions;
using Raven.Client;
using Raven.Client.Linq;

namespace Glen.Raven.Repositories
{

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class 
    {
        protected readonly IDocumentSession Session;
        protected readonly ILifetimeScope LifetimeScope;

        public GenericRepository(IDocumentSession sess, ILifetimeScope lifetimeScope)
        {
            Session = sess;
            LifetimeScope = lifetimeScope;
        }

        public IEnumerable<TEntity> All
        {
            get { return Session.Query<TEntity>().AsEnumerable(); }
        }
        public IList<TEntity> Find(Expression<Func<TEntity, bool>> criteria)
        {
            var list = Session.Query<TEntity>().ToList().Where(criteria.Compile()).ToList();
            if (typeof( TEntity ).BaseType == typeof(EntityBase<TEntity>)) 
                list.ForEach( x=> (x as EntityBase<TEntity>).IoCScopeSet( LifetimeScope ) );
            return list;
        }

        public virtual TEntity Get(string fullId)
        {
            if (fullId == null)
                return null;
            var obj = Session.Load<TEntity>(fullId);
            if ((obj as EntityBase<TEntity>) != null)
                (obj as EntityBase<TEntity>).IoCScopeSet(LifetimeScope);
            return obj;
        }
        public virtual void Delete(TEntity obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (Session.Advanced.GetDocumentId( obj ) == null )
                throw new InvalidOperationException("Cant delete entity that has not been saved");

            Session.Delete(obj );
            Session.SaveChanges();
        }
        public virtual void Delete(string fullId)
        {
            if (fullId == null)
                throw new ArgumentNullException("fullId");
            var obj = Get(fullId);

            if (obj != null)
                Delete(obj );
        }

        public bool Exists(string fullId)
        {
            var request = (HttpWebRequest)WebRequest.Create( MyDocumentStore.DocsUrl + fullId );
            request.Method = WebRequestMethods.Http.Head;
            
            try
            {
                request.GetResponse();
            }
            catch (WebException we)
            {
                if (we.Status == WebExceptionStatus.ProtocolError &&
                    we.Response != null && ((HttpWebResponse) we.Response).StatusCode == HttpStatusCode.NotFound)
                    return false;
                else
                    throw;
            }
            return true;
        }

        public virtual void SaveOrUpdate(TEntity obj)
        {
            Session.Advanced.Clear();
            Session.Store(obj);
            try
            {
                Session.SaveChanges();
            }
            catch (ConcurrencyException ex)
            {
                Session.Advanced.Evict( obj );
                throw new InvalidOperationException("Trying to save duplicate entity.");
            }
        }

        #region Disposing .. 
        private bool _disposed; 
        public void DoDispose()
        {
            if (_disposed) return; 
            Session.Dispose();
            _disposed = true; 
        }

        public void Dispose()
        {
            DoDispose();
        }

        ~GenericRepository()
        {
            DoDispose();
        }
        #endregion

    }
}
