class Scanner {

    private lastTime: number = null;
    private nextTime: number = null;
    private code: string = '';
    private keyboardInput: string = "";

    /**
     * 注册扫码枪输入事件
    */
    constructor(private scanInput: (codeInput: string) => void) {
        let vm = this;

        document.onkeydown = function (e) {
            vm.nextTime = new Date().getTime();
            vm.scanCode(e.keyCode || e.which || e.charCode, e);
        }
    }

    private triggerEvt() {
        this.scanInput(this.code || this.keyboardInput);

        this.code = "";
        this.keyboardInput = "";
    }

    private scanCode(keycode: number, e: KeyboardEvent) {
        if (keycode === 13) {
            if (this.lastTime && (this.nextTime - this.lastTime < 30)) {
                // 扫码枪输入
                // do something
                this.triggerEvt();
            } else {
                // 键盘输入
                // do nothing
            }

            this.code = '';
            this.lastTime = null;

            e.preventDefault();
        } else {
            let c: string = String.fromCharCode(keycode);

            if (!this.lastTime) {
                this.code = c;
                this.keyboardInput = c;
            } else {
                if (this.nextTime - this.lastTime < 30) {
                    this.code += c;
                } else {
                    this.code = '';
                    this.keyboardInput += c;

                    // 上上下下左右左右BA进入测试模式
                    if (this.keyboardInput.toUpperCase() == "&&((%'%'BA") {
                        this.triggerEvt();
                    }
                }
            }

            this.lastTime = this.nextTime;
        }
    }
}