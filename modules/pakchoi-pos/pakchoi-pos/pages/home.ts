namespace pages {

    export class home extends Bootstrap {

        public get appName(): string {
            return "home";
        }

        protected init(): void {
            this.showTransactions();
        }

        private showTransactions(page: number = 1) {
            let vm = this;

            $ts.get(`@load_trades?page=${page}`, function (result) {
                if (result.code != 0) {
                    nifty.errorMsg(<string>result.info);
                } else {
                    let table = $ts("#trades").clear();
                    let tr: IHTMLElement;
                    let link: IHTMLElement;
                    let type: IHTMLElement;

                    for (let trade of <models.tradeInformation[]>result.info) {
                        tr = $ts("<tr>");
                        link = $ts("<a>", {
                            class: "btn-link",
                            href: ""
                        }).display(trade.transaction_id);

                        if (trade.money > 0) {
                            type = $ts("<div>", { class: "label label-table label-info" }).display("消费");
                        } else {
                            type = $ts("<div>", { class: "label label-table label-danger" }).display("退款");
                        }

                        tr.appendElement($ts("<td>").display(link));
                        tr.appendElement($ts("<td>").display(trade.vip));
                        tr.appendElement($ts("<td>").display(trade.time));
                        tr.appendElement($ts("<td>").display(`￥${trade.money}`))
                        tr.appendElement($ts("<td>", { class: "text-center" }).display(type));
                        tr.appendElement($ts("<td>", { class: "text-center" }).display(trade.note));

                        table.appendElement(tr);
                    }
                }
            });
        }
    }
}