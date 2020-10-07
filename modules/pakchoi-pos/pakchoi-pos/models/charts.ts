namespace models.charts {

    export interface inventories_sparkline extends sparkline {
        sales: number;
        total: number;
    }

    export interface sparkline {
        sparkline: number[];
    }

    export interface sales_sparkline extends sparkline {
        day: number;
        total: number;
    }

    export interface inventories_sparkbar extends sparkline {
        today: number;
        total: number;
    }
}