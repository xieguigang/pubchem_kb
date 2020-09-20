<?php

# template for do website configuration
define("AppViews", __DIR__ . "/views");

return [
    'DB_TYPE' => 'mysql',
    'DB_HOST' => '127.0.0.1',
    'DB_NAME' => 'root',
    'DB_USER' => 'root',
    'DB_PWD'  => 'root',
    'DB_PORT' => '3306',

    // 框架配置参数
	"ERR_HANDLER_DISABLE" => "FALSE",
    "RFC7231"       => __DIR__ . "/views/http_errors/",
    "show.stacktrace" => false,
	"CACHE" => false,
    "APP_NAME" => "pakchoi-pos",
    "APP_TITLE" => "小白菜收银系统",
    "APP_VERSION" => "0.0.0.1-alpha",
	"MVC_VIEW_ROOT" => [		
		"index" => AppViews
    ],
	
	// config for email sender
	"mailer" => [
		"server" => "email.com",
		"user"  => "username",
		"password" => "*******",
		"port" => 994 
	]
];