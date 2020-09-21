<?php

include dirname(__DIR__) . "/../framework/bootstrap.php";

class App {

    /**
     * @uses api
     * 
    */
    public function add($item_id, $name, $vendor_id, $price, $note, $gender) {
        $check = (new Table("goods"))
            ->where(["item_id" => $item_id])
            ->find();

        if (empty($check) || $check == false) {
            $result = (new Table("goods"))->add([
                "name" => $name,
                "add_time" => Utils::Now(),
                "price" => $price,
                "gender" => $gender,
                "display" => "",
                "item_id" => $item_id,
                "vendor_id" => $vendor_id,
                "operator" => web::login_userId(),
                "note" => $note
            ]);

            if ($result == false) {
                controller::error(ERR_MYSQL_INSERT_FAILURE);
            } else {
                controller::success($result);
            }
        } else {
            controller::error("已经存在商品编号【{$item_id}】了。");
        }
    }
}