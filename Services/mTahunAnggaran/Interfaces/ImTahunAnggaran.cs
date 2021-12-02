using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mTahunAnggaran.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mTahunAnggaran.Interfaces
{
    public interface ImTahunAnggaran
    {
        IList<mTahunAnggaranModel> GetAll();
        DatabaseActionResultModel Create(mTahunAnggaranModel model);
        DatabaseActionResultModel Update(mTahunAnggaranModel model);
        string GetUserTahunAktif();
    }
}
