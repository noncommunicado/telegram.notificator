namespace Services;

public interface IFileService
{
	Task SaveFileAsync(Guid fileId, Stream fileStream, CancellationToken ct = default);
	(bool Exist, FileStream FileStream) GetFileStream(Guid fileId, CancellationToken ct = default);

	/// <summary>
	/// Removes file from disk if it exists
	/// </summary>
	/// <param name="fileId"></param>
	void Remove(Guid fileId);
}


public sealed class FileService : IFileService
{
	private readonly string _fileStoreDirectory;
	public FileService(string webRootPath)
	{
		_fileStoreDirectory = webRootPath;
	}
	public async Task SaveFileAsync(Guid fileId, Stream fileStream, CancellationToken ct = default)
	{
		var fileInfo = new FileInfo(Path.Combine(_fileStoreDirectory, fileId.ToString()));
		if (!fileInfo.Directory!.Exists) fileInfo.Directory.Create();
		using var fs = File.Create(fileInfo.FullName);
		fs.Position = 0;
		fs.Seek(0, SeekOrigin.Begin);
		fileStream.Position = 0;
		fileStream.Seek(0, SeekOrigin.Begin);
		await fileStream.CopyToAsync(fs, ct);
	}

	public (bool Exist, FileStream FileStream) GetFileStream(Guid fileId, CancellationToken ct = default)
	{
		var fileInfo = new FileInfo(Path.Combine(_fileStoreDirectory, fileId.ToString()));
		if (fileInfo.Exists is false) return (false, null);
		return (true, fileInfo.OpenRead());
	}

	public void Remove(Guid fileId)
	{
		var fileInfo = new FileInfo(Path.Combine(_fileStoreDirectory, fileId.ToString()));
		if (fileInfo.Exists)
			fileInfo.Delete();
	}
}