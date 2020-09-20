<?php

imports("MVC.controller");
imports("MVC.restrictions");

class accessController extends controller {
    
    public function __construct() {
        # 将用户名显示在导航栏的右上角
        View::Push("user.name", web::login_userName());
        View::Push("user.role", web::login_userRole());
        View::Push("user.email", web::login_userMail());
        //View::Push("user.islogin", BioDeep::IsUserLogin());
        View::Push("appTitle", DotNetRegistry::Read("APP_TITLE", null));
    }

    public function accessControl() {
        $access = $this->getAccessLevel();        

        if ($this->AccessByEveryOne()) {
            return true;
        } else if ($_SESSION["lockscreen"] && $_SERVER["REDIRECT_URL"] != "/lockscreen" && $_SERVER["REDIRECT_URL"] != "/api/unlock") {            
            redirect("/lockscreen");
            exit(0);
        }
        
        if (empty($access)) {
            // 什么也没有填写的时候，默认为登录用户才可以访问
            return web::login_userId() > 0;
        } else {
            // access现在不为空的了
            $access = explode("|", $access);
        } 
        
        // 例如控制器被标记为
        // @access wechat|admin
        // 表示只能够通过微信端或者具有管理员权限的用户才可以访问当前的这个控制器

        // 控制器被标记为
        // @access admin
        // 表示只能够具有管理员权限的用户才可以访问当前的这个控制器
        //
        // 则权限管理可以通过下面的循环来实现
        $roles = self::getUserRoleEnums();

        foreach ($access as $role) {
            if (array_key_exists($role, $roles)) {
                if (self::isRole($roles[$role])) {
                    return true;
                }
            }
        }
        
        // 当前的用户任何权限条件都不满足，则禁止访问当前的这个控制器
        return false;
    }

    private static function isRole($roleId) {
        return false;
    }

    private static function getUserRoleEnums() {
        return [
            "admin" => 999
        ];
    }
}