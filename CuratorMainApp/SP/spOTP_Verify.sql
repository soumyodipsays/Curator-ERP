CREATE OR ALTER PROC spOTP_Verify
    @UserID BIGINT,
    @OTP NVARCHAR(100)
AS
BEGIN

    DECLARE @ErrMsg VARCHAR(5000)
    DECLARE @DbOTP NVARCHAR(100)
    DECLARE @ExpiresOn DATETIME
    DECLARE @Attempts INT
    DECLARE @IsVerified BIT

    -- check if user exists
    IF NOT EXISTS(
        SELECT 1
        FROM tblUser u
        WHERE u.UserID = @UserID
    )
    BEGIN
        SELECT @ErrMsg = 'Invalid User'
        GOTO ErrHandler
    END;

    -- check if OTP record exists
    IF NOT EXISTS(
        SELECT 1
        FROM tblUserVerification uv
        WHERE uv.UserID = @UserID
    )
    BEGIN
        SELECT @ErrMsg = 'OTP not found'
        GOTO ErrHandler
    END;

    -- fetch data
    SELECT 
        @DbOTP = uv.OTP,
        @ExpiresOn = uv.ExpiresOn,
        @Attempts = uv.Attempts,
        @IsVerified = uv.IsVerified
    FROM tblUserVerification uv
    WHERE uv.UserID = @UserID;

    -- already used OTP
    IF @IsVerified = 1
    BEGIN
        SELECT @ErrMsg = 'OTP already used'
        GOTO ErrHandler
    END;

    -- check expiry
    IF SYSDATETIME() > @ExpiresOn
    BEGIN
        SELECT @ErrMsg = 'OTP expired'
        GOTO ErrHandler
    END;

    -- check attempts (max 3)
    IF @Attempts >= 3
    BEGIN
        SELECT @ErrMsg = 'Too many attempts. Please request a new OTP'
        GOTO ErrHandler
    END;

    -- validate OTP
    IF @DbOTP <> @OTP
    BEGIN
        UPDATE tblUserVerification
        SET Attempts = Attempts + 1
        WHERE UserID = @UserID;

        SELECT @ErrMsg = 'Invalid OTP'
        GOTO ErrHandler
    END;

    -- success : mark verified
    UPDATE tblUserVerification
    SET 
        IsVerified = 1
    WHERE UserID = @UserID;

    SELECT 
        1 AS Success,
        'OTP verified successfully' AS Message

    RETURN

END

ERRHANDLER:
BEGIN
    RAISERROR(@ErrMsg, 16, 1, 0)
    RETURN
END