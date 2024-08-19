
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE Category_Crud
	-- Add the parameters for the stored procedure here
	@Action VARCHAR(20),
	@CategoryId INT = NULL,
	@Name VARCHAR(100) = NULL,
	@IsActive BIT = false,
	@ImageUrl VARCHAR(MAX) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- SELECT
	IF @Action = 'SELECT'
		BEGIN
			SELECT * FROM dbo.categories ORDER BY created_date DESC
		END
	-- SELECT ONLY ACTIVE
	IF @Action = 'SELECTACTIVE'
		BEGIN
			SELECT * FROM dbo.categories WHERE is_active = 1
		END
	-- INSERT
	IF @Action = 'INSERT'
		BEGIN
			INSERT INTO dbo.categories(name, image_url, is_active, created_date)
			VALUES (@Name, @ImageUrl, @IsActive, GETDATE())
		END
	-- UPDATE
	IF @Action = 'UPDATE'
		BEGIN
			DECLARE @UPDATE_IMAGE VARCHAR(20)
			SELECT @UPDATE_IMAGE = (CASE WHEN @ImageUrl IS NULL THEN 'NO' ELSE 'YES' END)
			IF @UPDATE_IMAGE = 'NO'
				BEGIN
					UPDATE dbo.categories
					SET name = @Name, is_active = @IsActive
					WHERE category_id = @CategoryId
				END
			ELSE
				BEGIN
					UPDATE dbo.categories
					SET name = @Name, image_url = @ImageUrl, is_active = @IsActive
					WHERE category_id = @CategoryId
				END
		END
	-- DELETE
	IF @Action = 'DELETE'
		BEGIN
			DELETE FROM dbo.categories WHERE category_id = @CategoryId
		END
	-- GETBYID
	IF @Action = 'GETBYID'
		BEGIN
			SELECT * FROM dbo.categories WHERE category_id = @CategoryId
		END

END
GO
