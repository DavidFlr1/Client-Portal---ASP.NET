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
