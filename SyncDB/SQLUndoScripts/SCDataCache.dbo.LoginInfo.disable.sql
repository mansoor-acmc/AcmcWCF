IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[LoginInfo]')) 
   ALTER TABLE [dbo].[LoginInfo] 
   DISABLE  CHANGE_TRACKING
GO
