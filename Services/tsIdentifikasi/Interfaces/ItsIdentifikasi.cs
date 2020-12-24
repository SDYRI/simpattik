using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.tsIdentifikasi.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.tsIdentifikasi.Interfaces
{
    public interface ItsIdentifikasi
    {
        IList<tsIdentifikasiModel> GetAll(int spesifikasi);
        IList<tsIdentifikasiModel> GetAll(int spesifikasi, string paket);
        IList<tsIdentifikasiModel> GetAll();
        DatabaseActionResultModel Create(tsIdentifikasiModel model);
        DatabaseActionResultModel Update(tsIdentifikasiModel model);
        DatabaseActionResultModel Remove(int model);
    }
}
