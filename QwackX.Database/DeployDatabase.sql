-- -- docker cp DeployDatabase.sql sqlserver:/home/DeployDatabase.sql
-- -- docker exec -it sqlserver bash
-- -- /opt/mssql-tools18/bin/sqlcmd -?
-- -- /opt/mssql-tools18/bin/sqlcmd -S localhost,1433 -d QwackX -U sa -P 'P#ssw0rd' -i /home/DeployDatabase.sql -C TrustServerCertificate=yes
-- 
-- -- 1. Création du schéma utilisateur
-- :r Security/Schemas/AppUserSchema.sql
-- :r Secutity/Roles/AppUserRole.sql
-- :r Secutity/Roles/PostDeployment.sql
-- 
-- -- 2. Création des tables
-- :r Tables/Users.sql
-- :r Tables/Roles.sql
-- :r Tables/UsersRoles.sql
-- :r Tables/Post.sql
-- :r Tables/Replies.sql
-- :r Tables/Likes.sql
-- 
-- -- 3. Création des procédures stockées
-- :r Procedures/Users/CreateUser.sql
-- :r Procedures/Posts/CreatePost.sql
-- :r Procedures/Posts/DeletePost.sql
-- :r Procedures/Posts/GetPostDetail.sql
-- :r Procedures/Posts/GetPostsTitles.sql
-- :r Procedures/Replies/AddReply.sql
-- :r Procedures/Replies/DeleteReply.sql
-- :r Procedures/Replies/GetPostReplies.sql
-- :r Procedures/Likes/LikePost.sql

CREATE SCHEMA AppUserSchema;
GO

CREATE ROLE [AppUserRole]
GO

GRANT EXECUTE ON SCHEMA::[AppUserSchema] TO [AppUserRole]
GO

IF NOT EXISTS (SELECT * FROM sys.syslogins WHERE [name] = N'QwackXApiLogin')
    BEGIN
        CREATE LOGIN QwackXApiLogin WITH PASSWORD=N'P@ssw0rd!'
    END
GO

IF NOT EXISTS (SELECT * FROM sys.sysusers WHERE [name] = N'QwackXApiUser')
    BEGIN
        CREATE USER QwackXApiUser FOR LOGIN QwackXApiLogin;
        ALTER ROLE AppUserRole ADD MEMBER QwackXApiUser;
    END
GO

ALTER USER QwackXApiUser WITH DEFAULT_SCHEMA = AppUserSchema;
GO

create type PostViewType as table
(
    PostId   int,
    UserId   int,
    ViewedAt datetime
)
go

create table Users
(
    Id           int identity
        constraint PK_Users
            primary key,
    Username     nvarchar(50)  not null
        unique,
    Email        nvarchar(255) not null
        unique,
    PasswordHash nvarchar(255) not null,
    CreatedAt    datetime default getdate()
)
go

create table Likes
(
    Id         int identity
        constraint PK_Likes
            primary key,
    EntityType varchar(10)        not null
        check ([EntityType] = 'Reply' OR [EntityType] = 'Post'),
    EntityId   int                not null,
    UserId     int                not null
        references Users,
    IsDisliked bit      default 0 not null,
    CreatedAt  datetime default getdate(),
    unique (EntityType, EntityId, UserId)
)
go

create table Posts
(
    Id          int identity
        constraint PK_Posts
            primary key,
    UserId      int           not null
        references Users
            on delete cascade,
    Title       nvarchar(255) not null,
    Description nvarchar(max) not null,
    IsDeleted   bit      default 0,
    CreatedAt   datetime default getdate(),
    ViewCount   int      default 0
)
go

create table PostViews
(
    PostId   int not null
        references Posts,
    UserId   int not null
        references Users,
    ViewedAt datetime default getdate(),
    primary key (PostId, UserId)
)
go

create table Replies
(
    Id        int identity
        constraint PK_Replies
            primary key,
    PostId    int           not null
        references Posts
            on delete cascade,
    UserId    int           not null
        references Users,
    Content   nvarchar(max) not null,
    IsDeleted bit      default 0,
    CreatedAt datetime default getdate()
)
go

CREATE FUNCTION [dbo].[CreatePasswd]
(
    @Passwd NVARCHAR(20)
)
    RETURNS BINARY(64)
AS
BEGIN
    DECLARE @PreSalt NCHAR(2048) = N'peALmr3S@BuYyKc-^E_enw*KVTFfU8D+7XQaTL2S-6?pryv&f#wPg9^u=avQP2qE+=bepTUqfn-ZmWUkbursn4JzfF#9J6s#AxgSE&Nz+S6rYm?Z_s5W4?zkJ3tf^Q&pu_@8J@n%p_#4kQE424Lp=qN!#FMe9MamfMEruF=$*kMK&J^jMzjD*NV2v*_Z5q^__MKkw%daVpBK^&X?**5LCy5%C_N95vZuX_2qgj5pxWHtsJqg!BT6+RKkkZBmSFX7zDhahFkjr^whp?W6Nr-vW_NdHeKdH4!u!VLS_H$ce*?=+x4r3bac+xe#Cd2G&KR6daCXw4V5APNb2MdFXj*2jAH5Q$yFrg=rXTY_Nb=te5Fyt$^E4gNwdvz6na64@MBz$pbnVFB6DPgWCsN^7!GkPZA4C8*-JsN+z&CTYWtLE^nJu=3dk!8Rt#Jz9RNzyUVMkd7KA&r@z6?KUTa!HWagSv_CqCSc^Th@H=LzwV58@HE8p5!DV6e4zKr45B-!gDhfZXM4GeGZnBJRbM3RH^ekjqSrQ5&BA3crz23epVGwNAG=Y2Rpng%aUtXcnSJUTq%7MkThY^&g5j%*Z4f&3=-E&657HcfX6bTW6h@KM+mCR^5%Y*tFV8Ha-@j7*6yA^=rZ3!=%dyx#sYrLZdW@PqanN=r4S4zE$MSNT+tYMPtR?dqy4WSk&uAhU_%6JeqnAq?SAd%TM_a%G68wj^t-*dH8XJ_GGLEyypqQ@@RafdgmWPTpk2L2y&Y9=$MkzWFF5b!JVY7z_X^7xqDC-Myf%AvYnj$B9h3HE_sY$rD^+MJjZz7BVbxeGpg=nPQauLMEwNA&k&dD=W-9SG9M$p%3d!YA&p-z3TUm5&9azjQ$aFT3uB3K^9vD2W9Y?J?4rwWh-qPv5-Yp!8z!fVxQ4nYK=#Y-V&P9JSB6@EKzQ849Vww&r=dJE=3BphkXGnr3KbPzRc+TS@QhD4cZgD*#7fxh7mZMtbrR8AZ+a=&j5U8+M3qqM3$#*LLHdDHyEP=&XyP7e^+^q3-a%WtqW9_HU$2Hg5kSrKySHNv-a9+F85v2rfSqG%WuC!fA-HzD$_%Hzma?m&!7WaMXsgqtF35UVr%9@mg7cCNPccu4K3?6b&K3r$pEY+XTQN8Tn6+D_HxBBB2=7ns2Yfcq3WBbP=@tQse6WW3N8T4w-W9YD?HAsLt!eZj#$B9!Nb%su2z-UxVej+Rwq3@+b6ERNuTeJNmCX4^CSp%nzGH^&P_@r$+mLDJhT3-R$WtsA%U&fjS78yX3Lwm_BgBQDv83t#L-F_6M^pEN4Nh=@Qpj2!!sSNzJvZGk8v5VHVzFgcL-KM!hnYYjJYpmg!E*K^PRSgSs#q%rFFfNF5Kux?zeVAJLw9fEwLpE73Hkcq526nkM&u5aNH@WDqnX-cyDXadVxjCv4e=9q_2@Ech$kHcCbd=+BzWd=X&JTquj+h2NndA#T%B$j*rjhVh#j$Eqyn-jkrSgVEFRV!K=%h=$mvG8Df9GJc3t3UfYFKGANGpBmYnGysgk9yBD4$wj2NwaS$&K%q4bu_DcBTPd_8JLa--=ypc9M8B7KwweGXH#5E?-BkFD26*Knq6q-D?HP8bD4mRe$pLV!R624K8&hkB6MuX$b3%uFFmCFeuC&wxRn7ss99aaEJSmVwv%hy&#CeErj59@9jvUTh8GYpf+z!FHWq8F@k*u3*6SMSV&35M$SKNBXrA3f4&u*K76d#uQ&qM_Fny!Tu8_XSER?2fL^+n%R_xgF5+k?gcy6jXcDw?K+b+y?Ena#XyWHVUn9pf$UcEJ2C4#j8BhVPLxEUa&Bn22s9bVYN65Be!_t_9ju&C6P6%RMv!g9k9gJKc6wVR%$h-^d%bM%&amGGY4BGY&ber+%b*MQTX_G-du592fpaVrx!W3sqUE93k@Dt@GFZKdKVH!ybR2bRBHFgap=u8BRPPAJng$NE8kG@xyRmqPeJk^Fm6***yrjXtJmmpFVhRTf#U%xRhFR64z+qsRak&V*UUKD43%fB7m5ud88^n*wnK=$N3Cr^G$XJFA*Ju^&JgfW+=#';
    DECLARE @PostSalt NCHAR(2048) = N'*e-mE&Ey7Fz=w^cTujjjDc3sDV*7B@2xN3UtVGmxrXt2CdvBkLPrj3=9Ws&#ms_4js!+ADzE%5QKTm8*$wBfu%ftPW@GZk*zk&A4g?J8GdNKa@CZ7v=E9XW!?sx@_Nb&Qn?EDkXt_8&LAaL_GuXVYb%r3tgxPkj9A3*X?C-_sgL4#uHhJT-x5%X?WpusDkbYnFBU=@dEzL6Xr%--rxLQBg3nZBYQAYVfFqcxnN26e@7c&RKTu8xVuVgkvs@#*X^F!JmFp4YUq-tWqCFwjky&VPj2yw3eJd7?pqVDK+3kBjq65jrD^=%z*FV^aJR@QYdwQXCS%pByUcc4bbRRq4^ZaV9x9%t+DN^!E&V4!zGuA5qQfqe^v7TPL%&H4$SGPFhDYXHruaD=9Cgydqp8a+3AZ#=hwc#C?+LYGL%EM3W#cNH%LK*vLRUBgNC6f?sCAb=BcgVC4?xk9D#3ks&RByTLDQq5h+q4*M=yhBsB92&QtVUgv#t&D9!sYh%RQK6+Rq4@TEk_Qaw8#9t-aYRg?yx6S8DkHALhk8?ePrE8y=*FLBaWb+qSVa4jTm_@*M=L-MpUheB2P%ZQ9Q_8c=#4WW2cyNFTxXfuwkQfb_ULrGff=n+=@4mMuj?S+&DVV-a?Cg#%GB!wHP*u6#%PspDKs+HnUrKxnkEnauzEsc-FAN*DT@57SrsVUjqSWq%vtNXhRQM6Vg2_?cP59J%SPmWv=tTFwWcA==C*9se@JjQ?AEnmhEzCphQzW7_zk-@b$XT45Yuyfz$4Sbzd6*N-Ch!Z#aqZ5j6t+75?s$7ZS!?-E6!kQ+d5ggZg^pGBRt7RvJ2$B7qCB@RrNbsaMn+X-Dd_b3ZL#CYMxBTteUd?EWJ+FaMKRbwp-unBG*xFvRwBwwqrfE69*zQsegZMwUePJT3E7X?B_2C+W-yTT2md7vgsWAMP36gVQ%@$AX3dP=HAzQfGm!szZRWH2KPBL+tbkTN?GuC+$dd=^s?Xk!&Qvj#+$dHPRT#9GZ2p&H-wK@WngNd?9qTv#@rDFEvap?dXjeCuxqV^vFcW2_PaK-pqYk&WPVrA-P?gS6qrwq^EtrY3g@+Kfx*Vj=dcd-T-mzFVv%TzX+st5Pxp*@V#3mjK^=FYbvxx$KFVk7ke5F7GqD9MZhLmaALHZ?3#5pVZpqt-b-zNz$%&_G_y=fh_jSQ*#7By+6?t8pKC&mhM_LPgUZn^%c%N!qWq%UUebwHn7^4%55=mF!SnvvRzc*&6zuFLYKD9FuV?Hnc-P65g!jNcYPmWsA+sjsja36&@=BwE3vSwvRdxsB$-f5PhkJB6ZxMJ7KqZg6jAEd3YVDN278aCtqyH6KCuKwzkStRc-BS%$58*GYKS-xqhN6*Mv+=$EYqtbMqw#HCJ2sN+w!mX$e8Ksj#Y-UPtaG=#3L3h?cn4s5J?w@ZhwGaNFyAE^PwmDN^erxBvLSQ-tUq69LG^cfVs&gM%kCqrc#LYFxXamAf3$^$D!*YS+6DpeGhZqJ*BE*m4E5?w9N$g_Zg4m=!8EuMJfVh%xsBn#m6ZyEMF%ChT%7dp4Wm&d!DV5eL35?vQ==DFzD=^*3hJ3gK6JqDF=y6N3HLDp%kHf&JspV3hcQ&$=?r5_9u*_-!e&H2aV=R7=bJUPvXeh7@P&eYD@es+P53sH+yfxV-h+rm_WfJGxxv*zDWh=yGCrmj-+xYNEQJV4nK$dU6Kwp$kA9wmc#jtBp47Q*Nf%cxeUPq$U6cHS7ze%M_g*4$*#_DKT8^jJ2Y-nz*EU4r@n$Esg!NL6X$$&=ZLvDZw=9JPMUZ-N+ZD34ZKHE2wpPX$6xR%mxZ6@M_mVXS$+e5pnP=h-L5vp#cb5JAMRhtATc942?KQ?DXw*_L#WUSEWHzrHVuKfBcSrf%wWk#MtutAk6Q-hrwcnqjeR+ZK-8Y2@?PVAAxMGZ+NjP5NSa&jnHGNqTVeG2!GV@*ZKNQqnzwC&!tRJmsWJu&%^5#tdWnMcyZZuVFgNLyvD39cYyTqa6muB29B_Df@?5Dp=Ezef$J57-&PT_S8rj&WU5S@gYztqF3FeMhB';

    DECLARE @x INT = 0;
    DECLARE @Cypher BINARY(64) = HASHBYTES('SHA2_512', CONCAT(@PreSalt, @Passwd, @PostSalt));

    WHILE @x < 4
        BEGIN
            SET @Cypher = HASHBYTES('SHA2_512', CONCAT(@PreSalt, @Cypher, @PostSalt));
            SET @x = @x + 1;
        END

    RETURN @Cypher;
END
go

CREATE PROCEDURE [AppUserSchema].[CreatePost]
    @UserId INT,
    @Title NVARCHAR(255),
    @Description NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO [dbo].[Posts] ([UserId], [Title], [Description])
    VALUES (@UserId, @Title, @Description);
END;
go

CREATE PROCEDURE [AppUserSchema].[CreateReply]
    @PostId INT,
    @UserId INT,
    @Content NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO [dbo].[Replies] ([PostId], [UserId], [Content])
    VALUES (@PostId, @UserId, @Content);
END;
go

CREATE PROCEDURE [AppUserSchema].[CreateUser]
    @Username NVARCHAR(50),
    @Email NVARCHAR(255),
    @Password NVARCHAR(20)
AS
BEGIN
    BEGIN TRY
        IF LEN(TRIM(@Username)) = 0
            BEGIN
                RAISERROR (N'Invalid value in @Nom', 16, 1);
                RETURN;
            END

        INSERT INTO [dbo].[Users] (Username, Email, PasswordHash)
        VALUES (@Username, @Email, [dbo].CreatePasswd(@Password));
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN
    END CATCH
END;
go

CREATE PROCEDURE [AppUserSchema].[DeletePost]
@PostId INT
AS
BEGIN
    UPDATE [dbo].[Posts]
    SET [IsDeleted] = 1
    WHERE [Id] = @PostId;
END;
go

CREATE PROCEDURE [AppUserSchema].[DeleteReply]
@ReplyId INT
AS
BEGIN
    UPDATE [dbo].[Replies]
    SET [IsDeleted] = 1
    WHERE [Id] = @ReplyId;
END;
go

CREATE PROCEDURE [AppUserSchema].[DeleteUser]
@UserId INT
AS
BEGIN
    DELETE FROM [dbo].[Users]
    WHERE [Id] = @UserId;
END;
go

CREATE PROCEDURE [AppUserSchema].[DetailPost]
    @PostId INT,
    @UserId INT
AS
BEGIN
    SELECT
        p.[Id],
        p.[Title],
        p.[Description],
        p.[CreatedAt],
        u.[Username],
        p.[UserId],
        (SELECT COUNT(*)
         FROM [dbo].[Likes]
         WHERE [EntityType] = 'Post'
           AND [EntityId] = p.[Id]
           AND [IsDisliked] = 0) AS [LikeCount],
        CASE
            WHEN EXISTS (SELECT 1
                         FROM [dbo].[Likes]
                         WHERE [EntityType] = 'Post'
                           AND [EntityId] = p.[Id]
                           AND [UserId] = @UserId
                           AND [IsDisliked] = 0)
                THEN CONVERT(BIT, 1)
            ELSE CONVERT(BIT, 0)
            END AS [IsLiked],
        p.[ViewCount],
        CASE
            WHEN EXISTS (SELECT 1
                         FROM [dbo].[PostViews]
                         WHERE [PostId] = p.[Id]
                           AND [UserId] = @UserId)
                THEN 1
            ELSE 0
            END AS [HasViewed]
    FROM [dbo].[Posts] p
             JOIN [dbo].[Users] u ON p.[UserId] = u.[Id]
    WHERE p.[Id] = @PostId AND p.[IsDeleted] = 0;
END;
go

CREATE PROCEDURE [AppUserSchema].[DetailUser]
@UserId INT
AS
BEGIN
    SELECT [Id], [Username], [Email], [PasswordHash], [CreatedAt]
    FROM [dbo].[Users]
    WHERE [Id] = @UserId;
END;
go

CREATE PROCEDURE [AppUserSchema].[EditUser]
    @UserId  INT,
    @Username NVARCHAR(50),
    @Email NVARCHAR(255),
    @Password NVARCHAR(255)
AS
BEGIN
    UPDATE [dbo].[Users]
    SET
        [Username] = @Username,
        [Email] = @Email,
        [PasswordHash] = [dbo].CreatePasswd(@Password)
    WHERE Id = @UserId;
END
go

CREATE PROCEDURE [AppUserSchema].[LikePost]
    @UserId INT,
    @EntityId INT,
    @EntityType NVARCHAR(50)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Likes] WHERE [EntityType] = @EntityType AND [EntityId] = @EntityId AND [UserId] = @UserId)
        BEGIN
            INSERT INTO [dbo].[Likes] ([EntityType], [EntityId], [UserId])
            VALUES (@EntityType, @EntityId, @UserId);
        END
    ELSE
        BEGIN
            UPDATE [dbo].[Likes]
            SET [IsDisliked] = CASE WHEN [IsDisliked] = 1 THEN 0 ELSE 1 END
            WHERE [EntityType] = @EntityType AND [EntityId] = @EntityId AND [UserId] = @UserId;
        END
END;
go

CREATE PROCEDURE [AppUserSchema].[LikeReply]
    @UserId INT,
    @EntityId INT,
    @EntityType NVARCHAR(50)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Likes] WHERE [EntityType] = @EntityType AND [EntityId] = @EntityId AND [UserId] = @UserId)
        BEGIN
            INSERT INTO [dbo].[Likes] ([EntityType], [EntityId], [UserId])
            VALUES (@EntityType, @EntityId, @UserId);
        END
    ELSE
        BEGIN
            UPDATE [dbo].[Likes]
            SET [IsDisliked] = CASE WHEN [IsDisliked] = 1 THEN 0 ELSE 1 END
            WHERE [EntityType] = @EntityType AND [EntityId] = @EntityId AND [UserId] = @UserId;
        END
END;
go

CREATE PROCEDURE [AppUserSchema].[ListPostReplies]
    @PostId INT,
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT r.[Id], r.[Content], r.[CreatedAt], u.[Username],
           (SELECT COUNT(*) FROM [dbo].[Likes] WHERE [EntityType] = 'Reply' AND [EntityId] = r.[Id] AND [IsDisliked] = 0) AS [LikeCount],
           CASE
               WHEN EXISTS (SELECT 1 FROM [dbo].[Likes] WHERE [EntityType] = 'Reply' AND [EntityId] = r.[Id] AND [UserId] = @UserId AND [IsDisliked] = 0)
                   THEN CONVERT(BIT, 1)
               ELSE CONVERT(BIT, 0)
               END AS [IsLiked]
    FROM [dbo].[Replies] r
             JOIN [dbo].[Users] u ON r.[UserId] = u.[Id]
    WHERE r.[PostId] = @PostId AND r.[IsDeleted] = 0;
END;
go

CREATE PROCEDURE [AppUserSchema].[ListPostsTitles]
@UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.[Id], p.[Title], p.[CreatedAt], u.[Username], p.[UserId], p.[ViewCount],
           (SELECT COUNT(*) FROM [dbo].[Likes] WHERE [EntityType] = 'Post' AND [EntityId] = p.[Id] AND [IsDisliked] = 0) AS [LikeCount],
           (SELECT COUNT(*) FROM [dbo].[Replies] WHERE [PostId] = p.[Id] AND [IsDeleted] = 0) AS [ReplyCount],
           CASE
               WHEN EXISTS (SELECT 1 FROM [dbo].[Likes] WHERE [EntityType] = 'Post' AND [EntityId] = p.[Id] AND [UserId] = @UserId AND [IsDisliked] = 0)
                   THEN 1
               ELSE 0
               END AS [IsLiked]
    FROM [dbo].[Posts] p
             JOIN [dbo].[Users] u ON p.[UserId] = u.[Id]
    WHERE p.[IsDeleted] = 0
    ORDER BY p.[CreatedAt] DESC;
END;
go

CREATE PROCEDURE AppUserSchema.ListUsers
AS
BEGIN
    SELECT Id, Username, Email, PasswordHash, CreatedAt
    FROM [dbo].[Users];
END;
go

CREATE PROCEDURE [AppUserSchema].[LoginUser]
    @Email NVARCHAR(255),
    @Password NVARCHAR(20)
AS
BEGIN
    SELECT Id, Username, Email, PasswordHash, CreatedAt
    FROM [dbo].[Users]
    WHERE Email = @Email AND PasswordHash = dbo.CreatePasswd(@Password);
END;
go

CREATE PROCEDURE [AppUserSchema].[PostsViewsBulkInsert]
    @PostViews [AppUserSchema].PostViewType READONLY,
    @RowsAffected INT OUTPUT -- Paramètre de sortie
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    BEGIN TRY
        SET @RowsAffected = 0;

        IF EXISTS (
            SELECT 1 FROM @PostViews pv
                              LEFT JOIN dbo.Posts p ON p.Id = pv.PostId
            WHERE p.Id IS NULL
        )
            BEGIN
                SELECT pv.PostId
                FROM @PostViews pv
                         LEFT JOIN dbo.Posts p ON p.Id = pv.PostId
                WHERE p.Id IS NULL;

                THROW 50000, 'Un ou plusieurs PostId n''existent pas dans la table dbo.Posts.', 1;
            END

        MERGE INTO [dbo].[PostViews] AS target
        USING (SELECT DISTINCT PostId, UserId, ViewedAt FROM @PostViews) AS source
        ON target.PostId = source.PostId AND target.UserId = source.UserId
        WHEN MATCHED THEN
            UPDATE SET target.ViewedAt = source.ViewedAt
        WHEN NOT MATCHED THEN
            INSERT (PostId, UserId, ViewedAt)
            VALUES (source.PostId, source.UserId, source.ViewedAt);

        UPDATE p
        SET p.ViewCount = p.ViewCount + 1
        FROM [dbo].[Posts] p
                 INNER JOIN @PostViews pv ON p.Id = pv.PostId;

        SET @RowsAffected = @@ROWCOUNT;
    END TRY
    BEGIN CATCH
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        PRINT '❌ Erreur lors de l''insertion : ' + @ErrorMessage;
        
        THROW;
    END CATCH
END;
go

CREATE PROCEDURE [AppUserSchema].[RegisterUser]
    @Username NVARCHAR(50),
    @Email NVARCHAR(255),
    @Password NVARCHAR(20)
AS
BEGIN
    BEGIN TRY
        IF LEN(TRIM(@Username)) = 0
            BEGIN
                RAISERROR (N'Invalid value in @Username', 16, 1);
                RETURN;
            END

        IF LEN(TRIM(@Email)) = 0 OR CHARINDEX('@', @Email) = 0
            BEGIN
                RAISERROR (N'Invalid value in @Email', 16, 1);
                RETURN;
            END

        IF LEN(@Password) < 6
            BEGIN
                RAISERROR (N'Password must be at least 6 characters long', 16, 1);
                RETURN;
            END

        INSERT INTO [dbo].[Users] (Username, Email, PasswordHash)
        VALUES (@Username, @Email, dbo.CreatePasswd(@Password));

        SELECT Id, Username, Email, CreatedAt
        FROM [dbo].[Users]
        WHERE Email = @Email;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH
END;
go

