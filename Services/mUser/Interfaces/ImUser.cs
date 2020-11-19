using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mUser.Interfaces
{
    public interface ImUser
    {
        IList<mUserModel> GetAll();
        UserValidationResultModel UserValidation(UserValidationArgsModel Args);
        DatabaseActionResultModel Create(mUserModel model);
        DatabaseActionResultModel Update(mUserModel model);
        DatabaseActionResultModel Remove(string model);
    }
}
