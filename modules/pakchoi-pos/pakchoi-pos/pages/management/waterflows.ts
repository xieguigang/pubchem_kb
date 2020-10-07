namespace pages {

    export class waterflows extends Bootstrap {

        public get appName(): string {
            return "waterflows";
        }

        protected init(): void {
            this.loadWaterflows();
        }

        private loadWaterflows(page: number = 1) {
            $ts.get(`@load?page=${page}`, function (result) {


            })
        }
    }
}