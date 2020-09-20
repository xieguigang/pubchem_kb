<?php

include dirname(__DIR__) . "/../framework/bootstrap.php";

class App {

    public function load($page = 1, $page_size = 100) {
        $start = ($page - 1) * $page_size;
        $list  = (new Table("vendor"))
            ->left_join("admin")
            ->on(["vendor" => "operator", "admin" => "id"])
            ->limit($start, $page_size)
            ->order_by("id desc")
            ->select(["vendor.*", "admin.realname"]); 

        if (empty($list) || $list == false || count($list) == 0) {
            controller::error("对不起，无查询结果数据");
        } else {
            controller::success($list);
        }
    }

    /**
     * @uses api
     * @method POST
    */
    public function add($name, $tel, $url, $address, $note) {
        $check = (new Table("vendor"))
            ->where(["tolower(`name`)" => strtolower($name)])
            ->find();

        if (!(empty($check) || $check == false)) {
            controller::error("供应商'$name'已经存在与系统中了。");
        }

        $result = (new Table("vendor"))->add([
            "name" => $name,
            "tel" => $tel,
            "url" => $url,
            "address" => $address,
            "note" => $note,
            "add_time" => Utils::Now(),
            "operator" => web::login_userId(),
            "status" => 0
        ]);

        if ($result == false) {
            controller::error("对不起，数据库错误或者数据库已离线，请稍后重试。");
        } else {
            controller::success($result);
        }
    }

    /**
     * @method POST
     * @uses api
    */
    public function change_status($id) {
        $vendor = (new Table("vendor"))->where(["id" => $id])->find();

        if (empty($vendor) || $vendor == false) {
            controller::error("对不起，目标供应商不存在");
        } else {
            $status = $vendor["status"] == 0 ? 1 : 0;

            (new Table("vendor"))
                ->where(["id" => $id])
                ->save(["status" => $status]);
            controller::success("1");
        }
    }
}