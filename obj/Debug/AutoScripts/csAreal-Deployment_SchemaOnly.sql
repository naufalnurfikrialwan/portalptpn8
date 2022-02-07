SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[func_Split] 
    (   
    @DelimitedString    varchar(8000),
    @Delimiter              varchar(100) 
    )
RETURNS @tblArray TABLE
    (
    ElementID   int IDENTITY(1,1),  -- Array index
    Element     varchar(1000)               -- Array element contents
    )
AS
BEGIN

    -- Local Variable Declarations
    -- ---------------------------
    DECLARE @Index      smallint,
                    @Start      smallint,
                    @DelSize    smallint

    SET @DelSize = LEN(@Delimiter)

    -- Loop through source string and add elements to destination table array
    -- ----------------------------------------------------------------------
    WHILE LEN(@DelimitedString) > 0
    BEGIN

        SET @Index = CHARINDEX(@Delimiter, @DelimitedString)

        IF @Index = 0
            BEGIN

                INSERT INTO
                    @tblArray 
                    (Element)
                VALUES
                    (LTRIM(RTRIM(@DelimitedString)))

                BREAK
            END
        ELSE
            BEGIN

                INSERT INTO
                    @tblArray 
                    (Element)
                VALUES
                    (LTRIM(RTRIM(SUBSTRING(@DelimitedString, 1,@Index - 1))))

                SET @Start = @Index + @DelSize
                SET @DelimitedString = SUBSTRING(@DelimitedString, @Start , LEN(@DelimitedString) - @Start + 1)

            END
    END

    RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ContextKey] [nvarchar](300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [xml] NOT NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArealEFGabungPembanding](
	[KodeLokasi] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TahunTanam] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[GroupMesin] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[KodeBarang] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[R_BI_HKKerja] [decimal](18, 2) NULL,
	[R_BI_HKSosial] [decimal](18, 2) NULL,
	[R_BI_JumlahUpah] [decimal](18, 2) NULL,
	[R_BI_HasilPanen] [decimal](18, 2) NULL,
	[R_BI_HasilPanenLump] [decimal](18, 2) NULL,
	[R_BI_HasilPanenTBS] [decimal](18, 2) NULL,
	[R_BI_Jelajah] [decimal](18, 2) NULL,
	[R_BI_JumlahPohon] [decimal](18, 2) NULL,
	[R_BI_JelajahMesin] [decimal](18, 2) NULL,
	[R_BI_HasilPemelliharaan] [decimal](18, 2) NULL,
	[R_BI_HasilLainLain] [decimal](18, 2) NULL,
	[R_BI_FisikTUP] [decimal](18, 2) NULL,
	[R_BI_NilaiTUP] [decimal](18, 2) NULL,
	[R_BI_NilaiKas] [decimal](18, 2) NULL,
	[R_BI_FisikKas] [decimal](18, 2) NULL,
	[R_BI_NilaiEAP] [decimal](18, 2) NULL,
	[R_BI_FisikEAP] [decimal](18, 2) NULL,
	[R_BI_NilaiLL] [decimal](18, 2) NULL,
	[R_BI_FisikLL] [decimal](18, 2) NULL,
	[R_SBI_HKKerja] [decimal](18, 2) NULL,
	[R_SBI_HKSosial] [decimal](18, 2) NULL,
	[R_SBI_JumlahUpah] [decimal](18, 2) NULL,
	[R_SBI_HasilPanen] [decimal](18, 2) NULL,
	[R_SBI_HasilPanenLump] [decimal](18, 2) NULL,
	[R_SBI_HasilPanenTBS] [decimal](18, 2) NULL,
	[R_SBI_Jelajah] [decimal](18, 2) NULL,
	[R_SBI_JumlahPohon] [decimal](18, 2) NULL,
	[R_SBI_JelajahMesin] [decimal](18, 2) NULL,
	[R_SBI_HasilPemelliharaan] [decimal](18, 2) NULL,
	[R_SBI_HasilLainLain] [decimal](18, 2) NULL,
	[R_SBI_FisikTUP] [decimal](18, 2) NULL,
	[R_SBI_NilaiTUP] [decimal](18, 2) NULL,
	[R_SBI_NilaiKas] [decimal](18, 2) NULL,
	[R_SBI_FisikKas] [decimal](18, 2) NULL,
	[R_SBI_NilaiEAP] [decimal](18, 2) NULL,
	[R_SBI_FisikEAP] [decimal](18, 2) NULL,
	[R_SBI_NilaiLL] [decimal](18, 2) NULL,
	[R_SBI_FisikLL] [decimal](18, 2) NULL,
	[A_BI_HKKerja] [decimal](18, 2) NULL,
	[A_BI_HKSosial] [decimal](18, 2) NULL,
	[A_BI_JumlahUpah] [decimal](18, 2) NULL,
	[A_BI_HasilPanen] [decimal](18, 2) NULL,
	[A_BI_HasilPanenLump] [decimal](18, 2) NULL,
	[A_BI_HasilPanenTBS] [decimal](18, 2) NULL,
	[A_BI_Jelajah] [decimal](18, 2) NULL,
	[A_BI_JumlahPohon] [decimal](18, 2) NULL,
	[A_BI_JelajahMesin] [decimal](18, 2) NULL,
	[A_BI_HasilPemelliharaan] [decimal](18, 2) NULL,
	[A_BI_HasilLainLain] [decimal](18, 2) NULL,
	[A_BI_FisikTUP] [decimal](18, 2) NULL,
	[A_BI_NilaiTUP] [decimal](18, 2) NULL,
	[A_BI_NilaiKas] [decimal](18, 2) NULL,
	[A_BI_FisikKas] [decimal](18, 2) NULL,
	[A_BI_NilaiEAP] [decimal](18, 2) NULL,
	[A_BI_FisikEAP] [decimal](18, 2) NULL,
	[A_BI_NilaiLL] [decimal](18, 2) NULL,
	[A_BI_FisikLL] [decimal](18, 2) NULL,
	[A_SBI_HKKerja] [decimal](18, 2) NULL,
	[A_SBI_HKSosial] [decimal](18, 2) NULL,
	[A_SBI_JumlahUpah] [decimal](18, 2) NULL,
	[A_SBI_HasilPanen] [decimal](18, 2) NULL,
	[A_SBI_HasilPanenLump] [decimal](18, 2) NULL,
	[A_SBI_HasilPanenTBS] [decimal](18, 2) NULL,
	[A_SBI_Jelajah] [decimal](18, 2) NULL,
	[A_SBI_JumlahPohon] [decimal](18, 2) NULL,
	[A_SBI_JelajahMesin] [decimal](18, 2) NULL,
	[A_SBI_HasilPemelliharaan] [decimal](18, 2) NULL,
	[A_SBI_HasilLainLain] [decimal](18, 2) NULL,
	[A_SBI_FisikTUP] [decimal](18, 2) NULL,
	[A_SBI_NilaiTUP] [decimal](18, 2) NULL,
	[A_SBI_NilaiKas] [decimal](18, 2) NULL,
	[A_SBI_FisikKas] [decimal](18, 2) NULL,
	[A_SBI_NilaiEAP] [decimal](18, 2) NULL,
	[A_SBI_FisikEAP] [decimal](18, 2) NULL,
	[A_SBI_NilaiLL] [decimal](18, 2) NULL,
	[A_SBI_FisikLL] [decimal](18, 2) NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArealEFGabungRealisasi](
	[KodeLokasi] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TahunTanam] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[GroupMesin] [xml] NULL,
	[KodeBarang] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[R_BI_HKKerja] [decimal](18, 2) NULL,
	[R_BI_HKSosial] [decimal](18, 2) NULL,
	[R_BI_JumlahUpah] [decimal](18, 2) NULL,
	[R_BI_HasilPanen] [decimal](18, 2) NULL,
	[R_BI_HasilPanenLump] [decimal](18, 2) NULL,
	[R_BI_HasilPanenTBS] [decimal](18, 2) NULL,
	[R_BI_Jelajah] [decimal](18, 2) NULL,
	[R_BI_JumlahPohon] [decimal](18, 2) NULL,
	[R_BI_JelajahMesin] [decimal](18, 2) NULL,
	[R_BI_HasilPemelliharaan] [decimal](18, 2) NULL,
	[R_BI_HasilLainLain] [decimal](18, 2) NULL,
	[R_BI_FisikTUP] [decimal](18, 2) NULL,
	[R_BI_NilaiTUP] [decimal](18, 2) NULL,
	[R_BI_NilaiKas] [decimal](18, 2) NULL,
	[R_BI_FisikKas] [decimal](18, 2) NULL,
	[R_BI_NilaiEAP] [decimal](18, 2) NULL,
	[R_BI_FisikEAP] [decimal](18, 2) NULL,
	[R_BI_NilaiLL] [decimal](18, 2) NULL,
	[R_BI_FisikLL] [decimal](18, 2) NULL,
	[R_SBI_HKKerja] [decimal](18, 2) NULL,
	[R_SBI_HKSosial] [decimal](18, 2) NULL,
	[R_SBI_JumlahUpah] [decimal](18, 2) NULL,
	[R_SBI_HasilPanen] [decimal](18, 2) NULL,
	[R_SBI_HasilPanenLump] [decimal](18, 2) NULL,
	[R_SBI_HasilPanenTBS] [decimal](18, 2) NULL,
	[R_SBI_Jelajah] [decimal](18, 2) NULL,
	[R_SBI_JumlahPohon] [decimal](18, 2) NULL,
	[R_SBI_JelajahMesin] [decimal](18, 2) NULL,
	[R_SBI_HasilPemelliharaan] [decimal](18, 2) NULL,
	[R_SBI_HasilLainLain] [decimal](18, 2) NULL,
	[R_SBI_FisikTUP] [decimal](18, 2) NULL,
	[R_SBI_NilaiTUP] [decimal](18, 2) NULL,
	[R_SBI_NilaiKas] [decimal](18, 2) NULL,
	[R_SBI_FisikKas] [decimal](18, 2) NULL,
	[R_SBI_NilaiEAP] [decimal](18, 2) NULL,
	[R_SBI_FisikEAP] [decimal](18, 2) NULL,
	[R_SBI_NilaiLL] [decimal](18, 2) NULL,
	[R_SBI_FisikLL] [decimal](18, 2) NULL,
	[A_BI_HKKerja] [decimal](18, 2) NULL,
	[A_BI_HKSosial] [decimal](18, 2) NULL,
	[A_BI_JumlahUpah] [decimal](18, 2) NULL,
	[A_BI_HasilPanen] [decimal](18, 2) NULL,
	[A_BI_HasilPanenLump] [decimal](18, 2) NULL,
	[A_BI_HasilPanenTBS] [decimal](18, 2) NULL,
	[A_BI_Jelajah] [decimal](18, 2) NULL,
	[A_BI_JumlahPohon] [decimal](18, 2) NULL,
	[A_BI_JelajahMesin] [decimal](18, 2) NULL,
	[A_BI_HasilPemelliharaan] [decimal](18, 2) NULL,
	[A_BI_HasilLainLain] [decimal](18, 2) NULL,
	[A_BI_FisikTUP] [decimal](18, 2) NULL,
	[A_BI_NilaiTUP] [decimal](18, 2) NULL,
	[A_BI_NilaiKas] [decimal](18, 2) NULL,
	[A_BI_FisikKas] [decimal](18, 2) NULL,
	[A_BI_NilaiEAP] [decimal](18, 2) NULL,
	[A_BI_FisikEAP] [decimal](18, 2) NULL,
	[A_BI_NilaiLL] [decimal](18, 2) NULL,
	[A_BI_FisikLL] [decimal](18, 2) NULL,
	[A_SBI_HKKerja] [decimal](18, 2) NULL,
	[A_SBI_HKSosial] [decimal](18, 2) NULL,
	[A_SBI_JumlahUpah] [decimal](18, 2) NULL,
	[A_SBI_HasilPanen] [decimal](18, 2) NULL,
	[A_SBI_HasilPanenLump] [decimal](18, 2) NULL,
	[A_SBI_HasilPanenTBS] [decimal](18, 2) NULL,
	[A_SBI_Jelajah] [decimal](18, 2) NULL,
	[A_SBI_JumlahPohon] [decimal](18, 2) NULL,
	[A_SBI_JelajahMesin] [decimal](18, 2) NULL,
	[A_SBI_HasilPemelliharaan] [decimal](18, 2) NULL,
	[A_SBI_HasilLainLain] [decimal](18, 2) NULL,
	[A_SBI_FisikTUP] [decimal](18, 2) NULL,
	[A_SBI_NilaiTUP] [decimal](18, 2) NULL,
	[A_SBI_NilaiKas] [decimal](18, 2) NULL,
	[A_SBI_FisikKas] [decimal](18, 2) NULL,
	[A_SBI_NilaiEAP] [decimal](18, 2) NULL,
	[A_SBI_FisikEAP] [decimal](18, 2) NULL,
	[A_SBI_NilaiLL] [decimal](18, 2) NULL,
	[A_SBI_FisikLL] [decimal](18, 2) NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArealEFRealisasi](
	[KodeLokasi] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[R_BI_HKKerja] [decimal](38, 2) NULL,
	[R_BI_HKSosial] [decimal](38, 2) NULL,
	[R_BI_JumlahUpah] [decimal](38, 2) NULL,
	[R_BI_HasilPanen] [decimal](38, 2) NULL,
	[R_BI_HasilPanenLump] [decimal](38, 2) NULL,
	[R_BI_HasilPanenTBS] [decimal](38, 2) NULL,
	[R_BI_Jelajah] [decimal](38, 2) NULL,
	[R_BI_JumlahPohon] [decimal](38, 2) NULL,
	[R_BI_JelajahMesin] [decimal](38, 2) NULL,
	[R_BI_HasilPemelliharaan] [decimal](38, 2) NULL,
	[R_BI_HasilLainLain] [decimal](38, 2) NULL,
	[R_BI_FisikTUP] [decimal](38, 2) NULL,
	[R_BI_NilaiTUP] [decimal](38, 2) NULL,
	[R_BI_NilaiKas] [decimal](38, 2) NULL,
	[R_BI_FisikKas] [decimal](38, 2) NULL,
	[R_BI_NilaiEAP] [decimal](38, 2) NULL,
	[R_BI_FisikEAP] [decimal](38, 2) NULL,
	[R_BI_NilaiLL] [decimal](38, 2) NULL,
	[R_BI_FisikLL] [decimal](38, 2) NULL,
	[R_SBI_HKKerja] [decimal](38, 2) NULL,
	[R_SBI_HKSosial] [decimal](38, 2) NULL,
	[R_SBI_JumlahUpah] [decimal](38, 2) NULL,
	[R_SBI_HasilPanen] [decimal](38, 2) NULL,
	[R_SBI_HasilPanenLump] [decimal](38, 2) NULL,
	[R_SBI_HasilPanenTBS] [decimal](38, 2) NULL,
	[R_SBI_Jelajah] [decimal](38, 2) NULL,
	[R_SBI_JumlahPohon] [decimal](38, 2) NULL,
	[R_SBI_JelajahMesin] [decimal](38, 2) NULL,
	[R_SBI_HasilPemelliharaan] [decimal](38, 2) NULL,
	[R_SBI_HasilLainLain] [decimal](38, 2) NULL,
	[R_SBI_FisikTUP] [decimal](38, 2) NULL,
	[R_SBI_NilaiTUP] [decimal](38, 2) NULL,
	[R_SBI_NilaiKas] [decimal](38, 2) NULL,
	[R_SBI_FisikKas] [decimal](38, 2) NULL,
	[R_SBI_NilaiEAP] [decimal](38, 2) NULL,
	[R_SBI_FisikEAP] [decimal](38, 2) NULL,
	[R_SBI_NilaiLL] [decimal](38, 2) NULL,
	[R_SBI_FisikLL] [decimal](38, 2) NULL,
	[A_BI_HKKerja] [decimal](38, 2) NULL,
	[A_BI_HKSosial] [decimal](38, 2) NULL,
	[A_BI_JumlahUpah] [decimal](38, 2) NULL,
	[A_BI_HasilPanen] [decimal](38, 2) NULL,
	[A_BI_HasilPanenLump] [decimal](38, 2) NULL,
	[A_BI_HasilPanenTBS] [decimal](38, 2) NULL,
	[A_BI_Jelajah] [decimal](38, 2) NULL,
	[A_BI_JumlahPohon] [decimal](38, 2) NULL,
	[A_BI_JelajahMesin] [decimal](38, 2) NULL,
	[A_BI_HasilPemelliharaan] [decimal](38, 2) NULL,
	[A_BI_HasilLainLain] [decimal](38, 2) NULL,
	[A_BI_FisikTUP] [decimal](38, 2) NULL,
	[A_BI_NilaiTUP] [decimal](38, 2) NULL,
	[A_BI_NilaiKas] [decimal](38, 2) NULL,
	[A_BI_FisikKas] [decimal](38, 2) NULL,
	[A_BI_NilaiEAP] [decimal](38, 2) NULL,
	[A_BI_FisikEAP] [decimal](38, 2) NULL,
	[A_BI_NilaiLL] [decimal](38, 2) NULL,
	[A_BI_FisikLL] [decimal](38, 2) NULL,
	[A_SBI_HKKerja] [decimal](38, 2) NULL,
	[A_SBI_HKSosial] [decimal](38, 2) NULL,
	[A_SBI_JumlahUpah] [decimal](38, 2) NULL,
	[A_SBI_HasilPanen] [decimal](38, 2) NULL,
	[A_SBI_HasilPanenLump] [decimal](38, 2) NULL,
	[A_SBI_HasilPanenTBS] [decimal](38, 2) NULL,
	[A_SBI_Jelajah] [decimal](38, 2) NULL,
	[A_SBI_JumlahPohon] [decimal](38, 2) NULL,
	[A_SBI_JelajahMesin] [decimal](38, 2) NULL,
	[A_SBI_HasilPemelliharaan] [decimal](38, 2) NULL,
	[A_SBI_HasilLainLain] [decimal](38, 2) NULL,
	[A_SBI_FisikTUP] [decimal](38, 2) NULL,
	[A_SBI_NilaiTUP] [decimal](38, 2) NULL,
	[A_SBI_NilaiKas] [decimal](38, 2) NULL,
	[A_SBI_FisikKas] [decimal](38, 2) NULL,
	[A_SBI_NilaiEAP] [decimal](38, 2) NULL,
	[A_SBI_FisikEAP] [decimal](38, 2) NULL,
	[A_SBI_NilaiLL] [decimal](38, 2) NULL,
	[A_SBI_FisikLL] [decimal](38, 2) NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArealEFRekapPembanding](
	[KodeLokasi] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[R_BI_HKKerja] [decimal](38, 2) NULL,
	[R_BI_HKSosial] [decimal](38, 2) NULL,
	[R_BI_JumlahUpah] [decimal](38, 2) NULL,
	[R_BI_HasilPanen] [decimal](38, 2) NULL,
	[R_BI_HasilPanenLump] [decimal](38, 2) NULL,
	[R_BI_HasilPanenTBS] [decimal](38, 2) NULL,
	[R_BI_Jelajah] [decimal](38, 2) NULL,
	[R_BI_JumlahPohon] [decimal](38, 2) NULL,
	[R_BI_JelajahMesin] [decimal](38, 2) NULL,
	[R_BI_HasilPemelliharaan] [decimal](38, 2) NULL,
	[R_BI_HasilLainLain] [decimal](38, 2) NULL,
	[R_BI_FisikTUP] [decimal](38, 2) NULL,
	[R_BI_NilaiTUP] [decimal](38, 2) NULL,
	[R_BI_NilaiKas] [decimal](38, 2) NULL,
	[R_BI_FisikKas] [decimal](38, 2) NULL,
	[R_BI_NilaiEAP] [decimal](38, 2) NULL,
	[R_BI_FisikEAP] [decimal](38, 2) NULL,
	[R_BI_NilaiLL] [decimal](38, 2) NULL,
	[R_BI_FisikLL] [decimal](38, 2) NULL,
	[R_SBI_HKKerja] [decimal](38, 2) NULL,
	[R_SBI_HKSosial] [decimal](38, 2) NULL,
	[R_SBI_JumlahUpah] [decimal](38, 2) NULL,
	[R_SBI_HasilPanen] [decimal](38, 2) NULL,
	[R_SBI_HasilPanenLump] [decimal](38, 2) NULL,
	[R_SBI_HasilPanenTBS] [decimal](38, 2) NULL,
	[R_SBI_Jelajah] [decimal](38, 2) NULL,
	[R_SBI_JumlahPohon] [decimal](38, 2) NULL,
	[R_SBI_JelajahMesin] [decimal](38, 2) NULL,
	[R_SBI_HasilPemelliharaan] [decimal](38, 2) NULL,
	[R_SBI_HasilLainLain] [decimal](38, 2) NULL,
	[R_SBI_FisikTUP] [decimal](38, 2) NULL,
	[R_SBI_NilaiTUP] [decimal](38, 2) NULL,
	[R_SBI_NilaiKas] [decimal](38, 2) NULL,
	[R_SBI_FisikKas] [decimal](38, 2) NULL,
	[R_SBI_NilaiEAP] [decimal](38, 2) NULL,
	[R_SBI_FisikEAP] [decimal](38, 2) NULL,
	[R_SBI_NilaiLL] [decimal](38, 2) NULL,
	[R_SBI_FisikLL] [decimal](38, 2) NULL,
	[A_BI_HKKerja] [decimal](38, 2) NULL,
	[A_BI_HKSosial] [decimal](38, 2) NULL,
	[A_BI_JumlahUpah] [decimal](38, 2) NULL,
	[A_BI_HasilPanen] [decimal](38, 2) NULL,
	[A_BI_HasilPanenLump] [decimal](38, 2) NULL,
	[A_BI_HasilPanenTBS] [decimal](38, 2) NULL,
	[A_BI_Jelajah] [decimal](38, 2) NULL,
	[A_BI_JumlahPohon] [decimal](38, 2) NULL,
	[A_BI_JelajahMesin] [decimal](38, 2) NULL,
	[A_BI_HasilPemelliharaan] [decimal](38, 2) NULL,
	[A_BI_HasilLainLain] [decimal](38, 2) NULL,
	[A_BI_FisikTUP] [decimal](38, 2) NULL,
	[A_BI_NilaiTUP] [decimal](38, 2) NULL,
	[A_BI_NilaiKas] [decimal](38, 2) NULL,
	[A_BI_FisikKas] [decimal](38, 2) NULL,
	[A_BI_NilaiEAP] [decimal](38, 2) NULL,
	[A_BI_FisikEAP] [decimal](38, 2) NULL,
	[A_BI_NilaiLL] [decimal](38, 2) NULL,
	[A_BI_FisikLL] [decimal](38, 2) NULL,
	[A_SBI_HKKerja] [decimal](38, 2) NULL,
	[A_SBI_HKSosial] [decimal](38, 2) NULL,
	[A_SBI_JumlahUpah] [decimal](38, 2) NULL,
	[A_SBI_HasilPanen] [decimal](38, 2) NULL,
	[A_SBI_HasilPanenLump] [decimal](38, 2) NULL,
	[A_SBI_HasilPanenTBS] [decimal](38, 2) NULL,
	[A_SBI_Jelajah] [decimal](38, 2) NULL,
	[A_SBI_JumlahPohon] [decimal](38, 2) NULL,
	[A_SBI_JelajahMesin] [decimal](38, 2) NULL,
	[A_SBI_HasilPemelliharaan] [decimal](38, 2) NULL,
	[A_SBI_HasilLainLain] [decimal](38, 2) NULL,
	[A_SBI_FisikTUP] [decimal](38, 2) NULL,
	[A_SBI_NilaiTUP] [decimal](38, 2) NULL,
	[A_SBI_NilaiKas] [decimal](38, 2) NULL,
	[A_SBI_FisikKas] [decimal](38, 2) NULL,
	[A_SBI_NilaiEAP] [decimal](38, 2) NULL,
	[A_SBI_FisikEAP] [decimal](38, 2) NULL,
	[A_SBI_NilaiLL] [decimal](38, 2) NULL,
	[A_SBI_FisikLL] [decimal](38, 2) NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArealEFViewResult](
	[LokasiId] [uniqueidentifier] NULL,
	[KodeLokasi] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Peta] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Produksi] [decimal](18, 2) NULL,
	[PembandingProduksi] [decimal](18, 2) NULL,
	[LuasTM] [decimal](18, 4) NULL,
	[Nama] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[R_Fisik] [decimal](18, 2) NULL,
	[R_Biaya] [decimal](18, 2) NULL,
	[A_Fisik] [decimal](18, 2) NULL,
	[A_Biaya] [decimal](18, 2) NULL,
	[R_PembandingFisik] [decimal](18, 2) NULL,
	[R_PembandingBiaya] [decimal](18, 2) NULL,
	[A_PembandingFisik] [decimal](18, 2) NULL,
	[A_PembandingBiaya] [decimal](18, 2) NULL,
	[Warna] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[KeteranganWarna] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Jml_Fisik] [decimal](18, 2) NULL,
	[Jml_Biaya] [decimal](18, 2) NULL,
	[Jml_PembandingFisik] [decimal](18, 2) NULL,
	[Jml_PembandingBiaya] [decimal](18, 2) NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LegendPetaKegiatan](
	[LegendPetaKegiatanId] [uniqueidentifier] NOT NULL,
	[MenuId] [uniqueidentifier] NOT NULL,
	[Uraian] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[WarnaDepan] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[WarnaBelakang] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NoUrut] [int] NOT NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[ProjectId] [uniqueidentifier] NOT NULL,
	[BagianId] [uniqueidentifier] NOT NULL,
	[KodeProject] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Tanggal] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Deskripsi] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Mitra] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectBlok](
	[ProjectBlokId] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[BlokId] [uniqueidentifier] NOT NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectProgress](
	[ProjectProgressId] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[NamaKegiatan] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UraianKegiatan] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[StartDate] [smalldatetime] NOT NULL,
	[FinishDate] [smalldatetime] NOT NULL,
	[PIC] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Progress] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StandardProduktivitasKaret](
	[StandardProduktivitasKaretId] [uniqueidentifier] NOT NULL,
	[Umur] [int] NOT NULL,
	[UmurSadap] [int] NOT NULL,
	[TonPerHa] [decimal](18, 2) NOT NULL,
	[Rotasi] [decimal](18, 2) NOT NULL,
	[PohonDisadapPerHanca] [decimal](18, 2) NOT NULL,
	[PctLateks] [decimal](18, 2) NOT NULL,
	[PctLump] [decimal](18, 2) NOT NULL,
	[PctKKK] [decimal](18, 2) NOT NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StandardProduktivitasSawit](
	[StandardProduktivitasSawitId] [uniqueidentifier] NOT NULL,
	[Umur] [int] NOT NULL,
	[TonTBS_S1] [decimal](18, 2) NOT NULL,
	[RBT_S1] [decimal](18, 2) NOT NULL,
	[RJT_S1] [decimal](18, 2) NOT NULL,
	[TonTBS_S2] [decimal](18, 2) NOT NULL,
	[RBT_S2] [decimal](18, 2) NOT NULL,
	[RJT_S2] [decimal](18, 2) NOT NULL,
	[TonTBS_S3] [decimal](18, 2) NOT NULL,
	[RBT_S3] [decimal](18, 2) NOT NULL,
	[RJT_S3] [decimal](18, 2) NOT NULL,
	[Rotasi] [decimal](18, 2) NOT NULL,
	[HKperHa] [decimal](18, 2) NOT NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StandardProduktivitasTeh](
	[StandardProduktivitasTehId] [uniqueidentifier] NOT NULL,
	[KgPerHa] [decimal](18, 2) NOT NULL,
	[Dataran] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RotasiGunting] [decimal](18, 2) NOT NULL,
	[RotasiMesin] [decimal](18, 2) NOT NULL,
	[LuasJelajah] [decimal](18, 2) NOT NULL
)

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
CREATE PROCEDURE [dbo].[PetaView]
@Id as nvarchar(max),						-- kode induk nya, kalo @Layer='Kebun' @Id='', kalo @Layer='Afdeling' @Id=Kode Kebun, kalo @Layer='Blok' @Id = Kode Afdeling
@Layer as nvarchar(max),					-- pilihan nya : 'Kebun', 'Afdeling' atau 'Blok'
@BulanAwal as int,@TahunAwal as int,@BulanAkhir as int, @TahunAkhir as int,
@Budidaya as nvarchar(max),					-- Kode Budidaya
@KodeRekening as nvarchar(max),				-- Kode Rekening
@FieldYangDibandingkan as nvarchar(max),	-- 'HK','PRESTASI','TUP','KAS','LL','SELURUH','PROTAS'
@DibandingkanDengan as nvarchar(max),		-- 'INDUK','RKAP','PKB' atau 'BALAI'
@OrderBulan as nvarchar(max)				-- 'BI' atau 'SBI'
AS
BEGIN
	SET NOCOUNT ON;
	SET DATEFORMAT DMY

	DECLARE @tglAwal AS DATETIME 
	DECLARE @tglAkhir AS DATETIME 
	
	set @tglAwal = CAST('01/'+cast(@BulanAwal as varchar(10))+'/'+cast(@TahunAwal as varchar(10)) as datetime)  
	if @BulanAkhir=12
	begin
		set @tglAkhir = CAST('01/01/'+cast(@TahunAkhir+1 as varchar(10)) as datetime)  
		set @tglAkhir = @tglAkhir-1
	end
	else
	begin
		set @tglAkhir = CAST('01/'+cast(@BulanAkhir+1 as varchar(10))+'/'+cast(@TahunAkhir as varchar(10)) as datetime)  
		set @tglAkhir = @tglAkhir-1
	end

	-- data gaji/upah
	-- dari table GajiAbsensi Absen '1'
	IF OBJECT_ID(N'dbo.ArealEFUpah', N'U') IS NOT NULL
	BEGIN
		drop table dbo.ArealEFUpah
	END
	
    SELECT 
		A.KMK as KMK, 
		A.kodeunit as KodeUnit,
		--A.kodebud1 as KodeBudidaya,
		A.kodeafd1 as KodeAfdeling,
		A.kdblok as KodeBlok,
		month(A.tanggal) as Bulan, 
		year(A.tanggal) as Tahun, 
		A.kdpanen as KodePanen,
		A.thntnm as TahunTanam,
		A.grup as Grup,
		--substring(A.norek,1,LEN(@KodeRekening)) as Norek,
		HKKerja = COUNT(*),
		HKSosial = 0,
		SUM(A.jmlupah) as JumlahUpah, 
		SUM(A.hslpanen) as HasilPanen, 
		SUM(A.hslpanenlump) as HasilPanenLump, 
        SUM(A.hslpanenTBS) as HasilPanenTBS, 
        sum(A.jelajahHA) as Jelajah, 
        sum(A.nbrd) as JumlahPohon, 
        avg(A.jelajahHA) as JelajahMesin,
        SUM(A.hslpemel) as HasilPemelliharaan, 
        SUM(A.hsllain2) as HasilLainLain
        INTO ArealEFUpah FROM [SPDK_KBN_KONSOL].[dbo].[GajiAbsensi] as A 
        where A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
			and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
			and A.kodebud1=@Budidaya and A.kodeabs='1'
			and @Id=substring(A.kodeunit+A.kodeafd1+A.kdblok,1,LEN(@Id))
        group by A.KMK, A.kodeunit, 
			--A.kodebud1, 
			A.kodeafd1, A.kdblok, 
			month(A.tanggal), year(A.tanggal), A.kdpanen, A.thntnm, 
			A.grup
			--,substring(A.norek,1,LEN(@KodeRekening))
			
	-- dari table GajiAbsensi Absen 2,3,4,5,6,9,A,B dan ''
    INSERT INTO ArealEFUpah
    SELECT 
		A.KMK as KMK, 
		A.kodeunit as KodeUnit,
		--A.kodebud1 as KodeBudidaya,
		A.kodeafd1 as KodeAfdeling,
		A.kdblok as KodeBlok,
		month(A.tanggal) as Bulan, 
		year(A.tanggal) as Tahun, 
		A.kdpanen as KodePanen,
		A.thntnm as TahunTanam,
		A.grup as Grup,
		--substring(A.norek,1,LEN(@KodeRekening)) as Norek,
		HKKerja = 0,
		HKSosial = COUNT(*),
		SUM(A.jmlupah) as JumlahUpah, 
		SUM(A.hslpanen) as HasilPanen, 
		SUM(A.hslpanenlump) as HasilPanenLump, 
        SUM(A.hslpanenTBS) as HasilPanenTBS, 
        sum(A.jelajahHA) as Jelajah, 
        sum(A.nbrd) as JumlahPohon, 
        avg(A.jelajahHA) as JelajahMesin,
        SUM(A.hslpemel) as HasilPemelliharaan, 
        SUM(A.hsllain2) as HasilLainLain
        FROM [SPDK_KBN_KONSOL].[dbo].[GajiAbsensi] as A 
        where A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
			and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
			
			and A.kodebud1=@Budidaya 
			and (A.kodeabs='2' or A.kodeabs='3' or A.kodeabs='4' or A.kodeabs='5' or A.kodeabs='6' or A.kodeabs='9' or A.kodeabs='A' or A.kodeabs='B' or A.kodeabs='')
			and @Id=substring(A.kodeunit+A.kodeafd1+A.kdblok,1,LEN(@Id))
        group by A.KMK, A.kodeunit, 
			--A.kodebud1, 
			A.kodeafd1, A.kdblok, 
			month(A.tanggal), year(A.tanggal), A.kdpanen, A.thntnm, 
			A.grup
			--,substring(A.norek,1,LEN(@KodeRekening))
			
	-- dari table GajiKulir 
    INSERT INTO ArealEFUpah
    SELECT 
		KMK = A.KMK, 
		KodeUnit = A.kodeunit,
		--KodeBudidaya = A.kodebud,
		KodeAfdeling = A.kodeafd,
		KodeBlok = A.kdblok,
		Bulan = month(A.tanggal), 
		Tahun = year(A.tanggal), 
		KodePanen = '',
		TahunTanam = A.thntnm,
		Grup = '',
		--Norek = substring(A.norek,1,LEN(@KodeRekening)),
		HKKerja = 0,
		HKSosial = 0,
		JumlahUpah = SUM(A.nilai), 
		HasilPanen = 0, 
		HasilPanenLump = 0,
		HasilPanenTBS = 0,
		Jelajah = 0,
		JumlahPohon = 0,
		JelajahMesin = 0,
		HasilPemelliharaan = 0,
		HasilLainLain = SUM(A.jmlhasil)
		FROM [SPDK_KBN_KONSOL].[dbo].[GajiKulir] as A 
        where A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
			and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
			and A.kodebud=@Budidaya 
			and @Id=substring(A.kodeunit+A.kodeafd+A.kdblok,1,LEN(@Id))
        group by A.KMK, A.kodeunit, 
			--A.kodebud, 
			A.kodeafd, A.kdblok, 
			month(A.tanggal), year(A.tanggal), A.thntnm
			--,substring(A.norek,1,LEN(@KodeRekening))			
		
	-- dari table GajiJurnal -> Gaji IB-IVD ?? norek
    INSERT INTO ArealEFUpah
    SELECT 
		'01' as KMK, 
		A.kodeunit as KodeUnit,
		--A.kodebud as KodeBudidaya,
		A.kodeafd as KodeAfdeling,
		'' as KodeBlok,
		month(A.tanggal) as Bulan, 
		year(A.tanggal) as Tahun, 
		'' as KodePanen,
		A.thntnm as TahunTanam,
		'' as Grup,
		--substring(A.norek,1,LEN(@KodeRekening)) as Norek,
		HKKerja = 0,
		HKSosial = 0,
		SUM(A.nilai) as JumlahUpah, 
		0 as HasilPanen, 
		0 as HasilPanenLump, 
        0 as HasilPanenTBS, 
        0 as Jelajah, 
        0 as JumlahPohon, 
        0 as JelajahMesin,
        0 as HasilPemelliharaan, 
        SUM(A.jmlfisik) as HasilLainLain
        FROM [SPDK_KBN_KONSOL].[dbo].[GajiJurnal] as A 
        where A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
			and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
			and A.kodebud=@Budidaya 
			and @Id=substring(A.kodeunit+A.kodeafd,1,LEN(@Id))
			and A.DK='D'
			and substring(A.nobukti,12,5) in('00101','00102','00103','00104','00105','00106','00108','00109','00112',
				'00115','00201','00202','00203','00204','00301','00302','00303','00304','00305','00401','00402',
				'10101','10102','10104','10105','10108','10109','10110','10111','10112','10201','10202','10203',
				'10301','10302','10303','10304','10305','10401','10402','10116')
        group by A.kodeunit, 
			--A.kodebud, 
			A.kodeafd,
			month(A.tanggal), year(A.tanggal), A.thntnm, 
			A.grup
			--,substring(A.norek,1,LEN(@KodeRekening))
	
	-- dari table GajiPremi 
    INSERT INTO ArealEFUpah
    SELECT 
		KMK = A.KMK, 
		KodeUnit = A.kodeunit,
		--KodeBudidaya = A.kodebud,
		KodeAfdeling = A.kodeafd,
		KodeBlok = '',
		Bulan = month(A.tanggal), 
		Tahun = year(A.tanggal), 
		KodePanen = '',
		TahunTanam = A.thntnm,
		Grup = '',
		--Norek = substring(A.norek,1,LEN(@KodeRekening)),
		HKKerja = 0,
		HKSosial = 0,
		JumlahUpah = SUM(A.nilai), 
		HasilPanen = 0, 
		HasilPanenLump = 0,
		HasilPanenTBS = 0,
		Jelajah = 0,
		JumlahPohon = 0,
		JelajahMesin = 0,
		HasilPemelliharaan = 0,
		HasilLainLain = SUM(A.jmlfisik)
		FROM [SPDK_KBN_KONSOL].[dbo].[GajiPremi] as A 
        where A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
			and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
			and A.kodebud=@Budidaya 
			and @Id=substring(A.kodeunit+A.kodeafd,1,LEN(@Id))
        group by A.KMK, A.kodeunit, 
			--A.kodebud, 
			A.kodeafd,  
			month(A.tanggal), year(A.tanggal), A.thntnm
			--,substring(A.norek,1,LEN(@KodeRekening))			
		
	-- dari table GajiPremiPanen 
    INSERT INTO ArealEFUpah
    SELECT 
		KMK = A.KMK, 
		KodeUnit = A.kodeunit,
		--KodeBudidaya = A.kodebud,
		KodeAfdeling = A.kodeafd,
		KodeBlok = '',
		Bulan = month(A.tanggal), 
		Tahun = year(A.tanggal), 
		KodePanen = A.kodepanen,
		TahunTanam = '',
		Grup = '',
		--Norek = substring(A.norek,1,LEN(@KodeRekening)),
		HKKerja = 0,
		HKSosial = 0,
		JumlahUpah = SUM(A.jmlpremi), 
		HasilPanen = 0, 
		HasilPanenLump = 0,
		HasilPanenTBS = 0,
		Jelajah = 0,
		JumlahPohon = 0,
		JelajahMesin = 0,
		HasilPemelliharaan = 0,
		HasilLainLain = SUM(A.jmlhasil)
		FROM [SPDK_KBN_KONSOL].[dbo].[GajiPremiPanen] as A 
        where A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
			and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
			and A.kodebud=@Budidaya 
			and @Id=substring(A.kodeunit+A.kodeafd,1,LEN(@Id))
        group by A.KMK, A.kodeunit, 
			--A.kodebud, 
			A.kodeafd, A.kodepanen, 
			month(A.tanggal), year(A.tanggal)
			--,substring(A.norek,1,LEN(@KodeRekening))			

	-- dari table GajiSPKHonor
    INSERT INTO ArealEFUpah
    SELECT 
		KMK = A.KMK, 
		KodeUnit = A.kodeunit,
		--KodeBudidaya = A.kodebud,
		KodeAfdeling = A.kodeafd,
		KodeBlok = A.kdblok,
		Bulan = month(A.tanggal), 
		Tahun = year(A.tanggal), 
		KodePanen = '',
		TahunTanam = '',
		Grup = '',
		--Norek = substring(A.norek,1,LEN(@KodeRekening)),
		HKKerja = SUM(A.jmlhadir),
		HKSosial = 0,
		JumlahUpah = SUM(A.jmlupah), 
		HasilPanen = 0, 
		HasilPanenLump = 0,
		HasilPanenTBS = 0,
		Jelajah = 0,
		JumlahPohon = 0,
		JelajahMesin = 0,
		HasilPemelliharaan = 0,
		HasilLainLain = SUM(A.jmlhasil)
		FROM [SPDK_KBN_KONSOL].[dbo].[GajiSPKHonor] as A 
        where A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
			and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
			and A.kodebud=@Budidaya 
			and @Id=substring(A.kodeunit+A.kodeafd,1,LEN(@Id))
        group by A.KMK, A.kodeunit, 
			--A.kodebud, 
			A.kodeafd, A.kdblok, 
			month(A.tanggal), year(A.tanggal)
			--,substring(A.norek,1,LEN(@KodeRekening))			
	
	
	IF OBJECT_ID(N'dbo.ArealEFGabungRealisasi', N'U') IS NOT NULL
	BEGIN
		drop table dbo.ArealEFGabungRealisasi
	END
	CREATE TABLE ArealEFGabungRealisasi
	(
		KodeLokasi nvarchar(100), 
		TahunTanam nvarchar(100),
		--KodeBudidaya nvarchar(100),
		GroupMesin nvarchar(100),
		KodeBarang nvarchar(100),
		R_BI_HKKerja decimal(18,2) null default 0,
		R_BI_HKSosial decimal(18,2) null default 0,
		R_BI_JumlahUpah decimal(18,2) null default 0, 
		R_BI_HasilPanen decimal(18,2) null default 0, 
		R_BI_HasilPanenLump decimal(18,2) null default 0,
		R_BI_HasilPanenTBS decimal(18,2) null default 0,
		R_BI_Jelajah decimal(18,2) null default 0,
		R_BI_JumlahPohon decimal(18,2) null default 0,
		R_BI_JelajahMesin decimal(18,2) null default 0,
		R_BI_HasilPemelliharaan decimal(18,2) null default 0,
		R_BI_HasilLainLain decimal(18,2) null default 0,
		R_BI_FisikTUP decimal(18,2) null default 0,
		R_BI_NilaiTUP decimal(18,2) null default 0,
		R_BI_NilaiKas decimal(18,2) null default 0,
		R_BI_FisikKas decimal(18,2) null default 0,
		R_BI_NilaiEAP decimal(18,2) null default 0,
		R_BI_FisikEAP decimal(18,2) null default 0,
		R_BI_NilaiLL decimal(18,2) null default 0,
		R_BI_FisikLL decimal(18,2) null default 0,
		R_SBI_HKKerja decimal(18,2) null default 0,
		R_SBI_HKSosial decimal(18,2) null default 0,
		R_SBI_JumlahUpah decimal(18,2) null default 0, 
		R_SBI_HasilPanen decimal(18,2) null default 0, 
		R_SBI_HasilPanenLump decimal(18,2) null default 0,
		R_SBI_HasilPanenTBS decimal(18,2) null default 0,
		R_SBI_Jelajah decimal(18,2) null default 0,
		R_SBI_JumlahPohon decimal(18,2) null default 0,
		R_SBI_JelajahMesin decimal(18,2) null default 0,
		R_SBI_HasilPemelliharaan decimal(18,2) null default 0,
		R_SBI_HasilLainLain decimal(18,2) null default 0,
		R_SBI_FisikTUP decimal(18,2) null default 0,
		R_SBI_NilaiTUP decimal(18,2) null default 0,
		R_SBI_NilaiKas decimal(18,2) null default 0,
		R_SBI_FisikKas decimal(18,2) null default 0,
		R_SBI_NilaiEAP decimal(18,2) null default 0,
		R_SBI_FisikEAP decimal(18,2) null default 0,
		R_SBI_NilaiLL decimal(18,2) null default 0,
		R_SBI_FisikLL decimal(18,2) null default 0,		
		A_BI_HKKerja decimal(18,2) null default 0,
		A_BI_HKSosial decimal(18,2) null default 0,
		A_BI_JumlahUpah decimal(18,2) null default 0, 
		A_BI_HasilPanen decimal(18,2) null default 0, 
		A_BI_HasilPanenLump decimal(18,2) null default 0,
		A_BI_HasilPanenTBS decimal(18,2) null default 0,
		A_BI_Jelajah decimal(18,2) null default 0,
		A_BI_JumlahPohon decimal(18,2) null default 0,
		A_BI_JelajahMesin decimal(18,2) null default 0,
		A_BI_HasilPemelliharaan decimal(18,2) null default 0,
		A_BI_HasilLainLain decimal(18,2) null default 0,
		A_BI_FisikTUP decimal(18,2) null default 0,
		A_BI_NilaiTUP decimal(18,2) null default 0,
		A_BI_NilaiKas decimal(18,2) null default 0,
		A_BI_FisikKas decimal(18,2) null default 0,
		A_BI_NilaiEAP decimal(18,2) null default 0,
		A_BI_FisikEAP decimal(18,2) null default 0,
		A_BI_NilaiLL decimal(18,2) null default 0,
		A_BI_FisikLL decimal(18,2) null default 0,
		A_SBI_HKKerja decimal(18,2) null default 0,
		A_SBI_HKSosial decimal(18,2) null default 0,
		A_SBI_JumlahUpah decimal(18,2) null default 0, 
		A_SBI_HasilPanen decimal(18,2) null default 0, 
		A_SBI_HasilPanenLump decimal(18,2) null default 0,
		A_SBI_HasilPanenTBS decimal(18,2) null default 0,
		A_SBI_Jelajah decimal(18,2) null default 0,
		A_SBI_JumlahPohon decimal(18,2) null default 0,
		A_SBI_JelajahMesin decimal(18,2) null default 0,
		A_SBI_HasilPemelliharaan decimal(18,2) null default 0,
		A_SBI_HasilLainLain decimal(18,2) null default 0,
		A_SBI_FisikTUP decimal(18,2) null default 0,
		A_SBI_NilaiTUP decimal(18,2) null default 0,
		A_SBI_NilaiKas decimal(18,2) null default 0,
		A_SBI_FisikKas decimal(18,2) null default 0,
		A_SBI_NilaiEAP decimal(18,2) null default 0,
		A_SBI_FisikEAP decimal(18,2) null default 0,
		A_SBI_NilaiLL decimal(18,2) null default 0,
		A_SBI_FisikLL decimal(18,2) null default 0
	)
	
	
	-- Realisasi Gaji/Upah
	-- R - Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
		--KodeBudidaya,
		GroupMesin,KodeBarang,
		R_BI_HKKerja,R_BI_HKSosial,R_BI_JumlahUpah, R_BI_HasilPanen, 
		R_BI_HasilPanenLump,R_BI_HasilPanenTBS,R_BI_Jelajah,R_BI_JumlahPohon,
		R_BI_JelajahMesin, R_BI_HasilPemelliharaan,R_BI_HasilLainLain)
	SELECT 
		case 
			when @Layer='Kebun' then A.KodeUnit
			when @Layer='Afdeling' then A.KodeUnit+A.KodeAfdeling
			else A.KodeUnit+A.KodeAfdeling+A.KodeBlok
		end,
		A.TahunTanam,
		--A.KodeBudidaya,
		A.Grup,
		'',
		A.HKKerja,
		A.HKSosial,
		A.JumlahUpah, 
		A.HasilPanen, 
		A.HasilPanenLump,
		A.HasilPanenTBS,
		A.Jelajah,
		A.JumlahPohon,
		A.JelajahMesin,
		A.HasilPemelliharaan,
		A.HasilLainLain
	FROM ArealEFUpah AS A
	WHERE A.Bulan=@BulanAkhir and A.Tahun=@TahunAkhir and (A.KMK='01' or A.KMK is null or rtrim(A.KMK)='' or rtrim(A.KMK)='') 	
	
	-- R - sd Bulan Ini 
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
	--KodeBudidaya,
		GroupMesin,KodeBarang,
		R_SBI_HKKerja,R_SBI_HKSosial,R_SBI_JumlahUpah, R_SBI_HasilPanen, 
		R_SBI_HasilPanenLump,R_SBI_HasilPanenTBS,R_SBI_Jelajah,R_SBI_JumlahPohon,
		R_SBI_JelajahMesin, R_SBI_HasilPemelliharaan,R_SBI_HasilLainLain)
	SELECT 
		case 
			when @Layer='Kebun' then A.KodeUnit
			when @Layer='Afdeling' then A.KodeUnit+A.KodeAfdeling
			else A.KodeUnit+A.KodeAfdeling+A.KodeBlok
		end,
		A.TahunTanam,
		--A.KodeBudidaya,
		A.Grup,
		'',
		A.HKKerja,
		A.HKSosial,
		A.JumlahUpah, 
		A.HasilPanen, 
		A.HasilPanenLump,
		A.HasilPanenTBS,
		A.Jelajah,
		A.JumlahPohon,
		A.JelajahMesin,
		A.HasilPemelliharaan,
		A.HasilLainLain
	FROM ArealEFUpah AS A
	WHERE (A.KMK='01' or A.KMK is null or rtrim(A.KMK)='' or rtrim(A.KMK)='') 	

	-- A Bulan Ini
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
		--KodeBudidaya,
		GroupMesin,KodeBarang,
		A_BI_HKKerja,A_BI_HKSosial,A_BI_JumlahUpah, A_BI_HasilPanen, 
		A_BI_HasilPanenLump,A_BI_HasilPanenTBS,A_BI_Jelajah,A_BI_JumlahPohon,
		A_BI_JelajahMesin, A_BI_HasilPemelliharaan,A_BI_HasilLainLain)
	SELECT 
		case 
			when @Layer='Kebun' then A.KodeUnit
			when @Layer='Afdeling' then A.KodeUnit+A.KodeAfdeling
			else A.KodeUnit+A.KodeAfdeling+A.KodeBlok
		end,
		A.TahunTanam,
		--A.KodeBudidaya,
		A.Grup,
		'',
		A.HKKerja,
		A.HKSosial,
		A.JumlahUpah, 
		A.HasilPanen, 
		A.HasilPanenLump,
		A.HasilPanenTBS,
		A.Jelajah,
		A.JumlahPohon,
		A.JelajahMesin,
		A.HasilPemelliharaan,
		A.HasilLainLain
	FROM ArealEFUpah	AS A
	WHERE A.Bulan=@BulanAkhir and A.Tahun=@TahunAkhir and A.KMK='02' 	
	
	-- A - sd Bulan Ini 
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
		--KodeBudidaya,
		GroupMesin,KodeBarang,
		A_SBI_HKKerja,A_SBI_HKSosial,A_SBI_JumlahUpah, A_SBI_HasilPanen, 
		A_SBI_HasilPanenLump,A_SBI_HasilPanenTBS,A_SBI_Jelajah,A_SBI_JumlahPohon,
		A_SBI_JelajahMesin, A_SBI_HasilPemelliharaan,A_SBI_HasilLainLain)
	SELECT 
		case 
			when @Layer='Kebun' then A.KodeUnit
			when @Layer='Afdeling' then A.KodeUnit+A.KodeAfdeling
			else A.KodeUnit+A.KodeAfdeling+A.KodeBlok
		end,
		A.TahunTanam,
		--A.KodeBudidaya,
		A.Grup,
		'',
		A.HKKerja,
		A.HKSosial,
		A.JumlahUpah, 
		A.HasilPanen, 
		A.HasilPanenLump,
		A.HasilPanenTBS,
		A.Jelajah,
		A.JumlahPohon,
		A.JelajahMesin,
		A.HasilPemelliharaan,
		A.HasilLainLain
	FROM ArealEFUpah	AS A
	WHERE A.KMK='02' 
	
	
	-- Realisasi EAP
	-- R - Bulain Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
		--KodeBudidaya,
		GroupMesin,KodeBarang,R_BI_NilaiEAP,R_BI_FisikEAP)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.kdafd
			else A.kodeunit+A.kdafd+A.kdblok
		end,
		'',
		--A.KodeBudidaya,
		'','',
		A.Nilai,A.jmlfisik
	FROM [SPDK_KBN_KONSOL].[dbo].[EAP] AS A	
	WHERE MONTH(tanggal)=@BulanAkhir and YEAR(tanggal)=@TahunAkhir 
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kdafd+A.kdblok,1,LEN(@Id))
		and A.kdbud=@Budidaya
		and A.KMK='01'

	-- R - sd Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
		GroupMesin,KodeBarang,R_SBI_NilaiEAP,R_SBI_FisikEAP)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.kdafd
			else A.kodeunit+A.kdafd+A.kdblok
		end,
		'',
		--A.KodeBudidaya,
		'','',
		A.nilai,A.jmlfisik
	FROM [SPDK_KBN_KONSOL].[dbo].[EAP] AS A	
	WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kdafd+A.kdblok,1,LEN(@Id))
		and A.kdbud=@Budidaya
		and A.KMK='02'

	-- Realisasi Kas Bank
	-- R - Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
		--KodeBudidaya,
		GroupMesin,KodeBarang,R_BI_NilaiKas, R_BI_FisikKas)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.kdafd
			else A.kodeunit+A.kdafd+A.kdblok
		end,
		A.thntnm,
		--A.kdbud,
		'','',
		A.nilai,A.jml_fisik
	FROM [SPDK_KBN_KONSOL].[dbo].[KasBank] AS A	
	WHERE MONTH(tanggal)=@BulanAkhir and YEAR(tanggal)=@TahunAkhir and (A.KMK='01' or A.KMK is null or rtrim(A.KMK)='') 	
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kdafd+A.kdblok,1,LEN(@Id))
		and A.kdbud=@Budidaya 
	
	-- R - sd Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
		--KodeBudidaya,
		GroupMesin,KodeBarang,R_SBI_NilaiKas, R_SBI_FisikKas)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.kdafd
			else A.kodeunit+A.kdafd+A.kdblok
		end,
		A.thntnm,
		--A.kdbud,
		'','',
		A.nilai,A.jml_fisik
	FROM [SPDK_KBN_KONSOL].[dbo].[KasBank] AS A	
	WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
		and (A.KMK='01' or A.KMK is null or rtrim(A.KMK)='') 	
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kdafd+A.kdblok,1,LEN(@Id))
		and A.kdbud=@Budidaya

	-- A - Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
		--KodeBudidaya,
		GroupMesin,KodeBarang,A_BI_NilaiKas, A_BI_FisikKas)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.kdafd
			else A.kodeunit+A.kdafd+A.kdblok
		end,
		A.thntnm,
		--A.kdbud,
		'','',
		A.nilai,A.jml_fisik
	FROM [SPDK_KBN_KONSOL].[dbo].[KasBank] AS A	
	WHERE MONTH(tanggal)=@BulanAkhir and YEAR(tanggal)=@TahunAkhir and A.KMK='02'	
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kdafd+A.kdblok,1,LEN(@Id))
		and A.kdbud=@Budidaya

	
	-- A - sd Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
	--KodeBudidaya,
	GroupMesin,KodeBarang,A_SBI_NilaiKas, A_SBI_FisikKas)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.kdafd
			else A.kodeunit+A.kdafd+A.kdblok
		end,
		A.thntnm,
		--A.kdbud,
		'','',
		A.nilai,A.jml_fisik
	FROM [SPDK_KBN_KONSOL].[dbo].[KasBank] AS A	
	WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
		and A.KMK='02'	
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kdafd+A.kdblok,1,LEN(@Id))
		and A.kdbud=@Budidaya

	-- Realisasi TUP
	-- R - Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
	--KodeBudidaya,
	GroupMesin,KodeBarang,R_BI_NilaiTUP, R_BI_FisikTUP)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.KodeAfdeling
			else A.kodeunit+A.KodeAfdeling+A.kdblok
		end,
		A.thntnm,
		--A.KodeBudidaya,
		'',A.kodebarang,
		A.nilai,A.qty
	FROM [SPDK_KBN_KONSOL].[dbo].[TUP] AS A	
	WHERE MONTH(tanggal)=@BulanAkhir and YEAR(tanggal)=@TahunAkhir and (A.KMK='01' or A.KMK is null or rtrim(A.KMK)='') 	
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kodeafdeling+A.kdblok,1,LEN(@Id))
		and A.kodebudidaya=@Budidaya
		
		
	-- R - sd Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
	--KodeBudidaya,
	GroupMesin,KodeBarang,R_SBI_NilaiTUP, R_SBI_FisikTUP)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.KodeAfdeling
			else A.kodeunit+A.KodeAfdeling+A.kdblok
		end,
		A.thntnm,
		--A.KodeBudidaya,
		'',A.kodebarang,
		A.nilai,A.qty
	FROM [SPDK_KBN_KONSOL].[dbo].[TUP] AS A	
	WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
		and (A.KMK='01' or A.KMK is null or rtrim(A.KMK)='') 	
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kodeafdeling+A.kdblok,1,LEN(@Id))
		and A.kodebudidaya=@Budidaya
		

	-- A - Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
	--KodeBudidaya,
	GroupMesin,KodeBarang,A_BI_NilaiTUP, A_BI_FisikTUP)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.KodeAfdeling
			else A.kodeunit+A.KodeAfdeling+A.kdblok
		end,
		A.thntnm,
		--A.KodeBudidaya,
		'',A.kodebarang,
		A.nilai,A.qty
	FROM [SPDK_KBN_KONSOL].[dbo].[TUP] AS A	
	WHERE MONTH(tanggal)=@BulanAkhir and YEAR(tanggal)=@TahunAkhir and A.KMK='02'
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kodeafdeling+A.kdblok,1,LEN(@Id))
		and A.kodebudidaya=@Budidaya
			
	
	-- A - sd Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
	--KodeBudidaya,
	GroupMesin,KodeBarang,A_SBI_NilaiTUP, A_SBI_FisikTUP)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.KodeAfdeling
			else A.kodeunit+A.KodeAfdeling+A.kdblok
		end,
		A.thntnm,
		--A.KodeBudidaya,
		'',A.kodebarang,
		A.nilai,A.qty
	FROM [SPDK_KBN_KONSOL].[dbo].[TUP] AS A	
	WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
		and A.KMK='02'	
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kodeafdeling+A.kdblok,1,LEN(@Id))
		and A.kodebudidaya=@Budidaya


	-- Realisasi Memorial
	-- R - Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
	--KodeBudidaya,
	GroupMesin,KodeBarang,R_BI_NilaiLL, R_BI_FisikLL)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.KodeAfd
			else A.kodeunit+A.KodeAfd+A.kdblok
		end,
		A.thntnm,
		--A.KodeBud,
		'','',
		case 
			when A.DK='D' then A.nilai 
			else -1 * A.nilai 
		end,
		A.jmlfisik
	FROM [SPDK_KBN_KONSOL].[dbo].[AkunMemorial] AS A	
	WHERE MONTH(tanggal)=@BulanAkhir and YEAR(tanggal)=@TahunAkhir and (A.KMK='01' or A.KMK is null or rtrim(A.KMK)='') 	
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kodeafd+A.kdblok,1,LEN(@Id))
		and A.kodebud=@Budidaya
		and substring(A.NoInput,1,3)='JLL'
	

	---- R - sd Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
	--KodeBudidaya,
	GroupMesin,KodeBarang,R_SBI_NilaiLL, R_SBI_FisikLL)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.KodeAfd
			else A.kodeunit+A.KodeAfd+A.kdblok
		end,
		A.thntnm,
		--A.KodeBud,
		'','',
		case 
			when A.DK='D' then A.nilai 
			else -1 * A.nilai 
		end,
		A.jmlfisik
	FROM [SPDK_KBN_KONSOL].[dbo].[AkunMemorial] AS A	
	WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
		and (A.KMK='01' or A.KMK is null or rtrim(A.KMK)='') 	
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kodeafd+A.kdblok,1,LEN(@Id))
		and A.kodebud=@Budidaya
		and substring(A.NoInput,1,3)='JLL'

	---- A - Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
	--KodeBudidaya,
	GroupMesin,KodeBarang,A_BI_NilaiLL, A_BI_FisikLL)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.KodeAfd
			else A.kodeunit+A.KodeAfd+A.kdblok
		end,
		A.thntnm,
		--A.KodeBud,
		'','',
		case 
			when A.DK='D' then A.nilai 
			else -1 * A.nilai 
		end,
		A.jmlfisik
	FROM [SPDK_KBN_KONSOL].[dbo].[AkunMemorial] AS A	
	WHERE MONTH(tanggal)=@BulanAkhir and YEAR(tanggal)=@TahunAkhir and A.KMK='02'
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kodeafd+A.kdblok,1,LEN(@Id))
		and A.kodebud=@Budidaya
		and substring(A.NoInput,1,3)='JLL'
	
	---- A - sd Bulan Ini / Bulan Akhir
	INSERT INTO ArealEFGabungRealisasi
	(KodeLokasi,TahunTanam,
	--KodeBudidaya,
	GroupMesin,KodeBarang,A_SBI_NilaiLL, A_SBI_FisikLL)
	select 
		case 
			when @Layer='Kebun' then A.kodeunit
			when @Layer='Afdeling' then A.kodeunit+A.KodeAfd
			else A.kodeunit+A.KodeAfd+A.kdblok
		end,
		A.thntnm,
		--A.KodeBud,
		'','',
		case 
			when A.DK='D' then A.nilai 
			else -1 * A.nilai 
		end,
		A.jmlfisik
	FROM [SPDK_KBN_KONSOL].[dbo].[AkunMemorial] AS A	
		WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
		and A.KMK='02'	
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kodeafd+A.kdblok,1,LEN(@Id))
		and A.kodebud=@Budidaya
		and substring(A.NoInput,1,3)='JLL'


	IF OBJECT_ID(N'dbo.ArealEFRealisasi', N'U') IS NOT NULL
	BEGIN
		drop table dbo.ArealEFRealisasi
	END
	SELECT  
		KodeLokasi, 
		--TahunTanam,
		--KodeBudidaya,
		--GroupMesin,
		--KodeBarang,
		R_BI_HKKerja=sum(R_BI_HKKerja),
		R_BI_HKSosial=sum(R_BI_HKSosial),
		R_BI_JumlahUpah=sum(R_BI_JumlahUpah), 
		R_BI_HasilPanen=sum(R_BI_HasilPanen), 
		R_BI_HasilPanenLump=sum(R_BI_HasilPanenLump),
		R_BI_HasilPanenTBS=sum(R_BI_HasilPanenTBS),
		R_BI_Jelajah=sum(R_BI_Jelajah),
		R_BI_JumlahPohon=sum(R_BI_JumlahPohon),
		R_BI_JelajahMesin=sum(R_BI_JelajahMesin),
		R_BI_HasilPemelliharaan=sum(R_BI_HasilPemelliharaan),
		R_BI_HasilLainLain=sum(R_BI_HasilLainLain),
		R_BI_FisikTUP=sum(R_BI_FisikTUP),
		R_BI_NilaiTUP=sum(R_BI_NilaiTUP),
		R_BI_NilaiKas=sum(R_BI_NilaiKas),
		R_BI_FisikKas=sum(R_BI_FisikKas),
		R_BI_NilaiEAP=sum(R_BI_NilaiEAP),
		R_BI_FisikEAP=sum(R_BI_FisikEAP),
		R_BI_NilaiLL=sum(R_BI_NilaiLL),
		R_BI_FisikLL=sum(R_BI_FisikLL),
		R_SBI_HKKerja=sum(R_SBI_HKKerja),
		R_SBI_HKSosial=sum(R_SBI_HKSosial),
		R_SBI_JumlahUpah=sum(R_SBI_JumlahUpah), 
		R_SBI_HasilPanen=sum(R_SBI_HasilPanen), 
		R_SBI_HasilPanenLump=sum(R_SBI_HasilPanenLump),
		R_SBI_HasilPanenTBS=sum(R_SBI_HasilPanenTBS),
		R_SBI_Jelajah=sum(R_SBI_Jelajah),
		R_SBI_JumlahPohon=sum(R_SBI_JumlahPohon),
		R_SBI_JelajahMesin=sum(R_SBI_JelajahMesin),
		R_SBI_HasilPemelliharaan=sum(R_SBI_HasilPemelliharaan),
		R_SBI_HasilLainLain=sum(R_SBI_HasilLainLain),
		R_SBI_FisikTUP=sum(R_SBI_FisikTUP),
		R_SBI_NilaiTUP=sum(R_SBI_NilaiTUP),
		R_SBI_NilaiKas=sum(R_SBI_NilaiKas),
		R_SBI_FisikKas=sum(R_SBI_FisikKas),
		R_SBI_NilaiEAP=sum(R_SBI_NilaiEAP),
		R_SBI_FisikEAP=sum(R_SBI_FisikEAP),
		R_SBI_NilaiLL=sum(R_SBI_NilaiLL),
		R_SBI_FisikLL=sum(R_SBI_FisikLL),		
		A_BI_HKKerja=sum(A_BI_HKKerja),
		A_BI_HKSosial=sum(A_BI_HKSosial),
		A_BI_JumlahUpah=sum(A_BI_JumlahUpah), 
		A_BI_HasilPanen=sum(A_BI_HasilPanen), 
		A_BI_HasilPanenLump=sum(A_BI_HasilPanenLump),
		A_BI_HasilPanenTBS=sum(A_BI_HasilPanenTBS),
		A_BI_Jelajah=sum(A_BI_Jelajah),
		A_BI_JumlahPohon=sum(A_BI_JumlahPohon),
		A_BI_JelajahMesin=sum(A_BI_JelajahMesin),
		A_BI_HasilPemelliharaan=sum(A_BI_HasilPemelliharaan),
		A_BI_HasilLainLain=sum(A_BI_HasilLainLain),
		A_BI_FisikTUP=sum(A_BI_FisikTUP),
		A_BI_NilaiTUP=sum(A_BI_NilaiTUP),
		A_BI_NilaiKas=sum(A_BI_NilaiKas),
		A_BI_FisikKas=sum(A_BI_FisikKas),
		A_BI_NilaiEAP=sum(A_BI_NilaiEAP),
		A_BI_FisikEAP=sum(A_BI_FisikEAP),
		A_BI_NilaiLL=sum(A_BI_NilaiLL),
		A_BI_FisikLL=sum(A_BI_FisikLL),
		A_SBI_HKKerja=sum(A_SBI_HKKerja),
		A_SBI_HKSosial=sum(A_SBI_HKSosial),
		A_SBI_JumlahUpah=sum(A_SBI_JumlahUpah), 
		A_SBI_HasilPanen=sum(A_SBI_HasilPanen), 
		A_SBI_HasilPanenLump=sum(A_SBI_HasilPanenLump),
		A_SBI_HasilPanenTBS=sum(A_SBI_HasilPanenTBS),
		A_SBI_Jelajah=sum(A_SBI_Jelajah),
		A_SBI_JumlahPohon=sum(A_SBI_JumlahPohon),
		A_SBI_JelajahMesin=sum(A_SBI_JelajahMesin),
		A_SBI_HasilPemelliharaan=sum(A_SBI_HasilPemelliharaan),
		A_SBI_HasilLainLain=sum(A_SBI_HasilLainLain),
		A_SBI_FisikTUP=sum(A_SBI_FisikTUP),
		A_SBI_NilaiTUP=sum(A_SBI_NilaiTUP),
		A_SBI_NilaiKas=sum(A_SBI_NilaiKas),
		A_SBI_FisikKas=sum(A_SBI_FisikKas),
		A_SBI_NilaiEAP=sum(A_SBI_NilaiEAP),
		A_SBI_FisikEAP=sum(A_SBI_FisikEAP),
		A_SBI_NilaiLL=sum(A_SBI_NilaiLL),
		A_SBI_FisikLL=sum(A_SBI_FisikLL)
	INTO ArealEFRealisasi from ArealEFGabungRealisasi as A
	WHERE len(rtrim(A.KodeLokasi)) = 
		case 
			when @Layer='Kebun' then 2 
			when @layer='Afdeling' then 4
			else 8
		end
		--and A.KodeBudidaya=@Budidaya
	GROUP BY KodeLokasi

	IF OBJECT_ID(N'dbo.ArealEFGabungPembanding', N'U') IS NOT NULL
	BEGIN
		drop table dbo.ArealEFGabungPembanding
	END

	CREATE TABLE dbo.ArealEFGabungPembanding
	(
		KodeLokasi nvarchar(100), 
		TahunTanam nvarchar(100),
		--KodeBudidaya nvarchar(100),
		GroupMesin nvarchar(100),
		KodeBarang nvarchar(100),
		R_BI_HKKerja decimal(18,2) null default 0,
		R_BI_HKSosial decimal(18,2) null default 0,
		R_BI_JumlahUpah decimal(18,2) null default 0, 
		R_BI_HasilPanen decimal(18,2) null default 0, 
		R_BI_HasilPanenLump decimal(18,2) null default 0,
		R_BI_HasilPanenTBS decimal(18,2) null default 0,
		R_BI_Jelajah decimal(18,2) null default 0,
		R_BI_JumlahPohon decimal(18,2) null default 0,
		R_BI_JelajahMesin decimal(18,2) null default 0,
		R_BI_HasilPemelliharaan decimal(18,2) null default 0,
		R_BI_HasilLainLain decimal(18,2) null default 0,
		R_BI_FisikTUP decimal(18,2) null default 0,
		R_BI_NilaiTUP decimal(18,2) null default 0,
		R_BI_NilaiKas decimal(18,2) null default 0,
		R_BI_FisikKas decimal(18,2) null default 0,
		R_BI_NilaiEAP decimal(18,2) null default 0,
		R_BI_FisikEAP decimal(18,2) null default 0,
		R_BI_NilaiLL decimal(18,2) null default 0,
		R_BI_FisikLL decimal(18,2) null default 0,
		R_SBI_HKKerja decimal(18,2) null default 0,
		R_SBI_HKSosial decimal(18,2) null default 0,
		R_SBI_JumlahUpah decimal(18,2) null default 0, 
		R_SBI_HasilPanen decimal(18,2) null default 0, 
		R_SBI_HasilPanenLump decimal(18,2) null default 0,
		R_SBI_HasilPanenTBS decimal(18,2) null default 0,
		R_SBI_Jelajah decimal(18,2) null default 0,
		R_SBI_JumlahPohon decimal(18,2) null default 0,
		R_SBI_JelajahMesin decimal(18,2) null default 0,
		R_SBI_HasilPemelliharaan decimal(18,2) null default 0,
		R_SBI_HasilLainLain decimal(18,2) null default 0,
		R_SBI_FisikTUP decimal(18,2) null default 0,
		R_SBI_NilaiTUP decimal(18,2) null default 0,
		R_SBI_NilaiKas decimal(18,2) null default 0,
		R_SBI_FisikKas decimal(18,2) null default 0,
		R_SBI_NilaiEAP decimal(18,2) null default 0,
		R_SBI_FisikEAP decimal(18,2) null default 0,
		R_SBI_NilaiLL decimal(18,2) null default 0,
		R_SBI_FisikLL decimal(18,2) null default 0,		
		A_BI_HKKerja decimal(18,2) null default 0,
		A_BI_HKSosial decimal(18,2) null default 0,
		A_BI_JumlahUpah decimal(18,2) null default 0, 
		A_BI_HasilPanen decimal(18,2) null default 0, 
		A_BI_HasilPanenLump decimal(18,2) null default 0,
		A_BI_HasilPanenTBS decimal(18,2) null default 0,
		A_BI_Jelajah decimal(18,2) null default 0,
		A_BI_JumlahPohon decimal(18,2) null default 0,
		A_BI_JelajahMesin decimal(18,2) null default 0,
		A_BI_HasilPemelliharaan decimal(18,2) null default 0,
		A_BI_HasilLainLain decimal(18,2) null default 0,
		A_BI_FisikTUP decimal(18,2) null default 0,
		A_BI_NilaiTUP decimal(18,2) null default 0,
		A_BI_NilaiKas decimal(18,2) null default 0,
		A_BI_FisikKas decimal(18,2) null default 0,
		A_BI_NilaiEAP decimal(18,2) null default 0,
		A_BI_FisikEAP decimal(18,2) null default 0,
		A_BI_NilaiLL decimal(18,2) null default 0,
		A_BI_FisikLL decimal(18,2) null default 0,
		A_SBI_HKKerja decimal(18,2) null default 0,
		A_SBI_HKSosial decimal(18,2) null default 0,
		A_SBI_JumlahUpah decimal(18,2) null default 0, 
		A_SBI_HasilPanen decimal(18,2) null default 0, 
		A_SBI_HasilPanenLump decimal(18,2) null default 0,
		A_SBI_HasilPanenTBS decimal(18,2) null default 0,
		A_SBI_Jelajah decimal(18,2) null default 0,
		A_SBI_JumlahPohon decimal(18,2) null default 0,
		A_SBI_JelajahMesin decimal(18,2) null default 0,
		A_SBI_HasilPemelliharaan decimal(18,2) null default 0,
		A_SBI_HasilLainLain decimal(18,2) null default 0,
		A_SBI_FisikTUP decimal(18,2) null default 0,
		A_SBI_NilaiTUP decimal(18,2) null default 0,
		A_SBI_NilaiKas decimal(18,2) null default 0,
		A_SBI_FisikKas decimal(18,2) null default 0,
		A_SBI_NilaiEAP decimal(18,2) null default 0,
		A_SBI_FisikEAP decimal(18,2) null default 0,
		A_SBI_NilaiLL decimal(18,2) null default 0,
		A_SBI_FisikLL decimal(18,2) null default 0
	)

	-- Pembanding
	-- R Bulan Ini
	INSERT INTO ArealEFGabungPembanding
	(KodeLokasi, TahunTanam, 
		--KodeBudidaya,
		GroupMesin,KodeBarang,
		R_BI_HKKerja, 
		R_BI_JumlahUpah, 
		R_BI_HasilPanen, 
		R_BI_Jelajah, 
		R_BI_HasilPemelliharaan, 
		R_BI_HasilLainLain 
	)
	SELECT 
		case 
			when @Layer='Kebun' then A.KodeUnit
			when @Layer='Afdeling' then A.KodeUnit+A.KodeAfd
			else A.KodeUnit+A.KodeAfd+A.KodeBlok
		end,
		A.thntnm,
		--A.kodebud,
		'',
		kodebarang,
		case
			when @DibandingkanDengan='RKAP' then A.jmlHK_RKAP
			when @DibandingkanDengan='PKB' then A.jmlHK_PMK
			when @DibandingkanDengan='INDUK' then 0
			when @DibandingkanDengan='BALAI' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' then A.nilai_RKAP
			when @DibandingkanDengan='PKB' then A.nilai_PMK
			when @DibandingkanDengan='INDUK' then 0
			when @DibandingkanDengan='BALAI' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' and SUBSTRING(A.norek,1,3)='602' then A.jmlhsl_RKAP
			when @DibandingkanDengan='PKB' and SUBSTRING(A.norek,1,3)='602' then A.jmlhsl_PMK
			when @DibandingkanDengan='INDUK' and SUBSTRING(A.norek,1,3)='602' then 0
			when @DibandingkanDengan='BALAI' and SUBSTRING(A.norek,1,3)='602' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' then A.HaJelajah_RKAP
			when @DibandingkanDengan='PKB' then A.HaJelajah_PMK
			when @DibandingkanDengan='INDUK' then 0
			when @DibandingkanDengan='BALAI' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' and SUBSTRING(A.norek,1,3)='601' then A.jmlhsl_RKAP
			when @DibandingkanDengan='PKB' and SUBSTRING(A.norek,1,3)='601' then A.jmlhsl_PMK
			when @DibandingkanDengan='INDUK' and SUBSTRING(A.norek,1,3)='601' then 0
			when @DibandingkanDengan='BALAI' and SUBSTRING(A.norek,1,3)='601' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then A.jmlhsl_RKAP
			when @DibandingkanDengan='PKB' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then A.jmlhsl_PMK
			when @DibandingkanDengan='INDUK' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then 0
			when @DibandingkanDengan='BALAI' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then 0
		end
		
	FROM SPDK_KBN_KONSOL.dbo.AkunRKAP AS A
		WHERE (A.KMK='01' or A.KMK is null or A.KMK='')	
		and MONTH(tanggal)=@BulanAkhir and YEAR(tanggal)=@TahunAkhir
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kodeafd+A.kodeblok,1,LEN(@Id))
		and A.kodebud=@Budidaya
	
	-- Pembanding
	-- R sd Bulan Ini
	INSERT INTO ArealEFGabungPembanding
	(KodeLokasi, TahunTanam, 
	--KodeBudidaya,
	GroupMesin,KodeBarang,
		R_SBI_HKKerja, 
		R_SBI_JumlahUpah, 
		R_SBI_HasilPanen, 
		R_SBI_Jelajah, 
		R_SBI_HasilPemelliharaan, 
		R_SBI_HasilLainLain 
	)
	SELECT 
		case 
			when @Layer='Kebun' then A.KodeUnit
			when @Layer='Afdeling' then A.KodeUnit+A.KodeAfd
			else A.KodeUnit+A.KodeAfd+A.KodeBlok
		end,
		A.thntnm,
		--A.kodebud,
		'',
		kodebarang,
		case
			when @DibandingkanDengan='RKAP' then A.jmlHK_RKAP
			when @DibandingkanDengan='PKB' then A.jmlHK_PMK
			when @DibandingkanDengan='INDUK' then 0
			when @DibandingkanDengan='BALAI' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' then A.nilai_RKAP
			when @DibandingkanDengan='PKB' then A.nilai_PMK
			when @DibandingkanDengan='INDUK' then 0
			when @DibandingkanDengan='BALAI' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' and SUBSTRING(A.norek,1,3)='602' then A.jmlhsl_RKAP
			when @DibandingkanDengan='PKB' and SUBSTRING(A.norek,1,3)='602' then A.jmlhsl_PMK
			when @DibandingkanDengan='INDUK' and SUBSTRING(A.norek,1,3)='602' then 0
			when @DibandingkanDengan='BALAI' and SUBSTRING(A.norek,1,3)='602' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' then A.HaJelajah_RKAP
			when @DibandingkanDengan='PKB' then A.HaJelajah_PMK
			when @DibandingkanDengan='INDUK' then 0
			when @DibandingkanDengan='BALAI' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' and SUBSTRING(A.norek,1,3)='601' then A.jmlhsl_RKAP
			when @DibandingkanDengan='PKB' and SUBSTRING(A.norek,1,3)='601' then A.jmlhsl_PMK
			when @DibandingkanDengan='INDUK' and SUBSTRING(A.norek,1,3)='601' then 0
			when @DibandingkanDengan='BALAI' and SUBSTRING(A.norek,1,3)='601' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then A.jmlhsl_RKAP
			when @DibandingkanDengan='PKB' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then A.jmlhsl_PMK
			when @DibandingkanDengan='INDUK' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then 0
			when @DibandingkanDengan='BALAI' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then 0
		end
		
	FROM SPDK_KBN_KONSOL.dbo.AkunRKAP AS A
		WHERE (A.KMK='01' or A.KMK is null or A.KMK='')	
		and tanggal>=@tglAwal and tanggal<=@tglAkhir
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kodeafd+A.kodeblok,1,LEN(@Id))
		and A.kodebud=@Budidaya

	-- A Bulan Ini
	INSERT INTO ArealEFGabungPembanding
	(KodeLokasi, TahunTanam, 
	--KodeBudidaya,
	GroupMesin,KodeBarang,
		A_BI_HKKerja, 
		A_BI_JumlahUpah, 
		A_BI_HasilPanen, 
		A_BI_Jelajah, 
		A_BI_HasilPemelliharaan, 
		A_BI_HasilLainLain 
	)
	SELECT 
		case 
			when @Layer='Kebun' then A.KodeUnit
			when @Layer='Afdeling' then A.KodeUnit+A.KodeAfd
			else A.KodeUnit+A.KodeAfd+A.KodeBlok
		end,
		A.thntnm,
		--A.kodebud,
		'',
		kodebarang,
		case
			when @DibandingkanDengan='RKAP' then A.jmlHK_RKAP
			when @DibandingkanDengan='PKB' then A.jmlHK_PMK
			when @DibandingkanDengan='INDUK' then 0
			when @DibandingkanDengan='BALAI' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' then A.nilai_RKAP
			when @DibandingkanDengan='PKB' then A.nilai_PMK
			when @DibandingkanDengan='INDUK' then 0
			when @DibandingkanDengan='BALAI' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' and SUBSTRING(A.norek,1,3)='602' then A.jmlhsl_RKAP
			when @DibandingkanDengan='PKB' and SUBSTRING(A.norek,1,3)='602' then A.jmlhsl_PMK
			when @DibandingkanDengan='INDUK' and SUBSTRING(A.norek,1,3)='602' then 0
			when @DibandingkanDengan='BALAI' and SUBSTRING(A.norek,1,3)='602' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' then A.HaJelajah_RKAP
			when @DibandingkanDengan='PKB' then A.HaJelajah_PMK
			when @DibandingkanDengan='INDUK' then 0
			when @DibandingkanDengan='BALAI' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' and SUBSTRING(A.norek,1,3)='601' then A.jmlhsl_RKAP
			when @DibandingkanDengan='PKB' and SUBSTRING(A.norek,1,3)='601' then A.jmlhsl_PMK
			when @DibandingkanDengan='INDUK' and SUBSTRING(A.norek,1,3)='601' then 0
			when @DibandingkanDengan='BALAI' and SUBSTRING(A.norek,1,3)='601' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then A.jmlhsl_RKAP
			when @DibandingkanDengan='PKB' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then A.jmlhsl_PMK
			when @DibandingkanDengan='INDUK' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then 0
			when @DibandingkanDengan='BALAI' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then 0
		end
		
	FROM SPDK_KBN_KONSOL.dbo.AkunRKAP AS A
		WHERE A.KMK='02' 
		and MONTH(tanggal)=@BulanAkhir and YEAR(tanggal)=@TahunAkhir
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kodeafd+A.kodeblok,1,LEN(@Id))
		and A.kodebud=@Budidaya
	

	-- Pembanding
	-- A sd Bulan Ini
	INSERT INTO ArealEFGabungPembanding
	(KodeLokasi, TahunTanam, 
	--KodeBudidaya,
	GroupMesin,KodeBarang,
		A_SBI_HKKerja, 
		A_SBI_JumlahUpah, 
		A_SBI_HasilPanen, 
		A_SBI_Jelajah, 
		A_SBI_HasilPemelliharaan, 
		A_SBI_HasilLainLain 
	)
	SELECT 
		case 
			when @Layer='Kebun' then A.KodeUnit
			when @Layer='Afdeling' then A.KodeUnit+A.KodeAfd
			else A.KodeUnit+A.KodeAfd+A.KodeBlok
		end,
		A.thntnm,
		--A.kodebud,
		'',
		kodebarang,
		case
			when @DibandingkanDengan='RKAP' then A.jmlHK_RKAP
			when @DibandingkanDengan='PKB' then A.jmlHK_PMK
			when @DibandingkanDengan='INDUK' then 0
			when @DibandingkanDengan='BALAI' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' then A.nilai_RKAP
			when @DibandingkanDengan='PKB' then A.nilai_PMK
			when @DibandingkanDengan='INDUK' then 0
			when @DibandingkanDengan='BALAI' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' and SUBSTRING(A.norek,1,3)='602' then A.jmlhsl_RKAP
			when @DibandingkanDengan='PKB' and SUBSTRING(A.norek,1,3)='602' then A.jmlhsl_PMK
			when @DibandingkanDengan='INDUK' and SUBSTRING(A.norek,1,3)='602' then 0
			when @DibandingkanDengan='BALAI' and SUBSTRING(A.norek,1,3)='602' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' then A.HaJelajah_RKAP
			when @DibandingkanDengan='PKB' then A.HaJelajah_PMK
			when @DibandingkanDengan='INDUK' then 0
			when @DibandingkanDengan='BALAI' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' and SUBSTRING(A.norek,1,3)='601' then A.jmlhsl_RKAP
			when @DibandingkanDengan='PKB' and SUBSTRING(A.norek,1,3)='601' then A.jmlhsl_PMK
			when @DibandingkanDengan='INDUK' and SUBSTRING(A.norek,1,3)='601' then 0
			when @DibandingkanDengan='BALAI' and SUBSTRING(A.norek,1,3)='601' then 0
		end,
		case
			when @DibandingkanDengan='RKAP' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then A.jmlhsl_RKAP
			when @DibandingkanDengan='PKB' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then A.jmlhsl_PMK
			when @DibandingkanDengan='INDUK' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then 0
			when @DibandingkanDengan='BALAI' and SUBSTRING(A.norek,1,3)!='601' and SUBSTRING(A.norek,1,3)!='602' then 0
		end
		
	FROM SPDK_KBN_KONSOL.dbo.AkunRKAP AS A
		WHERE A.KMK='02' 	
		and tanggal>=@tglAwal and tanggal<=@tglAkhir
		and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
		and @Id=substring(A.kodeunit+A.kodeafd+A.kodeblok,1,LEN(@Id))
		and A.kodebud=@Budidaya

	-- gabungkan pembangding
	IF OBJECT_ID(N'dbo.ArealEFRekapPembanding', N'U') IS NOT NULL
	BEGIN
		drop table dbo.ArealEFRekapPembanding
	END
	
	
	SELECT 
		KodeLokasi, 
		--TahunTanam,
		--KodeBudidaya,
		--GroupMesin,
		--KodeBarang,
		R_BI_HKKerja=sum(R_BI_HKKerja),
		R_BI_HKSosial=sum(R_BI_HKSosial),
		R_BI_JumlahUpah=sum(R_BI_JumlahUpah), 
		R_BI_HasilPanen=sum(R_BI_HasilPanen), 
		R_BI_HasilPanenLump=sum(R_BI_HasilPanenLump),
		R_BI_HasilPanenTBS=sum(R_BI_HasilPanenTBS),
		R_BI_Jelajah=sum(R_BI_Jelajah),
		R_BI_JumlahPohon=sum(R_BI_JumlahPohon),
		R_BI_JelajahMesin=sum(R_BI_JelajahMesin),
		R_BI_HasilPemelliharaan=sum(R_BI_HasilPemelliharaan),
		R_BI_HasilLainLain=sum(R_BI_HasilLainLain),
		R_BI_FisikTUP=sum(R_BI_FisikTUP),
		R_BI_NilaiTUP=sum(R_BI_NilaiTUP),
		R_BI_NilaiKas=sum(R_BI_NilaiKas),
		R_BI_FisikKas=sum(R_BI_FisikKas),
		R_BI_NilaiEAP=sum(R_BI_NilaiEAP),
		R_BI_FisikEAP=sum(R_BI_FisikEAP),
		R_BI_NilaiLL=sum(R_BI_NilaiLL),
		R_BI_FisikLL=sum(R_BI_FisikLL),
		R_SBI_HKKerja=sum(R_SBI_HKKerja),
		R_SBI_HKSosial=sum(R_SBI_HKSosial),
		R_SBI_JumlahUpah=sum(R_SBI_JumlahUpah), 
		R_SBI_HasilPanen=sum(R_SBI_HasilPanen), 
		R_SBI_HasilPanenLump=sum(R_SBI_HasilPanenLump),
		R_SBI_HasilPanenTBS=sum(R_SBI_HasilPanenTBS),
		R_SBI_Jelajah=sum(R_SBI_Jelajah),
		R_SBI_JumlahPohon=sum(R_SBI_JumlahPohon),
		R_SBI_JelajahMesin=sum(R_SBI_JelajahMesin),
		R_SBI_HasilPemelliharaan=sum(R_SBI_HasilPemelliharaan),
		R_SBI_HasilLainLain=sum(R_SBI_HasilLainLain),
		R_SBI_FisikTUP=sum(R_SBI_FisikTUP),
		R_SBI_NilaiTUP=sum(R_SBI_NilaiTUP),
		R_SBI_NilaiKas=sum(R_SBI_NilaiKas),
		R_SBI_FisikKas=sum(R_SBI_FisikKas),
		R_SBI_NilaiEAP=sum(R_SBI_NilaiEAP),
		R_SBI_FisikEAP=sum(R_SBI_FisikEAP),
		R_SBI_NilaiLL=sum(R_SBI_NilaiLL),
		R_SBI_FisikLL=sum(R_SBI_FisikLL),		
		A_BI_HKKerja=sum(A_BI_HKKerja),
		A_BI_HKSosial=sum(A_BI_HKSosial),
		A_BI_JumlahUpah=sum(A_BI_JumlahUpah), 
		A_BI_HasilPanen=sum(A_BI_HasilPanen), 
		A_BI_HasilPanenLump=sum(A_BI_HasilPanenLump),
		A_BI_HasilPanenTBS=sum(A_BI_HasilPanenTBS),
		A_BI_Jelajah=sum(A_BI_Jelajah),
		A_BI_JumlahPohon=sum(A_BI_JumlahPohon),
		A_BI_JelajahMesin=sum(A_BI_JelajahMesin),
		A_BI_HasilPemelliharaan=sum(A_BI_HasilPemelliharaan),
		A_BI_HasilLainLain=sum(A_BI_HasilLainLain),
		A_BI_FisikTUP=sum(A_BI_FisikTUP),
		A_BI_NilaiTUP=sum(A_BI_NilaiTUP),
		A_BI_NilaiKas=sum(A_BI_NilaiKas),
		A_BI_FisikKas=sum(A_BI_FisikKas),
		A_BI_NilaiEAP=sum(A_BI_NilaiEAP),
		A_BI_FisikEAP=sum(A_BI_FisikEAP),
		A_BI_NilaiLL=sum(A_BI_NilaiLL),
		A_BI_FisikLL=sum(A_BI_FisikLL),
		A_SBI_HKKerja=sum(A_SBI_HKKerja),
		A_SBI_HKSosial=sum(A_SBI_HKSosial),
		A_SBI_JumlahUpah=sum(A_SBI_JumlahUpah), 
		A_SBI_HasilPanen=sum(A_SBI_HasilPanen), 
		A_SBI_HasilPanenLump=sum(A_SBI_HasilPanenLump),
		A_SBI_HasilPanenTBS=sum(A_SBI_HasilPanenTBS),
		A_SBI_Jelajah=sum(A_SBI_Jelajah),
		A_SBI_JumlahPohon=sum(A_SBI_JumlahPohon),
		A_SBI_JelajahMesin=sum(A_SBI_JelajahMesin),
		A_SBI_HasilPemelliharaan=sum(A_SBI_HasilPemelliharaan),
		A_SBI_HasilLainLain=sum(A_SBI_HasilLainLain),
		A_SBI_FisikTUP=sum(A_SBI_FisikTUP),
		A_SBI_NilaiTUP=sum(A_SBI_NilaiTUP),
		A_SBI_NilaiKas=sum(A_SBI_NilaiKas),
		A_SBI_FisikKas=sum(A_SBI_FisikKas),
		A_SBI_NilaiEAP=sum(A_SBI_NilaiEAP),
		A_SBI_FisikEAP=sum(A_SBI_FisikEAP),
		A_SBI_NilaiLL=sum(A_SBI_NilaiLL),
		A_SBI_FisikLL=sum(A_SBI_FisikLL)
	INTO ArealEFRekapPembanding FROM ArealEFGabungPembanding
	GROUP BY KodeLokasi 
	
	-- View
	IF OBJECT_ID(N'dbo.ArealEFViewResult', N'U') IS NOT NULL
	BEGIN
		drop table dbo.ArealEFViewResult
	END
	
	CREATE TABLE ArealEFViewResult
	(
		LokasiId uniqueidentifier null default newid(),
		KodeLokasi nvarchar(100) null default '',
		Peta nvarchar(max) null default '',
		Produksi decimal(18,2) null default 0.00,
		PembandingProduksi decimal(18,2) null default 0.00,
		LuasTM decimal(18,4) null default 0.0000,
		Nama nvarchar(max) null default '',
		R_Fisik decimal(18,2) null default 0.00,
		R_Biaya decimal(18,2) null default 0.00,
		A_Fisik decimal(18,2) null default 0.00,
		A_Biaya decimal(18,2) null default 0.00,
		R_PembandingFisik decimal(18,2) null default 0.00,
		R_PembandingBiaya decimal(18,2) null default 0.00,
		A_PembandingFisik decimal(18,2) null default 0.00,
		A_PembandingBiaya decimal(18,2) null default 0.00,
		Warna nvarchar(max) null default '',
		KeteranganWarna nvarchar(max) null default '',
		Jml_Fisik decimal(18,2) null default 0.00,
		Jml_Biaya decimal(18,2) null default 0.00,
		Jml_PembandingFisik decimal(18,2) null default 0.00,
		Jml_PembandingBiaya decimal(18,2) null default 0.00
	)		
	
	INSERT INTO ArealEFViewResult 
	SELECT 
		LokasiId =
			case 
				when @Layer='Kebun' then (select top 1 kebunid from [ReferensiEF].[dbo].[Kebun] where kd_kbn=A.KodeLokasi)
				when @Layer='Afdeling' then (select top 1 afdelingid from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.KodeLokasi)
				else (select top 1 blokid from [ReferensiEF].[dbo].[Blok] where kd_blok=A.KodeLokasi)
			end,
		KodeLokasi=A.KodeLokasi,
		Peta='',
		Produksi=
			case 
				when @Layer='Kebun' then 
					case when @Budidaya!='01' then (select SUM(bi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=A.KodeLokasi and [kodevar]='JADI' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir)
					else (select SUM(bi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=A.KodeLokasi and [kodevar]='BASAH' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir) end
				when @Layer='Afdeling' then (A.A_SBI_HasilPanen+A.R_SBI_HasilPanen+A.A_SBI_HasilPanenTBS+A.R_SBI_HasilPanenTBS+A.A_SBI_HasilPanenLump+A.R_SBI_HasilPanenLump)*
					case when @Budidaya='02' then (select SUM(bi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=substring(A.KodeLokasi,1,2) and [kodevar]='JADI' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir)/(select SUM(bi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=substring(A.KodeLokasi,1,2) and [kodevar]='BASAH' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir)
					else 1 end
				else (A.A_SBI_HasilPanen+A.R_SBI_HasilPanen+A.A_SBI_HasilPanenTBS+A.R_SBI_HasilPanenTBS+A.A_SBI_HasilPanenLump+A.R_SBI_HasilPanenLump)*
					case when @Budidaya='02' then (select SUM(bi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=substring(A.KodeLokasi,1,2) and [kodevar]='JADI' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir)/(select SUM(bi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=substring(A.KodeLokasi,1,2) and [kodevar]='BASAH' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir)
					else 1 end
			end,
		PembandingProduksi=0,
		LuasTM=
			case 
				when @Layer='Kebun' then (select SUM(luas) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun=A.KodeLokasi and [status]='TM')
				when @Layer='Afdeling' then (select cast(SUM(luas) as decimal(18,4)) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun+kodeafdeling=A.KodeLokasi and [status]='TM')
				else (select SUM(luas) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun+kodeafdeling+kodeblok=A.KodeLokasi and [status]='TM')
			end,
		Nama=
			case 
				when @Layer='Kebun' then (select top 1 NamaKebun from SPDK_KBN_KONSOL.dbo.Ref_Kebun where KodeKebun=A.KodeLokasi)
				when @Layer='Afdeling' then (select top 1 namaafdeling from SPDK_KBN_KONSOL.dbo.Ref_Afdeling where kodekebun+kodeafdeling=A.KodeLokasi)
				else (select top 1 namablok from SPDK_KBN_KONSOL.dbo.Ref_Areal where kodekebun+kodeafdeling+kodeblok=A.KodeLokasi)
			end,

		R_Fisik =
			case
				when @FieldYangDibandingkan='HK' then
					case 
						when @OrderBulan='BI' then R_BI_HKKerja
						else R_SBI_HKKerja
					end
				when @FieldYangDibandingkan='PRESTASI' then
					case 
						when @OrderBulan='BI' then R_BI_HasilLainLain+R_BI_HasilPanen+R_BI_HasilPanenLump+R_BI_HasilPanenTBS+R_BI_HasilPemelliharaan
						else R_SBI_HasilLainLain+R_SBI_HasilPanen+R_SBI_HasilPanenLump+R_SBI_HasilPanenTBS+R_SBI_HasilPemelliharaan
					end
				when @FieldYangDibandingkan='TUP' then
					case 
						when @OrderBulan='BI' then R_BI_FisikTUP
						else R_SBI_FisikTUP
					end
				when @FieldYangDibandingkan='KAS' then
					case 
						when @OrderBulan='BI' then R_BI_FisikKAS
						else R_SBI_FisikKAS
					end
				when @FieldYangDibandingkan='LL' then
					case 
						when @OrderBulan='BI' then R_BI_FisikEAP+R_BI_FisikLL
						else R_SBI_FisikEAP+R_SBI_FisikLL
					end
				when @FieldYangDibandingkan='SELURUH' then
					case 
						when @OrderBulan='BI' then 0
						else 0
					end
				when @FieldYangDibandingkan='PROTAS' then
					case 
						when @OrderBulan='BI' then 
							(R_BI_HasilPanen+R_BI_HasilPanenLump+R_BI_HasilPanenTBS+A_BI_HasilPanen+A_BI_HasilPanenLump+A_BI_HasilPanenTBS)
							/
							(case 
								when @Layer='Kebun' then (select SUM(luas) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun=A.KodeLokasi and [status]='TM')
								when @Layer='Afdeling' then (select cast(SUM(luas) as decimal(18,4)) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun+kodeafdeling=A.KodeLokasi and [status]='TM')
								else (select SUM(luas) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun+kodeafdeling+kodeblok=A.KodeLokasi and [status]='TM')
							end)
						else 
							(R_SBI_HasilPanen+R_SBI_HasilPanenLump+R_SBI_HasilPanenTBS+A_SBI_HasilPanen+A_SBI_HasilPanenLump+A_SBI_HasilPanenTBS)
							/
							(case 
								when @Layer='Kebun' then (select SUM(luas) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun=A.KodeLokasi and [status]='TM')
								when @Layer='Afdeling' then (select cast(SUM(luas) as decimal(18,4)) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun+kodeafdeling=A.KodeLokasi and [status]='TM')
								else (select SUM(luas) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun+kodeafdeling+kodeblok=A.KodeLokasi and [status]='TM')
							end)
					end
			end,
		R_Biaya =
			case
				when @FieldYangDibandingkan='HK' or @FieldYangDibandingkan='PRESTASI'   then
					case 
						when @OrderBulan='BI' then R_BI_JumlahUpah
						else R_SBI_JumlahUpah
					end
				when @FieldYangDibandingkan='TUP' then
					case 
						when @OrderBulan='BI' then R_BI_NilaiTUP
						else R_SBI_NilaiTUP
					end
				when @FieldYangDibandingkan='KAS' then
					case 
						when @OrderBulan='BI' then R_BI_NilaiKAS
						else R_SBI_NilaiKAS
					end
				when @FieldYangDibandingkan='LL' then
					case 
						when @OrderBulan='BI' then R_BI_NilaiEAP+R_BI_NilaiLL
						else R_SBI_NilaiEAP+R_SBI_NilaiLL
					end
				when @FieldYangDibandingkan='SELURUH' then
					case 
						when @OrderBulan='BI' then R_BI_JumlahUpah+R_BI_NilaiTUP+R_BI_NilaiKAS+R_BI_NilaiEAP+R_BI_NilaiLL
						else R_SBI_JumlahUpah+R_SBI_NilaiTUP+R_SBI_NilaiKAS+R_SBI_NilaiEAP+R_SBI_NilaiLL
					end
					
			end,
		A_Fisik =
			case
				when @FieldYangDibandingkan='HK' then
					case 
						when @OrderBulan='BI' then A_BI_HKKerja
						else A_SBI_HKKerja
					end
				when @FieldYangDibandingkan='PRESTASI' then
					case 
						when @OrderBulan='BI' then A_BI_HasilLainLain+A_BI_HasilPanen+A_BI_HasilPanenLump+A_BI_HasilPanenTBS+A_BI_HasilPemelliharaan
						else A_SBI_HasilLainLain+A_SBI_HasilPanen+A_SBI_HasilPanenLump+A_SBI_HasilPanenTBS+A_SBI_HasilPemelliharaan
					end
				when @FieldYangDibandingkan='TUP' then
					case 
						when @OrderBulan='BI' then A_BI_FisikTUP
						else A_SBI_FisikTUP
					end
				when @FieldYangDibandingkan='KAS' then
					case 
						when @OrderBulan='BI' then A_BI_FisikKAS
						else A_SBI_FisikKAS
					end
				when @FieldYangDibandingkan='LL' then
					case 
						when @OrderBulan='BI' then A_BI_FisikEAP+A_BI_FisikLL
						else A_SBI_FisikEAP+A_SBI_FisikLL
					end
				when @FieldYangDibandingkan='SELURUH' then
					case 
						when @OrderBulan='BI' then 0
						else 0
					end

			end,
		A_Biaya =
			case
				when @FieldYangDibandingkan='HK' or @FieldYangDibandingkan='PRESTASI' then
					case 
						when @OrderBulan='BI' then A_BI_JumlahUpah
						else A_SBI_JumlahUpah
					end
				when @FieldYangDibandingkan='TUP' then
					case 
						when @OrderBulan='BI' then A_BI_NilaiTUP
						else A_SBI_NilaiTUP
					end
				when @FieldYangDibandingkan='KAS' then
					case 
						when @OrderBulan='BI' then A_BI_NilaiKAS
						else A_SBI_NilaiKAS
					end
				when @FieldYangDibandingkan='LL' then
					case 
						when @OrderBulan='BI' then A_BI_NilaiEAP+A_BI_NilaiLL
						else A_SBI_NilaiEAP+A_SBI_NilaiLL
					end
				when @FieldYangDibandingkan='SELURUH' then
					case 
						when @OrderBulan='BI' then A_BI_JumlahUpah+A_BI_NilaiTUP+A_BI_NilaiKAS+A_BI_NilaiEAP+A_BI_NilaiLL
						else A_SBI_JumlahUpah+A_SBI_NilaiTUP+A_SBI_NilaiKAS+A_SBI_NilaiEAP+A_SBI_NilaiLL
					end
			end,
		R_PembandingFisik=0,
		R_PembandingBiaya=0,
		A_PembandingFisik=0,
		A_PembandingBiaya=0,
		Warna='',
		KeteranganWarna='',
		Jml_Fisik=
			case
				when @FieldYangDibandingkan='HK' then
					case 
						when @OrderBulan='BI' then A_BI_HKKerja+R_BI_HKKerja
						else A_SBI_HKKerja+R_SBI_HKKerja
					end
				when @FieldYangDibandingkan='PRESTASI' then
					case 
						when @OrderBulan='BI' then A_BI_HasilLainLain+A_BI_HasilPanen+A_BI_HasilPanenLump+A_BI_HasilPanenTBS+A_BI_HasilPemelliharaan+
							R_BI_HasilLainLain+R_BI_HasilPanen+R_BI_HasilPanenLump+R_BI_HasilPanenTBS+R_BI_HasilPemelliharaan
						else A_SBI_HasilLainLain+A_SBI_HasilPanen+A_SBI_HasilPanenLump+A_SBI_HasilPanenTBS+A_SBI_HasilPemelliharaan+
							R_SBI_HasilLainLain+R_SBI_HasilPanen+R_SBI_HasilPanenLump+R_SBI_HasilPanenTBS+R_SBI_HasilPemelliharaan
					end
				when @FieldYangDibandingkan='TUP' then
					case 
						when @OrderBulan='BI' then A_BI_FisikTUP+R_BI_FisikTUP
						else A_SBI_FisikTUP+R_SBI_FisikTUP
					end
				when @FieldYangDibandingkan='KAS' then
					case 
						when @OrderBulan='BI' then A_BI_FisikKAS+R_BI_FisikKAS
						else A_SBI_FisikKAS+R_SBI_FisikKAS
					end
				when @FieldYangDibandingkan='LL' then
					case 
						when @OrderBulan='BI' then A_BI_FisikEAP+A_BI_FisikLL+R_BI_FisikEAP+R_BI_FisikLL
						else A_SBI_FisikEAP+A_SBI_FisikLL+R_SBI_FisikEAP+R_SBI_FisikLL
					end
				when @FieldYangDibandingkan='SELURUH' then
					case 
						when @OrderBulan='BI' then 0
						else 0
					end

			end,
		
		Jml_Biaya=
			case
				when @FieldYangDibandingkan='HK' or @FieldYangDibandingkan='PRESTASI' then
					case 
						when @OrderBulan='BI' then A_BI_JumlahUpah+R_BI_JumlahUpah
						else A_SBI_JumlahUpah+R_SBI_JumlahUpah
					end
				when @FieldYangDibandingkan='TUP' then
					case 
						when @OrderBulan='BI' then A_BI_NilaiTUP+R_BI_NilaiTUP
						else A_SBI_NilaiTUP+R_SBI_NilaiTUP
					end
				when @FieldYangDibandingkan='KAS' then
					case 
						when @OrderBulan='BI' then A_BI_NilaiKAS+R_BI_NilaiKAS
						else A_SBI_NilaiKAS+R_SBI_NilaiKAS
					end
				when @FieldYangDibandingkan='LL' then
					case 
						when @OrderBulan='BI' then A_BI_NilaiEAP+A_BI_NilaiLL+R_BI_NilaiEAP+R_BI_NilaiLL
						else A_SBI_NilaiEAP+A_SBI_NilaiLL+R_SBI_NilaiEAP+R_SBI_NilaiLL
					end
				when @FieldYangDibandingkan='SELURUH' then
					case 
						when @OrderBulan='BI' then A_BI_JumlahUpah+A_BI_NilaiTUP+A_BI_NilaiKAS+A_BI_NilaiEAP+A_BI_NilaiLL+
							R_BI_JumlahUpah+R_BI_NilaiTUP+R_BI_NilaiKAS+R_BI_NilaiEAP+R_BI_NilaiLL
						else A_SBI_JumlahUpah+A_SBI_NilaiTUP+A_SBI_NilaiKAS+A_SBI_NilaiEAP+A_SBI_NilaiLL+
							R_SBI_JumlahUpah+R_SBI_NilaiTUP+R_SBI_NilaiKAS+R_SBI_NilaiEAP+R_SBI_NilaiLL
					end
			end,
		
		Jml_PembandingFisik=0,
		Jml_PembandingBiaya=0
	FROM ArealEFRealisasi AS A
	
	
	INSERT INTO ArealEFViewResult 
	SELECT 
		LokasiId =
			case 
				when @Layer='Kebun' then (select top 1 kebunid from [ReferensiEF].[dbo].[Kebun] where kd_kbn=A.KodeLokasi)
				when @Layer='Afdeling' then (select top 1 afdelingid from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.KodeLokasi)
				else (select top 1 blokid from [ReferensiEF].[dbo].[Blok] where kd_blok=A.KodeLokasi)
			end,
		KodeLokasi=A.KodeLokasi,
		Peta='',
		Produksi=0,
		PembandingProduksi=
			case 
				when @Layer='Kebun' and @DibandingkanDengan='RKAP' then 
					case when @Budidaya!='01' then (select SUM(rbi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=A.KodeLokasi and [kodevar]='JADI' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir)
					else (select SUM(rbi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=A.KodeLokasi and [kodevar]='BASAH' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir) end
					
				when @Layer='Kebun' and @DibandingkanDengan='PKB' then 
					case when @Budidaya!='01' then (select SUM(pbi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=A.KodeLokasi and [kodevar]='JADI' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir)
					else (select SUM(pbi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=A.KodeLokasi and [kodevar]='BASAH' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir) end
				when @Layer='Afdeling' then (A.A_SBI_HasilPanen+A.R_SBI_HasilPanen+A.A_SBI_HasilPanenTBS+A.R_SBI_HasilPanenTBS+A.A_SBI_HasilPanenLump+A.R_SBI_HasilPanenLump)* 
					case when @Budidaya='02' then (select SUM(bi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=substring(A.KodeLokasi,1,2) and [kodevar]='JADI' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir)/(select SUM(bi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=substring(A.KodeLokasi,1,2) and [kodevar]='BASAH' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir)
					else 1 end
				else (A.A_SBI_HasilPanen+A.R_SBI_HasilPanen+A.A_SBI_HasilPanenTBS+A.R_SBI_HasilPanenTBS+A.A_SBI_HasilPanenLump+A.R_SBI_HasilPanenLump)*
					case when @Budidaya='02' then (select SUM(bi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=substring(A.KodeLokasi,1,2) and [kodevar]='JADI' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir)/(select SUM(bi) from [SPDK_KBN_KONSOL].[dbo].[AkunProduksiHP] where kodebud=@Budidaya and kodeunit=substring(A.KodeLokasi,1,2) and [kodevar]='BASAH' and [norek]='X' and tanggal>=@tglAwal and tanggal<=@tglAkhir)
					else 1 end
			end,	
		LuasTM=
			case 
				when @Layer='Kebun' then (select SUM(luas) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun=A.KodeLokasi and [status]='TM')
				when @Layer='Afdeling' then (select cast(SUM(luas) as decimal(18,4)) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun+kodeafdeling=A.KodeLokasi and [status]='TM')
				else (select SUM(luas) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun+kodeafdeling+kodeblok=A.KodeLokasi and [status]='TM')
			end,
		Nama=
			case 
				when @Layer='Kebun' then (select top 1 NamaKebun from SPDK_KBN_KONSOL.dbo.Ref_Kebun where KodeKebun=A.KodeLokasi)
				when @Layer='Afdeling' then (select top 1 namaafdeling from SPDK_KBN_KONSOL.dbo.Ref_Afdeling where kodekebun+kodeafdeling=A.KodeLokasi)
				else (select top 1 namablok from SPDK_KBN_KONSOL.dbo.Ref_Areal where kodekebun+kodeafdeling+kodeblok=A.KodeLokasi)
			end,
		R_Fisik=0,
		R_Biaya=0,
		A_Fisik=0,
		A_Biaya=0,
		R_PembandingFisik =
			case
				when @FieldYangDibandingkan='HK' then
					case 
						when @OrderBulan='BI' then R_BI_HKKerja
						else R_SBI_HKKerja
					end
				when @FieldYangDibandingkan='PRESTASI' then
					case 
						when @OrderBulan='BI' then R_BI_HasilLainLain+R_BI_HasilPanen+R_BI_HasilPanenLump+R_BI_HasilPanenTBS+R_BI_HasilPemelliharaan
						else R_SBI_HasilLainLain+R_SBI_HasilPanen+R_SBI_HasilPanenLump+R_SBI_HasilPanenTBS+R_SBI_HasilPemelliharaan
					end
				when @FieldYangDibandingkan='TUP' then
					case 
						when @OrderBulan='BI' then R_BI_FisikTUP
						else R_SBI_FisikTUP
					end
				when @FieldYangDibandingkan='KAS' then
					case 
						when @OrderBulan='BI' then R_BI_FisikKAS
						else R_SBI_FisikKAS
					end
				when @FieldYangDibandingkan='LL' then
					case 
						when @OrderBulan='BI' then R_BI_FisikEAP+R_BI_FisikLL
						else R_SBI_FisikEAP+R_SBI_FisikLL
					end
				when @FieldYangDibandingkan='SELURUH' then
					case 
						when @OrderBulan='BI' then 0
						else 0
					end
				when @FieldYangDibandingkan='PROTAS' then
					case 
						when @OrderBulan='BI' then 
							(R_BI_HasilPanen+R_BI_HasilPanenLump+R_BI_HasilPanenTBS+A_BI_HasilPanen+A_BI_HasilPanenLump+A_BI_HasilPanenTBS)
							/
							(case 
								when @Layer='Kebun' then (select SUM(luas) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun=A.KodeLokasi and [status]='TM')
								when @Layer='Afdeling' then (select cast(SUM(luas) as decimal(18,4)) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun+kodeafdeling=A.KodeLokasi and [status]='TM')
								else (select SUM(luas) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun+kodeafdeling+kodeblok=A.KodeLokasi and [status]='TM')
							end)
						else 
							(R_SBI_HasilPanen+R_SBI_HasilPanenLump+R_SBI_HasilPanenTBS+A_SBI_HasilPanen+A_SBI_HasilPanenLump+A_SBI_HasilPanenTBS)
							/
							(case 
								when @Layer='Kebun' then (select SUM(luas) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun=A.KodeLokasi and [status]='TM')
								when @Layer='Afdeling' then (select cast(SUM(luas) as decimal(18,4)) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun+kodeafdeling=A.KodeLokasi and [status]='TM')
								else (select SUM(luas) from [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] where kodebudidaya=@Budidaya and kodekebun+kodeafdeling+kodeblok=A.KodeLokasi and [status]='TM')
							end)
					end
			end,
		R_PembandingBiaya =
			case
				when @FieldYangDibandingkan='HK' or @FieldYangDibandingkan='PRESTASI'   then
					case 
						when @OrderBulan='BI' then R_BI_JumlahUpah
						else R_SBI_JumlahUpah
					end
				when @FieldYangDibandingkan='TUP' then
					case 
						when @OrderBulan='BI' then R_BI_NilaiTUP
						else R_SBI_NilaiTUP
					end
				when @FieldYangDibandingkan='KAS' then
					case 
						when @OrderBulan='BI' then R_BI_NilaiKAS
						else R_SBI_NilaiKAS
					end
				when @FieldYangDibandingkan='LL' then
					case 
						when @OrderBulan='BI' then R_BI_NilaiEAP+R_BI_NilaiLL
						else R_SBI_NilaiEAP+R_SBI_NilaiLL
					end
				when @FieldYangDibandingkan='SELURUH' then
					case 
						when @OrderBulan='BI' then R_BI_JumlahUpah+R_BI_NilaiTUP+R_BI_NilaiKAS+R_BI_NilaiEAP+R_BI_NilaiLL
						else R_SBI_JumlahUpah+R_SBI_NilaiTUP+R_SBI_NilaiKAS+R_SBI_NilaiEAP+R_SBI_NilaiLL
					end
					
			end,
		A_PembandingFisik =
			case
				when @FieldYangDibandingkan='HK' then
					case 
						when @OrderBulan='BI' then A_BI_HKKerja
						else A_SBI_HKKerja
					end
				when @FieldYangDibandingkan='PRESTASI' then
					case 
						when @OrderBulan='BI' then A_BI_HasilLainLain+A_BI_HasilPanen+A_BI_HasilPanenLump+A_BI_HasilPanenTBS+A_BI_HasilPemelliharaan
						else A_SBI_HasilLainLain+A_SBI_HasilPanen+A_SBI_HasilPanenLump+A_SBI_HasilPanenTBS+A_SBI_HasilPemelliharaan
					end
				when @FieldYangDibandingkan='TUP' then
					case 
						when @OrderBulan='BI' then A_BI_FisikTUP
						else A_SBI_FisikTUP
					end
				when @FieldYangDibandingkan='KAS' then
					case 
						when @OrderBulan='BI' then A_BI_FisikKAS
						else A_SBI_FisikKAS
					end
				when @FieldYangDibandingkan='LL' then
					case 
						when @OrderBulan='BI' then A_BI_FisikEAP+A_BI_FisikLL
						else A_SBI_FisikEAP+A_SBI_FisikLL
					end
				when @FieldYangDibandingkan='SELURUH' then
					case 
						when @OrderBulan='BI' then 0
						else 0
					end

			end,
		A_PembandingBiaya =
			case
				when @FieldYangDibandingkan='HK' or @FieldYangDibandingkan='PRESTASI' then
					case 
						when @OrderBulan='BI' then A_BI_JumlahUpah
						else A_SBI_JumlahUpah
					end
				when @FieldYangDibandingkan='TUP' then
					case 
						when @OrderBulan='BI' then A_BI_NilaiTUP
						else A_SBI_NilaiTUP
					end
				when @FieldYangDibandingkan='KAS' then
					case 
						when @OrderBulan='BI' then A_BI_NilaiKAS
						else A_SBI_NilaiKAS
					end
				when @FieldYangDibandingkan='LL' then
					case 
						when @OrderBulan='BI' then A_BI_NilaiEAP+A_BI_NilaiLL
						else A_SBI_NilaiEAP+A_SBI_NilaiLL
					end
				when @FieldYangDibandingkan='SELURUH' then
					case 
						when @OrderBulan='BI' then A_BI_JumlahUpah+A_BI_NilaiTUP+A_BI_NilaiKAS+A_BI_NilaiEAP+A_BI_NilaiLL
						else A_SBI_JumlahUpah+A_SBI_NilaiTUP+A_SBI_NilaiKAS+A_SBI_NilaiEAP+A_SBI_NilaiLL
					end
			end,
			Warna='',
			KeteranganWarna='',
			Jml_Fisik=0,
			Jml_Biaya=0,
			Jml_PembandingFisik=
			case
				when @FieldYangDibandingkan='HK' then
					case 
						when @OrderBulan='BI' then A_BI_HKKerja+R_BI_HKKerja
						else A_SBI_HKKerja+R_SBI_HKKerja
					end
				when @FieldYangDibandingkan='PRESTASI' then
					case 
						when @OrderBulan='BI' then A_BI_HasilLainLain+A_BI_HasilPanen+A_BI_HasilPanenLump+A_BI_HasilPanenTBS+A_BI_HasilPemelliharaan+
							R_BI_HasilLainLain+R_BI_HasilPanen+R_BI_HasilPanenLump+R_BI_HasilPanenTBS+R_BI_HasilPemelliharaan
						else A_SBI_HasilLainLain+A_SBI_HasilPanen+A_SBI_HasilPanenLump+A_SBI_HasilPanenTBS+A_SBI_HasilPemelliharaan+
							R_SBI_HasilLainLain+R_SBI_HasilPanen+R_SBI_HasilPanenLump+R_SBI_HasilPanenTBS+R_SBI_HasilPemelliharaan
					end
				when @FieldYangDibandingkan='TUP' then
					case 
						when @OrderBulan='BI' then A_BI_FisikTUP+R_BI_FisikTUP
						else A_SBI_FisikTUP+R_SBI_FisikTUP
					end
				when @FieldYangDibandingkan='KAS' then
					case 
						when @OrderBulan='BI' then A_BI_FisikKAS+R_BI_FisikKAS
						else A_SBI_FisikKAS+R_SBI_FisikKAS
					end
				when @FieldYangDibandingkan='LL' then
					case 
						when @OrderBulan='BI' then A_BI_FisikEAP+A_BI_FisikLL+R_BI_FisikEAP+R_BI_FisikLL
						else A_SBI_FisikEAP+A_SBI_FisikLL+R_SBI_FisikEAP+R_SBI_FisikLL
					end
				when @FieldYangDibandingkan='SELURUH' then
					case 
						when @OrderBulan='BI' then 0
						else 0
					end

			end,
			
			Jml_PembandingBiaya=
			case
				when @FieldYangDibandingkan='HK' or @FieldYangDibandingkan='PRESTASI' then
					case 
						when @OrderBulan='BI' then A_BI_JumlahUpah+R_BI_JumlahUpah
						else A_SBI_JumlahUpah+R_SBI_JumlahUpah
					end
				when @FieldYangDibandingkan='TUP' then
					case 
						when @OrderBulan='BI' then A_BI_NilaiTUP+R_BI_NilaiTUP
						else A_SBI_NilaiTUP+R_SBI_NilaiTUP
					end
				when @FieldYangDibandingkan='KAS' then
					case 
						when @OrderBulan='BI' then A_BI_NilaiKAS+R_BI_NilaiKAS
						else A_SBI_NilaiKAS+R_SBI_NilaiKAS
					end
				when @FieldYangDibandingkan='LL' then
					case 
						when @OrderBulan='BI' then A_BI_NilaiEAP+A_BI_NilaiLL+R_BI_NilaiEAP+R_BI_NilaiLL
						else A_SBI_NilaiEAP+A_SBI_NilaiLL+R_SBI_NilaiEAP+R_SBI_NilaiLL
					end
				when @FieldYangDibandingkan='SELURUH' then
					case 
						when @OrderBulan='BI' then A_BI_JumlahUpah+A_BI_NilaiTUP+A_BI_NilaiKAS+A_BI_NilaiEAP+A_BI_NilaiLL+
							R_BI_JumlahUpah+R_BI_NilaiTUP+R_BI_NilaiKAS+R_BI_NilaiEAP+R_BI_NilaiLL
						else A_SBI_JumlahUpah+A_SBI_NilaiTUP+A_SBI_NilaiKAS+A_SBI_NilaiEAP+A_SBI_NilaiLL+
							R_SBI_JumlahUpah+R_SBI_NilaiTUP+R_SBI_NilaiKAS+R_SBI_NilaiEAP+R_SBI_NilaiLL
					end
			end
			
	FROM ArealEFRekapPembanding AS A
	
	SELECT 
		LokasiId,
		KodeLokasi,
		Nama,
		Peta,
		Produksi=SUM(Produksi),
		PembandingProduksi=SUM(PembandingProduksi),
		LuasTM=avg(LuasTM),
		R_Fisik=SUM(R_Fisik),
		R_Biaya=SUM(R_Biaya),
		A_Fisik=SUM(A_Fisik),
		A_Biaya=SUM(A_Biaya),
		R_PembandingFisik=
			case
				when @DibandingkanDengan='INDUK' then (select AVG(R_Fisik) from ArealEFViewResult)
				else SUM(R_PembandingFisik)
			end,
		R_PembandingBiaya=
			case
				when @DibandingkanDengan='INDUK' then (select AVG(R_Biaya) from ArealEFViewResult)
				else SUM(R_PembandingBiaya)
			end,	
		A_PembandingFisik=
			case
				when @DibandingkanDengan='INDUK' then (select AVG(A_Fisik) from ArealEFViewResult)
				else SUM(A_PembandingFisik)
			end,	
		A_PembandingBiaya=
			case
				when @DibandingkanDengan='INDUK' then (select AVG(A_Biaya) from ArealEFViewResult)
				else SUM(A_PembandingBiaya)
			end,
		Warna='',
		KeteranganWarna='',
		Jml_Fisik=SUM(Jml_Fisik),
		Jml_Biaya=SUM(jml_biaya),
		Jml_PembandingFisik=SUM(jml_pembandingfisik),
		Jml_PembandingBiaya=sum(Jml_PembandingBiaya)	
		FROM ArealEFViewResult
		GROUP BY LokasiId,KodeLokasi,Peta,Nama
		
END		

 
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
CREATE PROCEDURE [dbo].[PetaViewDariKartu]
@Id as nvarchar(max),						-- kode induk nya, kalo @Layer='Kebun' @Id='', kalo @Layer='Afdeling' @Id=Kode Kebun, kalo @Layer='Blok' @Id = Kode Afdeling
@Layer as nvarchar(max),					-- pilihan nya : 'Kebun', 'Afdeling' atau 'Blok'
@BulanAwal as int,
@TahunAwal as int,
@BulanAkhir as int, 
@TahunAkhir as int,
@Budidaya as nvarchar(max),					-- Kode Budidaya
@KodeRekening as nvarchar(max),				-- Kode Rekening
@FieldYangDibandingkan as nvarchar(max),	-- 'HK','PRESTASI','TUP','KAS','LL','SELURUH','PROTAS'
@DibandingkanDengan as nvarchar(max),		-- 'INDUK','RKAP','PKB' atau 'BALAI'
@OrderBulan as nvarchar(max)				-- 'BI' atau 'SBI'
AS
BEGIN
	SET NOCOUNT ON;
	SET DATEFORMAT DMY

	DECLARE @tglAwal AS DATETIME 
	DECLARE @tglAkhir AS DATETIME 
	
	set @tglAwal = CAST('01/'+cast(@BulanAwal as varchar(10))+'/'+cast(@TahunAwal as varchar(10)) as datetime)  
	if @BulanAkhir=12
	begin
		set @tglAkhir = CAST('01/01/'+cast(@TahunAkhir+1 as varchar(10)) as datetime)  
		set @tglAkhir = @tglAkhir-1
	end
	else
	begin
		set @tglAkhir = CAST('01/'+cast(@BulanAkhir+1 as varchar(10))+'/'+cast(@TahunAkhir as varchar(10)) as datetime)  
		set @tglAkhir = @tglAkhir-1
	end

	-- View
	
	DECLARE @ArealEFViewResult TABLE
	(
		LokasiId uniqueidentifier null default newid(),
		KodeLokasi nvarchar(100) null default '',
		Peta nvarchar(max) null default '',
		Produksi decimal(18,2) null default 0.00,
		PembandingProduksi decimal(18,2) null default 0.00,
		LuasTM decimal(18,4) null default 0.0000,
		Nama nvarchar(max) null default '',
		R_Fisik decimal(18,2) null default 0.00,
		R_Biaya decimal(18,2) null default 0.00,
		A_Fisik decimal(18,2) null default 0.00,
		A_Biaya decimal(18,2) null default 0.00,
		R_PembandingFisik decimal(18,2) null default 0.00,
		R_PembandingBiaya decimal(18,2) null default 0.00,
		A_PembandingFisik decimal(18,2) null default 0.00,
		A_PembandingBiaya decimal(18,2) null default 0.00,
		Warna nvarchar(max) null default '',
		KeteranganWarna nvarchar(max) null default '',
		Jml_Fisik decimal(18,2) null default 0.00,
		Jml_Biaya decimal(18,2) null default 0.00,
		Jml_PembandingFisik decimal(18,2) null default 0.00,
		Jml_PembandingBiaya decimal(18,2) null default 0.00
	)		

	
	IF (@Layer='Kebun')
	BEGIN
		-- dari produksi 
		IF @Budidaya != '01'
		BEGIN 
			INSERT INTO @ArealEFViewResult 
			SELECT 
				LokasiId = (select top 1 kebunid from [ReferensiEF].[dbo].[Kebun] where kd_kbn=A.kodeunit),
				KodeLokasi= A.KodeUnit,
				Peta='',
				Produksi=sum(A.bi),
				PembandingProduksi=
					case 
						when @DibandingkanDengan='RKAP' then sum(A.rbi)
						when @DibandingkanDengan='PKB' then sum(A.pbi)
						when @DibandingkanDengan='INDUK' then 
							(select sum(bi) from SPDK_KanpusEF.dbo.AkunProduksiHP WHERE tanggal >= @tglAwal and tanggal <= @tglAkhir and norek='X' and kodevar='JADI' and KodeBud=@Budidaya )
						else 0
					end,	
				LuasTM=0,
				Nama=(select top 1 Nama from [ReferensiEF].[dbo].[Kebun] where kd_kbn=A.kodeunit),
				R_Fisik = 0,
				R_Biaya = 0,
				A_Fisik = 0,
				A_Biaya = 0,
				R_PembandingFisik=0,
				R_PembandingBiaya=0,
				A_PembandingFisik=0,
				A_PembandingBiaya=0,
				Warna='',
				KeteranganWarna='',
				Jml_Fisik=0,
				Jml_Biaya=0,
				Jml_PembandingFisik=0,
				Jml_PembandingBiaya=0
			FROM SPDK_KanpusEF.dbo.AkunProduksiHP AS A
			WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir and norek='X' and kodevar='JADI' and A.KodeBud=@Budidaya
			GROUP BY A.kodeunit
		END
		ELSE
		BEGIN
			INSERT INTO @ArealEFViewResult 
			SELECT 
				LokasiId = (select top 1 kebunid from [ReferensiEF].[dbo].[Kebun] where kd_kbn=A.kodeunit),
				KodeLokasi= A.KodeUnit,
				Peta='',
				Produksi=sum(A.bi),
				PembandingProduksi=
					case 
						when @DibandingkanDengan='RKAP' then sum(A.rbi)
						when @DibandingkanDengan='PKB' then sum(A.pbi)
						when @DibandingkanDengan='INDUK' then 
							(select sum(bi) from SPDK_KanpusEF.dbo.AkunProduksiHP WHERE tanggal >= @tglAwal and tanggal <= @tglAkhir and norek='X' and kodevar='JADI' and KodeBud=@Budidaya )
						else 0
					end,	
				LuasTM=0,
				Nama=(select top 1 Nama from [ReferensiEF].[dbo].[Kebun] where kd_kbn=A.kodeunit),
				R_Fisik = 0,
				R_Biaya = 0,
				A_Fisik = 0,
				A_Biaya = 0,
				R_PembandingFisik=0,
				R_PembandingBiaya=0,
				A_PembandingFisik=0,
				A_PembandingBiaya=0,
				Warna='',
				KeteranganWarna='',
				Jml_Fisik=0,
				Jml_Biaya=0,
				Jml_PembandingFisik=0,
				Jml_PembandingBiaya=0
			FROM SPDK_KanpusEF.dbo.AkunProduksiHP AS A
			WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir and norek='X' and kodevar='BASAH' and A.KodeBud=@Budidaya
			GROUP BY A.kodeunit
		END	

		-- dari areal
		INSERT INTO @ArealEFViewResult 
		SELECT 
			LokasiId = (select top 1 kebunid from [ReferensiEF].[dbo].[Kebun] where kd_kbn=A.kodekebun),
			KodeLokasi= A.KodeKebun,
			Peta='',
			Produksi=0,
			PembandingProduksi=0,
			LuasTM=sum(A.luas),
			Nama=(select top 1 Nama from [ReferensiEF].[dbo].[Kebun] where kd_kbn=A.kodekebun),
			R_Fisik = 0,
			R_Biaya = 0,
			A_Fisik = 0,
			A_Biaya = 0,
			R_PembandingFisik=0,
			R_PembandingBiaya=0,
			A_PembandingFisik=0,
			A_PembandingBiaya=0,
			Warna='',
			KeteranganWarna='',
			Jml_Fisik=0,
			Jml_Biaya=0,
			Jml_PembandingFisik=
				case when @DibandingkanDengan='INDUK' then
					(select SUM(luas) FROM [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] WHERE kodebudidaya=@Budidaya and [status]='TM')
				else
					SUM(A.luas)
				end,		
			Jml_PembandingBiaya=0
		FROM [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] AS A
		WHERE A.kodebudidaya=@Budidaya and [status]='TM'
		GROUP BY A.kodekebun
		

		select * from @ArealEFViewResult 

		-- dari kartu
		INSERT INTO @ArealEFViewResult 
		SELECT 
			LokasiId = (select top 1 kebunid from [ReferensiEF].[dbo].[Kebun] where kd_kbn=A.kodeunit),
			KodeLokasi= A.KodeUnit,
			Peta='',
			Produksi=0,
			PembandingProduksi=0,
			LuasTM=0,
			Nama=(select top 1 Nama from [ReferensiEF].[dbo].[Kebun] where kd_kbn=A.kodeunit),
			R_Fisik = 0,
			R_Biaya = 0,
			A_Fisik = 0,
			A_Biaya = 0,
			R_PembandingFisik=0,
			R_PembandingBiaya=0,
			A_PembandingFisik=0,
			A_PembandingBiaya=0,
			Warna='',
			KeteranganWarna='',
			Jml_Fisik=sum(A.Jml_fisik),
			Jml_Biaya=sum(A.Debet-A.Kredit),
			Jml_PembandingFisik=0,
			Jml_PembandingBiaya=0
		FROM [SPDK_KBN_KONSOL].[dbo].[Kartu] AS A
		WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
			and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
			and A.KodeBudidaya=@Budidaya
		GROUP BY A.KodeUnit
		
		-- dari RKAP
		INSERT INTO @ArealEFViewResult 
		SELECT 
			LokasiId = (select top 1 kebunid from [ReferensiEF].[dbo].[Kebun] where kd_kbn=A.kodeunit),
			KodeLokasi= A.KodeUnit,
			Peta='',
			Produksi=0,
			PembandingProduksi=0,
			LuasTM=0,
			Nama=(select top 1 Nama from [ReferensiEF].[dbo].[Kebun] where kd_kbn=A.kodeunit),
			R_Fisik = 0,
			R_Biaya = 0,
			A_Fisik = 0,
			A_Biaya = 0,
			R_PembandingFisik=0,
			R_PembandingBiaya=0,
			A_PembandingFisik=0,
			A_PembandingBiaya=0,
			Warna='',
			KeteranganWarna='',
			Jml_Fisik=0,
			Jml_Biaya=0,
			Jml_PembandingFisik=0,
			Jml_PembandingBiaya=
				case 
					when @DibandingkanDengan='RKAP' then sum(A.nilai_RKAP)
					when @DibandingkanDengan='PKB' then sum(A.nilai_PMK)
					else 0
				end	
		FROM SPDK_KanpusEF.dbo.AkunRKAP AS A
		WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
			and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
			and A.KodeBud=@Budidaya
		GROUP BY A.KodeUnit
		
	END
	ELSE IF (@Layer='Afdeling')
	BEGIN
		-- Produksi ambil dari basah LHKH dan SPK x BK dari Produksi
		IF @Budidaya!='01'
		BEGIN
			INSERT INTO @ArealEFViewResult 
			SELECT 
				LokasiId = (select top 1 afdelingid from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodeunit+A.kodeafd),
				KodeLokasi= A.KodeUnit+A.kodeafd,
				Peta='',
				Produksi=(select sum(bi) from [SPDK_KanpusEF].[dbo].[AkunProduksiHP] where kodekbn=A.kodeunit and kodevar='JADI' and norek='X' and tanggal >= @tglAwal and tanggal <= @tglAkhir and kodebud=@Budidaya)*
							sum(A.hslpanen+A.hslpanenlump+A.hsllain2)/
							(select sum(hslpanen+hslpanenlump+hsllain2) from [SPDK_KanpusEF].[dbo].[GajiAbsensi] WHERE tanggal >= @tglAwal and tanggal <= @tglAkhir and substring(norek,1,5)='60203' and KodeBud=@Budidaya and kodeunit=@Id),
				PembandingProduksi=0,
				LuasTM=0,
				Nama=(select top 1 Nama from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodeunit+A.kodeafd),
				R_Fisik = 0,
				R_Biaya = 0,
				A_Fisik = 0,
				A_Biaya = 0,
				R_PembandingFisik=0,
				R_PembandingBiaya=0,
				A_PembandingFisik=0,
				A_PembandingBiaya=0,
				Warna='',
				KeteranganWarna='',
				Jml_Fisik=0,
				Jml_Biaya=0,
				Jml_PembandingFisik=0,
				Jml_PembandingBiaya=0
			FROM SPDK_KanpusEF.dbo.GajiAbsensi AS A
			WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir and substring(norek,1,5)='60203' and A.KodeBud=@Budidaya and A.kodeunit=@Id
			GROUP BY A.kodeunit,A.kodeafd
			
			-- dari produksi sbg pembanding
			INSERT INTO @ArealEFViewResult 
			SELECT 
				LokasiId = (select top 1 afdelingid from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodeunit+A.kodeafd),
				KodeLokasi= A.KodeUnit+A.kodeafd,
				Peta='',
				Produksi=0,
				PembandingProduksi=
					case 
						when @DibandingkanDengan='RKAP' then
							(select sum(rbi) from [SPDK_KanpusEF].[dbo].[AkunProduksiHP] where kodekbn=A.kodeunit and kodevar='JADI' and norek='X' and tanggal >= @tglAwal and tanggal <= @tglAkhir and kodebud=@Budidaya)*
							sum(A.hslpanen+A.hslpanenlump+A.hsllain2)/
							(select sum(hslpanen+hslpanenlump+hsllain2) from [SPDK_KanpusEF].[dbo].[GajiAbsensi] WHERE tanggal >= @tglAwal and tanggal <= @tglAkhir and substring(norek,1,5)='60203' and KodeBud=@Budidaya and kodeunit=@Id)
						when @DibandingkanDengan='PKB' then
							(select sum(pbi) from [SPDK_KanpusEF].[dbo].[AkunProduksiHP] where kodekbn=A.kodeunit and kodevar='JADI' and norek='X' and tanggal >= @tglAwal and tanggal <= @tglAkhir and kodebud=@Budidaya)*
							sum(A.hslpanen+A.hslpanenlump+A.hsllain2)/
							(select sum(hslpanen+hslpanenlump+hsllain2) from [SPDK_KanpusEF].[dbo].[GajiAbsensi] WHERE tanggal >= @tglAwal and tanggal <= @tglAkhir and substring(norek,1,5)='60203' and KodeBud=@Budidaya and kodeunit=@Id)
						when @DibandingkanDengan='INDUK' then
							(select sum(bi) from [SPDK_KanpusEF].[dbo].[AkunProduksiHP] where kodekbn=A.kodeunit and kodevar='JADI' and norek='X' and tanggal >= @tglAwal and tanggal <= @tglAkhir and kodebud=@Budidaya and kodeunit=@Id)
					end,						
				LuasTM=0,
				Nama=(select top 1 Nama from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodeunit+A.kodeafd),
				R_Fisik = 0,
				R_Biaya = 0,
				A_Fisik = 0,
				A_Biaya = 0,
				R_PembandingFisik=0,
				R_PembandingBiaya=0,
				A_PembandingFisik=0,
				A_PembandingBiaya=0,
				Warna='',
				KeteranganWarna='',
				Jml_Fisik=0,
				Jml_Biaya=0,
				Jml_PembandingFisik=0,
				Jml_PembandingBiaya=0
			FROM SPDK_KanpusEF.dbo.GajiAbsensi AS A
			WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir and substring(norek,1,5)='60203' and A.KodeBud=@Budidaya and A.kodeunit=@Id
			GROUP BY A.kodeunit,A.kodeafd		
		END
		ELSE
		BEGIN
			INSERT INTO @ArealEFViewResult 
			SELECT 
				LokasiId = (select top 1 afdelingid from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodeunit+A.kodeafd),
				KodeLokasi= A.KodeUnit+A.kodeafd,
				Peta='',
				Produksi=(select sum(bi) from [SPDK_KanpusEF].[dbo].[AkunProduksiHP] where kodekbn=A.kodeunit and kodevar='BASAH' and norek='X' and tanggal >= @tglAwal and tanggal <= @tglAkhir and kodebud=@Budidaya)*
							sum(A.hslpanen+A.hslpanenlump+A.hsllain2)/
							(select sum(hslpanen+hslpanenlump+hsllain2) from [SPDK_KanpusEF].[dbo].[GajiAbsensi] WHERE tanggal >= @tglAwal and tanggal <= @tglAkhir and substring(norek,1,5)='60203' and KodeBud=@Budidaya and kodeunit=@Id),
				PembandingProduksi=0,
				LuasTM=0,
				Nama=(select top 1 Nama from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodeunit+A.kodeafd),
				R_Fisik = 0,
				R_Biaya = 0,
				A_Fisik = 0,
				A_Biaya = 0,
				R_PembandingFisik=0,
				R_PembandingBiaya=0,
				A_PembandingFisik=0,
				A_PembandingBiaya=0,
				Warna='',
				KeteranganWarna='',
				Jml_Fisik=0,
				Jml_Biaya=0,
				Jml_PembandingFisik=0,
				Jml_PembandingBiaya=0
			FROM SPDK_KanpusEF.dbo.GajiAbsensi AS A
			WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir and substring(norek,1,5)='60203' and A.KodeBud=@Budidaya and A.kodeunit=@Id
			GROUP BY A.kodeunit,A.kodeafd
			
			-- dari produksi sbg pembanding
			INSERT INTO @ArealEFViewResult 
			SELECT 
				LokasiId = (select top 1 afdelingid from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodeunit+A.kodeafd),
				KodeLokasi= A.KodeUnit+A.kodeafd,
				Peta='',
				Produksi=0,
				PembandingProduksi=
					case 
						when @DibandingkanDengan='RKAP' then
							(select sum(rbi) from [SPDK_KanpusEF].[dbo].[AkunProduksiHP] where kodekbn=A.kodeunit and kodevar='BASAH' and norek='X' and tanggal >= @tglAwal and tanggal <= @tglAkhir and kodebud=@Budidaya)*
							sum(A.hslpanen+A.hslpanenlump+A.hsllain2)/
							(select sum(hslpanen+hslpanenlump+hsllain2) from [SPDK_KanpusEF].[dbo].[GajiAbsensi] WHERE tanggal >= @tglAwal and tanggal <= @tglAkhir and substring(norek,1,5)='60203' and KodeBud=@Budidaya and kodeunit=@Id)
						when @DibandingkanDengan='PKB' then
							(select sum(pbi) from [SPDK_KanpusEF].[dbo].[AkunProduksiHP] where kodekbn=A.kodeunit and kodevar='BASAH' and norek='X' and tanggal >= @tglAwal and tanggal <= @tglAkhir and kodebud=@Budidaya)*
							sum(A.hslpanen+A.hslpanenlump+A.hsllain2)/
							(select sum(hslpanen+hslpanenlump+hsllain2) from [SPDK_KanpusEF].[dbo].[GajiAbsensi] WHERE tanggal >= @tglAwal and tanggal <= @tglAkhir and substring(norek,1,5)='60203' and KodeBud=@Budidaya and kodeunit=@Id)
						when @DibandingkanDengan='INDUK' then
							(select sum(bi) from [SPDK_KanpusEF].[dbo].[AkunProduksiHP] where kodekbn=A.kodeunit and kodevar='BASAH' and norek='X' and tanggal >= @tglAwal and tanggal <= @tglAkhir and kodebud=@Budidaya and kodeunit=@Id)
					end,						
				LuasTM=0,
				Nama=(select top 1 Nama from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodeunit+A.kodeafd),
				R_Fisik = 0,
				R_Biaya = 0,
				A_Fisik = 0,
				A_Biaya = 0,
				R_PembandingFisik=0,
				R_PembandingBiaya=0,
				A_PembandingFisik=0,
				A_PembandingBiaya=0,
				Warna='',
				KeteranganWarna='',
				Jml_Fisik=0,
				Jml_Biaya=0,
				Jml_PembandingFisik=0,
				Jml_PembandingBiaya=0
			FROM SPDK_KanpusEF.dbo.GajiAbsensi AS A
			WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir and substring(norek,1,5)='60203' and A.KodeBud=@Budidaya and A.kodeunit=@Id
			GROUP BY A.kodeunit,A.kodeafd		
		END
		-- dari areal
		INSERT INTO @ArealEFViewResult 
		SELECT 
			LokasiId = (select top 1 afdelingid from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodekebun+A.kodeafdeling),
			KodeLokasi= A.kodekebun+A.kodeafdeling,
			Peta='',
			Produksi=0,
			PembandingProduksi=0,
			LuasTM=sum(A.luas),
			Nama=(select top 1 Nama from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodekebun+A.kodeafdeling),
			R_Fisik = 0,
			R_Biaya = 0,
			A_Fisik = 0,
			A_Biaya = 0,
			R_PembandingFisik=0,
			R_PembandingBiaya=0,
			A_PembandingFisik=0,
			A_PembandingBiaya=0,
			Warna='',
			KeteranganWarna='',
			Jml_Fisik=0,
			Jml_Biaya=0,
			Jml_PembandingFisik=
				case when @DibandingkanDengan='INDUK' then
					(select SUM(luas) FROM [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] WHERE kodebudidaya=@Budidaya and [status]='TM' and kodekebun=@Id)
				else
					SUM(A.luas)
				end,		
			Jml_PembandingBiaya=0
		FROM [SPDK_KBN_KONSOL].[dbo].[Ref_Areal] AS A
		WHERE A.kodebudidaya=@Budidaya and [status]='TM' and A.kodekebun=@Id
		GROUP BY A.kodekebun,A.kodeafdeling
		
		-- dari kartu
		INSERT INTO @ArealEFViewResult 
		SELECT 
			LokasiId = (select top 1 afdelingid from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodeunit+A.KodeAfdeling),
			KodeLokasi= A.KodeUnit+A.KodeAfdeling,
			Peta='',
			Produksi=0,
			PembandingProduksi=0,
			LuasTM=0,
			Nama=(select top 1 Nama from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodeunit+A.KodeAfdeling),
			R_Fisik = 0,
			R_Biaya = 0,
			A_Fisik = 0,
			A_Biaya = 0,
			R_PembandingFisik=0,
			R_PembandingBiaya=0,
			A_PembandingFisik=0,
			A_PembandingBiaya=0,
			Warna='',
			KeteranganWarna='',
			Jml_Fisik=sum(A.Jml_fisik),
			Jml_Biaya=sum(A.Debet-A.Kredit),
			Jml_PembandingFisik=0,
			Jml_PembandingBiaya=0
		FROM [SPDK_KBN_KONSOL].[dbo].[Kartu] AS A
		WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
			and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
			and A.KodeBudidaya=@Budidaya and A.KodeUnit=@Id
		GROUP BY A.KodeUnit,A.KodeAfdeling
		
		-- dari RKAP
		INSERT INTO @ArealEFViewResult 
		SELECT 
			LokasiId = (select top 1 afdelingid from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodeunit+A.kodeafd),
			KodeLokasi= A.KodeUnit+A.kodeafd,
			Peta='',
			Produksi=0,
			PembandingProduksi=0,
			LuasTM=0,
			Nama=(select top 1 Nama from [ReferensiEF].[dbo].[Afdeling] where kd_afd=A.kodeunit+A.kodeafd),
			R_Fisik = 0,
			R_Biaya = 0,
			A_Fisik = 0,
			A_Biaya = 0,
			R_PembandingFisik=0,
			R_PembandingBiaya=0,
			A_PembandingFisik=0,
			A_PembandingBiaya=0,
			Warna='',
			KeteranganWarna='',
			Jml_Fisik=0,
			Jml_Biaya=0,
			Jml_PembandingFisik=0,
			Jml_PembandingBiaya=
				case 
					when @DibandingkanDengan='RKAP' then sum(A.nilai_RKAP)
					when @DibandingkanDengan='PKB' then sum(A.nilai_PMK)
					else 0
				end	
		FROM SPDK_KanpusEF.dbo.AkunRKAP AS A
		WHERE A.tanggal >= @tglAwal and A.tanggal <= @tglAkhir 
			and 
			(
				substring(A.norek,1,3) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,5) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,7) in (select [element] from dbo.func_split(@KodeRekening,',')) or
				substring(A.norek,1,9) in (select [element] from dbo.func_split(@KodeRekening,',')) 
			) 
			and A.KodeBud=@Budidaya and A.KodeUnit=@Id
		GROUP BY A.KodeUnit,A.kodeafd
	END

	declare @budID as uniqueidentifier
	set @budID = (select top 1 MainBudidayaId from ReferensiEF.dbo.MainBudidaya where kd_bud=@Budidaya)
	
	
		SELECT 
			A.LokasiId,
			A.KodeLokasi,
			A.Nama,
			A.Peta,
			Produksi=SUM(A.Produksi),
			PembandingProduksi=SUM(A.PembandingProduksi),
			LuasTM=sum(A.LuasTM),
			R_Fisik=0,
			R_Biaya=0,
			A_Fisik=0,
			A_Biaya=0,
			R_PembandingFisik=0,
			Warna='',
			KeteranganWarna='',
			Jml_Fisik=SUM(A.Jml_Fisik),
			Jml_Biaya=SUM(A.jml_biaya),
			Jml_PembandingFisik=SUM(A.jml_pembandingfisik),
			Jml_PembandingBiaya=sum(A.Jml_PembandingBiaya)	
			FROM @ArealEFViewResult A 
			GROUP BY LokasiId,KodeLokasi,Peta,Nama
			ORDER by A.KodeLokasi
			
END		



GO
