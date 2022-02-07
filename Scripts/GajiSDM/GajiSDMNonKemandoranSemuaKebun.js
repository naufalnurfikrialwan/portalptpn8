
var wndLeaveGrid, wnd, wndDetail, prt, kd_kebun;

$(document).ready(function () {
    prt = $("#printWindow").kendoWindow({
        title: "Print",
        modal: true,
        visible: false,
        resizable: false,
        width: 800,
        height: 500
    }).data("kendoWindow");
});


function btnPrintHeaderOnClick() {
    var viewer = $("#reportViewer").data("telerik_ReportViewer");
    viewer.reportSource({
        report: viewer.reportSource().report,
        parameters: { KebunId: $('#KebunId').val(), Bulan: $('#Bulan').val(), Tahun: $('#Tahun').val() }
    });
    viewer.refreshReport();
    prt.open().center();
}
