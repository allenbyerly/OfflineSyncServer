using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Erp;

namespace OfflineServer
{
    class GoOnlineSyncController
    {

        public bool EstablishAxConnection(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.AX erp)
        {
            OfflineServer.ClientConnectionSettings connectionSettings = offlineServer.Message.ClientConnectionSettings;
            
            if (!ValidateCredentials(connectionSettings, erp)){ return false;}
            if (!ValidateUser(connectionSettings, erp)) { return false; }

            return true;
        }

        public bool PrepareData(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
             if (!PreparePublication(SyncController, offlineServer, erp)){ return false; }
             if (!PrepareSubscription(SyncController, offlineServer, erp)){ return false; }

            return true;
        }

        public bool GatherOfflineData(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            if (!FilterDataSet(SyncController, offlineServer, erp)) { return false; }
            return true;
        }

        public bool FinalizeDataSet(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            if (!FinalizePublication(SyncController, offlineServer, erp)) { return false; }
            if (!FinalizeSubscription(SyncController, offlineServer, erp)) { return false; }

            return true;
        }

        public bool ReplicateData(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            if (!MergeReplication(SyncController, offlineServer, erp)) { return false; }
            return true;
        }

        public bool GoOnline(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            if (!ChangeMode(SyncController, offlineServer, erp)) { return false; }

            return true;
        }

        public bool ValidateCredentials(OfflineServer.ClientConnectionSettings connectionSettings, Erp.AX erp)
        {
            if (!erp.Equals(null))
            {
                return true;
            }
            else
            {
                return false;
            }           
        }

        public bool ValidateUser(OfflineServer.ClientConnectionSettings connectionSettings, Erp.AX erp)
        {
            erp.newAxLogin();

            return erp.axLogin.ValidateLogin(connectionSettings.Username, connectionSettings.Password, connectionSettings.Warehouse, connectionSettings.AxInstance);
          //  return erp.axLogin.ValidateLogin("allenbyerly", "ab", "SEU", connectionSettings.AxInstance);

        }

        public bool PreparePublication(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            offlineServer.LoadServerSettings();
            return true;
        }

        public bool PrepareSubscription(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            return true;
        }

        public bool FilterDataSet(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
          
            return true;
        }

        public bool FinalizePublication(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {

            offlineServer.Message.replicationSettings.Hostname = offlineServer.Message.ClientConnectionSettings.Username;
            /*
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();

            builder.UserID = offlineServer.Message.replicationSettings.PublisherUser;
            builder.Password = offlineServer.Message.replicationSettings.PublisherPwd;
            builder.DataSource = offlineServer.Message.replicationSettings.Publisher;
            builder.IntegratedSecurity = false;
            builder.InitialCatalog = offlineServer.Message.replicationSettings.PublisherDB;
            builder.ConnectTimeout = 30;

            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(builder.ConnectionString);

            connection.Open();

            using (connection)
            {

                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

                System.Data.SqlClient.SqlParameter identityParam = new System.Data.SqlClient.SqlParameter("@Identity", System.Data.SqlDbType.NVarChar);

                identityParam.Value = offlineServer.Message.ClientConnectionSettings.Username;

                command.Parameters.Add(identityParam);

                command.CommandText = "SELECT drv.DRIVEREMPLID FROM SPNA_FSROUTETRANS drv, SYSCOMPANYUSERINFO emp, USERINFO us " +
                                        "WHERE drv.DRIVEREMPLID = emp.EMPLID " +
                                        "AND emp.USERID = us.ID " +
                                        "AND us.NETWORKALIAS = @Identity";

                command.Connection = connection;

                System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("DRIVEREMPLID")))
                    {
                        offlineServer.Message.replicationSettings.Hostname = reader[0].ToString();
                    }
                }

                reader.Close();

            }

            connection.Close();*/
            return true;
        }

        public bool FinalizeSubscription(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            return true;
        }

        public bool MergeReplication(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
          //  offlineServer.LoadServerSettings();
            return true;
        }

        public bool ChangeMode(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            offlineServer.Message.Status = OfflineServer.OnlineStatusEnum.ONLINE;
            offlineServer.Message.Action = OfflineServer.Actions.MERGE;
            return true;
        }

        public void applyUserFilter(string username)
        {
        }

        public void applyDeviceFilter(string username)
        {
        }

        public void applyDateFilter(string username)
        {
        }
    }
}
