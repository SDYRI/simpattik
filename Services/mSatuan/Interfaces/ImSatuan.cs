using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mSatuan.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mSatuan.Interfaces
{
    public interface ImSatuan
    {
        IList<mSatuanModel> GetAll();
        DatabaseActionResultModel Create(mSatuanModel model);
        DatabaseActionResultModel Update(mSatuanModel model);
        //DatabaseActionResultModel Remove(int model);
    }
}
