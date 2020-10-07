namespace pages {

    export class waterflows extends Bootstrap {

        public get appName(): string {
            return "waterflows";
        }

        protected init(): void {
            this.loadWaterflows();
        }

        private loadWaterflows(page: number = 1) {
            $ts.get(`@load?page=${page}`, function (result) {
                if (result.code != 0) {
                    nifty.errorMsg(<string>result.info);
                } else {
                    let list = $ts("#list").clear();
                    let tr: IHTMLElement;

                    for (let trade of <models.tradeInformation[]>result.info) {
                        tr = $ts("<tr>");

                        tr.appendElement($ts("<td>").display(trade.transaction_id));
                        tr.appendElement($ts("<td>").display(trade.time));
                        tr.appendElement($ts("<td>").display(<any>trade.money));
                        tr.appendElement($ts("<td>").display(<any>trade.count));
                        tr.appendElement($ts("<td>").display(trade.vip));
                        tr.appendElement($ts("<td>").display(trade.admin));
                        tr.appendElement($ts("<td>").display(trade.note));

                        list.appendElement(tr);
                    }
                }
            })
        }
    }
}