<?php

include dirname(__DIR__) . "/../framework/bootstrap.php";

class App {

    /**
     * 进行交易结算
     * 
     * @method POST
     * @uses api
    */
    public function settlement($goods, $discount, $transaction, $vip = -1) {
        # 获取商品信息
        $item_ids = array_keys($goods);
        $items = (new Table("goods"))
            ->where(["id" => in($item_ids)])
            ->limit(count($item_ids))
            ->select();
        $money = 0;
        $counts = 0;

        foreach($items as $goodItem) {
            $price = $goodItem["price"] * $goods[$goodItem["id"]];
            $money = $money + $price;
            $counts = $counts + $goods[$goodItem["id"]];
        }

        if ($vip > 0) {
            $vip_info = (new Table("VIP_members"))->where(["id" => $vip])->find();

            if (Utils::isDbNull($vip_info)) {
                controller::error("会员信息错误");
            }
    
            # 应付金额减去余额得到剩余应该支付的金额
            $left = $money - $vip_info["balance"];
            # 这计算得到通过会员余额支付的金额部分
            $balance_pay = $left <= 0 ? $money : $vip_info["balance"];
        } else {
            $balance_pay = 0;
        }

        # 添加商品交易信息
        $money = $money * $discount;
        $trade = (new Table("waterflow"))
            ->add([
                "goods" => json_encode($goods),
                "time" => Utils::Now(),
                # 交易的金额是总的
                "money" => $money,
                # 通过会员账户余额支付的部分                
                "vip_balance" => $balance_pay,
                "buyer" => $vip,
                "operator" => web::login_userId(),
                "count" => $counts,
                "discount" => $discount,
                "note" => "",
                "transaction_id" => $transaction
            ]);

        if (empty($trade) || $trade == false) {
            controller::error(ERR_MYSQL_INSERT_FAILURE);
        } else {
            $details = new Table("trade_items");
            $inventories = new Table("inventories");

            # 添加详细售卖信息
            foreach($items as $goodItem) {
                $counts = $goods[$goodItem["id"]];

                # 库存变化
                # 选择出一个剩余库存数量大于counts的批次
                $batch = $inventories
                    ->where(["item_id" => $goodItem["id"], "remnant" => gt_eq($counts)])
                    ->find();

                if (Utils::isDbNull($batch)) {
                    controller::error("商品信息错误");
                }

                $inventories->where(["id" => $batch["id"]])->save(["remnant" => "~`remnant` - $counts"]);
                $details->add([
                    "item_id" => $goodItem["id"],
                    "count" => $counts,
                    "batch_id" => $batch["id"],
                    "waterflow" => $trade
                ]);
            }

            if ($vip > 0) {
                # 修改会员余额和记录流水
                (new Table("VIP_members"))->where(["id" => $vip])->save(["balance" => "~balance - $balance_pay"]);
                # 添加流水记录
                (new Table("VIP_waterflow"))->add([
                    "vip" => $vip,
                    "balance" => $balance_pay,
                    "waterflow_id" => $trade,
                    "time" => Utils::Now(),
                    "note" => "会员消费",
                    "operator" => web::login_userId()
                ]);

                if ($left > 0) {
                    controller::error($left, 400);
                } else {
                    controller::success(1);
                }
            } else {
                controller::success(1);
            }
        }

        controller::error($items);
    }

    /**
     * 加载交易流水信息
    */
    public function load($page = 1, $page_size = 100) {
        $start = ($page - 1) * $page_size;
        $waterflow = new Table("waterflow");
        $list = $waterflow
            ->left_join("admin")
            ->on(["waterflow" => "operator", "admin" => "id"])   
            ->left_join("VIP_members")
            ->on(["waterflow" => "buyer", "VIP_members" => "id"])         
            ->limit($start, $page_size)
            ->order_by("id desc")
            ->select(["waterflow.*", "admin.realname as admin", "VIP_members.name as vip"]);

        if (empty($list) || $list == false || count($list) == 0) {
            controller::error("对不起，无查询结果数据", 1, $waterflow->getLastMySql());
        } else {
            controller::success($list);
        }
    }
}