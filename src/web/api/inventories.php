<?php

include dirname(__DIR__) . "/../framework/bootstrap.php";

class App {

    /**
     * 商品入库进货
     * 
     * @uses api
     * @method POST
    */
    public function add($item_id, $batch_id, $count, $note = "") {
        $inventories = new Table("inventories");
        $item = (new Table("goods"))
            ->where(["vendor_item_id" => $item_id])
            ->find();

        if (empty($item) || $item == false) {
            controller::error("没有找到目标商品，请确认商品编码正确或者先【<a href='/goods'>创建商品信息</a>】。");
        }

        if (Strings::Empty($batch_id, true)) {
            $batch_id = $item_id . year() . month() . str_pad($inventories->where([]));
        }

        $check = (new Table("inventories"));
    }
}