using Kendo.Mvc.UI;
using Ptpn8.Models;
using Ptpn8.UserManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class LokasiKerja
    {
        public Guid LokasiKerjaId { get; set; }
        public Guid OrganisasiId { get; set; }
        public Guid DireksiId { get; set; }
        public string Direksi { get; set; }
        public Guid WilayahId { get; set; }
        public string Wilayah { get; set; }
        public Guid BidangId { get; set; }
        public Guid KebunId { get; set; }
        public string Kebun { get; set; }
        public Guid AfdelingId { get; set; }
        public Guid MandorBesarId { get; set; }
        public Guid MandorId { get; set; }
        public Guid BagianId { get; set; }
        public string Bagian { get; set; }
        public Guid UrusanId { get; set; }
        public string Text { get; set; }
        public string Nama { get; set; }
        public string kd_unit { get; set; }
        public string kd_afd { get; set; }
        public DateTime TanggalKadaluarsa { get; set; }
    }

    public class StatusLokasiKerja
    {
        public Guid LokasiKerjaId { get; set; }
        public string NamaLokasiKerjaId { get; set; }
        public string NamaLokasiKerja { get; set; }     // nama kelompok lokasi : 'kebun', 'afdeling', 'bagian', dll
        public string PosisiLokasiKerja { get; set; }   // 'direktur', 'bagian', 'wilayah' atau 'kebun'
        public Guid PosisiLokasiKerjaId { get; set; } // id kantor pusat, id wilayah atau id kebun
        public List<Guid> LokasiKerjaIdAnak { get; set; }  // id lokasi anaknya
        public List<Guid> LokasiKerjaIdTetanggaSatuInduk { get; set; } // id lokasi tetangga satu induk
        public List<Guid> LokasiKerjaIdTetanggaSatuBudidaya { get; set; } // id lokasi tetangga satu induk
        public Guid LokasiKerjaIdInduk { get; set; } // id lokasi induk
        public List<Guid> BudidayaYangDikelola { get; set; }
        public string KodeUnit { get; set; }
        public StatusLokasiKerja()
        {
            LokasiKerjaId = Guid.Empty;
            NamaLokasiKerjaId = "";
            NamaLokasiKerja = "";
            PosisiLokasiKerja = "";
            LokasiKerjaIdAnak = new List<Guid>();
            LokasiKerjaIdTetanggaSatuInduk = new List<Guid>();
            LokasiKerjaIdTetanggaSatuBudidaya = new List<Guid>();
            LokasiKerjaIdInduk = Guid.Empty;
            KodeUnit = "";
        }
    }
    
    public class CRUDLokasiKerja
    {
        public static CRUDLokasiKerja CRUD = new CRUDLokasiKerja();
        private static Guid organisasiId = Guid.Parse("78A299AF-9733-412B-835D-083953E67E0D");

        public IEnumerable<TreeViewItemModel> ListTreeLokasiKerja
        {
            get
            {
                IEnumerable<TreeViewItemModel> result = (List<TreeViewItemModel>)HttpContext.Current.Session["ListTreeLokasiKerja"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListTreeLokasiKerja"] = result = ReadTree();
                }
                return result;
            }
        }

        public List<LokasiKerja> ListFlatLokasiKerja
        {
            get
            {
                List<LokasiKerja> result = (List<LokasiKerja>)HttpContext.Current.Session["ListFlatLokasiKerja"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListFlatLokasiKerja"] = result = ReadFlat();
                }

                
                return result;
            }
        }

        public IEnumerable<TreeViewItemModel> getTreeData()
        {
            return ListTreeLokasiKerja;
        }

        public List<LokasiKerja> getFlatData()
        {
            return ListFlatLokasiKerja;
        }

        public string getFlatData(Guid lokasiKerjaId)
        {

            var mdl = new LokasiKerja();
            mdl = getFlatData().FirstOrDefault(o => o.LokasiKerjaId == lokasiKerjaId);
            if (mdl != null)
                return mdl.Text;
            else
                return "";
        }

        public List<LokasiKerja> ReadFlat()
        {
            var results = new List<LokasiKerja>();
            var mOrganisasi = (from a in CRUDOrganisasi.CRUD.ListOrganisasi.Where(o => o.OrganisasiId == organisasiId || o.OrganisasiId == Guid.Parse("DE43C18F-5F8A-40BC-9E95-0B5C91D694AA"))
                               select new LokasiKerja {
                                   LokasiKerjaId = a.OrganisasiId,
                                   OrganisasiId = a.OrganisasiId,
                                   Text = a.Nama,
                                   Nama = a.Nama,
                                   kd_unit = "",
                                   kd_afd = "",
                                   TanggalKadaluarsa = a.TanggalKadaluarsa
                               }).ToList();
            results.AddRange(mOrganisasi);
            var mDireksi = (from a in CRUDDireksi.CRUD.ListDireksi.Where(o => o.OrganisasiId == organisasiId)
                            select new LokasiKerja {
                                LokasiKerjaId = a.DireksiId,
                                OrganisasiId = a.OrganisasiId,
                                DireksiId = a.DireksiId,
                                Text = a.Nama,
                                Direksi = a.Nama,
                                Nama = a.Nama,
                                kd_unit = "00",
                                kd_afd = "",
                                TanggalKadaluarsa = a.TanggalKadaluarsa
                            }).ToList();
            
            foreach (var a1 in mDireksi)
            {
                a1.Text += "=>"+mOrganisasi.FirstOrDefault(o => o.OrganisasiId == a1.OrganisasiId).Text;
                results.Add(a1);

                var mWilayah = (from a in CRUDWilayah.CRUD.ListWilayah.Where(o => o.DireksiId == a1.DireksiId)
                                select new LokasiKerja {
                                    LokasiKerjaId = a.WilayahId,
                                    OrganisasiId = a1.OrganisasiId,
                                    DireksiId = a1.DireksiId,
                                    WilayahId = a.WilayahId,
                                    Text = a.Nama,
                                    Wilayah = a.Nama,
                                    Nama = a.Nama,
                                    Direksi = a1.Direksi,
                                    kd_unit = a.kd_kbn,
                                    kd_afd = "02",
                                    TanggalKadaluarsa = a.TanggalKadaluarsa
                                }).ToList();

                var mBagian = (from a in CRUDBagian.CRUD.ListBagian.Where(o => o.DireksiId == a1.DireksiId)
                                select new LokasiKerja {
                                    LokasiKerjaId = a.BagianId,
                                    OrganisasiId = a1.OrganisasiId,
                                    DireksiId = a1.DireksiId,
                                    BagianId = a.BagianId,
                                    Text = a.Nama,
                                    Bagian = a.Nama,
                                    Nama = a.Nama,
                                    Direksi = a1.Direksi,
                                    kd_unit = a.kd_kbn.Substring(0,2),
                                    kd_afd = a.kd_kbn.Substring(2,2),
                                    TanggalKadaluarsa = a.TanggalKadaluarsa
                                }).ToList();
                foreach (var a2 in mWilayah)
                {
                    a2.Text += "=>" + mDireksi.FirstOrDefault(o => o.DireksiId == a2.DireksiId).Text;
                    results.Add(a2);

                    var mBidang = (from a in CRUDBidang.CRUD.ListBidang.Where(o => o.WilayahId == a2.WilayahId)
                                   select new LokasiKerja
                                   {
                                       LokasiKerjaId = a.BidangId,
                                       OrganisasiId = a1.OrganisasiId,
                                       DireksiId = a1.DireksiId,
                                       WilayahId = a2.WilayahId,
                                       BidangId = a.BidangId,
                                       Text = a.Nama,
                                       Nama = a.Nama,
                                       Wilayah = a2.Wilayah,
                                       kd_unit = a.kd_kbn.Substring(0,2),
                                       kd_afd = a.kd_kbn.Substring(2, 2),
                                       TanggalKadaluarsa = a.TanggalKadaluarsa
                                   }).ToList();

                    var mKebun = (from a in CRUDKebun.CRUD.ListKebun.Where(o => o.WilayahId == a2.WilayahId)
                                  select new LokasiKerja
                                  {
                                      LokasiKerjaId = a.KebunId,
                                      OrganisasiId = a1.OrganisasiId,
                                      DireksiId = a1.DireksiId,
                                      KebunId = a.KebunId,
                                      WilayahId = a2.WilayahId,
                                      Text = a.Nama,
                                      Kebun = a.Nama,
                                      Nama = a.Nama,
                                      Wilayah = a2.Wilayah,
                                      kd_unit = a.kd_kbn,
                                      kd_afd = "02",
                                      TanggalKadaluarsa = a.TanggalKadaluarsa
                                  }).ToList();

                    foreach (var a2a in mBidang)
                    {
                        a2a.Text += "=>" + mWilayah.FirstOrDefault(o => o.WilayahId == a2a.WilayahId).Text;
                        results.Add(a2a);
                    }

                    foreach (var a2a in mKebun)
                    {
                        a2a.Text += "=>" + mWilayah.FirstOrDefault(o => o.WilayahId == a2a.WilayahId).Text;
                        results.Add(a2a);

                        var mAfdeling = (from a in CRUDAfdeling.CRUD.ListAfdeling.Where(o => o.KebunId == a2a.KebunId)
                                         select new LokasiKerja
                                         {
                                             LokasiKerjaId = a.AfdelingId,
                                             OrganisasiId = a1.OrganisasiId,
                                             DireksiId = a1.DireksiId,
                                             WilayahId = a2.WilayahId,
                                             KebunId = a2a.KebunId,
                                             AfdelingId = a.AfdelingId,
                                             Text = a.Nama,
                                             Nama = a.Nama,
                                             Kebun = a2a.Kebun,
                                             kd_unit = a.kd_afd.Substring(0,2),
                                             kd_afd = a.kd_afd.Substring(2,2),
                                             TanggalKadaluarsa = a.TanggalKadaluarsa
                                         }).ToList();

                        foreach (var a2a1 in mAfdeling)
                        {
                            a2a1.Text += "=>" + mKebun.FirstOrDefault(o => o.KebunId == a2a1.KebunId).Text;
                            results.Add(a2a1);
                        }
                        var mKepTan = (from a in CRUDKepTan.CRUD.ListKepTan.Where(o => o.KebunId == a2a.KebunId)
                                         select new LokasiKerja
                                         {
                                             LokasiKerjaId = a.KepTanId,
                                             OrganisasiId = a1.OrganisasiId,
                                             DireksiId = a1.DireksiId,
                                             WilayahId = a2.WilayahId,
                                             KebunId = a2a.KebunId,
                                             AfdelingId = a.KepTanId,
                                             Text = a.Nama,
                                             Nama = a.Nama,
                                             Kebun = a2a.Kebun,
                                             kd_unit = "",
                                             kd_afd = "",
                                             TanggalKadaluarsa = a.TanggalKadaluarsa
                                         }).ToList();

                        foreach (var a2a2 in mKepTan)
                        {
                            a2a2.Text += "=>" + mKebun.FirstOrDefault(o => o.KebunId == a2a2.KebunId).Text;
                            results.Add(a2a2);
                        }


                    }
                }

                foreach (var a3 in mBagian)
                {
                    a3.Text += "=>" + mDireksi.FirstOrDefault(o => o.DireksiId == a3.DireksiId).Text;
                    results.Add(a3);
                    var mUrusan = (from a in CRUDUrusan.CRUD.ListUrusan.Where(o => o.BagianId == a3.BagianId)
                                     select new LokasiKerja
                                     {
                                         LokasiKerjaId = a.UrusanId,
                                         OrganisasiId = a1.OrganisasiId,
                                         DireksiId = a1.DireksiId,
                                         BagianId = a3.BagianId,
                                         UrusanId = a.UrusanId,
                                         Text = a.Nama,
                                         Nama = a.Nama,
                                         Bagian = a3.Bagian,
                                         kd_unit = a.kd_kbn.Substring(0,2),
                                         kd_afd = a.kd_kbn.Substring(2,2),
                                         TanggalKadaluarsa = a.TanggalKadaluarsa
                                     }).ToList();
                    foreach (var a3a in mUrusan)
                    {
                        a3a.Text += "=>" + mBagian.FirstOrDefault(o => o.BagianId == a3a.BagianId).Text;
                        results.Add(a3a);
                    }
                }
            }
            return results;
        }
        public IEnumerable<TreeViewItemModel> ReadTree()
        {
            List<TreeViewItemModel> results = new List<TreeViewItemModel>();
            TreeViewItemModel result = new TreeViewItemModel();

            var organisasiModels = CRUDOrganisasi.CRUD.ListOrganisasi.Where(o => o.OrganisasiId == organisasiId).ToList();
            foreach (var organisasiModel in organisasiModels)
            {
                result = new TreeViewItemModel
                {
                    Id = organisasiModel.OrganisasiId.ToString(),
                    Text = organisasiModel.Nama,
                    // level direksi 
                    Items = (from a in CRUDDireksi.CRUD.ListDireksi
                             where a.OrganisasiId == organisasiModel.OrganisasiId
                             orderby a.Nomenklatur
                             select new TreeViewItemModel
                             {
                                 Id = a.DireksiId.ToString(),
                                 Text = a.Nama,
                                 // level wilayah dan bagian
                                 Items = (from b in CRUDWilayah.CRUD.ListWilayah
                                          where b.DireksiId == a.DireksiId
                                          orderby b.Nomenklatur
                                          select new TreeViewItemModel
                                          {
                                              Id = b.WilayahId.ToString(),
                                              Text = b.Nama,
                                              // level kebun dan bidang
                                              Items = (from b1 in CRUDBidang.CRUD.ListBidang
                                                       where b1.WilayahId == b.WilayahId
                                                       orderby b1.Nomenklatur
                                                       select new TreeViewItemModel
                                                       {
                                                           Id = b1.BidangId.ToString(),
                                                           Text = b1.Nama,
                                                       }).Union
                                                    (from b2 in CRUDKebun.CRUD.ListKebun
                                                     where b2.WilayahId == b.WilayahId
                                                     orderby b2.Nomenklatur
                                                     select new TreeViewItemModel
                                                     {
                                                         Id = b2.KebunId.ToString(),
                                                         Text = b2.Nama,
                                                         // level afdeling
                                                         Items = (from b2a in CRUDAfdeling.CRUD.ListAfdeling
                                                                  where b2a.KebunId == b2.KebunId
                                                                  orderby b2a.Nomenklatur
                                                                  select new TreeViewItemModel
                                                                  {
                                                                      Id = b2a.AfdelingId.ToString(),
                                                                      Text = b2a.Nama
                                                                  }).Union
                                                                  (from b2b in CRUDKepTan.CRUD.ListKepTan
                                                                   where b2b.KebunId == b2.KebunId
                                                                   orderby b2b.Nomenklatur
                                                                   select new TreeViewItemModel
                                                                   {
                                                                       Id = b2b.KepTanId.ToString(),
                                                                       Text = b2b.Nama
                                                                   }).ToList()
                                                     }).ToList()
                                          }).Union
                                          (from c in CRUDBagian.CRUD.ListBagian
                                           where c.DireksiId == a.DireksiId
                                           orderby c.Nomenklatur
                                           select new TreeViewItemModel
                                           {
                                               Id = c.BagianId.ToString(),
                                               Text = c.Nama,
                                               // level urusan
                                               Items = (from c2 in CRUDUrusan.CRUD.ListUrusan
                                                        where c2.BagianId == c.BagianId
                                                        orderby c2.Nomenklatur
                                                        select new TreeViewItemModel
                                                        {
                                                            Id = c2.UrusanId.ToString(),
                                                            Text = c2.Nama
                                                        }).ToList()
                                           }).ToList()
                             }).ToList()
                };
                results.Add(result);

            }
            return results;
        }
        public StatusLokasiKerja ReadStatusLokasiKerja(ApplicationUser user)
        {
            string tahunBuku = DateTime.Now.Year.ToString();
            var result = new StatusLokasiKerja();
            if (user != null)
            {
                var flatData = ListFlatLokasiKerja.Where(o => o.LokasiKerjaId == user.LokasiKerjaId).FirstOrDefault();
                if (flatData != null)
                {
                    result.LokasiKerjaId = user.LokasiKerjaId;
                    if (flatData.AfdelingId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Afdeling";
                        result.PosisiLokasiKerja = "KBN:" + flatData.Kebun;
                        result.PosisiLokasiKerjaId = flatData.KebunId;
                    }
                    else if (flatData.UrusanId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Urusan";
                        result.PosisiLokasiKerja = "KP:" + flatData.Bagian;
                        result.PosisiLokasiKerjaId = flatData.BagianId;
                    }
                    else if (flatData.BidangId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Bidang";
                        result.PosisiLokasiKerja = "MW:" + flatData.Wilayah;
                        result.PosisiLokasiKerjaId = flatData.WilayahId;
                    }
                    else if (flatData.KebunId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Kebun";
                        result.PosisiLokasiKerja = "KBN:" + flatData.Kebun;
                        result.PosisiLokasiKerjaId = flatData.KebunId;
                    }
                    else if (flatData.WilayahId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Wilayah";
                        result.PosisiLokasiKerja = "MW:" + flatData.Wilayah;
                        result.PosisiLokasiKerjaId = flatData.WilayahId;
                    }
                    else if (flatData.BagianId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Bagian";
                        result.PosisiLokasiKerja = "KP:" + flatData.Bagian;
                        result.PosisiLokasiKerjaId = flatData.BagianId;
                    }
                    else if (flatData.DireksiId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Direksi";
                        result.PosisiLokasiKerja = "Dir:" + flatData.Direksi;
                        result.PosisiLokasiKerjaId = flatData.DireksiId;
                    }
                    else
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Buyer";
                        result.PosisiLokasiKerja = "Buyer:" + flatData.Direksi;
                        result.PosisiLokasiKerjaId = user.LokasiKerjaId;

                    }
                }
            }
            return result;
        }
        private StatusLokasiKerja ReadStatusLokasiKerja(string userName)
        {
            string tahunBuku = DateTime.Now.Year.ToString();
            var result = new StatusLokasiKerja();
            if (userName == "") return result;
            var user = CRUDApplicationUser.CRUD.Get1Record(userName);
            if (user != null)
            {
                var flatData = ListFlatLokasiKerja.Where(o => o.LokasiKerjaId == user.LokasiKerjaId).FirstOrDefault();
                if(flatData != null)
                {
                    result.LokasiKerjaId = user.LokasiKerjaId;
                    if (flatData.AfdelingId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Afdeling";
                        result.PosisiLokasiKerja = "KBN:" + flatData.Kebun;
                        result.PosisiLokasiKerjaId = flatData.KebunId;
                        result.KodeUnit = flatData.kd_unit;
                    }
                    else if (flatData.UrusanId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Urusan";
                        result.PosisiLokasiKerja = "KP:" + flatData.Bagian;
                        result.PosisiLokasiKerjaId = flatData.BagianId;
                        result.KodeUnit = flatData.kd_unit;
                    }
                    else if (flatData.BidangId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Bidang";
                        result.PosisiLokasiKerja = "MW:" + flatData.Wilayah;
                        result.PosisiLokasiKerjaId = flatData.WilayahId;
                        result.KodeUnit = flatData.kd_unit;
                    }
                    else if (flatData.KebunId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Kebun";
                        result.PosisiLokasiKerja = "KBN:" + flatData.Kebun;
                        result.PosisiLokasiKerjaId = flatData.KebunId;
                        result.KodeUnit = flatData.kd_unit;
                    }
                    else if (flatData.WilayahId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Wilayah";
                        result.PosisiLokasiKerja = "MW:" + flatData.Wilayah;
                        result.PosisiLokasiKerjaId = flatData.WilayahId;
                        result.KodeUnit = flatData.kd_unit;
                    }
                    else if (flatData.BagianId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Bagian";
                        result.PosisiLokasiKerja = "KP:" + flatData.Bagian;
                        result.PosisiLokasiKerjaId = flatData.BagianId;
                        result.KodeUnit = flatData.kd_unit;
                    }
                    else if (flatData.DireksiId != Guid.Empty)
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Direksi";
                        result.PosisiLokasiKerja = "Dir:" + flatData.Direksi;
                        result.PosisiLokasiKerjaId = flatData.DireksiId;
                        result.KodeUnit = flatData.kd_unit;
                    }
                    else 
                    {
                        result.NamaLokasiKerjaId = flatData.Nama;
                        result.NamaLokasiKerja = "Buyer";
                        result.PosisiLokasiKerja = "Buyer:" + flatData.Direksi;
                        result.PosisiLokasiKerjaId = user.LokasiKerjaId;
                        result.KodeUnit = flatData.kd_unit;
                    }

                }
            }
            return result;
        }
        
        public StatusLokasiKerja LoginLokasiKerja(string userName)
        {

            //string x = getFlatData().FirstOrDefault(o => o.LokasiKerjaId == Guid.Parse("8FC50B2C-2A83-42CD-9C3C-0A14B6C9CD5A")).Text;

            var model = ReadStatusLokasiKerja(userName);
            return model;
        }

        public StatusLokasiKerja LoginLokasiKerja(ApplicationUser user)
        {
            var model = ReadStatusLokasiKerja(user);
            return model;
        }

    }
}