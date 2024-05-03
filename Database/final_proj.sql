USE [master]
GO
/****** Object:  Database [DBAuctioningWeb]    Script Date: 6/7/2020 11:29:29 PM ******/
CREATE DATABASE [DBAuctioningWeb]
GO
ALTER DATABASE [DBAuctioningWeb] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DBAuctioningWeb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DBAuctioningWeb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET ARITHABORT OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DBAuctioningWeb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DBAuctioningWeb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DBAuctioningWeb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DBAuctioningWeb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET RECOVERY FULL 
GO
ALTER DATABASE [DBAuctioningWeb] SET  MULTI_USER 
GO
ALTER DATABASE [DBAuctioningWeb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DBAuctioningWeb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DBAuctioningWeb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DBAuctioningWeb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DBAuctioningWeb] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'DBAuctioningWeb', N'ON'
GO
ALTER DATABASE [DBAuctioningWeb] SET QUERY_STORE = OFF
GO
USE [DBAuctioningWeb]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 6/7/2020 11:29:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_role] [int] NOT NULL,
	[username] [varchar](250) NOT NULL,
	[password] [varchar](250) NOT NULL,
	[phone] [varchar](250) NOT NULL,
	[isActive] [bit] NOT NULL,
	[avatar] [varchar](250) NULL,
	[point] [float] NULL,
	[email] [varchar](250) NULL,
	[paypal_account] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Auction]    Script Date: 6/7/2020 11:29:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Auction](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_category] [int] NOT NULL,
	[id_account] [int] NOT NULL,
	[auction_name] [varchar](250) NOT NULL,
	[auction_description] [ntext] NOT NULL,
	[auction_image] [varchar](250) NULL,
	[auction_file] [varchar](250) NULL,
	[auction_minimum_bid] [float] NOT NULL,
	[auction_gap_bid] [float] NULL,
	[auction_bid_increment] [float] NULL,
	[auction_start_date] [datetime] NOT NULL,
	[auction_end_date] [datetime] NOT NULL,
	[auction_isCompleted] [bit] NULL,
	[id_buyer_account] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bid_history]    Script Date: 6/7/2020 11:29:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bid_history](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_auction] [int] NOT NULL,
	[bid] [float] NOT NULL,
	[bidding_time] [date] NOT NULL,
	[id_account] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bill]    Script Date: 6/7/2020 11:29:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bill](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_account] [int] NOT NULL,
	[id_auction] [int] NOT NULL,
	[final_bid] [float] NOT NULL,
	[payment_method] [varchar](250) NULL,
	[isActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 6/7/2020 11:29:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[category_name] [varchar](250) NOT NULL,
	[avatar] [varchar](250) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 6/7/2020 11:29:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[role_name] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([id], [id_role], [username], [password], [phone], [isActive], [avatar], [point], [email], [paypal_account]) VALUES (1, 1, N'JohnDoe', N'123', N'09123123', 1, N'default.png', 0, N'huynguyen111204@gmail.com', NULL)
INSERT [dbo].[Account] ([id], [id_role], [username], [password], [phone], [isActive], [avatar], [point], [email], [paypal_account]) VALUES (4, 2, N'Carrick', N'MCarrick', N'090-123-0909', 1, N'default.png', NULL, N'asdf123@gmail.com', NULL)
SET IDENTITY_INSERT [dbo].[Account] OFF
SET IDENTITY_INSERT [dbo].[Auction] ON 

INSERT [dbo].[Auction] ([id], [id_category], [id_account], [auction_name], [auction_description], [auction_image], [auction_file], [auction_minimum_bid], [auction_gap_bid], [auction_bid_increment], [auction_start_date], [auction_end_date], [auction_isCompleted], [id_buyer_account]) VALUES (38, 1, 1, N'Ferrari Enzo', N'This is really super car', N'SuperCar.jpg', NULL, 200, 200, NULL, CAST(N'2020-06-07T19:57:00.000' AS DateTime), CAST(N'2020-06-07T20:00:00.000' AS DateTime), 0, NULL)
INSERT [dbo].[Auction] ([id], [id_category], [id_account], [auction_name], [auction_description], [auction_image], [auction_file], [auction_minimum_bid], [auction_gap_bid], [auction_bid_increment], [auction_start_date], [auction_end_date], [auction_isCompleted], [id_buyer_account]) VALUES (39, 2, 1, N'Kusanagi Sword', N'Old sword of the Japanese', N'Kusanagi.jpg', NULL, 200, 200, NULL, CAST(N'2020-06-08T07:00:00.000' AS DateTime), CAST(N'2020-06-08T08:00:00.000' AS DateTime), 0, NULL)
SET IDENTITY_INSERT [dbo].[Auction] OFF
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([id], [category_name], [avatar]) VALUES (1, N'Vehicle', N'vehicle.jpg')
INSERT [dbo].[Category] ([id], [category_name], [avatar]) VALUES (2, N'Antique', N'antique.jpg')
INSERT [dbo].[Category] ([id], [category_name], [avatar]) VALUES (5, N'Art', N'art.jpg')
SET IDENTITY_INSERT [dbo].[Category] OFF
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([id], [role_name]) VALUES (1, N'ADMIN')
INSERT [dbo].[Role] ([id], [role_name]) VALUES (2, N'USER')
SET IDENTITY_INSERT [dbo].[Role] OFF
ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([id_role])
REFERENCES [dbo].[Role] ([id])
GO
ALTER TABLE [dbo].[Auction]  WITH CHECK ADD FOREIGN KEY([id_account])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[Auction]  WITH CHECK ADD FOREIGN KEY([id_category])
REFERENCES [dbo].[Category] ([id])
GO
ALTER TABLE [dbo].[Bid_history]  WITH CHECK ADD FOREIGN KEY([id_auction])
REFERENCES [dbo].[Auction] ([id])
GO
ALTER TABLE [dbo].[Bid_history]  WITH CHECK ADD  CONSTRAINT [FK_Bid_history_Account] FOREIGN KEY([id_account])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[Bid_history] CHECK CONSTRAINT [FK_Bid_history_Account]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([id_account])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([id_auction])
REFERENCES [dbo].[Auction] ([id])
GO
USE [master]
GO
ALTER DATABASE [DBAuctioningWeb] SET  READ_WRITE 
GO
