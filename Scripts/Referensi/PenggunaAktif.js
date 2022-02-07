var menuId = '3390B02F-6F64-4943-AAF9-179B70C26674';
$(document).ajaxStart(function () {
    $("#_header").addClass('disabledbutton');
    $("#_detail").addClass('disabledbutton');
    $("#_footer").addClass('disabledbutton');
    $("#progress").show();
}).ajaxStop(function () {
    $("#progress").hide();
    $("#_header").removeClass('disabledbutton');
    $("#_detail").removeClass('disabledbutton');
    $("#_footer").removeClass('disabledbutton');
});
$(document).ready(function () {
    var h1 = isNaN(parseInt($(".content-header").css("height"))) ? 0 : parseInt($(".content-header").css("height"));
    var h2 = isNaN(parseInt($(".hdr").css("height"))) ? 0 : parseInt($(".hdr").css("height"));
    var h3 = isNaN(parseInt($(".k-grid-toolbar").css("height"))) ? 50 : parseInt($(".k-grid-toolbar").css("height"));
    var h4 = isNaN(parseInt($(".k-grid-header-wrap").css("height"))) ? 0 : parseInt($(".k-grid-header-wrap").css("height"));
    var h5 = isNaN(parseInt($(".__footer").css("height")) + 20)? 0 : parseInt($(".__footer").css("height")) + 20;
	$("#grdDetail").css("height", window.screen.height - (h1 + h2 + h3 + h4 + h5));
    gridStatus = 'belum-ngapa-ngapain';
});

$(window).on({
		beforeunload: function(){
			return "Please don't leave me!";
		},
		unload: function(){
			hapusPenggunaPortalYangAktif();
		}
});

function hapusPenggunaPortalYangAktif() {
    var url = '/Account/HapusPenggunaPortalYangAktif';
    return $.ajax({
        url: url,
        data: {
            _menuId: menuId
        },
        type: 'POST',
        datatype: 'json'
    });
}

function filterdetailRead(e) {
    return { _menuId: menuId };
}