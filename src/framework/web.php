<?php

define("ERR_MYSQL_INSERT_FAILURE", "对不起，数据库服务器繁忙或者数据库已离线，请稍后重试。。。");

class web {
	
	public static function login_userId() {
		return self::session("id", -1);
	}

	public static function login_userName() {
		return self::session("realname", "未登录");
	}

	public static function login_userRole() {
		return self::session("role", "普通收银员");
	}

	public static function login_userMail() {
		return self::session("email", "未登录");
	}

	public static function log($code) {
		(new Table("page_views"))->add([
			"page" => Utils::URL(false),
			"code" => $code,
			"time" => Utils::Now(),
			"operator" => self::login_userId(),
			"user_agent" => $_SERVER['HTTP_USER_AGENT']
		]);

		if ($code == 200) {
			return true;
		} else {
			return false;
		}
	}

	public static function session($name, $default = null) {
		if (array_key_exists($name, $_SESSION) && !empty($_SESSION[$name])) {
			return $_SESSION[$name];
		} else {
			return $default;
		}
	}
}