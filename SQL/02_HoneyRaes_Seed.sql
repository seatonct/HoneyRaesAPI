\c HoneyRaes

INSERT INTO Customer (Name, Address) VALUES ('Val Freeman', '128 React Router Dom Dr.');
INSERT INTO Customer (Name, Address) VALUES ('Steve Brownlee', '12345 Shark Attack Way');
INSERT INTO Customer (Name, Address) VALUES ('Jeffrey Lebowski', '12 Golfer Ct.');

INSERT INTO Employee (Name, Specialty) VALUES ('Jack Handy', 'Changin tires');
INSERT INTO Employee (Name, Specialty) VALUES ('Elvin Johnson', 'navigation systems');

INSERT INTO ServiceTicket (CustomerId, EmployeeId, Description, Emergency, DateCompleted) VALUES (1, 1, 'dropped in toilet', true, '2023-12-30T00:00:00');
INSERT INTO ServiceTicket (CustomerId, EmployeeId, Description, Emergency, DateCompleted) VALUES (2, 1, 'cheese whiz in charging port', false, '2024-01-02T00:00:00');
INSERT INTO ServiceTicket (CustomerId, EmployeeId, Description) VALUES (3, 2, 'cracked screen');
INSERT INTO ServiceTicket (CustomerId, Description) VALUES (1, 'toilet again');
INSERT INTO ServiceTicket (CustomerId, Description, Emergency) VALUES (2, 'more cheese whiz, now in headphone jack', true);
