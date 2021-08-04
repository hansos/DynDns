using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Route53;
using Amazon.Route53.Model;

namespace DynDns
{
    [Obsolete("Moved to Ip.IpEngine",true)]
    internal static class AWS
    {

        internal static async Task<HostedZone> ZoneByIdAsync(string hostedZoneId)
        {
            using (var r53 = new Amazon.Route53.AmazonRoute53Client())
            {
                var response = await r53.GetHostedZoneAsync(new GetHostedZoneRequest
                {
                    Id = hostedZoneId
                });
                DelegationSet delegationSet = response.DelegationSet;
                HostedZone hostedZone = response.HostedZone;
                return hostedZone;
            }
        }

        internal static async Task<bool> ZoneByIdExistsAsync(string hostedZoneId)
        {
            try
            {
                using (var r53 = new Amazon.Route53.AmazonRoute53Client())
                {
                    var response = await r53.GetHostedZoneAsync(new GetHostedZoneRequest
                    {
                        Id = hostedZoneId
                    });
                    if (response.HostedZone != null)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception("Record not found.");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal static async Task<bool> RecordSetExists(string hostedZoneId, string zoneName)
        {
            using (var r53 = new Amazon.Route53.AmazonRoute53Client())
            {
                var listResp = await r53.ListResourceRecordSetsAsync(new Amazon.Route53.Model.ListResourceRecordSetsRequest
                {
                    HostedZoneId = hostedZoneId,
                });
                
                foreach (var rec in listResp.ResourceRecordSets) 
                { 
                    if (rec.Name.ToLower().Equals(zoneName.ToLower().Trim()))
                    {
                        return true;
                    }
                }

                return false;
            };
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

        internal static async Task<bool> ChangeResourceRecordSet(string hostedZoneId, string zoneName, string recName, string ip)
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
                    var recordsetResponse = await r53.ChangeResourceRecordSetsAsync(recordsetRequest);
                    Log.WriteTrace(Log.TraceLevel.Success, "AWS.ChangeResourceRecordSet", $"Done!");
                    return true;
                }

            }
            catch (Exception ex)
            {
                Log.WriteTrace(Log.TraceLevel.Error, "AWS.ChangeResourceRecordSet", $"Error Changing ResourceRecordSet: ", ex);
                return false;
            }
        }
    
    }
}
