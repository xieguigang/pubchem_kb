<?php

include dirname(__DIR__) . "/framework/bootstrap.php";

class App {

	/**
	 * @access *
	*/
	public function login() {
		View::Display();
	}

	public function phpinfo() {
		phpinfo();		
	}
}