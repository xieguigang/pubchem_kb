<?php

include dirname(__DIR__) . "/../framework/bootstrap.php";

class App {

    /**
     * 添加会员信息
     * 
     * @uses api
     * @method POST
    */
    public function add($name, $phone, $address, $gender, $note) {
        $check = (new Table("VIP_members"))->where(["name" => $name])->find();

        if (!(empty($check) || $check == false)) {
            controller::error("对不起，会员名已经重复了。");
        }

        $result = (new Table("VIP_members"))->add([
            "name" => $name,
            "gender" => $gender,
            "phone" => $phone,
            "address" => $address,
            "join_time" => Utils::Now(),
            "operator" => web::login_userId(),
            "note" => $note
        ]);

        if ($result == false) {
            controller::error(ERR_MYSQL_INSERT_FAILURE);
        } else {
            controller::success($result);
        }
    }

    public function load($page = 1, $page_size = 100) {
        $start = ($page - 1) * $page_size;
        $VIP_members = new Table("VIP_members");
        $list = $VIP_members 
            ->left_join("admin")
            ->on(["VIP_members" => "operator", "admin" => "id"])
            ->limit($start, $page_size)
            ->order_by("id desc")
            ->select(["VIP_members.*", "admin.realname as admin"]);

        if (empty($list) || $list == false || count($list) == 0) {
            controller::error("对不起，无查询结果数据", 1, $VIP_members->getLastMySql());
        } else {
            controller::success($list);
        }
    }
}