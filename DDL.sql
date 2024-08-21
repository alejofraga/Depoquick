-- master.dbo.Clients definition

-- Drop table

-- DROP TABLE master.dbo.Clients;

CREATE TABLE master.dbo.Clients (
	Email nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	IsAdmin bit NOT NULL,
	CONSTRAINT PK_Clients PRIMARY KEY (Email)
);


-- master.dbo.Deposits definition

-- Drop table

-- DROP TABLE master.dbo.Deposits;

CREATE TABLE master.dbo.Deposits (
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Area nvarchar(1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Size] nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Conditioning bit NOT NULL,
	CONSTRAINT PK_Deposits PRIMARY KEY (Id)
);
 CREATE  UNIQUE NONCLUSTERED INDEX IX_Deposits_Name ON dbo.Deposits (  Name ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- master.dbo.Promotions definition

-- Drop table

-- DROP TABLE master.dbo.Promotions;

CREATE TABLE master.dbo.Promotions (
	Id int IDENTITY(1,1) NOT NULL,
	Tag nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	DiscountPercentage int NOT NULL,
	StartDate datetime2 NOT NULL,
	EndDate datetime2 NOT NULL,
	CONSTRAINT PK_Promotions PRIMARY KEY (Id)
);


-- master.dbo.[__EFMigrationsHistory] definition

-- Drop table

-- DROP TABLE master.dbo.[__EFMigrationsHistory];

CREATE TABLE master.dbo.[__EFMigrationsHistory] (
	MigrationId nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductVersion nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);


-- master.dbo.Actions definition

-- Drop table

-- DROP TABLE master.dbo.Actions;

CREATE TABLE master.dbo.Actions (
	Id int IDENTITY(1,1) NOT NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ClientEmail nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_Actions PRIMARY KEY (Id),
	CONSTRAINT FK_Actions_Clients_ClientEmail FOREIGN KEY (ClientEmail) REFERENCES master.dbo.Clients(Email) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Actions_ClientEmail ON dbo.Actions (  ClientEmail ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- master.dbo.DateRanges definition

-- Drop table

-- DROP TABLE master.dbo.DateRanges;

CREATE TABLE master.dbo.DateRanges (
	Id int IDENTITY(1,1) NOT NULL,
	DepositId int NOT NULL,
	StartDate datetime2 NOT NULL,
	EndDate datetime2 NOT NULL,
	CONSTRAINT PK_DateRanges PRIMARY KEY (Id),
	CONSTRAINT FK_DateRanges_Deposits_DepositId FOREIGN KEY (DepositId) REFERENCES master.dbo.Deposits(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_DateRanges_DepositId ON dbo.DateRanges (  DepositId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- master.dbo.DepositPromotion definition

-- Drop table

-- DROP TABLE master.dbo.DepositPromotion;

CREATE TABLE master.dbo.DepositPromotion (
	DepositId int NOT NULL,
	PromotionId int NOT NULL,
	CONSTRAINT PK_DepositPromotion PRIMARY KEY (DepositId,PromotionId),
	CONSTRAINT FK_DepositPromotion_Deposits_DepositId FOREIGN KEY (DepositId) REFERENCES master.dbo.Deposits(Id) ON DELETE CASCADE,
	CONSTRAINT FK_DepositPromotion_Promotions_PromotionId FOREIGN KEY (PromotionId) REFERENCES master.dbo.Promotions(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_DepositPromotion_PromotionId ON dbo.DepositPromotion (  PromotionId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- master.dbo.Notifications definition

-- Drop table

-- DROP TABLE master.dbo.Notifications;

CREATE TABLE master.dbo.Notifications (
	Id int IDENTITY(1,1) NOT NULL,
	ClientEmail nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	NotificationType int NOT NULL,
	DepositName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	StartDate datetime2 NOT NULL,
	EndDate datetime2 NOT NULL,
	CONSTRAINT PK_Notifications PRIMARY KEY (Id),
	CONSTRAINT FK_Notifications_Clients_ClientEmail FOREIGN KEY (ClientEmail) REFERENCES master.dbo.Clients(Email) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Notifications_ClientEmail ON dbo.Notifications (  ClientEmail ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- master.dbo.Reservations definition

-- Drop table

-- DROP TABLE master.dbo.Reservations;

CREATE TABLE master.dbo.Reservations (
	Id int IDENTITY(1,1) NOT NULL,
	RejectedMessage nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	IsReviewed bit NOT NULL,
	StartDate datetime2 NOT NULL,
	EndDate datetime2 NOT NULL,
	IsConfirmed bit NOT NULL,
	IsRejected bit NOT NULL,
	DepositId int NOT NULL,
	ClientEmail nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	PaymentStatus int NOT NULL,
	CONSTRAINT PK_Reservations PRIMARY KEY (Id),
	CONSTRAINT FK_Reservations_Clients_ClientEmail FOREIGN KEY (ClientEmail) REFERENCES master.dbo.Clients(Email) ON DELETE CASCADE,
	CONSTRAINT FK_Reservations_Deposits_DepositId FOREIGN KEY (DepositId) REFERENCES master.dbo.Deposits(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Reservations_ClientEmail ON dbo.Reservations (  ClientEmail ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Reservations_DepositId ON dbo.Reservations (  DepositId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- master.dbo.Reviews definition

-- Drop table

-- DROP TABLE master.dbo.Reviews;

CREATE TABLE master.dbo.Reviews (
	Id int IDENTITY(1,1) NOT NULL,
	Valoration int NOT NULL,
	DepositId int NOT NULL,
	Comment nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_Reviews PRIMARY KEY (Id),
	CONSTRAINT FK_Reviews_Deposits_DepositId FOREIGN KEY (DepositId) REFERENCES master.dbo.Deposits(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Reviews_DepositId ON dbo.Reviews (  DepositId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;