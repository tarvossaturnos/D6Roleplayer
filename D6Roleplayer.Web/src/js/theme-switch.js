"use strict";

loadThemeFromCookie();

function loadThemeFromCookie() {
    let cookie = getCookie("theme");

    if (cookie == "light") {
        document.getElementById('theme-switch').checked = true;
        switchTheme();
    }
}

function switchTheme() {;
    let rootClassList = document.getElementsByTagName("html")[0].classList;

    if (document.getElementById('theme-switch').checked) {
        rootClassList.remove('theme-dark');        
        rootClassList.add('theme-light');
        setCookie("theme", "light");
    } else {
        rootClassList.remove('theme-light');        
        rootClassList.add('theme-dark');
        setCookie("theme", "dark");  
    }
};