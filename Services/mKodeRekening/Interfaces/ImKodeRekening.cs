using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mKodeRekening.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mKodeRekening.Interfaces
{
    public interface ImKodeRekening
    {
        IList<mKodeRekeningModel> GetAll(int posisi);
        DatabaseActionResultModel Create(mKodeRekeningModel model);
        DatabaseActionResultModel Update(mKodeRekeningModel model);
        DatabaseActionResultModel Remove(int model);
    }
}
