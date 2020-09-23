namespace pages {

    export class POS extends Bootstrap {

        public get appName(): string {
            return "POS";
        }

        protected init(): void {
            window.setInterval(() => POS.clock, 1);
        }

        private static clock() {
            let time = new Date();
            let hh = Strings.PadLeft(time.getHours().toString(), 2, "0");
            let mm = Strings.PadLeft(time.getMinutes().toString(), 2, "0");

            console.log(time.toString());

            $ts("#clock").display(`${hh}:${mm}`);
        }

    }
}