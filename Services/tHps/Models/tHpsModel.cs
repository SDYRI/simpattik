using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.tHps.Models
{
    public class tHpsModel : DefaultTableDBStructureModel
    {
        public string idhps { get; set; }
        public string lembaga { get; set; }
        public string opd { get; set; }
        public string namaopd { get; set; }
        public string pejabat { get; set; }
        public string thanggrn { get; set; }
        public string uraianpekerjaan { get; set; }
        public string nomatapembayaran { get; set; }
        public string volume { get; set; }
        public string satuan { get; set; }
        public string pajak { get; set; }
        public string jumlahharga { get; set; }
        public string harga { get; set; }
        public string hargassh { get; set; }
        public string keteranganhps { get; set; }
        public string idpaket { get; set; }
        public string namapaket { get; set; }
        public string selisihpagu { get; set; }
        public string selisihvolume { get; set; }
        public string filehps { get; set; }
        public DateTime? mdfdate { get; set; }
    }
}
