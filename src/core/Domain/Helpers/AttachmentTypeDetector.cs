
using System.Globalization;
using Domain.Entities;

namespace Domain.Helpers;

public sealed class AttachmentTypeDetector
{
	public static AttachmentType Detect(string fileName) =>
		Path.GetExtension(fileName).ToLower(CultureInfo.InvariantCulture) switch {
			".png" => AttachmentType.Photo,
			".jpg" => AttachmentType.Photo,
			".jpeg" => AttachmentType.Photo,
			".gif" => AttachmentType.Photo,
			".heic" => AttachmentType.Photo,
			".tiff" => AttachmentType.Photo,
			".webp" => AttachmentType.Photo,
			".svg" => AttachmentType.Photo,
			_ => AttachmentType.File
		};
}