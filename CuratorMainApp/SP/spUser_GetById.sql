CREATE OR ALTER PROCEDURE spUser_GetById
(
    @UserID BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        UserID,
        UserName,
        Email,
        UserTypeCode,
        CAST(IsActive AS BIT) AS IsActive
    FROM tblUser
    WHERE UserID = @UserID;
END
GO