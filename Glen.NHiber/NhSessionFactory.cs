
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.SqlCommand;
using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using Glen.Domain.Entities;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Persister.Entity;
using NHibernate.Tool.hbm2ddl;

namespace Glen.NHiber
{


    public class SessionFactory
    {
        private static ISessionFactory _obj;
        public static ISessionFactory Object => _obj ?? (_obj = Conf());


        public static ISessionFactory Conf()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["main"].ConnectionString;
            var bb = MsSqlConfiguration.MsSql2012.ConnectionString(connectionString);

            return Fluently.Configure()
                .Database(bb)
                .Mappings(m =>
                    m.AutoMappings.Add(Build))
                .ExposeConfiguration(cfg =>
                {
                    if (ConfigurationManager.AppSettings["UpdateSchema"] == "true")
                        new SchemaUpdate(cfg).Execute(false, true);
                })
                .BuildSessionFactory();
        }

        private static AutoPersistenceModel Build()
        {
            return AutoMap.AssemblyOf<Installation>(new MyConf())
//                .IgnoreBase<Contact>()
                .UseOverridesFromAssemblyOf<SessionFactory>();
        }
    }
    

    public class MyConf : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Namespace != null
                   && type.Namespace == "Glen.Domain.Entities"
                   && !type.IsEnum;
        }
        public override bool IsId(Member member)
        {
            return member.Name == "Id";
        }

    }
    

}
