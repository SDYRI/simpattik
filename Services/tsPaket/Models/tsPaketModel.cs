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
        public string lembaga { get; set; }
        public string opd { get; set; }
        public string pejabat { get; set; }
        public string thanggrn { get; set; }
        public string nmpaket { get; set; }
        public string krtbrg { get; set; }
        public string krtpelaku { get; set; }
        public string uraiankrj { get; set; }
        public string spesifikasi { get; set; }
        public string mtdpemilihan { get; set; }
        public string smbrdna { get; set; }
        public string jdwlplksanaan { get; set; }
        public string jeniskebutuhan { get; set; }
    }
}
