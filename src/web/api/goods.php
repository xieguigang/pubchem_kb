<?php

include dirname(__DIR__) . "/../framework/bootstrap.php";

class App {

    /**
     * @uses api
     * @method POST
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

    public static function load($page = 1, $page_size = 100) {
        $start = ($page - 1) * $page_size;
        $goods = new Table("goods");
        $list  = $goods
            ->left_join("admin")
            ->on(["goods" => "operator", "admin" => "id"])
            ->left_join("vendor")
            ->on(["goods" => "vendor_id", "vendor" => "id"])
            ->limit($start, $page_size)
            ->order_by("id desc")
            ->select(["goods.*", "admin.realname", "`vendor`.`name` as vendor"]);         

        if (empty($list) || $list == false || count($list) == 0) {
            controller::error("对不起，无查询结果数据", 1, $goods->getLastMySql());
        } else {
            controller::success($list);
        }
    }
}