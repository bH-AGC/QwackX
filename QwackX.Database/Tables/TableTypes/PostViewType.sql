DROP TYPE IF EXISTS [dbo].PostViewType;
GO
CREATE TYPE [dbo].PostViewType AS TABLE
(
    PostId INT,
    UserId INT,
    ViewedAt DATETIME
);
