﻿class Scanner {

    private lastTime: number = null;
    private nextTime: number = null;
    private code: string = '';

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

    private scanCode(keycode: number, e: KeyboardEvent) {
        if (keycode === 13) {
            if (this.lastTime && (this.nextTime - this.lastTime < 30)) {
                // 扫码枪输入
                // do something
                this.scanInput(this.code);
            } else {
                // 键盘输入
                // do nothing
            }

            this.code = '';
            this.lastTime = null;

            e.preventDefault();
        } else {
            if (!this.lastTime) {
                this.code = String.fromCharCode(keycode);
            } else {
                if (this.nextTime - this.lastTime < 30) {
                    this.code += String.fromCharCode(keycode);
                } else {
                    this.code = '';
                }
            }

            this.lastTime = this.nextTime;
        }
    }
}