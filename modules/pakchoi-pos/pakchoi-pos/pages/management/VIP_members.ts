namespace pages {

    export class VIP_members extends Bootstrap {

        public get appName(): string {
            return "VIP_members";
        }

        private scanner: Scanner;

        protected init(): void {
            this.loadList();
            this.scanner = new Scanner(card_id => $ts.value("#card_id", card_id));
        }

        private loadList(page: number = 1) {
            let vm = this;

            $ts.get(`@load?page=${page}`, function (result) {
                if (result.code != 0) {
                    nifty.errorMsg(<string>result.info);
                } else {
                    vm.showVIPListTable(<models.VIP_members[]>result.info);
                }
            });
        }

        private showVIPListTable(members: models.VIP_members[]) {
            let list = $ts("#list").clear();
            let tr: IHTMLElement;
            let str: string;
            let vm = this;

            for (let vip of members) {
                tr = $ts("<tr>");
                tr.appendElement($ts("<td>").display(vip.card_id));
                tr.appendElement($ts("<td>").display(vip.name));
                tr.appendElement($ts("<td>").display(vip.balance.toString()));

                if (vip.gender == "1") {
                    str = "男";
                } else if (vip.gender == "0") {
                    str = "女";
                } else {
                    str = "未记录";
                }

                tr
                    .appendElement($ts("<td>").display(str))
                    .appendElement($ts("<td>").display(vip.phone))
                    .appendElement($ts("<td>").display(vip.address))
                    .appendElement($ts("<td>").display(vip.join_time))
                    // .appendElement($ts("<td>").display(vip.note))
                    .appendElement($ts("<td>").display((<any>vip).admin));

                let opBtns = $ts("<td>");

                opBtns.appendElement($ts("<button>", {
                    class: ["btn", "btn-default", "btn-rounded"],
                    onclick: function () {
                        $goto("/VIP?card_id=" + vip.card_id);
                    }
                }).display("查看消费记录"));
                opBtns.appendElement($ts("<button>", {
                    class: ["btn", "btn-danger", "btn-rounded"],
                    onclick: function () {
                        vm.deleteVIP(vip.id);
                    }
                }).display("删除"))

                tr.appendElement(opBtns);

                list.appendElement(tr);
            }
        }

        private deleteVIP(id: string) {
            bootbox.prompt("【危险操作】请输入会员名来确认删除", function (input) {
                if (!Strings.Empty(input, true)) {
                    // 确认删除
                    $ts.post("@delete", { id: id, name: input }, function (result) {
                        if (result.code == 0) {
                            location.reload();
                        } else {
                            nifty.errorMsg(<string>result.info);
                        }
                    })
                } else {
                    // 取消
                };
            });
        }

        public save() {
            let name: string = $ts.value("#name");
            let card_id: string = $ts.value("#card_id");

            if (Strings.Empty(name, true)) {
                return nifty.showAlert("对不起，会员姓名不可以为空！");
            } else if (Strings.Empty(card_id, true)) {
                return nifty.showAlert("对不起，请刷一次会员卡获取卡号信息！");
            }

            let phone: string = $ts.value("#phone");
            let address: string = $ts.value("#address");
            let gender: string = $ts.select.getOption("#gender");
            let note: string = $ts.value("#note");

            let data: models.VIP_members = <models.VIP_members>{
                name: name,
                phone: phone,
                address: address,
                gender: gender,
                note: note,
                card_id: card_id
            };

            $ts.post("@save", data, function (result) {
                if (result.code == 0) {
                    location.reload();
                } else {
                    nifty.showAlert(<string>result.info);
                }
            });
        }
    }
}