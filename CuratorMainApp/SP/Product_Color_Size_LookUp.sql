CREATE TABLE tblProductSize_LookUp (
	ID udt_ID IDENTITY(1,1) PRIMARY KEY,
	ProductID udt_ID NOT NULL,
	SizeID udt_ID NOT NULL,

	CONSTRAINT fk_ProductLink 
	FOREIGN KEY (ProductID)
	REFERENCES tblProduct(ProductID),

	CONSTRAINT fk_SizeLink
	FOREIGN KEY (SizeID)
	REFERENCES tblProductSize(SizeID),

	CONSTRAINT uq_Product_Size UNIQUE(ProductID, SizeID)
);

CREATE TABLE tblProductColor_LookUp (
	ID udt_ID IDENTITY(1,1) PRIMARY KEY,
	ProductID udt_ID NOT NULL,
	ColorID udt_ID NOT NULL,

	CONSTRAINT fk_ProductColorLink 
	FOREIGN KEY (ProductID)
	REFERENCES tblProduct(ProductID),

	CONSTRAINT fk_ColorLink
	FOREIGN KEY (ColorID)
	REFERENCES tblProductColor(ColorID),

	CONSTRAINT uq_Product_Color UNIQUE(ProductID, ColorID)
);

