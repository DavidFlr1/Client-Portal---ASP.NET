-- Insert User
CREATE PROCEDURE sp_insertUser
  @UserName NVARCHAR(256),
  @FirstName NVARCHAR(100),
  @LastName NVARCHAR(100),
  @Email NVARCHAR(100),
  @PasswordHash NVARCHAR(MAX),
  @Role NVARCHAR(20)
AS
BEGIN
  DECLARE @UserId NVARCHAR(450) = NEWID()
  
  INSERT INTO AspNetUsers (
    Id, UserName, NormalizedUserName, Email, NormalizedEmail, 
    EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,
    FirstName, LastName, FullName, Role, IsActive, CreatedAt,
    AccessFailedCount, LockoutEnabled, TwoFactorEnabled, PhoneNumberConfirmed
  )
  VALUES (
    @UserId, @UserName, UPPER(@UserName), @Email, UPPER(@Email),
    0, @PasswordHash, NEWID(), NEWID(),
    @FirstName, @LastName, @FirstName + ' ' + @LastName, @Role, 1, GETUTCDATE(),
    0, 1, 0, 0
  )
END
GO

-- Crear un request
CREATE PROCEDURE sp_CreateServiceRequest
    @UserId INT,
    @Title NVARCHAR(150),
    @Description NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO ServiceRequests (UserId, Title, Description)
    VALUES (@UserId, @Title, @Description)
END
GO

-- Obtener requests por usuario
CREATE PROCEDURE sp_GetRequestsByUser
    @UserId INT
AS
BEGIN
    SELECT * FROM ServiceRequests WHERE UserId = @UserId
END
GO

-- Registrar pago
CREATE PROCEDURE sp_LogPayment
    @UserId INT,
    @RequestId INT,
    @Amount DECIMAL(10,2),
    @TransactionId NVARCHAR(150)
AS
BEGIN
    INSERT INTO Payments (UserId, RequestId, Amount, TransactionId, Status)
    VALUES (@UserId, @RequestId, @Amount, @TransactionId, 'Completed')
END
GO
