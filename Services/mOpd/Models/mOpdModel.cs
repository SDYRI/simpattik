using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mOpd.Models
{
    public class mOpdModel : DefaultTableDBStructureModel
    {
        public mOpdModel(){}

        public int IdOpd { get; set; }
        public int IdLembaga { get; set; }
        public string KodeUrusan { get; set; }
        public string KodeOpd { get; set; }
        public string KodeSubOpd { get; set; }
        public string NamaUrusan { get; set; }
        public string NamaOpd { get; set; }
        public string NamaSubOpd { get; set; }
        public int IdParentU { get; set; }
        public int IdParent { get; set; }
        public int IdPosisi { get; set; }
    }
}
