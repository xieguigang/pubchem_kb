namespace pages {

    export class password_reminder extends Bootstrap {

        public get appName(): string {
            return "password_reminder";
        }

        protected init(): void {

        }

        public send() {
            let email: string = Strings.Trim($ts.value("#email"));
            let tokens: string[] = email.split("@");

            if (tokens.length == 0) {
                return nifty.errorMsg("电子邮件地址不可以为空！");
            } else if (tokens.length == 1) {
                return nifty.errorMsg("您所输入的电子邮件地址格式不正确！");
            } else {
                // BOOTBOX - CUSTOM HTML CONTENTS
                // =================================================================
                // Require Bootbox
                // http://bootboxjs.com/
                // =================================================================
                bootbox.dialog({
                    title: "密码重置",
                    message: `
<div class="media">
    <div class="media-left">
        <img class="media-object img-lg img-circle" src="/assets/img/email-marketing-subject-line-icons.jpg" alt="Profile picture">
    </div>
    <div class="media-body">
        <p class="text-semibold text-main">
            我们已经向${email}发送了一封密码重置所需要的电子邮件，您需要登录该电子邮箱，按照邮件中的提示完成密码重置操作。
        </p>
    </div>
</div>`,
                    buttons: {
                        confirm: {
                            label: "确定"
                        }
                    }
                });
            }
        }
    }
}