CREATE OR ALTER PROC spOTP_InsertUpdate
	@UserID BIGINT,
	@OTP NVARCHAR(100)
AS
BEGIN
	
	DECLARE @ErrMsg VARCHAR(5000)
	DECLARE @OtpCreatedOn DATETIME

	-- check if userID exists
	IF NOT EXISTS(
		SELECT 1
		FROM tblUser u
		WHERE u.UserID = @UserID
	)
	BEGIN
		SELECT @ErrMsg = 'Invalid User'  
		GOTO ErrHandler  	
	END;
	
	-- check if OTP is not present by this userID then store it
	IF NOT EXISTS(
		SELECT 1
		FROM tblUserVerification uv
		WHERE uv.UserID = @UserID
	)
	BEGIN
		INSERT INTO tblUserVerification(UserID, OTP, CreatedOn)
		VALUES(
			@UserID,
			@OTP,
			SYSDATETIME()
		)
	END;

	-- check if OTP is expired update the OTP (since User has resend the code)

	-- Get CreatedOn
		SELECT @OtpCreatedOn = uv.CreatedOn
		FROM tblUserVerification uv
		WHERE uv.UserID = @UserID;

		-- Check expiry (example: 10 minutes)
		IF DATEDIFF(MINUTE, @OtpCreatedOn, SYSDATETIME()) > 10
		BEGIN
			UPDATE tblUserVerification
			SET OTP = @OTP,
				CreatedOn = SYSDATETIME()
			WHERE UserID = @UserID;
		END


END
ERRHANDLER:   
BEGIN  
 RAISERROR(@ErrMsg, 16, 1, 0)  
 RETURN  
END