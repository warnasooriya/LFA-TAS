USE [<<DBNAME>>]
GO
/****** Object:  Table [dbo].[BAndWPolicy]    Script Date: 11/13/2015 6:40:01 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BAndWPolicy](
	[PolicyId] [uniqueidentifier] NOT NULL,
	[BAndWId] [uniqueidentifier] NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BAndWPolicy] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BrownAndWhiteDetails]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BrownAndWhiteDetails](
	[Id] [uniqueidentifier] NOT NULL,
	[ItemPurchasedDate] [datetime] NULL,
	[MakeId] [uniqueidentifier] NULL,
	[ModelId] [uniqueidentifier] NULL,
	[SerialNo] [nvarchar](50) NULL,
	[ItemPrice] [numeric](18, 0) NULL,
	[CategoryId] [uniqueidentifier] NULL,
	[ModelYear] [nvarchar](50) NULL,
	[AddnSerialNo] [nvarchar](50) NULL,
	[ItemStatusId] [uniqueidentifier] NULL,
	[InvoiceNo] [nvarchar](50) NULL,
	[ModelCode] [nvarchar](50) NULL,
	[DealerPrice] [numeric](18, 0) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_BrownAndWhiteDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BundledProduct]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BundledProduct](
	[Id] [uniqueidentifier] NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL,
	[ParentProductId] [uniqueidentifier] NOT NULL,
	[IsCurrentProduct] [bit] NOT NULL,
 CONSTRAINT [PK_BundledProduct] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[City]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[City](
	[CityName] [varchar](50) NOT NULL,
	[ZipCode] [varchar](10) NULL,
	[Id] [uniqueidentifier] NULL,
	[CountryId] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommodityCategory]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommodityCategory](
	[CommodityCategoryId] [uniqueidentifier] NOT NULL,
	[CommodityCategoryDescription] [nvarchar](150) NULL,
	[CommodityTypeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK__Commodit__659BE4D9FBB7E6BE] PRIMARY KEY CLUSTERED 
(
	[CommodityCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CommodityType]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CommodityType](
	[CommodityTypeId] [uniqueidentifier] NOT NULL,
	[CommodityTypeDescription] [nvarchar](150) NULL,
	[DisplayDescription] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[CommodityTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Contract]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contract](
	[Id] [uniqueidentifier] NOT NULL,
	[CountryId] [uniqueidentifier] NULL,
	[DealerId] [uniqueidentifier] NULL,
	[CommodityTypeId] [uniqueidentifier] NULL,
	[ProductId] [uniqueidentifier] NULL,
	[LinkDealId] [uniqueidentifier] NULL,
	[ItemStatusId] [uniqueidentifier] NULL,
	[ManufacturerWarrantyId] [uniqueidentifier] NULL,
	[WarrantyTypeId] [uniqueidentifier] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[IsAutoRenewal] [bit] NULL,
	[IsActive] [bit] NULL,
	[DealName] [nvarchar](200) NULL,
	[DealType] [nvarchar](500) NULL,
	[Remark] [nvarchar](500) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContractExtensionCylinderCount]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractExtensionCylinderCount](
	[Id] [uniqueidentifier] NOT NULL,
	[ContractExtensionId] [uniqueidentifier] NULL,
	[CylinderCountId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ContractExtensionCylinderCount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContractExtensionEngineCapacity]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractExtensionEngineCapacity](
	[Id] [uniqueidentifier] NOT NULL,
	[ContractExtensionId] [uniqueidentifier] NULL,
	[EngineCapacityId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ContractExtensionEngineCapacity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContractExtensionMake]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractExtensionMake](
	[Id] [uniqueidentifier] NOT NULL,
	[ContractExtensionId] [uniqueidentifier] NULL,
	[MakeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ContractExtensionMake] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContractExtensionModel]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractExtensionModel](
	[Id] [uniqueidentifier] NOT NULL,
	[ContractExtensionId] [uniqueidentifier] NULL,
	[ModelId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ContractExtensionModel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContractExtensions]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractExtensions](
	[Id] [uniqueidentifier] NOT NULL,
	[ContractId] [uniqueidentifier] NULL,
	[ExtensionTypeId] [uniqueidentifier] NULL,
	[PremiumBasedOnId] [uniqueidentifier] NULL,
	[IsCustAvailable] [bit] NULL,
	[Min] [numeric](18, 0) NULL,
	[Max] [numeric](18, 0) NULL,
	[PremiumTotal] [numeric](18, 0) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
	[AttributeSpecification] [nvarchar](500) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContractExtensionsPremiumAddon]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractExtensionsPremiumAddon](
	[Id] [uniqueidentifier] NOT NULL,
	[ContractExtensionId] [uniqueidentifier] NULL,
	[PremiumAddonTypeId] [uniqueidentifier] NULL,
	[Value] [int] NULL,
 CONSTRAINT [PK_ContractExtensionsPremiumAddon] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Country]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Country](
	[CountryCode] [varchar](5) NULL,
	[CountryName] [varchar](50) NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[CurrencyCode] [nvarchar](50) NULL,
	[CurrencySymbol] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Currency]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currency](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Country] [nvarchar](100) NULL,
	[Currency] [nvarchar](100) NULL,
	[Code] [nvarchar](100) NULL,
	[Symbol] [nvarchar](100) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customer]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NULL,
	[UserName] [varchar](20) NULL,
	[Password] [varchar](30) NULL,
	[NationalityId] [int] NULL,
	[DateOfBirth] [date] NULL,
	[MobileNo] [varchar](15) NULL,
	[OtherTelNo] [varchar](15) NULL,
	[CustomerTypeId] [int] NOT NULL,
	[UsageTypeId] [int] NOT NULL,
	[Gender] [char](1) NOT NULL,
	[Address1] [varchar](100) NULL,
	[Address2] [varchar](100) NULL,
	[Address3] [varchar](100) NULL,
	[Address4] [varchar](100) NULL,
	[IDNo] [varchar](15) NOT NULL,
	[IDTypeId] [int] NOT NULL,
	[DLIssueDate] [date] NULL,
	[Email] [varchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[BusinessName] [varchar](100) NULL,
	[BusinessAddress1] [varchar](100) NULL,
	[BusinessAddress2] [varchar](100) NULL,
	[BusinessAddress3] [varchar](100) NULL,
	[BusinessAddress4] [varchar](100) NULL,
	[BusinessTelNo] [varchar](15) NULL,
	[EntryDateTime] [datetime] NOT NULL,
	[EntryUserId] [varchar](50) NOT NULL,
	[LastModifiedDateTime] [datetime] NULL,
	[ProfilePicture] [image] NULL,
	[CountryId] [uniqueidentifier] NULL,
	[CityId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CustomerType]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CustomerType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerTypeName] [varchar](30) NOT NULL,
	[CustomerTypeDescription] [varchar](100) NULL,
 CONSTRAINT [PK_CustomerType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CylinderCount]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CylinderCount](
	[Id] [uniqueidentifier] NOT NULL,
	[Count] [int] NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dealer]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dealer](
	[Id] [uniqueidentifier] NOT NULL,
	[DealerCode] [nvarchar](200) NULL,
	[DealerName] [nvarchar](500) NULL,
	[DealerAliase] [nvarchar](200) NULL,
	[CountryId] [uniqueidentifier] NULL,
	[Type] [nvarchar](200) NULL,
	[CommodityTypeId] [uniqueidentifier] NULL,
	[InsurerId] [uniqueidentifier] NULL,
	[CityId] [uniqueidentifier] NULL,
	[Location] [nvarchar](500) NULL,
	[IsActive] [bit] NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Dealer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DealerLocation]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DealerLocation](
	[Id] [uniqueidentifier] NOT NULL,
	[DealerId] [uniqueidentifier] NULL,
	[CityId] [uniqueidentifier] NULL,
	[SalesContactPerson] [nvarchar](500) NULL,
	[SalesTelephone] [nvarchar](500) NULL,
	[SalesFax] [nvarchar](500) NULL,
	[SalesEmail] [nvarchar](500) NULL,
	[Location] [nvarchar](500) NULL,
	[ServiceContactPerson] [nvarchar](500) NULL,
	[ServiceTelephone] [nvarchar](500) NULL,
	[ServiceFax] [nvarchar](500) NULL,
	[ServiceEmail] [nvarchar](500) NULL,
	[HeadOfficeLocation] [nvarchar](500) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DealerMakes]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DealerMakes](
	[Id] [uniqueidentifier] NOT NULL,
	[DealerId] [uniqueidentifier] NULL,
	[MakeId] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DriveType]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DriveType](
	[Id] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](50) NULL,
	[DriveTypeDescription] [nvarchar](150) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EngineCapacity]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EngineCapacity](
	[Id] [uniqueidentifier] NOT NULL,
	[EngineCapacityNumber] [decimal](18, 4) NULL,
	[MesureType] [nvarchar](20) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_EngineCapacity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExtensionType]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtensionType](
	[Id] [uniqueidentifier] NOT NULL,
	[Km] [int] NULL,
	[Month] [int] NULL,
	[Hours] [int] NULL,
	[CommodityTypeId] [uniqueidentifier] NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
	[ExtensionName] [nvarchar](500) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FuelType]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FuelType](
	[FuelTypeId] [uniqueidentifier] NOT NULL,
	[FuelTypeCode] [nvarchar](200) NULL,
	[FuelTypeDescription] [nvarchar](250) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_FuelType] PRIMARY KEY CLUSTERED 
(
	[FuelTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IDType]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IDType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IDTypeName] [varchar](20) NOT NULL,
	[IDTypeDescription] [varchar](50) NULL,
 CONSTRAINT [PK_IDType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Image]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Image](
	[Id] [uniqueidentifier] NOT NULL,
	[ImageName] [nvarchar](200) NULL,
	[ImageByte] [varbinary](max) NULL,
	[Description] [nvarchar](500) NULL,
	[ImageStatus] [bit] NULL,
	[DateUploaded] [datetime] NULL,
 CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Insurer]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Insurer](
	[Id] [uniqueidentifier] NOT NULL,
	[InsurerCode] [nvarchar](100) NULL,
	[BookletCode] [nvarchar](50) NULL,
	[InsurerShortName] [nvarchar](200) NULL,
	[InsurerFullName] [nvarchar](500) NULL,
	[Comments] [nvarchar](1500) NULL,
	[IsActive] [bit] NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InsurerCountries]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InsurerCountries](
	[Id] [uniqueidentifier] NOT NULL,
	[InsurerId] [uniqueidentifier] NULL,
	[CountryId] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ItemStatus]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemStatus](
	[Id] [uniqueidentifier] NOT NULL,
	[Status] [nvarchar](200) NULL,
	[ItemStatusDescription] [nvarchar](500) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Log]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Thread] [varchar](255) NOT NULL,
	[Level] [varchar](50) NOT NULL,
	[Logger] [varchar](255) NOT NULL,
	[Message] [varchar](4000) NOT NULL,
	[Exception] [varchar](2000) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Make]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Make](
	[Id] [uniqueidentifier] NOT NULL,
	[MakeName] [nvarchar](200) NULL,
	[MakeCode] [nvarchar](200) NULL,
	[CommodityTypeId] [uniqueidentifier] NULL,
	[ManufacturerId] [uniqueidentifier] NULL,
	[WarantyGiven] [bit] NULL,
	[IsActive] [bit] NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Manufacturer]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Manufacturer](
	[Id] [uniqueidentifier] NOT NULL,
	[ManufacturerCode] [varchar](20) NOT NULL,
	[ManufacturerName] [varchar](50) NOT NULL,
	[ManufacturerClassId] [uniqueidentifier] NULL,
	[IsWarrentyGiven] [bit] NULL,
	[IsActive] [bit] NULL,
	[EntryDateTime] [datetime] NOT NULL,
	[EntryUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Manufacturer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ManufacturerComodityTypes]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ManufacturerComodityTypes](
	[Id] [uniqueidentifier] NOT NULL,
	[ManufacturerId] [uniqueidentifier] NULL,
	[CommodityTypeId] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ManufacturerWarranty]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ManufacturerWarranty](
	[Id] [uniqueidentifier] NOT NULL,
	[WarrantyName] [nvarchar](200) NOT NULL,
	[ApplicableFrom] [datetime] NULL,
	[ApplicableTo] [datetime] NULL,
	[WarrantyMonths] [int] NULL,
	[WarrantyKm] [int] NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [nvarchar](200) NULL,
	[MakeId] [uniqueidentifier] NULL,
	[ModelId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ManufacturerWarranty] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[WarrantyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Model]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Model](
	[Id] [uniqueidentifier] NOT NULL,
	[ModelCode] [nvarchar](200) NULL,
	[ModelName] [nvarchar](500) NULL,
	[MakeId] [uniqueidentifier] NULL,
	[CategoryId] [uniqueidentifier] NULL,
	[RiskStartDate] [datetime] NULL,
	[WarantyGiven] [bit] NULL,
	[IsActive] [bit] NULL,
	[ContryOfOrigineId] [uniqueidentifier] NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Model] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Nationality]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Nationality](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NationalityName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Nationality_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Policy]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Policy](
	[Id] [uniqueidentifier] NOT NULL,
	[CommodityTypeId] [uniqueidentifier] NULL,
	[ProductId] [uniqueidentifier] NULL,
	[DealerId] [uniqueidentifier] NULL,
	[DealerLocationId] [uniqueidentifier] NULL,
	[ContractId] [uniqueidentifier] NULL,
	[ExtensionTypeId] [uniqueidentifier] NULL,
	[PremiumCurrencyTypeId] [uniqueidentifier] NULL,
	[CoverTypeId] [uniqueidentifier] NULL,
	[SalesPersonId] [uniqueidentifier] NULL,
	[DealerPaymentCurrencyTypeId] [uniqueidentifier] NULL,
	[CustomerPaymentCurrencyTypeId] [uniqueidentifier] NULL,
	[PaymentModeId] [uniqueidentifier] NULL,
	[CustomerId] [uniqueidentifier] NULL,
	[HrsUsedAtPolicySale] [nvarchar](200) NULL,
	[PolicyNo] [nvarchar](500) NULL,
	[RefNo] [nvarchar](200) NULL,
	[Comment] [nvarchar](200) NULL,
	[Premium] [numeric](18, 0) NULL,
	[DealerPayment] [numeric](18, 0) NULL,
	[CustomerPayment] [numeric](18, 0) NULL,
	[IsPreWarrantyCheck] [bit] NULL,
	[IsSpecialDeal] [bit] NULL,
	[IsPartialPayment] [bit] NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
	[PolicySoldDate] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PremiumAddonType]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PremiumAddonType](
	[Id] [uniqueidentifier] NOT NULL,
	[CommodityTypeId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](200) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PremiumAddonType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PremiumBasedOn]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PremiumBasedOn](
	[Id] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](200) NULL,
	[Description] [nvarchar](200) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PremiumBasedOn] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/13/2015 6:40:02 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [uniqueidentifier] NOT NULL,
	[ProductName] [varchar](50) NULL,
	[CommodityTypeId] [uniqueidentifier] NOT NULL,
	[ProductCode] [varchar](5) NULL,
	[ProductDescription] [varchar](max) NULL,
	[ProductShortDescription] [varchar](500) NULL,
	[DisplayImage] [uniqueidentifier] NULL,
	[IsBundledProduct] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsMandatoryProduct] [bit] NOT NULL,
	[EntryDatetime] [datetime] NOT NULL,
	[EntryUser] [uniqueidentifier] NOT NULL,
	[LastUpdateDatetime] [datetime] NULL,
	[LastUpdateUser] [uniqueidentifier] NULL,
	[ProductTypeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[Id] [uniqueidentifier] NOT NULL,
	[Category] [nvarchar](200) NULL,
	[ProductCategoryDescription] [nvarchar](500) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProductType]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductType](
	[Id] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](200) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Reinsurer]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reinsurer](
	[Id] [uniqueidentifier] NOT NULL,
	[ReinsurerCode] [nvarchar](100) NULL,
	[ReinsurerName] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ReinsurerContract]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReinsurerContract](
	[Id] [uniqueidentifier] NOT NULL,
	[ReinsurerId] [uniqueidentifier] NULL,
	[UWYear] [nvarchar](100) NULL,
	[CountryId] [uniqueidentifier] NULL,
	[CommodityTypeId] [uniqueidentifier] NULL,
	[InsurerId] [uniqueidentifier] NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[ContractNo] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemUser]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SystemUser](
	[Id] [varchar](50) NOT NULL,
	[SequanceNumber] [int] NOT NULL,
	[RecordVersion] [int] NOT NULL,
	[EntryDate] [datetime] NOT NULL,
	[EntryBy] [nvarchar](250) NOT NULL,
	[FirstName] [nvarchar](250) NULL,
	[LastName] [nvarchar](250) NULL,
	[DateOfBirth] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[Email] [nvarchar](150) NULL,
	[Password] [nvarchar](200) NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[CountryId] [uniqueidentifier] NULL,
	[MobileNo] [nvarchar](50) NULL,
	[OtherTelNo] [nvarchar](50) NULL,
	[InternalExtension] [nvarchar](50) NULL,
	[Gender] [char](1) NULL,
	[Address1] [nvarchar](500) NULL,
	[Address2] [nvarchar](500) NULL,
	[Address3] [nvarchar](500) NULL,
	[Address4] [nvarchar](500) NULL,
	[IDNo] [nvarchar](50) NULL,
	[IDTypeId] [int] NULL,
	[DLIssueDate] [datetime] NULL,
	[ProfilePicture] [image] NULL,
	[NationalityId] [int] NULL,
 CONSTRAINT [PK__SystemUs__3214EC0703317E3D] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Table]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Table](
	[Id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TPA]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TPA](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](500) NULL,
	[TelNumber] [nvarchar](50) NULL,
	[Address] [nvarchar](500) NULL,
	[DiscountDescription] [nvarchar](500) NULL,
	[Banner] [uniqueidentifier] NULL,
	[Logo] [uniqueidentifier] NULL,
 CONSTRAINT [PK_TPA] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TpaBranch]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TpaBranch](
	[Id] [uniqueidentifier] NOT NULL,
	[BranchName] [nvarchar](200) NULL,
	[BranchCode] [nvarchar](50) NULL,
	[ContryId] [uniqueidentifier] NULL,
	[CityId] [uniqueidentifier] NULL,
	[State] [nvarchar](200) NULL,
	[TimeZone] [bigint] NULL,
	[Address] [nvarchar](500) NULL,
	[Address1] [nvarchar](250) NULL,
	[Address2] [nvarchar](250) NULL,
	[Address3] [nvarchar](250) NULL,
	[Address4] [nvarchar](250) NULL,
	[IsHeadOffice] [bit] NULL,
	[TpaId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_TpaBranch] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TransmissionType]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransmissionType](
	[Id] [uniqueidentifier] NOT NULL,
	[TransmissionTypeCode] [nvarchar](200) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_TransmissionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UsageType]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UsageType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UsageTypeCode] [varchar](10) NOT NULL,
	[UsageTypeName] [varchar](50) NULL,
 CONSTRAINT [PK_UsageType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[RoleId] [uniqueidentifier] NOT NULL,
	[RoleCode] [nvarchar](50) NULL,
	[RoleName] [nvarchar](200) NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Variant]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Variant](
	[Id] [uniqueidentifier] NOT NULL,
	[ModelId] [uniqueidentifier] NULL,
	[CommodityTypeId] [uniqueidentifier] NULL,
	[VariantName] [nvarchar](50) NULL,
	[FromModelYear] [nvarchar](50) NULL,
	[ToModelYear] [nvarchar](50) NULL,
	[BodyCode] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
	[EngineCapacityId] [uniqueidentifier] NULL,
	[CylinderCountId] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VariantAspirations]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VariantAspirations](
	[Id] [uniqueidentifier] NOT NULL,
	[VariantId] [uniqueidentifier] NULL,
	[AspirationId] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VariantBodyTypes]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VariantBodyTypes](
	[Id] [uniqueidentifier] NOT NULL,
	[VariantId] [uniqueidentifier] NULL,
	[BodyTypeId] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VariantCountrys]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VariantCountrys](
	[Id] [uniqueidentifier] NOT NULL,
	[VariantId] [uniqueidentifier] NULL,
	[CountryId] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VariantDriveTypes]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VariantDriveTypes](
	[Id] [uniqueidentifier] NOT NULL,
	[VariantId] [uniqueidentifier] NULL,
	[DriveTypeId] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VariantFuelTypes]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VariantFuelTypes](
	[Id] [uniqueidentifier] NOT NULL,
	[VariantId] [uniqueidentifier] NULL,
	[FuelTypeId] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VariantTransmissions]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VariantTransmissions](
	[Id] [uniqueidentifier] NOT NULL,
	[VariantId] [uniqueidentifier] NULL,
	[TransmissionId] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleAspirationType]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleAspirationType](
	[Id] [uniqueidentifier] NOT NULL,
	[AspirationTypeCode] [nvarchar](200) NULL,
	[EntryUser] [uniqueidentifier] NULL,
	[EntryDateTime] [datetime] NULL,
 CONSTRAINT [PK_VehicleAspirationType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleBodyType]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleBodyType](
	[Id] [uniqueidentifier] NOT NULL,
	[VehicleBodyTypeCode] [nvarchar](200) NULL,
	[VehicleBodyTypeDescription] [nvarchar](300) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_VehicleBodyType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleColor]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VehicleColor](
	[Id] [uniqueidentifier] NOT NULL,
	[VehicleColorCode] [varchar](10) NULL,
	[Color] [varchar](30) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_VehicleColor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VehicleDetails]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleDetails](
	[Id] [uniqueidentifier] NOT NULL,
	[VINNo] [nvarchar](50) NULL,
	[MakeId] [uniqueidentifier] NULL,
	[ModelId] [uniqueidentifier] NULL,
	[CategoryId] [uniqueidentifier] NULL,
	[ItemStatusId] [uniqueidentifier] NULL,
	[CylinderCountId] [uniqueidentifier] NULL,
	[BodyTypeId] [uniqueidentifier] NULL,
	[PlateNo] [nvarchar](50) NULL,
	[ModelYear] [nvarchar](50) NULL,
	[FuelTypeId] [uniqueidentifier] NULL,
	[AspirationId] [uniqueidentifier] NULL,
	[TransmissionId] [uniqueidentifier] NULL,
	[ItemPurchasedDate] [datetime] NULL,
	[EngineCapacityId] [uniqueidentifier] NULL,
	[DriveTypeId] [uniqueidentifier] NULL,
	[VehiclePrice] [numeric](18, 0) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
	[Variant] [uniqueidentifier] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehiclePolicy]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehiclePolicy](
	[PolicyId] [uniqueidentifier] NOT NULL,
	[VehicleId] [uniqueidentifier] NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_VehiclePolicy] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleType]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleType](
	[Id] [uniqueidentifier] NOT NULL,
	[VehicleTypeCode] [nvarchar](200) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_VehicleType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WarrantyType]    Script Date: 11/13/2015 6:40:03 පෙ.ව ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WarrantyType](
	[Id] [uniqueidentifier] NOT NULL,
	[WarrantyTypeDescription] [nvarchar](500) NULL,
	[EntryDateTime] [datetime] NULL,
	[EntryUser] [nvarchar](200) NULL,
 CONSTRAINT [PK_WarrantyType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[BundledProduct] ADD  CONSTRAINT [DF_BundledProduct_IsCurrentProduct]  DEFAULT ((0)) FOR [IsCurrentProduct]
GO
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [DF_Customer_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_IsBundledProduct]  DEFAULT ((0)) FOR [IsBundledProduct]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_IsMandatoryProduct]  DEFAULT ((0)) FOR [IsMandatoryProduct]
GO
ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF__SystemUse__Recor__0519C6AF]  DEFAULT ((1)) FOR [RecordVersion]
GO
ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_UserStatus]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BundledProduct]  WITH CHECK ADD  CONSTRAINT [FK_BundledProduct_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[BundledProduct] CHECK CONSTRAINT [FK_BundledProduct_Product]
GO
ALTER TABLE [dbo].[BundledProduct]  WITH CHECK ADD  CONSTRAINT [FK_BundledProduct_Product1] FOREIGN KEY([ParentProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[BundledProduct] CHECK CONSTRAINT [FK_BundledProduct_Product1]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_CustomerType] FOREIGN KEY([CustomerTypeId])
REFERENCES [dbo].[CustomerType] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_CustomerType]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_IDType] FOREIGN KEY([IDTypeId])
REFERENCES [dbo].[IDType] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_IDType]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Nationality] FOREIGN KEY([NationalityId])
REFERENCES [dbo].[Nationality] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Nationality]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_SystemUser] FOREIGN KEY([EntryUserId])
REFERENCES [dbo].[SystemUser] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_SystemUser]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_UsageType] FOREIGN KEY([UsageTypeId])
REFERENCES [dbo].[UsageType] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_UsageType]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_CommodityType] FOREIGN KEY([CommodityTypeId])
REFERENCES [dbo].[CommodityType] ([CommodityTypeId])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_CommodityType]
GO
