using NewRelic.Platform.Sdk;
using NLog;
using System;
using System.Collections.Generic;

namespace MSMQNewRelicPlugin
{
    class MSMQAgent : Agent
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        public override string Guid { get { return "com.truemark.newrelic.msmq"; } }

        public override string Version { get { return "1.0.0"; } }

        private String host, domain, username, password, agentName;

        public MSMQAgent(String agentName, String host, String domain, String username, String password)
        {
            this.agentName = agentName;
            this.host = host;
            this.domain = domain;
            this.username = username;
            this.password = password;
        }

        public override string GetAgentName()
        {
            return this.agentName;
        }

        public override void PollCycle()
        {
            try
            {
                MSMQConnectSession connection = MSMQConnectionUtil.connect(this.host, this.domain, this.username, this.password);
                MSMQServiceMeteric serviceMetric = connection.getMSMQServiceMetric();
                List<MSMQMeteric> msmqMetric = connection.getMSMQMetric();
                logger.Debug("---------------------------------------------------------- Reporting meteric" + GetAgentName());
                ReportMetric("ALL/" + MSMQServiceMeteric.incomingMessagesPerSecLabel, "messages/second", Int64.Parse(serviceMetric.IncomingMessagesPerSec.ToString()));
                ReportMetric("ALL/" + MSMQServiceMeteric.incomingMultiCastSessionsLabel, "sessions", Int64.Parse(serviceMetric.IncomingMultiCastSessions.ToString()));
                ReportMetric("ALL/" + MSMQServiceMeteric.ipSessionsLabel, "sessions", Int64.Parse(serviceMetric.IpSessions.ToString()));
                ReportMetric("ALL/" + MSMQServiceMeteric.incomingMessagesCountLabel, "messages", Int64.Parse(serviceMetric.IncomingMessagesCount.ToString()));
                ReportMetric("ALL/" + MSMQServiceMeteric.outgoingMessagesCountLabel, "messages", Int64.Parse(serviceMetric.OutgoingMessagesCount.ToString()));
                ReportMetric("ALL/" + MSMQServiceMeteric.outgoingHttpSessionsLabel, "sessions", Int64.Parse(serviceMetric.OutgoingHttpSessions.ToString()));
                ReportMetric("ALL/" + MSMQServiceMeteric.outgoingMessagesPerSecLabel, "messages/second", Int64.Parse(serviceMetric.OutgoingMessagesPerSec.ToString()));
                ReportMetric("ALL/" + MSMQServiceMeteric.outgoingMulticastSessionsLabel, "sessions", Int64.Parse(serviceMetric.OutgoingMulticastSessions.ToString()));
                ReportMetric("ALL/" + MSMQServiceMeteric.sessionsLabel, "sessions", Int64.Parse(serviceMetric.Sessions.ToString()));
                ReportMetric("ALL/" + MSMQServiceMeteric.totalBytesInAllQueuesLabel, "bytes", Int64.Parse(serviceMetric.TotalBytesInAllQueues.ToString()));
                ReportMetric("ALL/" + MSMQServiceMeteric.totalMessagesInAllqueuesLabel, "messages", Int64.Parse(serviceMetric.TotalMessagesInAllQueues.ToString()));

                logger.Debug(MSMQServiceMeteric.incomingMessagesPerSecLabel + "Messages /Second " + int.Parse(serviceMetric.IncomingMessagesPerSec.ToString()));
                logger.Debug(MSMQServiceMeteric.incomingMultiCastSessionsLabel + "Session " + int.Parse(serviceMetric.IncomingMultiCastSessions.ToString()));
                logger.Debug(MSMQServiceMeteric.ipSessionsLabel + "Session " + int.Parse(serviceMetric.IpSessions.ToString()));
                logger.Debug(MSMQServiceMeteric.incomingMessagesCountLabel + "Message " + int.Parse(serviceMetric.IncomingMessagesCount.ToString()));
                logger.Debug(MSMQServiceMeteric.outgoingMessagesCountLabel + "Message " + int.Parse(serviceMetric.OutgoingMessagesCount.ToString()));
                logger.Debug(MSMQServiceMeteric.outgoingHttpSessionsLabel + "Session " + int.Parse(serviceMetric.OutgoingHttpSessions.ToString()));
                logger.Debug(MSMQServiceMeteric.outgoingMessagesPerSecLabel + "Messages/Second " + int.Parse(serviceMetric.OutgoingMessagesPerSec.ToString()));
                logger.Debug(MSMQServiceMeteric.outgoingMulticastSessionsLabel + "Session " + int.Parse(serviceMetric.OutgoingMulticastSessions.ToString()));
                logger.Debug(MSMQServiceMeteric.sessionsLabel + "Session " + int.Parse(serviceMetric.Sessions.ToString()));
                logger.Debug(MSMQServiceMeteric.totalBytesInAllQueuesLabel + "Bytes " + int.Parse(serviceMetric.TotalBytesInAllQueues.ToString()));
                logger.Debug(MSMQServiceMeteric.totalMessagesInAllqueuesLabel + "Message " + int.Parse(serviceMetric.TotalMessagesInAllQueues.ToString()));

                foreach (MSMQMeteric m in msmqMetric)
                {
                    logger.Debug("#############################: " + m.Name);
                    ReportMetric(m.Name + "/" + MSMQMeteric.bytesInJournalQueueLabel, "bytes", Int64.Parse(m.BytesInJournalQueue.ToString()));
                    ReportMetric(m.Name + "/" + MSMQMeteric.bytesInQueueLabel, "bytes", Int64.Parse(m.BytesInQueue.ToString()));
                    ReportMetric(m.Name + "/" + MSMQMeteric.messagesInJournalQueueLabel, "messages", Int64.Parse(m.MessagesInJournalQueue.ToString()));
                    ReportMetric(m.Name + "/" + MSMQMeteric.messagesInQueueLabel, "messages", Int64.Parse(m.MessagesInQueue.ToString()));
                    logger.Debug(MSMQMeteric.bytesInJournalQueueLabel + "Bytes  " + int.Parse(m.BytesInJournalQueue.ToString()));
                    logger.Debug(MSMQMeteric.bytesInQueueLabel + "Bytes  " + int.Parse(m.BytesInQueue.ToString()));
                    logger.Debug(MSMQMeteric.messagesInJournalQueueLabel + "Messages " + int.Parse(m.MessagesInJournalQueue.ToString()));
                    logger.Debug(MSMQMeteric.messagesInQueueLabel + "Messages  " + int.Parse(m.MessagesInQueue.ToString()));

                }
            }
            catch (Exception e)
            {
                logger.Error("----------------------------------------------------------" + e);
            }
        }
    }
}
