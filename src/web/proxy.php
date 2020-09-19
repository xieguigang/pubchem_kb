<?php

include dirname(__DIR__) . "/framework/bootstrap.php";

class App {

    /**
     * @access *
     * @require file=string
    */
    public function assets() {
        $file = WebRequest::getPath("file");
        $file = APP_DOC_ROOT . "/assets/$file";

        Utils::PushDownload($file);
    }
}