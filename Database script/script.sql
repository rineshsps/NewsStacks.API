USE [master]
GO
/****** Object:  Database [news]    Script Date: 06-03-2021 06:39:11 PM ******/
CREATE DATABASE [news]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'news', FILENAME = N'C:\Users\AL2432\news.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'news_log', FILENAME = N'C:\Users\AL2432\news_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [news] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [news].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [news] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [news] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [news] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [news] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [news] SET ARITHABORT OFF 
GO
ALTER DATABASE [news] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [news] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [news] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [news] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [news] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [news] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [news] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [news] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [news] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [news] SET  DISABLE_BROKER 
GO
ALTER DATABASE [news] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [news] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [news] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [news] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [news] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [news] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [news] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [news] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [news] SET  MULTI_USER 
GO
ALTER DATABASE [news] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [news] SET DB_CHAINING OFF 
GO
ALTER DATABASE [news] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [news] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [news] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [news] SET QUERY_STORE = OFF
GO
USE [news]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [news]
GO
/****** Object:  Table [dbo].[Articles]    Script Date: 06-03-2021 06:39:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Articles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Tags] [nvarchar](max) NOT NULL,
	[Category] [varchar](25) NOT NULL,
	[IsDraft] [bit] NOT NULL,
	[WriteDone] [bit] NOT NULL,
	[ReviewerDone] [bit] NOT NULL,
	[ReviewerComments] [varchar](250) NULL,
	[EditorDone] [bit] NOT NULL,
	[EditorComments] [varchar](250) NULL,
	[PublishDone] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[PublishedDate] [datetime] NULL,
	[Likes] [int] NOT NULL,
 CONSTRAINT [PK_Articles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArticleUsers]    Script Date: 06-03-2021 06:39:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArticleUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ArticleId] [int] NOT NULL,
	[UserRoleId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ArticleUsers_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_ArticleUsers] UNIQUE NONCLUSTERED 
(
	[UserRoleId] ASC,
	[ArticleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 06-03-2021 06:39:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 06-03-2021 06:39:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_UserRoles] UNIQUE NONCLUSTERED 
(
	[RoleId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 06-03-2021 06:39:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [varchar](25) NOT NULL,
	[DNDActive] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Users_Unique] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Roles]    Script Date: 06-03-2021 06:39:11 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Roles] ON [dbo].[Roles]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Articles] ADD  CONSTRAINT [DF_Articles_Likes]  DEFAULT ((0)) FOR [Likes]
GO
ALTER TABLE [dbo].[ArticleUsers]  WITH CHECK ADD  CONSTRAINT [FK_ArticleUsers_Articles] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Articles] ([Id])
GO
ALTER TABLE [dbo].[ArticleUsers] CHECK CONSTRAINT [FK_ArticleUsers_Articles]
GO
ALTER TABLE [dbo].[ArticleUsers]  WITH CHECK ADD  CONSTRAINT [FK_ArticleUsers_UserRoles] FOREIGN KEY([UserRoleId])
REFERENCES [dbo].[UserRoles] ([Id])
GO
ALTER TABLE [dbo].[ArticleUsers] CHECK CONSTRAINT [FK_ArticleUsers_UserRoles]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_User]
GO
USE [master]
GO
ALTER DATABASE [news] SET  READ_WRITE 
GO
