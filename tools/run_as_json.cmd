@echo off

SET R_ENV="R#"
SET script_r="E:\pubchem_kb\tools\ttlobject_json.R"
SET src="E:\pubchem_kb"


REM run imports via cli

REM CALL %R_ENV% %script_r% --attach %src% --ttl E:\pubchem_kb\test\pc_concept.ttl --what pc_concept 
CALL %R_ENV% %script_r% --attach %src% --ttl E:\pubchem_kb\test\pc_concept.ttl --what pc_concept 