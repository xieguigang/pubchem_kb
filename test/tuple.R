require(pubchem);

setwd(@dir);

data = get_cid_tupleData("E:\pubchem\descriptor\compound\pc_descr_canSMILES_value_000001.ttl", lazy = FALSE);

write.csv(data, file = "./pc_descr_canSMILES_value_000001.csv");

require(JSON);


for(x in data) {
    print(json_encode(x));
}
