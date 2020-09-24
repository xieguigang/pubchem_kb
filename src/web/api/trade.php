<?php

include dirname(__DIR__) . "/../framework/bootstrap.php";

class App {

    /**
     * 进行交易结算
     * 
     * @method POST
     * @uses api
    */
    public function settlement($goods, $discount) {
        $item_ids = array_keys($goods);
        $items = (new Table("goods"))
            ->where(["id" => in($item_ids)])
            ->limit(count($item_ids))
            ->select();

        controller::error($items);
    }
}