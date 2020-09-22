namespace pages {

    export class VIP_members extends Bootstrap {

        public get appName(): string {
            return "VIP_members";
        }

        protected init(): void {

        }

        public save() {
            let name: string = $ts.value("#name");

            if (Strings.Empty(name, true)) {
                return nifty.showAlert("对不起，会员姓名不可以为空！");
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
                note: note
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