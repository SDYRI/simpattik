using System.Collections.Generic;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mUrusan.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mUrusan.Interfaces
{
    public interface ImUrusan
    {
        IList<mUrusanModel> GetAll(int posisi);
        DatabaseActionResultModel Create(mUrusanModel model);
        DatabaseActionResultModel Update(mUrusanModel model);
        DatabaseActionResultModel Remove(int model);
    }
}
