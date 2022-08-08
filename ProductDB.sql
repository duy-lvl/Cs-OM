--DROP DATABASE PRN_ProductDB
USE master
--drop DATABASE PRN_ProductDB
go
CREATE DATABASE PRN_ProductDB
go
USE PRN_ProductDB

go

CREATE TABLE Category (
	ID int PRIMARY KEY,
	Name nvarchar(50),
	[Status] int
)

go

INSERT INTO Category(ID, Name, [Status]) 
VALUES (1, N'Rau', 1), (2, N'Thịt', 1), (3, N'Quần áo', 1)

go
CREATE TABLE Product(
	ID int PRIMARY KEY,
	Name nvarchar(50),
	Price float,
	CreatedDate date,
	CategoryID int REFERENCES Category(ID),
	[Status] int
)
INSERT INTO Product(ID, Name, Price, CreatedDate, CategoryID, [Status]) 
			VALUES  (1, N'Xà lách', 30000, '2022-06-12', 1, 1),
					(2, N'Thịt heo', 80000, '2022-07-01', 2, 1),
					(3, N'Thịt gà', 70000, '2022-07-10', 2, 1),
					(4, N'Áo thun', 150000, '2022-05-01', 3, 1)

go
CREATE TABLE [Order](
	ID int IDENTITY(1,1) PRIMARY KEY,
	CustomerName nvarchar(50),
	[Address] nvarchar(100),
	OrderDate DateTime,
	TotalAmount float,
	[Status] int,
	
)
INSERT INTO [Order](ID, CustomerName, [Address], OrderDate, TotalAmount, [Status]) 
			VALUES  ('Nguyen Van A', 'TPHCM', '2022-7-19', 100000, 1),
					('Nguyen Van B', 'Ha Noi', '2022-7-18', 150000, 1)
--select * from [Order]
go
CREATE TABLE Payment(
	ID int IDENTITY(1,1) PRIMARY KEY,
	PayTime datetime,
	Amount float,
	PayType varchar(50),
	OrderID int REFERENCES [Order](ID)
)

go
CREATE TABLE OrderDetail(
	ID int IDENTITY(1,1) PRIMARY KEY,
	OrderID int REFERENCES [Order](ID),
	ProductID int REFERENCES Product(ID),
	Quantity int,
	Price float
)

INSERT INTO OrderDetail(OrderID, ProductID, Quantity, Price) 
			VALUES  (1, 1, 1, 30000), 
					(1, 3, 1, 70000), 
					(2, 2, 1, 80000), 
					(2, 3, 1, 70000)

--

INSERT INTO Payment(PayTime, Amount, PayType, OrderID) 
			VALUES  ('2022-7-19 13:20:05', 70000, 'Online banking', 1),
					('2022-7-20 15:50:05', 70000, 'COD', 1),
					('2022-7-18 10:20:00', 150000, 'MoMo', 2)
--select * from OrderDetail
--select * from Payment
--select * from Product