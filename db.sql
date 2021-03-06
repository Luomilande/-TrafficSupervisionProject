USE [master]
GO
/****** Object:  Database [MyProjectDB]    Script Date: 2020/6/20 4:12:10 ******/
CREATE DATABASE [MyProjectDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MyProjectDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\MyProjectDB.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MyProjectDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\MyProjectDB_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MyProjectDB] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MyProjectDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MyProjectDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MyProjectDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MyProjectDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MyProjectDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MyProjectDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [MyProjectDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MyProjectDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MyProjectDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MyProjectDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MyProjectDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MyProjectDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MyProjectDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MyProjectDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MyProjectDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MyProjectDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MyProjectDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MyProjectDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MyProjectDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MyProjectDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MyProjectDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MyProjectDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MyProjectDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MyProjectDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MyProjectDB] SET  MULTI_USER 
GO
ALTER DATABASE [MyProjectDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MyProjectDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MyProjectDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MyProjectDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [MyProjectDB] SET DELAYED_DURABILITY = DISABLED 
GO
USE [MyProjectDB]
GO
/****** Object:  Table [dbo].[Notice_info]    Script Date: 2020/6/20 4:12:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Notice_info](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](255) NOT NULL,
	[NoticeContent] [text] NULL,
	[CreationDate] [varchar](20) NOT NULL,
	[State] [bit] NOT NULL CONSTRAINT [DF_Notice_info_State]  DEFAULT ((1)),
	[Author] [int] NOT NULL,
	[Reading] [int] NOT NULL CONSTRAINT [DF_Notice_info_Reading]  DEFAULT ((0)),
 CONSTRAINT [PK__Notice__3213E83F84D8AEE9] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User_info]    Script Date: 2020/6/20 4:12:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User_info](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Email] [varchar](20) NOT NULL,
	[Password] [varchar](32) NOT NULL,
	[PhoneNumber] [nvarchar](11) NOT NULL,
	[Address] [varchar](255) NULL,
	[Sex] [bit] NULL,
	[Age] [int] NULL,
	[Date] [varchar](20) NOT NULL,
	[State] [bit] NOT NULL CONSTRAINT [DF_User_info_State]  DEFAULT ((1)),
	[Role] [varchar](10) NOT NULL CONSTRAINT [DF_User_info_Role]  DEFAULT ('user'),
 CONSTRAINT [PK__User_inf__3214EC27B8199A6A] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Video_info]    Script Date: 2020/6/20 4:12:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Video_info](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](255) NULL,
	[Synopsis] [text] NULL,
	[UpDate] [varchar](20) NULL,
	[VideoContent] [varchar](255) NULL,
	[AV_Number] [varchar](10) NULL,
	[OccurrenceTime] [varchar](20) NULL,
	[Address] [varchar](255) NULL,
	[AuditStatus] [bit] NULL CONSTRAINT [DF_Video_info_AuditStatus]  DEFAULT ((0)),
	[AuditResultStatus] [bit] NULL,
	[VideoStatus] [bit] NULL CONSTRAINT [DF_Video_info_VideoStatus]  DEFAULT ((1)),
	[AuthorID] [int] NULL,
	[Reading] [int] NULL CONSTRAINT [DF_Video_info_VideoRead]  DEFAULT ((0)),
 CONSTRAINT [PK__Video_in__3214EC27142D0680] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Video_Inspect]    Script Date: 2020/6/20 4:12:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Video_Inspect](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AuditContents] [varchar](255) NOT NULL,
	[AuditDate] [varchar](20) NOT NULL,
	[AuditAdminID] [int] NOT NULL,
	[AV_Number] [varchar](10) NOT NULL,
	[AuditStatus] [bit] NOT NULL,
	[Score] [int] NOT NULL CONSTRAINT [DF_Video_Inspect_Score]  DEFAULT ((0)),
 CONSTRAINT [PK__Video_In__3214EC27004E5E60] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Video_Vote]    Script Date: 2020/6/20 4:12:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Video_Vote](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AV_Number] [varchar](10) NULL,
	[Vote_T] [int] NULL CONSTRAINT [DF_Video_Vote_Vote_T]  DEFAULT ((0)),
	[Vote_F] [int] NULL CONSTRAINT [DF_Video_Vote_Vote_F]  DEFAULT ((0)),
 CONSTRAINT [PK__Video_Vo__3214EC2772F76B71] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WebSet_Tip]    Script Date: 2020/6/20 4:12:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebSet_Tip](
	[Id] [int] NOT NULL,
	[Tip0] [text] NULL,
	[Tip1] [text] NULL,
	[Tip2] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notice_info', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notice_info', @level2type=N'COLUMN',@level2name=N'NoticeContent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章发布时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notice_info', @level2type=N'COLUMN',@level2name=N'CreationDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章状态1、正常2、删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notice_info', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作者Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notice_info', @level2type=N'COLUMN',@level2name=N'Author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'阅读量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notice_info', @level2type=N'COLUMN',@level2name=N'Reading'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_info', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'电子邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_info', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_info', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_info', @level2type=N'COLUMN',@level2name=N'PhoneNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_info', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_info', @level2type=N'COLUMN',@level2name=N'Sex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'年龄' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_info', @level2type=N'COLUMN',@level2name=N'Age'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_info', @level2type=N'COLUMN',@level2name=N'Date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账号状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_info', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身份权限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_info', @level2type=N'COLUMN',@level2name=N'Role'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_info', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事故简介' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_info', @level2type=N'COLUMN',@level2name=N'Synopsis'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上传时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_info', @level2type=N'COLUMN',@level2name=N'UpDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'违章内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_info', @level2type=N'COLUMN',@level2name=N'VideoContent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'视频编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_info', @level2type=N'COLUMN',@level2name=N'AV_Number'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发生时间newdate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_info', @level2type=N'COLUMN',@level2name=N'OccurrenceTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发生地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_info', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态：0.未审核、1.已审核' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_info', @level2type=N'COLUMN',@level2name=N'AuditStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核结果：0、审核驳回1、审核通过' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_info', @level2type=N'COLUMN',@level2name=N'AuditResultStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'视频状态：0、封锁 1、正常' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_info', @level2type=N'COLUMN',@level2name=N'VideoStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上传用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_info', @level2type=N'COLUMN',@level2name=N'AuthorID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'阅读量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_info', @level2type=N'COLUMN',@level2name=N'Reading'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_Inspect', @level2type=N'COLUMN',@level2name=N'AuditContents'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_Inspect', @level2type=N'COLUMN',@level2name=N'AuditDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核员ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_Inspect', @level2type=N'COLUMN',@level2name=N'AuditAdminID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对应视频的AV号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_Inspect', @level2type=N'COLUMN',@level2name=N'AV_Number'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通过or驳回' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_Inspect', @level2type=N'COLUMN',@level2name=N'AuditStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扣分值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Video_Inspect', @level2type=N'COLUMN',@level2name=N'Score'
GO
USE [master]
GO
ALTER DATABASE [MyProjectDB] SET  READ_WRITE 
GO
