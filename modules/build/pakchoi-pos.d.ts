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
declare class bootbox {
    static prompt(message: string, handle: (input: string) => void): any;
    static dialog(config: {
        title: string;
        message: string;
        buttons: {
            cancel?: bootboxButton;
            confirm: bootboxButton;
        };
    }): any;
}
interface bootboxButton {
    label: string;
    className?: string;
    callback?: Delegate.Action;
}
declare namespace app {
    function start(): void;
    function print(): void;
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
    interface VIP_waterflow {
        admin: string;
        id: string;
        balance: number;
        waterflow_id: string;
        time: string;
        note: string;
        /**
         * 0 - 消费
         * 1 - 充值
         * -1 - 退款
         *
        */
        type: number;
    }
    interface tradeInformation {
        admin: string;
        vip: string;
        id: string;
        goods: string;
        time: string;
        money: number;
        buyer: number;
        operator: string;
        count: number;
        discount: number;
        note: string;
        transaction_id: string;
        vip_balance: number;
    }
}
declare namespace nifty {
    function errorMsg(msg: string, callback?: Delegate.Action): void;
    function showAlert(message: string): void;
    function clearAlert(): void;
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
    class VIP_member extends Bootstrap {
        readonly appName: string;
        private card_id;
        private vip_id;
        protected init(): void;
        private loadVIP;
        private loadWaterflow;
    }
}
declare namespace pages {
    class VIP_members extends Bootstrap {
        readonly appName: string;
        private scanner;
        private editMode;
        protected init(): void;
        private inputScanner;
        private loadList;
        private showVIPListTable;
        private operatorButtons;
        private charge;
        private editRow;
        addrow(): void;
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
    class waterflows extends Bootstrap {
        readonly appName: string;
        protected init(): void;
        private loadWaterflows;
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
        private card_prefix;
        private vip_info;
        protected init(): void;
        private loadItem;
        private showVIP;
        private refresh;
        private addGoodsItem;
        private total;
        /**
         * 点击账单结算按钮进行支付结算
         *
        */
        settlement(): void;
    }
}
