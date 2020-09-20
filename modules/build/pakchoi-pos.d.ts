/// <reference path="linq.d.ts" />
declare namespace app {
    function start(): void;
}
declare namespace nifty {
    function errorMsg(msg: string): void;
}
declare namespace pages {
    class inventories extends Bootstrap {
        readonly appName: string;
        protected init(): void;
        /**
         * 商品入库
        */
        save(): void;
    }
}
declare namespace pages {
    class lockscreen extends Bootstrap {
        readonly appName: string;
        protected init(): void;
        unlock(): void;
    }
}
declare namespace pages {
    class login extends Bootstrap {
        readonly appName: string;
        protected init(): void;
        login(): void;
    }
}
