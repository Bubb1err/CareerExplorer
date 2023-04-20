"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("ReceiveNotification", function (receiverId, content) {
    var p = document.createElement("p");
    document.getElementById("notification").appendChild(p);
    p.textContent = content;
    document.getElementById("notification").removeAttribute("hidden");
});

connection.start();