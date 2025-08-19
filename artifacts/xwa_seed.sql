USE [XWA]
GO

INSERT INTO [xwaapi].[ProvisionTypes]
           ([Id]
           ,[Type]
           ,[Name]
           ,[Score])
     VALUES
           (NEWID()
           ,'Sensor'
           ,'Sensor Window'
           ,1)
GO

INSERT INTO [xwaapi].[ProvisionTypes]
           ([Id]
           ,[Type]
           ,[Name]
           ,[Score])
     VALUES
           (NEWID()
           ,'Droid'
           ,'Astromech Droid'
           ,1)
GO

INSERT INTO [xwaapi].[ProvisionTypes]
           ([Id]
           ,[Type]
           ,[Name]
           ,[Score])
     VALUES
           (NEWID()
           ,'Servo'
           ,'Servo Actuator'
           ,1)
GO

INSERT INTO [xwaapi].[ProvisionTypes]
           ([Id]
           ,[Type]
           ,[Name]
           ,[Score])
     VALUES
           (NEWID()
           ,'Power'
           ,'Power Generator'
           ,1)
GO

INSERT INTO [xwaapi].[ProvisionTypes]
           ([Id]
           ,[Type]
           ,[Name]
           ,[Score])
     VALUES
           (NEWID()
           ,'Shield'
           ,'Deflector Shield'
           ,1)
GO