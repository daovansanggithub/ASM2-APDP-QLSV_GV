IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Courses] (
    [CourseId] int NOT NULL IDENTITY,
    [CourseName] nvarchar(max) NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([CourseId])
);
GO

CREATE TABLE [Students] (
    [StudentId] int NOT NULL IDENTITY,
    [StudentName] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [Phone] nvarchar(max) NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([StudentId])
);
GO

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [UserName] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NULL DEFAULT N'',
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);
GO

CREATE TABLE [Teachers] (
    [TeacherId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [Phone] nvarchar(max) NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Teachers] PRIMARY KEY ([TeacherId]),
    CONSTRAINT [FK_Teachers_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE TABLE [CourseAssignments] (
    [CourseAssignmentId] int NOT NULL IDENTITY,
    [TeacherId] int NOT NULL,
    [CourseId] int NOT NULL,
    [StudentId] int NOT NULL,
    CONSTRAINT [PK_CourseAssignments] PRIMARY KEY ([CourseAssignmentId]),
    CONSTRAINT [FK_CourseAssignments_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE CASCADE,
    CONSTRAINT [FK_CourseAssignments_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE CASCADE,
    CONSTRAINT [FK_CourseAssignments_Teachers_TeacherId] FOREIGN KEY ([TeacherId]) REFERENCES [Teachers] ([TeacherId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_CourseAssignments_CourseId] ON [CourseAssignments] ([CourseId]);
GO

CREATE INDEX [IX_CourseAssignments_StudentId] ON [CourseAssignments] ([StudentId]);
GO

CREATE INDEX [IX_CourseAssignments_TeacherId] ON [CourseAssignments] ([TeacherId]);
GO

CREATE UNIQUE INDEX [IX_Teachers_UserId] ON [Teachers] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240811102953_V0', N'8.0.7');
GO

COMMIT;
GO

