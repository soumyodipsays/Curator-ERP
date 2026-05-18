var Auth = (function () {

    var config = window.AppConfig || {};

    var profileUrl = config.profileUrl || "";
    var loginPage = config.mvcLoginPage || "/";
    var refreshTokenUrl = config.refreshTokenUrl || "";
    var logoutUrl = config.logoutUrl || "";
    var refreshPromise = null;


    // TOKEN METHODS
    function getToken() {

        return sessionStorage.getItem("token");
    }

    function saveToken(token) {

        if (!token) {

            console.error("TOKEN IS EMPTY");
            return;
        }

        sessionStorage.setItem("token", token);
    }

    function login(token) {

        saveToken(token);
    }

    function clearTokens() {

        sessionStorage.removeItem("token");
    }

    function clear() {

        clearTokens();

        sessionStorage.removeItem("currentUser");
    }

    // LOGOUT
    async function logout(msg) {

        try {

            if (logoutUrl) {

                await $.ajax({

                    url: logoutUrl,
                    type: "POST",

                    xhrFields: {
                        withCredentials: true
                    }
                });
            }
        }
        catch (e) {

            console.warn("Logout API failed:", e);
        }

        clear();

        if (msg) {
            alert(msg);
        }

        setTimeout(function () {

            window.location.href = loginPage;

        }, 500);
    }

    // JWT PARSER
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

    // TOKEN EXPIRY
    function isExpired(token) {

        var payload = parseJwt(token);

        if (!payload || !payload.exp) {

            console.warn("Invalid token or missing exp");

            return true;
        }

        var currentTime =
            Math.floor(Date.now() / 1000);

        // 30-second safety buffer
        return payload.exp < (currentTime + 30);
    }

    // REFRESH ACCESS TOKEN
    async function refreshAccessToken() {

        // Prevent concurrent refresh requests
        if (refreshPromise) {

            return refreshPromise;
        }

        refreshPromise = $.ajax({

            url: refreshTokenUrl,
            type: "POST",

            xhrFields: {
                withCredentials: true
            }

        }).then(function (res) {

            if (!res.success || !res.accessToken) {

                throw new Error("Invalid refresh response");
            }

            saveToken(res.accessToken);

            refreshPromise = null;

            return res.accessToken;

        }).catch(function (err) {

            refreshPromise = null;

            console.error("Refresh token failed:", err);

            clear();

            return null;
        });

        return refreshPromise;
    }

    // VALIDATE SESSION
    async function validate(success, fail) {

        try {

            var token = getToken();

            // No token at all
            if (!token) {

                return false;
            }

            // Access token expired
            if (isExpired(token)) {

                token = await refreshAccessToken();

                if (!token) {

                    if (fail) {
                        fail();
                    }

                    return false;
                }
            }

            // Optional profile validation
            if (profileUrl) {

                const res = await $.ajax({

                    url: profileUrl,
                    type: "POST",

                    headers: {
                        Authorization: "Bearer " + token
                    }
                });

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

                    if (success) {
                        success(res);
                    }

                    return true;
                }
            }

            return true;
        }
        catch (xhr) {

            console.error("Validation Error:", xhr);

            if (xhr.status === 401) {

                try {

                    var newToken =
                        await refreshAccessToken();

                    if (newToken) {

                        if (success) {
                            success();
                        }

                        return true;
                    }
                }
                catch (e) {

                    console.error(e);
                }
            }

            if (fail) {
                fail(xhr);
            }

            return false;
        }
    }

    // CURRENT USER
    function currentUser() {

        var data =
            sessionStorage.getItem("currentUser");

        return data
            ? JSON.parse(data)
            : null;
    }

    // GLOBAL AJAX TOKEN HANDLER
    function ajaxToken() {

        $(document).ajaxSend(function (event, xhr) {

            var token = getToken();

            if (token) {

                xhr.setRequestHeader(

                    "Authorization",
                    "Bearer " + token
                );
            }
        });
    }


    // PAGE PROTECTION
    function protectPage() {

        $(async function () {

            var valid = await validate();

            if (!valid) {

                await logout(
                    "Please login to continue"
                );

                return;
            }

            ajaxToken();
        });
    }

    // PUBLIC API
    return {

        login: login,
        saveToken: saveToken,
        getToken: getToken,
        logout: logout,
        validate: validate,
        protectPage: protectPage,
        currentUser: currentUser,
        ajaxToken: ajaxToken,
        refreshAccessToken: refreshAccessToken,
        parseJwt: parseJwt,
        isExpired: isExpired
    };

})();