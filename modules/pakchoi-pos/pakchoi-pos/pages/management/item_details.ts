namespace pages {

    export class item extends Bootstrap {

        public get appName(): string {
            return "goods_item";
        };

        protected init(): void {

        }

        public print() {
            app.print("#item-details");
        }
    }
}