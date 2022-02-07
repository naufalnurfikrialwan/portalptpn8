var menuId;
var xmlHttp;
var minDate = new Date();
var maxDate = new Date();
var TanggalAwalServer = new Date();
var TanggalAkhirServer = new Date();
var TglDisabled = [];
var DataIjinUploadUlang = [];

$(document).ajaxStart(function () {
    $("#progress").show();
}).ajaxStop(function () {
    $("#progress").hide();
});

$(document).ready(function () {
    menuId = "2d0f74c8-7e06-4eb5-b0e8-0393f4a5d356";

    srvTime().done(function (data) {
        if(data)
        {
            minDate = new Date($('#nTahun').val(), $('#nBulan').val()-1, 1)             
            for (var i = 31; i >= 1; i--) {
                var date = new Date($('#nTahun').val(), $('#nBulan').val() - 1, i);
                if (date.getMonth() + 1 == $('#nBulan').val()) {
                    maxDate = date;
                    break;
                }
            }

            TanggalAkhirServer = new Date(data[0][0], data[0][1], data[0][2]);
            TanggalAwalServer.setDate(TanggalAkhirServer.getDate() - $('#PengurangHari').val());

            var dp = $('#dtpTanggalTransaksi').data("kendoDatePicker");
            dp.setOptions({
                min: new Date(minDate),
                max: new Date(maxDate),
                disableDates: function (date) {
                    var disabled = HitungDisabled();
                    if (date && disabled.indexOf(date.getDate()) > -1) {
                        return true;
                    } else {
                        return false;
                    }
                }
            });


        }
    });
    
    var validator = $("#PilihTanggal").kendoValidator({
        rules: {
            datepicker: function(input) {
                if (input.is("[data-role=datepicker]")) {
                    var r = input.data("kendoDatePicker").value()

                    if (r == null)
                    {
                        $('#files').data('kendoUpload').disable();
                    }
                    else
                    {
                        if (r.getMonth() + 1 == $('#nBulan').val() && r.getFullYear() == $('#nTahun').val()) {
                            $('#files').data('kendoUpload').enable();
                        }
                        else
                        {
                            r = null;
                            $('#files').data('kendoUpload').disable();
                        }
                    }

                    return r;
                } else {
                    return true;
                }
            }
        },
        messages: {
            datepicker: "Please enter valid date!"
        }
    }).data("kendoValidator");

    AmbilLokasiKebun().done(function (data) {
        if (data) {
            $('#KebunId').val(data[0][0]);
        }
    });

});



function srvTime() {
    var url = '/UploadDataHarian/ExecSQLData';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select year(GETDATE()) as tahun, month(getdate())-1 as bulan, day(getdate()) as hari ",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function txtKeyCodeOnChange(e)
{
    IjinUploadUlang();
}

function AmbilLokasiKebun()
{
    var url = '/UploadDataHarian/ExecSQLData';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "select PosisiLokasiKerjaId from [Identity].[dbo].[AspNetUsers] where UserName='"+$('#UserName').val()+"'",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });

}

function IjinUploadUlang()
{
    $('#files').data('kendoUpload').disable();
    AmbilIjinUploadUlang().done(function (data) {
        if(data.length>0)
        {
            for (var j = 0; j < data.length; j++)
            {
                var tglAwal = new Date(data[j][0]);
                var tglAkhir = new Date(data[j][1]);
                for (var i = tglAwal.getDate() ; i <= tglAkhir.getDate() ; i++) {
                    DataIjinUploadUlang.push(i);
                }
            }
        }
        else
        {
            DataIjinUploadUlang = []
        }

        minDate = new Date($('#nTahun').val(), $('#nBulan').val() - 1, 1);
        for (var i = 31; i >= 1; i--) {
            var date = new Date($('#nTahun').val(), $('#nBulan').val() - 1, i);
            if (date.getMonth() + 1 == $('#nBulan').val()) {
                maxDate = new Date(date);
                break;
            }
        }
        var dp = $('#dtpTanggalTransaksi').data("kendoDatePicker");
        dp.setOptions({
            min: new Date(minDate),
            max: new Date(maxDate),
            disableDates: function (date) {
                var disabled = HitungDisabled();
                if (date && disabled.indexOf(date.getDate()) > -1) {
                    return true;
                } else {
                    return false;
                }
            }
        });

    });
}

function AmbilIjinUploadUlang()
{
    var url = '/UploadDataHarian/ExecSQLData';
    return $.ajax({
        url: url,
        data: {
            scriptSQL: "set dateformat mdy; select cast(month(tanggal_awal_upload) as varchar(10))+'/'+cast(day(tanggal_awal_upload) as varchar(10))+'/'+cast(year(tanggal_awal_upload) as varchar(10)) as awal, " +
                "cast(month(tanggal_akhir_upload) as varchar(10))+'/'+cast(day(tanggal_akhir_upload) as varchar(10))+'/'+cast(year(tanggal_akhir_upload) as varchar(10)) as akhir " +
                "from [SPDK_KanpusEF].[dbo].[DTL_IjinUploadUlang] " +
                "where KebunId='"+$('#KebunId').val()+"' and SudahUploadUlang=0 and KeyCodeUploadUlang='"+$('#txtKeyCode').val()+"' "+
                "and month(tanggal_awal_upload)=" + $('#nBulan').val() + " and month(tanggal_akhir_upload)=" + $('#nBulan').val() + " " +
                "and year(tanggal_awal_upload)=" + $('#nTahun').val() + " and year(tanggal_akhir_upload)=" + $('#nTahun').val() + " " +
                "and getdate()<=tanggal_kadaluwarsa",
            _menuId: menuId
        },
        type: 'GET',
        datatype: 'json'
    });
}

function HitungDisabled()
{
    var tgl = new Date();
    var nBulan = parseInt($('#nBulan').val())-1;
    var nTahun = parseInt($('#nTahun').val());
    var dates = [];
    for (var i = 1; i <= 31; i++) {
        tgl = new Date(nTahun, nBulan, i);
        if (tgl.getMonth() == nBulan) {
            if ((tgl < TanggalAwalServer || tgl > TanggalAkhirServer) && DataIjinUploadUlang.indexOf(i)==-1) {
                dates.push(i);   // tanggal yang di disable
            }
        }
    }
    return dates;
}

function BulanTahunOnChange()
{
    IjinUploadUlang();
}



