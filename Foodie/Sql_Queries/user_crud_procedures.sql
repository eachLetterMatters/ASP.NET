CREATE PROCEDURE [dbo].[User_Crud]
	@Action VARCHAR(20),
	@UserId INT = NULL,
	@Name VARCHAR(50) = NULL,
	@Username VARCHAR(50) = NULL,
	@Mobile VARCHAR(50) = NULL,
	@Email VARCHAR(50) = NULL,
	@Address VARCHAR(MAX) = NULL,
	@PostCode VARCHAR(50) = NULL,
	@Password VARCHAR(50) = NULL,
	@ImageUrl VARCHAR(MAX) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	-- SELECT FOR LOGIN
	IF @Action = 'SELECT4LOGIN'
		BEGIN
			SELECT * FROM dbo.users WHERE username = @Username AND password = @Password
		END
	-- SELECT FOR USERS PROFILE
	IF @Action = 'SELECT4PROFILE'
		BEGIN
			SELECT * FROM dbo.users WHERE user_id = @UserId
		END
	-- INSERT (REGISTRATION)
	IF @Action = 'INSERT'
		BEGIN
			INSERT INTO dbo.users(name, username, mobile, email, address, post_code, password, image_url, created_date)
			VALUES (@Name, @Username, @Mobile, @Email, @Address, @PostCode, @Password, @ImageUrl, GETDATE())
		END
	-- UPDATE USER PROFILE
	IF @Action = 'UPDATE'
		BEGIN
			DECLARE @UPDATE_IMAGE VARCHAR(20)
			SELECT @UPDATE_IMAGE = (CASE WHEN @ImageUrl IS NULL THEN 'NO' ELSE 'YES' END)
			IF @UPDATE_IMAGE = 'NO'
				BEGIN
					UPDATE dbo.users
					SET name = @Name, username = @Username, mobile = @Mobile, email = @Email, address = @Address, post_code = @Postcode
					WHERE user_id = @UserId
				END
			ELSE
				BEGIN
					UPDATE dbo.users
					SET name = @Name, username = @Username, mobile = @Mobile, email = @Email, address = @Address, post_code = @Postcode,
					image_url = @ImageUrl
					WHERE user_id = @UserId
				END
		END
	-- SELECT FOR ADMIN
	IF @Action = 'SELECT4ADMIN'
		BEGIN
			SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [SrNo], user_id, name, username, email, created_date
			FROM users
		END
	-- DELETE BY ADMIN
	IF @Action = 'DELETE'
		BEGIN
			DELETE FROM dbo.users WHERE user_id = @UserId
		END
END
GO