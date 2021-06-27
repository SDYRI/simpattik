using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.tHps.Models
{
    public class mSshModel : DefaultTableDBStructureModel
    {
        public int typessh { get; set; }
        public int idssh { get; set; }
        public string namassh { get; set; }
        public Int64 hargassh { get; set; }
    }

    public class mSshModelJson
    {
        public int typessh { get; set; }
        public int idssh { get; set; }
        public string namassh { get; set; }
        public Int64 hargassh { get; set; }
    }
}
