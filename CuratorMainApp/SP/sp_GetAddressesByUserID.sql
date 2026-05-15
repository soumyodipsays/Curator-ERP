CREATE OR ALTER PROC sp_GetAddressesByUserID
	@UserID udt_ID
AS
BEGIN
	DECLARE @PersonID udt_ID;
	DECLARE @ErrMsg	VARCHAR(4000)

	-- Find the PersonID using the UserID
	SELECT
		@PersonID = u.PersonID
	FROM tblUser u
	WHERE u.UserID = @UserID;

	-- Check if User exists
	IF @PersonID IS NULL
	BEGIN
		RAISERROR('User not found.', 16, 1);
        RETURN;
	END;

	-- Return all address by PersonID
	SELECT
		a.AddressID,
		a.Address1,
		a.Address2,
		a.City,
		s.State,
		a.PinCode,
		a.isDefault
	FROM tblAddress a
	INNER JOIN tblState s
	ON s.StateID = a.StateID 
	WHERE a.PersonID = @PersonID;

END;