using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Services.Middleware.Models
{
    public class MiddlewareModel
    {
        public MiddlewareModel() { }
        public string NamaIndex { get; set; }
    }

    public class MiddlewareUserModel
    {
        public MiddlewareUserModel() { }
        public int Id { get; set; }
        public int IdAkun { get; set; }
        public string LinkUrl { get; set; }
    }
}
