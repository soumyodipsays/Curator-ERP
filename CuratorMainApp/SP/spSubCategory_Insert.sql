CREATE PROCEDURE spSubCategory_Insert
(
    @CategoryID     udt_ID,
    @SubCategory    udt_Name,
    @DisplayOrder   udt_ID2 = 0,
    @CreatedBy      udt_Name = 'system'
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO tblProductSubCategory
    (
        CategoryID,
        SubCategory,
        IsActive,
        ActivationDate,
        DisplayOrder,
        CreatedBy,
        CreatedOn
    )
    VALUES
    (
        @CategoryID,
        @SubCategory,
        1,
        [dbo].UTC2Indian(GETDATE()),
        @DisplayOrder,
        @CreatedBy,
        [dbo].UTC2Indian(GETDATE())
    )
END


