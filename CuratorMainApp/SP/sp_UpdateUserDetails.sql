CREATE OR ALTER PROC sp_UpdateUserDetails
	@CustomerID udt_ID,
	@UserID udt_ID,

	@UserName udt_User,
	@FirstName udt_Name,
	@LastName udt_Name,

	@PhoneNumber udt_Phone,

	@Address1 udt_LongName,
	@Address2 udt_LongName
AS
BEGIN
	SET NOCOUNT ON;

	-- Update Username in tblUser
	UPDATE tblUser
	SET 
		UserName = @UserName
	WHERE UserID = @UserID;

	-- Update firstName, lastName in tblPerson
	UPDATE p
	SET
		p.FirstName = @FirstName,
		p.LastName = @LastName
	FROM tblPerson p
	INNER JOIN tblUser u
	ON p.PersonID = u.PersonID
	WHERE u.UserID = @UserID

	-- Update Phone Number
	UPDATE t
	SET
		t.Mobile = @PhoneNumber
	FROM tblTelecom t
	INNER JOIN tblUser u
	ON t.PersonID = u.PersonID
	WHERE u.UserID = @UserID

	-- Update addresses
	UPDATE a
	SET
		a.Address1 = @Address1,
		a.Address2 = @Address2
	FROM tblAddress a
	INNER JOIN tblUser u
	ON a.PersonID = u.PersonID
	WHERE u.UserID = @UserID
END;

/** 
	first name
	last name
	username
	phone number
	address -> home and office which is address1 and address2
**/