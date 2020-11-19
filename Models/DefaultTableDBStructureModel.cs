using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Models
{
    public class DefaultTableDBStructureModel : DataManagerRequest
    {
        public string CreatedBy { get; set; }
        public DateTime? TanggalDibuat { get; set; }
        public DateTime? TanggalModifikasi { get; set; }
        public bool HapusFlag { get; set; }
    }
}
