using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
namespace XHD.Excel
{
	public static class FileRight
	{
		public static void SetRight(string path, string userName, string userRight)
		{
			FileSystemRights fileSystemRights = (FileSystemRights)0;
			if (userRight.IndexOf("R") >= 0)
			{
				fileSystemRights |= FileSystemRights.Read;
			}
			if (userRight.IndexOf("C") >= 0)
			{
				fileSystemRights |= FileSystemRights.ChangePermissions;
			}
			if (userRight.IndexOf("F") >= 0)
			{
				fileSystemRights |= FileSystemRights.FullControl;
			}
			if (userRight.IndexOf("W") >= 0)
			{
				fileSystemRights |= FileSystemRights.Write;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			DirectorySecurity accessControl = directoryInfo.GetAccessControl();
			InheritanceFlags inheritanceFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
			FileSystemAccessRule rule = new FileSystemAccessRule(userName, fileSystemRights, inheritanceFlags, PropagationFlags.None, AccessControlType.Allow);
			bool flag;
			accessControl.ModifyAccessRule(AccessControlModification.Add, rule, out flag);
			directoryInfo.SetAccessControl(accessControl);
		}
		public static string GetAllAccountRight(string path)
		{
			DirectorySecurity accessControl = Directory.GetAccessControl(path, AccessControlSections.All);
			string text = string.Empty;
			foreach (FileSystemAccessRule fileSystemAccessRule in accessControl.GetAccessRules(true, true, typeof(NTAccount)))
			{
				if ((fileSystemAccessRule.FileSystemRights & FileSystemRights.Read) != (FileSystemRights)0)
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						fileSystemAccessRule.IdentityReference,
						":",
						fileSystemAccessRule.FileSystemRights.ToString(),
						"|"
					});
				}
			}
			text.TrimEnd(new char[]
			{
				'|'
			});
			return text;
		}
		public static string GetRightByAccount(string path, string account)
		{
			DirectorySecurity accessControl = Directory.GetAccessControl(path, AccessControlSections.All);
			string text = string.Empty;
			foreach (FileSystemAccessRule fileSystemAccessRule in accessControl.GetAccessRules(true, true, typeof(NTAccount)))
			{
				if ((fileSystemAccessRule.FileSystemRights & FileSystemRights.Read) != (FileSystemRights)0)
				{
					if (fileSystemAccessRule.IdentityReference.ToString().IndexOf(account, StringComparison.OrdinalIgnoreCase) != -1)
					{
						text += fileSystemAccessRule.FileSystemRights.ToString();
					}
				}
			}
			text.TrimEnd(new char[]
			{
				'|'
			});
			return text;
		}
	}
}
