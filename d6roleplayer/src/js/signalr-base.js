const params = new URLSearchParams(window.location.search);
const id = params.get('sessionId');
var connection = new signalR.HubConnectionBuilder().withUrl(`/diceRollHub?sessionId=${id}`).build();

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    window.alert("Could not connect to the session. Please reload the page.")
    return console.error(err.toString());
});