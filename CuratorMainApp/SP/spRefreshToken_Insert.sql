CREATE OR ALTER PROC spRefreshToken_Insert (
	@UserID udt_ID,
	@RefreshToken NVARCHAR(500),
	@RefreshTokenExpiry DATETIME
)
AS
BEGIN
	UPDATE tblUser
	SET
		RefreshToken = @RefreshToken,
		RefreshTokenExpiry = @RefreshTokenExpiry
	WHERE UserID = @UserID;
END