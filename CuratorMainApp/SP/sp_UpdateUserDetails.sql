USE [Ecommerce]
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateUserDetails]    Script Date: 5/14/2026 10:05:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[sp_UpdateUserDetails]
(
    @CustomerID udt_ID = 1,
    @UserID udt_ID,

    @UserName udt_User = NULL,
    @FirstName udt_Name = NULL,
    @LastName udt_Name = NULL,

    @PhoneNumber udt_Phone = NULL,

    @AddressID udt_ID = 0,
    @Address1 udt_LongName = NULL,
    @Address2 udt_LongName = NULL,
    @City udt_Code50 = NULL,
    @PinCode udt_Zip = NULL,
    @StateID udt_ID = NULL,
    @IsDefault BIT = 0
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ErrMsg VARCHAR(500);
    DECLARE @PersonID INT;

    ---------------------------------------------------
    -- VALIDATE USER
    ---------------------------------------------------
    IF NOT EXISTS (
        SELECT 1
        FROM tblUser u
        WHERE u.UserID = @UserID
    )
    BEGIN
        RAISERROR('User not found.', 16, 1);
        RETURN;
    END;

    ---------------------------------------------------
    -- USERNAME CHECK (FIXED)
    ---------------------------------------------------
    IF @UserName IS NOT NULL AND EXISTS (
        SELECT 1
        FROM tblUser u
        WHERE u.UserName = @UserName
          AND u.UserID <> @UserID
    )
    BEGIN
        RAISERROR('Username already exists.', 16, 1);
        RETURN;
    END;

    ---------------------------------------------------
    -- UPDATE USER
    ---------------------------------------------------
    UPDATE tblUser
    SET UserName = ISNULL(@UserName, UserName)
    WHERE UserID = @UserID;

    UPDATE p
    SET
        p.FirstName = ISNULL(@FirstName, p.FirstName),
        p.LastName = ISNULL(@LastName, p.LastName)
    FROM tblPerson p
    INNER JOIN tblUser u
        ON p.PersonID = u.PersonID
    WHERE u.UserID = @UserID;

    ---------------------------------------------------
    -- GET PERSON ID
    ---------------------------------------------------
    SELECT @PersonID = PersonID
    FROM tblUser
    WHERE UserID = @UserID;

    ---------------------------------------------------
    -- PHONE UPSERT
    ---------------------------------------------------
    IF EXISTS (SELECT 1 FROM tblTelecom WHERE PersonID = @PersonID)
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
    -- ADDRESS UPSERT (FIXED)
    ---------------------------------------------------
    IF @AddressID IS NULL OR @AddressID = 0
    BEGIN
        INSERT INTO tblAddress
        (
            PersonID,
            Address1,
            Address2,
            City,
            StateID,
            PinCode,
            IsDefault,
            CreatedBy,
            CreatedOn
        )
        VALUES
        (
            @PersonID,
            @Address1,
            @Address2,
            @City,
            @StateID,
            @PinCode,
            ISNULL(@IsDefault, 0),
            N'Admin',
            GETUTCDATE()
        );

        SET @AddressID = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        UPDATE tblAddress
        SET
            Address1 = ISNULL(@Address1, Address1),
            Address2 = ISNULL(@Address2, Address2),
            City     = ISNULL(@City, City),
            StateID  = ISNULL(@StateID, StateID),
            PinCode  = ISNULL(@PinCode, PinCode),
            IsDefault = ISNULL(@IsDefault, IsDefault),
            ModifiedOn = GETUTCDATE(),
            ModifiedBy = N'Admin'
        WHERE AddressID = @AddressID
          AND PersonID = @PersonID;

        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('Address not found for user.', 16, 1);
            RETURN;
        END;
    END;

    ---------------------------------------------------
    -- ENSURE SINGLE DEFAULT ADDRESS
    ---------------------------------------------------
    IF @IsDefault = 1
    BEGIN
        UPDATE tblAddress
        SET IsDefault = 0
        WHERE PersonID = @PersonID
          AND AddressID <> @AddressID;
    END;

END;