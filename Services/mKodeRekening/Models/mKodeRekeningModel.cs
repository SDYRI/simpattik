using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mKodeRekening.Models
{
    public class mKodeRekeningModel : DefaultTableDBStructureModel
    {
        public mKodeRekeningModel() { }

        public int IdKodeRekening { get; set; }
        public int IdLembaga { get; set; }
        public string KodeAkun { get; set; }
        public string KodeKelompok { get; set; }
        public string KodeJenis { get; set; }
        public string KodeObjek { get; set; }
        public string KodeRincian { get; set; }
        public string KodeSubRincian { get; set; }
        public string KodeRekening { get; set; }
        public string NamaAkun { get; set; }
        public string NamaKelompok { get; set; }
        public string NamaJenis { get; set; }
        public string NamaObjek { get; set; }
        public string NamaRincian { get; set; }
        public string NamaSubRincian { get; set; }
        public string ViewKodeRekening { get; set; }

        public int IdParentA { get; set; }
        public int IdParentK { get; set; }
        public int IdParentJ { get; set; }
        public int IdParentO { get; set; }
        public int IdParentU { get; set; }
        public int IdParent { get; set; }
        public int IdPosisi { get; set; }
    }
}
