using System;
using System.Collections.Generic;
using System.Text;

namespace Konference
{
    class Migrator
    {
        public string ProjectId { get; set; }
        public string SourceProjectId
        {
            get
            {
                return "db96e910-edf8-0094-b795-f3ce073c7ae0";
            }
        }
        public string ApiKey { get; set; }
        public string SourceApiKey
        {
            get
            {
                return "ew0KICAiYWxnIjogIkhTMjU2IiwNCiAgInR5cCI6ICJKV1QiDQp9.ew0KICAianRpIjogIjYwMGZmNDQxYjY3NDQzMWY4M2E1NjE3NGRjMTU0MmJmIiwNCiAgImlhdCI6ICIxNTc4OTIwMzk1IiwNCiAgImV4cCI6ICIxOTI0NTIwMzk1IiwNCiAgInByb2plY3RfaWQiOiAiZGI5NmU5MTBlZGY4MDA5NGI3OTVmM2NlMDczYzdhZTAiLA0KICAidmVyIjogIjIuMS4wIiwNCiAgInVpZCI6ICJOdGpncGVsaTlhUm1zZ2t1OGZYeUJGb3VBNTJiM1prWmJQcDYydWN2TkVJIiwNCiAgImF1ZCI6ICJtYW5hZ2Uua2VudGljb2Nsb3VkLmNvbSINCn0.odVRfVi0Nv4pbORSNfz6WBopZ2jx87FAGA1gzmhl3LE";
            }
        }

        public Migrator(string projectId, string apiKey)
        {
            ProjectId = projectId;
            ApiKey = apiKey;
        }
    }
}
