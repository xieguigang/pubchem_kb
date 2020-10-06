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
var Scanner = /** @class */ (function () {
    /**
     * 注册扫码枪输入事件
    */
    function Scanner(scanInput) {
        this.scanInput = scanInput;
        this.lastTime = null;
        this.nextTime = null;
        this.code = '';
        this.keyboardInput = "";
        var vm = this;
        document.onkeydown = function (e) {
            vm.nextTime = new Date().getTime();
            vm.scanCode(e.keyCode || e.which || e.charCode, e);
        };
    }
    Scanner.prototype.triggerEvt = function () {
        this.scanInput(this.code || this.keyboardInput);
        this.code = "";
        this.keyboardInput = "";
    };
    Scanner.prototype.scanCode = function (keycode, e) {
        if (keycode === 13) {
            if (this.lastTime && (this.nextTime - this.lastTime < 30)) {
                // 扫码枪输入
                // do something
                this.triggerEvt();
            }
            else {
                // 键盘输入
                // do nothing
            }
            this.code = '';
            this.lastTime = null;
            e.preventDefault();
        }
        else {
            var c = String.fromCharCode(keycode);
            if (!this.lastTime) {
                this.code = c;
                this.keyboardInput = c;
            }
            else {
                if (this.nextTime - this.lastTime < 30) {
                    this.code += c;
                }
                else {
                    this.code = '';
                    this.keyboardInput += c;
                    // 上上下下左右左右BA进入测试模式
                    if (this.keyboardInput.toUpperCase() == "&&((%'%'BA") {
                        this.triggerEvt();
                    }
                }
            }
            this.lastTime = this.nextTime;
        }
    };
    return Scanner;
}());
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
        Router.AddAppHandler(new pages.VIP_members());
        Router.AddAppHandler(new pages.VIP_member());
        Router.AddAppHandler(new pages.billing());
        Router.AddAppHandler(new pages.POS());
        Router.RunApp();
    }
    app.start = start;
    function print() {
        $("#print").print({
            //Use Global styles
            globalStyles: false,
            //Add link with attrbute media=print
            mediaPrint: false,
            //Custom stylesheet
            stylesheet: "",
            //Print in a hidden iframe
            iframe: true,
            //Don't print this
            noPrintSelector: "",
            //Add this at top
            prepend: "",
            //Add this on bottom
            append: "===============<br/>" + new Date().toString(),
            deferred: $.Deferred().done(function () {
                alert("????");
            })
        });
    }
    app.print = print;
})(app || (app = {}));
$ts.mode = Modes.debug;
$ts(app.start);
var nifty;
(function (nifty) {
    function errorMsg(msg, callback) {
        if (callback === void 0) { callback = null; }
        $.niftyNoty({
            type: 'danger',
            message: msg,
            container: 'floating',
            timer: 5000,
            onHidden: callback
        });
    }
    nifty.errorMsg = errorMsg;
    function showAlert(message) {
        $ts("#alert").show();
        $ts("#message").show().display(message);
    }
    nifty.showAlert = showAlert;
    function clearAlert() {
        $ts("#alert").hide();
        $ts("#message").hide().clear();
    }
    nifty.clearAlert = clearAlert;
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
    var VIP_member = /** @class */ (function (_super) {
        __extends(VIP_member, _super);
        function VIP_member() {
            var _this = _super !== null && _super.apply(this, arguments) || this;
            _this.card_id = $ts.location("card_id");
            return _this;
        }
        Object.defineProperty(VIP_member.prototype, "appName", {
            get: function () {
                return "VIP_member";
            },
            enumerable: true,
            configurable: true
        });
        VIP_member.prototype.init = function () {
            if (Strings.Empty(this.card_id, true)) {
                return nifty.errorMsg("对不起，会员卡号不可以为空！", function () {
                    $goto("/");
                });
            }
            else {
                this.loadVIP();
            }
        };
        VIP_member.prototype.loadVIP = function () {
            var vm = this;
            $ts.get("@load?card_id=" + this.card_id, function (result) {
                if (result.code != 0) {
                    nifty.errorMsg(result.info, function () {
                        $goto("/");
                    });
                }
                else {
                    var vip = result.info;
                    $ts("#card_id").display(vip.card_id);
                    $ts("#name").display(vip.name);
                    $ts("#balance").display("\uFFE5" + Strings.round(vip.balance, 2));
                    $ts("#phone").display(vip.phone);
                    $ts("#address").display(vip.address);
                    $ts("#gender").display(vip.gender == "0" ? "女" : (vip.gender == "1" ? "男" : "未记录"));
                    $ts("#note").display(vip.note);
                    vm.vip_id = vip.id;
                    vm.loadWaterflow();
                }
            });
        };
        VIP_member.prototype.loadWaterflow = function (page) {
            if (page === void 0) { page = 1; }
            $ts.get("@waterflow?card_id=" + this.vip_id + "&page=" + page, function (result) {
                if (result.code == 0) {
                    var table = $ts("#list").clear();
                    var tr = void 0;
                    for (var _i = 0, _a = result.info; _i < _a.length; _i++) {
                        var waterflow = _a[_i];
                        tr = $ts("<tr>");
                        tr.appendElement($ts("<td>").display(waterflow.id));
                        tr.appendElement($ts("<td>").display(Math.abs(waterflow.money)));
                        tr.appendElement($ts("<td>").display(waterflow.waterflow_id == "-1" ? "n/a" : waterflow.waterflow_id));
                        tr.appendElement($ts("<td>").display(waterflow.time));
                        tr.appendElement($ts("<td>").display(waterflow.money > 0 ? "充值" : "消费"));
                        tr.appendElement($ts("<td>").display(waterflow.note));
                        table.appendElement(tr);
                    }
                }
                else {
                    nifty.errorMsg(result.info);
                }
            });
        };
        return VIP_member;
    }(Bootstrap));
    pages.VIP_member = VIP_member;
})(pages || (pages = {}));
var pages;
(function (pages) {
    var VIP_members = /** @class */ (function (_super) {
        __extends(VIP_members, _super);
        function VIP_members() {
            var _this = _super !== null && _super.apply(this, arguments) || this;
            _this.editMode = false;
            return _this;
        }
        Object.defineProperty(VIP_members.prototype, "appName", {
            get: function () {
                return "VIP_members";
            },
            enumerable: true,
            configurable: true
        });
        VIP_members.prototype.init = function () {
            var _this = this;
            this.loadList();
            this.scanner = new Scanner(function (card_id) { return _this.inputScanner(card_id); });
        };
        VIP_members.prototype.inputScanner = function (card_id) {
            if (this.editMode) {
                nifty.showAlert("目前正在编辑会员信息，请完成编辑后再扫码新增会员信息");
            }
            else {
                $ts.value("#card_id", card_id);
            }
        };
        VIP_members.prototype.loadList = function (page) {
            if (page === void 0) { page = 1; }
            var vm = this;
            $ts.get("@load?page=" + page, function (result) {
                if (result.code != 0) {
                    nifty.errorMsg(result.info);
                }
                else {
                    vm.showVIPListTable(result.info);
                }
            });
        };
        VIP_members.prototype.showVIPListTable = function (members) {
            var list = $ts("#list").clear();
            var tr;
            var str;
            var vm = this;
            for (var _i = 0, members_1 = members; _i < members_1.length; _i++) {
                var vip = members_1[_i];
                tr = $ts("<tr>");
                tr.appendElement($ts("<td>").display(vip.card_id));
                tr.appendElement($ts("<td>").display(vip.name));
                tr.appendElement($ts("<td>").display(vip.balance.toString()));
                if (vip.gender == "1") {
                    str = "男";
                }
                else if (vip.gender == "0") {
                    str = "女";
                }
                else {
                    str = "未记录";
                }
                tr
                    .appendElement($ts("<td>").display(str))
                    .appendElement($ts("<td>").display(vip.phone))
                    .appendElement($ts("<td>").display(vip.address))
                    .appendElement($ts("<td>").display(vip.join_time))
                    // .appendElement($ts("<td>").display(vip.note))
                    .appendElement($ts("<td>").display(vip.admin))
                    .appendElement(vm.operatorButtons(vip));
                list.appendElement(tr);
            }
        };
        VIP_members.prototype.operatorButtons = function (vip) {
            var vm = this;
            var view = $ts("<button>", {
                class: ["btn", "btn-default", "btn-rounded"], onclick: function () {
                    $goto("/VIP?card_id=" + vip.card_id);
                }
            }).display("查看消费记录");
            var edit = $ts("<button>", {
                class: ["btn", "btn-primary", "btn-rounded"], onclick: function () {
                    vm.editRow(vip);
                }
            }).display("编辑会员信息");
            var charge = $ts("<button>", {
                class: ["btn", "btn-primary", "btn-rounded"], onclick: function () {
                    vm.charge(vip);
                }
            }).display("会员充值");
            var _delete = $ts("<button>", {
                class: ["btn", "btn-danger", "btn-rounded"], onclick: function () {
                    vm.deleteVIP(vip.id);
                }
            }).display("删除");
            return $ts("<td>")
                .appendElement(view)
                .appendElement(edit)
                .appendElement(charge)
                .appendElement(_delete);
        };
        VIP_members.prototype.charge = function (vip) {
            bootbox.prompt("请输入所需要充值的金额：", function (input) {
                if (!Strings.Empty(input, true)) {
                    // 确认
                    if (!Strings.isNumericPattern(input)) {
                        return nifty.errorMsg("请输入正确格式的金额数字！");
                    }
                    else {
                        $ts.post("@charge", { id: vip.id, add: parseFloat(input) }, function (result) {
                            if (result.code == 0) {
                                location.reload();
                            }
                            else {
                                nifty.errorMsg(result.info);
                            }
                        });
                    }
                }
                else {
                    // 取消
                }
                ;
            });
        };
        VIP_members.prototype.editRow = function (vip) {
            this.editMode = true;
            $ts.value("#name", vip.name);
            $ts.value("#card_id", vip.card_id);
            $ts.value("#phone", vip.phone);
            $ts.value("#address", vip.address);
            $ts.value("#gender", vip.gender);
            $ts.value("#note", vip.note);
            nifty.clearAlert();
            $ts("#save").display("保存会员信息");
            $('#add-modal').modal('show');
        };
        VIP_members.prototype.addrow = function () {
            this.editMode = false;
            $ts.value("#name", "");
            $ts.value("#card_id", "");
            $ts.value("#phone", "");
            $ts.value("#address", "");
            $ts.value("#gender", "-1");
            $ts.value("#note", "");
            nifty.clearAlert();
            $ts("#save").display("新增会员信息");
            $('#add-modal').modal('show');
        };
        VIP_members.prototype.deleteVIP = function (id) {
            bootbox.prompt("【危险操作】请输入会员名来确认删除", function (input) {
                if (!Strings.Empty(input, true)) {
                    // 确认删除
                    $ts.post("@delete", { id: id, name: input }, function (result) {
                        if (result.code == 0) {
                            location.reload();
                        }
                        else {
                            nifty.errorMsg(result.info);
                        }
                    });
                }
                else {
                    // 取消
                }
                ;
            });
        };
        VIP_members.prototype.save = function () {
            var name = $ts.value("#name");
            var card_id = $ts.value("#card_id");
            if (Strings.Empty(name, true)) {
                return nifty.showAlert("对不起，会员姓名不可以为空！");
            }
            else if (Strings.Empty(card_id, true)) {
                return nifty.showAlert("对不起，请刷一次会员卡获取卡号信息！");
            }
            var phone = $ts.value("#phone");
            var address = $ts.value("#address");
            var gender = $ts.select.getOption("#gender");
            var note = $ts.value("#note");
            var data = {
                name: name,
                phone: phone,
                address: address,
                gender: gender,
                note: note,
                card_id: card_id
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
        return VIP_members;
    }(Bootstrap));
    pages.VIP_members = VIP_members;
})(pages || (pages = {}));
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
            this.load();
            this.scanner = new Scanner(function (item_id) {
                $ts.value("#item_id", item_id);
            });
        };
        goods.prototype.load = function (page) {
            if (page === void 0) { page = 1; }
            var vm = this;
            $ts.get("@load?page=" + page, function (result) {
                if (result.code != 0) {
                    nifty.errorMsg("对不起，商品信息加载失败，请刷新页面重试。。。");
                }
                else {
                    vm.showList(result.info);
                }
            });
        };
        goods.prototype.showList = function (result) {
            var list = $ts("#list").clear();
            var tr;
            var str;
            for (var _i = 0, result_1 = result; _i < result_1.length; _i++) {
                var goods_1 = result_1[_i];
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
                tr.appendElement($ts("<td>").display(str));
                tr.appendElement($ts("<td>").display(goods_1.item_id));
                str = goods_1.vendor;
                if (Strings.Empty(str, true)) {
                    str = "自产商品（无供货商）";
                }
                tr.appendElement($ts("<td>").display(str));
                tr.appendElement($ts("<td>").display(goods_1.add_time));
                tr.appendElement($ts("<td>").display("0"));
                tr.appendElement($ts("<td>").display(goods_1.price));
                tr.appendElement($ts("<td>").display(goods_1.note));
                tr.appendElement($ts("<td>").display(goods_1.realname));
                list.appendElement(tr);
            }
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
            this.showInventories(1);
            this.scanner = new Scanner(function (item_id) { return $ts.value("#item_id", item_id); });
        };
        inventories.prototype.showInventories = function (page) {
            $ts.get("@load?page=" + page, function (result) {
                if (result.code != 0) {
                    nifty.errorMsg(result.info);
                }
                else {
                    var list = $ts("#list").clear();
                    var tr = void 0;
                    for (var _i = 0, _a = result.info; _i < _a.length; _i++) {
                        var record = _a[_i];
                        tr = $ts("<tr>");
                        tr.appendElement($ts("<td>").display(record.name));
                        tr.appendElement($ts("<td>").display(record.batch_id));
                        tr.appendElement($ts("<td>").display(record.inbound_time));
                        tr.appendElement($ts("<td>").display(record.count));
                        tr.appendElement($ts("<td>").display(record.admin));
                        list.appendElement(tr);
                    }
                }
            });
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
                    status_1 = $ts("<span>", { class: ["label", "label-table", "label-warning"] }).display("已终止合作");
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
var pages;
(function (pages) {
    pages.firstItemKey = "first-item";
    var POS = /** @class */ (function (_super) {
        __extends(POS, _super);
        function POS() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        Object.defineProperty(POS.prototype, "appName", {
            get: function () {
                return "POS";
            },
            enumerable: true,
            configurable: true
        });
        POS.prototype.init = function () {
            var _this = this;
            var vm = this;
            vm.scanner = new Scanner(POS.startBilling);
            // idle events
            window.setInterval(function () { return _this.clock(); }, 1000);
        };
        POS.startBilling = function (item_id) {
            localStorage.setItem(pages.firstItemKey, item_id);
            $goto("/billing");
        };
        POS.prototype.clock = function () {
            var time = new Date();
            var hh = Strings.PadLeft(time.getHours().toString(), 2, "0");
            var mm = Strings.PadLeft(time.getMinutes().toString(), 2, "0");
            var ss = Strings.PadLeft(time.getSeconds().toString(), 2, "0");
            $ts("#clock").display(hh + ":" + mm + ":" + ss);
            $ts("#date").display(time.toDateString());
        };
        return POS;
    }(Bootstrap));
    pages.POS = POS;
})(pages || (pages = {}));
var pages;
(function (pages) {
    var billing = /** @class */ (function (_super) {
        __extends(billing, _super);
        function billing() {
            var _this = _super !== null && _super.apply(this, arguments) || this;
            _this.card_prefix = $ts("@card_prefix");
            return _this;
        }
        Object.defineProperty(billing.prototype, "appName", {
            get: function () {
                return "billing";
            },
            enumerable: true,
            configurable: true
        });
        billing.prototype.init = function () {
            var firstItem = localStorage.getItem(pages.firstItemKey);
            var vm = this;
            if (Strings.Empty(firstItem, true)) {
                $goto("/POS");
            }
            else {
                localStorage.setItem(pages.firstItemKey, null);
            }
            $ts("#vip_name").display("非会员");
            this.goods = new Dictionary({});
            this.loadItem(firstItem);
            this.scanner = new Scanner(function (item_id) { return vm.loadItem(item_id); });
        };
        billing.prototype.loadItem = function (item_id) {
            var vm = this;
            if (item_id.startsWith(this.card_prefix)) {
                // 是vip卡
                vm.showVIP(item_id);
            }
            else if (this.goods.ContainsKey(item_id)) {
                this.goods.Item(item_id).count += 1;
                this.refresh();
            }
            else {
                $ts.get("@get?item_id=" + item_id, function (result) {
                    if (result.code == 0) {
                        vm.goods.Add(item_id, { item: result.info, count: 1 });
                        vm.refresh();
                    }
                    else {
                        nifty.errorMsg(result.info);
                    }
                });
            }
        };
        billing.prototype.showVIP = function (card_id) {
            var vm = this;
            $ts.get("@get_vip?card_id=" + card_id, function (result) {
                if (result.code == 0) {
                    vm.vip_info = result.info;
                    vm.refresh();
                    $ts("#vip_name").display(vm.vip_info.name + " \u5361\u53F7\uFF1A" + card_id + " \u5145\u503C\u4F59\u989D\uFF1A\uFFE5" + vm.vip_info.balance);
                }
                else {
                    // 没有找到会员信息
                }
            });
        };
        billing.prototype.refresh = function () {
            var table = $ts("#invoice-table").clear();
            var total = 0;
            for (var _i = 0, _a = this.goods.Values.ToArray(); _i < _a.length; _i++) {
                var item = _a[_i];
                table.appendElement(this.addGoodsItem(item.item, item.count));
                total += item.item.price * item.count;
            }
            table.appendElement(this.total(total));
        };
        billing.prototype.addGoodsItem = function (item, count) {
            var tr = $ts("<tr>");
            var displayText;
            if (count == 1) {
                displayText = item.name;
            }
            else {
                displayText = item.name + " &nbsp; x" + count;
            }
            tr.appendElement($ts("<td>").display(displayText));
            tr.appendElement($ts("<td>", { class: "alignright" }).display("\uFFE5 " + item.price * count));
            return tr;
        };
        billing.prototype.total = function (cost) {
            var tr = $ts("<tr>", { class: "total" });
            var item = "总金额";
            var pay = "\uFFE5 " + Strings.round(cost, 2).toString();
            var width = "80%";
            if (!isNullOrUndefined(this.vip_info)) {
                item = item + "<br />" + "会员余额结算";
                width = "60%";
                if (this.vip_info.balance >= cost) {
                    pay = pay + "<br />" + "可以使用余额全额支付";
                }
                else {
                    pay = pay + "<br />" + ("<span style=\"color: darkred; font-size: 0.95em;\">\u4F59\u989D\u4E0D\u8DB3\uFF0C\u8FD8\u9700\u8981\u652F\u4ED8</span> \uFFE5" + (cost - this.vip_info.balance));
                }
            }
            tr.appendElement($ts("<td>", { class: "alignright", style: "width:" + width + ";" }).display(item));
            tr.appendElement($ts("<td>", { class: "alignright" }).display(pay));
            return tr;
        };
        /**
         * 点击账单结算按钮进行支付结算
         *
        */
        billing.prototype.settlement = function () {
            var vip_id = isNullOrUndefined(this.vip_info) ? -1 : this.vip_info.id;
            var data = {
                goods: {},
                discount: 1,
                vip: vip_id,
                transaction: $ts("@transaction")
            };
            var vm = this;
            for (var _i = 0, _a = this.goods.Values.ToArray(); _i < _a.length; _i++) {
                var item = _a[_i];
                data.goods[item.item.id] = item.count;
            }
            $ts("#settlement").display("结算中").classList.add("disabled");
            $ts.post('@trade', data, function (result) {
                if (result.code != 0) {
                    $ts("#settlement").display("系统错误").classList.remove("disabled");
                }
                else {
                    $ts("#settlement").display("交易成功！").classList.remove("disabled");
                    setTimeout(function () { return $goto("/POS"); }, 1000);
                }
            });
        };
        return billing;
    }(Bootstrap));
    pages.billing = billing;
})(pages || (pages = {}));
//# sourceMappingURL=pakchoi-pos.js.map