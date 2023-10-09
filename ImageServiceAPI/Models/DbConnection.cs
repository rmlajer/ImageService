using System;
using System.Configuration;
public class DbConnection
{
    public string? connectionString = Environment.GetEnvironmentVariable("connectionstring");

}


