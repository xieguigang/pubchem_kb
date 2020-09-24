<?php

include dirname(__DIR__) . "/../framework/bootstrap.php";

class App {

    /**
     * 进行交易结算
     * 
     * @method POST
     * @uses api
    */
    public function settlement($goods) {
        $item_ids = array_keys($goods);
        $items = (new Table("goods"))
            ->where(["item_id" => in($item_ids)])
            ->limit(count($item_ids))
            ->select();

        
    }
}