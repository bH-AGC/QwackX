CREATE ROLE [AppUserRole]
GO

GRANT EXECUTE ON SCHEMA::[AppUserSchema] TO [AppUserRole]
--(REVOKE|GRANT|DENY) <Action> ON <Objet> TO <À qui> (ROLE|USER)