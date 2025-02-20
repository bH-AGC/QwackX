CREATE TABLE [AppUserSchema].[UserRoles] (
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    FOREIGN KEY ([UserId]) REFERENCES [AppUserSchema].[Users]([Id]) ON DELETE CASCADE,
    FOREIGN KEY ([RoleId]) REFERENCES [AppUserSchema].[Roles]([Id]) ON DELETE CASCADE
);