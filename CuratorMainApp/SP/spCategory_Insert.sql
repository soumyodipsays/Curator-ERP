CREATE PROCEDURE spCategory_Insert
(
    @Category        udt_Name,
    @DisplayOrder    udt_ID2 = 0,
    @CreatedBy       udt_Name = 'system'
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
        CreatedOn
    )
    VALUES
    (
        @Category,
        1,
        [dbo].UTC2Indian(GETDATE()),
        @DisplayOrder,
        @CreatedBy,
        [dbo].UTC2Indian(GETDATE())
    )
END


