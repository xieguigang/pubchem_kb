namespace pages {

    export class VIP_members extends Bootstrap {

        public get appName(): string {
            return "VIP_members";
        }

        private scanner: Scanner;
        private editMode: boolean = false;

        protected init(): void {
            this.loadList();
            this.scanner = new Scanner(card_id => this.inputScanner(card_id));
        }

        private inputScanner(card_id: string) {
            if (this.editMode) {
                nifty.showAlert("目前正在编辑会员信息，请完成编辑后再扫码新增会员信息");
            } else {
                $ts.value("#card_id", card_id)
            }
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
                    .appendElement($ts("<td>").display((<any>vip).admin))
                    .appendElement(vm.operatorButtons(vip));

                list.appendElement(tr);
            }
        }

        private operatorButtons(vip: models.VIP_members) {
            let vm = this;
            let view = $ts("<button>", {
                class: ["btn", "btn-default", "btn-rounded"], onclick: function () {
                    $goto("/VIP?card_id=" + vip.card_id);
                }
            }).display("查看消费记录");
            let edit = $ts("<button>", {
                class: ["btn", "btn-primary", "btn-rounded"], onclick: function () {
                    vm.editRow(vip);
                }
            }).display("编辑会员信息");
            let charge = $ts("<button>", {
                class: ["btn", "btn-primary", "btn-rounded"], onclick: function () {
                    vm.charge(vip);
                }
            }).display("会员充值");
            let _delete = $ts("<button>", {
                class: ["btn", "btn-danger", "btn-rounded"], onclick: function () {
                    vm.deleteVIP(vip.id);
                }
            }).display("删除");

            return $ts("<td>")
                .appendElement(view)
                .appendElement(edit)
                .appendElement(charge)
                .appendElement(_delete)
                ;
        }

        private charge(vip: models.VIP_members) {
            bootbox.prompt("请输入所需要充值的金额：", function (input) {
                if (!Strings.Empty(input, true)) {
                    // 确认
                    if (!Strings.isNumericPattern(input)) {
                        return nifty.errorMsg("请输入正确格式的金额数字！");
                    } else {
                        $ts.post(`@charge`, { id: vip.id, add: parseFloat(input) }, function (result) {
                            if (result.code == 0) {
                                location.reload();
                            } else {
                                nifty.errorMsg(<string>result.info);
                            }
                        });
                    }
                } else {
                    // 取消
                };
            });
        }

        private editRow(vip: models.VIP_members) {
            this.editMode = true;

            $ts.value("#name", vip.name);
            $ts.value("#card_id", vip.card_id);
            $ts.value("#phone", vip.phone);
            $ts.value("#address", vip.address);
            $ts.value("#gender", vip.gender);
            $ts.value("#note", vip.note);

            nifty.clearAlert();

            $ts("#save").display("保存会员信息");
            $('#add-modal').modal('show');
        }

        public addrow() {
            this.editMode = false;

            $ts.value("#name", "");
            $ts.value("#card_id", "");
            $ts.value("#phone", "");
            $ts.value("#address", "");
            $ts.value("#gender", "-1");
            $ts.value("#note", "");

            nifty.clearAlert();

            $ts("#save").display("新增会员信息");
            $('#add-modal').modal('show');
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