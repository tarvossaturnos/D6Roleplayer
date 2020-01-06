"use strict";

document.getElementById("sendButton").disabled = true;

connection.on("UpdateDiceRolls", function (username, message, rolls, resultMessage, success) {
    var user = username.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    var encodedMsg = `Performs action ${msg}`;
    var resultMsg = `${rolls} - ${resultMessage}`;
    
    var li = document.createElement("li");

    var userDiv = document.createElement("b");
    userDiv.className = "roll-user";
    userDiv.textContent = user;

    var actionDiv = document.createElement("div");
    actionDiv.textContent = encodedMsg;

    var resultDiv = document.createElement("div");
    resultDiv.className = success ? "roll-succes" : "roll-failure";
    resultDiv.textContent = resultMsg;

    var hr = document.createElement("hr");

    li.appendChild(hr);
    li.append(userDiv);
    li.append(actionDiv);
    li.appendChild(resultDiv);

    var list = document.getElementById("messagesList");
    list.insertBefore(li, list.childNodes[0]);
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var username = document.getElementById("username").value;
    var message = document.getElementById("message").value;
    var count = document.getElementById("count").value;

    document.cookie = `user=${username}; expires=Sat, 1 Jan 2022 12:00:00 UTC; path=/`;

    connection.invoke("RequestDiceRoll", username, message, count)
        .catch(function (err) {
            window.alert("Connection to the session has been lost. Please reload the page." + err.toString());
            return console.error(err.toString());
    });
    event.preventDefault();
});