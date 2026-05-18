CREATE OR ALTER PROC spRefreshToken_GetUserByRefreshToken (
	@RefreshToken NVARCHAR(500)
)
AS
BEGIN 
	SELECT 
		[UserID],
		[UserName],
		[UserMobile],
		[UserTypeCode],
		[Email],
		[RefreshToken],
		[RefreshTokenExpiry],
		[isActive]
	FROM tblUser
	WHERE RefreshToken = @RefreshToken;
END
