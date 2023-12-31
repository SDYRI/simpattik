﻿using Syncfusion.EJ2.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Models
{
    public class UserValidationResultModel : DatabaseActionResultModel
    {
        public UserValidationResultModel()
        {
            UserMenuValidation = new List<MenuValidationResultModel>();
            Toolbar = new List<string>();
            ToolbarTahun = new List<string>();
            ToolbarMaster = new List<string>();
        }

        public string IDAkun { get; set; }
        public string IDReff { get; set; }
        public string IDMor { get; set; }
        public string Nama { get; set; }
        public string Nip { get; set; }
        public string Jabatan { get; set; }
        public string Golongan { get; set; }
        public int Tipe { get; set; }
        public string Username { get; set; }
        public string UserIdUsman { get; set; }
        public bool UbahPasswordFlag { get; set; }
        public List<MenuValidationResultModel> UserMenuValidation { get; set; }
        public string AksiUsman { get; set; }
        public string Urusan { get; set; }
        public string Opd { get; set; }
        public string OpdName { get; set; }
        public string TahunAktif { get; set; }
        public string PaPpk { get; set; }
        public List<string> Toolbar { get; set; }
        public List<string> ToolbarTahun { get; set; }
        public List<string> ToolbarMaster { get; set; }
    }
}
