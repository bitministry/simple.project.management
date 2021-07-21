using Glen.Domain.Entities;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;

namespace Glen.Raven
{
    public class MyDocumentStore
    {
        public static string DocsUrl;

        public DocumentStore DocumentStore { get; private set; }
        public MyDocumentStore( string connectionString )
        {
            var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionString(connectionString);
            parser.Parse();

            DocumentStore = new DocumentStore { Url = parser.ConnectionStringOptions.Url };
            DocsUrl = parser.ConnectionStringOptions.Url + "/docs/";
            DocumentStore.Initialize();
            DocumentStore.DefaultDatabase = DocumentStore.DefaultDatabase ?? parser.ConnectionStringOptions.DefaultDatabase;
            
            if (DocumentStore.DefaultDatabase != null )
                DocumentStore.DatabaseCommands.EnsureDatabaseExists(DocumentStore.DefaultDatabase);

//            DocumentStore.Conventions.RegisterIdConvention<Employee>((dbname, commands, contact) => "employees/" + contact.Email);
  //          DocumentStore.Conventions.RegisterIdConvention<Customer>((dbname, commands, contact) => "customers/" + contact.Email);

        }

        public IDocumentSession OpenSession()
        {
            return DocumentStore.OpenSession();
        }

    }

    public class MyOpenSession
    {
        private readonly IDocumentSession _session;
        public MyOpenSession(DocumentStore docstore)
        {
            _session = docstore.OpenSession();
        }

        public IDocumentSession Session
        {
            get { return _session; }
        }
    }
}
