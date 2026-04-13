CREATE PROCEDURE spBrand_Insert
(
    @CategoryID     udt_ID,
    @SubCategoryID  udt_ID,
    @Brand          udt_Name,
    @DisplayOrder   udt_ID2 = 0,
    @CreatedBy      udt_Name = 'system'
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO tblBrand
    (
        CategoryID,
        SubCategoryID,
        Brand,
        IsActive,
        ActivationDate,
        DisplayOrder,
        CreatedBy,
        CreatedOn
    )
    VALUES
    (
        @CategoryID,
        @SubCategoryID,
        @Brand,
        1,
        [dbo].UTC2Indian(GETDATE()),
        @DisplayOrder,
        @CreatedBy,
        [dbo].UTC2Indian(GETDATE())
    )
END