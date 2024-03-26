@echo off

SET R_ENV="R#"
SET script_r="E:\pubchem_kb\tools\ttlobject_json.R"
SET src="E:\pubchem_kb"


REM run imports via cli

REM CALL %R_ENV% %script_r% --attach %src% --ttl E:\pubchem_kb\test\pc_concept.ttl --what pc_concept 
REM CALL %R_ENV% %script_r% --attach %src% --ttl "C:\Users\Administrator\Downloads\pc_gene.ttl" --what pc_gene

REM CALL %R_ENV% %script_r% --attach %src% --ttl "C:\Users\Administrator\Downloads\pc_cooccurrence_gene_and_disease_000001.ttl" --what pc_gene_disease
REM CALL %R_ENV% %script_r% --attach %src% --ttl "C:\Users\Administrator\Downloads\pc_cooccurrence_gene_and_disease_000001.ttl" --what pc_gene_disease

CALL %R_ENV% %script_r% --attach %src% --ttl "C:\Users\Administrator\Downloads\pc_pathway.ttl" --what pc_pathway

pause