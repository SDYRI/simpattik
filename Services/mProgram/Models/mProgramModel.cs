using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mProgram.Models
{
    public class mProgramModel : DefaultTableDBStructureModel
    {
        public mProgramModel() { }

        public int IdProgram { get; set; }
        public int IdLembaga { get; set; }
        public string KodeProgram { get; set; }
        public string KodeKegiatan { get; set; }
        public string KodeSubkegiatan { get; set; }
        public string NamaProgram { get; set; }
        public string NamaKegiatan { get; set; }
        public string NamaSubkegiatan { get; set; }
        public int IdParentU { get; set; }
        public int IdParent { get; set; }
        public int IdPosisi { get; set; }
        public int IdUrusan { get; set; }
        public string NamaUrusan { get; set; }
        public string IdUserPPK { get; set; }
        public string NamaUserPPK { get; set; }
    }
}
