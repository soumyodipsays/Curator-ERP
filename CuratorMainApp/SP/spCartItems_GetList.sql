USE [Ecommerce]
GO
/****** Object:  StoredProcedure [dbo].[spCartItems_GetList]    Script Date: 4/23/2026 4:25:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[spCartItems_GetList]
(
	@UserID		udt_ID
)
AS                          
BEGIN    
	BEGIN TRY
		DECLARE @ErrMsg	VARCHAR(4000)
					   
		SELECT  c.CartID,
				c.ProductID,
				c.Quantity,
				c.AddDate,
				c.IsBuy,
				c.BuyDate,
				c.IsRemove,
				c.RemoveDate,
				c.IsSaveForLater,
				c.SaveForLaterDate,
				c.CreatedOn,
				c.ModifiedOn,
				p.ProductName,
				p.ImageURL,
				p.Price,
				p.ProductDescription,
				d.IsInrOrPercentage, 
				(
					CASE 
					WHEN d.IsInrOrPercentage = 'I' 
					THEN d.INR 
					ELSE (p.Price*c.Quantity)*d.Percentage* 1.0/100 END
				) AS Discount
		FROM tblCart c
		INNER JOIN tblProduct p ON c.ProductID=p.ProductID
		LEFT JOIN tblDiscount d ON p.DiscountID=d.DiscountID
		WHERE UserID=@UserID 
		AND IsBuy = 0
		AND IsRemove = 0
		AND IsSaveForLater = 0
		ORDER BY AddDate

		RETURN
	END TRY 
	BEGIN CATCH 
		SET @ErrMsg = ERROR_MESSAGE()
		RAISERROR(@ErrMsg, 16, 1, 0)
		RETURN 
	END CATCH
END
