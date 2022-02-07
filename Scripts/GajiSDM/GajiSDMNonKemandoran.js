
var wndLeaveGrid, wnd, wndDetail, prt, kd_kebun;

$(document).ready(function () {
    getKodeKebun().done(function (data) {
        if (data) {
            $('#KebunId').val(data[0]);
            kd_kebun = $('#KebunId').val(data[0]);
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
    var url = '/Report/GetKebunId';
    return $.ajax({
        url: url,
        data: {},
        type: 'POST',
        datatype: 'json'
    });
}

function btnPrintHeaderOnClick() {
    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { KebunId: $('#KebunId').val(), Bulan: $('#Bulan').val(), Tahun: $('#Tahun').val() }
    });
    viewer.refreshReport();
    prt.open().center();
}
