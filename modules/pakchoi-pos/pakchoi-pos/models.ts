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
        id: string;
        name: string;
        add_time: string;
        price: number;
        gender: number;
        display: string;
        /**
         * the vendor item id
        */
        item_id: string;
        vendor_id: string;
        note: string;
    }
}
