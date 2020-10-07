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
	 * 交易详情
	 * 
	 * @uses view
	*/
	public function trade($transaction) {
		$transaction = (new Table("waterflow"))
			->left_join("admin")
			->on(["waterflow" => "operator", "admin" => "id"])
			->left_join("VIP_members")
			->on(["waterflow" => "buyer", "VIP_members" => "id"])
			->where(["transaction_id" => $transaction])
			->find([
				"waterflow.*",
				"VIP_members.name as vip",
				"admin.realname as admin"
			]);

		if (Utils::isDbNull($transaction)) {
			dotnet::PageNotFound("the given transaction is not exists!");
		}

		if (empty($transaction["vip"])) {
			$transaction["vip"] = "非会员";
		}
		if (Strings::Empty($transaction["note"])) {
			$transaction["note"] = "没有描述信息";
		}

		$goods = json_decode($transaction["goods"]);
		$list = [];
		$goods_info = (new Table("goods"));
		$batch_info = (new Table("trade_items"));

		foreach($goods as $item => $count) {
			$item = $goods_info
				->left_join("vendor")
				->on(["vendor" => "id", "goods" => "vendor_id"])
				->where(["`goods`.`id`" => $item])
				->find([
					"goods.*",
					"vendor.name as vendor"
				]);				

			if (empty($item["vendor"])) {
				$item["vendor"] = "无供应商";
			}

			$batch_id = $batch_info
				->left_join("inventories")
				->on(["trade_items" => "batch_id", "inventories" => "id"])
				->where(["`trade_items`.`item_id`" => $item["id"], "waterflow" => $transaction["id"]])
				->limit(1)
				->select("`inventories`.`batch_id` as batch_id")[0];

			if (Utils::isDbNull($batch_id)) {
				$batch_id = "n/a";
			} else {
				$batch_id = $batch_id["batch_id"];
			}

			$item["batch"] = $batch_id;
			$item["count"] = $count;
			$list[] = $item;
		} 

		$transaction["goods"] = $list;

		View::Display($transaction);
	}

	/**
	 * 账单结算
	 * 
	 * @uses view
	*/
	public function billing() {
		View::Display([
			"card_prefix" => "619",
			"transaction.no" => Utils::UnixTimeStamp() . "-" . str_pad(web::login_userId(), 4, "0", STR_PAD_LEFT),
			"transaction.time" => Utils::Now()
		]);
	}

	/**
	 * 会员信息管理
	 * 
	 * @uses view
	*/
	public function VIP($card_id = null) {
		if (empty($card_id)) {
			View::Display();
		} else {
			View::Show(APP_DOC_ROOT . "/src/.etc/views/VIP_details.html", ["title" => "会员信息详情"]);
		}
	}

	/**
	 * 商品销售前台
	 * 
	 * @uses view
	*/
	public function POS() {
		View::Display();
	}

	/**
	 * 查看服务器信息
	 * 
	 * @uses view
	*/
	public function phpinfo() {
		phpinfo();		
	}
}