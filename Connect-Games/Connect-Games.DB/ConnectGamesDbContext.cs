using Connect_Games.Games.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect_Games.DAL
{
    /// <summary>
    /// Controls access to the database.
    /// </summary>
    public class ConnectGamesDbContext : IdentityDbContext<Player>
    {
        public ConnectGamesDbContext(DbContextOptions<ConnectGamesDbContext> options) : base(options)
        {

        }

        public DbSet<Player> Players { get; set; }
        public DbSet<GameHistory> GameHistories { get; set; }
    }
}
