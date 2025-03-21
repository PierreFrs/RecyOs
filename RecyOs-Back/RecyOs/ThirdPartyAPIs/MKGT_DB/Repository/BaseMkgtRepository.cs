using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RecyOs.Helpers;

namespace RecyOs.MKGT_DB.Repository;

public class BaseMkgtRepository : BaseSqlRepository
{
    public BaseMkgtRepository(IConfiguration Configuration)
    {
        _connectionString = Configuration.GetConnectionString("MkgtDatabase");
    }
    
    
}