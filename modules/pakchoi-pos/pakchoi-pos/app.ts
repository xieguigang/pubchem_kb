/// <reference path="../../build/linq.d.ts" />

/// <reference path="pages/login.ts" />

namespace app {

    export function start() {
        Router.AddAppHandler(new pages.login());

        Router.RunApp();
    }
}

$ts.mode = Modes.debug;
$ts(app.start);