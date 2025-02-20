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