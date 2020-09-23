namespace pages {

    export class POS extends Bootstrap {

        private scanner: Scanner;

        public get appName(): string {
            return "POS";
        }

        protected init(): void {
            let vm = this;

            vm.scanner = new Scanner(POS.startBilling);
            // idle events
            window.setInterval(() => this.clock(), 1000);
        }

        private static startBilling(item_id: string) {
            localStorage.setItem("first-item", item_id);
            $goto("/billing");
        }

        private clock() {
            let time = new Date();
            let hh = Strings.PadLeft(time.getHours().toString(), 2, "0");
            let mm = Strings.PadLeft(time.getMinutes().toString(), 2, "0");
            let ss = Strings.PadLeft(time.getSeconds().toString(), 2, "0");

            $ts("#clock").display(`${hh}:${mm}:${ss}`);
            $ts("#date").display(time.toDateString());
        }

    }
}