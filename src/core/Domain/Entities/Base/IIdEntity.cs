namespace Domain.Entities.Base;

public interface IIdEntity<T>
{
	T Id { get; set; }
}