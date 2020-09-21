namespace pages {

    export class goods extends Bootstrap {

        public get appName(): string {
            return "goods";
        };

        protected init(): void {
            $ts.get("@vendors", function (result) {
                if (result.code != 0) {
                    nifty.errorMsg("对不起，加载供应商信息失败，请刷新页面重试。。。")
                } else {
                    let selects = $ts("#vendor");

                    for (let vendor of <models.vendor[]>result.info) {
                        selects.appendElement($ts("<option>", { value: vendor.id }).display(vendor.name));
                    }
                }
            });
        }

        public save() {
            let item_id: string = $ts.value("#item_id");
            let name: string = $ts.value("#name");
            let price: string = $ts.value("#price");

            if (Strings.Empty(item_id, true)) {
                return nifty.showAlert("商品编号不可以为空！");
            } else if (Strings.Empty(name, true)) {
                return nifty.showAlert("请填写商品名称。");
            } else if (Strings.Empty(price, true)) {
                return nifty.showAlert("请填写商品价格！");
            } else if (!Strings.isNumericPattern(price)) {
                return nifty.showAlert("商品价格的格式不正确！");
            }

            let data: models.goods = <models.goods>{
                item_id: item_id,
                name: name,
                vendor_id: $ts.select.getOption("#vendor"),
                price: parseFloat(price),
                note: $ts.value("#note"),
                gender: parseInt($ts.select.getOption("#gender"))
            }

            $ts.post("@save", data, function (result) {
                if (result.code == 0) {
                    location.reload();
                } else {
                    nifty.showAlert(<string>result.info);
                }
            })
        }
    }
}