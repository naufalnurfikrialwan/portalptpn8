var hdr_InvoiceId, err;
    var detailBaru = false;
    var userName;
	var menuId='D72772F3-3858-4C58-A2E5-645EA7775CC1';

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
		var h1 = parseInt($(".content-header").css("height"));
	    var h2 = parseInt($(".hdr").css("height"));
	    var h3 = parseInt($(".k-grid-toolbar").css("height"));
	    var h4 = parseInt($(".k-grid-header-wrap").css("height"));
	    var h5 = parseInt($(".__footer").css("height")) + 20;
	    $("#grdDetail").css("height", window.screen.height - (h1 + h2 + h3 + h4 + h5));
        err = $("#errWindow").kendoWindow({
            title: "Error Data",
            modal: true,
            visible: false,
            resizable: false,
            width: 300
        }).data("kendoWindow");
    });

$(window).on("beforeunload", function () {
	return "Please don't leave me!";
});

$(window).on("unload", function () {
    hapusPenggunaPortalYangAktif();
});


	function hapusPenggunaPortalYangAktif()
	{
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

	
    function grdOnChange(e) {
    }

    function grdOnEdit(e) {
        var model = e.model;
        uid = model.uid;
        this.expandRow(this.tbody.find("tr[data-uid='" + uid + "']"));
        if (model.isNew()) {
            model.id = model.uid;
            e.container.find("input[name=HDR_ShippingInstructionId]").val(model.id).change();
        }
        hdr_ShippingInstructionId = model.id;

		getUserName().done(function(data){
		        if (data) {
                    userName = data;
                }
                else {
                }
		}).fail(function(x){
			alert('Error! Hubungi Bagian TI');
		});
    }

	function getUserName()
	{
	    var url = '/Input/GetUserName';
        return $.ajax({
            url: url,
            data: {},
            type: 'GET',
            datatype: 'json'
            })
	}

	function grdOnDataBound(e) {
	    $("#grdDetail").find(".k-icon.k-i-collapse").trigger("click");
	}

    function filterGridUpdate(e) {
        var mdl = e.models;
        var arr = [];
        for (var i = 0; i < mdl.length; i++) {
            var m = mdl[i];
            if (m.VerKaur == true)
                m.UserNameKaur = userName;
            else
                m.UserNameKaur = "";
            arr.push(JSON.stringify(m));
        }
        return {
            _model: arr,
                _menuId: menuId
        };
    }

    function filterGridDestroy(e) {
        return {
            _model: JSON.stringify(e),
                _menuId: menuId
        };
    }
    function sendData() {
        var grid = $("#grdDetail").data("kendoGrid"),
            parameterMap = grid.dataSource.transport.parameterMap;

        //get the new and the updated records
        var currentData = grid.dataSource.data();
        var updatedRecords = [];
        var newRecords = [];

        for (var i = 0; i < currentData.length; i++) {
            if (currentData[i].isNew()) {
                //this record is new
                newRecords.push(JSON.stringify(currentData[i]));
            } else if (currentData[i].dirty) {
                updatedRecords.push(JSON.stringify(currentData[i]));
            }
        }

        //this records are deleted
        var deletedRecords = [];
        for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
            deletedRecords.push(JSON.stringify(grid.dataSource._destroyed[i]));
        }

        var data = {};
        $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }), parameterMap({ new: newRecords }), parameterMap({mnid: menuId}));
        _sendData(data).done(function (msg) {
            if (msg == "Success") {
                grid.dataSource.read([]);
                gridStatus = 'belum-ngapa-ngapain';
            }
            else {
                $('#errMsg').text(msg);
                gridStatus = 'sudah-nyoba-disimpan-tapi-model-masih-salah';
                openErrWindow();
            }
        }).fail(function (x) {
            $('#errMsg').text(msg);
            openErrWindow();
            grid.dataSource.read([]);
            gridStatus = 'sudah-nyoba-disimpan-tapi-gagal';
        });
    }

    function openErrWindow(e) {
        err.open().center();
        $("#ok").click(function () {
            err.close();
        });
    }

    function _sendData(data) {
        return $.ajax({
            url: "/Input/UpdateCreateDelete",
            data: data, 
            type: "POST"
        })
    }
    function filterdetailRead(e) {
        return { _menuId: menuId };
    }

    function filterdetailCreate(e) {
        return { _menuId: menuId };
    }
