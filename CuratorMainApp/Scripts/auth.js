var Auth = (function () {

    var profileUrl = window.AppConfig.profileUrl;
    var loginPage = "/User/Login";
    function getToken() {
        return localStorage.getItem("token") ||
            sessionStorage.getItem("token");
    }

    function saveToken(token, remember, UserID) {

        clear();

        if (remember) {
            localStorage.setItem("token", token);
        }
        else {
            sessionStorage.setItem("token", token);
        }

        localStorage.setItem("UserID", UserID);
    }
    function login(token, remember) {

        saveToken(token, remember);
    }

    function clear() {
        localStorage.removeItem("token");
        sessionStorage.removeItem("token");
        sessionStorage.removeItem("currentUser");
    }

    function logout(msg) {

        clear();

        if (msg)
            alert(msg);

        window.location.href = loginPage;
    }

    function parseJwt(token) {

        try {
            return JSON.parse(atob(token.split('.')[1]));
        }
        catch {
            return null;
        }
    }

    function isExpired(token) {

        var payload = parseJwt(token);

        if (!payload || !payload.exp)
            return true;

        return payload.exp < Math.floor(Date.now() / 1000);
    }

    function validate(success, fail) {

        var token = getToken();

        if (!token) {
            if (fail) fail();
            return;
        }

        if (isExpired(token)) {
            logout("Session expired");
            return;
        }

        $.ajax({
            url: profileUrl,
            type: "POST",
            headers: {
                Authorization: "Bearer " + token
            },
            success: function (res) {

                if (res.success) {

                    sessionStorage.setItem(
                        "currentUser",
                        JSON.stringify(res)
                    );

                    if (success) success(res);
                }
                else {
                    logout("Invalid token");
                }
            },
            error: function (xhr) {

                if (xhr.status === 401)
                    logout("Unauthorized");
                else if (xhr.status === 404)
                    alert("Profile route not found");
                else
                    alert("Server error");
            }
        });
    }

    function protectPage() {
        $(function () {
            validate();
        });
    }

    function currentUser() {

        var data = sessionStorage.getItem("currentUser");

        return data ? JSON.parse(data) : null;
    }

    function ajaxToken() {

        $.ajaxSetup({
            beforeSend: function (xhr) {

                var token = getToken();

                if (token) {
                    xhr.setRequestHeader(
                        "Authorization",
                        "Bearer " + token
                    );
                }
            }
        });
    }

    return {
        login: login, 
        saveToken: saveToken,
        getToken: getToken,
        logout: logout,
        validate: validate,
        protectPage: protectPage,
        currentUser: currentUser,
        ajaxToken: ajaxToken
    };

})();