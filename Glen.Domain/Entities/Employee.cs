using System.Collections.Generic;

namespace Glen.Domain.Entities
{
    public enum PositionEnum { Sales, Installer, ProjectManager, Boss };

    public partial class Employee : Contact
    {
        
        
        public virtual PositionEnum Position { get; set; }

        public virtual string Password { get; set; }

        

    }
}
