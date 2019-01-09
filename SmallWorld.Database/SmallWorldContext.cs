using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Updates;
using SmallWorld.Library;

namespace SmallWorld.Database
{
    public class SmallWorldContext : DbContext
    {
        public const string File = "smallworld.sqlite";

        public DbSet<Account> Accounts { get; set; }
        public DbSet<World> Worlds { get; set; }

        public DbSet<Pairing> Pairings { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Pair> Pairs { get; set; }

        public DbSet<Email> Emails { get; set; }

        public DbSet<MfroMigration> MfroMigrations { get; set; }

        public SmallWorldContext(DbContextOptions<SmallWorldContext> options) : base(MakeOptions(options)) { }

        public void Migrate()
        {
            Database.Migrate();

            var updates = from type in typeof(IUpdateSet).FindTypes()
                          let set = (IUpdateSet)Activator.CreateInstance(type.AsType())
                          from update in set.Updates()
                          select update;

            foreach (var update in updates)
            {
                var applied = MfroMigrations.SingleOrDefault(m => m.Identifier == update.Identifier);
                if (applied != null) continue;

                update.Apply(this);

                MfroMigrations.Add(new MfroMigration {
                    Identifier = update.Identifier
                });

                SaveChanges();
            }
        }

        private static DbContextOptions MakeOptions(DbContextOptions<SmallWorldContext> options)
        {
            return new DbContextOptionsBuilder<SmallWorldContext>(options)
                .Options;
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<World>()
                .HasOne(w => w.PairingSettings)
                .WithOne(p => p.World)
                .HasForeignKey<PairingSettings>(p => p.Id);

            model.Entity<Member>()
                .HasMany(u => u.Pairs1)
                .WithOne(p => p.Initiator);
            model.Entity<Member>()
                .HasMany(u => u.Pairs2)
                .WithOne(p => p.Receiver);
        }
    }
}