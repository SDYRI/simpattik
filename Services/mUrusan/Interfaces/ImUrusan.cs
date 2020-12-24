using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Services.mUrusan.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mUrusan.Interfaces
{
    public interface ImUrusan
    {
        IList<mUrusanModel> GetAll(int posisi);
    }
}
