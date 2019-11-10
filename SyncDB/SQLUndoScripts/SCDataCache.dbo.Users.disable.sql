IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[Users]')) 
   ALTER TABLE [dbo].[Users] 
   DISABLE  CHANGE_TRACKING
GO
