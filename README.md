# Sending Encrypted Data

## Blogs 

<ul>	
	<li><a href="https://damienbod.com/2020/08/19/symmetric-and-asymmetric-encryption-in-net-core/">Symmetric and Asymmetric Encryption in .NET Core</a></li>
	<li><a href="https://damienbod.com/2020/08/22/encrypting-texts-for-an-identity-in-asp-net-core-razor-pages-using-aes-and-rsa/">Encrypting texts for an Identity in ASP.NET Core Razor Pages using AES and RSA</a></li>
	<li><a href="https://damienbod.com/2020/09/01/using-digital-signatures-to-check-integrity-of-cipher-texts-in-asp-net-core-razor-pages/">Using Digital Signatures to check integrity of cipher texts in ASP.NET Core Razor Pages</a></li>

</ul>

## History

2021-02-28 Updated nuget packages

2020-12-26 Updated to .NET 5

## Migrations Razor page APP
```
Add-Migration "init_sts" -c ApplicationDbContext  
```

```
Update-Database -c ApplicationDbContext
```

# Links

https://docs.microsoft.com/en-us/dotnet/standard/security/encrypting-data

https://docs.microsoft.com/en-us/dotnet/standard/security/decrypting-data

https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview

https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.protecteddata.unprotect

https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-use-data-protection

https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=netcore-3.1

https://docs.microsoft.com/en-us/dotnet/standard/security/cross-platform-cryptography

https://docs.microsoft.com/en-us/dotnet/standard/security/vulnerabilities-cbc-mode

https://edi.wang/post/2019/1/15/caveats-in-aspnet-core-data-protection

https://dev.to/stratiteq/cryptography-with-practical-examples-in-net-core-1mc4

https://www.tpeczek.com/2020/08/supporting-encrypted-content-encoding.html

https://cryptobook.nakov.com/

https://www.meziantou.net/cryptography-in-dotnet.htm
