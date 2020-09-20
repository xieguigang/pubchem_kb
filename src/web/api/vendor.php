<?php

include dirname(__DIR__) . "/../framework/bootstrap.php";

class App {

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
            "note" => $note
        ]);

        if ($result == false) {
            controller::error("对不起，数据库错误或者数据库已离线，请稍后重试。");
        } else {
            controller::success($result);
        }
    }
}