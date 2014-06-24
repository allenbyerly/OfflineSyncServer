using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Erp;


namespace OfflineServer
{
    class MobileClientSyncController
    {
       // public bool Online { get; set; }

        public bool GoOfflineSyncronization(OfflineServer offlineServer, Erp.AX erp)
        {
            GoOfflineSyncController syncController = new GoOfflineSyncController();
             
            syncController.EstablishAxConnection(this, offlineServer, erp);
            syncController.PrepareData(this, offlineServer, erp);
            syncController.GatherOfflineData(this, offlineServer, erp);
            syncController.FinalizeDataSet(this, offlineServer, erp);
            syncController.ReplicateData(this, offlineServer, erp);
            syncController.GoOffline(this, offlineServer, erp);

            return true;
        }

        public bool GoOnlineSyncronization(OfflineServer offlineServer, Erp.AX erp)
        {

            GoOnlineSyncController syncController = new GoOnlineSyncController();

            syncController.EstablishAxConnection(this, offlineServer, erp);
            syncController.PrepareData(this, offlineServer, erp);
            syncController.FinalizeDataSet(this, offlineServer, erp);
            syncController.ReplicateData(this, offlineServer, erp);
            syncController.GoOnline(this, offlineServer, erp);

            return true;
        }

        public bool TransactionSyncronization(OfflineServer offlineServer, Erp.AX erp)
        {

            TransactioneSyncController syncController = new TransactioneSyncController();

            syncController.EstablishAxConnection(this, offlineServer, erp);
            syncController.PrepareData(this, offlineServer, erp);
            syncController.FinalizeDataSet(this, offlineServer, erp);
            syncController.ReplicateData(this, offlineServer, erp);

            return true;
        }

        public bool AxSyncronization(OfflineServer offlineServer, Erp.AX erp)
        {

            AxSyncController syncController = new AxSyncController();

            syncController.EstablishAxConnection(this, offlineServer, erp);
            syncController.PrepareData(this, offlineServer, erp);
            syncController.FinalizeDataSet(this, offlineServer, erp);
            syncController.ReplicateData(this, offlineServer, erp);

            return true;
        }


       
    }

}
