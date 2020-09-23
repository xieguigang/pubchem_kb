namespace pages {

    export class billing extends Bootstrap {

        public get appName(): string {
            return "billing";
        }

        protected init(): void {
            let vm = this;
            let firstItem: string = localStorage.getItem(firstItemKey);

            // STATE BUTTON
            // =================================================================
            // Require Bootstrap Button
            // -----------------------------------------------------------------
            // http://getbootstrap.com/javascript/#buttons
            // =================================================================
            $('#settlement').on('click', function () {
                // 这个状态变化必须要通过jQuery来进行触发
                // 否则会出现丢失文档碎片的错误？
                $(this).button('loading');
                // business logic...
                vm.settlement();
            });

            if (Strings.Empty(firstItem, true)) {
                $goto("/POS");
            } else {
                localStorage.setItem(firstItemKey, null);
            }
        }

        /**
         * 点击账单结算按钮进行支付结算
        */
        private settlement() {

        }
    }
}