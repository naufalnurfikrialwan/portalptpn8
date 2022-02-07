
var wndLeaveGrid, wnd, wndDetail, prt;

$(document).ready(function () {
    getKodeKebun().done(function (data) {
        if (data) {
            $('#KodeKebun').val(data[0]);
            $('#NamaKebun').val(data[1]);
        }
    });
    prt = $("#printWindow").kendoWindow({
        title: "Print",
        modal: true,
        visible: false,
        resizable: false,
        width: 800,
        height: 500
    }).data("kendoWindow");
});

    

function getKodeKebun() {
    var url = '/Report/GetKodeKebun';
    return $.ajax({
        url: url,
        data: {},
        type: 'POST',
        datatype: 'json'
    });
}

function filterKebun() {
    return {
        key: "NamaKebun",
        value: $("#NamaKebun").val()
    };
}

function NamaKebunOnSelect(e) {
    var kebun = this.dataItem(e.item);
    if (typeof (kebun) != 'undefined') {
        $('#KodeKebun').val(kebun.kd_kbn);
        $('#NamaKebun').val(kebun.Nama);
    }
}

function btnProsesOnClick()
{
    var grid = $('#grdDetail').data('kendoGrid');
    grid.dataSource.read();
}

function filterReport(e) {
    return {
        strClassView: 'Ptpn8.Remise.Models.DaftarPengadaanBarangBahan',
        scriptSQL: "exec [SPDK_KanpusEF].[dbo].[DaftarPengadaanBarangBahan] '" + $('#KodeKebun').val() + "','" +
            $('#Bulan').val() + "','" + $('#Tahun').val() + "'"
    }

}

function btnPrintHeaderOnClick() {
    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { kebun: $('#KodeKebun').val(), bulan: $('#Bulan').val(), tahun: $('#Tahun').val() }
    });
    viewer.refreshReport();
    prt.open().center();
}
