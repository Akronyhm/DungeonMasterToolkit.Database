-- AspNetRoles
CREATE TABLE [AspNetRoles] (
    [Id]              NVARCHAR(450) NOT NULL,
    [Name]            NVARCHAR(256) NULL,
    [NormalizedName]  NVARCHAR(256) NULL,
    [ConcurrencyStamp] NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

-- AspNetUsers
CREATE TABLE [AspNetUsers] (
    [Id]                   NVARCHAR(450) NOT NULL,
    [UserName]             NVARCHAR(256) NULL,
    [NormalizedUserName]   NVARCHAR(256) NULL,
    [Email]                NVARCHAR(256) NULL,
    [NormalizedEmail]      NVARCHAR(256) NULL,
    [EmailConfirmed]       BIT NOT NULL,
    [PasswordHash]         NVARCHAR(MAX) NULL,
    [SecurityStamp]        NVARCHAR(MAX) NULL,
    [ConcurrencyStamp]     NVARCHAR(MAX) NULL,
    [PhoneNumber]          NVARCHAR(MAX) NULL,
    [PhoneNumberConfirmed] BIT NOT NULL,
    [TwoFactorEnabled]     BIT NOT NULL,
    [LockoutEnd]           DATETIMEOFFSET NULL,
    [LockoutEnabled]       BIT NOT NULL,
    [AccessFailedCount]    INT NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

-- AspNetRoleClaims
CREATE TABLE [AspNetRoleClaims] (
    [Id]        INT IDENTITY(1,1) NOT NULL,
    [RoleId]    NVARCHAR(450) NOT NULL,
    [ClaimType] NVARCHAR(MAX) NULL,
    [ClaimValue] NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
        FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

-- AspNetUserClaims
CREATE TABLE [AspNetUserClaims] (
    [Id]        INT IDENTITY(1,1) NOT NULL,
    [UserId]    NVARCHAR(450) NOT NULL,
    [ClaimType] NVARCHAR(MAX) NULL,
    [ClaimValue] NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
        FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

-- AspNetUserLogins
CREATE TABLE [AspNetUserLogins] (
    [LoginProvider]     NVARCHAR(450) NOT NULL,
    [ProviderKey]       NVARCHAR(450) NOT NULL,
    [ProviderDisplayName] NVARCHAR(MAX) NULL,
    [UserId]            NVARCHAR(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
        FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

-- AspNetUserRoles
CREATE TABLE [AspNetUserRoles] (
    [UserId] NVARCHAR(450) NOT NULL,
    [RoleId] NVARCHAR(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
        FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
        FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

-- AspNetUserTokens
CREATE TABLE [AspNetUserTokens] (
    [UserId]        NVARCHAR(450) NOT NULL,
    [LoginProvider] NVARCHAR(450) NOT NULL,
    [Name]          NVARCHAR(450) NOT NULL,
    [Value]         NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
        FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

-- Indexes

CREATE INDEX [IX_AspNetRoleClaims_RoleId]
    ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex]
    ON [AspNetRoles] ([NormalizedName])
    WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId]
    ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId]
    ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId]
    ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex]
    ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex]
    ON [AspNetUsers] ([NormalizedUserName])
    WHERE [NormalizedUserName] IS NOT NULL;
GO
