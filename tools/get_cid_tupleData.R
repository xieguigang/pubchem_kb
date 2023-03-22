require(pubchem);

const dir_src as string = ?"--dir" || stop("No directory source folder was provided!");
const as.table = function(file) {
    let data = get_cid_tupleData(file, lazy = FALSE);
    let save = `${dirname(file)}/${basename(file)}.csv`;

    write.csv(data, file = save);
}

for(file in list.files(dir_src, pattern = "*.ttl")) {
    as.table(file);
}