@echo off

git pull gitlink HEAD
git push gitlink HEAD

git pull local HEAD
REM git pull gitee HEAD

git push local HEAD
REM git push gitee HEAD

echo synchronization of this code repository job done!