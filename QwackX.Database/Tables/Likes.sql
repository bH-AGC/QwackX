CREATE TABLE [dbo].[Likes] (
    [Id] INT NOT NULL IDENTITY,
    [EntityType] VARCHAR(10) NOT NULL CHECK ([EntityType] IN ('Post', 'Reply')), -- Enum simulé
    [EntityId] INT NOT NULL,
    [UserId] INT NOT NULL,
    [IsDisliked] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    CONSTRAINT [PK_Likes] PRIMARY KEY ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [AppUserSchema].[Users]([Id]) ON DELETE NO ACTION,
UNIQUE ([EntityType], [EntityId], [UserId])
);