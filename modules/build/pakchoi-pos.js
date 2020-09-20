var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../../build/linq.d.ts" />
var app;
(function (app) {
    function start() {
        Router.AddAppHandler(new pages.login());
        Router.AddAppHandler(new pages.lockscreen());
        Router.RunApp();
    }
    app.start = start;
})(app || (app = {}));
$ts.mode = Modes.debug;
$ts(app.start);
var nifty;
(function (nifty) {
    function errorMsg(msg) {
        $.niftyNoty({
            type: 'danger',
            message: msg,
            container: 'floating',
            timer: 5000
        });
    }
    nifty.errorMsg = errorMsg;
})(nifty || (nifty = {}));
var pages;
(function (pages) {
    var lockscreen = /** @class */ (function (_super) {
        __extends(lockscreen, _super);
        function lockscreen() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        Object.defineProperty(lockscreen.prototype, "appName", {
            get: function () {
                return "lockscreen";
            },
            enumerable: true,
            configurable: true
        });
        lockscreen.prototype.init = function () {
        };
        lockscreen.prototype.unlock = function () {
            var passwd = $ts.value("#passwd");
            if (Strings.Empty(passwd)) {
                nifty.errorMsg("<strong>对不起，</strong>密码不可以为空！");
            }
            else {
                $ts.post("@unlock", { passwd: md5(passwd) }, function (result) {
                    if (result.code == 0) {
                        $goto("/");
                    }
                    else {
                        nifty.errorMsg(result.info);
                    }
                });
            }
        };
        return lockscreen;
    }(Bootstrap));
    pages.lockscreen = lockscreen;
})(pages || (pages = {}));
var pages;
(function (pages) {
    var login = /** @class */ (function (_super) {
        __extends(login, _super);
        function login() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        Object.defineProperty(login.prototype, "appName", {
            get: function () {
                return "login";
            },
            enumerable: true,
            configurable: true
        });
        login.prototype.init = function () {
            var user = localStorage.getItem("user");
            if (!Strings.Empty(user, true)) {
                $ts.value("#user", user);
                $input("#remember_user").checked = true;
            }
        };
        login.prototype.login = function () {
            var user = $ts.value("#user");
            var passwd = md5($ts.value("#passwd"));
            var post = { user: user, passwd: passwd };
            if (Strings.Empty(user, true)) {
                nifty.errorMsg("<strong>对不起，</strong>输入的账号不可以为空！");
            }
            else if (Strings.Empty($ts.value("#passwd"))) {
                nifty.errorMsg("<strong>对不起，</strong>输入的密码不可以为空！");
            }
            else {
                $ts.post("@login", post, function (result) {
                    if (result.code == 0) {
                        localStorage.setItem("user", user);
                        $goto("/");
                    }
                    else {
                        nifty.errorMsg(result.info);
                    }
                });
            }
        };
        return login;
    }(Bootstrap));
    pages.login = login;
})(pages || (pages = {}));
//# sourceMappingURL=pakchoi-pos.js.map