namespace pages {

    export class inventories extends Bootstrap {

        public get appName(): string {
            return "inventories";
        }

        protected init(): void {

        }

        /**
         * 商品入库
        */
        public save() {
            let item_id: string = $ts.value("#item_id");
            let batch_id: string = $ts.value("#batch_id");

            if (Strings.Empty(item_id, true)) {
                return nifty.errorMsg("对不起，商品编号不可以为空！");
            }

        }
    }
}