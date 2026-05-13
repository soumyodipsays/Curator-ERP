USE [Ecommerce]
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateUserDetails]    Script Date: 5/13/2026 10:14:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROC [dbo].[sp_UpdateUserDetails]
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
	DECLARE @PersonID INT;

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

    -- Check if username already exists
	IF EXISTS(
		SELECT 1
		FROM tblUser u
		WHERE u.UserName = @UserName
	)
	BEGIN
		SELECT @ErrMsg = 'Username already exists.';
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

    

	-- Get PersonID
	SELECT @PersonID = PersonID
	FROM tblUser
	WHERE UserID = @UserID;

	---------------------------------------------------
	-- Phone Number UPSERT
	---------------------------------------------------

	IF EXISTS (
		SELECT 1
		FROM tblTelecom
		WHERE PersonID = @PersonID
	)
	BEGIN
		UPDATE tblTelecom
		SET Mobile = ISNULL(@PhoneNumber, Mobile)
		WHERE PersonID = @PersonID;
	END
	ELSE
	BEGIN
		INSERT INTO tblTelecom
		(
			PersonID,
			Mobile,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@PersonID,
			@PhoneNumber,
			N'Admin',
			GETUTCDATE()
		);
	END;

	---------------------------------------------------
	-- Address UPSERT
	---------------------------------------------------

	IF EXISTS (
		SELECT 1
		FROM tblAddress
		WHERE PersonID = @PersonID
	)
	BEGIN
		UPDATE tblAddress
		SET
			Address1 = ISNULL(@Address1, Address1),
			Address2 = ISNULL(@Address2, Address2)
		WHERE PersonID = @PersonID;
	END
	ELSE
	BEGIN
		INSERT INTO tblAddress
		(
			PersonID,
			Address1,
			Address2,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@PersonID,
			@Address1,
			@Address2,
			N'Admin',
			GETUTCDATE()
		);
	END;

END;
