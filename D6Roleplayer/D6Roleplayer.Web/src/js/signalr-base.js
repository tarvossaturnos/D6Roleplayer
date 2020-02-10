"use strict";

const params = new URLSearchParams(window.location.search);
const id = params.get('sessionId');
var connection = new signalR.HubConnectionBuilder().withUrl(`/diceRollHub?sessionId=${id}`).build();

connection.start().then(function () {
    document.getElementById("sessionConnected").textContent = "Connected";
    document.getElementById("sendButton").disabled = false;
    connection.invoke("UpdateUserCount");
}).catch(function (err) {
    window.alert("Could not connect to the session. Please reload the page.")
    return console.error(err.toString());
});

connection.onclose(function() {
    document.getElementById("sessionConnected").textContent = "Not connected";    
    connection.invoke("UpdateUserCount");
});

connection.on("UpdateUserCount", function (amount) {
    var userCount = parseInt(document.getElementById('userCount').innerHTML);
    userCount = amount;    
    document.getElementById('userCount').innerHTML = userCount;
});