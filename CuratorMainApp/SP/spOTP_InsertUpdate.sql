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
    
    -- insert if not exists
    IF NOT EXISTS(
        SELECT 1
        FROM tblUserVerification uv
        WHERE uv.UserID = @UserID
    )
    BEGIN
        INSERT INTO tblUserVerification
        (
            UserID,
            OTP,
            CreatedOn,
            IsVerified,
            Attempts,
            ExpiresOn
        )
        VALUES
        (
            @UserID,
            @OTP,
            SYSDATETIME(),
            0,
            0,
            DATEADD(MINUTE, 10, SYSDATETIME())
        )

        RETURN
    END;

    -- get existing OTP time
    SELECT @OtpCreatedOn = uv.CreatedOn
    FROM tblUserVerification uv
    WHERE uv.UserID = @UserID;

    -- if expired → update
    IF DATEDIFF(MINUTE, @OtpCreatedOn, SYSDATETIME()) > 10
    BEGIN
        UPDATE tblUserVerification
        SET 
            OTP = @OTP,
            CreatedOn = SYSDATETIME(),
            ExpiresOn = DATEADD(MINUTE, 10, SYSDATETIME()),
            Attempts = 0,
            IsVerified = 0
        WHERE UserID = @UserID;

        RETURN
    END;

    -- still valid → block resend
    SELECT @ErrMsg = 'OTP already sent. Please wait before requesting a new one'
    GOTO ErrHandler

END

ERRHANDLER:   
BEGIN  
    RAISERROR(@ErrMsg, 16, 1, 0)  
    RETURN  
END