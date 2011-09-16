@echo off
Coco.exe -namespace "Scl.Grammar" scl.atg

echo cleaning up
del Parser.Generated.cs
del Scanner.Generated.cs

echo renaming generated files
ren Parser.cs Parser.Generated.cs
ren Scanner.cs Scanner.Generated.cs

echo done
