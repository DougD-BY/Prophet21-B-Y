﻿using P21.Entity.Database;
using P21.Extensions.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P21.Entity.Services
{
    public class BusinessRuleService
    {
        private P21DbContext p21Db;

        public P21DbContext Db
        {
            get
            {
                if (p21Db == null)
                {
                    if (CurrentRule != null && CurrentRule.Session != null)
                    {
                        //TODO: consider using EntityConnectionStringBuilder if metadata information for model mapping is required.
                        SqlConnectionStringBuilder sqlConnection = new SqlConnectionStringBuilder()
                        {
                            PersistSecurityInfo = false,
                            IntegratedSecurity = true, // Trusted Connection (Windows Authentication)
                            DataSource = CurrentRule.Session.Server,
                            InitialCatalog = CurrentRule.Session.Database
                        };
                        p21Db = new P21DbContext(sqlConnection.ConnectionString);
                    }
                    else
                    {
                        if (Debugger.IsAttached)
                        {
                            p21Db = new P21DbContext("name=RemoteConnectionString");
                        }
                        else
                        {
                            p21Db = new P21DbContext("name=P21ConnectionString");
                        }
                    }
                }
                return p21Db;
            }
            set { p21Db = value; }
        }

        public WebBusinessRule CurrentRule { get; set; }

        public IEnumerable<business_rule> GetAllRules()
        {
            return Db.business_rule.Where(br => br.internal_rule_flag == "N").OrderBy(br => br.rule_name);
        }
    }
}
