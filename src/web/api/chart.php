<?php

include dirname(__DIR__) . "/../framework/bootstrap.php";

class App {

    public function __construct() {
        imports("System.DateTime");
    }

    /**
     * 销售收入变化曲线
     * 
     * @uses api
    */
    public function sales() {
        $now = new \System\DateTime();
        $week = $now->Date();
        
        $week->modify("-7 day");
        $aweek = $week->format("Y-m-d");

        $waterflow = (new Table("waterflow"));
        $day_begin = \System\DateTime::DayOfStart();
        $day_ends = \System\DateTime::DayOfEnd();

        $all = $waterflow->where(["time" => between($aweek, $now->ToString())])->ExecuteScalar("sum(`money`)");
        $sales = $waterflow->where(["time" => between($day_begin, $day_ends)])->ExecuteScalar("sum(`money`)");
        
        $aweek = $waterflow
            ->where(["time" => between($aweek, $now->ToString())])
            ->group_by('DATE_FORMAT(`time`, "%Y-%m-%d")')
            ->order_by('DATE_FORMAT(`time`, "%Y-%m-%d")', false)
            ->select(['DATE_FORMAT(`time`, "%Y-%m-%d") as day', "sum(`money`) as updates"]);       

        controller::success([
            "day" => $sales,
            "total" => $all,
            "sparkline" => self::sparkline_vector($aweek, 0, function($val, $day) {
                return $val + $day;
            })
        ]);
    }

    private static function sparkline_vector($aweek, $val, $aggreat) {
        $vec = [];        
        $yesterday = \System\DateTime::DayOfEnd()->Date();
        $oneDay    = date_interval_create_from_date_string('1 day');
        $dayFlags  = [];

        foreach($aweek as $day) {
            $dayFlags[$day["day"]] = $day["updates"];
        }      

        for($i = 0; $i < 7; $i++) {
            $yesterday = $yesterday->sub($oneDay);
            $yesterday = new \System\DateTime($yesterday);
            $key       = "{$yesterday->Year}-{$yesterday->Month}-{$yesterday->Day}";

            if (array_key_exists($key, $dayFlags)) {
                $val = $aggreat($val, floatval($dayFlags[$key]));            
            }

            $vec[] = $val;         
            $yesterday = $yesterday->Date();
        }

        return $vec;
    }

    /**
     * 库存变化表格
     * 
     * @uses api
    */
    public function inventories() {        
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
        
        controller::success([
            "sales" => $sales,
            "total" => $all,
            "sparkline" => self::sparkline_vector($aweek, floatval($all), function($val, $day) {
                return $val - $day;
            })
        ]);
    }

    /**
     * 销售直方图
     * 
     * @uses api
    */
    public function inventories_hist() {    
        $now = new \System\DateTime();
        $week = $now->Date();
        
        $week->modify("-7 day");
        $aweek = $week->format("Y-m-d");

        $day_begin = \System\DateTime::DayOfStart();
        $day_ends = \System\DateTime::DayOfEnd();

        $inventory = (new Table("inventories"));
        
        $all = $inventory->where(["time" => between($aweek, $now->ToString())])->ExecuteScalar("sum(`count`)");
        $aweek = $week->format("Y-m-d");
        $aweek = (new Table("trade_items"))
            ->where(["time" => between($aweek, $now->ToString())])
            ->group_by('DATE_FORMAT(`time`, "%Y-%m-%d")')
            ->order_by('DATE_FORMAT(`time`, "%Y-%m-%d")', false)
            ->select(['DATE_FORMAT(`time`, "%Y-%m-%d") as day', "sum(`count`) as updates"]);
        $today = $aweek[$now->Date()->format("Y-m-d")]["updates"];

        controller::success([
            "today" => $today,
            "total" => $all,
            "sparkline" => self::sparkline_vector($aweek, floatval($all), function($val, $day) {
                return $day;
            })
        ]);
    }
}