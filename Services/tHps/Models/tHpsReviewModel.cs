using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.tHps.Models
{
    public class tHpsReviewModel : DefaultTableDBStructureModel
    {
        public int idrvwhps { get; set; }
        public string idssh { get; set; }
        public string namassh { get; set; }
        public int volume { get; set; }
        public int pagu { get; set; }
        public int nilaissh { get; set; }
        public int hrgpasar { get; set; }
        public int slshpagu { get; set; }
        public int slshvolume { get; set; }
        public string jenisssh { get; set; }
        public string hpsflag { get; set; }
        public DateTime? mdfdate { get; set; }
        public string crtbyus { get; set; }
        public DateTime? crtdate { get; set; }
        public string idpaket { get; set; }
    }
}
