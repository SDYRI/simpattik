using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mUrusan.Models
{
    public class mUrusanModel : DefaultTableDBStructureModel
    {
        public mUrusanModel() { }

        public int IdUrusan { get; set; }
        public int IdLembaga { get; set; }
        public string KodeUrusan { get; set; }
        public string KodeSubUrusan { get; set; }
        public string NamaUrusan { get; set; }
        public string NamaSubUrusan { get; set; }
        public int IdParentU { get; set; }
        public int IdParent { get; set; }
        public int IdPosisi { get; set; }
    }
}
