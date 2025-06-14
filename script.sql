USE [master]
GO
/****** Object:  Database [ReservatieService]    Script Date: 9/01/2023 18:18:54 ******/
CREATE DATABASE [ReservatieService]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ReservatieService', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ReservatieService.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ReservatieService_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ReservatieService_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [ReservatieService] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ReservatieService].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ReservatieService] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ReservatieService] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ReservatieService] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ReservatieService] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ReservatieService] SET ARITHABORT OFF 
GO
ALTER DATABASE [ReservatieService] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ReservatieService] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ReservatieService] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ReservatieService] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ReservatieService] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ReservatieService] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ReservatieService] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ReservatieService] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ReservatieService] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ReservatieService] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ReservatieService] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ReservatieService] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ReservatieService] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ReservatieService] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ReservatieService] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ReservatieService] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ReservatieService] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ReservatieService] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ReservatieService] SET  MULTI_USER 
GO
ALTER DATABASE [ReservatieService] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ReservatieService] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ReservatieService] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ReservatieService] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ReservatieService] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ReservatieService] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [ReservatieService] SET QUERY_STORE = OFF
GO
USE [ReservatieService]
GO
/****** Object:  Table [dbo].[Gebruiker]    Script Date: 9/01/2023 18:18:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gebruiker](
	[GebruikerId] [int] IDENTITY(1,1) NOT NULL,
	[Naam] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[Telefoonnummer] [varchar](255) NOT NULL,
	[LocatieId] [int] NOT NULL,
	[Is_visible] [int] NOT NULL,
 CONSTRAINT [PK_Gebruiker] PRIMARY KEY CLUSTERED 
(
	[GebruikerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locatie]    Script Date: 9/01/2023 18:18:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locatie](
	[LocatieId] [int] IDENTITY(1,1) NOT NULL,
	[Postcode] [int] NOT NULL,
	[Gemeente] [varchar](255) NOT NULL,
	[Straat] [varchar](255) NULL,
	[Huisnummer] [varchar](50) NULL,
	[Is_visible] [int] NOT NULL,
 CONSTRAINT [PK_Locatie] PRIMARY KEY CLUSTERED 
(
	[LocatieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reservatie]    Script Date: 9/01/2023 18:18:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservatie](
	[Reservatienummer] [int] IDENTITY(1,1) NOT NULL,
	[RestaurantId] [int] NOT NULL,
	[GebruikerId] [int] NOT NULL,
	[Tafelnummer] [int] NOT NULL,
	[Datum] [datetime] NOT NULL,
	[Uur] [datetime] NOT NULL,
	[Einduur] [datetime] NOT NULL,
	[AantalPlaatsen] [int] NOT NULL,
	[Is_visible] [int] NOT NULL,
 CONSTRAINT [PK_Reservatie_1] PRIMARY KEY CLUSTERED 
(
	[Reservatienummer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Restaurant]    Script Date: 9/01/2023 18:18:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Restaurant](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Naam] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[Telefoonnummer] [varchar](255) NOT NULL,
	[LocatieId] [int] NOT NULL,
	[Keuken] [varchar](255) NOT NULL,
	[Is_visible] [int] NOT NULL,
 CONSTRAINT [PK_Restaurant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tafel]    Script Date: 9/01/2023 18:18:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tafel](
	[Tafelnummer] [int] NOT NULL,
	[AantalPlaatsen] [int] NOT NULL,
	[RestaurantId] [int] NOT NULL,
	[Is_visible] [int] NOT NULL,
 CONSTRAINT [PK_Tafel] PRIMARY KEY CLUSTERED 
(
	[Tafelnummer] ASC,
	[RestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Gebruiker]  WITH CHECK ADD  CONSTRAINT [FK_Gebruiker_Locatie] FOREIGN KEY([LocatieId])
REFERENCES [dbo].[Locatie] ([LocatieId])
GO
ALTER TABLE [dbo].[Gebruiker] CHECK CONSTRAINT [FK_Gebruiker_Locatie]
GO
ALTER TABLE [dbo].[Reservatie]  WITH CHECK ADD  CONSTRAINT [FK_Reservatie_Gebruiker] FOREIGN KEY([GebruikerId])
REFERENCES [dbo].[Gebruiker] ([GebruikerId])
GO
ALTER TABLE [dbo].[Reservatie] CHECK CONSTRAINT [FK_Reservatie_Gebruiker]
GO
ALTER TABLE [dbo].[Reservatie]  WITH CHECK ADD  CONSTRAINT [FK_Reservatie_Restaurant] FOREIGN KEY([RestaurantId])
REFERENCES [dbo].[Restaurant] ([Id])
GO
ALTER TABLE [dbo].[Reservatie] CHECK CONSTRAINT [FK_Reservatie_Restaurant]
GO
ALTER TABLE [dbo].[Reservatie]  WITH CHECK ADD  CONSTRAINT [FK_Reservatie_Tafel] FOREIGN KEY([Tafelnummer], [RestaurantId])
REFERENCES [dbo].[Tafel] ([Tafelnummer], [RestaurantId])
GO
ALTER TABLE [dbo].[Reservatie] CHECK CONSTRAINT [FK_Reservatie_Tafel]
GO
ALTER TABLE [dbo].[Restaurant]  WITH CHECK ADD  CONSTRAINT [FK_Restaurant_Locatie] FOREIGN KEY([LocatieId])
REFERENCES [dbo].[Locatie] ([LocatieId])
GO
ALTER TABLE [dbo].[Restaurant] CHECK CONSTRAINT [FK_Restaurant_Locatie]
GO
ALTER TABLE [dbo].[Tafel]  WITH CHECK ADD  CONSTRAINT [FK_Tafel_Restaurant] FOREIGN KEY([RestaurantId])
REFERENCES [dbo].[Restaurant] ([Id])
GO
ALTER TABLE [dbo].[Tafel] CHECK CONSTRAINT [FK_Tafel_Restaurant]
GO
USE [master]
GO
ALTER DATABASE [ReservatieService] SET  READ_WRITE 
GO
