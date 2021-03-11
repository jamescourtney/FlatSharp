
rmdir /S /Q .\Generated\FlatSharpTests
..\Google.FlatBuffers\flatc --csharp --gen-object-api -o .\OracleTests\Generated\ .\Tests.fbs