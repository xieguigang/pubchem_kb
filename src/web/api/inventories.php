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
            controller::error("没有找到目标商品，请确认商品编码正确或者先【<a href='/goods' target='__blank'>创建商品信息</a>】。");
        }

        if (Strings::Empty($batch_id, true)) {
            $batch_id = $item_id . year() . month() . str_pad($inventories->where(["item_id" => $item["id"]])->count(), 6, "0", STR_PAD_LEFT);
        }

        $check = (new Table("inventories"))->where(["batch_id" => $batch_id])->find();

        if ((!empty($check)) || $check !== false) {
            controller::error("对不起，当前的库存批次编号【{$batch_id}】已经重复，请输入一个新的库存批次编号。");
        }

        $result = $inventories->add([
            "batch_id" => $batch_id,
            "inbound_time" => Utils::Now(),
            "item_id" => $item["id"],
            "count" => $count,
            "note" => $note
        ]);

        if ($result == false) {
            controller::error("对不起，数据库繁忙或者数据离线，请稍后重试。。。");
        } else {
            controller::success("入库成功！");
        }
    }
}