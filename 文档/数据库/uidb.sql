USE [master]
GO
/****** Object:  Database [UIDB]    Script Date: 08/01/2019 15:50:36 ******/
CREATE DATABASE [UIDB] ON  PRIMARY 
( NAME = N'UIDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\UIDB.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'UIDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\UIDB_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [UIDB] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [UIDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [UIDB] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [UIDB] SET ANSI_NULLS OFF
GO
ALTER DATABASE [UIDB] SET ANSI_PADDING OFF
GO
ALTER DATABASE [UIDB] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [UIDB] SET ARITHABORT OFF
GO
ALTER DATABASE [UIDB] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [UIDB] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [UIDB] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [UIDB] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [UIDB] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [UIDB] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [UIDB] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [UIDB] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [UIDB] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [UIDB] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [UIDB] SET  DISABLE_BROKER
GO
ALTER DATABASE [UIDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [UIDB] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [UIDB] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [UIDB] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [UIDB] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [UIDB] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [UIDB] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [UIDB] SET  READ_WRITE
GO
ALTER DATABASE [UIDB] SET RECOVERY FULL
GO
ALTER DATABASE [UIDB] SET  MULTI_USER
GO
ALTER DATABASE [UIDB] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [UIDB] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'UIDB', N'ON'
GO
USE [UIDB]
GO
/****** Object:  Table [dbo].[DocumentFolder]    Script Date: 08/01/2019 15:50:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DocumentFolder](
	[UID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentFolderNo] [varchar](128) NOT NULL,
	[FolderUID] [int] NOT NULL,
	[DocumentUID] [int] NOT NULL,
	[Creater] [varchar](32) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[EditTime] [datetime] NOT NULL,
	[Remark] [varchar](max) NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_DocumentFolder] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentFolder', @level2type=N'COLUMN',@level2name=N'UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentFolder', @level2type=N'COLUMN',@level2name=N'DocumentFolderNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件夹标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentFolder', @level2type=N'COLUMN',@level2name=N'FolderUID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentFolder', @level2type=N'COLUMN',@level2name=N'DocumentUID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentFolder', @level2type=N'COLUMN',@level2name=N'Creater'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentFolder', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentFolder', @level2type=N'COLUMN',@level2name=N'Editor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentFolder', @level2type=N'COLUMN',@level2name=N'EditTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentFolder', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态标志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentFolder', @level2type=N'COLUMN',@level2name=N'State'
GO
SET IDENTITY_INSERT [dbo].[DocumentFolder] ON
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (113, N'TN000001', 3, 3, N'3', CAST(0x0000A14200000000 AS DateTime), N'3', CAST(0x0000A14200000000 AS DateTime), N'3', 3)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (114, N'TN000002', 3, 3, N'3', CAST(0x0000A14200000000 AS DateTime), N'3', CAST(0x0000A14200000000 AS DateTime), N'3', 3)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (115, N'TN000003', 3, 3, N'3', CAST(0x0000A14200000000 AS DateTime), N'3', CAST(0x0000A14200000000 AS DateTime), N'3', 3)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (116, N'TN000004', 3, 3, N'3', CAST(0x0000A14200000000 AS DateTime), N'3', CAST(0x0000A14200000000 AS DateTime), N'3', 3)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (117, N'TN000005', 3, 3, N'3', CAST(0x0000A14200000000 AS DateTime), N'3', CAST(0x0000A14200000000 AS DateTime), N'3', 3)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (118, N'TN000006', 3, 3, N'3', CAST(0x0000A14200000000 AS DateTime), N'3', CAST(0x0000A14200000000 AS DateTime), N'3', 3)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (119, N'TN000007', 3, 3, N'3', CAST(0x0000A14200000000 AS DateTime), N'3', CAST(0x0000A14200000000 AS DateTime), N'3', 3)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (120, N'TN000008', 3, 3, N'3', CAST(0x0000A14200000000 AS DateTime), N'3', CAST(0x0000A14200000000 AS DateTime), N'3', 3)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (121, N'TN000009', 3, 3, N'3', CAST(0x0000A14200000000 AS DateTime), N'3', CAST(0x0000A14200000000 AS DateTime), N'3', 3)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (126, N'TN000011', 7, 66, N'ui', CAST(0x0000A89000B25E4F AS DateTime), N'ui', CAST(0x0000A89000B25E4F AS DateTime), NULL, 10)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (138, N'TN000016', 5, 68, N'ui', CAST(0x0000A89000EE4B4E AS DateTime), N'ui', CAST(0x0000A89000EE4B4E AS DateTime), NULL, 10)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (139, N'TN000016', 12, 68, N'ui', CAST(0x0000A89000EE4B4E AS DateTime), N'ui', CAST(0x0000A89000EE4B4E AS DateTime), NULL, 10)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (147, N'TN000017', 79, 69, N'ui', CAST(0x0000A89000F00706 AS DateTime), N'ui', CAST(0x0000A89000F00706 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (148, N'TN000017', 80, 69, N'ui', CAST(0x0000A89000F00706 AS DateTime), N'ui', CAST(0x0000A89000F00706 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (155, N'TN000018', 74, 67, N'ui', CAST(0x0000A89000F1ACFD AS DateTime), N'ui', CAST(0x0000A89000F1ACFD AS DateTime), NULL, 10)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (156, N'TN000018', 79, 67, N'ui', CAST(0x0000A89000F1ACFD AS DateTime), N'ui', CAST(0x0000A89000F1ACFD AS DateTime), NULL, 10)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (157, N'TN000018', 80, 67, N'ui', CAST(0x0000A89000F1ACFD AS DateTime), N'ui', CAST(0x0000A89000F1ACFD AS DateTime), NULL, 10)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (158, N'TN000019', 12, 71, N'ui', CAST(0x0000A89000F21E90 AS DateTime), N'ui', CAST(0x0000A89000F21E90 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentFolder] ([UID], [DocumentFolderNo], [FolderUID], [DocumentUID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (159, N'TN000019', 12, 70, N'ui', CAST(0x0000A89000F21E90 AS DateTime), N'ui', CAST(0x0000A89000F21E90 AS DateTime), NULL, 10)
SET IDENTITY_INSERT [dbo].[DocumentFolder] OFF
/****** Object:  Table [dbo].[Folder]    Script Date: 08/01/2019 15:50:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Folder](
	[UID] [int] IDENTITY(1,1) NOT NULL,
	[FolderNo] [varchar](128) NOT NULL,
	[FolderName] [varchar](32) NOT NULL,
	[FatherID] [int] NULL,
	[Creater] [varchar](32) NOT NULL,
	[CreateTime] [datetime] NULL,
	[Editor] [varchar](32) NOT NULL,
	[EditTime] [datetime] NOT NULL,
	[Remark] [varchar](max) NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_Folder] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Folder] UNIQUE NONCLUSTERED 
(
	[FolderNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Folder', @level2type=N'COLUMN',@level2name=N'UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件夹编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Folder', @level2type=N'COLUMN',@level2name=N'FolderNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件夹名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Folder', @level2type=N'COLUMN',@level2name=N'FolderName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Folder', @level2type=N'COLUMN',@level2name=N'FatherID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Folder', @level2type=N'COLUMN',@level2name=N'Creater'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Folder', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Folder', @level2type=N'COLUMN',@level2name=N'Editor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Folder', @level2type=N'COLUMN',@level2name=N'EditTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Folder', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Folder', @level2type=N'COLUMN',@level2name=N'State'
GO
SET IDENTITY_INSERT [dbo].[Folder] ON
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (5, N'123', N'慢病系统', -1, N'1323', CAST(0x00009E5E00000000 AS DateTime), N'23', CAST(0x00009E5E00000000 AS DateTime), N'123123', 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (7, N'2', N'菜单图标', 5, N'1323', CAST(0x00009E5E00000000 AS DateTime), N'23', CAST(0x00009E5E00000000 AS DateTime), N'123123', 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (12, N'4', N'工具栏图标1', 5, N'1323', CAST(0x00009E5E00000000 AS DateTime), N'ui', CAST(0x0000A88200ED3F40 AS DateTime), N'123123', 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (14, N'5', N'功能图标', 5, N'1323', CAST(0x00009E5E00000000 AS DateTime), N'ui', CAST(0x0000A85700B9AE44 AS DateTime), N'123123', 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (37, N'6', N'菜单2', 10, N'ui', CAST(0x0000A85500ACDEA8 AS DateTime), N'ui', CAST(0x0000A85500ACDEA8 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (39, N'87', N'菜单4', 48, N'ui', CAST(0x0000A85500AD7CEA AS DateTime), N'ui', CAST(0x0000A85500AD7CEA AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (48, N'23', N'菜单7', 87, N'ui', CAST(0x0000A85500ACDEA8 AS DateTime), N'ui', CAST(0x0000A85500ACDEA8 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (50, N'35', N'门诊系统', -1, N'ui', CAST(0x0000A85500ACDEA8 AS DateTime), N'ui', CAST(0x0000A85500ACDEA8 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (51, N'86', N'门诊菜单图标', 50, N'ui', CAST(0x0000A85700BB2F63 AS DateTime), N'ui', CAST(0x0000A85E00B962BD AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (52, N'61', N'门诊工具栏图标', 51, N'ui', CAST(0x0000A85700BBEEBB AS DateTime), N'ui', CAST(0x0000A85700BBEEBB AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (53, N'60', N'门诊功能图标', 50, N'ui', CAST(0x0000A85700BC7481 AS DateTime), N'ui', CAST(0x0000A85700BC7481 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (64, N'44', N'健康小屋', -1, N'ui', CAST(0x0000A85E00C001BD AS DateTime), N'ui', CAST(0x0000A85E00C001BD AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (65, N'27', N'住院系统', -1, N'ui', CAST(0x0000A85E00C0B5C6 AS DateTime), N'ui', CAST(0x0000A85E00C0B5C6 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (66, N'77', N'妇幼系统', -1, N'ui', CAST(0x0000A85E00C2955E AS DateTime), N'ui', CAST(0x0000A85E00C2955E AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (67, N'68', N'报表系统', -1, N'ui', CAST(0x0000A85E00C2C17F AS DateTime), N'ui', CAST(0x0000A85E00C2C17F AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (68, N'84', N'挂号系统', -1, N'ui', CAST(0x0000A85E00C2D733 AS DateTime), N'ui', CAST(0x0000A85E00C2D733 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (69, N'15', N'药品系统', -1, N'ui', CAST(0x0000A85E00C44579 AS DateTime), N'ui', CAST(0x0000A85E00C44579 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (73, N'16', N'测试系统', -1, N'ui', CAST(0x0000A85E00C44579 AS DateTime), N'ui', CAST(0x0000A85E00C44579 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (74, N'70', N'test3', 12, N'ui', CAST(0x0000A88200EF4557 AS DateTime), N'ui', CAST(0x0000A88201076693 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (78, N'72', N'323', 5, N'ui', CAST(0x0000A8860117D654 AS DateTime), N'ui', CAST(0x0000A8860117D654 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (79, N'TN000088', N'test3-1', 74, N'ui', CAST(0x0000A88F0117CBC6 AS DateTime), N'ui', CAST(0x0000A88F0117CBC6 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (80, N'TN000089', N'test3-1-1', 79, N'ui', CAST(0x0000A88F01182438 AS DateTime), N'ui', CAST(0x0000A88F01182438 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (81, N'TN000090', N'test3-1-1-2', 80, N'ui', CAST(0x0000A88F011B9498 AS DateTime), N'ui', CAST(0x0000A88F011B9498 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (82, N'TN000091', N'tes333', 80, N'ui', CAST(0x0000A890009FC10D AS DateTime), N'ui', CAST(0x0000A890009FC10D AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (83, N'TN000092', N'test4', 80, N'ui', CAST(0x0000A89000A58B70 AS DateTime), N'ui', CAST(0x0000A89000A58B70 AS DateTime), NULL, 10)
INSERT [dbo].[Folder] ([UID], [FolderNo], [FolderName], [FatherID], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (84, N'TN000093', N'test3333', 79, N'ui', CAST(0x0000A89000A5A1E5 AS DateTime), N'ui', CAST(0x0000A89000A5A1E5 AS DateTime), NULL, 10)
SET IDENTITY_INSERT [dbo].[Folder] OFF
/****** Object:  Table [dbo].[Task]    Script Date: 08/01/2019 15:50:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Task](
	[UID] [int] IDENTITY(1,1) NOT NULL,
	[TaskNo] [varchar](128) NOT NULL,
	[TaskName] [varchar](32) NOT NULL,
	[Creater] [varchar](32) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[EditTime] [datetime] NOT NULL,
	[Remark] [varchar](max) NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Task] UNIQUE NONCLUSTERED 
(
	[TaskNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'TaskNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'TaskName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Creater'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Editor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'EditTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'State'
GO
SET IDENTITY_INSERT [dbo].[Task] ON
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (31, N'TN000001', N'任务1', N'XX', CAST(0x0000901A00000000 AS DateTime), N'ui', CAST(0x0000A8A300AF30FD AS DateTime), N'xx', 10)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (33, N'TN000002', N'任务2', N'', CAST(0x0000A88F00AED717 AS DateTime), N'ui', CAST(0x0000A8A300A53394 AS DateTime), N'我是备注', 10)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (34, N'TN000003', N'任务3', N'', CAST(0x0000A88F00AEE36D AS DateTime), N'', CAST(0x0000A88F00AEE36D AS DateTime), N'22', 10)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (35, N'TN000004', N'任务4', N'', CAST(0x0000A88F00B3EE3A AS DateTime), N'', CAST(0x0000A88F00B3EE3A AS DateTime), N'2214', 10)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (36, N'TN000005', N'任务5', N'', CAST(0x0000A89300B30063 AS DateTime), N'', CAST(0x0000A89300B30063 AS DateTime), N'11', 20)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (37, N'TN000006', N'任务6', N'', CAST(0x0000A89300B4BB34 AS DateTime), N'', CAST(0x0000A89300B4BB34 AS DateTime), N'11', 20)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (38, N'TN000007', N'任务7', N'', CAST(0x0000A89A0114EE39 AS DateTime), N'', CAST(0x0000A89A0114EE39 AS DateTime), N'123', 10)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (39, N'TN000008', N'任务8', N'', CAST(0x0000A89A01163BC2 AS DateTime), N'', CAST(0x0000A89A01163BC2 AS DateTime), N'23', 10)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (40, N'TN000009', N'任务9', N'', CAST(0x0000A89A0117043A AS DateTime), N'', CAST(0x0000A89A0117043A AS DateTime), N'23', 10)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (41, N'TN000010', N'任务10', N'', CAST(0x0000A89A01177402 AS DateTime), N'', CAST(0x0000A89A01177402 AS DateTime), N'242', 10)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (42, N'TN000011', N'任务11', N'', CAST(0x0000A89B00A3C492 AS DateTime), N'', CAST(0x0000A89B00A3C492 AS DateTime), N'333', 10)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (43, N'TN000012', N'任务12', N'', CAST(0x0000A89B00A55EDC AS DateTime), N'', CAST(0x0000A89B00A55EDC AS DateTime), N'232', 10)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (44, N'TN000013', N'任务13', N'', CAST(0x0000A89B00A6DF98 AS DateTime), N'', CAST(0x0000A89B00A6DF98 AS DateTime), N'234', 10)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (45, N'TN000014', N'任务14', N'', CAST(0x0000A89B00A83160 AS DateTime), N'', CAST(0x0000A89B00A83160 AS DateTime), N'42342', 10)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (46, N'TN000015', N'任务15', N'', CAST(0x0000A89B00A897A7 AS DateTime), N'', CAST(0x0000A89B00A897A7 AS DateTime), N'32e', 10)
INSERT [dbo].[Task] ([UID], [TaskNo], [TaskName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (47, N'TN000016', N'任务16', N'', CAST(0x0000A89B00A90F1B AS DateTime), N'', CAST(0x0000A89B00A90F1B AS DateTime), N'32e', 10)
SET IDENTITY_INSERT [dbo].[Task] OFF
/****** Object:  Table [dbo].[TaskList]    Script Date: 08/01/2019 15:50:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaskList](
	[UID] [int] IDENTITY(1,1) NOT NULL,
	[TaskListNo] [varchar](128) NOT NULL,
	[TaskListName] [varchar](32) NOT NULL,
	[TaskUID] [int] NOT NULL,
	[DocumentUID] [int] NOT NULL,
	[PictureSize] [varchar](32) NULL,
	[PictureResolution] [varchar](32) NOT NULL,
	[PictureBackground] [varchar](32) NULL,
	[DocumentType] [varchar](32) NOT NULL,
	[Creater] [varchar](32) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[EditTime] [datetime] NULL,
	[Remark] [varchar](max) NULL,
	[State] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
 CONSTRAINT [PK_TaskList] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_TaskList] UNIQUE NONCLUSTERED 
(
	[TaskListNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务清单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'TaskListNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务清单名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'TaskListName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'TaskUID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件夹标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'DocumentUID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'PictureSize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片分辨率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'PictureResolution'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片背景色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'PictureBackground'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'DocumentType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'Creater'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'Editor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'EditTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskList', @level2type=N'COLUMN',@level2name=N'State'
GO
SET IDENTITY_INSERT [dbo].[TaskList] ON
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (8, N'55', N'病人分类设置', 19, 0, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A83B00E0A316 AS DateTime), N'ui', CAST(0x0000A83B00E0A316 AS DateTime), N'Remark', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (9, N'44', N'病人建档', 19, 0, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A83B00E0A482 AS DateTime), N'ui', CAST(0x0000A83B00E0A482 AS DateTime), N'Remark', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (10, N'33', N'病人进度', 19, 0, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A842011237FC AS DateTime), N'ui', CAST(0x0000A842011237FC AS DateTime), N'12312', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (11, N'22', N'病人类别设置', 19, 0, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A84201129FB6 AS DateTime), N'ui', CAST(0x0000A84201129FB6 AS DateTime), N'24', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (12, N'11', N'病人日程', 19, 0, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A8420114E4C8 AS DateTime), N'ui', CAST(0x0000A8420114E4C8 AS DateTime), N'345435', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (13, N'27', N'短信发送3', 33, 70, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A87F00FCBFCD AS DateTime), N'ui', CAST(0x0000A87F00FCBFCD AS DateTime), N'3', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (14, N'95', N'短信发送2', 33, 69, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A87F00FCC002 AS DateTime), N'ui', CAST(0x0000A8A20123E69C AS DateTime), N'33', 0, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (15, N'49', N'短信发送1', 33, 68, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (18, N'50', N'短信发送4', 33, 0, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'3', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (28, N'53', N'短信发送6', 7, 0, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'3', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (29, N'54', N'短信发送7', 7, 0, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (34, N'6', N'短信发送8', 7, 0, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (35, N'7', N'短信发送9', 7, 0, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (43, N'8', N'短信发送9', 10, 0, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (44, N'9', N'短信发送12', 10, 0, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (46, N'10', N'短信发送10', 10, 267, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'3', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (58, N'12', N'短信发送11', 10, 267, N'1', N'1', N'1', N'1', N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'ui', CAST(0x0000A87F00FCC01B AS DateTime), N'3', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (99, N'TN000096', N'短信发送5', 33, 0, N'2', N'2', N'2', N'2', N'ui', CAST(0x0000A8A3009C800C AS DateTime), N'ui', CAST(0x0000A8A3009C800C AS DateTime), N'2', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (100, N'TN000097', N'短信发送6', 33, 0, N'4', N'3', N'3', N'3', N'ui', CAST(0x0000A8A3009CBC49 AS DateTime), N'ui', CAST(0x0000A8A3009CBC49 AS DateTime), N'3', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (101, N'TN000098', N'短信发送7', 33, 0, N'4', N'4', N'4', N'4', N'ui', CAST(0x0000A8A3009CBCAB AS DateTime), N'ui', CAST(0x0000A8A3009CBCAB AS DateTime), N'4', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (102, N'TN000099', N'短信发送8', 33, 0, N'5', N'5', N'5', N'5', N'ui', CAST(0x0000A8A300A4D68C AS DateTime), N'ui', CAST(0x0000A8A300A4D68C AS DateTime), N'5', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (103, N'TN000100', N'短信发送9', 33, 0, N'6', N'6', N'6', N'6', N'ui', CAST(0x0000A8A300A53480 AS DateTime), N'ui', CAST(0x0000A8A300A53480 AS DateTime), N'6', 10, 0)
INSERT [dbo].[TaskList] ([UID], [TaskListNo], [TaskListName], [TaskUID], [DocumentUID], [PictureSize], [PictureResolution], [PictureBackground], [DocumentType], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State], [ProjectId]) VALUES (104, N'TN000101', N'远程会诊', 31, 0, N'30x30', N'100dpi', N'#fwfewf', N'jpg', N'ui', CAST(0x0000A8A300AF3142 AS DateTime), N'ui', CAST(0x0000A8A300AF3142 AS DateTime), N'345345', 10, 0)
SET IDENTITY_INSERT [dbo].[TaskList] OFF
/****** Object:  UserDefinedFunction [dbo].[GetNextTN]    Script Date: 08/01/2019 15:50:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetNextTN](@TableName varchar(MAX))
RETURNS char(8)
AS
BEGIN
declare @TN varchar(20);

  IF (@TableName = 'Document')
  BEGIN
   (SELECT  @TN='TN'+RIGHT(1000001+ISNULL(RIGHT(MAX(DocumentNo),6),0),6) FROM Document WITH(XLOCK,PAGLOCK))
  END
  ELSE IF (@TableName = 'DocumentFolder')
  BEGIN
    (SELECT  @TN='TN'+RIGHT(1000001+ISNULL(RIGHT(MAX(DocumentFolderNo),6),0),6) FROM DocumentFolder WITH(XLOCK,PAGLOCK))
  END
  ELSE IF (@TableName = 'Folder')
  BEGIN
    (SELECT  @TN='TN'+RIGHT(1000001+ISNULL(RIGHT(MAX(FolderNo),6),0),6) FROM Folder WITH(XLOCK,PAGLOCK))
  END
  ELSE IF (@TableName = 'Task')
  BEGIN
    (SELECT  @TN='TN'+RIGHT(1000001+ISNULL(RIGHT(MAX(TaskNo),6),0),6) FROM Task WITH(XLOCK,PAGLOCK))
  END
  ELSE IF (@TableName = 'TaskList')
  BEGIN
    (SELECT  @TN='TN'+RIGHT(1000001+ISNULL(RIGHT(MAX(TaskListNo),6),0),6) FROM TaskList WITH(XLOCK,PAGLOCK))
  END
     RETURN @TN
END
GO
/****** Object:  Table [dbo].[Document]    Script Date: 08/01/2019 15:50:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Document](
	[UID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentNo] [varchar](128) NOT NULL,
	[DocumentName] [varchar](128) NOT NULL,
	[DocumentType] [varchar](32) NOT NULL,
	[PictureSize] [varchar](32) NULL,
	[PictureResolution] [varchar](32) NULL,
	[PictureBackground] [varchar](32) NULL,
	[DocumentPath] [varchar](max) NOT NULL,
	[Creater] [varchar](32) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[EditTime] [datetime] NOT NULL,
	[Remark] [varchar](max) NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Icon] UNIQUE NONCLUSTERED 
(
	[DocumentNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'DocumentNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'DocumentName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'DocumentType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'PictureSize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片分辨率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'PictureResolution'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片背景色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'PictureBackground'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'DocumentPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'Creater'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'Editor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'EditTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态标志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document', @level2type=N'COLUMN',@level2name=N'State'
GO
SET IDENTITY_INSERT [dbo].[Document] ON
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (66, N'TN000001', N'短信发送_L', N'.png', N'32X32', N'96dpi', NULL, N'/UploadFiles/images/短信发送_L.png', N'ui', CAST(0x0000A88F00C335CA AS DateTime), N'ui', CAST(0x0000A88F00C335CA AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (67, N'TN000002', N'短信发送_a', N'.png', N'32X32', N'96dpi', NULL, N'/UploadFiles/images/短信发送_a.png', N'ui', CAST(0x0000A88F00C33683 AS DateTime), N'ui', CAST(0x0000A89A01082895 AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (68, N'TN000003', N'短信发送1', N'.png', N'32X32', N'96dpi', NULL, N'/UploadFiles/images/短信发送1.png', N'ui', CAST(0x0000A88F00C336C8 AS DateTime), N'ui', CAST(0x0000A88F00C336C8 AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (69, N'TN000004', N'短信发送2', N'.png', N'32X32', N'96dpi', NULL, N'/UploadFiles/images/短信发送2.png', N'ui', CAST(0x0000A88F00C33741 AS DateTime), N'ui', CAST(0x0000A88F00C33741 AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (70, N'TN000005', N'短信发送3', N'.png', N'32X32', N'96dpi', NULL, N'/UploadFiles/images/短信发送3.png', N'ui', CAST(0x0000A88F00C33787 AS DateTime), N'ui', CAST(0x0000A88F00C33787 AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (71, N'TN000006', N'额外字典设置_20180226102925056.png', N'.png', N'64X64', N'96dpi', NULL, N'/UploadFiles/images/额外字典设置_20180226102925056.png', N'ui', CAST(0x0000A88F00C337DB AS DateTime), N'ui', CAST(0x0000A89300ACE017 AS DateTime), NULL, 20)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (72, N'TN000007', N'短信发送4', N'.jpg', N'600X800', N'96dpi', NULL, N'/UploadFiles/images/额外字典设置_20180226102925056.png', N'ui', CAST(0x0000A89000F84029 AS DateTime), N'ui', CAST(0x0000A89000F8402A AS DateTime), NULL, 20)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (73, N'TN000008', N'03087bf40ad162d91ec378071bdfa9ec8a13cdf8', N'.jpg', N'600X375', N'96dpi', NULL, N'/UploadFiles/images/额外字典设置_20180226102925056.png', N'ui', CAST(0x0000A89000F84297 AS DateTime), N'ui', CAST(0x0000A89000F84297 AS DateTime), NULL, 20)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (74, N'TN000009', N'e9c4fc6813e845abb35f3a05bd41cc5e_990_0_max_JPG', N'.jpg', N'990X672', N'96dpi', NULL, N'/UploadFiles/images/短信发送3.png', N'ui', CAST(0x0000A89000F844E2 AS DateTime), N'ui', CAST(0x0000A89000F844E2 AS DateTime), NULL, 20)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (75, N'TN000010', N'u=4039883674,4245899092&fm=27&gp=0', N'.jpg', N'500X333', N'96dpi', NULL, N'/UploadFiles/images/额外字典设置_20180226102925056.png', N'ui', CAST(0x0000A89000F847B4 AS DateTime), N'ui', CAST(0x0000A89000F847B4 AS DateTime), NULL, 20)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (76, N'TN000011', N'短信接口', N'.png', N'32X32', N'96dpi', NULL, N'/UploadFiles/images/短信接口.png', N'ui', CAST(0x0000A89300AC9FAF AS DateTime), N'ui', CAST(0x0000A89300AC9FAF AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (77, N'TN000012', N'短信模板', N'.png', N'32X32', N'96dpi', NULL, N'/UploadFiles/images/短信模板.png', N'ui', CAST(0x0000A89300ACAC6A AS DateTime), N'ui', CAST(0x0000A89300ACAC6A AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (78, N'TN000013', N'短信内容配置', N'.png', N'32X32', N'96dpi', NULL, N'/UploadFiles/images/短信内容配置.png', N'ui', CAST(0x0000A89300ACAEFC AS DateTime), N'ui', CAST(0x0000A89300ACAEFC AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (79, N'TN000014', N'短信网关配置', N'.png', N'32X32', N'96dpi', NULL, N'/UploadFiles/images/短信网关配置.png', N'ui', CAST(0x0000A89300ACB1AF AS DateTime), N'ui', CAST(0x0000A89300ACB1AF AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (80, N'TN000015', N'多量表数据导出', N'.png', N'32X32', N'96dpi', NULL, N'/UploadFiles/images/多量表数据导出.png', N'ui', CAST(0x0000A89300ACD15D AS DateTime), N'ui', CAST(0x0000A89300ACD15D AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (81, N'TN000016', N'额外字典设置', N'.png', N'64X64', N'96dpi', NULL, N'/UploadFiles/images/额外字典设置.png', N'ui', CAST(0x0000A89300ACE10E AS DateTime), N'ui', CAST(0x0000A89300ACE10E AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (82, N'TN000017', N'儿童健康体检', N'.png', N'32X32', N'96dpi', NULL, N'/UploadFiles/images/儿童健康体检.png', N'ui', CAST(0x0000A89300ACE4AA AS DateTime), N'ui', CAST(0x0000A89300ACE4AA AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (83, N'TN000018', N'发票管理（P）', N'.png', N'64X64', N'96dpi', NULL, N'/UploadFiles/images/发票管理（P）.png', N'ui', CAST(0x0000A89300ACE833 AS DateTime), N'ui', CAST(0x0000A89300ACE833 AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (84, N'TN000019', N'发票管理', N'.png', N'64X64', N'96dpi', NULL, N'/UploadFiles/images/发票管理.png', N'ui', CAST(0x0000A89300ACEAFA AS DateTime), N'ui', CAST(0x0000A89300ACEAFA AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (85, N'TN000020', N'废票原因', N'.png', N'64X64', N'96dpi', NULL, N'/UploadFiles/images/废票原因.png', N'ui', CAST(0x0000A89300ACED87 AS DateTime), N'ui', CAST(0x0000A89300ACED87 AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (86, N'TN000021', N'费用查询', N'.png', N'64X64', N'96dpi', NULL, N'/UploadFiles/images/费用查询.png', N'ui', CAST(0x0000A89300ACF03A AS DateTime), N'ui', CAST(0x0000A89300ACF03A AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (87, N'TN000022', N'费用分摊比例', N'.png', N'64X64', N'96dpi', NULL, N'/UploadFiles/images/费用分摊比例.png', N'ui', CAST(0x0000A89300ACF2F4 AS DateTime), N'ui', CAST(0x0000A89300ACF2F4 AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (88, N'TN000023', N'付款方字典', N'.png', N'64X64', N'96dpi', NULL, N'/UploadFiles/images/付款方字典.png', N'ui', CAST(0x0000A89300C54AF9 AS DateTime), N'ui', CAST(0x0000A89300C54AF9 AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (89, N'TN000024', N'妇女病普查', N'.png', N'32X32', N'96dpi', NULL, N'/UploadFiles/images/妇女病普查.png', N'ui', CAST(0x0000A89300C55CE5 AS DateTime), N'ui', CAST(0x0000A89300C55CE5 AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (90, N'TN000025', N'妇幼保健', N'.png', N'128X128', N'96dpi', NULL, N'/UploadFiles/images/妇幼保健.png', N'ui', CAST(0x0000A89300C56383 AS DateTime), N'ui', CAST(0x0000A89300C56383 AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (91, N'TN000026', N'给药方式对照字典', N'.png', N'64X64', N'96dpi', NULL, N'/UploadFiles/images/给药方式对照字典.png', N'ui', CAST(0x0000A89300C56BE4 AS DateTime), N'ui', CAST(0x0000A89300C56BE4 AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (92, N'TN000027', N'给药方式类型字典', N'.png', N'64X64', N'96dpi', NULL, N'/UploadFiles/images/给药方式类型字典.png', N'ui', CAST(0x0000A89300C572F6 AS DateTime), N'ui', CAST(0x0000A89300C572F6 AS DateTime), NULL, 10)
INSERT [dbo].[Document] ([UID], [DocumentNo], [DocumentName], [DocumentType], [PictureSize], [PictureResolution], [PictureBackground], [DocumentPath], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (93, N'TN000028', N'给药方式字典', N'.png', N'64X64', N'96dpi', NULL, N'/UploadFiles/images/给药方式字典.png', N'ui', CAST(0x0000A89300C57961 AS DateTime), N'ui', CAST(0x0000A89300C57961 AS DateTime), NULL, 10)
SET IDENTITY_INSERT [dbo].[Document] OFF
/****** Object:  Table [dbo].[User]    Script Date: 08/01/2019 15:50:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UID] [int] IDENTITY(1,1) NOT NULL,
	[UserNo] [int] NOT NULL,
	[UserName] [varchar](32) NOT NULL,
	[Password] [varchar](32) NOT NULL,
	[Creater] [varchar](32) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[EditTime] [datetime] NOT NULL,
	[Remark] [varchar](max) NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_User] UNIQUE NONCLUSTERED 
(
	[UserNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'UserNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'Creater'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'Editor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'EditTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'State'
GO
SET IDENTITY_INSERT [dbo].[User] ON
INSERT [dbo].[User] ([UID], [UserNo], [UserName], [Password], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (1, 1, N'test', N'test', N'admin', CAST(0x0000A80E01077C7C AS DateTime), N'admin', CAST(0x0000A80E01077C7C AS DateTime), NULL, 0)
INSERT [dbo].[User] ([UID], [UserNo], [UserName], [Password], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (4, 2, N'ui', N'7d5c009e4eb8bbc78647caeca308e61b', N'admin', CAST(0x0000A83500000000 AS DateTime), N'admin', CAST(0x0000A83500000000 AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[User] OFF
/****** Object:  Table [dbo].[Project]    Script Date: 08/01/2019 15:50:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Project](
	[UID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectNo] [varchar](128) NULL,
	[ProjectName] [varchar](32) NOT NULL,
	[Creater] [varchar](32) NOT NULL,
	[CreateTime] [datetime] NULL,
	[Editor] [varchar](32) NULL,
	[EditTime] [datetime] NULL,
	[Remark] [varchar](max) NULL,
	[State] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ProjectNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Project] ON
INSERT [dbo].[Project] ([UID], [ProjectNo], [ProjectName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (1, N'58f467bcc7014567', N'test123', N'ui', CAST(0x0000A83C00A6506C AS DateTime), N'ui', CAST(0x0000A83C00A652B8 AS DateTime), N'12313', 1)
INSERT [dbo].[Project] ([UID], [ProjectNo], [ProjectName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (2, N'a58056101f822055', N't3', N'ui', CAST(0x0000A842011C95F7 AS DateTime), N'ui', CAST(0x0000A842011C95F7 AS DateTime), N'353', 1)
INSERT [dbo].[Project] ([UID], [ProjectNo], [ProjectName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (3, N'58', N'test1', N'ui', CAST(0x0000A8860116BA27 AS DateTime), N'ui', CAST(0x0000A8860116BA27 AS DateTime), N'', 10)
INSERT [dbo].[Project] ([UID], [ProjectNo], [ProjectName], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (4, N'27', N'r2r23', N'ui', CAST(0x0000A8860117CC86 AS DateTime), N'ui', CAST(0x0000A8860117CC86 AS DateTime), N'33', 10)
SET IDENTITY_INSERT [dbo].[Project] OFF
/****** Object:  Table [dbo].[Permission]    Script Date: 08/01/2019 15:50:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Permission](
	[UID] [int] NOT NULL,
	[PermissionNo] [int] NOT NULL,
	[PermissionName] [varchar](32) NOT NULL,
	[Remark] [varchar](max) NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permission', @level2type=N'COLUMN',@level2name=N'UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permission', @level2type=N'COLUMN',@level2name=N'PermissionNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限级别名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permission', @level2type=N'COLUMN',@level2name=N'PermissionName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permission', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permission', @level2type=N'COLUMN',@level2name=N'State'
GO
/****** Object:  StoredProcedure [dbo].[GetUnfinishTaskPer]    Script Date: 08/01/2019 15:50:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUnfinishTaskPer] --显示主任务未完成 百分比 数量

AS
BEGIN
declare @count int,
        @WheresStr varchar(MAX),
        @SelectStr varchar(MAX)
--        set @WheresStr='where '+'TaskList.TaskListName not in (select Document.DocumentName from Document) and TaskList.TaskUID=Task.UID)';
        
--  set  @SelectStr ='select Task.UID,Task.TaskName,
--  (select cast(cast(COUNT(*)*1.0/(select  case when  COUNT(*)= ''0'' then ''1'' else  COUNT(*) end 
--  from TaskList where TaskList.TaskUID=Task.UID) *100 as decimal(16,0)) as varchar(10))+''%''
--  from TaskList 
--'+@WheresStr+' as Per,
--(select  COUNT(*) from TaskList 
-- '+@WheresStr+' as TotalCount from Task';

 set @SelectStr ='select t.UID,t.TaskName,
(select cast(cast(COUNT(*)*1.0/(
select  case when  COUNT(*)= ''0'' then ''1'' else  COUNT(*) end from TaskList as tl where tl.TaskUID=t.UID)*100  as decimal(16,0)) as varchar(10))+''%''    
from TaskList as tl1
where tl1.DocumentUID = ''0'' and tl1.TaskUID=t.UID ) as Per,
(select count(*) from TaskList as tl1
where tl1.DocumentUID = ''0'' and tl1.TaskUID=t.UID)as TotalCount 
 from task as t where t.state = ''10'''; -- 未完成
 exec(@SelectStr);
END
GO
/****** Object:  StoredProcedure [dbo].[GetFinishTaskPer]    Script Date: 08/01/2019 15:50:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetFinishTaskPer] --显示主任务已完成 百分比 数量

AS
BEGIN
declare 
        @SelectStr varchar(MAX)
        

set @SelectStr ='select t.UID,t.TaskName,t.CreateTime,t.State,
(select cast(cast(COUNT(*)*1.0/(
select  case when  COUNT(*)= ''0'' then ''1'' else  COUNT(*) end from TaskList as tl where tl.TaskUID=t.UID)*100  as decimal(16,0)) as varchar(10))+''%''    
from TaskList as tl1 
where tl1.DocumentUID != ''0'' and tl1.TaskUID=t.UID ) as Per,
(select count(*) from TaskList as tl1
where tl1.DocumentUID != ''0'' and tl1.TaskUID=t.UID and tl1.state=''10'')as TotalCount 
 from task as t where t.state =''10'''; -- 已完成
 exec(@SelectStr);
END
GO
/****** Object:  Table [dbo].[DocumentTag]    Script Date: 08/01/2019 15:50:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DocumentTag](
	[UID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentTagNo] [varchar](128) NULL,
	[DocumentUID] [int] NOT NULL,
	[Tag] [varchar](32) NOT NULL,
	[Creater] [varchar](32) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[EditTime] [datetime] NOT NULL,
	[Remark] [varchar](max) NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_IconTag] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_IconTag] UNIQUE NONCLUSTERED 
(
	[DocumentUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentTag', @level2type=N'COLUMN',@level2name=N'UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentTag', @level2type=N'COLUMN',@level2name=N'DocumentUID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件标签' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentTag', @level2type=N'COLUMN',@level2name=N'Tag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentTag', @level2type=N'COLUMN',@level2name=N'Creater'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentTag', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentTag', @level2type=N'COLUMN',@level2name=N'Editor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentTag', @level2type=N'COLUMN',@level2name=N'EditTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentTag', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocumentTag', @level2type=N'COLUMN',@level2name=N'State'
GO
SET IDENTITY_INSERT [dbo].[DocumentTag] ON
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (1, NULL, 163, N'aa|bb', N'ui', CAST(0x0000A87F00A338CB AS DateTime), N'ui', CAST(0x0000A87F00A338CB AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (2, NULL, 164, N'aa|bb|cc', N'ui', CAST(0x0000A87F00A3398F AS DateTime), N'ui', CAST(0x0000A87F00A3398F AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (3, NULL, 165, N'aa|ccc', N'ui', CAST(0x0000A87F00A339F1 AS DateTime), N'ui', CAST(0x0000A87F00A339F1 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (4, NULL, 166, N'11|2608', N'ui', CAST(0x0000A87F00A33A56 AS DateTime), N'ui', CAST(0x0000A87F00A33A56 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (5, NULL, 167, N'23|23', N'ui', CAST(0x0000A87F00A33AB0 AS DateTime), N'ui', CAST(0x0000A87F00A33AB0 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (6, NULL, 168, N'111|1111', N'ui', CAST(0x0000A87F00A33B3D AS DateTime), N'ui', CAST(0x0000A87F00A33B3D AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (7, NULL, 169, N'33', N'ui', CAST(0x0000A87F00B65A7E AS DateTime), N'ui', CAST(0x0000A87F00B65A7E AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (8, NULL, 170, N'33', N'ui', CAST(0x0000A87F00B65ABA AS DateTime), N'ui', CAST(0x0000A87F00B65ABA AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (9, NULL, 171, N'33', N'ui', CAST(0x0000A87F00B65B20 AS DateTime), N'ui', CAST(0x0000A87F00B65B20 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (10, NULL, 172, N'33', N'ui', CAST(0x0000A87F00B65B50 AS DateTime), N'ui', CAST(0x0000A87F00B65B50 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (11, NULL, 173, N'33', N'ui', CAST(0x0000A87F00B65B98 AS DateTime), N'ui', CAST(0x0000A87F00B65B98 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (14, NULL, 176, N'33', N'ui', CAST(0x0000A87F00B65C91 AS DateTime), N'ui', CAST(0x0000A87F00B65C91 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (15, NULL, 177, N'33', N'ui', CAST(0x0000A87F00B65CDB AS DateTime), N'ui', CAST(0x0000A87F00B65CDB AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (16, NULL, 178, N'33', N'ui', CAST(0x0000A87F00B65D27 AS DateTime), N'ui', CAST(0x0000A87F00B65D27 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (17, NULL, 179, N'44', N'ui', CAST(0x0000A87F00B65D89 AS DateTime), N'ui', CAST(0x0000A87F00B65D89 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (18, NULL, 180, N'55', N'ui', CAST(0x0000A87F00B65E09 AS DateTime), N'ui', CAST(0x0000A88400F98DE0 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (25, NULL, 188, N'323', N'ui', CAST(0x0000A87F00BA3C90 AS DateTime), N'ui', CAST(0x0000A87F00BA3C90 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (28, NULL, 193, N'33', N'ui', CAST(0x0000A87F00BCB9C1 AS DateTime), N'ui', CAST(0x0000A87F00BCB9C1 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (29, NULL, 194, N'33', N'ui', CAST(0x0000A87F00FFF9E7 AS DateTime), N'ui', CAST(0x0000A87F00FFF9E7 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (30, NULL, 195, N'33', N'ui', CAST(0x0000A87F01001330 AS DateTime), N'ui', CAST(0x0000A87F01001330 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (31, NULL, 196, N'555', N'ui', CAST(0x0000A87F010014DF AS DateTime), N'ui', CAST(0x0000A87F010014DF AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (32, NULL, 199, N'44', N'ui', CAST(0x0000A881011E21AB AS DateTime), N'ui', CAST(0x0000A881011E21AB AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (57, NULL, 257, N'', N'ui', CAST(0x0000A8820107B34D AS DateTime), N'ui', CAST(0x0000A8820107B34D AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (58, NULL, 258, N'', N'ui', CAST(0x0000A8820107B37D AS DateTime), N'ui', CAST(0x0000A8820107B37D AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (59, NULL, 259, N'', N'ui', CAST(0x0000A8820107B3A2 AS DateTime), N'ui', CAST(0x0000A8820107B3A2 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (60, NULL, 260, N'', N'ui', CAST(0x0000A882010EEA29 AS DateTime), N'ui', CAST(0x0000A882010EEA29 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (61, NULL, 261, N'', N'ui', CAST(0x0000A882010EEA4C AS DateTime), N'ui', CAST(0x0000A882010EEA4C AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (62, NULL, 262, N'', N'ui', CAST(0x0000A882010EEA6E AS DateTime), N'ui', CAST(0x0000A882010EEA6E AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (63, NULL, 263, N'', N'ui', CAST(0x0000A882010EEA98 AS DateTime), N'ui', CAST(0x0000A882010EEA98 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (64, NULL, 264, N'', N'ui', CAST(0x0000A8820112ABE4 AS DateTime), N'ui', CAST(0x0000A8820112ABE4 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (65, NULL, 265, N'', N'ui', CAST(0x0000A8820112AC05 AS DateTime), N'ui', CAST(0x0000A8820112AC05 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (66, NULL, 266, N'', N'ui', CAST(0x0000A8820112AC2C AS DateTime), N'ui', CAST(0x0000A8820112AC2C AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (67, NULL, 267, N'', N'ui', CAST(0x0000A8820112AC75 AS DateTime), N'ui', CAST(0x0000A8820112AC75 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (68, NULL, 66, N'', N'ui', CAST(0x0000A88F00C335DB AS DateTime), N'ui', CAST(0x0000A88F00C335DB AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (69, NULL, 67, N'', N'ui', CAST(0x0000A88F00C33694 AS DateTime), N'ui', CAST(0x0000A88F00C33694 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (70, NULL, 68, N'', N'ui', CAST(0x0000A88F00C336D7 AS DateTime), N'ui', CAST(0x0000A88F00C336D7 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (71, NULL, 69, N'', N'ui', CAST(0x0000A88F00C33759 AS DateTime), N'ui', CAST(0x0000A88F00C33759 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (72, NULL, 70, N'', N'ui', CAST(0x0000A88F00C3379E AS DateTime), N'ui', CAST(0x0000A88F00C3379E AS DateTime), NULL, 20)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (73, NULL, 71, N'', N'ui', CAST(0x0000A88F00C337FD AS DateTime), N'ui', CAST(0x0000A88F00C337FD AS DateTime), NULL, 20)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (74, NULL, 72, N'', N'ui', CAST(0x0000A89000F840FD AS DateTime), N'ui', CAST(0x0000A89000F840FD AS DateTime), NULL, 20)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (75, NULL, 73, N'', N'ui', CAST(0x0000A89000F84338 AS DateTime), N'ui', CAST(0x0000A89000F84338 AS DateTime), NULL, 20)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (76, NULL, 74, N'', N'ui', CAST(0x0000A89000F845D0 AS DateTime), N'ui', CAST(0x0000A89000F845D0 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (77, NULL, 75, N'', N'ui', CAST(0x0000A89000F848C8 AS DateTime), N'ui', CAST(0x0000A89000F848C8 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (78, NULL, 76, N'', N'ui', CAST(0x0000A89300ACA9AF AS DateTime), N'ui', CAST(0x0000A89300ACA9AF AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (79, NULL, 77, N'', N'ui', CAST(0x0000A89300ACAD46 AS DateTime), N'ui', CAST(0x0000A89300ACAD46 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (80, NULL, 78, N'', N'ui', CAST(0x0000A89300ACAFC0 AS DateTime), N'ui', CAST(0x0000A89300ACAFC0 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (81, NULL, 79, N'', N'ui', CAST(0x0000A89300ACB5D5 AS DateTime), N'ui', CAST(0x0000A89300ACB5D5 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (82, NULL, 80, N'', N'ui', CAST(0x0000A89300ACDD76 AS DateTime), N'ui', CAST(0x0000A89300ACDD76 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (83, NULL, 81, N'', N'ui', CAST(0x0000A89300ACE1CE AS DateTime), N'ui', CAST(0x0000A89300ACE1CE AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (84, NULL, 82, N'', N'ui', CAST(0x0000A89300ACE5D3 AS DateTime), N'ui', CAST(0x0000A89300ACE5D3 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (85, NULL, 83, N'', N'ui', CAST(0x0000A89300ACE910 AS DateTime), N'ui', CAST(0x0000A89300ACE910 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (86, NULL, 84, N'', N'ui', CAST(0x0000A89300ACEBD6 AS DateTime), N'ui', CAST(0x0000A89300ACEBD6 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (87, NULL, 85, N'', N'ui', CAST(0x0000A89300ACEE56 AS DateTime), N'ui', CAST(0x0000A89300ACEE56 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (88, NULL, 86, N'', N'ui', CAST(0x0000A89300ACF12D AS DateTime), N'ui', CAST(0x0000A89300ACF12D AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (89, NULL, 87, N'', N'ui', CAST(0x0000A89300ACF40F AS DateTime), N'ui', CAST(0x0000A89300ACF40F AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (90, NULL, 88, N'', N'ui', CAST(0x0000A89300C55679 AS DateTime), N'ui', CAST(0x0000A89300C55679 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (91, NULL, 89, N'', N'ui', CAST(0x0000A89300C55E6D AS DateTime), N'ui', CAST(0x0000A89300C55E6D AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (92, NULL, 90, N'', N'ui', CAST(0x0000A89300C56480 AS DateTime), N'ui', CAST(0x0000A89300C56480 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (93, NULL, 91, N'', N'ui', CAST(0x0000A89300C56DBA AS DateTime), N'ui', CAST(0x0000A89300C56DBA AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (94, NULL, 92, N'', N'ui', CAST(0x0000A89300C57452 AS DateTime), N'ui', CAST(0x0000A89300C57452 AS DateTime), NULL, 10)
INSERT [dbo].[DocumentTag] ([UID], [DocumentTagNo], [DocumentUID], [Tag], [Creater], [CreateTime], [Editor], [EditTime], [Remark], [State]) VALUES (95, NULL, 93, N'', N'ui', CAST(0x0000A89300C57B96 AS DateTime), N'ui', CAST(0x0000A89300C57B96 AS DateTime), NULL, 10)
SET IDENTITY_INSERT [dbo].[DocumentTag] OFF
/****** Object:  Table [dbo].[Version]    Script Date: 08/01/2019 15:50:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Version](
	[UID] [int] IDENTITY(1,1) NOT NULL,
	[VersionNo] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[IconID] [varchar](128) NOT NULL,
	[VersionName] [varchar](128) NOT NULL,
	[Remark] [varchar](max) NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_Version] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Version] UNIQUE NONCLUSTERED 
(
	[VersionNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Version', @level2type=N'COLUMN',@level2name=N'UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Version', @level2type=N'COLUMN',@level2name=N'VersionNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Version', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图标编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Version', @level2type=N'COLUMN',@level2name=N'IconID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Version', @level2type=N'COLUMN',@level2name=N'VersionName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Version', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Version', @level2type=N'COLUMN',@level2name=N'State'
GO
/****** Object:  Table [dbo].[UserPermission]    Script Date: 08/01/2019 15:50:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserPermission](
	[UID] [int] NOT NULL,
	[UserPermissionNo] [int] NOT NULL,
	[UserNo] [int] NOT NULL,
	[PermissionID] [int] NOT NULL,
	[Pdownload] [int] NOT NULL,
	[Pupload] [int] NOT NULL,
	[Pmodify] [int] NOT NULL,
	[Pdelete] [int] NOT NULL,
	[Creater] [varchar](32) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[EditTime] [datetime] NOT NULL,
	[State] [int] NOT NULL,
	[Remark] [varchar](max) NULL,
 CONSTRAINT [PK_User_Permission] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_User_Permission] UNIQUE NONCLUSTERED 
(
	[UserPermissionNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标识符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'UserPermissionNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'UserNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'PermissionID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下载权限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'Pdownload'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上传权限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'Pupload'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改权限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'Pmodify'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除权限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'Pdelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'Creater'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'Editor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'EditTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态标志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPermission', @level2type=N'COLUMN',@level2name=N'Remark'
GO
/****** Object:  StoredProcedure [dbo].[User_Query]    Script Date: 08/01/2019 15:50:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[User_Query](
	@userName varchar(32)
)
AS
BEGIN
	SELECT [Password] 
	FROM [dbo].[User]
	WHERE [UserName] = @userName
END
GO
/****** Object:  Default [DF_Document_DocumentNo]    Script Date: 08/01/2019 15:50:39 ******/
ALTER TABLE [dbo].[Document] ADD  CONSTRAINT [DF_Document_DocumentNo]  DEFAULT ([dbo].[GetNextTN]('Document')) FOR [DocumentNo]
GO
