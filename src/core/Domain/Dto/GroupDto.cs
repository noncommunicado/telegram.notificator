
namespace Domain.Dto;

public record GroupDto
{
	public Guid Id { get; init; }
	public string Name { get; init; }
	public string Code { get; init; }
	public DateTime? SysCreated { get; init; }
}