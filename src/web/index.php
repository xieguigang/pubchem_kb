<?php

include dirname(__DIR__) . "/framework/bootstrap.php";

class App {

	/**
	 * 系统首页
	 * 
	 * @uses view
	*/
	public function index() {
		View::Display();
	}

	/**
	 * 登录系统
	 * 
	 * @uses view
	 * @access *
	*/
	public function login() {
		View::Display();
	}

	/**
	 * 系统锁屏
	 * 
	 * @uses view
	*/
	public function lockscreen() {
		View::Display();
	}

	/**
	 * 商品库存管理
	 * 
	 * @uses view
	*/
	public function inventories() {
		View::Display();
	}

	/**
	 * 重置密码
	 * 
	 * @uses view
	 * @access *
	*/
	public function password_reminder() {
		View::Display();
	}

	/**
	 * 商品信息管理
	 * 
	 * @uses view
	*/
	public function goods() {
		View::Display();
	}

	/**
	 * 供应商信息管理
	 * 
	 * @uses view
	*/
	public function vendor() {
		View::Display();
	}

	/**
	 * 销售流水记录
	 * 
	 * @uses view
	*/
	public function waterflow() {
		View::Display();
	}

	/**
	 * 账单结算
	 * 
	 * @uses view
	*/
	public function billing() {
		View::Display();
	}

	/**
	 * 会员信息管理
	 * 
	 * @uses view
	*/
	public function VIP() {
		View::Display();
	}

	/**
	 * 商品销售前台
	 * 
	 * @uses view
	*/
	public function POS() {
		View::Display();
	}

	public function phpinfo() {
		phpinfo();		
	}
}