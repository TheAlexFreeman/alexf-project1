CREATE SCHEMA Store;
GO
-- Create all tables for Store store DB with necessary columns and keys
CREATE TABLE Store.Items (
	-- Columns
	ID INT IDENTITY,
	[Name] NVARCHAR(32) NOT NULL,
	Cost MONEY NOT NULL DEFAULT(0)
	-- Constraints
	CONSTRAINT PK_ItemID PRIMARY KEY CLUSTERED (ID),
	CONSTRAINT U_ItemName UNIQUE ([Name])
);
--CREATE TABLE Store.ProductTypes (
--	-- Columns
--	ID INT IDENTITY,
--	[Name] NVARCHAR(32) NOT NULL,
--	TypeOf INT NULL,
--	-- Constraints
--	CONSTRAINT PK_TypeID PRIMARY KEY CLUSTERED (ID),
--	CONSTRAINT U_TypeName UNIQUE ([Name]),
--	CONSTRAINT FK_ProductType_TypeOf_to_ProductType FOREIGN KEY (TypeOf) REFERENCES Store.ProductType(ID)
--		--ON UPDATE DELETE
--		--ON DELETE CASCADE
--);
CREATE TABLE Store.Products (
	-- Columns
	ID INT IDENTITY,
	[Name] NVARCHAR(64) NOT NULL,
	--TypeID INT NOT NULL,
	Price MONEY NOT NULL DEFAULT(0)
	-- Constraints
	CONSTRAINT PK_ProductID PRIMARY KEY CLUSTERED (ID),
	CONSTRAINT U_ProductName UNIQUE ([Name]),
	--CONSTRAINT FK_Product_TypeID_to_ProductType FOREIGN KEY (TypeID) REFERENCES Store.ProductType(ID)
	--	ON UPDATE CASCADE
	--	ON DELETE CASCADE
);
--CREATE TABLE Store.[Addresses] (
--	-- Columns
--	ID INT IDENTITY,
--	Line1 NVARCHAR(64) NOT NULL,
--	Line2 NVARCHAR(64) NULL,
--	City NVARCHAR(64) NOT NULL,
--	[State] NVARCHAR(16) NULL,
--	Country NVARCHAR(32) NOT NULL,
--	PostalCode NVARCHAR(16) NOT NULL
--	-- Constraints
--	CONSTRAINT PK_AddressID PRIMARY KEY CLUSTERED (ID)
--);
CREATE TABLE Store.[Locations] (
	-- Columns
	ID INT IDENTITY,
	[Name] NVARCHAR(32) NOT NULL,
	--StoreAddress INT NOT NULL,
	-- Constraints
	CONSTRAINT PK_LocationID PRIMARY KEY CLUSTERED (ID),
	CONSTRAINT U_StoreName UNIQUE ([Name]),
	--CONSTRAINT FK_Location_StoreAddress_to_Address FOREIGN KEY (StoreAddress) REFERENCES Store.[Address](ID)
	--	ON UPDATE CASCADE
	--	ON DELETE CASCADE
);
CREATE TABLE Store.Customers (
	-- Columns
	ID INT IDENTITY,
	FirstName NVARCHAR(32) NOT NULL,
	LastName NVARCHAR(32) NOT NULL,
	--Phone NVARCHAR(32) NULL,
	--Email NVARCHAR(32) NULL,
	--HomeAddress INT NOT NULL,
	DefaultStoreID INT NULL,
	-- Constraints
	CONSTRAINT PK_CustomerID PRIMARY KEY CLUSTERED (ID),
	--CONSTRAINT FK_Customer_HomeAddress_to_Address FOREIGN KEY (HomeAddress) REFERENCES Store.[Address](ID)
	--	ON UPDATE CASCADE
	--	ON DELETE CASCADE,
	CONSTRAINT FK_Customers_DefaultStoreID_to_Locations FOREIGN KEY (DefaultStoreID) REFERENCES Store.[Locations](ID)
		--ON UPDATE CASCADE
		--ON DELETE CASCADE
);
CREATE TABLE Store.[Orders] (
	-- Columns
	ID INT IDENTITY,
	[Start] DATETIME2 NOT NULL DEFAULT(GETDATE()),
	LastModified DATETIME2 NOT NULL DEFAULT(GETDATE()),
	IsOpen BIT NOT NULL,
	BuyerID INT NOT NULL,
	SellerID INT NOT NULL,
	-- Constraints
	CONSTRAINT PK_OrderID PRIMARY KEY CLUSTERED (ID),
	CONSTRAINT FK_Orders_BuyerID_to_Customers FOREIGN KEY (BuyerID) REFERENCES Store.Customers(ID)
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	CONSTRAINT FK_Orders_SellerID_to_Locations FOREIGN KEY (SellerID) REFERENCES Store.[Locations](ID)
		--ON UPDATE CASCADE
		--ON DELETE CASCADE
);
CREATE TABLE Store.ProductOrders (
	-- Columns
	OrderID INT NOT NULL,
	ProductID INT NOT NULL,
	Quantity INT NOT NULL DEFAULT(0),
	-- Constraints
	CONSTRAINT PK_OrderID_ProductID PRIMARY KEY CLUSTERED (OrderID, ProductID),
	CONSTRAINT FK_ProductOrders_OrderID_to_Orders FOREIGN KEY (OrderID) REFERENCES Store.[Orders](ID)
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	CONSTRAINT FK_ProductOrders_ProductID_to_Products FOREIGN KEY (ProductID) REFERENCES Store.Products(ID)
		ON UPDATE CASCADE
		ON DELETE CASCADE
);
CREATE TABLE Store.InventoryItems (
	-- Columns
	LocationID INT NOT NULL,
	ItemID INT NOT NULL,
	Quantity INT NOT NULL DEFAULT(0),
	-- Constraints
	CONSTRAINT PK_LocationID_ItemID PRIMARY KEY CLUSTERED (LocationID, ItemID),
	CONSTRAINT FK_InventoryItems_LocationID_to_Locations FOREIGN KEY (LocationID) REFERENCES Store.[Locations](ID)
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	CONSTRAINT FK_InventoryItems_ItemID_to_Items FOREIGN KEY (ItemID) REFERENCES Store.Items(ID)
		ON UPDATE CASCADE
		ON DELETE CASCADE
);
CREATE TABLE Store.ProductItems (
	-- Columns
	ProductID INT NOT NULL,
	ItemID INT NOT NULL,
	Quantity INT NOT NULL DEFAULT(0),
	-- Constraints
	CONSTRAINT PK_ProductID_ItemID PRIMARY KEY CLUSTERED (ProductID, ItemID),
	CONSTRAINT FK_ProductItems_ProductID_to_Products FOREIGN KEY (ProductID) REFERENCES Store.Products(ID)
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	CONSTRAINT FK_ProductItems_ItemID_to_Items FOREIGN KEY (ItemID) REFERENCES Store.Items(ID)
		ON UPDATE CASCADE
		ON DELETE CASCADE
);
CREATE TABLE Store.ProductLocations (
	-- Columns
	ProductID INT NOT NULL,
	LocationID INT NOT NULL
	-- Constraints
	CONSTRAINT PK_ProductID_LocationID PRIMARY KEY CLUSTERED (ProductID, LocationID),
	CONSTRAINT FK_ProductLocations_ProductID_to_Products FOREIGN KEY (ProductID) REFERENCES Store.Products(ID)
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	CONSTRAINT FK_ProductLocations_LocationID_to_Locations FOREIGN KEY (LocationID) REFERENCES Store.[Locations](ID)
		ON UPDATE CASCADE
		ON DELETE CASCADE
);
GO
INSERT INTO Store.ProductLocation(LocationID, ProductID) VALUES
	((SELECT ID FROM Store.Location WHERE [Name] = 'Arlington'),
		(SELECT ID FROM Store.Item WHERE [Name] = 'Cookie')),
	((SELECT ID FROM Store.Location WHERE [Name] = 'Arlington'),
		(SELECT ID FROM Store.Item WHERE [Name] = 'Brownie'));

INSERT INTO Store.InventoryItem(LocationID, ItemID, Quantity) VALUES
	(
		(SELECT ID FROM Store.Location WHERE [Name] = 'Arlington'),
		(SELECT ID FROM Store.Item WHERE [Name] = 'Water Bottle'),
		100
	);
	(
		(SELECT ID FROM Store.Location WHERE [Name] = 'Berkeley'),
		(SELECT ID FROM Store.Item WHERE [Name] = 'Brownie'),
		100
	),
	(
		(SELECT ID FROM Store.Location WHERE [Name] = 'Las Vegas'),
		(SELECT ID FROM Store.Item WHERE [Name] = 'Cookie'),
		100
	),
	(
		(SELECT ID FROM Store.Location WHERE [Name] = 'Las Vegas'),
		(SELECT ID FROM Store.Item WHERE [Name] = 'Brownie'),
		100
	),
	(
		(SELECT ID FROM Store.Location WHERE [Name] = 'Las Vegas'),
		(SELECT ID FROM Store.Item WHERE [Name] = 'Water Bottle'),
		1000
	),
	(
		(SELECT ID FROM Store.Location WHERE [Name] = 'Arlington'),
		(SELECT ID FROM Store.Item WHERE [Name] = 'Cookie'),
		100
	),
	(
		(SELECT ID FROM Store.Location WHERE [Name] = 'Arlington'),
		(SELECT ID FROM Store.Item WHERE [Name] = 'Brownie'),
		100
	);