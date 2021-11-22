using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mOpd.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mOpd.Interfaces
{
    public interface ImOpd
    {
        IList<mOpdModel> GetAll(int posisi);
        IList<mOpdModel> GetAllUrusan(int posisi, string idurusan);
        DatabaseActionResultModel Create(mOpdModel model);
        DatabaseActionResultModel Update(mOpdModel model);
        DatabaseActionResultModel Remove(int model);
    }
}
