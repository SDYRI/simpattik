using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Models
{
    public class DatabaseActionResultModel
    {
        public bool Success { get; set; }
        public string Kode { get; set; }
        public string Pesan { get; set; }
        public object Data { get; set; }
        public object Data2 { get; set; }
    }
}
