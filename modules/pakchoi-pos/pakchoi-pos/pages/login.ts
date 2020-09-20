namespace pages {

    export class login extends Bootstrap {

        public get appName(): string {
            return "login";
        }

        protected init(): void {
            let user: string = localStorage.getItem("user");

            if (!Strings.Empty(user, true)) {
                $ts.value("#user", user);
                $input("#remember_user").checked = true;
            }
        }

        public login() {
            let user: string = $ts.value("#user");
            let passwd: string = md5($ts.value("#passwd"));
            let post = { user: user, passwd: passwd }

            if (Strings.Empty(user, true)) {
                nifty.errorMsg("<strong>对不起，</strong>输入的账号不可以为空！")
            } else if (Strings.Empty($ts.value("#passwd"))) {
                nifty.errorMsg("<strong>对不起，</strong>输入的密码不可以为空！")
            } else {
                $ts.post("@login", post, function (result) {
                    if (result.code == 0) {
                        localStorage.setItem("user", user);
                        $goto("/");
                    } else {
                        nifty.errorMsg(<string>result.info);
                    }
                });
            }
        }
    }
}