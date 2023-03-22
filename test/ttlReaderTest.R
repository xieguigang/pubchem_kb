require(pubchem);

setwd(@dir);

data = parseTtl("../test/pc_disease.ttl", lazy = FALSE);
data = data |> to_ttlobject;

require(JSON);

data 
|> json_encode()
|> writeLines(con = "./pc_disease.json")
;

data = parseTtl("E:\pubchem\gene\pc_gene.ttl", lazy = FALSE);
data = data |> to_ttlobject;

require(JSON);

data 
|> json_encode()
|> writeLines(con = "./pc_gene.json")
;