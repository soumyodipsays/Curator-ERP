CREATE PROCEDURE spColor_Insert
(
    @CategoryID     udt_ID,
    @SubCategoryID  udt_ID,
    @Color          udt_Name,
    @ColorCode      udt_Name,
    @DisplayOrder   udt_ID2 = 0,
    @CreatedBy      udt_Name = 'system'
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO tblProductColor
    (
        CategoryID,
        SubCategoryID,
        Color,
        ColorCode,
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
        @Color,
        @ColorCode,
        1,
        [dbo].UTC2Indian(GETDATE()),
        @DisplayOrder,
        @CreatedBy,
        [dbo].UTC2Indian(GETDATE())
    )
END