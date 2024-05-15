using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public sealed class MainDbContext : DbContext
{
	public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

	public DbSet<GroupEntity> Groups { get; set; }
	public DbSet<MessageEntity> Messages { get; set; }
	public DbSet<GroupMemberEntity> GroupMembers { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<GroupMemberEntity>()
			.HasKey(x => new {
				x.ChatId, x.GroupId, x.ThreadId
			});

		modelBuilder.Entity<GroupEntity>().HasIndex(x => x.SysCode).IsUnique();
		base.OnModelCreating(modelBuilder);
	}
}