SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ContextKey] [nvarchar](300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AksesWebSite](
	[Tanggal] [date] NULL,
	[NamaPage] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[JumlahHitStr] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[JumlahHit] [decimal](18, 0) NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aplikasi](
	[AplikasiId] [uniqueidentifier] NOT NULL,
	[NamaAplikasi] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Pemilik] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_dbo.Aplikasi] PRIMARY KEY CLUSTERED 
(
	[AplikasiId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InisiasiInputProperty](
	[InisiasiInputPropertyId] [uniqueidentifier] NOT NULL,
	[MenuId] [uniqueidentifier] NOT NULL,
	[AreaInput] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NamaProperty] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CSharpScript] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Param1] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Param2] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Param3] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Method] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NamaClass] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NoUrut] [int] NULL,
 CONSTRAINT [PK_dbo.InisiasiInputProperty] PRIMARY KEY CLUSTERED 
(
	[InisiasiInputPropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KondisiCRUD](
	[KondisiCRUDId] [uniqueidentifier] NOT NULL,
	[MenuId] [uniqueidentifier] NOT NULL,
	[CRUD] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AreaInput] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Key] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Value] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ClassView] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Operator] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_dbo.KondisiCRUD] PRIMARY KEY CLUSTERED 
(
	[KondisiCRUDId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[MenuId] [uniqueidentifier] NOT NULL,
	[AplikasiId] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NOT NULL,
	[LinkText] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ActionName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ControllerName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Area] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HtmlAttribute] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[InRole] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[KodeMenu] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[classHeaderView] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[classHeaderTable] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[classDetailView] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[classDetailTable] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NamaTabelHeader] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NamaTabelDetail] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NamaView] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ConnectionString] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FieldTahunBuku] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FieldNomorInput] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FieldKeyHeader] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FieldKeyDetail] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FieldNomorInputView] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[bolehBuatBaru] [bit] NOT NULL,
	[ConnectionStringServerLain] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ScriptTriggerHeaderServerLain] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ScriptTriggerDetailServerLain] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NamaHttpContext] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TanggalKadaluarsa] [datetime] NULL,
	[ListViewName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_dbo.Menu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuX](
	[MenuId] [uniqueidentifier] NOT NULL,
	[AplikasiId] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NOT NULL,
	[LinkText] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ActionName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ControllerName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Area] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HtmlAttribute] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[InRole] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[KodeMenu] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[classHeaderView] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[classHeaderTable] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[classDetailView] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[classDetailTable] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NamaTabelHeader] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NamaTabelDetail] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NamaView] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ConnectionString] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FieldTahunBuku] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FieldNomorInput] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FieldKeyHeader] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FieldKeyDetail] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FieldNomorInputView] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[bolehBuatBaru] [bit] NOT NULL,
	[ConnectionStringServerLain] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ScriptTriggerHeaderServerLain] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ScriptTriggerDetailServerLain] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NamaHttpContext] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TanggalKadaluarsa] [datetime] NULL,
	[ListViewName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RiwayatAkses](
	[RiwayatAksesId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TanggalAkses] [datetime] NOT NULL,
	[PageYangDiakses] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.RiwayatAkses] PRIMARY KEY CLUSTERED 
(
	[RiwayatAksesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RiwayatAksesKaryawan](
	[Tahun] [int] NULL,
	[UserName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)

GO
CREATE NONCLUSTERED INDEX [IX_AplikasiId] ON [dbo].[Menu]
(
	[AplikasiId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
ALTER TABLE [dbo].[Aplikasi] ADD  DEFAULT ('81889704-ab3f-437a-9d43-e61e4f2a5f88') FOR [AplikasiId]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ('38f9b43a-af7e-4549-abb9-e44e11c4f9e8') FOR [MenuId]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [AplikasiId]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [ParentId]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ((0)) FOR [bolehBuatBaru]
GO
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_TanggalKadaluarsa]  DEFAULT (getdate()) FOR [TanggalKadaluarsa]
GO
ALTER TABLE [dbo].[RiwayatAkses] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [Id]
GO
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Menu_dbo.Aplikasi_AplikasiId] FOREIGN KEY([AplikasiId])
REFERENCES [dbo].[Aplikasi] ([AplikasiId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Menu] CHECK CONSTRAINT [FK_dbo.Menu_dbo.Aplikasi_AplikasiId]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER RiwayatAkses_AfterInsert
   ON dbo.RiwayatAkses
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @userName as nvarchar(max)
	declare @id as uniqueidentifier
	declare curs cursor for SELECT [UserName] from inserted 
	open curs
	fetch next from curs into @userName
	while(@@FETCH_STATUS<>-1)
	begin
		set @id=(select top 1 [Id] from [Identity].[dbo].AspNetUsers where UserName=@userName)
		update [PortalPTPN8].[dbo].[RiwayatAkses] set Id=@id where UserName=@userName
		fetch next from curs into @userName
	end
	close curs
	deallocate curs
    -- Insert statements for trigger here

END

GO
ALTER TABLE [dbo].[RiwayatAkses] DISABLE TRIGGER [RiwayatAkses_AfterInsert]
GO
