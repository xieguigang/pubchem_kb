namespace pages {

    export class vendor extends Bootstrap {

        public get appName(): string {
            return "vendor";
        }

        protected init(): void {
            this.load();
        }

        private load() {
            $ts.get("@load?page=1", function (result: IMsg<models.vendor[]>) {
                if (result.code == 0) {
                    let list = $ts("#content-list").clear();

                    for (let vendor of <models.vendor[]>result.info) {
                        let status: HTMLElement;

                        if (vendor.status == "0") {
                            status = $ts("<span>", { class: ["label", "label-table", "label-primary"] }).display("合作中");
                        } else {
                            status = $ts("<span>", { class: ["label", "label-table", "label-warning"] }).display("已终止");
                        }

                        list.appendElement(
                            $ts("<tr>")
                                .appendElement($ts("<td>").display(vendor.name))
                                .appendElement($ts("<td>").display(vendor.tel))
                                .appendElement($ts("<td>").display(vendor.url))
                                .appendElement($ts("<td>").display(vendor.address))
                                .appendElement($ts("<td>").display(vendor.add_time))
                                .appendElement($ts("<td>").display((<any>vendor).realname))
                                .appendElement($ts("<td>").display(status))
                                .appendElement($ts("<td>").display(vendor.note))
                        );
                    }
                } else {
                    nifty.errorMsg(<string>result.info);
                }
            });
        }

        public save() {
            let name: string = $ts.value("#name");

            if (Strings.Empty(name, true)) {
                return nifty.showAlert("请输入供应商名称！");
            }

            let post = <models.vendor>{
                name: name,
                tel: $ts.value("#tel"),
                url: $ts.value("#url"),
                address: $ts.value("#address"),
                note: $ts.value("#note")
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