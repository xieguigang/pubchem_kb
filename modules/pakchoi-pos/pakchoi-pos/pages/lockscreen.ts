namespace pages {

    export class lockscreen extends Bootstrap {

        public get appName(): string {
            return "lockscreen";
        }

        protected init(): void {

        }

        public unlock() {
            let passwd: string = $ts.value("#passwd");

            if (Strings.Empty(passwd)) {
                nifty.errorMsg("<strong>对不起，</strong>密码不可以为空！");
            } else {
                $ts.post("@unlock", { passwd: md5(passwd) }, function (result) {
                    if (result.code == 0) {
                        $goto("/");
                    } else {
                        nifty.errorMsg(<string>result.info);
                    }
                });
            }
        }
    }
}