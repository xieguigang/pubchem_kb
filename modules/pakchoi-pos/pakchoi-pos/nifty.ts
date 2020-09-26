namespace nifty {

    export function errorMsg(msg: string, callback: Delegate.Action = null) {
        (<any>$).niftyNoty({
            type: 'danger',
            message: msg,
            container: 'floating',
            timer: 5000,
            onHidden: callback
        });
    }

    export function showAlert(message: string) {
        $ts("#alert").show();
        $ts("#message").show().display(message);
    }

    export function clearAlert() {
        $ts("#alert").hide();
        $ts("#message").hide().clear();
    }
}