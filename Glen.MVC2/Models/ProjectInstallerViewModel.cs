using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glen.Domain.Entities;

namespace Glen.MVC.Models
{
    public class ProjectInstallerViewModel
    {
        public Employee Employee { get; set; }
        public bool IsAssigned { get; set; }
    }
}