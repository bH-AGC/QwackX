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