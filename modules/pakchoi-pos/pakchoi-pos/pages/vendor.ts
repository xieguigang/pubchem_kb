namespace pages {

    export class vendor extends Bootstrap {

        public get appName(): string {
            return "vendor";
        }

        protected init(): void {

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