namespace pages {

    export class VIP_member extends Bootstrap {

        public get appName(): string {
            return "VIP_member";
        }

        private card_id: string = $ts.location("card_id")
        private vip_id: string;

        protected init(): void {
            if (Strings.Empty(this.card_id, true)) {
                return nifty.errorMsg("对不起，会员卡号不可以为空！", function () {
                    $goto("/");
                });
            } else {
                this.loadVIP();
            }
        }

        private loadVIP() {
            let vm = this;

            $ts.get(`@load?card_id=${this.card_id}`, function (result: IMsg<models.VIP_members>) {
                if (result.code != 0) {
                    nifty.errorMsg(<string>result.info, function () {
                        $goto("/");
                    })
                } else {
                    let vip = <models.VIP_members>result.info;

                    $ts("#card_id").display(vip.card_id);
                    $ts("#name").display(vip.name);
                    $ts("#balance").display(`￥${Strings.round(vip.balance, 2)}`);
                    $ts("#phone").display(vip.phone);
                    $ts("#address").display(vip.address);
                    $ts("#gender").display(vip.gender == "0" ? "女" : (vip.gender == "1" ? "男" : "未记录"));
                    $ts("#note").display(vip.note);

                    vm.vip_id = vip.id;
                    vm.loadWaterflow();
                }
            });
        }

        private loadWaterflow(page: number = 1) {
            $ts.get(`@waterflow?card_id=${this.vip_id}&page=${page}`, function (result) {
                if (result.code == 0) {
                    let table = $ts("#list").clear();
                    let tr: IHTMLElement;

                    for (let waterflow of <models.VIP_waterflow[]>result.info) {
                        tr = $ts("<tr>");
                        tr.appendElement($ts("<td>").display(waterflow.id));
                        tr.appendElement($ts("<td>").display(<any>Math.abs(waterflow.balance)));
                        tr.appendElement($ts("<td>").display(waterflow.waterflow_id == "-1" ? "n/a" : waterflow.transaction_id));
                        tr.appendElement($ts("<td>").display(waterflow.time));
                        tr.appendElement($ts("<td>").display(waterflow.type == 1 ? "充值" : (waterflow.type == 0 ? "消费" : "退款")));
                        tr.appendElement($ts("<td>").display(waterflow.note));

                        table.appendElement(tr);
                    }
                } else {
                    nifty.errorMsg(<string>result.info);
                }
            })
        }
    }
}