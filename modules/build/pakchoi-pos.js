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
        };
        return login;
    }(Bootstrap));
    pages.login = login;
})(pages || (pages = {}));
/// <reference path="../../build/linq.d.ts" />
/// <reference path="pages/login.ts" />
var app;
(function (app) {
    function start() {
        Router.AddAppHandler(new pages.login());
        Router.RunApp();
    }
    app.start = start;
})(app || (app = {}));
$ts.mode = Modes.debug;
$ts(app.start);
//# sourceMappingURL=pakchoi-pos.js.map