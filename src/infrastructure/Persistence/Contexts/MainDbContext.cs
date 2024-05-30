using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public sealed class MainDbContext : DbContext
{
	public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

	public DbSet<GroupEntity> Groups { get; set; }
	public DbSet<MessageEntity> Messages { get; set; }
	public DbSet<GroupMemberEntity> GroupMembers { get; set; }
	public DbSet<AttachmentEntity> Attachments { get; set; }

	protected override void OnModelCreating(ModelBuilder mb)
	{
		mb.Entity<GroupMemberEntity>()
			.HasKey(x => new {
				x.ChatId, x.GroupId, x.ThreadId
			});

		mb.Entity<GroupEntity>().HasIndex(x => x.SysCode).IsUnique();

		base.OnModelCreating(mb);
	}
}