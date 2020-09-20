<?php

include dirname(__DIR__) . "/framework/bootstrap.php";

class App {

    /**
     * @access *
     * @uses api
     * @method POST
    */
    public function login($user, $passwd) {
        $check = (new Table("admin"))
            ->where([
                "lower(`realname`)|lower(`email`)" => strtolower($user),
                "lower(`password`)" => strtolower($passwd)
            ])
            ->find();

        if (empty($check) || $check == false) {
            controller::error("用户账号未找到或者密码错误！");
        } else {
            foreach($check as $item => $val) {
                $_SESSION[$item] = $val;
            }

            $_SESSION["lockscreen"] = false;

            controller::success("/");
        }
    }

    /**
     * 锁屏
    */
    public function lockscreen() {
        $_SESSION["lockscreen"] = true;
        redirect("/lockscreen");
    }

    /**
     * 解锁屏幕
     * 
     * @method POST
     * @uses api
    */
    public function unlock($passwd) {
        if (strtolower($_SESSION["password"]) != strtolower($passwd)) {
            controller::error("对不起，请输入正确的当前用户登录密码！");
        } else {
            $_SESSION["lockscreen"] = false;
            controller::success("欢迎回来！");
        }
    }
}