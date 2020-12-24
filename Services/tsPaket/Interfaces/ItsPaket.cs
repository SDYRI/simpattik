﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.tsPaket.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.tsPaket.Interfaces
{
    public interface ItsPaket
    {
        IList<tsPaketModel> GetAll(int spesifikasi);
        DatabaseActionResultModel Create(tsPaketModel model);
        DatabaseActionResultModel Update(tsPaketModel model);
        DatabaseActionResultModel Remove(int model);
    }
}
