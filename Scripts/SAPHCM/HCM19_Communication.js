var menuId = '306B0DA3-0E26-2085-1761-A2CCD57E9370';

$('.k-window.titlebar').css('style', 'display: none');
$(".k-button-icontext").addClass("k-state-disabled").removeClass("k-grid-add");
$(document).ready(function () {
});

angular.module("__header", ["kendo.directives"])
    .controller("MyCtrl", function ($scope) {
        $scope.validate = function (event) {
            event.preventDefault();
            if ($scope.validator.validate()) {
                SimpanData().done(function () {
                    alert("Data Sudah Tersimpan...");
                });
            } else {
                alert("Ada Kesalahan Data!!!Tidak dapat disimpan...");
            }
        }
    });
$.extend({
    distinct: function (anArray) {
        var result = [];
        $.each(anArray, function (i, v) {
            if ($.inArray(v, result) == -1) result.push(v);
        });
        return result;
    }
});

$(window).on("beforeunload", function () {
    return "Please don't leave me!";
});

$(window).on("unload", function () {
    hapusPenggunaPortalYangAktif();
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

