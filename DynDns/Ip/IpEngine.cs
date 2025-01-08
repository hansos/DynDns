using Amazon.Route53;
using Amazon.Route53.Model;
using DynDns.Logging;
using System;
using System.Collections.Generic;

namespace DynDns.Ip
{
    internal class IpEngine
    {

        private readonly Log.TraceLevel _maxLevel;
        private  readonly bool _quiet;
        private Log _log;
        public IpEngine(Log.TraceLevel maxLevel, bool quiet, Log log)
        {
            _maxLevel = maxLevel;
            _quiet = quiet;
            _log = log;
        }


        internal bool ChangeResourceRecordSet(string hostedZoneId, string recName, string ip, string currentIp, bool sharp = true )
        {
            try
            {

                var settings = Infrastruct.SettingsTools.LoadSettings();
                using (var r53 = new AmazonRoute53Client(settings.AwsSettings.AccessKeyId, settings.AwsSettings.SecretAccessKey, Amazon.RegionEndpoint.USEast1))
                {
                    var recordSet = CreateResourceRecordSet(recName, ip, RRType.A);
                    _log.WriteTrace(Log.TraceLevel.Success, _maxLevel, "AWS.ChangeResourceRecordSet", $"Resource Record set defined for zone {hostedZoneId}: Record name = '{recName}', IP = '{ip}'", _quiet);
                    Change change1 = new Change
                    {
                        ResourceRecordSet = recordSet,
                        Action = ChangeAction.UPSERT
                    };

                    // Update the zone's resource record sets
                    ChangeResourceRecordSetsRequest recordsetRequest = new ChangeResourceRecordSetsRequest()
                    {
                        HostedZoneId = hostedZoneId,
                        ChangeBatch = new ChangeBatch
                        {
                            Changes = new List<Change> { change1 }
                        }
                    };
                    _log.WriteTrace(Log.TraceLevel.Success, _maxLevel, "AWS.ChangeResourceRecordSet", $"Sending to AWS...", _quiet);
                    _log.LogIpChange(_maxLevel, currentIp, ip, _quiet);
                    if (sharp)
                    {
                        var recordsetResponse = r53.ChangeResourceRecordSetsAsync(recordsetRequest);
                        string msg = $"DNS Zone Record updated: AWS returned response code '{recordsetResponse.Result.HttpStatusCode}' at {recordsetResponse.Result.ChangeInfo.SubmittedAt}";
                        _log.WriteTrace(Log.TraceLevel.Success, _maxLevel, "AWS.ChangeResourceRecordSet", msg, _quiet);
                    }
                    else
                    {
                        string msg = "Running in test modus. DNS Zone Record not updated.";
                        _log.WriteTrace(Log.TraceLevel.Success, _maxLevel, "AWS.ChangeResourceRecordSet", msg, _quiet);
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                _log.WriteTrace(Log.TraceLevel.Error,_maxLevel, "AWS.ChangeResourceRecordSet", $"Error Changing ResourceRecordSet: ", _quiet, ex);
                return false;
            }
        }

        private static ResourceRecordSet CreateResourceRecordSet(string recName, string ip, RRType type, long ttl = 60)
        {
            return new ResourceRecordSet()
            {
                Name = recName,
                TTL = ttl,
                Type = type,
                ResourceRecords = new List<ResourceRecord>
                {
                    new ResourceRecord
                    {
                        Value = ip,
                    }
                }
            };

        }


    }
}
