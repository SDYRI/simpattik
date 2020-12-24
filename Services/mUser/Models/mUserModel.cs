using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mUser.Models
{
    public class mUserModel : DefaultTableDBStructureModel
    {
        public mUserModel(){}

        public string IdUser { get; set; }
        public string NamaUser { get; set; }
        public string NipUser { get; set; }
        public string JabatanUser { get; set; }
        public string GolonganUser { get; set; }
        public string SaltUser { get; set; }
        public string PasswordUser { get; set; }
        public string UserName { get; set; }
        public string ListOpdUser { get; set; }
        public string ListIdOpdUser { get; set; }
    }
}
