export default {
    setCookie: function (cname, cvalue, exdays) {
        const d = new Date();
        d.setTime(d.getTime() + (exdays * 3600 * 1000 * 24));
        let expires = "expires=" + d.toUTCString();
        document.cookie = `${cname}=${encodeURIComponent(cvalue)};${expires};path=/`;
    },

    getCookie: function (cname) {
        let name = cname + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    },

    deleteCookie: (cname) => {
        if (getCookie(cname)) {
            document.cookie = `${cname}=;expires=Thu, 01 Jan 1970 00:00:01 GMT;path=/;domain=${location.hostname}`;
        }
    }
}