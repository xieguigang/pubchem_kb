<?php

include dirname(__DIR__) . "/../framework/bootstrap.php";

class App {

    /**
     * 库存变化表格
     * 
     * @uses api
    */
    public function inventories() {
        imports("System.DateTime");
        
        $now = new \System\DateTime();
        $week = $now->Date();
        
        $week->modify("-7 day");

        $inventory = (new Table("inventories"));
        
        $all = $inventory->ExecuteScalar("sum(`count`)");
        $sales = $all - $inventory->ExecuteScalar("sum(`remnant`)");
        $aweek = $week->format("Y-m-d");
        $aweek = (new Table("trade_items"))
            ->where(["time" => between($aweek, $now->ToString())])
            ->group_by('DATE_FORMAT(`time`, "%Y-%m-%d")')
            ->order_by('DATE_FORMAT(`time`, "%Y-%m-%d")', false)
            ->select(['DATE_FORMAT(`time`, "%Y-%m-%d") as day', "sum(`count`) as updates"]);
        $vec = [];

        foreach($aweek as $day) {
            $vec[] = $day["updates"];
        }

        controller::success([
            "sales" => $sales,
            "total" => $all,
            "sparkline" => $vec
        ]);
    }

}