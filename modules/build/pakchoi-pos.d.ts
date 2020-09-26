/// <reference path="linq.d.ts" />
declare class Scanner {
    private scanInput;
    private lastTime;
    private nextTime;
    private code;
    private keyboardInput;
    /**
     * 注册扫码枪输入事件
    */
    constructor(scanInput: (codeInput: string) => void);
    private triggerEvt;
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
        card_id: string;
        name: string;
        balance?: number;
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
        private scanner;
        protected init(): void;
        private loadList;
        private showVIPListTable;
        private deleteVIP;
        save(): void;
    }
}
declare namespace pages {
    class goods extends Bootstrap {
        private scanner;
        readonly appName: string;
        protected init(): void;
        private load;
        private showList;
        save(): void;
    }
}
declare namespace pages {
    class inventories extends Bootstrap {
        readonly appName: string;
        private scanner;
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
    const firstItemKey: string;
    class POS extends Bootstrap {
        private scanner;
        readonly appName: string;
        protected init(): void;
        private static startBilling;
        private clock;
    }
}
declare namespace pages {
    class billing extends Bootstrap {
        readonly appName: string;
        private goods;
        private scanner;
        protected init(): void;
        private loadItem;
        private refresh;
        private addGoodsItem;
        private total;
        /**
         * 点击账单结算按钮进行支付结算
        */
        private settlement;
    }
}
