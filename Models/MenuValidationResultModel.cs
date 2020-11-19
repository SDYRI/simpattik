using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Models
{
    public class MenuValidationResultModel
    {
        public string Urut { get; set; }
        public string IdSidebar_PK { get; set; }
        public string IdParentSidebar_PK { get; set; }
        public string Posisi { get; set; }
        public string DividerFlagClass { get; set; }
        public string NamaSidebar { get; set; }
        public string ControllerSidebar { get; set; }
        public string ActionSidebar { get; set; }
        public string Breadcrumb { get; set; }
        public string ClassMenu { get; set; }
        public string IconSidebar { get; set; }
    }
}
