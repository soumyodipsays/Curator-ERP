CREATE OR ALTER PROC sp_AddUserAddress
	@UserID udt_ID,
	@Address1 udt_LongName,
	@Address2 udt_LongName,
	@City udt_Code50,
	@StateID udt_ID,
	@PinCode udt_Zip,
	@isDefault BIT = 0
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @PersonID udt_ID;

	SELECT
		@PersonID = p.PersonID
	FROM tblPerson p
	INNER JOIN tblUser u
		ON p.PersonID = u.PersonID
	WHERE u.UserID = @UserID;

	IF @PersonID IS NULL
	BEGIN
		RAISERROR('Invalid UserID.', 16, 1);
		RETURN;
	END

	-- First address automatically becomes default
	IF NOT EXISTS
	(
		SELECT 1
		FROM tblAddress
		WHERE PersonID = @PersonID
	)
	BEGIN
		SET @isDefault = 1;
	END

	-- If this address is marked default,
	-- unset previous default addresses
	IF @isDefault = 1
	BEGIN
		UPDATE tblAddress
		SET isDefault = 0
		WHERE PersonID = @PersonID;
	END

	INSERT INTO tblAddress
	(
		PersonID,
		Address1,
		Address2,
		City,
		StateID,
		PinCode,
		CreatedBy,
		CreatedOn,
		isDefault
	)
	VALUES
	(
		@PersonID,
		@Address1,
		@Address2,
		@City,
		@StateID,
		@PinCode,
		'User',
		GETUTCDATE(),
		@isDefault
	);
END;