namespace pages {

    export class billing extends Bootstrap {

        public get appName(): string {
            return "billing";
        }

        private goods: Dictionary<{ item: models.goods, count: number }>;
        private scanner: Scanner;

        protected init(): void {
            let firstItem: string = localStorage.getItem(firstItemKey);
            let vm = this;

            if (Strings.Empty(firstItem, true)) {
                $goto("/POS");
            } else {
                localStorage.setItem(firstItemKey, null);
            }

            this.goods = new Dictionary({});

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
            this.scanner = new Scanner(item_id => vm.loadItem(item_id));
        }

        private loadItem(item_id: string) {
            let vm = this;

            if (this.goods.ContainsKey(item_id)) {
                this.goods.Item(item_id).count += 1;
                this.refresh();
            } else {
                $ts.get(`@get?item_id=${item_id}`, function (result) {
                    if (result.code == 0) {
                        vm.goods.Add(item_id, { item: <models.goods>result.info, count: 1 });
                        vm.refresh();
                    } else {
                        nifty.errorMsg(<string>result.info);
                    }
                });
            }
        }

        private refresh() {
            let table = $ts("#invoice-table").clear();
            let total: number = 0;

            for (let item of this.goods.Values.ToArray()) {
                table.appendElement(this.addGoodsItem(item.item, item.count));
                total += item.item.price * item.count;
            }

            table.appendElement(this.total(total));
        }

        private addGoodsItem(item: models.goods, count: number) {
            let tr = $ts("<tr>");
            let displayText: string;

            if (count == 1) {
                displayText = item.name;
            } else {
                displayText = `${item.name} &nbsp; x${count}`;
            }

            tr.appendElement($ts("<td>").display(displayText));
            tr.appendElement($ts("<td>", { class: "alignright" }).display(`￥ ${item.price * count}`));

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
            let data = {
                goods: {}
            };

            for (let item of this.goods.Values.ToArray()) {
                data.goods[item.item.item_id] = item.count;
            }

            $ts.post('@trade', data, function (result) {

            });
        }
    }
}