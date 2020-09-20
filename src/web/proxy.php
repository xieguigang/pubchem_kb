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

        if (!file_exists($file)) {
            dotnet::PageNotFound("the required resource file is not found!");
        } else {
            Utils::PushDownload($file);
        }
    }

    /**
     * @access *
     * @require file=string
    */
    public function script() {
        $file = WebRequest::getPath("file");
        $file = APP_DOC_ROOT . "/modules/build/$file";

        if (!file_exists($file)) {
            dotnet::PageNotFound("the required app script file is not found!");
        } else {
            Utils::PushDownload($file);
        }
    }
}