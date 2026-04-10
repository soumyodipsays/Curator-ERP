CREATE TABLE tblHeroSections (
    HeroId INT IDENTITY PRIMARY KEY,
    CustomerID BIGINT,
    ImageUrl NVARCHAR(500),
    ImageAlt NVARCHAR(200),
    EyebrowLabel NVARCHAR(100),
    Heading NVARCHAR(500),
    Subtitle NVARCHAR(500),
    PrimaryCtaText NVARCHAR(100),
    PrimaryCtaUrl NVARCHAR(300),
    SecondaryCtaText NVARCHAR(100),
    SecondaryCtaUrl NVARCHAR(300),
    IsActive BIT DEFAULT 1,

    FOREIGN KEY (CustomerID) REFERENCES tblCustomer(CustomerID)
);

CREATE TABLE tblBrands (
    BrandId INT IDENTITY PRIMARY KEY,
    CustomerID BIGINT,
    BrandName NVARCHAR(200),
    DisplayOrder INT,

    FOREIGN KEY (CustomerID) REFERENCES tblCustomer(CustomerID)
);

CREATE TABLE tblFeaturedProducts (
    Id INT IDENTITY PRIMARY KEY,
    CustomerID BIGINT,
    ProductID BIGINT,
    DisplayOrder INT,

    FOREIGN KEY (CustomerID) REFERENCES tblCustomer(CustomerID),
    FOREIGN KEY (ProductID) REFERENCES tblProduct(ProductID)
);

CREATE TABLE tblFlashSales (
    FlashSaleId INT IDENTITY PRIMARY KEY,
    CustomerID BIGINT,
    SaleEndTime DATETIME,
    IsActive BIT DEFAULT 1,

    FOREIGN KEY (CustomerID) REFERENCES tblCustomer(CustomerID)
);

CREATE TABLE tblFlashSaleProducts (
    Id INT IDENTITY PRIMARY KEY,
    FlashSaleId INT,
    ProductID BIGINT,
    SalePrice DECIMAL(10,2),

    FOREIGN KEY (FlashSaleId) REFERENCES tblFlashSales(FlashSaleId),
    FOREIGN KEY (ProductID) REFERENCES tblProduct(ProductID)
);

CREATE TABLE tblUserVerification (
    VerificationID INT IDENTITY PRIMARY KEY,
    UserID BIGINT,
    OTP NVARCHAR(100),
    CreatedOn DATETIME,

    FOREIGN KEY (UserID) REFERENCES tblUser(UserID)
);


