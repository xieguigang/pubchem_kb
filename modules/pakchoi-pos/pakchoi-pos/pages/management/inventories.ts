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
                return nifty.showAlert("商品编号不可以为空！");
            }
            if (Strings.Empty(batch_id, true)) {
                batch_id = "";
            }

            let count: string = $ts.value("#count");

            if (!Strings.isIntegerPattern(count)) {
                return nifty.showAlert("商品件数错误，商品件数应该是一个大于零的整数！");
            }

            let note: string = $ts.value("#note");
            let post = {
                item_id: item_id,
                batch_id: batch_id,
                count: count,
                note: note
            }

            $ts.post("@save", post, function (result) {
                if (result.code == 0) {
                    location.reload();
                } else {
                    nifty.showAlert(<string>result.info);
                }
            });
        }
    }
}