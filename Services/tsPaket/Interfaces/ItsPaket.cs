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
        IList<tsPaketModel> GetAll(int spesifikasi, int tipePaket);
        IList<tsPaketModel> GetAllReview(int spesifikasi, int tipePaket, string opdReview);
        IList<tsPaketModel> GetAllPerencanaan(int spesifikasi, int tipePaket);
        IList<tsPaketModel> GetAllStrategis(int spesifikasi, int tipePaket);
        DatabaseActionResultModel Create(tsPaketModel model);
        DatabaseActionResultModel Update(tsPaketModel model);
        DatabaseActionResultModel UpdateReview(tsPaketModel model);
        DatabaseActionResultModel UpdateStatusReview(tsPaketModel model);
        DatabaseActionResultModel Remove(string model);
    }
}
