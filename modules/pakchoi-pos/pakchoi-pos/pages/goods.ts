namespace pages {

    export class goods extends Bootstrap {

        public get appName(): string {
            return "goods";
        };

        protected init(): void {
            $ts.get("@vendors", function (result) {
                if (result.code == 0) {
                    nifty.errorMsg("对不起，加载供应商信息失败，请刷新页面重试。。。")
                } else {
                    let selects = $ts("#vendor");

                    for (let vendor of <models.vendor[]>result.info) {
                        selects.appendElement($ts("<option>", { value: vendor.id }).display(vendor.name));
                    }
                }
            });
        }


    }
}