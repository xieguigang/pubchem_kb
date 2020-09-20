namespace pages {

    export class login extends Bootstrap {

        public get appName(): string {
            return "login";
        }

        protected init(): void {

        }

        public login() {
            let user: string = $ts.value("#user");
            let passwd: string = md5($ts.value("#passwd"));
            let post = { user: user, passwd: passwd }

            $ts.post("@login", post, function (result) {
                console.log(result);
            });
        }
    }
}