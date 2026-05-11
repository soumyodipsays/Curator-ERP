CREATE OR ALTER PROC sp_UpdateUserDetails
(
    @CustomerID udt_ID,
    @UserID udt_ID,

    @UserName udt_User = NULL,
    @FirstName udt_Name = NULL,
    @LastName udt_Name = NULL,

    @PhoneNumber udt_Phone = NULL,

    @Address1 udt_LongName = NULL,
    @Address2 udt_LongName = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ErrMsg VARCHAR(500);

    -- Check if user exists
    IF NOT EXISTS
    (
        SELECT 1
        FROM tblUser u
        WHERE u.UserID = @UserID
    )
    BEGIN
        SELECT @ErrMsg = 'User not found.';
        RAISERROR(@ErrMsg, 16, 1);
        RETURN;
    END;

    -- Update Username in tblUser
    UPDATE tblUser
    SET
        UserName = ISNULL(@UserName, UserName)
    WHERE UserID = @UserID;

    -- Update FirstName + LastName in tblPerson
    UPDATE p
    SET
        p.FirstName = ISNULL(@FirstName, p.FirstName),
        p.LastName = ISNULL(@LastName, p.LastName)
    FROM tblPerson p
    INNER JOIN tblUser u
        ON p.PersonID = u.PersonID
    WHERE u.UserID = @UserID;

    -- Update Phone Number
    UPDATE t
    SET
        t.Mobile = ISNULL(@PhoneNumber, t.Mobile)
    FROM tblTelecom t
    INNER JOIN tblUser u
        ON t.PersonID = u.PersonID
    WHERE u.UserID = @UserID;

    -- Update Addresses
    UPDATE a
    SET
        a.Address1 = ISNULL(@Address1, a.Address1),
        a.Address2 = ISNULL(@Address2, a.Address2)
    FROM tblAddress a
    INNER JOIN tblUser u
        ON a.PersonID = u.PersonID
    WHERE u.UserID = @UserID;

END;
GO