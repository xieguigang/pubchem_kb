namespace pages {

    export class billing extends Bootstrap {

        public get appName(): string {
            return "billing";
        }

        private goods: models.goods[] = [];

        protected init(): void {
            let firstItem: string = localStorage.getItem(firstItemKey);
            let vm = this;

            if (Strings.Empty(firstItem, true)) {
                $goto("/POS");
            } else {
                localStorage.setItem(firstItemKey, null);
            }

            // STATE BUTTON
            // =================================================================
            // Require Bootstrap Button
            // -----------------------------------------------------------------
            // http://getbootstrap.com/javascript/#buttons
            // =================================================================
            $('#settlement').on('click', function () {
                // 这个状态变化必须要通过jQuery来进行触发
                // 否则会出现丢失文档碎片的错误？
                $(this).button('loading');
                // business logic...
                vm.settlement();
            });

            this.loadItem(firstItem);
        }

        private loadItem(item_id: string) {
            let vm = this;

            $ts.get(`@get?item_id=${item_id}`, function (result) {
                if (result.code == 0) {
                    vm.goods.push(<models.goods>result.info);
                    vm.refresh();
                } else {
                    nifty.errorMsg(<string>result.info);
                }
            });
        }

        private refresh() {
            let table = $ts("#invoice-table").clear();
            let total: number = 0;

            for (let item of this.goods) {
                table.appendElement(this.addGoodsItem(item));
                total += item.price;
            }

            table.appendElement(this.total(total));
        }

        private addGoodsItem(item: models.goods) {
            let tr = $ts("<tr>");

            tr.appendElement($ts("<td>").display(item.name));
            tr.appendElement($ts("<td>", { class: "alignright" }).display(`￥ ${item.price}`));

            return tr;
        }

        private total(cost: number): HTMLElement {
            let tr = $ts("<tr>", { class: "total" });

            tr.appendElement($ts("<td>", { class: "alignright", style: "width:80%;" }).display("总金额"))
            tr.appendElement($ts("<td>", { class: "alignright" }).display(`￥ ${Strings.round(cost, 2).toString()}`))

            return tr;
        }

        /**
         * 点击账单结算按钮进行支付结算
        */
        private settlement() {

        }
    }
}