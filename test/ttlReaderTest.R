require(pubchem);

setwd(@dir);

data = parseTtl("../test/pc_disease.ttl", lazy = FALSE);

require(JSON);

data 
|> json_encode()
|> writeLines(con = "./pc_disease.json")
;