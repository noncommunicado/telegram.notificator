using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IMainDbContext : IDbContextBase
{
	public DbSet<GroupEntity> Groups { get; set; }
	public DbSet<MessageEntity> Messages { get; set; }
	public DbSet<GroupMemberEntity> GroupMembers { get; set; }
}