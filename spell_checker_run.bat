echo off
cls
set gavSrc=spell_checker.cs levenstain.cs
set iModeComp=1
set gavSubsystem=exe
set gavPlatform=anycpu
set sVSPath=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
set sVSPathName=%sVSPath%\csc.exe
set gavIncPathVS="%sVSPath%"
if not "%gavIncPathMy%"=="" (set gavIncPathAll=-I"%gavIncPathMy%")
if not %gavIncPathVS%=="" (set gavIncPathAll=%gavIncPathAll% -I%gavIncPathVS%)
echo Include pathes: %gavIncPathAll%
set gavLibPathVS=%sVSPath%
if not "%gavLibPathVS%"=="" (set gavLibPathAll=/lib:"%gavLibPathVS%")
if not "%gavLibPathMy%"=="" (set gavLibPathAll=%gavLibPathAll% /lib:"%gavLibPathMy%")
echo Lib pathes: %gavLibPathAll%
if not "%gavLibFilesCrt%"=="" (set gavLibFilesAll=%gavLibFilesCrt%)
echo Lib files: %gavLibFilesAll%
if %iModeComp%==0 (
               set gavCompilFlags=%gavCompilFlags% /D:_RELEASE 
               set gavCompMode=/debug-
               set gavOptimize=/optimize+
              )

if %iModeComp%==1 (
               set gavCompilFlags=%gavCompilFlags% /D:_DEBUG 
               set gavCompMode=/debug+
               set gavOptimize=/optimize-
              ) 
if not %gavSubsystem%=="" (set gavLinkSubsys=/t:%gavSubsystem%)
set gavDelExt=*.obj, *.exe, *.log, *.pdb
echo. 
echo Deleting old files: %gavDelExt% ...
echo. 
del %gavDelExt%
echo. 
echo ------------------
echo Compiling start...
echo ------------------
echo. 
echo on
"%sVSPathName%" %gavDefine% %gavCompilFlags% %gavCompMode% %gavOptimize% %gavLinkSubsys% /utf8output /fullpaths /platform:%gavPlatform% %gavLibPathAll% %gavLibFilesAll% %gavSrc%>__vc#_compile.log
@echo off
