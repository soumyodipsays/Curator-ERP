CREATE OR ALTER PROC sp_GetUserProfile
	@UserID udt_ID
AS
BEGIN
	SET NOCOUNT ON;

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

		t.Mobile,

		ISNULL(oc.OrderCount, 0) AS OrderCount,
		ISNULL(cc.CartProductCount, 0) AS CartProductCount,

		a.Address1,
		a.Address2,
		a.City,
		a.StateID,
		a.PinCode

	FROM tblUser u

	LEFT JOIN tblPerson p
		ON u.PersonID = p.PersonID

	LEFT JOIN tblTelecom t
		ON u.PersonID = t.PersonID

	LEFT JOIN tblAddress a
		ON u.PersonID = a.PersonID

	LEFT JOIN
	(
		SELECT
			UserID,
			COUNT(*) AS OrderCount
		FROM tblOrder
		GROUP BY UserID
	) oc
		ON u.UserID = oc.UserID

	LEFT JOIN
	(
		SELECT
			UserID,
			COUNT(*) AS CartProductCount
		FROM tblCart
		GROUP BY UserID
	) cc
		ON u.UserID = cc.UserID

	WHERE u.UserID = @UserID;

END;