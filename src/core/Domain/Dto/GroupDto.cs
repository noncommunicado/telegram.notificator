
namespace Domain.Dto;

public record GroupDto
{
	public int Id { get; init; }
	public string Name { get; init; }
	public string Code { get; init; }
	public DateTime? SysCreated { get; init; }
}