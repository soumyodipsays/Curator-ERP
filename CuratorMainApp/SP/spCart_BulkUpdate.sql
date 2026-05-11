USE [Ecommerce]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[spCart_BulkUpdate] 
(	
	@UserID			INT,            -- Matches UserID column
	@CartItemsJSON	NVARCHAR(MAX),  -- Your stringified JSON array
	@UserName		VARCHAR(50)     -- Matches ModifiedBy column
)	
AS BEGIN

	DECLARE @ErrMsg VARCHAR(500)
	
	BEGIN TRY
		BEGIN TRANSACTION
		
        IF (@CartItemsJSON IS NOT NULL AND LTRIM(RTRIM(@CartItemsJSON)) <> '')
        BEGIN
            UPDATE C
            SET 
                -- Update Quantity
                C.Quantity = J.Quantity,
                
                -- Handle auto-removal if quantity drops to 0
                C.IsRemove = CASE WHEN J.Quantity <= 0 THEN 1 ELSE C.IsRemove END,
                C.RemoveDate = CASE WHEN J.Quantity <= 0 THEN GETDATE() ELSE C.RemoveDate END,
                C.IsSaveForLater = CASE WHEN J.Quantity <= 0 THEN 0 ELSE C.IsSaveForLater END,
                
                -- Audit fields
                C.ModifiedBy = @UserName,
                C.ModifiedOn = GETDATE()
                
            FROM tblCart C
            -- Read the JSON
            INNER JOIN OPENJSON(@CartItemsJSON) WITH (
                CartID INT '$.CartID',
                Quantity INT '$.Quantity'
            ) J ON C.CartID = J.CartID
            
            -- Security/Filter: Only active items belonging to this user
            WHERE C.UserID = @UserID 
              AND C.IsBuy = 0;
              
        END

		COMMIT TRANSACTION		
		RETURN	
	END TRY
	BEGIN CATCH				
		ROLLBACK TRANSACTION
		IF (@ErrMsg IS NULL) SELECT @ErrMsg = 'There was a problem bulk updating the cart. Please contact support.'
		RAISERROR(@ErrMsg , 16, 1, 0)
		RETURN
	END CATCH
END
GO