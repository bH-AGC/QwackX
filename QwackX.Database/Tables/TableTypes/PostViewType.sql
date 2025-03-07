DROP TYPE IF EXISTS [AppUserSchema].PostViewType;
GO
CREATE TYPE [AppUserSchema].PostViewType AS TABLE
(
    PostId INT,
    UserId INT,
    ViewedAt DATETIME
);
