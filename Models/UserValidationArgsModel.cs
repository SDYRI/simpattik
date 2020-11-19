using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Models
{
    public class UserValidationArgsModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Action { get; set; }
    }
}
