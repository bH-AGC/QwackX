CREATE TABLE [dbo].[Replies] (
    [Id] INT NOT NULL IDENTITY,
    [PostId] INT NOT NULL,
    [UserId] INT NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [IsDeleted] BIT DEFAULT 0,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    CONSTRAINT [PK_Replies] PRIMARY KEY ([Id]),
    FOREIGN KEY ([PostId]) REFERENCES [dbo].[Posts]([Id]) ON DELETE CASCADE,
    FOREIGN KEY ([UserId]) REFERENCES [AppUserSchema].[Users]([Id]) ON DELETE NO ACTION
);