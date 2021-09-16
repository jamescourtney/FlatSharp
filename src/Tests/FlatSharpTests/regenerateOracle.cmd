
rmdir /S /Q .\Generated\FlatSharpTests
..\..\ext\flatc\windows\flatc.exe --csharp --gen-object-api -o .\OracleTests\Generated\ .\Tests.fbs
..\..\ext\flatc\windows\flatc.exe -b -o . .\Tests.fbs