namespace pages {

    export class billing extends Bootstrap {

        public get appName(): string {
            return "billing";
        }

        private goods: Dictionary<{ item: models.goods, count: number }>;
        private scanner: Scanner;
        private card_prefix: string = <any>$ts("@card_prefix");
        private vip_info: models.VIP_members;

        protected init(): void {
            let firstItem: string = localStorage.getItem(firstItemKey);
            let vm = this;

            if (Strings.Empty(firstItem, true)) {
                $goto("/POS");
            } else {
                localStorage.setItem(firstItemKey, null);
            }

            $ts("#vip_name").display("非会员");

            this.goods = new Dictionary({});
            this.loadItem(firstItem);
            this.scanner = new Scanner(item_id => vm.loadItem(item_id));
        }

        private loadItem(item_id: string) {
            let vm = this;

            if (item_id.startsWith(this.card_prefix)) {
                // 是vip卡
                vm.showVIP(item_id);
            } else if (this.goods.ContainsKey(item_id)) {
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

        private showVIP(card_id: string) {
            let vm = this;

            $ts.get(`@get_vip?card_id=${card_id}`, function (result) {
                if (result.code == 0) {
                    vm.vip_info = <models.VIP_members>result.info;
                    vm.refresh();

                    $ts("#vip_name").display(`${vm.vip_info.name} 卡号：${card_id} 充值余额：￥${vm.vip_info.balance}`);
                } else {
                    // 没有找到会员信息
                }
            });
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
            let item: string = "总金额";
            let pay: string = `￥ ${Strings.round(cost, 2).toString()}`;
            let width: string = "80%";

            if (!isNullOrUndefined(this.vip_info)) {
                item = item + "<br />" + "会员余额结算";
                width = "60%";

                if (this.vip_info.balance >= cost) {
                    pay = pay + "<br />" + "可以使用余额全额支付";
                } else {
                    pay = pay + "<br />" + `<span style="color: darkred; font-size: 0.95em;">余额不足，还需要支付</span> ￥${cost - this.vip_info.balance}`;
                }
            }

            tr.appendElement($ts("<td>", { class: "alignright", style: `width:${width};` }).display(item))
            tr.appendElement($ts("<td>", { class: "alignright" }).display(pay));

            return tr;
        }

        /**
         * 点击账单结算按钮进行支付结算
         * 
        */
        public settlement() {
            let vip_id = isNullOrUndefined(this.vip_info) ? -1 : this.vip_info.id;
            let data = {
                goods: {},
                discount: 1,
                vip: vip_id,
                transaction: $ts("@transaction")
            };
            let vm = this;

            for (let item of this.goods.Values.ToArray()) {
                data.goods[item.item.id] = item.count;
            }

            $ts("#settlement").display("结算中").classList.add("disabled");
            $ts.post('@trade', data, function (result) {
                if (result.code != 0) {
                    $ts("#settlement").display("系统错误").classList.remove("disabled");
                } else {
                    $ts("#settlement").display("交易成功！").classList.remove("disabled");
                    setTimeout(() => $goto("/POS"), 1000);
                }
            });
        }
    }
}