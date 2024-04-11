DROP VIEW IF EXISTS dbo.v_OrderInfo_Top_10
GO 

CREATE VIEW dbo.v_OrderInfo_Top_10 AS
SELECT TOP 10 o.Code, p.Name, p.Quantity, o.BillingAddress_City AS City, o.BillingAddress_Country as Country, o.BillingAddress_Postcode as Postcode, o.BillingAddress_Street AS Street 
	FROM Orders o
	LEFT JOIN Products p ON o.ProductId = p.Id
GO