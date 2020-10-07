/// <reference path="../../build/linq.d.ts" />

namespace app {

    export function start() {
        Router.AddAppHandler(new pages.login());
        Router.AddAppHandler(new pages.password_reminder());
        Router.AddAppHandler(new pages.lockscreen());
        Router.AddAppHandler(new pages.home());

        Router.AddAppHandler(new pages.trade());
        Router.AddAppHandler(new pages.item());

        Router.AddAppHandler(new pages.inventories());
        Router.AddAppHandler(new pages.goods());
        Router.AddAppHandler(new pages.vendor());
        Router.AddAppHandler(new pages.VIP_members());
        Router.AddAppHandler(new pages.VIP_member());
        Router.AddAppHandler(new pages.waterflows());

        Router.AddAppHandler(new pages.billing());
        Router.AddAppHandler(new pages.POS());

        Router.RunApp();
    }

    export function print(id: string = "#print") {
        (<any>$(id)).print({
            //Use Global styles
            globalStyles: true,
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
            append: "================================================================================<br/>" + new Date().toString(),
            deferred: $.Deferred().done(function () {
                alert("打印成功！");
            })
        });
    }
}

$ts.mode = Modes.debug;
$ts(app.start);