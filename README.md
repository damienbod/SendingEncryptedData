# Sending Encrypted Data

## Migrations Razor page APP
```
Add-Migration "init_sts" -c ApplicationDbContext  
```

```
Update-Database -c ApplicationDbContext
```

# Links

https://docs.microsoft.com/en-us/dotnet/standard/security/decrypting-data

https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview

https://edi.wang/post/2019/1/15/caveats-in-aspnet-core-data-protection

https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.protecteddata.unprotect

https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-use-data-protection

https://edi.wang/post/2019/1/15/caveats-in-aspnet-core-data-protection