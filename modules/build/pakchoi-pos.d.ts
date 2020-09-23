/// <reference path="linq.d.ts" />
declare class Scanner {
    private scanInput;
    private lastTime;
    private nextTime;
    private code;
    /**
     * 注册扫码枪输入事件
    */
    constructor(scanInput: (codeInput: string) => void);
    private scanCode;
}
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
    interface goods {
        id?: string;
        name: string;
        add_time?: string;
        price: number;
        gender: number;
        display?: string;
        /**
         * the vendor item id
        */
        item_id: string;
        vendor_id: string;
        note: string;
        operator?: string;
    }
    interface inventories {
        id?: string;
        batch_id: string;
        inbound_time?: string;
        item_id: number;
        count: number;
        note: string;
        operator?: string;
    }
    interface VIP_members {
        id?: string;
        name: string;
        gender: string;
        phone: string;
        address: string;
        join_time?: string;
        operator?: string;
        note: string;
    }
}
declare namespace nifty {
    function errorMsg(msg: string): void;
    function showAlert(message: string): void;
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
    class VIP_members extends Bootstrap {
        readonly appName: string;
        protected init(): void;
        private loadList;
        save(): void;
    }
}
declare namespace pages {
    class goods extends Bootstrap {
        readonly appName: string;
        protected init(): void;
        save(): void;
    }
}
declare namespace pages {
    class inventories extends Bootstrap {
        readonly appName: string;
        protected init(): void;
        private showInventories;
        /**
         * 商品入库
        */
        save(): void;
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
declare namespace pages {
    class POS extends Bootstrap {
        readonly appName: string;
        protected init(): void;
    }
}
declare namespace pages {
    class billing extends Bootstrap {
        readonly appName: string;
        protected init(): void;
    }
}
