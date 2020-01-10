"use strict";

connection.on("UpdateInitiativeRolls", function (username, roll) {
    var user = username.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    var li = document.createElement("li");

    var rollDiv = document.createElement("div");
    rollDiv.textContent = `${roll} - ${user}`;

    li.append(rollDiv);

    var list, i, switching, b, shouldSwitch;
    list = document.getElementById("initiativeList");
    list.insertBefore(li, list.childNodes[0]);

    switching = true;
    while (switching) {
        switching = false;
        b = list.getElementsByTagName("LI");
        for (i = 0; i < (b.length - 1); i++) {
            shouldSwitch = false;

            if (Number(b[i].innerText.split(' ')[0]) < Number(b[i + 1].innerText.split(' ')[0])) {
                shouldSwitch = true;
                break;
            }
        }
        if (shouldSwitch) {
            b[i].parentNode.insertBefore(b[i + 1], b[i]);
            switching = true;
        }
    }
});

document.getElementById("initiativeButton").addEventListener("click", function (event) {
    var username = document.getElementById("username").value;
    var bonus = document.getElementById("bonus").value;

    document.cookie = `user=${username}; expires=Sat, 1 Jan 2022 12:00:00 UTC; path=/`;

    connection.invoke("RequestInitiativeRoll", username, bonus)
        .catch(function (err) {
            window.alert("Connection to the session has been lost. Please reload the page." + err.toString());
            return console.error(err.toString());
        });
    event.preventDefault();
});

connection.on("ResetInitiativeRolls", function () {
    var list = document.getElementById("initiativeList");
    list.innerHTML = '';
});

document.getElementById("resetInitiativeButton").addEventListener("click", function (event) {
    connection.invoke("RequestResetInitiativeRolls")
        .catch(function (err) {
            window.alert("Connection to the session has been lost. Please reload the page." + err.toString());
            return console.error(err.toString());
        });
    event.preventDefault();
});