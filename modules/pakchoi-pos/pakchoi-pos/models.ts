namespace models {

    export interface vendor {
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

    export interface goods {
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

    export interface inventories {
        id?: string;
        batch_id: string;
        inbound_time?: string;
        item_id: number;
        count: number;
        note: string;
        operator?: string;
    }

    export interface VIP_members {
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

    export interface VIP_waterflow {
        admin: string;
        id: string;
        balance: number;
        waterflow_id: string;
        transaction_id: string;
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

    export interface tradeInformation {
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
