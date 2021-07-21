using System.Linq;
using Autofac;
using Glen.Domain;
using Glen.Domain.Entities;
using Glen.NHiber;
using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlTypes;

namespace Glen.Konsool
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ss = SessionFactory.Object.OpenSession())
            {
                

                ss.SaveOrUpdate( new Customer()
                {
                    Id = 3,
                    Name = "tere t333 ere",
                    Email = "lalala@lala.ee",
                    Phone = "654654",
                    Address = new Address() { Id = 3}
                });
                ss.Flush();



            }


        }
        



        private Employee EmployeeTests( )
        {
            var emp = new Employee( )
            {
                Email = "mina2@sii2n.ee",
//                Position = PositionEnum.Installer, 
                Name = "Kalev",
                Phone = "5652231"
            };
//            emp.SaveOrUpdate();
            return emp; 
        }
        private Customer CusomerTests( )
        {
            

            var cust = new Customer( )
            {
                Email = "mina2@siin.ee",
                CustomerType = Customer.CustomerTypeEnum.Company, 
                Name = "Kalev",
                Phone = "5652231"
            };

//            cust.SaveOrUpdate();
            return cust;
        }
        private Project ProjectTests( Customer customer, Employee emp )
        {
            var prj = new Project( );

//            prj.AssignInstaller( emp );


            return prj;

            //var qq = prj.AssignmentsWithInstallers(); 

            //using (var fs = File.OpenRead(@"d:\tmp\Untitled.png"))
            //    prj.AddAttachment(fs, "duplicates");

            //var oi = prj.Attachments[0].RetrieveData();

            //using (var fs = new FileStream(@"d:\tmp\Untitled2.png", FileMode.CreateNew))
            //    oi.CopyTo( fs );

        }
    }

    

}
