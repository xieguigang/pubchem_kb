<?php

include dirname(__DIR__) . "/framework/bootstrap.php";

class App {

    /**
     * 锁屏
    */
    public function lockscreen() {
        $_SESSION["lockscreen"] = true;
        redirect("/lockscreen");
    }
}