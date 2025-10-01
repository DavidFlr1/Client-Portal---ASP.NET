-- 1. Users
CREATE TABLE Users (
    Id              INT IDENTITY PRIMARY KEY,
    FullName        NVARCHAR(100) NOT NULL,
    Email           NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash    NVARCHAR(255) NOT NULL,
    Role            NVARCHAR(20) DEFAULT 'Client',
    CreatedAt       DATETIME DEFAULT GETDATE()
);

-- 2. ServiceRequests
CREATE TABLE ServiceRequests (
    Id              INT IDENTITY PRIMARY KEY,
    UserId          INT NOT NULL,
    Title           NVARCHAR(150),
    Description     NVARCHAR(MAX),
    Status          NVARCHAR(20) DEFAULT 'Pending',
    CreatedAt       DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- 3. Payments
CREATE TABLE Payments (
    Id              INT IDENTITY PRIMARY KEY,
    UserId          INT NOT NULL,
    RequestId       INT NULL,
    Amount          DECIMAL(10, 2),
    Currency        NVARCHAR(10) DEFAULT 'USD',
    Status          NVARCHAR(20) DEFAULT 'Pending',
    TransactionId   NVARCHAR(150),
    CreatedAt       DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (RequestId) REFERENCES ServiceRequests(Id)
);
