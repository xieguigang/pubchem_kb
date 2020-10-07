namespace pages {

    export class trade extends Bootstrap {

        public get appName(): string {
            return "trade";
        };

        protected init(): void {

        }

        public print() {
            app.print("#trade-details");
        }
    }
}