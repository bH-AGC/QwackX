-- docker cp DeployDatabase.sql sqlserver:/home/DeployDatabase.sql
-- docker exec -it sqlserver bash
-- /opt/mssql-tools18/bin/sqlcmd -?
-- /opt/mssql-tools18/bin/sqlcmd -S localhost,1433 -d QwackX -U sa -P 'P#ssw0rd' -i /home/DeployDatabase.sql -C TrustServerCertificate=yes

-- 1. Création du schéma utilisateur
:r Security/Schemas/AppUserSchema.sql
:r Secutity/Roles/AppUserRole.sql

-- 2. Création des tables
:r Tables/Users.sql
:r Tables/Roles.sql
:r Tables/UsersRoles.sql
:r Tables/Post.sql
:r Tables/Replies.sql
:r Tables/Likes.sql

-- 3. Création des procédures stockées
:r Procedures/Users/CreateUser.sql
:r Procedures/Posts/CreatePost.sql
:r Procedures/Posts/DeletePost.sql
:r Procedures/Posts/GetPostDetail.sql
:r Procedures/Posts/GetPostsTitles.sql
:r Procedures/Replies/AddReply.sql
:r Procedures/Replies/DeleteReply.sql
:r Procedures/Replies/GetPostReplies.sql
:r Procedures/Likes/LikePost.sql


-- Création du schéma utilisateur
CREATE SCHEMA [AppUserSchema];
GO

-- Création des tables
-- Table [Users]
CREATE TABLE [AppUserSchema].[Users] (
                                         [Id] INT NOT NULL IDENTITY,
                                         [Username] NVARCHAR(50) UNIQUE NOT NULL,
                                         [Email] NVARCHAR(255) UNIQUE NOT NULL,
                                         [PasswordHash] NVARCHAR(255) NOT NULL,
                                         [CreatedAt] DATETIME DEFAULT GETDATE(),
                                         CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

-- Table [Roles]
CREATE TABLE [AppUserSchema].[Roles] (
                                         [Id] INT NOT NULL IDENTITY,
                                         [Name] NVARCHAR(50) UNIQUE NOT NULL,
                                         CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);
GO

-- Table [UserRoles]
CREATE TABLE [AppUserSchema].[UserRoles] (
                                             [UserId] INT NOT NULL,
                                             [RoleId] INT NOT NULL,
                                             CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
                                             FOREIGN KEY ([UserId]) REFERENCES [AppUserSchema].[Users]([Id]) ON DELETE CASCADE,
                                             FOREIGN KEY ([RoleId]) REFERENCES [AppUserSchema].[Roles]([Id]) ON DELETE CASCADE
);
GO

-- Table [Posts]
CREATE TABLE [dbo].[Posts] (
                               [Id] INT NOT NULL IDENTITY,
                               [UserId] INT NOT NULL,
                               [Title] NVARCHAR(255) NOT NULL,
                               [Description] NVARCHAR(MAX) NOT NULL,
                               [IsDeleted] BIT DEFAULT 0, -- Suppression logique
                               [CreatedAt] DATETIME DEFAULT GETDATE(),
                               CONSTRAINT [PK_Posts] PRIMARY KEY ([Id]),
                               FOREIGN KEY ([UserId]) REFERENCES [AppUserSchema].[Users]([Id]) ON DELETE CASCADE
);
GO

-- Table [Replies]
CREATE TABLE [dbo].[Replies] (
                                 [Id] INT NOT NULL IDENTITY,
                                 [PostId] INT NOT NULL,
                                 [UserId] INT NOT NULL,
                                 [Content] NVARCHAR(MAX) NOT NULL,
                                 [IsDeleted] BIT DEFAULT 0, -- Suppression logique
                                 [CreatedAt] DATETIME DEFAULT GETDATE(),
                                 CONSTRAINT [PK_Replies] PRIMARY KEY ([Id]),
                                 FOREIGN KEY ([PostId]) REFERENCES [dbo].[Posts]([Id]) ON DELETE CASCADE,
                                 FOREIGN KEY ([UserId]) REFERENCES [AppUserSchema].[Users]([Id]) ON DELETE NO ACTION
);
GO

-- Table [Likes]
CREATE TABLE [dbo].[Likes] (
                               [Id] INT NOT NULL IDENTITY,
                               [PostId] INT NOT NULL,
                               [UserId] INT NOT NULL,
                               [CreatedAt] DATETIME DEFAULT GETDATE(),
                               CONSTRAINT [PK_Likes] PRIMARY KEY ([Id]),
                               FOREIGN KEY ([PostId]) REFERENCES [dbo].[Posts]([Id]) ON DELETE CASCADE,
                               FOREIGN KEY ([UserId]) REFERENCES [AppUserSchema].[Users]([Id]) ON DELETE NO ACTION,
                               UNIQUE ([PostId], [UserId])
);
GO

-- Procédure pour créer un utilisateur
CREATE PROCEDURE [AppUserSchema].[CreateUser]
    @Username NVARCHAR(50),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    INSERT INTO [AppUserSchema].[Users] ([Username], [Email], [PasswordHash])
    VALUES (@Username, @Email, @PasswordHash);
END;
GO

-- Procédure pour ajouter un post
CREATE PROCEDURE [dbo].[CreatePost]
    @UserId INT,
    @Title NVARCHAR(255),
    @Description NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO [dbo].[Posts] ([UserId], [Title], [Description])
    VALUES (@UserId, @Title, @Description);
END;
GO

-- Procédure pour suppression logique d'un post
CREATE PROCEDURE [dbo].[DeletePost]
@PostId INT
AS
BEGIN
    UPDATE [dbo].[Posts]
    SET [IsDeleted] = 1
    WHERE [Id] = @PostId;
END;
GO

-- Procédure pour suppression logique d'une réponse
CREATE PROCEDURE [dbo].[DeleteReply]
@ReplyId INT
AS
BEGIN
    UPDATE [dbo].[Replies]
    SET [IsDeleted] = 1
    WHERE [Id] = @ReplyId;
END;
GO

-- Procédure pour récupérer un post (sans les supprimés)
CREATE PROCEDURE [dbo].[GetPostsTitles]
AS
BEGIN
    SELECT [Id], [Title]
    FROM [dbo].[Posts]
    WHERE [IsDeleted] = 0;
END;
GO


-- Procédure pour liker un post
CREATE PROCEDURE [dbo].[LikePost]
    @PostId INT,
    @UserId INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Likes] WHERE [PostId] = @PostId AND [UserId] = @UserId)
        BEGIN
            INSERT INTO [dbo].[Likes] ([PostId], [UserId])
            VALUES (@PostId, @UserId);
        END
END;
GO

-- Procédure pour récupérer un post avec ses réponses et likes
CREATE PROCEDURE [dbo].[GetPostDetails]
@PostId INT
AS
BEGIN
    -- Détails du post
    SELECT p.[Id], p.[Title], p.[Description], p.[CreatedAt], u.[Username],
           (SELECT COUNT(*)
            FROM [dbo].[Likes] WHERE [PostId] = p.[Id]) AS [LikeCount]
    FROM [dbo].[Posts] p
             JOIN [AppUserSchema].[Users] u ON p.[UserId] = u.[Id]
    WHERE p.[Id] = @PostId AND p.[IsDeleted] = 0;

    -- Réponses du post
    SELECT r.[Id], r.[Content], r.[CreatedAt], u.[Username]
    FROM [dbo].[Replies] r
             JOIN [AppUserSchema].[Users] u ON r.[UserId] = u.[Id]
    WHERE r.[PostId] = @PostId AND r.[IsDeleted] = 0;
END;
GO

-- Procédure pour ajouter une réponse
CREATE PROCEDURE [dbo].[AddReply]
    @PostId INT,
    @UserId INT,
    @Content NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO [dbo].[Replies] ([PostId], [UserId], [Content])
    VALUES (@PostId, @UserId, @Content);
END;
GO

-- Procédure pour récupérer les réponses d'un post spécifique
CREATE PROCEDURE [dbo].[GetPostReplies]
@PostId INT
AS
BEGIN
    SELECT r.[Id], r.[Content], r.[CreatedAt], u.[Username]
    FROM [dbo].[Replies] r
             JOIN [AppUserSchema].[Users] u ON r.[UserId] = u.[Id]
    WHERE r.[PostId] = @PostId AND r.[IsDeleted] = 0;
END;
GO

-- Procédure pour supprimer un utilisateur
CREATE PROCEDURE [AppUserSchema].[DeleteUser]
@UserId INT
AS
BEGIN
    -- Supprimer un utilisateur de la table Users de manière définitive
    DELETE FROM [AppUserSchema].[Users]
    WHERE [Id] = @UserId;
END;
GO
