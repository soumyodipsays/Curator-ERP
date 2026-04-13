USE [Ecommerce]
GO
/****** Object:  StoredProcedure [dbo].[spCategory_Insert]    Script Date: 4/13/2026 3:54:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spCategory_Insert]
(
    @Category        udt_Name,
    @DisplayOrder    udt_ID2 = 0,
    @CreatedBy       udt_Name = 'system',
	@CustomerID		 bigint
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO tblProductCategory
    (
        Category,
        IsActive,
        ActivationDate,
        DisplayOrder,
        CreatedBy,
        CreatedOn,
		CustomerID
    )
    VALUES
    (
        @Category,
        1,
        [dbo].UTC2Indian(GETDATE()),
        @DisplayOrder,
        @CreatedBy,
        [dbo].UTC2Indian(GETDATE()),
		@CustomerID
    )
END


