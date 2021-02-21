using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.tsIdentifikasi.Models
{
    public class sumberDanaModel
    {
        public int nilai { get; set; }
        public string smbrdn { get; set; }
    }

    public class kondisiModel
    {
        public int nilai { get; set; }
        public string kondisi { get; set; }
    }

    public class barangmaterialModel
    {
        public int nilai { get; set; }
        public string material { get; set; }
    }

    public class tsIdentifikasiModel : DefaultTableDBStructureModel
    {
        public tsIdentifikasiModel()
        {
            sumberdanal = new List<sumberDanaModel>();
            kondisil = new List<kondisiModel>();
            barangmateriall = new List<barangmaterialModel>();
        }

        public int ididetifikasi { get; set; }
        public string idpaket { get; set; }
        public int thanggrn { get; set; }
        public int lembaga { get; set; }
        public int idopd { get; set; }
        public string opd { get; set; }
        public string pejabat { get; set; }
        public string program { get; set; }
        public string namaprogram { get; set; }
        public string kegiatan { get; set; }
        public string namakegiatan { get; set; }
        public string subkegiatan { get; set; }
        public string namasubkegiatan { get; set; }
        public int idbrgkerj { get; set; }
        public string outputidentifikasi { get; set; }
        public string jeniskebutuhan { get; set; }
        public string namabrgkerj { get; set; }
        public string fungsi { get; set; }
        public string jumlahbarang { get; set; }
        public string waktu { get; set; }
        public string pihak { get; set; }
        public string totalwaktu { get; set; }
        public string ekatalog { get; set; }
        public string prioritas { get; set; }
        public int perkiraanbiaya { get; set; }
        public string jumlahpegawai { get; set; }
        public string bebantugas { get; set; }
        public string jumlahbarangtersedia { get; set; }
        public string jumlahbarangsejenis { get; set; }
        public string kondisilayak { get; set; }
        public string lokasi { get; set; }
        public string sumberdana { get; set; }
        public string kemudahan { get; set; }
        public string produsen { get; set; }
        public string kriteria { get; set; }
        public string tkdn { get; set; }
        public string nilaitkdn { get; set; }
        public string pengiriman { get; set; }
        public string pengakutan { get; set; }
        public string pemasangan { get; set; }
        public string penimbunan { get; set; }
        public string pengunaan { get; set; }
        public string kebutuhanpelatihan { get; set; }
        public string aspek { get; set; }
        public string barangsejenis { get; set; }
        public string indikasikonsolidasi { get; set; }
        public string target { get; set; }
        public string studikelayakan { get; set; }
        public string ded { get; set; }
        public string komplektifitas { get; set; }
        public string tahunpelaksanaan { get; set; }
        public string jumlahtahunpelaksanaan { get; set; }
        public string suratijin { get; set; }
        public string nomorsuratijin { get; set; }
        public string barangmaterial { get; set; }
        public string usahakecil { get; set; }
        public string pembebasanlahan { get; set; }
        public string luaspembebasanlahan { get; set; }
        public string pemanfaatantanah { get; set; }
        public string lamawaktu { get; set; }
        public string administrasipembayaran { get; set; }
        public string terdapatpengadaan { get; set; }
        public string badanusaha { get; set; }
        public string targetsasaran { get; set; }
        public string manfaat { get; set; }
        public string kuantitas { get; set; }
        public string spesifikasi { get; set; }
        public string waktupenggunaan { get; set; }
        public string ketersediaanusaha { get; set; }
        public string ukurankapasitas { get; set; }
        public string koderup { get; set; }
        public bool hpsflag { get; set; }
        public string crtbyus { get; set; }
        public string crpengdaan { get; set; }
        public string uraian { get; set; }
        public string kbki { get; set; }
        public string tipeswakelola { get; set; }
        public string pnyswakelola { get; set; }
        public int nilaisirup { get; set; }
        public DateTime crtdate { get; set; }
        public DateTime mdfdate { get; set; }

        #region sumberDana
        public string txtsumberdana { get; set; }
        public IList<sumberDanaModel> sumberdanal { get; set; }
        #endregion

        #region kondisi
        public string txtkondisi { get; set; }
        public IList<kondisiModel> kondisil { get; set; }
        #endregion

        #region barangmaterial
        public string txtbarangmaterial { get; set; }
        public IList<barangmaterialModel> barangmateriall { get; set; }
        #endregion
    }
}
