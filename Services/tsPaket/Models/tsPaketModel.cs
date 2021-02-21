using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.tsPaket.Models
{
    public class tsPaketModel : DefaultTableDBStructureModel
    {
        public tsPaketModel()
        {}

        public string idpaket { get; set; }
        public string idkonsolidasi { get; set; }
        public string lembaga { get; set; }
        public int tipePaket { get; set; }
        public string opd { get; set; }
        public string pejabat { get; set; }
        public string thanggrn { get; set; }
        public string nmpaket { get; set; }
        public string volume { get; set; }
        public string uraian { get; set; }
        public string spesifikasi { get; set; }
        public string prodlmnegeri { get; set; }
        public string ushkecil { get; set; }
        public string pradpa { get; set; }
        public string mtdpemilihanstlh { get; set; }
        public string jeniskebutuhan { get; set; }
        public string tipeswakelola { get; set; }
        public int penyelengaraswakelola { get; set; }
        public DateTime? pemanfaatanmulai { get; set; }
        public DateTime? pemanfaatanakhir { get; set; }
        public DateTime? pelaksanaanmulai { get; set; }
        public DateTime? pelaksanaanakhir { get; set; }
        public DateTime? pemilihanmulai { get; set; }
        public DateTime? pemilihanakhir { get; set; }
        public string mtdpemilihanstblm { get; set; }
        public DateTime? mdfdate { get; set; }
    }
}
