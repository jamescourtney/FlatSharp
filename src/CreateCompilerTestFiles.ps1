dotnet build FlatSharp.Compiler/FlatSharp.Compiler.csproj -c Release -f net8.0

# Native AOT stuff
$fbs = (gci -r Tests/CompileTests/NativeAot/*.fbs) -join ";"
dotnet FlatSharp.Compiler/bin/Release/net8.0/FlatSharp.Compiler.dll --nullable-warnings false --normalize-field-names true --input "$fbs" -o Tests/CompileTests/NativeAot
