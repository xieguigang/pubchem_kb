<?php

include dirname(__DIR__) . "/../framework/bootstrap.php";

class App {

    /**
     * 进行交易结算
     * 
     * @method POST
     * @uses api
    */
    public function settlement($goods, $discount, $vip = -1) {
        $item_ids = array_keys($goods);
        $items = (new Table("goods"))
            ->where(["id" => in($item_ids)])
            ->limit(count($item_ids))
            ->select();
        $money = 0;
        $counts = 0;

        foreach($items as $goodItem) {
            $price = $goodItem["price"] * $goods[$goodItem["id"]];
            $money = $money + $price;
            $counts = $counts + $goods[$goodItem["id"]];
        }

        $money = $money * $discount;
        $trade = (new Table("waterflow"))
            ->add([
                "goods" => json_encode($goods),
                "time" => Utils::Now(),
                "money" => $money,
                "buyer" => $vip,
                "operator" => web::login_userId(),
                "count" => $counts,
                "discount" => $discount,
                "note" => ""
            ]);

        if (empty($trade) || $trade == false) {
            controller::error(ERR_MYSQL_INSERT_FAILURE);
        } else {
            $details = new Table("trade_items");

            foreach($items as $goodItem) {
                $counts = $goods[$goodItem["id"]];
                $details->add([
                    "item_id" => $goodItem["id"],
                    "count" => $counts,
                    "batch_id" => -1,
                    "waterflow" => $trade
                ]);
            }
        }

        controller::error($items);
    }
}