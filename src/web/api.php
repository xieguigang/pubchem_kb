<?php

include dirname(__DIR__) . "/framework/bootstrap.php";

class App {

    /**
     * @access *
     * @uses api
     * @method POST
    */
    public function login($user, $passwd) {
        $admin = new Table("admin");
        $check = $admin
            ->where([
                "flag" => 0,
                "lower(`realname`)|lower(`email`)" => strtolower($user),
                "lower(`password`)" => strtolower($passwd)
            ])
            ->find();

        if (Utils::isDbNull($check)) {
            controller::error("用户账号未找到或者密码错误！", -1, $admin->getLastMySql());
        } else {
            foreach($check as $item => $val) {
                $_SESSION[$item] = $val;
            }

            $_SESSION["lockscreen"] = false;

            controller::success("/");
        }
    }

    /**
     * 
    */
    public function logout() {
        unset($_SESSION["id"]);
        redirect("/login"); 
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