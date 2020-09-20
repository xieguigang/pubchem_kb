/// <reference path="linq.d.ts" />
declare namespace pages {
    class login extends Bootstrap {
        readonly appName: string;
        protected init(): void;
        login(): void;
    }
}
declare namespace app {
    function start(): void;
}
declare namespace nifty {
    function errorMsg(msg: string): void;
}
