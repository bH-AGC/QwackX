IF NOT EXISTS (SELECT * FROM sys.syslogins WHERE [name] = N'QwackXApiLogin')
    BEGIN
        CREATE LOGIN QwackXApiLogin WITH PASSWORD=N'P@ssw0rd!' -- Login Sql Server
        --CREATE LOGIN [<domainName>|<login_name>] FROM WINDOWS;
        --CREATE LOGIN [TECHNOFUTURTIC\FORMA1900] FROM WINDOWS; -- Login Windows
    END

IF NOT EXISTS (SELECT * FROM sys.sysusers WHERE [name] = N'QwackXApiUser')
    BEGIN
        CREATE USER QwackXApiUser FOR LOGIN QwackXApiLogin;
        ALTER ROLE AppUserRole ADD MEMBER QwackXApiUser;
    END

ALTER USER QwackXApiUser WITH DEFAULT_SCHEMA = AppUserSchema;