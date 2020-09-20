/// <reference path="linq.d.ts" />
declare namespace app {
    function start(): void;
}
declare namespace models {
    interface vendor {
        id?: string;
        name: string;
        tel: string;
        url: string;
        address: string;
        note: string;
        operator: string;
        status: string;
        add_time: string;
    }
}
declare namespace nifty {
    function errorMsg(msg: string): void;
    function showAlert(message: string): void;
}
declare namespace pages {
    class goods extends Bootstrap {
        readonly appName: string;
        protected init(): void;
    }
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
declare namespace pages {
    class password_reminder extends Bootstrap {
        readonly appName: string;
        protected init(): void;
        send(): void;
    }
}
declare namespace pages {
    class vendor extends Bootstrap {
        readonly appName: string;
        protected init(): void;
        private load;
        private show_vendorList;
        private change_vendorStatus;
        save(): void;
    }
}
