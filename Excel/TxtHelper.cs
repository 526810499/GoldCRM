using System.IO;
namespace XHD.Excel
{
    public class TxtHelper
	{
		public string read(string path)
		{
			FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
			StreamReader streamReader = new StreamReader(fileStream);
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			fileStream.Close();
			return result;
		}
		public void write(string path, string content)
		{
			FileStream fileStream;
			if (File.Exists(path))
			{
				fileStream = new FileStream(path, FileMode.Truncate);
			}
			else
			{
				fileStream = new FileStream(path, FileMode.CreateNew);
			}
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.Write(content);
			streamWriter.Close();
			fileStream.Close();
		}
		public void CreateFile(string fullName, byte[] content)
		{
			using (FileStream fileStream = new FileStream(fullName, FileMode.Create))
			{
				fileStream.Seek(0L, SeekOrigin.Begin);
				fileStream.Write(content, 0, content.Length);
			}
		}
	}
}
