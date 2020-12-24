using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Services.Middleware.Models
{
    public class enumDataModel
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public List<enumDataModel> SumberDana()
        {
            List<enumDataModel> sumberdana = new List<enumDataModel>();
            sumberdana.Add(new enumDataModel { Text = "APBN", Value = "APBN" });
            sumberdana.Add(new enumDataModel { Text = "APBD", Value = "APBD" });
            return sumberdana;
        }

        public List<enumDataModel> YaTidak()
        {
            List<enumDataModel> yatidak = new List<enumDataModel>();
            yatidak.Add(new enumDataModel { Text = "Ya", Value = "1" });
            yatidak.Add(new enumDataModel { Text = "Tidak", Value = "0" });
            return yatidak;
        }
    }
}
