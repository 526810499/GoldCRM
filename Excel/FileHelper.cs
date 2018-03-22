using System;
using System.Collections.Generic;
using System.IO;
namespace XHD.Excel
{
	public static class FileHelper
	{
		private static List<string> fileInfoList = new List<string>();
		public static List<string> FileInfoList
		{
			get
			{
				return FileHelper.fileInfoList;
			}
			set
			{
				FileHelper.fileInfoList = value;
			}
		}
		public static void InitFileInfoList()
		{
			FileHelper.fileInfoList = new List<string>();
		}
		public static List<string> GetFileInfo(string searchKey, string path, bool isVague, bool isIgnoreCase)
		{
			FileSystemInfo[] fileSystemInfos;
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				fileSystemInfos = directoryInfo.GetFileSystemInfos();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			FileSystemInfo[] array = fileSystemInfos;
			for (int i = 0; i < array.Length; i++)
			{
				FileSystemInfo fileSystemInfo = array[i];
				if (!Directory.Exists(fileSystemInfo.FullName))
				{
					FileInfo fileInfo = new FileInfo(fileSystemInfo.FullName);
					StringComparison comparisonType;
					if (isIgnoreCase)
					{
						comparisonType = StringComparison.OrdinalIgnoreCase;
					}
					else
					{
						comparisonType = StringComparison.Ordinal;
					}
					if (isVague)
					{
						if (fileInfo.Name.IndexOf(searchKey, comparisonType) != -1)
						{
							FileHelper.fileInfoList.Add(fileInfo.FullName);
						}
					}
					else if (string.Equals(fileInfo.Name, searchKey, comparisonType))
					{
						FileHelper.fileInfoList.Add(fileInfo.FullName);
					}
				}
				else
				{
					FileHelper.GetFileInfo(searchKey, fileSystemInfo.FullName, isVague, isIgnoreCase);
				}
			}
			return FileHelper.fileInfoList;
		}
		public static List<string> GetFileInfo(string searchKey, string path, bool isVague, bool isIgnoreCase, string account)
		{
			FileSystemInfo[] array = null;
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				if (FileRight.GetRightByAccount(path, account).IndexOf("Read") != -1 || FileRight.GetRightByAccount(path, account).IndexOf("Full") != -1)
				{
					array = directoryInfo.GetFileSystemInfos();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			if (array != null)
			{
				FileSystemInfo[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					FileSystemInfo fileSystemInfo = array2[i];
					if (!Directory.Exists(fileSystemInfo.FullName))
					{
						FileInfo fileInfo = new FileInfo(fileSystemInfo.FullName);
						StringComparison comparisonType;
						if (isIgnoreCase)
						{
							comparisonType = StringComparison.OrdinalIgnoreCase;
						}
						else
						{
							comparisonType = StringComparison.Ordinal;
						}
						if (isVague)
						{
							if (fileInfo.Name.IndexOf(searchKey, comparisonType) != -1)
							{
								FileHelper.fileInfoList.Add(fileInfo.FullName);
							}
						}
						else if (string.Equals(fileInfo.Name, searchKey, comparisonType))
						{
							FileHelper.fileInfoList.Add(fileInfo.FullName);
						}
					}
					else
					{
						FileHelper.GetFileInfo(searchKey, fileSystemInfo.FullName, isVague, isIgnoreCase, account);
					}
				}
			}
			return FileHelper.fileInfoList;
		}
		public static List<string> GetFileInfoAtCurrentDirectory(string path)
		{
			List<string> list = new List<string>();
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
			FileSystemInfo[] array = fileSystemInfos;
			for (int i = 0; i < array.Length; i++)
			{
				FileSystemInfo fileSystemInfo = array[i];
				list.Add(fileSystemInfo.FullName);
			}
			return list;
		}
		public static List<string> GetFileInfoByDirectory(string path)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
			FileSystemInfo[] array = fileSystemInfos;
			for (int i = 0; i < array.Length; i++)
			{
				FileSystemInfo fileSystemInfo = array[i];
				if (!Directory.Exists(fileSystemInfo.FullName))
				{
					FileInfo fileInfo = new FileInfo(fileSystemInfo.FullName);
					FileHelper.fileInfoList.Add(fileInfo.FullName);
				}
				else
				{
					FileHelper.GetFileInfoByDirectory(fileSystemInfo.FullName);
				}
			}
			return FileHelper.fileInfoList;
		}
		public static List<string> GetFileAndDirectoryInfoByDirectory(string path)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
			FileSystemInfo[] array = fileSystemInfos;
			for (int i = 0; i < array.Length; i++)
			{
				FileSystemInfo fileSystemInfo = array[i];
				if (!Directory.Exists(fileSystemInfo.FullName))
				{
					FileInfo fileInfo = new FileInfo(fileSystemInfo.FullName);
					FileHelper.fileInfoList.Add(fileInfo.FullName);
				}
				else
				{
					FileHelper.fileInfoList.Add(fileSystemInfo.FullName);
					FileHelper.GetFileAndDirectoryInfoByDirectory(fileSystemInfo.FullName);
				}
			}
			return FileHelper.fileInfoList;
		}
	}
}
