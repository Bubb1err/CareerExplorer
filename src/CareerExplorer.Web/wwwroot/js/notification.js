"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("ReceiveNotification", function (receiverId, content) {
    var a = document.createElement("a");
    document.getElementById("notification").appendChild(a);
    a.textContent = content;
    a.href = content;
    document.getElementById("notification").removeAttribute("hidden");
});

connection.start();