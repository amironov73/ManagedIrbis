@echo off

del TestDb\TestDb.*
copy SaveDb\TestDb.* TestDb

FirstApp.exe