namespace pages {

    export class VIP_member extends Bootstrap {

        public get appName(): string {
            return "VIP_member";
        }

        private card_id: string = $ts.location("card_id")

        protected init(): void {
            if (Strings.Empty(this.card_id, true)) {
                nifty.errorMsg("对不起，会员卡号不可以为空！", function () {
                    $goto("/");
                });
            }
        }

    }
}