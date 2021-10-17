using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.tsIdentifikasi.Models
{
    public class SuratPenetapanModel : DefaultTableDBStructureModel
    {
        public SuratPenetapanModel() { }

        public string opd { get; set; }
        public int tahun { get; set; }
        public string urutdasar { get; set; }
        public string ketdasar { get; set; }
        public string tipedasar { get; set; }
        public string namappk { get; set; }
        public string nipppk { get; set; }

    }
}
