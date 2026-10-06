using DBCommandSystem.Models;
using Microsoft.EntityFrameworkCore;
using NotificationGate.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCommandSystem.Data;
public class DataDbContext : DbContext
{
    public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
    {

    }
    public DbSet<AlertDB> alertDbs { get; set; } = null!;
}


