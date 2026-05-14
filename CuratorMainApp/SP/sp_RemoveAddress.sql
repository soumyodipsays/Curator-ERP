CREATE OR ALTER PROC sp_RemoveAddress
    @AddressID udt_ID,
    @UserID udt_ID
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if user exists
    IF NOT EXISTS (
        SELECT 1
        FROM tblUser u
        WHERE u.UserID = @UserID
    )
    BEGIN
        RAISERROR('User not found.', 16, 1);
        RETURN;
    END;

    -- Check if address exists
    IF NOT EXISTS (
        SELECT 1
        FROM tblAddress a
        WHERE a.AddressID = @AddressID
    )
    BEGIN
        RAISERROR('Address not found.', 16, 1);
        RETURN;
    END;

    -- Check if address belongs to user
    IF NOT EXISTS (
        SELECT 1
        FROM tblUser u
        INNER JOIN tblAddress a
            ON a.PersonID = u.PersonID
        WHERE u.UserID = @UserID
          AND a.AddressID = @AddressID
    )
    BEGIN
        RAISERROR('Address does not belong to the user.', 16, 1);
        RETURN;
    END;

    -- Remove address
    DELETE FROM tblAddress
    WHERE AddressID = @AddressID;
END;