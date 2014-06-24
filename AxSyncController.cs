using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Erp;

namespace OfflineServer
{
    class AxSyncController
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
            return true;
        }

        public bool PrepareSubscription(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            return true;
        }

        public bool FilterDataSet(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
          /*  OfflineServer.DataSetFilter dataSetFilters = offlineServer.Message.DataSetFilter;
            string filter;
       
            foreach (string key in dataSetFilters.Filters.Keys)
            {
                filter = dataSetFilters.Filters[key];

                switch (key)
                {
                    case "username":
                        applyUserFilter(filter);
                        break;
                    case "device":
                        applyDeviceFilter(filter);
                        break;
                    case "date":
                        applyDateFilter(filter);
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
            offlineServer.LoadServerSettings();
            return true;
        }

        public bool ChangeMode(MobileClientSyncController SyncController, OfflineServer offlineServer, Erp.IEnterpriseResourcePlanning erp)
        {
            offlineServer.Message.Status = OfflineServer.OnlineStatusEnum.OFFLINE;
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
