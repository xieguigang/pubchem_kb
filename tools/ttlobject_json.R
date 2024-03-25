require(pubchem);
require(JSON);

const ttl_file as string    = ?"--ttl"    || stop("no ttl source data file was provided!");
const json_export as string = ?"--export" || `${dirname(ttl_file)}/${basename(ttl_file)}.json`;

ttl_file 
|> parseTtl(lazy = FALSE)
|> to_ttlobject()
|> json_encode()
|> writeLines(con = json_export)
;