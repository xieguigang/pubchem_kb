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
}