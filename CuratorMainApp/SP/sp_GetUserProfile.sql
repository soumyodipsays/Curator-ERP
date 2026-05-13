USE [Ecommerce]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUserProfile]    Script Date: 5/13/2026 10:36:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[sp_GetUserProfile]
    @UserID udt_ID
AS
BEGIN
    SET NOCOUNT ON;

    ---------------------------------------------------
    -- 1. USER CORE DETAILS
    ---------------------------------------------------
    SELECT
        u.UserID,
        u.IsActive,
        u.UserName,
        u.Email,
        u.CreatedOn,

        p.FirstName,
        p.MiddleInitials,
        p.LastName,
        p.avatar_url,

        ISNULL(oc.OrderCount, 0) AS OrderCount,
        ISNULL(cc.CartProductCount, 0) AS CartProductCount

    FROM tblUser u

    LEFT JOIN tblPerson p
        ON u.PersonID = p.PersonID

    LEFT JOIN (
        SELECT
            UserID,
            COUNT(*) AS OrderCount
        FROM tblOrder
        GROUP BY UserID
    ) oc
        ON u.UserID = oc.UserID

    LEFT JOIN (
        SELECT
            UserID,
            COUNT(*) AS CartProductCount
        FROM tblCart
        GROUP BY UserID
    ) cc
        ON u.UserID = cc.UserID

    WHERE u.UserID = @UserID;

    ---------------------------------------------------
    -- 2. PHONES
    ---------------------------------------------------
    SELECT
        t.Mobile
    FROM tblTelecom t
    INNER JOIN tblUser u
        ON u.PersonID = t.PersonID
    WHERE u.UserID = @UserID;

    ---------------------------------------------------
    -- 3. ADDRESSES
    ---------------------------------------------------
    SELECT
        a.Address1,
        a.Address2,
        a.City,
        s.State,
        a.PinCode
    FROM tblAddress a
    INNER JOIN tblUser u
        ON u.PersonID = a.PersonID
	LEFT JOIN tblState s
		ON a.StateID = s.StateID
    WHERE u.UserID = @UserID;

END;