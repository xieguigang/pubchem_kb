require(pubchem);

const dir_src as string = ?"--dir" || stop("No directory source folder was provided!");
const as.table = function(file) {
    let t0 = now();
    let data = get_hash_tupleData(file, lazy = FALSE);
    let save = `${dirname(file)}/${basename(file)}.csv`;

    print(` -> ${save} [${time_span(now() - t0)}]`);
    write.csv(data, file = save);
}

for(file in list.files(dir_src, pattern = "*.ttl")) {
    as.table(file);
}