CREATE PROCEDURE spSize_Insert
(
    @CategoryID     udt_ID,
    @SubCategoryID  udt_ID,
    @Size           udt_Name,
    @DisplayOrder   udt_ID2 = 0,
    @CreatedBy      udt_Name = 'system'
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO tblProductSize
    (
        CategoryID,
        SubCategoryID,
        Size,
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
        @Size,
        1,
        [dbo].UTC2Indian(GETDATE()),
        @DisplayOrder,
        @CreatedBy,
        [dbo].UTC2Indian(GETDATE())
    )
END