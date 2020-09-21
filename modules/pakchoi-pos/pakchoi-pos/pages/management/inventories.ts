namespace pages {

    export class inventories extends Bootstrap {

        public get appName(): string {
            return "inventories";
        }

        protected init(): void {
            this.showInventories(1);
        }

        private showInventories(page: number) {
            $ts.get(`@load?page=${page}`, function (result) {
                if (result.code != 0) {
                    nifty.errorMsg(<string>result.info);
                } else {
                    let list = $ts("#list").clear();
                    let tr: IHTMLElement;

                    for (let record of <models.inventories[]>result.info) {
                        tr = $ts("<tr>");

                        tr.appendElement($ts("<td>").display((<any>record).name));
                        tr.appendElement($ts("<td>").display(record.batch_id));
                        tr.appendElement($ts("<td>").display(record.inbound_time));
                        tr.appendElement($ts("<td>").display(<any>record.count));
                        tr.appendElement($ts("<td>").display((<any>record).admin));

                        list.appendElement(tr);
                    }
                }
            });
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