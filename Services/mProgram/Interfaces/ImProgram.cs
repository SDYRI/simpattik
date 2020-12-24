using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mProgram.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mProgram.Interfaces
{
    public interface ImProgram
    {
        IList<mProgramModel> GetAll(int posisi);
        DatabaseActionResultModel Create(mProgramModel model);
        DatabaseActionResultModel Update(mProgramModel model);
        DatabaseActionResultModel Remove(int model);
    }
}
