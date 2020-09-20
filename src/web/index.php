<?php

include dirname(__DIR__) . "/framework/bootstrap.php";

class App {

	public function index() {
		View::Display();
	}

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