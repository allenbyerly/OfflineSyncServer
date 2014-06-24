using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Erp;
//using System.Data.SqlServerCe;
using System.Data.SqlClient;

namespace OfflineServer
{
    class GoOfflineSyncController
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

        public bool GoOffline(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
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
            return erp.axLogin.ValidateLogin(connectionSettings.Username, connectionSettings.Password, connectionSettings.Warehouse, connectionSettings.AxInstance);
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

            ApplyHostNameFilter(offlineServer);
            ApplyDeviceFilter(offlineServer);
            ApplyDateFilter(offlineServer);
           
           /* OfflineServer.DataSetFilter dataSetFilters = offlineServer.Message.DataSetFilter;
            string filter;
       
            foreach (string key in dataSetFilters.Filters.Keys)
            {
                filter = dataSetFilters.Filters[key];

                switch (key)
                {
                    case "username":
                        getHostNameFilter(filter, offlineServer);
                        break;
                    case "device":
                        applyDeviceFilter(filter, offlineServer);
                        break;
                    case "date":
                        applyDateFilter(filter, offlineServer);
                        break;
                }
            }*/
            return true;
        }

        public bool FinalizePublication(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            return true;
        }

        public bool FinalizeSubscription(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            return true;
        }

        public bool MergeReplication(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            return true;
        }

        public bool ChangeMode(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            offlineServer.Message.Status = OfflineServer.OnlineStatusEnum.OFFLINE;
            offlineServer.Message.Action = OfflineServer.Actions.MERGE;
            return true;
        }

        public void ApplyHostNameFilter(OfflineServer offlineServer)
        {

            offlineServer.Message.replicationSettings.Hostname = offlineServer.Message.ClientConnectionSettings.Username;

           /* SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.UserID = offlineServer.Message.replicationSettings.PublisherUser;
            builder.Password = offlineServer.Message.replicationSettings.PublisherPwd;
            builder.DataSource = offlineServer.Message.replicationSettings.Publisher;
            builder.IntegratedSecurity = false;
            builder.InitialCatalog = offlineServer.Message.replicationSettings.PublisherDB;
            builder.ConnectTimeout = 30;

            SqlConnection connection = new SqlConnection(builder.ConnectionString);

            connection.Open();

            using (connection)
            {

                SqlCommand command = new SqlCommand();

                SqlParameter identityParam = new SqlParameter("@Identity", System.Data.SqlDbType.NVarChar);
                
                identityParam.Value = offlineServer.Message.ClientConnectionSettings.Username;

                command.Parameters.Add(identityParam);
                
                command.CommandText = "SELECT drv.DRIVEREMPLID FROM SPNA_FSROUTETRANS drv, SYSCOMPANYUSERINFO emp, USERINFO us " +
                                        "WHERE drv.DRIVEREMPLID = emp.EMPLID " +
                                        "AND emp.USERID = us.ID " +
                                        "AND us.NETWORKALIAS = @Identity";

                command.Connection = connection;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("DRIVEREMPLID")))
                    {
                        offlineServer.Message.replicationSettings.Hostname = reader[0].ToString();
                    }
                }

                reader.Close();

            }

           connection.Close();
            */
        }

        public void ApplyDeviceFilter(OfflineServer offlineServer)
        {/*
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.UserID = offlineServer.Message.replicationSettings.PublisherUser;
            builder.Password = offlineServer.Message.replicationSettings.PublisherPwd;
            builder.DataSource = offlineServer.Message.replicationSettings.Publisher;
            builder.IntegratedSecurity = false;
            builder.InitialCatalog = offlineServer.Message.replicationSettings.PublisherDB;
            builder.ConnectTimeout = 30;

            SqlConnection connection = new SqlConnection(builder.ConnectionString);

            connection.Open();

            using (connection)
            {

                SqlCommand command = new SqlCommand();

                SqlParameter identityParam = new SqlParameter("@Identity", System.Data.SqlDbType.NVarChar);

                identityParam.Value = offlineServer.Message.ClientConnectionSettings.Username;

                command.Parameters.Add(identityParam);

                command.CommandText = "SELECT drv.DRIVEREMPLID FROM SPNA_FSROUTETRANS drv, SYSCOMPANYUSERINFO emp, USERINFO us " +
                                        "WHERE drv.DRIVEREMPLID = emp.EMPLID " +
                                        "AND emp.USERID = us.ID " +
                                        "AND us.NETWORKALIAS = @Identity";

                command.Connection = connection;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("DRIVEREMPLID")))
                    {
                        offlineServer.Message.replicationSettings.Hostname = reader[0].ToString();
                    }
                }

                reader.Close();

            }

            connection.Close();
            */
        }

        public void ApplyDateFilter(OfflineServer offlineServer)
        {
         /*   

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            
            builder.UserID = offlineServer.Message.replicationSettings.PublisherUser;
            builder.Password = offlineServer.Message.replicationSettings.PublisherPwd;
            builder.DataSource = offlineServer.Message.replicationSettings.Publisher;
            builder.IntegratedSecurity = false;
            builder.InitialCatalog = offlineServer.Message.replicationSettings.PublisherDB;
            builder.ConnectTimeout = 30;

            SqlConnection connection = new SqlConnection(builder.ConnectionString);

            connection.Open();

            using (connection)
            {

                string emplID = "";

                SqlCommand command = new SqlCommand();

                SqlParameter identityParam = new SqlParameter("@Identity", System.Data.SqlDbType.NVarChar);

                identityParam.Value = offlineServer.Message.ClientConnectionSettings.Username;

                command.Parameters.Add(identityParam);

                command.CommandText = "SELECT drv.DRIVEREMPLID FROM SPNA_FSROUTETRANS drv, SYSCOMPANYUSERINFO emp, USERINFO us " +
                                        "WHERE drv.DRIVEREMPLID = emp.EMPLID " +
                                        "AND emp.USERID = us.ID " +
                                        "AND us.NETWORKALIAS = @Identity";

                command.Connection = connection;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("DRIVEREMPLID")))
                    {
                        emplID = reader[0].ToString();
                    }
                }




                connection.Close();

                connection.Open();

                SqlParameter dateParam = new SqlParameter("@Date", System.Data.SqlDbType.DateTime); 
                SqlParameter endDateParam = new SqlParameter("@Days", System.Data.SqlDbType.DateTime);
                SqlParameter empParam = new SqlParameter("@EmpId", System.Data.SqlDbType.NVarChar);

                double days = double.Parse(offlineServer.Message.DataSetFilter1.Value);
                DateTime endDate = DateTime.Parse(offlineServer.Message.DataSetFilter2.Value);
                
                endDate = endDate.Date.AddDays(-days);
                
                dateParam.Value = offlineServer.Message.DataSetFilter2.Value;
                endDateParam.Value = endDate.ToShortDateString();
                empParam.Value = emplID;

                command.Parameters.Add(dateParam);
                command.Parameters.Add(endDateParam);
                command.Parameters.Add(empParam);



                command.CommandText =   " UPDATE SPNA_FSROUTETRANS " +
                                        " SET bit = 1 " +
                                        " WHERE dlvdate <= @Date " +
                                        " AND dlvdate > @Days " +
                                        " AND driveremplid = @EmpId " +
                                        " AND processed = 0 " +
                                        " AND deliverycomplete = 0 ";

                command.Connection = connection;


                command.ExecuteNonQuery();



                command.CommandText =   " UPDATE ITEM " +
                                        " SET ITEM.bit = 1 " +
                                        " FROM SPNA_FSROUTETRANSITEM ITEM " +
                                        " JOIN SPNA_FSROUTETRANS TRANS " +
                                        " ON ITEM.ROUTETRANSID = TRANS.ROUTETRANSID " +
                                        " AND TRANS.dlvdate <= @Date " +
                                        " AND TRANS.dlvdate > @Days " +
                                        " AND TRANS.driveremplid = @EmpId " +
                                        " AND TRANS.processed = 0 " +
                                        " AND TRANS.deliverycomplete = 0 ";

                command.Connection = connection;


                command.ExecuteNonQuery();
      
            }

            
            connection.Close();*/
        }
    }
}
