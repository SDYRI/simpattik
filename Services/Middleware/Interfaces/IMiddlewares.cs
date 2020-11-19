using TasikmalayaKota.Simpatik.Web.Services.Middleware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Services.Middleware.Interfaces
{
    public interface IMiddlewares
    {
        MiddlewareModel GetControllerIndex(string _Param);
        MiddlewareUserModel GetUserController(int idAkun, string _Param);
        bool GetAksesAksiadditional(string Controller, string Aksi, string AksiTambahan, string UserId);
        bool GetAksesAksi(string Controller, string Aksi, int Type);
    }
}
