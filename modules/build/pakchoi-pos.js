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
        Router.AddAppHandler(new pages.password_reminder());
        Router.AddAppHandler(new pages.lockscreen());
        Router.AddAppHandler(new pages.inventories());
        Router.AddAppHandler(new pages.goods());
        Router.AddAppHandler(new pages.vendor());
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
    function showAlert(message) {
        $ts("#alert").show();
        $ts("#message").show().display(message);
    }
    nifty.showAlert = showAlert;
})(nifty || (nifty = {}));
var pages;
(function (pages) {
    var goods = /** @class */ (function (_super) {
        __extends(goods, _super);
        function goods() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        Object.defineProperty(goods.prototype, "appName", {
            get: function () {
                return "goods";
            },
            enumerable: true,
            configurable: true
        });
        ;
        goods.prototype.init = function () {
            $ts.get("@vendors", function (result) {
                if (result.code != 0) {
                    nifty.errorMsg("对不起，加载供应商信息失败，请刷新页面重试。。。");
                }
                else {
                    var selects = $ts("#vendor");
                    for (var _i = 0, _a = result.info; _i < _a.length; _i++) {
                        var vendor_1 = _a[_i];
                        selects.appendElement($ts("<option>", { value: vendor_1.id }).display(vendor_1.name));
                    }
                }
            });
            $ts.get("@load", function (result) {
                if (result.code != 0) {
                    nifty.errorMsg("对不起，商品信息加载失败，请刷新页面重试。。。");
                }
                else {
                    var list = $ts("#list").clear();
                    var tr = void 0;
                    var str = void 0;
                    for (var _i = 0, _a = result.info; _i < _a.length; _i++) {
                        var goods_1 = _a[_i];
                        tr = $ts("<tr>");
                        if (goods_1.gender == 0) {
                            str = goods_1.name + "\uFF08\u5973\u88C5\uFF09";
                        }
                        else if (goods_1.gender == 1) {
                            str = goods_1.name + "\uFF08\u7537\u88C5\uFF09";
                        }
                        else {
                            str = goods_1.name;
                        }
                        tr.appendElement($ts("<td>").display(goods_1.name));
                        tr.appendElement($ts("<td>").display(goods_1.vendor));
                        tr.appendElement($ts("<td>").display(goods_1.add_time));
                        tr.appendElement($ts("<td>").display("0"));
                        tr.appendElement($ts("<td>").display(goods_1.price));
                        tr.appendElement($ts("<td>").display(goods_1.note));
                        tr.appendElement($ts("<td>").display(goods_1.operator));
                        list.appendElement(tr);
                    }
                }
            });
        };
        goods.prototype.save = function () {
            var item_id = $ts.value("#item_id");
            var name = $ts.value("#name");
            var price = $ts.value("#price");
            if (Strings.Empty(item_id, true)) {
                return nifty.showAlert("商品编号不可以为空！");
            }
            else if (Strings.Empty(name, true)) {
                return nifty.showAlert("请填写商品名称。");
            }
            else if (Strings.Empty(price, true)) {
                return nifty.showAlert("请填写商品价格！");
            }
            else if (!Strings.isNumericPattern(price)) {
                return nifty.showAlert("商品价格的格式不正确！");
            }
            var data = {
                item_id: item_id,
                name: name,
                vendor_id: $ts.select.getOption("#vendor"),
                price: parseFloat(price),
                note: $ts.value("#note"),
                gender: parseInt($ts.select.getOption("#gender"))
            };
            $ts.post("@save", data, function (result) {
                if (result.code == 0) {
                    location.reload();
                }
                else {
                    nifty.showAlert(result.info);
                }
            });
        };
        return goods;
    }(Bootstrap));
    pages.goods = goods;
})(pages || (pages = {}));
var pages;
(function (pages) {
    var inventories = /** @class */ (function (_super) {
        __extends(inventories, _super);
        function inventories() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        Object.defineProperty(inventories.prototype, "appName", {
            get: function () {
                return "inventories";
            },
            enumerable: true,
            configurable: true
        });
        inventories.prototype.init = function () {
        };
        /**
         * 商品入库
        */
        inventories.prototype.save = function () {
            var item_id = $ts.value("#item_id");
            var batch_id = $ts.value("#batch_id");
            if (Strings.Empty(item_id, true)) {
                return nifty.showAlert("商品编号不可以为空！");
            }
            if (Strings.Empty(batch_id, true)) {
                batch_id = "";
            }
            var count = $ts.value("#count");
            if (!Strings.isIntegerPattern(count)) {
                return nifty.showAlert("商品件数错误，商品件数应该是一个大于零的整数！");
            }
            var note = $ts.value("#note");
            var post = {
                item_id: item_id,
                batch_id: batch_id,
                count: count,
                note: note
            };
            $ts.post("@save", post, function (result) {
                if (result.code == 0) {
                    location.reload();
                }
                else {
                    nifty.showAlert(result.info);
                }
            });
        };
        return inventories;
    }(Bootstrap));
    pages.inventories = inventories;
})(pages || (pages = {}));
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
var pages;
(function (pages) {
    var password_reminder = /** @class */ (function (_super) {
        __extends(password_reminder, _super);
        function password_reminder() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        Object.defineProperty(password_reminder.prototype, "appName", {
            get: function () {
                return "password_reminder";
            },
            enumerable: true,
            configurable: true
        });
        password_reminder.prototype.init = function () {
        };
        password_reminder.prototype.send = function () {
            var email = Strings.Trim($ts.value("#email"));
            var tokens = email.split("@");
            if (tokens.length == 0) {
                return nifty.errorMsg("电子邮件地址不可以为空！");
            }
            else if (tokens.length == 1) {
                return nifty.errorMsg("您所输入的电子邮件地址格式不正确！");
            }
            else {
                // BOOTBOX - CUSTOM HTML CONTENTS
                // =================================================================
                // Require Bootbox
                // http://bootboxjs.com/
                // =================================================================
                bootbox.dialog({
                    title: "密码重置",
                    message: "\n<div class=\"media\">\n    <div class=\"media-left\">\n        <img class=\"media-object img-lg img-circle\" src=\"/assets/img/email-marketing-subject-line-icons.jpg\" alt=\"Profile picture\">\n    </div>\n    <div class=\"media-body\">\n        <p class=\"text-semibold text-main\">\n            \u6211\u4EEC\u5DF2\u7ECF\u5411" + email + "\u53D1\u9001\u4E86\u4E00\u5C01\u5BC6\u7801\u91CD\u7F6E\u6240\u9700\u8981\u7684\u7535\u5B50\u90AE\u4EF6\uFF0C\u60A8\u9700\u8981\u767B\u5F55\u8BE5\u7535\u5B50\u90AE\u7BB1\uFF0C\u6309\u7167\u90AE\u4EF6\u4E2D\u7684\u63D0\u793A\u5B8C\u6210\u5BC6\u7801\u91CD\u7F6E\u64CD\u4F5C\u3002\n        </p>\n    </div>\n</div>",
                    buttons: {
                        confirm: {
                            label: "确定"
                        }
                    }
                });
            }
        };
        return password_reminder;
    }(Bootstrap));
    pages.password_reminder = password_reminder;
})(pages || (pages = {}));
var pages;
(function (pages) {
    var vendor = /** @class */ (function (_super) {
        __extends(vendor, _super);
        function vendor() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        Object.defineProperty(vendor.prototype, "appName", {
            get: function () {
                return "vendor";
            },
            enumerable: true,
            configurable: true
        });
        vendor.prototype.init = function () {
            this.load();
        };
        vendor.prototype.load = function () {
            var vm = this;
            $ts.get("@load?page=1", function (result) {
                if (result.code == 0) {
                    vm.show_vendorList(result.info);
                }
                else {
                    nifty.errorMsg(result.info);
                }
            });
        };
        vendor.prototype.show_vendorList = function (vendors) {
            var list = $ts("#content-list").clear();
            var vm = this;
            var _loop_1 = function (vendor_2) {
                var status_1 = void 0;
                if (vendor_2.status == "0") {
                    status_1 = $ts("<span>", { class: ["label", "label-table", "label-primary"] }).display("合作中");
                }
                else {
                    status_1 = $ts("<span>", { class: ["label", "label-table", "label-warning"] }).display("已终止");
                }
                status_1 = $ts("<a>", {
                    href: executeJavaScript,
                    onclick: function () {
                        vm.change_vendorStatus(vendor_2.id, vendor_2.name);
                    }
                }).display(status_1);
                list.appendElement($ts("<tr>")
                    .appendElement($ts("<td>").display(vendor_2.name))
                    .appendElement($ts("<td>").display(vendor_2.tel))
                    .appendElement($ts("<td>").display(vendor_2.url))
                    .appendElement($ts("<td>").display(vendor_2.address))
                    .appendElement($ts("<td>").display(vendor_2.add_time))
                    .appendElement($ts("<td>").display(vendor_2.realname))
                    .appendElement($ts("<td>").display(status_1))
                    .appendElement($ts("<td>").display(vendor_2.note)));
            };
            for (var _i = 0, vendors_1 = vendors; _i < vendors_1.length; _i++) {
                var vendor_2 = vendors_1[_i];
                _loop_1(vendor_2);
            }
        };
        vendor.prototype.change_vendorStatus = function (id, name) {
            var post = { id: id };
            bootbox.dialog({
                title: "更改合作状态",
                message: "\u5C06\u8981\u66F4\u6539\u4F9B\u5E94\u5546<code>" + name + "</code>\u7684\u5408\u4F5C\u72B6\u6001",
                buttons: {
                    cancel: {
                        label: "取消",
                        className: "btn-default",
                    },
                    confirm: {
                        label: "确认",
                        className: "btn-primary",
                        callback: function () {
                            $ts.post("@switch", post, function (result) {
                                if (result.code == 0) {
                                    location.reload();
                                }
                                else {
                                    nifty.errorMsg(result.info);
                                }
                            });
                        }
                    }
                }
            });
        };
        vendor.prototype.save = function () {
            var name = $ts.value("#name");
            if (Strings.Empty(name, true)) {
                return nifty.showAlert("请输入供应商名称！");
            }
            var post = {
                name: name,
                tel: $ts.value("#tel"),
                url: $ts.value("#url"),
                address: $ts.value("#address"),
                note: $ts.value("#note")
            };
            $ts.post("@save", post, function (result) {
                if (result.code == 0) {
                    location.reload();
                }
                else {
                    nifty.showAlert(result.info);
                }
            });
        };
        return vendor;
    }(Bootstrap));
    pages.vendor = vendor;
})(pages || (pages = {}));
//# sourceMappingURL=pakchoi-pos.js.map