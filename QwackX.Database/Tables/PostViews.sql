CREATE TABLE [dbo].[PostViews] (
    PostId INT NOT NULL,
    UserId INT NOT NULL,
    ViewedAt DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (PostId, UserId),
    FOREIGN KEY (PostId) REFERENCES [dbo].[Posts] (Id),
    FOREIGN KEY (UserId) REFERENCES [dbo].[Users] (Id)
);