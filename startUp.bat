:: msbuild SplendorAI.sln
start Server\bin\Debug\netcoreapp3.1\Server.exe
for %%x in (%*) do start Client\bin\Debug\netcoreapp3.1\Client.exe %%x
