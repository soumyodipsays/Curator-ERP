CREATE PROCEDURE spCoupon_Insert
(
    @CustomerID              udt_ID,
    @CouponCode              VARCHAR(25),
    @ApplyCouponToAllProduct udt_YesNo,
    @IsInrOrPercentage       CHAR(1),
    @INR                     udt_Money = NULL,
    @Percentage              udt_Percentage = NULL,
    @ValidFrom               udt_Datetime = NULL,
    @ValidTo                 udt_Datetime = NULL,
    @MinimumPurchase         udt_Money = 0,
    @DisplayOrder            udt_ID2 = 0,
    @CreatedBy               udt_ID
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO tblCoupon
    (
        CustomerID,
        CouponCode,
        ApplyCouponToAllProduct,
        IsInrOrPercentage,
        INR,
        Percentage,
        ValidFrom,
        ValidTo,
        MinimumPurchase,
        DisplayOrder,
        CreatedBy,
        CreatedOn
    )
    VALUES
    (
        @CustomerID,
        @CouponCode,
        @ApplyCouponToAllProduct,
        @IsInrOrPercentage,
        @INR,
        @Percentage,
        @ValidFrom,
        @ValidTo,
        @MinimumPurchase,
        @DisplayOrder,
        @CreatedBy,
        [dbo].UTC2Indian(GETDATE())
    )
END