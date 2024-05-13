namespace Domain.Exceptions;

public class DomainException : Exception
{
	public DomainException() { }

	public DomainException(int httpStatusCode)
	{
		HttpStatusCode = httpStatusCode;
	}

	public DomainException(int httpStatusCode, string message) : base(message)
	{
		HttpStatusCode = httpStatusCode;
	}

	public int HttpStatusCode { get; set; }

	public static void ThrowIfTrue(bool predicate, int httpStatusCode, string message = "")
	{
		if (predicate)
			throw new DomainException(httpStatusCode, message);
	}


	public static void ThrowIfTrue(Func<bool> predicate, int httpStatusCode, string message = "")
	{
		if (predicate.Invoke())
			throw new DomainException(httpStatusCode, message);
	}

	public static async Task ThrowIfTrue(Func<Task<bool>> predicate, int httpStatusCode, string message = "")
	{
		if (await predicate.Invoke())
			throw new DomainException(httpStatusCode, message);
	}
}