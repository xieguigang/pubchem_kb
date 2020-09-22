/// <reference path="../../build/linq.d.ts" />

namespace app {

    export function start() {
        Router.AddAppHandler(new pages.login());
        Router.AddAppHandler(new pages.password_reminder());
        Router.AddAppHandler(new pages.lockscreen());

        Router.AddAppHandler(new pages.inventories());
        Router.AddAppHandler(new pages.goods());
        Router.AddAppHandler(new pages.vendor());
        Router.AddAppHandler(new pages.VIP_members());

        Router.RunApp();
    }
}

$ts.mode = Modes.debug;
$ts(app.start);