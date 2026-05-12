USE [Ecommerce]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[spCoupon_Validate]
(
    @CustomerID BIGINT,
    @CouponCode VARCHAR(25),
    @CartTotal MONEY -- We need the cart total to check the MinimumPurchase and calculate Percentage discounts
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Variables to hold validation state
    DECLARE @IsValid BIT = 0;
    DECLARE @Message VARCHAR(250) = '';
    DECLARE @CalculatedDiscount MONEY = 0;

    -- Variables to hold the fetched coupon data
    DECLARE @CouponID BIGINT;
    DECLARE @ValidFrom DATETIME;
    DECLARE @ValidTo DATETIME;
    DECLARE @MinimumPurchase MONEY;
    DECLARE @IsInrOrPercentage CHAR(1);
    DECLARE @INR MONEY;
    DECLARE @Percentage DECIMAL(5,2);
    DECLARE @ApplyCouponToAllProduct BIT;

    BEGIN TRY
        -- ====================================================================
        -- STEP 1: Fetch the Coupon Details
        -- ====================================================================
        SELECT TOP 1
            @CouponID = CouponID,
            @ValidFrom = ValidFrom,
            @ValidTo = ValidTo,
            @MinimumPurchase = ISNULL(MinimumPurchase, 0),
            @IsInrOrPercentage = IsInrOrPercentage,
            @INR = INR,
            @Percentage = Percentage,
            @ApplyCouponToAllProduct = ApplyCouponToAllProduct
        FROM tblCoupon WITH (NOLOCK)
        WHERE CustomerID = @CustomerID AND CouponCode = @CouponCode;

        -- ====================================================================
        -- STEP 2: Run Validation Rules
        -- ====================================================================
        IF (@CouponID IS NULL)
        BEGIN
            SET @Message = 'Invalid Coupon Code.';
        END  
        -- Check if the coupon is active yet
        ELSE IF (@ValidFrom IS NOT NULL AND GETDATE() < @ValidFrom)
        BEGIN
            SET @Message = 'Invalid Coupon Code.';
        END
        -- Check if the coupon is expired
        ELSE IF (@ValidTo IS NOT NULL AND GETDATE() > @ValidTo)
        BEGIN
            SET @Message = 'This coupon has expired.';
        END
        -- Check minimum purchase requirements
        ELSE IF (@CartTotal < @MinimumPurchase)
        BEGIN
            SET @Message = 'A minimum purchase of ?' + CAST(@MinimumPurchase AS VARCHAR(50)) + ' is required to use this coupon.';
        END
        ELSE
        BEGIN
            -- If it passes all checks, it is valid!
            SET @IsValid = 1;
            SET @Message = 'Coupon applied successfully!';
        END

        -- ====================================================================
        -- STEP 3: Calculate the actual discount amount
        -- ====================================================================
        IF (@IsValid = 1)
        BEGIN
            IF (@IsInrOrPercentage = 'I') -- 'I' for INR (Flat Amount)
            BEGIN
                SET @CalculatedDiscount = ISNULL(@INR, 0);
            END
            ELSE IF (@IsInrOrPercentage = 'P') -- 'P' for Percentage
            BEGIN
                SET @CalculatedDiscount = (@CartTotal * ISNULL(@Percentage, 0)) / 100.0;
            END

            -- Failsafe: A discount cannot be larger than the cart total itself
            IF (@CalculatedDiscount > @CartTotal)
            BEGIN
                SET @CalculatedDiscount = @CartTotal;
            END
        END

        -- ====================================================================
        -- STEP 4: Return the Result to C#
        -- ====================================================================
        SELECT 
            @IsValid AS IsValid,
            @Message AS ReturnMessage,
            @CouponID AS CouponID,
            @IsInrOrPercentage AS DiscountType,
            ISNULL(@INR, 0) AS FlatDiscountAmount,
            ISNULL(@Percentage, 0) AS PercentageValue,
            @CalculatedDiscount AS CalculatedDiscountAmount,
            ISNULL(@ApplyCouponToAllProduct, 0) AS ApplyToAllProducts;

    END TRY
    BEGIN CATCH
        -- Return a safe error message if SQL crashes
        SELECT 
            0 AS IsValid,
            'An error occurred while validating the coupon.' AS ReturnMessage,
            0 AS CouponID,
            '' AS DiscountType,
            0 AS FlatDiscountAmount,
            0 AS PercentageValue,
            0 AS CalculatedDiscountAmount,
            0 AS ApplyToAllProducts;
    END CATCH
END
GO