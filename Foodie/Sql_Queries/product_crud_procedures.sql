CREATE PROCEDURE [dbo].[Product_Crud]
	@Action VARCHAR(10),
	@ProductId INT = NULL,
	@Name VARCHAR(100) = NULL,
	@Description VARCHAR(MAX) = NULL,
	@Price DECIMAL(18,2) = 0,
	@Quantity INT = NULL,
	@ImageUrl VARCHAR(MAX) = NULL,
	@CategoryId INT = NULL,
	@IsActive BIT = false
AS
BEGIN
	SET NOCOUNT ON;
	-- SELECT
	IF @Action = 'SELECT'
		BEGIN
			SELECT p.*,c.name AS category_name FROM dbo.products p
			INNER JOIN dbo.categories c ON c.category_id = p.category_id ORDER BY p.created_date DESC
		END
	-- SELECT
	IF @Action = 'SELECTACTIVE'
		BEGIN
			SELECT p.*,c.name AS category_name FROM dbo.products p
			INNER JOIN dbo.categories c ON c.category_id = p.category_id
			WHERE p.is_active = 1
		END
	-- INSERT
	IF @Action = 'INSERT'
		BEGIN
			INSERT INTO dbo.products(name, description, price, quantity, image_url, category_id, is_active, created_date)
			VALUES (@Name, @Description, @Price, @Quantity, @ImageUrl, @CategoryId, @IsActive, GETDATE())
		END
	-- UPDATE
	IF @Action = 'UPDATE'
		BEGIN
			DECLARE @UPDATE_IMAGE VARCHAR(20)
			SELECT @UPDATE_IMAGE = (CASE WHEN @ImageUrl IS NULL THEN 'NO' ELSE 'YES' END)
			IF @UPDATE_IMAGE = 'NO'
				BEGIN
					UPDATE dbo.products
					SET name = @Name, description = @Description, price = @Price, quantity = @Quantity,
					category_id = @CategoryId, is_active = @IsActive
					WHERE product_id = @ProductId
				END
			ELSE
				BEGIN
					UPDATE dbo.products
					SET name = @Name, description = @Description, price = @Price, quantity = @Quantity,
					image_url = @ImageUrl, category_id = @CategoryId, is_active = @IsActive
					WHERE product_id = @ProductId
				END
		END
	-- UPDATE QUANTITY
	IF @Action = 'QTYUPDATE'
		BEGIN
			UPDATE dbo.products 
			SET quantity = @Quantity
			WHERE product_id = @ProductId
		END
	--DELETE
	IF @Action = 'DELETE'
		BEGIN
			DELETE FROM dbo.products WHERE product_id = @ProductId
		END
	--GETBYID
	IF @Action = 'GETBYID'
		BEGIN
			SELECT * FROM dbo.products WHERE product_id = @ProductId
		END
END