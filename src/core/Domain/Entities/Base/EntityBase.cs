using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Base;

public abstract class EntityBase : IIdEntity<Guid>, ICreatedDateTimeEntity
{
	public EntityBase() { }

	protected EntityBase(Guid id)
	{
		Id = id;
	}

	[Column(TypeName = "timestamp without time zone")]
	public DateTime? SysCreated { get; set; } = DateTime.Now;

	[Key] public Guid Id { get; set; }
}