--Inserts en la tabla cients

INSERT INTO master.dbo.Clients (Email,Password,Name,IsAdmin) VALUES
	 (N'alejofraga220503@gmail.com',N'123456@Faf',N'Alejo Fraga',1),
	 (N'pepito@gmail.com',N'123456@Faf',N'pepito',0),
	 (N'robertito@gmail.com',N'123456@Faf',N'Roberto',0),
	 (N'sebavega2004@gmail.com',N'123456@Faf',N'Sebastian Vega',0);

--Inserts en la tabla Promotions

INSERT INTO master.dbo.Promotions (Tag,DiscountPercentage,StartDate,EndDate) VALUES
	 (N'Promo 1',10,'2024-06-07 00:00:00.0000000','2024-06-14 00:00:00.0000000'),
	 (N'Promo 2',17,'2024-06-07 00:00:00.0000000','2024-06-20 00:00:00.0000000'),
	 (N'Promo 3',20,'2024-06-07 00:00:00.0000000','2024-06-10 00:00:00.0000000'),
	 (N'Promo 4',50,'2024-06-07 00:00:00.0000000','2024-06-16 00:00:00.0000000'),
	 (N'Promo 5',50,'2024-06-19 00:00:00.0000000','2024-06-27 00:00:00.0000000'),
	 (N'Promo 6',55,'2024-06-19 00:00:00.0000000','2024-06-27 00:00:00.0000000'),
	 (N'Promo 7',60,'2024-06-19 00:00:00.0000000','2024-06-27 00:00:00.0000000'),
	 (N'Promo 8',65,'2024-06-19 00:00:00.0000000','2024-06-27 00:00:00.0000000'),
	 (N'Promo 9',70,'2024-06-19 00:00:00.0000000','2024-06-27 00:00:00.0000000'),
	 (N'Promo 10',75,'2024-06-19 00:00:00.0000000','2024-06-27 00:00:00.0000000');

--Inserts en la tabla Deposits

INSERT INTO master.dbo.Deposits (Name,Area,[Size],Conditioning) VALUES
	 (N'JUANCHO',N'A',N'Grande',0),
	 (N'OLIMPICO',N'B',N'Grande',0),
	 (N'GRAN DEPO',N'C',N'Grande',1),
	 (N'EL HANGAR',N'E',N'Pequeño',1),
	 (N'EL REVOLTIJO',N'D',N'Mediano',0),
	 (N'BEST DEPO',N'B',N'Grande',1),
	 (N'EL PEQUE',N'D',N'Pequeño',0);

--Inserts en la tabla DateRanges

INSERT INTO master.dbo.DateRanges (DepositId,StartDate,EndDate) VALUES
	 (7,'2024-06-20 00:00:00.0000000','2024-06-21 00:00:00.0000000'),
	 (7,'2024-06-22 00:00:00.0000000','2024-06-25 00:00:00.0000000'),
	 (1,'2024-08-15 00:00:00.0000000','2024-08-29 00:00:00.0000000'),
	 (1,'2024-11-20 00:00:00.0000000','2025-01-10 00:00:00.0000000'),
	 (7,'2024-08-15 00:00:00.0000000','2024-08-29 00:00:00.0000000'),
	 (7,'2025-01-08 00:00:00.0000000','2025-01-10 00:00:00.0000000'),
	 (3,'2024-06-14 00:00:00.0000000','2024-06-17 00:00:00.0000000'),
	 (4,'2024-06-14 00:00:00.0000000','2024-06-17 00:00:00.0000000'),
	 (2,'2024-06-14 00:00:00.0000000','2024-06-17 00:00:00.0000000'),
	 (5,'2024-06-14 00:00:00.0000000','2024-06-17 00:00:00.0000000');
INSERT INTO master.dbo.DateRanges (DepositId,StartDate,EndDate) VALUES
	 (6,'2024-06-07 00:00:00.0000000','2024-06-10 00:00:00.0000000'),
	 (6,'2024-06-12 00:00:00.0000000','2024-06-17 00:00:00.0000000');



--Inserts en la tabla DepositPromotion

INSERT INTO master.dbo.DepositPromotion (DepositId,PromotionId) VALUES
	 (1,1),
	 (7,1),
	 (2,2),
	 (3,2),
	 (7,2),
	 (2,3),
	 (3,3),
	 (7,3),
	 (4,4),
	 (5,4);
INSERT INTO master.dbo.DepositPromotion (DepositId,PromotionId) VALUES
	 (7,4),
	 (4,5),
	 (5,5),
	 (7,5),
	 (7,6),
	 (7,7),
	 (6,9),
	 (5,10);


--Inserts en la tabla Reservations


INSERT INTO master.dbo.Reservations (RejectedMessage,IsReviewed,StartDate,EndDate,IsConfirmed,IsRejected,DepositId,ClientEmail,PaymentStatus) VALUES
	 (NULL,0,'2024-06-07 00:00:00.0000000','2024-06-10 00:00:00.0000000',0,0,1,N'alejofraga220503@gmail.com',1),
	 (NULL,0,'2024-06-11 00:00:00.0000000','2024-06-14 00:00:00.0000000',0,0,1,N'alejofraga220503@gmail.com',1),
	 (NULL,0,'2024-06-07 00:00:00.0000000','2024-06-14 00:00:00.0000000',0,0,1,N'sebavega2004@gmail.com',1),
	 (NULL,0,'2024-06-07 00:00:00.0000000','2024-06-17 00:00:00.0000000',0,0,3,N'alejofraga220503@gmail.com',1),
	 (NULL,0,'2024-06-07 00:00:00.0000000','2024-06-17 00:00:00.0000000',0,0,4,N'alejofraga220503@gmail.com',1),
	 (NULL,0,'2024-06-07 00:00:00.0000000','2024-06-10 00:00:00.0000000',0,0,5,N'alejofraga220503@gmail.com',1),
	 (NULL,0,'2024-06-07 00:00:00.0000000','2024-06-12 00:00:00.0000000',0,0,6,N'alejofraga220503@gmail.com',1),
	 (NULL,0,'2024-06-07 00:00:00.0000000','2024-06-11 00:00:00.0000000',0,0,2,N'sebavega2004@gmail.com',1),
	 (NULL,0,'2024-06-12 00:00:00.0000000','2024-06-13 00:00:00.0000000',0,0,2,N'sebavega2004@gmail.com',1),
	 (NULL,0,'2024-06-14 00:00:00.0000000','2024-06-17 00:00:00.0000000',0,0,5,N'sebavega2004@gmail.com',1);
INSERT INTO master.dbo.Reservations (RejectedMessage,IsReviewed,StartDate,EndDate,IsConfirmed,IsRejected,DepositId,ClientEmail,PaymentStatus) VALUES
	 (NULL,0,'2024-06-14 00:00:00.0000000','2024-06-17 00:00:00.0000000',0,0,6,N'sebavega2004@gmail.com',1),
	 (NULL,0,'2024-06-10 00:00:00.0000000','2024-06-12 00:00:00.0000000',1,0,6,N'sebavega2004@gmail.com',2),
	 (NULL,0,'2024-06-07 00:00:00.0000000','2024-06-14 00:00:00.0000000',1,0,1,N'pepito@gmail.com',2),
	 (NULL,0,'2024-06-07 00:00:00.0000000','2024-06-14 00:00:00.0000000',1,0,2,N'pepito@gmail.com',2),
	 (NULL,0,'2024-06-07 00:00:00.0000000','2024-06-14 00:00:00.0000000',1,0,3,N'pepito@gmail.com',2),
	 (NULL,0,'2024-06-07 00:00:00.0000000','2024-06-14 00:00:00.0000000',1,0,4,N'pepito@gmail.com',2),
	 (NULL,0,'2024-06-07 00:00:00.0000000','2024-06-14 00:00:00.0000000',1,0,5,N'pepito@gmail.com',2),
	 (NULL,0,'2024-06-07 00:00:00.0000000','2024-06-14 00:00:00.0000000',0,0,6,N'pepito@gmail.com',1),
	 (NULL,0,'2024-06-21 00:00:00.0000000','2024-06-22 00:00:00.0000000',1,0,7,N'robertito@gmail.com',2),
	 (NULL,0,'2024-06-25 00:00:00.0000000','2024-06-30 00:00:00.0000000',1,0,7,N'robertito@gmail.com',2);
INSERT INTO master.dbo.Reservations (RejectedMessage,IsReviewed,StartDate,EndDate,IsConfirmed,IsRejected,DepositId,ClientEmail,PaymentStatus) VALUES
	 (NULL,0,'2024-08-29 00:00:00.0000000','2024-11-20 00:00:00.0000000',1,0,1,N'robertito@gmail.com',2),
	 (NULL,0,'2024-08-29 00:00:00.0000000','2025-01-08 00:00:00.0000000',1,0,7,N'robertito@gmail.com',2);


--Inserts en la tabla Actions

INSERT INTO master.dbo.Actions (Description,ClientEmail) VALUES
	 (N'6/7/2024 5:10:19 PM: Inicio de sesión',N'alejofraga220503@gmail.com'),
	 (N'6/7/2024 5:16:12 PM: Inicio de sesión',N'alejofraga220503@gmail.com'),
	 (N'6/7/2024 5:22:49 PM: Cierre de sesión',N'alejofraga220503@gmail.com'),
	 (N'6/7/2024 5:23:32 PM: Inicio de sesión',N'sebavega2004@gmail.com'),
	 (N'6/7/2024 5:23:53 PM: Cierre de sesión',N'sebavega2004@gmail.com'),
	 (N'6/7/2024 5:24:00 PM: Inicio de sesión',N'alejofraga220503@gmail.com'),
	 (N'6/7/2024 5:26:17 PM: Cierre de sesión',N'alejofraga220503@gmail.com'),
	 (N'6/7/2024 5:26:27 PM: Inicio de sesión',N'sebavega2004@gmail.com'),
	 (N'6/7/2024 5:28:19 PM: Cierre de sesión',N'sebavega2004@gmail.com'),
	 (N'6/7/2024 5:29:06 PM: Inicio de sesión',N'pepito@gmail.com');
INSERT INTO master.dbo.Actions (Description,ClientEmail) VALUES
	 (N'6/7/2024 5:30:11 PM: Cierre de sesión',N'pepito@gmail.com'),
	 (N'6/7/2024 5:30:22 PM: Inicio de sesión',N'alejofraga220503@gmail.com'),
	 (N'6/7/2024 5:31:18 PM: Cierre de sesión',N'alejofraga220503@gmail.com'),
	 (N'6/7/2024 5:31:50 PM: Inicio de sesión',N'robertito@gmail.com'),
	 (N'6/7/2024 5:31:58 PM: Cierre de sesión',N'robertito@gmail.com'),
	 (N'6/7/2024 5:32:10 PM: Inicio de sesión',N'alejofraga220503@gmail.com'),
	 (N'6/7/2024 5:33:32 PM: Cierre de sesión',N'alejofraga220503@gmail.com'),
	 (N'6/7/2024 5:33:43 PM: Inicio de sesión',N'robertito@gmail.com'),
	 (N'6/7/2024 5:38:39 PM: Cierre de sesión',N'robertito@gmail.com'),
	 (N'6/7/2024 5:38:45 PM: Inicio de sesión',N'alejofraga220503@gmail.com');
INSERT INTO master.dbo.Actions (Description,ClientEmail) VALUES
	 (N'6/7/2024 5:48:53 PM: Cierre de sesión',N'alejofraga220503@gmail.com'),
	 (N'6/7/2024 5:49:02 PM: Inicio de sesión',N'sebavega2004@gmail.com'),
	 (N'6/7/2024 5:49:10 PM: Cierre de sesión',N'sebavega2004@gmail.com'),
	 (N'6/7/2024 5:49:18 PM: Inicio de sesión',N'pepito@gmail.com'),
	 (N'6/7/2024 5:50:49 PM: Cierre de sesión',N'pepito@gmail.com');

--Inserts en la tabla Notifications

INSERT INTO master.dbo.Notifications (ClientEmail,NotificationType,DepositName,StartDate,EndDate) VALUES
	 (N'robertito@gmail.com',0,N'EL PEQUE','2024-06-21 00:00:00.0000000','2024-06-22 00:00:00.0000000'),
	 (N'robertito@gmail.com',0,N'EL PEQUE','2024-06-25 00:00:00.0000000','2024-06-30 00:00:00.0000000'),
	 (N'robertito@gmail.com',0,N'JUANCHO','2024-08-29 00:00:00.0000000','2024-11-20 00:00:00.0000000'),
	 (N'robertito@gmail.com',0,N'EL PEQUE','2024-08-29 00:00:00.0000000','2025-01-08 00:00:00.0000000'),
	 (N'pepito@gmail.com',0,N'GRAN DEPO','2024-06-07 00:00:00.0000000','2024-06-14 00:00:00.0000000'),
	 (N'pepito@gmail.com',0,N'EL HANGAR','2024-06-07 00:00:00.0000000','2024-06-14 00:00:00.0000000'),
	 (N'pepito@gmail.com',0,N'JUANCHO','2024-06-07 00:00:00.0000000','2024-06-14 00:00:00.0000000'),
	 (N'pepito@gmail.com',0,N'OLIMPICO','2024-06-07 00:00:00.0000000','2024-06-14 00:00:00.0000000'),
	 (N'pepito@gmail.com',0,N'EL REVOLTIJO','2024-06-07 00:00:00.0000000','2024-06-14 00:00:00.0000000'),
	 (N'sebavega2004@gmail.com',0,N'BEST DEPO','2024-06-10 00:00:00.0000000','2024-06-12 00:00:00.0000000');


--Insert de migrations history

INSERT INTO master.dbo.[__EFMigrationsHistory] (MigrationId,ProductVersion) VALUES
	 (N'20240608003123_setup',N'7.0.9');
