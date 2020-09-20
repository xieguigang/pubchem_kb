namespace nifty {

    export function errorMsg(msg: string) {
        (<any>$).niftyNoty({
            type: 'danger',
            message: msg,
            container: 'floating',
            timer: 5000
        });
    }

    export function showAlert(message: string) {
        $ts("#alert").show();
        $ts("#message").show().display(message);
    }
}