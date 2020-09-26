<?php

include dirname(__DIR__) . "/../framework/bootstrap.php";

class App {

    /**
     * 添加会员信息
     * 
     * @uses api
     * @method POST
    */
    public function add($card_id, $name, $phone, $address, $gender, $note) {
        $check = (new Table("VIP_members"))->where(["card_id" => $card_id])->find();

        if (!(empty($check) || $check == false)) {
            controller::error("对不起，当前输入的会员卡已经被使用了。");
        }

        $result = (new Table("VIP_members"))->add([
            "name" => $name,
            "gender" => $gender,
            "phone" => $phone,
            "address" => $address,
            "join_time" => Utils::Now(),
            "operator" => web::login_userId(),
            "note" => $note,
            "card_id" => $card_id,
            "balance" => 0
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
            ->where(["`VIP_members`.`flag`" => 0])
            ->limit($start, $page_size)
            ->order_by("id desc")
            ->select(["VIP_members.*", "admin.realname as admin"]);

        if (empty($list) || $list == false || count($list) == 0) {
            controller::error("对不起，无查询结果数据", 1, $VIP_members->getLastMySql());
        } else {
            controller::success($list);
        }
    }

    /**
     * @uses api
     * @method POST
    */
    public function delete($id, $name) {
        $check = (new Table("VIP_members"))->where(["id" => $id])->find();

        if (Utils::isDbNull($check) || $check["name"] != $name) {
            controller::error("对不起，没有找到这个会员或者输入的会员名确认信息不正确！");
        }

        // 为了保持记录完整性，这里不进行物理删除
        (new Table("VIP_members"))->where(["id" => $id])->save(["flag" => 1]);

        controller::success("1");
    }

    /**
     * @uses api
    */
    public function get($card_id) {
        $vip = (new Table("VIP_members"))->where(["card_id" => $card_id, "flag" => 0])->find();

        if (Utils::isDbNull($vip)){
            controller::error("对不起，没有找到卡号为{$card_id}的会员记录");
        } else {
            foreach(array_keys($vip) as $field) {
                if (Utils::isDbNull($vip[$field]) && $field != "balance") {
                    $vip[$field] = "没有填写";
                }
            }

            controller::success($vip);
        }
    }

    /**
     * @uses api
     * @method POST
    */
    public function charge($id, $add) {

    }
}