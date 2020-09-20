<?php

session_start();

define("APP_SRC_ROOT", dirname(__DIR__));
define("APP_DOC_ROOT", dirname(APP_SRC_ROOT));

include APP_SRC_ROOT . "/../modules/php-net/package.php";
include APP_SRC_ROOT . "/framework/accessController.php";
include APP_SRC_ROOT . "/framework/web.php";

imports("php.export");

define("YEAR", year());

dotnet::AutoLoad(APP_SRC_ROOT . "/.etc/config.ini.php");
dotnet::HandleRequest(new App(), new accessController());