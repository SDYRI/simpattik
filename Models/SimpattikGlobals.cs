using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Models
{
    static class SimpattikGlobals
    {
        public static List<string> Pengumuman;

        public static List<string> SetPengumuman(int idPengumuman)
        {
            List<string> setPengumuman = new List<string>();
            setPengumuman.Add("Mohon segera merubah password");
            setPengumuman.Add("Data master harus diisi");

            Pengumuman = setPengumuman;
            return setPengumuman;
        }
    }
}
