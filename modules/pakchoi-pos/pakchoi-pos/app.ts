/// <reference path="../../build/linq.d.ts" />

namespace app {

    export function start() {
        Router.AddAppHandler(new pages.login());
        Router.AddAppHandler(new pages.lockscreen());
        Router.AddAppHandler(new pages.inventories());

        Router.RunApp();
    }
}

$ts.mode = Modes.debug;
$ts(app.start);