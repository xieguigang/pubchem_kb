@echo off

REM git remote add gitlink https://gitlink.org.cn/xieguigang/pubchem_kb.git
git pull gitlink HEAD
git push gitlink HEAD

echo synchronization of this code repository job done!