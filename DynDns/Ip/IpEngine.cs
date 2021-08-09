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

        /// <summary>
        /// If detected address differs from stored address, the new IP address is returned.
        /// On error or unchanged value, null is returned.
        /// </summary>
        /// <returns>
        /// New IP address or null.
        /// </returns>
        //internal string CorrectPublicIp(Data.DynDnsData dynDnsData)
        //{
        //    var currentIp = DnsFinder.WhatismyipaddressCom(_maxLevel, _quiet);

        //    if(currentIp == null)
        //    {
        //        _log.WriteTrace(Log.TraceLevel.Success, _maxLevel, "IpEngine.CorrectPublicIp", $"Could not detect IP address.", _quiet);
        //        return null;
        //    }
        //    else if(currentIp != null && dynDnsData.CurrentIp.Equals(currentIp ))
        //    {
        //        return null;
        //    }
        //    else if (currentIp != null)
        //    {
        //        return currentIp;
        //    }

        //    //Log.WriteLine($"Reading data files from '{execDir}'.");

        //    return null;
        //}


        internal bool ChangeResourceRecordSet(string hostedZoneId, string recName, string ip, string currentIp, bool sharp = true )
        {
            try
            {

                using (var r53 = new AmazonRoute53Client())
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
