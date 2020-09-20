namespace nifty {

    export function errorMsg(msg: string) {
        (<any>$).niftyNoty({
            type: 'danger',
            message: msg,
            container: 'floating',
            timer: 5000
        });
    }
}