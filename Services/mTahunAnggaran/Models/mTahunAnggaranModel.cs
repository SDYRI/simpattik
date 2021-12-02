using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mTahunAnggaran.Models
{
    public class mTahunAnggaranModel : DefaultTableDBStructureModel
    {
        public mTahunAnggaranModel(){}

        public int TahunAnggaran { get; set; }
        public int TahunAnggaranOld { get; set; }
        public bool TahunAnggaranAktif { get; set; }
        public bool UserAktif { get; set; }
    }
}
