
function ssoSignInTry(userInfo) {    

    //init preloader
    ssoLoginManager.initPreloader();

    var applicationUser = {
        UserName: userInfo.preferred_username
    };

    if (enabledSSOLogin === 'True') {
        //close preloader
        ssoLoginManager.closePreloader();

        //go to dashboard
        var redirectUrl = `${app.getBaseUrl()}`;
        window.location.href = redirectUrl;

        return;
    }

    if (enabledSSOLogin === 'False') {

        $.post("/account/ssosignin", applicationUser, function (data) {
            //close preloader
            ssoLoginManager.closePreloader();

            if (!data.IsSuccess) {
                //go to logout and login page in auth service
                app.logoutForSSOLogin(); return;
            }
            //go to dashboard
            var redirectUrl = `${app.getBaseUrl()}/home/index`;
            window.location.href = redirectUrl;

        });
    }
}


const initOptions = {
    url: ApiRoutesConstants.AUTH_PATH,
    realm: 'GK_HEALTH',
    clientId: 'demo-asp-mvc-app',
    onLoad: 'login-required',
    //publicClent: true,
    //redirectUri: window.location.origin + '/sso-login'
}

const keycloak = Keycloak(initOptions);

async function getUserInfo(userInfoEndPoint, token) {
    const result = await fetch(userInfoEndPoint, {
        method: 'get',
        headers: new Headers({
            'Authorization': 'Bearer ' + token,
            'Content-Type': 'application/x-www-form-urlencoded'
        })

    });
    const user = await result.json();
    return user;
}

keycloak.init({ onLoad: initOptions.onLoad }).then((auth) => {
    if (!auth) {
        console.log('Authentication Failed')

    } else {
        console.log('Authenticated', auth);
        console.log('keycloak', keycloak);
        window.keycloak = keycloak;
        var userInfoEndPoint = `${ApiRoutesConstants.AUTH_PATH}/realms/GK_HEALTH/protocol/openid-connect/userinfo`;
        var token = keycloak.token;

        Cookies.set(CookieConstants.CURRENT_LOGGED_IN_ACCESSTOKEN, token, { expires: 7 })

        getUserInfo(userInfoEndPoint, token).then((res) => {
            console.log(res)           
            ssoSignInTry(res);
        });

        setInterval(() => {

            keycloak.updateToken(70).then((refreshed) => {

                if (refreshed) {
                    console.info('Token refreshed' + refreshed);
                } else {
                    console.info('Token not refreshed, valid for '
                        + Math.round(keycloak.tokenParsed.exp + keycloak.timeSkew - new Date().getTime() / 1000) + ' seconds');

                }
            }).catch(() => {
                console.error('Failed to refresh token');
            });
        }, 6000)
    }
}).catch(() => {
    console.error("Authenticated Failed");
});;



var ssoLoginManager = {
    initPreloader: function () {
        let $preloaderBlock = $('.preloader-block');
        if (!$preloaderBlock || $preloaderBlock.length<=0) return;
        
        $preloaderBlock.preloader({           
        });

        let $sectionPageFooter = $('.section-footer');
        if ($sectionPageFooter.length > 0) {
            $sectionPageFooter.attr('style', 'z-index: -1;opacity: 0.2;')
        }
    },
    closePreloader: function () {
        let $preloaderBlock = $('.preloader-block');
        $preloaderBlock.preloader('remove');
    },
}


$(function () {

})


