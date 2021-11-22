using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mSatuan.Models
{
    public class mSatuanModel : DefaultTableDBStructureModel
    {
        public mSatuanModel() { }

        public int IdSatuan { get; set; }
        public string NamaSatuan { get; set; }
    }
}
