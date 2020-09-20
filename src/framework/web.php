<?php

class web {
	
	public static function login_userId() {
		return self::session("id", 9999);
	}

	public static function login_userName() {
		return self::session("realname", "未登录");
	}

	public static function login_userRole() {
		return self::session("role", "未登录");
	}

	public static function session($name, $default = null) {
		if (array_key_exists($name, $_SESSION)) {
			return $_SESSION[$name];
		} else {
			return $default;
		}
	}
}