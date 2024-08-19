CREATE PROCEDURE Cart_Crud
	@Action VARCHAR(20), 
	@ProductId INT = NULL,
	@Quantity INT = NULL,
	@UserId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- SELECT
	IF @Action = 'SELECT'
		BEGIN
			SELECT c.product_id, p.name, p.image_url, p.price, c.quantity AS cart_quantity, p.quantity AS product_quantity
			FROM dbo.carts c
			INNER JOIN dbo.products p ON p.product_id = c.product_id
			WHERE c.user_id = @UserId
		END
	-- INSERT
	IF @Action = 'INSERT'
		BEGIN
			INSERT INTO dbo.carts(product_id, quantity, user_id)
			VALUES(@ProductId, @Quantity, @UserId)
		END
	-- UPDATE
	IF @Action = 'UPDATE'
		BEGIN
			UPDATE dbo.carts
			SET quantity = @Quantity
			WHERE product_id = @ProductId AND user_id = @UserId
		END
	-- DELETE
	IF @Action = 'DELETE'
		BEGIN
			DELETE FROM dbo.carts
			WHERE product_id = @ProductId AND user_id = @UserId
		END
	-- GET BY USERID AND PRODUCT ID
	IF @Action = 'GETBYID'
		BEGIN
			SELECT * FROM dbo.carts
			WHERE product_id = @ProductId AND user_id = @UserId
		END

END