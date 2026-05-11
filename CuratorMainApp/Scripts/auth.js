var Auth = (function () {

    var config = window.AppConfig || {};

    var profileUrl = config.profileUrl || "";
    var loginPage = config.mvcLoginPage || "/";

    console.log("Profile URL:", profileUrl);
    console.log("Login Page:", loginPage);

    function getToken() {

        return localStorage.getItem("token") ||
            sessionStorage.getItem("token");
    }

    function saveToken(token, remember, UserID) {

        console.log("Saving Token:", token);

        clear();

        if (!token) {

            console.error("TOKEN IS EMPTY");
            return;
        }

        if (remember) {
            localStorage.setItem("token", token);
        }
        else {
            sessionStorage.setItem("token", token);
        }

        localStorage.setItem("UserID", UserID);
    }

    function login(token, remember, UserID) {

        saveToken(token, remember, UserID);
    }

    function clear() {

        localStorage.removeItem("token");
        localStorage.removeItem("UserID");

        sessionStorage.removeItem("token");
        sessionStorage.removeItem("currentUser");
    }

    function logout(msg) {

        clear();

        if (msg) {
            alert(msg);
        }

        setTimeout(function () {
            window.location.href = loginPage;
        }, 2000);
    }

    function parseJwt(token) {

        try {

            var base64Url = token.split('.')[1];

            var base64 = base64Url
                .replace(/-/g, '+')
                .replace(/_/g, '/');

            var jsonPayload = decodeURIComponent(
                atob(base64)
                    .split('')
                    .map(function (c) {

                        return '%' + (
                            '00' + c.charCodeAt(0).toString(16)
                        ).slice(-2);

                    }).join('')
            );

            return JSON.parse(jsonPayload);
        }
        catch (e) {

            console.error("JWT Parse Error:", e);
            return null;
        }
    }

    function isExpired(token) {

        var payload = parseJwt(token);

        console.log("JWT Payload:", payload);

        if (!payload || !payload.exp) {
            console.log("Token invalid or exp missing");
            return true;
        }

        var currentTime = Math.floor(Date.now() / 1000);

        console.log("Current Time:", currentTime);
        console.log("Token Expiry:", payload.exp);

        return payload.exp < currentTime;
    }

    function validate(success, fail) {

        var token = getToken();

        if (!token) {

            if (fail) {
                fail();
            }

            return;
        }

        if (isExpired(token)) {

            logout("Session expired");
            return;
        }

        if (!profileUrl) {

            console.error("Profile URL missing.");
            logout("Configuration error");
            return;
        }

        $.ajax({

            url: profileUrl,
            type: "POST",

            headers: {
                Authorization: "Bearer " + token
            },

            success: function (res) {

                console.log("PROFILE RESPONSE:", res);

                if (res.success) {

                    sessionStorage.setItem(
                        "currentUser",
                        JSON.stringify({
                            userId: res.userId,
                            userName: res.userName,
                            email: res.email,
                            role: res.role
                        })
                    );

                    console.log(
                        "CURRENT USER:",
                        sessionStorage.getItem("currentUser")
                    );

                    if (success) {
                        success(res);
                    }
                }
            },

            error: function (xhr) {

                if (xhr.status === 401) {

                    logout("Unauthorized");
                }
                else if (xhr.status === 404) {

                    console.error("Profile route not found");
                    alert("Profile API not found.");
                }
                else {

                    console.error("Server Error:", xhr);
                    alert("Server error occurred.");
                }
            }
        });
    }

    function protectPage() {

        $(function () {

            validate(function () {

                ajaxToken();
            });
        });
    }

    function currentUser() {

        var data = sessionStorage.getItem("currentUser");

        return data
            ? JSON.parse(data)
            : null;
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