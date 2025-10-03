EXEC sp_insertUser
  @FirstName = 'David',
  @LastName = 'Flores',
  @UserName = 'dflores',
  @Email = 'davidricardo.flores33@gmail.com',
  @PasswordHash = 'admin123!',
  @Role = 'admin';


SELECT UserName, Role, Email, IsActive FROM AspNetUsers;


INSERT INTO Clients (Name, Email, Phone) VALUES
('Acme Corp', 'contact@acme.com', '123-456-7890'),
('Beta LLC', 'info@beta.com', '555-555-5555');

INSERT INTO Projects (ClientId, Name, Description, Status, StartDate)
VALUES
(1, 'Website Redesign', 'Redesign corporate website', 'In Progress', '2025-01-01'),
(2, 'Payment Gateway Integration', 'Implement Stripe payments', 'Pending', '2025-02-15');
