CREATE PROCEDURE spDiscount_Insert
(
    @CustomerID        udt_ID,
    @Discount          udt_LongName,
    @IsInrOrPercentage CHAR(1),
    @INR               udt_Money = NULL,
    @Percentage        udt_Percentage = NULL,
    @MinimumPurchase   udt_Money = 0,
    @DisplayOrder      udt_ID2 = 0,
    @CreatedBy         udt_ID
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO tblDiscount
    (
        CustomerID,
        Discount,
        IsInrOrPercentage,
        INR,
        Percentage,
        MinimumPurchase,
        DisplayOrder,
        CreatedBy,
        CreatedOn
    )
    VALUES
    (
        @CustomerID,
        @Discount,
        @IsInrOrPercentage,
        @INR,
        @Percentage,
        @MinimumPurchase,
        @DisplayOrder,
        @CreatedBy,
        [dbo].UTC2Indian(GETDATE())
    )
END