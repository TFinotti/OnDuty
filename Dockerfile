COPY COMP231-W2020-OnDuty-Web/bin/Release/netcoreapp2.1/publish/ app/

ENTRYPOINT ["dotnet", "COMP231-W2020-OnDuty-Web/COMP231_W2020_OnDuty_Web.dll"]