﻿using Microsoft.EntityFrameworkCore;

namespace test.Services.Api;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
}
