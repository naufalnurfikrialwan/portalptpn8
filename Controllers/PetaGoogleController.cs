using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ptpn8.Controllers
{
    public class PetaGoogleController : Controller
    {
        // GET: PetaGoogle
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KebunGeoJson()
        {
            var mdl1 = (from m in CRUDKebunPeta.CRUD.GetAllRecord().Where(o => o.Peta != null)
                       select new
                       {
                           Lokasi = "KEBUN",
                           LokasiId = m.KebunId,
                           Nama = CRUDKebun.CRUD.Get1Record(m.KebunId).Nama,
                           Warna = CRUDKebun.CRUD.Get1Record(m.KebunId).Warna,
                           Peta = m.Peta.ToString()
                       }).ToList();

            var mdl2 = (from m in CRUDAfdeling.CRUD.GetAllRecord()
                       join n in CRUDAfdelingPeta.CRUD.GetAllRecord().Where(o=>o.Peta != null) on m.AfdelingId equals n.AfdelingId
                       select new
                       {
                           Lokasi = "AFDELING",
                           LokasiId = m.AfdelingId,
                           Nama = m.Nama,
                           Warna = m.Warna,
                           Peta = n.Peta.ToString()
                       }).ToList();

            var mdl3 = (from m in CRUDBlok.CRUD.GetAllRecord()
                       join n in CRUDBlokRealisasi.CRUD.GetAllRecord().Where(o=>o.Peta != null && o.TahunBuku=="2017") on m.BlokId equals n.BlokId
                       select new
                       {
                           Lokasi = "BLOK",
                           LokasiId = m.BlokId,
                           Nama = m.Nama,
                           Warna = m.Warna,
                           Peta = n.Peta.ToString()
                       }).ToList();

            var mdl = mdl1.Union(mdl2.Union(mdl3)).ToList();

            var featureCollection = new NetTopologySuite.Features.FeatureCollection();

            foreach (var k in mdl)
            {
                if (k.Peta != "")
                {
                    var wktReader = new NetTopologySuite.IO.WKTReader();
                    var geom = wktReader.Read(k.Peta);
                    var feature = new NetTopologySuite.Features.Feature(geom, new NetTopologySuite.Features.AttributesTable());
                    string contentStr = "<div id='content'>" +
                                            "<div id='siteNotice'>" +
                                            "</div>" +
                                            "<h1 id='firstHeading' class='firstHeading'>"+k.Nama+"</h1>" +
                                            "<div id='bodyContent'>" +
                                            "<p><b>"+k.Nama+"</b>, also referred to as <b>Ayers Rock</b>, is a large " +
                                            "sandstone rock formation in the southern part of the " +
                                            "Northern Territory, central Australia. It lies 335&#160;km (208&#160;mi) " +
                                            "south west of the nearest large town, Alice Springs; 450&#160;km " +
                                            "(280&#160;mi) by road. Kata Tjuta and Uluru are the two major " +
                                            "features of the Uluru - Kata Tjuta National Park. Uluru is " +
                                            "sacred to the Pitjantjatjara and Yankunytjatjara, the " +
                                            "Aboriginal people of the area. It has many springs, waterholes, " +
                                            "rock caves and ancient paintings. Uluru is listed as a World " +
                                            "Heritage Site.</p>" +
                                            "<p>Attribution: Uluru, <a href='https://en.wikipedia.org/w/index.php?title=Uluru&oldid=297882194'>" +
                                            "https://en.wikipedia.org/w/index.php?title=Uluru</a> " +
                                            "(last visited June 22, 2009).</p>" +
                                            "</div>" +
                                            "</div>";
                    feature.Attributes.AddAttribute("LOKASI", k.Lokasi);
                    feature.Attributes.AddAttribute("NAMA_LOKASI", k.Nama);
                    feature.Attributes.AddAttribute("CENTER_X", feature.Geometry.Coordinates.Average(o => o.X));
                    feature.Attributes.AddAttribute("CENTER_Y", feature.Geometry.Coordinates.Average(o => o.Y));
                    feature.Attributes.AddAttribute("CONTENT", contentStr);
                    feature.Attributes.AddAttribute("COLOR", k.Warna);
                    feature.Attributes.AddAttribute("ID", k.LokasiId);
                    feature.Attributes.AddAttribute("FULLCOLOR", true);
                    featureCollection.Add(feature);
                }
            }

            var sb = new StringBuilder();
            var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, featureCollection);
            }

            return Content(sb.ToString(), "application/json");
        }

        public ActionResult AfdelingGeoJson(Guid kebunId)
        {
            var mdl = (from m in CRUDAfdeling.CRUD.GetAllRecord().Where(o => o.KebunId == kebunId)
                       join n in CRUDAfdelingPeta.CRUD.GetAllRecord() on m.AfdelingId equals n.AfdelingId
                       select new
                       {
                           LokasiId = m.AfdelingId,
                           Nama = m.Nama,
                           Warna = m.Warna,
                           Peta = n.Peta.ToString()
                       }).ToList();

            var featureCollection = new NetTopologySuite.Features.FeatureCollection();
            foreach (var k in mdl )
            {
                if (k.Peta != "")
                {
                    var wktReader = new NetTopologySuite.IO.WKTReader();
                    var geom = wktReader.Read(k.Peta);
                    var feature = new NetTopologySuite.Features.Feature(geom, new NetTopologySuite.Features.AttributesTable());
                    string contentStr = "<div id='content'>" +
                                            "<div id='siteNotice'>" +
                                            "</div>" +
                                            "<h1 id='firstHeading' class='firstHeading'>" + k.Nama + "</h1>" +
                                            "<div id='bodyContent'>" +
                                            "<p><b>" + k.Nama + "</b>, also referred to as <b>Ayers Rock</b>, is a large " +
                                            "sandstone rock formation in the southern part of the " +
                                            "Northern Territory, central Australia. It lies 335&#160;km (208&#160;mi) " +
                                            "south west of the nearest large town, Alice Springs; 450&#160;km " +
                                            "(280&#160;mi) by road. Kata Tjuta and Uluru are the two major " +
                                            "features of the Uluru - Kata Tjuta National Park. Uluru is " +
                                            "sacred to the Pitjantjatjara and Yankunytjatjara, the " +
                                            "Aboriginal people of the area. It has many springs, waterholes, " +
                                            "rock caves and ancient paintings. Uluru is listed as a World " +
                                            "Heritage Site.</p>" +
                                            "<p>Attribution: Uluru, <a href='https://en.wikipedia.org/w/index.php?title=Uluru&oldid=297882194'>" +
                                            "https://en.wikipedia.org/w/index.php?title=Uluru</a> " +
                                            "(last visited June 22, 2009).</p>" +
                                            "</div>" +
                                            "</div>";

                    feature.Attributes.AddAttribute("LOKASI", "AFDELING");
                    feature.Attributes.AddAttribute("NAMA_LOKASI", k.Nama);
                    feature.Attributes.AddAttribute("CENTER_X", feature.Geometry.Coordinates.Average(o => o.X));
                    feature.Attributes.AddAttribute("CENTER_Y", feature.Geometry.Coordinates.Average(o => o.Y));
                    feature.Attributes.AddAttribute("CONTENT", contentStr);
                    feature.Attributes.AddAttribute("COLOR", k.Warna);
                    feature.Attributes.AddAttribute("ID", k.LokasiId);
                    feature.Attributes.AddAttribute("FULLCOLOR", true);
                    featureCollection.Add(feature);
                }
            }

            var sb = new StringBuilder();
            var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, featureCollection);
            }

            return Content(sb.ToString(), "application/json");
        }

        public ActionResult BlokGeoJson(Guid afdelingId)
        {
            var mdl = (from m in CRUDBlok.CRUD.GetAllRecord().Where(o => o.AfdelingId == afdelingId)
                       join n in CRUDBlokRealisasi.CRUD.GetAllRecord() on m.BlokId equals n.BlokId
                       select new
                       {
                           LokasiId = m.BlokId,
                           Nama = m.Nama,
                           Warna = m.Warna,
                           Peta = n.Peta.ToString()
                       }).ToList();

            var featureCollection = new NetTopologySuite.Features.FeatureCollection();
            foreach (var k in mdl)
            {
                if (k.Peta != "")
                {
                    var wktReader = new NetTopologySuite.IO.WKTReader();
                    var geom = wktReader.Read(k.Peta);
                    var feature = new NetTopologySuite.Features.Feature(geom, new NetTopologySuite.Features.AttributesTable());
                    string contentStr = "<div id='content'>" +
                                            "<div id='siteNotice'>" +
                                            "</div>" +
                                            "<h1 id='firstHeading' class='firstHeading'>" + k.Nama + "</h1>" +
                                            "<div id='bodyContent'>" +
                                            "<p><b>" + k.Nama + "</b>, also referred to as <b>Ayers Rock</b>, is a large " +
                                            "sandstone rock formation in the southern part of the " +
                                            "Northern Territory, central Australia. It lies 335&#160;km (208&#160;mi) " +
                                            "south west of the nearest large town, Alice Springs; 450&#160;km " +
                                            "(280&#160;mi) by road. Kata Tjuta and Uluru are the two major " +
                                            "features of the Uluru - Kata Tjuta National Park. Uluru is " +
                                            "sacred to the Pitjantjatjara and Yankunytjatjara, the " +
                                            "Aboriginal people of the area. It has many springs, waterholes, " +
                                            "rock caves and ancient paintings. Uluru is listed as a World " +
                                            "Heritage Site.</p>" +
                                            "<p>Attribution: Uluru, <a href='https://en.wikipedia.org/w/index.php?title=Uluru&oldid=297882194'>" +
                                            "https://en.wikipedia.org/w/index.php?title=Uluru</a> " +
                                            "(last visited June 22, 2009).</p>" +
                                            "</div>" +
                                            "</div>";

                    feature.Attributes.AddAttribute("LOKASI", "BLOK");
                    feature.Attributes.AddAttribute("NAMA_LOKASI", k.Nama);
                    feature.Attributes.AddAttribute("CENTER_X", feature.Geometry.Coordinates.Average(o => o.X));
                    feature.Attributes.AddAttribute("CENTER_Y", feature.Geometry.Coordinates.Average(o => o.Y));
                    feature.Attributes.AddAttribute("CONTENT", contentStr);
                    feature.Attributes.AddAttribute("COLOR", k.Warna);
                    feature.Attributes.AddAttribute("ID", k.LokasiId);
                    feature.Attributes.AddAttribute("FULLCOLOR", true);
                    featureCollection.Add(feature);
                }
            }

            var sb = new StringBuilder();
            var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, featureCollection);
            }

            return Content(sb.ToString(), "application/json");
        }


    }
}