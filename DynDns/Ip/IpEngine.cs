using Amazon.Route53;
using Amazon.Route53.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns.Ip
{
    internal class IpEngine
    {

        /// <summary>
        /// If detected address differs from stored address, the new IP address is returned.
        /// On error or unchanged value, null is returned.
        /// </summary>
        /// <returns>
        /// New IP address or null.
        /// </returns>
        internal string CorrectPublicIp(Data.DynDnsData dynDnsData)
        {
            var currentIp = DnsFinder.WhatismyipaddressCom;

            if(currentIp == null)
            {
                Log.WriteTrace(Log.TraceLevel.Success, "IpEngine.CorrectPublicIp", $"Could not detect IP address.");
                return null;
            }
            else if(currentIp != null && dynDnsData.CurrentIp.Equals(currentIp ))
            {
                return null;
            }
            else if (currentIp != null)
            {
                return currentIp;
            }

            //Log.WriteLine($"Reading data files from '{execDir}'.");

            return null;
        }


        internal bool ChangeResourceRecordSet(string hostedZoneId, string recName, string ip, string currentIp)
        {
            try
            {

                using (var r53 = new AmazonRoute53Client())
                {
                    var recordSet = CreateResourceRecordSet(recName, ip, RRType.A);
                    Log.WriteTrace(Log.TraceLevel.Success, "AWS.ChangeResourceRecordSet", $"Resource Record set defined for zone {hostedZoneId}: Record name = '{recName}', IP = '{ip}'");
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
                    Log.WriteTrace(Log.TraceLevel.Success, "AWS.ChangeResourceRecordSet", $"Sending to AWS...");
                    Log.LogIpChange(currentIp,ip);
                    var recordsetResponse = r53.ChangeResourceRecordSetsAsync(recordsetRequest);
                    Log.WriteTrace(Log.TraceLevel.Success, "AWS.ChangeResourceRecordSet", $"Done! ({recordsetResponse.Result.HttpStatusCode}) {recordsetResponse.Result.ChangeInfo.SubmittedAt}");
                    return true;
                }

            }
            catch (Exception ex)
            {
                Log.WriteTrace(Log.TraceLevel.Error, "AWS.ChangeResourceRecordSet", $"Error Changing ResourceRecordSet: ", ex);
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
