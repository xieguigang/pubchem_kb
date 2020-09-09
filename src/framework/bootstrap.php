<?php

define("APP_SRC_ROOT", dirname(__DIR__));

include APP_SRC_ROOT . "/../modules/php-net/package.php";
include APP_SRC_ROOT . "/framework/accessController.php";

dotnet::AutoLoad(APP_SRC_ROOT . "/.etc/config.ini.php");
dotnet::HandleRequest(new App(), new accessController());