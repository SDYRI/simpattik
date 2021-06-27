using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.tHps.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.tHps.Interfaces
{
    public interface ItHps
    {
        IList<tHpsModel> GetAll();
        IList<tHpsReviewModel> GetReviewAll(string idPaket);
        IList<mSshModel> SshGetAll();
        DatabaseActionResultModel Create(tHpsModel model);
        DatabaseActionResultModel Update(tHpsModel model);
        DatabaseActionResultModel Remove(string model);
        DatabaseActionResultModel Create(tHpsReviewModel model);
        DatabaseActionResultModel Update(tHpsReviewModel model);
        DatabaseActionResultModel RemoveReview(string model);
        mSshModelJson GetDetailJson(int tipe, int id);
    }
}
