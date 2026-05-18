CREATE OR ALTER PROC sp_GetProductListWithOffset
    @CustomerID udt_ID,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT =
        (@PageNumber - 1) * @PageSize;

    BEGIN TRY

        SELECT 
            -- Parent Product
            MIN(p.ProductID) AS ProductID,
            p.ProductName,
            p.ProductDescription,
            p.ProductCaption,
            p.CategoryID,
            ISNULL(p.BrandID, 0) AS BrandID,
            pb.Brand,
            pg.Category,
            psg.SubCategory,

            -- Price Range
            MIN(CAST(p.Price AS DECIMAL(18,2))) AS MinPrice,
            MAX(CAST(p.Price AS DECIMAL(18,2))) AS MaxPrice,

            -- Rating
            ISNULL(AVG(CAST(r.Rating AS DECIMAL(18,2))), 0) AS Rating,

            -- Variants JSON
            (
                SELECT 
                    p2.ProductID,
                    p2.ImageURL,
                    CAST(p2.Price AS DECIMAL(18,2)) AS Price,

                    CASE
                        WHEN p2.Price BETWEEN 0 AND 50 THEN 1
                        WHEN p2.Price BETWEEN 51 AND 100 THEN 2
                        WHEN p2.Price BETWEEN 101 AND 200 THEN 3
                        WHEN p2.Price BETWEEN 201 AND 300 THEN 4
                        WHEN p2.Price BETWEEN 301 AND 400 THEN 5
                        WHEN p2.Price BETWEEN 401 AND 500 THEN 6
                        WHEN p2.Price BETWEEN 501 AND 600 THEN 7
                        WHEN p2.Price BETWEEN 601 AND 700 THEN 8
                        ELSE 9
                    END AS PriceID,

                    p2.SizeID,
                    ps.Size AS SizeName,

                    p2.ColorID,
                    pc.Color AS ColorName,
                    pc.ColorCode,

                    p2.IsActive,
                    p2.DropDate,
                    p2.ActivationDate,
                    p2.Status,

                    p2.DiscountID,
                    d.IsInrOrPercentage,
                    d.Discount,
                    d.INR,
                    d.Percentage

                FROM tblProduct p2
                LEFT JOIN tblProductSize ps
                    ON p2.SizeID = ps.SizeID

                LEFT JOIN tblProductColor pc
                    ON p2.ColorID = pc.ColorID

                LEFT JOIN tblDiscount d
                    ON p2.DiscountID = d.DiscountID

                WHERE 
                    p2.ProductName = p.ProductName
                    AND p2.BrandID = p.BrandID
                    AND p2.CategoryID = p.CategoryID
                    AND p2.SubCategoryID = p.SubCategoryID
                    AND p2.CustomerID = p.CustomerID

                ORDER BY 
                    ps.Size,
                    pc.Color

                FOR JSON PATH
            ) AS Variants

        FROM tblProduct p

        LEFT JOIN tblProductBrand pb
            ON p.BrandID = pb.BrandID

        LEFT JOIN tblProductRatings r
            ON p.ProductID = r.ProductID

        LEFT JOIN tblProductCategory pg
            ON p.CategoryID = pg.CategoryID

        LEFT JOIN tblProductSubCategory psg
            ON p.SubCategoryID = psg.SubCategoryID

        WHERE p.CustomerID = @CustomerID

        GROUP BY 
            p.ProductName,
            p.ProductDescription,
            p.ProductCaption,
            p.CategoryID,
            p.SubCategoryID,
            p.BrandID,
            pb.Brand,
            pg.Category,
            psg.SubCategory,
            p.CustomerID

        ORDER BY p.ProductName

        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY;

    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg VARCHAR(5000);

        SET @ErrMsg = ERROR_MESSAGE();

        RAISERROR(@ErrMsg, 16, 1);
    END CATCH
END;