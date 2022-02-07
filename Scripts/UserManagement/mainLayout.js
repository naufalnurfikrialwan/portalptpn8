var broadcastRoom = "broadcastRoom"
var chatRoom = "chatRoom"
var winBroadcast, winChat, winChatGrid;


kendo.culture("id-ID");
$(document).ready(function () {
    $('#kmenu').css("display", "block");
    $('#kmenu').kendoMenu({
        openOnClick: true
    });

    winBroadcast = $('#broadcastWindow').kendoWindow({
        title: "Broadcast Message",
        modal: true,
        visible: false,
        resizable: false,
        width: 400,
        height: 250
    }).data('kendoWindow');

    winChat = $('#chatWindow').kendoWindow({
        title: "Private Message",
        modal: true,
        visible: false,
        resizable: false,
        width: 800,
        height: 500
    }).data('kendoWindow');

    winChatGrid = $('#chatGrid').kendoWindow({
        title: "Chat Dengan ",
        modal: true,
        visible: false,
        resizable: false,
        width: 800,
        height: 300
    }).data('kendoWindow');

});

$(function () {
    // Declare a proxy to reference the hub.
    var chat = $.connection.chatHub;
    // Create a function that the hub can call to broadcast messages.
    chat.client.broadcastMessage = function (data) {
        // Html encode display name and message.
        var grid = $('#grdBroadcast').data('kendoGrid');
        grid.dataSource.data(data);
        var aSound = document.createElement('audio');
        aSound.setAttribute('src', '/Sounds/Notify.wav');
        aSound.play();
    };
    chat.client.hubReceived = function (data) {
        $('#welcome').text(data);
    };

    $('#txtBroadcastMessage').focus();

    // Start the connection.
    $.connection.hub.start().done(function () {
        $('#btnBroadcastMessage').click(function () {
            // Call the Send method on the hub.
            chat.server.broadcast(broadcastRoom, $('#txtBroadcastMessage').val());
            // Clear text box and reset focus for next comment.
            $('#txtBroadcastMessage').val('').focus();
        });

        $('#btnPrivateChat').click(function () {
            // Call the Send method on the hub.
            //chat.server.broadcast(chatRoom, $('#txtBroadcastMessage').val());
            // Clear text box and reset focus for next comment.
            //$('#txtBroadcastMessage').val('').focus();

            $('#grdPrivateChat').data('kendoGrid').dataSource.read();
            winChat.open().center();
        });

    });
});

function filterReadChat() {
    return { roomName: chatRoom }
}

function onCloseBroadcast() {
    $("#btnBroadcast").show();
}

function getPrivateChat(e) {
    var grid = $('#grdPrivateChat').data('kendoGrid');

    winChatGrid.open().center();

}