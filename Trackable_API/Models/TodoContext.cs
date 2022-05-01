﻿using Microsoft.EntityFrameworkCore;

namespace Trackable_API.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {

        }

        public DbSet<TodoItem> TodoItems { get; set; } = null!;
    }
}