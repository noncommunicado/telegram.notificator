using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Base;

public abstract class EntityBase : IIdEntity, ICreatedDateTimeEntity
{
	public EntityBase() { }

	protected EntityBase(int id)
	{
		Id = id;
	}
	
	[Key]
	public int Id { get; set; }
	
	[Column(TypeName = "timestamp without time zone")]
	public  DateTime? SysCreated { get; set; } = DateTime.Now;
}