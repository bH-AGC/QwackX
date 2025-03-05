CREATE TABLE [dbo].[Posts] (
    [Id] INT NOT NULL IDENTITY,
    [UserId] INT NOT NULL,
    [Title] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [IsDeleted] BIT DEFAULT 0,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [ViewCount] INT DEFAULT 0,
    CONSTRAINT [PK_Posts] PRIMARY KEY ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE
);