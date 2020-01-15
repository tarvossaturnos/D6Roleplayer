"use strict";

document.getElementById("sendButton").disabled = true;

connection.on("UpdateDiceRolls", function (username, message, rolls, resultMessage, success) {
    // User.
    username = username.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var userDiv = document.createElement("b");
    userDiv.className = "roll-user";
    userDiv.textContent = username;

    // Description.
    message = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    message = `Performs action ${message}`;
    var messageDiv = document.createElement("div");
    messageDiv.textContent = message;

    // Result.
    resultMessage = `${rolls} - ${resultMessage}`; 
    var resultDiv = document.createElement("div");
    resultDiv.className = success ? "roll-succes" : "roll-failure";
    resultDiv.textContent = resultMessage;

    var hr = document.createElement("hr");
    var li = document.createElement("li");
    li.appendChild(hr);
    li.append(userDiv);
    li.append(messageDiv);
    li.appendChild(resultDiv);

    var list = document.getElementById("diceRolls");
    list.insertBefore(li, list.childNodes[0]);
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var username = document.getElementById("username").value;
    var message = document.getElementById("message").value;
    var count = document.getElementById("count").value;
    
    setCookie("user", username);

    connection.invoke("RequestDiceRoll", username, message, count)
        .catch(function (err) {
            window.alert("Connection to the session has been lost. Please reload the page." + err.toString());
            return console.error(err.toString());
    });
    event.preventDefault();
});