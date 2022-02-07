using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ptpn8.Areas.Referensi.Models;
using Ptpn8.Areas.Referensi.Models.CRUD;
using Ptpn8.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Ptpn8.Areas.Referensi.Controllers
{
    public class ddlController : Controller
    {
        [Authorize]
        public ActionResult GetOrganisasi()
        {
            return Json(CRUDOrganisasi.CRUD.ListOrganisasi.OrderBy(o=>o.Nama), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GetDireksi(Guid organisasiId)
        {
            return Json(CRUDDireksi.CRUD.ListDireksi.OrderBy(o => o.Nama).Where(o => o.OrganisasiId == organisasiId), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GetBagian(string value)
        {
            return Json(CRUDBagian.CRUD.GetAllRecord().Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GetWilayah(Guid direksiId)
        {
            return Json(CRUDWilayah.CRUD.ListWilayah.OrderBy(o => o.Nama).Where(o => o.DireksiId == direksiId), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetNamaRelasi(string value)
        {
            var model = CRUDTehRelasi_DaftarPenerima.CRUD.ListTehRelasi_DaftarPenerima.Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetNamaProduk(string value)
        {
            return Json(CRUDTehRelasi_DaftarProduk.CRUD.ListTehRelasi_DaftarProduk.Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetKebun(string kebunId="")
        {
            var model = (from m in CRUDKebun.CRUD.GetAllRecord().Where(o=>kebunId==""? true : o.KebunId!=Guid.Parse(kebunId))
                         select new { m.KebunId, m.Alamat, m.Email, m.Faksimili, m.kd_kbn, m.KodeRekening, m.Nama, m.Nomenklatur, m.NPWP }).ToList();
            return Json(model.OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetKebun1(string value)
        {
            var model = (from m in CRUDKebun.CRUD.ListKebun.Where(o => o.Nama.ToLower().Contains(value.ToLower()))
                        select new {
                            KebunId = m.KebunId,
                            Nama = m.Nama,
                            kd_kbn = m.kd_kbn
                        }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetKelompokTransferUang()
        {
            var model = CRUDKelompokTransferUang.CRUD.ListKelompokTransferUang.ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpGet]
        public ActionResult GetKelompokTransferUang1(string value)
        {
            var model = CRUDKelompokTransferUang.CRUD.ListKelompokTransferUang.Where(o => o.NamaKelompok.ToLower().Contains(value.ToLower())).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetBankOrganisasi()
        {
            var model = CRUDBankOrganisasi.CRUD.ListBankOrganisasi.ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetBankOrganisasi1(string value)
        {
            var model = CRUDBankOrganisasi.CRUD.ListBankOrganisasi.Where(o => o.NomorAkunBank.ToLower().Contains(value.ToLower())).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetMerk()
        {
            var model = (from m in CRUDKebunBudidaya.CRUD.GetAllRecord()
                         select new { m.KebunBudidayaId, m.Merk}).ToList();
            return Json(model.OrderBy(o => o.Merk), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetMerk1(string value, string subBudidayaId="")
        {
           if (subBudidayaId == "") subBudidayaId = Guid.Empty.ToString();
            var model = CRUDKebunBudidaya.CRUD.ListKebunBudidaya.Where(o=>o.SubBudidayaId==Guid.Parse(subBudidayaId)).Where(o => o.Merk.ToLower().Contains(value.ToLower())).OrderBy(o => o.Merk).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        public ActionResult GetMerk2([DataSourceRequest] DataSourceRequest request, string mainBudidayaId="")
        {
            if (mainBudidayaId == "") mainBudidayaId = Guid.Empty.ToString();
            var model = (from m in CRUDKebunBudidaya.CRUD.GetAllRecord() where m.MainBudidayaId==Guid.Parse(mainBudidayaId) select m);
            return Json(model.OrderBy(o => o.Merk).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetAfdeling()
        {
            var LokasiKebun = CRUDAfdeling.CRUD.Get1Record(CRUDApplicationUser.CRUD.Get1Record(HttpContext.User.Identity.Name).LokasiKerjaId).KebunId;
            var model = from m in CRUDAfdeling.CRUD.GetAllRecord().Where(o => o.KebunId == LokasiKebun).Where(o => o.AfdelingId != CRUDApplicationUser.CRUD.Get1Record(HttpContext.User.Identity.Name).LokasiKerjaId)
                        select new { m.AfdelingId, m.Nama, m.Faksimili, m.KebunId, m.Telepon};
            return Json(model.OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetBudidaya()
        {
            List<MainBudidaya> model = CRUDBudidaya.CRUD.ListBudidaya.OrderBy(o => o.Nama).ToList();
            foreach (var m in model)
            {
                CRUDImage.CRUD.ReadToView(m.FileFoto, m.Foto);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetBudidayaAuc(string value)
        {
            List<MainBudidaya> model = CRUDBudidaya.CRUD.ListBudidaya.Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama).ToList();
            foreach (var m in model)
            {
                CRUDImage.CRUD.ReadToView(m.FileFoto, m.Foto);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetBudidaya1(string value)
        {
            List<MainBudidaya> model = CRUDBudidaya.CRUD.ListBudidaya.Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama).ToList();
            foreach (var m in model)
            {
                CRUDImage.CRUD.ReadToView(m.FileFoto, m.Foto);
            }
            MainBudidaya mdl = new MainBudidaya();
            mdl.Nama = "Seluruh";
            mdl.MainBudidayaId = Guid.Empty;
            model.Add(mdl);
            return Json(model.OrderBy(o=>o.MainBudidayaId), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetBudidaya2()
        {
            List<MainBudidaya> model = CRUDBudidaya.CRUD.ListBudidaya.OrderBy(o => o.Nama).Where(o=>o.Nama=="Teh" || o.Nama=="Karet" || o.Nama=="Sawit" ).ToList();
            foreach (var m in model)
            {
                CRUDImage.CRUD.ReadToView(m.FileFoto, m.Foto);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetGrade(Guid subBudidayaId)
        {
            var model = CRUDGrade.CRUD.ListGrade.Where(o=>o.SubBudidayaId==subBudidayaId)
                .OrderBy(o => o.Nama).ToList();
            foreach (var m in model)
            {
                CRUDImage.CRUD.ReadToView(m.FileFoto, m.Foto);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetGrade1(string value, string subBudidayaId="")
        {
            if (subBudidayaId == "") subBudidayaId = Guid.Empty.ToString();
            var model = CRUDGrade.CRUD.ListGrade.Where(o=>o.SubBudidayaId==Guid.Parse(subBudidayaId)).Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama);
            return Json(model.OrderBy(o=>o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetTipeKamar(string value, string kebunId = "")
        {
            if (kebunId == "") kebunId = Guid.Empty.ToString();
            var model = CRUDTipeKamar.CRUD.ListTipeKamar.Where(o => o.KebunId == Guid.Parse(kebunId)).Where(o => o.JenisKamar.ToLower().Contains(value.ToLower())).OrderBy(o => o.JenisKamar);
            return Json(model.OrderBy(o => o.JenisKamar), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetGrade2([DataSourceRequest] DataSourceRequest request, string subBudidayaId="")
        {
            if (subBudidayaId == "") subBudidayaId = Guid.Empty.ToString();
            var model = CRUDGrade.CRUD.ListGrade.Where(o => o.SubBudidayaId == Guid.Parse(subBudidayaId)).OrderBy(o => o.Nama);
            return Json(model.OrderBy(o=>o.Nama).ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult GetBuyer(string value)
        {
            var model = CRUDBuyer.CRUD.ListBuyer.Where(o=>o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o=>o.Nama).ToList();
            foreach (var m in model)
            {
                CRUDImage.CRUD.ReadToView(m.FileFoto, m.Foto);
            }
            return Json(model.OrderBy(o=>o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetBuyer1(string value, string mainBudidayaId="")
        {
            if (mainBudidayaId == "")
            {
                var model = CRUDBuyer.CRUD.ListBuyer.Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var model = (from m in CRUDBuyer.CRUD.ListBuyer.Where(o => o.Nama.ToLower().Contains(value.ToLower()))
                             join n in CRUDBuyerBudidaya.CRUD.ListBuyerBudidaya
                             on m.BuyerId equals n.BuyerId
                             where n.MainBudidayaId == Guid.Parse(mainBudidayaId)
                             orderby m.Nama select m).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult GetVessel(string value)
        {
            var model = CRUDVessel.CRUD.ListVessel.Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public ActionResult GetBroker(string value)
        {
            var model = CRUDBroker.CRUD.ListBroker.Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama).ToList();
            return Json(model.OrderBy(o=>o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetAlokasi(string value)
        {
            var model = CRUDAlokasi.CRUD.ListAlokasi.Where(o => o.NamaAlokasi.ToLower().Contains(value.ToLower())).OrderBy(o => o.NamaAlokasi).ToList();
            return Json(model.OrderBy(o => o.NamaAlokasi), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetBuyerBudidaya()
        {
            var model = (from m in CRUDBuyerBudidaya.CRUD.ListBuyerBudidaya
                         join n in CRUDBuyer.CRUD.ListBuyer
                         on m.BuyerId equals n.BuyerId
                         select new { BuyerId = m.BuyerId, Nama = n.Nama }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRekening(String value)
        {
            return Json(CRUDRekening.CRUD.GetnRecord(value).OrderBy(o => o.Norek), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRekening1(string value)
        {
            var model = CRUDRekening.CRUD.ListRekening.Where(o => o.Norek.Substring(0,value.Length)==value).ToList();
            return Json(model.OrderBy(o=>o.Norek), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRekeningbyNorek(String value)
        {
            return Json(CRUDRekening.CRUD.GetmRecord(value).OrderBy(o => o.Norek), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GetRekening2(string value, string barangId = "")
        {
            if (barangId == "") barangId = Guid.Empty.ToString();
            var model = CRUDRekening.CRUD.ListRekening.Where(o => o.RekeningId == Guid.Parse(barangId)).Where(o => o.Norek.ToLower().Contains(value.ToLower())).OrderBy(o => o.Norek);
            return Json(model.OrderBy(o => o.Norek), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetKodeBarang(string value)
        {
            var model = CRUDBarang.CRUD.ListBarang.Where(o => o.KodeBarang.Substring(0, value.Length) == value).ToList();
            return Json(model.OrderBy(o => o.KodeBarang), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetKodeBarang1(string value)
        {
            var model = CRUDBarang.CRUD.ListBarang.Where(o => o.NamaBarang.Substring(0, value.Length) == value).ToList();
            return Json(model.OrderBy(o => o.KodeBarang), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetNamaSatuan(string value)
        {
            var model = CRUDBarang.CRUD.ListBarang.Where(o => o.NamaSatuan.Substring(0, value.Length) == value).ToList();
            return Json(model.OrderBy(o => o.NamaSatuan), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNamaSatuan1(string value)
        {
            var model = CRUDBarang.CRUD.ListBarang.Where(o => o.NamaBarang.Substring(0, value.Length) == value).ToList();
            return Json(model.OrderBy(o => o.NamaSatuan), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetSatuan()
        {
            return Json(CRUDSatuan.CRUD.ListSatuan.OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetSatuan1(string value)
        {
            return Json(CRUDSatuan.CRUD.ListSatuan.Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetSatuan2([DataSourceRequest] DataSourceRequest request)
        {
            return Json(CRUDSatuan.CRUD.ListSatuan.OrderBy(o => o.Nama).ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult GetKemasan()
        {
            return Json(CRUDKemasan.CRUD.ListKemasan.OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetSubBudidaya(Guid mainBudidayaId)
        {
            return Json(CRUDSubBudidaya.CRUD.ListSubBudidaya.Where(o=>o.MainBudidayaId == mainBudidayaId).OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetSubBudidaya1(Guid mainBudidayaId, string value)
        {
            List<SubBudidaya> model = CRUDSubBudidaya.CRUD.ListSubBudidaya.Where(o => o.MainBudidayaId == mainBudidayaId).Where(o => o.Nama.ToLower().Contains(value.ToLower())).ToList();
            SubBudidaya mdl = new SubBudidaya();
            mdl.Nama = "Seluruh";
            mdl.MainBudidayaId = Guid.Empty;
            mdl.SubBudidayaId = Guid.Empty;
            model.Add(mdl);
            return Json(model.OrderBy(o => o.SubBudidayaId), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetSubBudidaya2([DataSourceRequest] DataSourceRequest request, string mainBudidayaId = "")
        {
            if (mainBudidayaId == "") mainBudidayaId = Guid.Empty.ToString();
            var model = from m in CRUDSubBudidaya.CRUD.ListSubBudidaya.Where(o => o.MainBudidayaId == Guid.Parse(mainBudidayaId)) select m;
            return Json(model.OrderBy(o => o.Nama).ToDataSourceResult(request));
        }
        public ActionResult GetNamaBarang(string value)
        {
            return Json(CRUDBarang.CRUD.GetAllRecord().Where(o => o.NamaBarang.ToLower().Contains(value.ToLower())).OrderBy(o => o.NamaBarang), JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        //public ActionResult GetNamaBarang(string value, string rekeningId = "")
        //{
        //    if (rekeningId == "") rekeningId = Guid.Empty.ToString();
        //    var model = CRUDBarang.CRUD.ListBarang.Where(o => o.RekeningId == Guid.Parse(rekeningId)).Where(o => o.NamaBarang.ToLower().Contains(value.ToLower())).OrderBy(o => o.NamaBarang);
        //    return Json(model.OrderBy(o => o.NamaBarang), JsonRequestBehavior.AllowGet);
        //}

        [Authorize]
        public ActionResult GetNegara()
        {
            return Json(CRUDNegara.CRUD.ListNegara.OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetPropinsi()
        {
            return Json(CRUDPropinsi.CRUD.ListPropinsi.OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetKota(Guid propinsiId)
        {
            return Json(CRUDKota.CRUD.ListKota.OrderBy(o => o.Nama).Where(o => o.PropinsiId == propinsiId), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetKecamatan(Guid kotaId)
        {
            return Json(CRUDKecamatan.CRUD.ListKecamatan.OrderBy(o => o.Nama).Where(o => o.KotaId == kotaId), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GetKelurahan(Guid kecamatanId)
        {
            return Json(CRUDKelurahan.CRUD.ListKelurahan.OrderBy(o => o.Nama).Where(o => o.KecamatanId == kecamatanId), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[HttpPost]
        public ActionResult GetWilayah1(string value)
        {
            var mdlWilayah = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.WilayahId != Guid.Empty && o.BidangId == Guid.Empty && o.KebunId == Guid.Empty && o.AfdelingId == Guid.Empty);
            var model = mdlWilayah;
            return Json(model.Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[HttpPost]
        public ActionResult GetLokasiKerja1(string value)
        {
            var mdlBagian = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.BagianId != Guid.Empty && o.UrusanId == Guid.Empty);
            var mdlKebun = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.KebunId != Guid.Empty && o.AfdelingId == Guid.Empty);
            var mdlWilayah = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.WilayahId != Guid.Empty && o.BidangId == Guid.Empty && o.KebunId==Guid.Empty && o.AfdelingId==Guid.Empty);
            var model = mdlBagian.Union(mdlKebun).Union(mdlWilayah);
            return Json(model.Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[HttpPost]
        public ActionResult GetLokasiKerjaTerbaru(string value)
        {
            var mdlBagian = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.BagianId != Guid.Empty && o.UrusanId == Guid.Empty && o.TanggalKadaluarsa == DateTime.Parse("2020-07-01 00:00:00.000"));
            var mdlKebun = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.KebunId != Guid.Empty && o.AfdelingId == Guid.Empty && o.TanggalKadaluarsa == DateTime.Parse("2020-07-01 00:00:00.000"));
            var mdlWilayah = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.WilayahId != Guid.Empty && o.BidangId == Guid.Empty && o.KebunId == Guid.Empty && o.AfdelingId == Guid.Empty && o.TanggalKadaluarsa == DateTime.Parse("2020-07-01 00:00:00.000"));
            var model = mdlBagian.Union(mdlKebun).Union(mdlWilayah);
            return Json(model.Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //untuk BPD
        public ActionResult GetLokasiKerja2(string value)
        {
            List<LokasiKerja> mdls = new List<LokasiKerja>();
            LokasiKerja mdl = new LokasiKerja();
            mdl.Nama = "Lain-Lain < 50";
            mdl.LokasiKerjaId = Guid.Parse("D2B0BF38-4DBD-44E3-BAC3-C68DB269427F");
            mdls.Add(mdl);

            List<LokasiKerja> mdls2 = new List<LokasiKerja>();
            LokasiKerja mdl2 = new LokasiKerja();
            mdl2.Nama = "Lain-Lain > 50";
            mdl2.LokasiKerjaId = Guid.Parse("EEE0D3A2-C3E6-4298-9E0E-9FCA9D4870FA");
            mdls2.Add(mdl2);

            var mdlBagian = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.BagianId != Guid.Empty && o.UrusanId == Guid.Empty && o.TanggalKadaluarsa == DateTime.Parse("2020-07-01 00:00:00.000"));
            var mdlKebun = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.KebunId != Guid.Empty && o.AfdelingId == Guid.Empty);
            var mdlWilayah = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.WilayahId != Guid.Empty && o.BidangId == Guid.Empty && o.KebunId == Guid.Empty && o.AfdelingId == Guid.Empty);
            var model = mdlBagian.Union(mdlKebun).Union(mdlWilayah).Union(mdls).Union(mdls2);


            return Json(model.Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[HttpPost]
        public ActionResult GetLokasiKerja3(string value)
        {
            var mdlBagian = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.BagianId != Guid.Empty && o.UrusanId == Guid.Empty && (o.TanggalKadaluarsa == DateTime.Parse("1996-03-11 00:00:00.000") || o.TanggalKadaluarsa == DateTime.Parse("2020-07-01 00:00:00.000")));
            var mdlKebun = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.KebunId != Guid.Empty && o.AfdelingId == Guid.Empty);
            var mdlWilayah = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.WilayahId != Guid.Empty && o.BidangId == Guid.Empty && o.KebunId == Guid.Empty && o.AfdelingId == Guid.Empty);
            var model = mdlBagian.Union(mdlKebun).Union(mdlWilayah);
            return Json(model.Where(o => o.Nama.ToLower().Contains(value.ToLower())).OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        //[HttpPost]
        //public ActionResult GetLokasiKerja1(Guid value)
        //{
        //    var mdlBagian = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.BagianId != Guid.Empty && o.UrusanId == Guid.Empty);
        //    var mdlKebun = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.KebunId != Guid.Empty && o.AfdelingId == Guid.Empty);
        //    var mdlWilayah = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.WilayahId != Guid.Empty && o.BidangId == Guid.Empty && o.KebunId == Guid.Empty && o.AfdelingId == Guid.Empty);
        //    var model = mdlBagian.Union(mdlKebun).Union(mdlWilayah);

        //    return Json(model.Where(o => o.LokasiKerjaId==value).FirstOrDefault(), JsonRequestBehavior.AllowGet);
        //}

        [Authorize]
        [HttpPost]
        public ActionResult GetRoleName1(string value)
        {
            return Json(CRUDApplicationRole.CRUD.GetAllRecord().Where(o => o.Name.ToLower().Contains(value.ToLower())).OrderBy(o => o.Name), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetUnit1(string value="")
        {
            if (value != "")
            {
                var model = (from m in CRUDUnit.CRUD.ListUnit.Where(o => o.NamaUnit.ToLower().Contains(value.ToLower()))
                             select new
                             {
                                 OrgId = m.OrgId,
                                 NamaUnit = m.NamaUnit
                             }).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var model = (from m in CRUDUnit.CRUD.ListUnit
                             select new
                             {
                                 OrgId = m.OrgId,
                                 NamaUnit = m.NamaUnit
                             }).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            
        }


        [Authorize]
        public ActionResult GetLokasiKerjaBOD1()
        {
            var mdlDireksi = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.DireksiId != Guid.Empty && o.BagianId == Guid.Empty && o.WilayahId == Guid.Empty).Where(o => o.TanggalKadaluarsa.Month == 7 && o.TanggalKadaluarsa.Year == 2020);
            var mdlBagian = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.BagianId != Guid.Empty && o.UrusanId == Guid.Empty).Where(o => o.TanggalKadaluarsa.Month == 7 && o.TanggalKadaluarsa.Year == 2020);
            var mdlKebun = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.KebunId != Guid.Empty && o.AfdelingId == Guid.Empty).Where(o => o.TanggalKadaluarsa.Month == 7 && o.TanggalKadaluarsa.Year == 2020);
            var mdlWilayah = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.WilayahId != Guid.Empty && o.BidangId == Guid.Empty && o.KebunId == Guid.Empty && o.AfdelingId == Guid.Empty).Where(o => o.TanggalKadaluarsa.Month == 7 && o.TanggalKadaluarsa.Year == 2020);
            var model = mdlBagian.Union(mdlKebun).Union(mdlWilayah).Union(mdlDireksi).ToList();
            return Json(model.OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[HttpPost]
        public ActionResult GetLokasiKerjaBOD2(string lokasiKerjaId = "")
        {
            var mdlDireksi = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.DireksiId == Guid.Parse(lokasiKerjaId)).Where(o => o.TanggalKadaluarsa.Month == 7 && o.TanggalKadaluarsa.Year == 2020);
            var mdlBagian = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.BagianId == Guid.Parse(lokasiKerjaId) && o.UrusanId == Guid.Empty).Where(o => o.TanggalKadaluarsa.Month == 7 && o.TanggalKadaluarsa.Year == 2020);
            var mdlUrusan = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.BagianId == Guid.Parse(lokasiKerjaId) && o.UrusanId != Guid.Empty).Where(o => o.TanggalKadaluarsa.Month == 7 && o.TanggalKadaluarsa.Year == 2020);
            var mdlKebun = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.KebunId == Guid.Parse(lokasiKerjaId) && o.AfdelingId == Guid.Empty).Where(o => o.TanggalKadaluarsa.Month == 7 && o.TanggalKadaluarsa.Year == 2020);
            var mdlAfdeling = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.KebunId == Guid.Parse(lokasiKerjaId) && o.AfdelingId != Guid.Empty).Where(o => o.TanggalKadaluarsa.Month == 7 && o.TanggalKadaluarsa.Year == 2020);
            var mdlWilayah = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.WilayahId == Guid.Parse(lokasiKerjaId) && o.BidangId == Guid.Empty && o.KebunId == Guid.Empty && o.AfdelingId == Guid.Empty).Where(o => o.TanggalKadaluarsa.Month == 7 && o.TanggalKadaluarsa.Year == 2020);
            var mdlBidang = CRUDLokasiKerja.CRUD.getFlatData().Where(o => o.WilayahId == Guid.Parse(lokasiKerjaId) && o.BidangId != Guid.Empty && o.KebunId == Guid.Empty && o.AfdelingId == Guid.Empty).Where(o => o.TanggalKadaluarsa.Month == 7 && o.TanggalKadaluarsa.Year == 2020);
            var model = mdlUrusan.Union(mdlKebun).Union(mdlAfdeling).Union(mdlWilayah).Union(mdlBidang).Union(mdlBagian).Union(mdlDireksi);
            return Json(model.OrderBy(o => o.Nama), JsonRequestBehavior.AllowGet);
        }

    }
}