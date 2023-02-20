
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Base;

public interface IIdEntity
{
	int Id { get; set; }
}