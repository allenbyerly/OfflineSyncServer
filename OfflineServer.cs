using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
using System.Text;
using Erp;

namespace OfflineServer
{
        public class OfflineServer
        {
            public enum Actions { NONE, LOAD, SAVE, DEFAULT, GOOFFLINE, GOONLINE, MERGE, FAIL }
            public enum OnlineStatusEnum  { ONLINE, UNLINKED, OFFLINE, SYNCREADY, NONE }
            public ClientMessage Message;
    //        public ReplicationSettings replicationSettings;
    //        public ClientConnectionSettings clientConnectionSettings;

            private string Path { set; get; }
            private string Assembly { set; get; }
            private string Directory { set; get; }
            private string Filename { set; get; }

            private XmlSerializer serializer;
            private XmlSerializer msgSerializer;
            private TextReader reader;
            private TextWriter writer;

            public OfflineServer()
            {
                try
                {
                    this.Message = new ClientMessage();
                    this.Message.replicationSettings = new ReplicationSettings();
                    //this.Assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                    string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    this.Directory = System.IO.Path.GetDirectoryName(location);
                    this.Filename = "ReplicationSettings.xml";
                    this.Path = Directory + "\\" + Filename;
                    this.serializer = new XmlSerializer(typeof(ReplicationSettings));
                    this.msgSerializer = new XmlSerializer(typeof(ClientMessage));
                    LoadServerSettings();

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            public string Process(string request, Erp.AX erp)
            {
                string response = "";

                DeserializeMessage(request);
                
                switch (Message.Action)
                {
                    case Actions.GOONLINE:
                        GoOnline(erp);
                        break;
                    case Actions.GOOFFLINE:
                        GoOffline(erp);
                        break;
                    default:
                        break;
                }

                response = SerializeMessage();

                return response;

            }

            public void DeserializeMessage(string request)
            {
                try
                {
                    reader = new StringReader(request);
                    Message = (ClientMessage)msgSerializer.Deserialize(reader);
                    reader.Dispose();
                }
                catch (Exception e)
                {
                    reader.Dispose();
                    ResetServerSettings();
                }

            }

            public string SerializeMessage()
            {
                string response = "";

                try
                {
                    writer = new StringWriter();
                    msgSerializer.Serialize(writer, Message);
                    response = writer.ToString();
                    writer.Dispose();

                    return response;
                }
                catch (Exception)
                {
                    writer.Dispose();
                    return response;
                }
            }

            public void GoOffline(Erp.AX erp)
            {
                if (Message.Status.Equals(OnlineStatusEnum.ONLINE))
                {
                    MobileClientSyncController syncController = new MobileClientSyncController();
                    syncController.GoOfflineSyncronization(this, erp);
                }
                else
                {
                    Message.Action = Actions.FAIL;
                    Message.Error = "The Client Must Be Online in order to Go Offline.";
                }
            }

            public void GoOnline(Erp.AX erp)
            {
                try
                {
                    if (Message.Status.Equals(OnlineStatusEnum.SYNCREADY) || Message.Status.Equals(OnlineStatusEnum.OFFLINE))
                    {
                        MobileClientSyncController syncController = new MobileClientSyncController();
                        syncController.GoOnlineSyncronization(this, erp);
                    }
                    else
                    {
                        Message.Action = Actions.FAIL;
                        Message.Error = "The Client Must Be Sync Ready in order to Go Online.";
                    }
                }
                catch (Exception e)
                {
                    Message.Action = Actions.FAIL;
                    Message.Error = e.Message;
                }
            }


            public void LoadServerSettings()
            {

                if (File.Exists(Path))
                {
                    DeserializeServer();
                }
                else
                {
                    ResetServerSettings();
                }
            }

            public string LoadServerSettings(string repSettings)
            {
                /*
                if (File.Exists(Path))
                {
                    DeserializeServer();
                }
                else
                {
                    ResetServerSettings();
                }

                server.Action = Actions.LOAD.ToString();*/
                string response = SerializeServer(repSettings);

                return response;
            }

            public void ResetServerSettings()
            {
//                writer = new StreamWriter(Path);
         //       writer.Write(global::OfflineServer.Resources.DefaultSettings);
//                writer.Dispose();
                SerializeServer();
                DeserializeServer();
            }

            public string ResetServerSettings(string repSettings)
            {
                writer = new StreamWriter(Path);
        //        writer.Write(global::Listener.Properties.Resources.DefaultSettings);
                writer.Dispose();
                DeserializeServer();

                Message.replicationSettings.Action = Actions.DEFAULT;
                string response = SerializeServer(repSettings);

                return response;
            }

            public void Save()
            {
                SerializeServer();
            }

            public string Save(string repSettings)
            {
                DeserializeServer(repSettings);
                SerializeServer();

                Message.replicationSettings.Action = Actions.NONE;
                string response = SerializeServer(repSettings);

                return response;
            }

            public void SerializeServer()
            {
                try
                {
                    writer = new StreamWriter(Path);
                    serializer.Serialize(writer, Message.replicationSettings);
                    writer.Dispose();
                }
                catch (Exception e)
                {
                    writer.Dispose();
                }
            }
            public string SerializeServer(string repSettings)
            {
                try
                {
                    writer = new StringWriter();
                    serializer.Serialize(writer, Message.replicationSettings);
                    repSettings = writer.ToString();
                    writer.Dispose();

                    return repSettings;
                }
                catch (Exception)
                {
                    writer.Dispose();
                    return repSettings;
                }
            }
            public void DeserializeServer()
            {
                try
                {
                    reader = new StreamReader(Path);
                    Message.replicationSettings = (ReplicationSettings)serializer.Deserialize(reader);
                    reader.Dispose();

                    //if (server.InternetURL.Equals(""))
                    //{
                    //    ResetServerSettings();
                    //}
                }
                catch (Exception)
                {
                    reader.Dispose();
                    ResetServerSettings();
                }

            }

            public string DeserializeServer(string repSettings)
            {
                try
                {
                    reader = new StringReader(repSettings);
                    Message.replicationSettings = (ReplicationSettings)serializer.Deserialize(reader);
                    reader.Dispose();
                    return repSettings;
                }
                catch (Exception)
                {
                    reader.Dispose();
                    ResetServerSettings();

                    return repSettings;
                    // MessageBox.Show(e.Message, "XML Read Error: Client Settings Have Been Reset");
                }

            }

            public class ClientMessage
            {
                public Actions Action { get; set; }
                public OnlineStatusEnum Status { get; set; }
                public String Error { get; set; }
                public ClientConnectionSettings ClientConnectionSettings { get; set; }
                public List<DataSetFilter> DataSetFilters { get; set; }
                public DataSetFilter DataSetFilter1 { get; set; }
                public DataSetFilter DataSetFilter2 { get; set; }
                public ReplicationSettings replicationSettings { get; set; }

                public ClientMessage()
                {
                    Action = Actions.NONE;
                    Status = OnlineStatusEnum.NONE;
                    Error = "";
                }
            }

            public class ReplicationSettings
            {
                public Actions Action { get; set; }
                public string Hostname { get; set; }
                public string InternetURL { get; set; }
                public string InternetUser { get; set; }
                public string InternetPwd { get; set; }
                public string Publisher { get; set; }
                public string PublisherDB { get; set; }
                public string PublisherUser { get; set; }
                public string PublisherPwd { get; set; }
                public string PublicationDynAx { get; set; }
                public string PublicationSwx { get; set; }
                public string PublicationTrans { get; set; }
                public string Subscriber { get; set; }
                public string ConnectString { get; set; }

                public ReplicationSettings()
                {
                    Action = Actions.NONE;
                    Hostname = "";
                    InternetURL = "";
                    InternetUser = "";
                    InternetPwd = "";
                    Publisher = "";
                    PublisherDB = "";
                    PublisherUser = "";
                    PublisherPwd = "";
                    PublicationDynAx = "";
                    PublicationSwx = "";
                    PublicationTrans = "";
                    Subscriber = "";
                    ConnectString = "";
                }


            }

            public class ClientConnectionSettings
            {
                public string Action { get; set; }
                public string InternetURL { get; set; }
                public string InternetUser { get; set; }
                public string InternetPwd { get; set; }
                public string AxInstance { get; set; }
                public string Username { get; set; }
                public string Password { get; set; }
                public string Warehouse { get; set; }
                public string DeviceId { get; set; }
                public string SessionKey { get; set; }
                public string SessionStatus { get; set; }
                public string SyncKey { get; set; }
                public string SyncStatus { get; set; }
               
                public ClientConnectionSettings()
                {
                    Action = "";
                    InternetURL = "";
                    InternetUser = "";
                    InternetPwd = "";
                    AxInstance = "";
                    Username = "";
                    Password = "";
                    DeviceId = "";
                    SessionKey = "";
                    SessionStatus = "";
                    SyncKey = "";
                    SyncStatus = "";
                }
            }

            public class DataSetFilter
            {
                public string Name { get; set; }
                public string Value { get; set; }

                public DataSetFilter()
                {
                    Name = "";
                    Value = "";
                }
            }

        }


}
