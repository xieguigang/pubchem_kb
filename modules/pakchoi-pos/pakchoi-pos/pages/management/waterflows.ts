namespace pages {

    export class waterflows extends Bootstrap {

        public get appName(): string {
            return "waterflows";
        }

        protected init(): void {
            this.loadWaterflows();
        }

        private loadWaterflows(page: number = 1) {
            let vm = this;

            $ts.get(`@load?page=${page}`, function (result) {
                if (result.code != 0) {
                    nifty.errorMsg(<string>result.info);
                } else {
                    let list = $ts("#list").clear();
                    let tr: IHTMLElement;
                    let buttons: IHTMLElement;

                    for (let trade of <models.tradeInformation[]>result.info) {
                        tr = $ts("<tr>");
                        buttons = $ts("<button>", {
                            class: ["btn", "btn-primary", "btn-rounded"],
                            onclick: function () {
                                vm.view_details(trade.id);
                            }
                        }).display("查看交易明细");

                        tr.appendElement($ts("<td>").display(trade.transaction_id));
                        tr.appendElement($ts("<td>").display(trade.time));
                        tr.appendElement($ts("<td>").display(<any>trade.money));
                        tr.appendElement($ts("<td>").display(<any>trade.count));
                        tr.appendElement($ts("<td>").display(trade.vip));
                        tr.appendElement($ts("<td>").display(trade.admin));
                        tr.appendElement($ts("<td>").display(trade.note));
                        tr.appendElement($ts("<td>").display(buttons));

                        list.appendElement(tr);
                    }
                }
            })
        }

        private view_details(trade_id: string) {

        }
    }
}