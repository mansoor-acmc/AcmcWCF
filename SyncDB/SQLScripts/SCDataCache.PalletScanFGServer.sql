IF NOT EXISTS (SELECT * FROM sys.change_tracking_databases WHERE database_id = DB_ID(N'PalletScanFGServer')) 
   ALTER DATABASE [PalletScanFGServer] 
   SET  CHANGE_TRACKING = ON
GO
