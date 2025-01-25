using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IniFileManager
{

	public class IniFileManager : IDisposable
	{
		#region => DLL

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		#endregion => DLL

		#region => Field

		private string _filePath = string.Empty;

		#endregion => Field

		#region => Constructor

		public IniFileManager(string filePath)
		{
			_filePath = filePath;
		}

		#endregion => Constructor

		#region => Method

		public void WriteString(string section, string key, string value)
		{
			WritePrivateProfileString(section, key, value, this._filePath);
		}

		public void WriteInt(string section, string key, int value)
		{
			WritePrivateProfileString(section, key, value.ToString(), this._filePath);
		}

		public void WriteDouble(string section, string key, double value)
		{
			WritePrivateProfileString(section, key, value.ToString(), this._filePath);
		}

		public string ReadString(string section, string key, string defaultValue)
		{
			StringBuilder temp = new StringBuilder(8192);

			if (GetPrivateProfileString(section, key, "", temp, 8192, this._filePath) == 0)
			{
				return defaultValue;
			}

			return temp.ToString();
		}

		public int ReadInt(string section, string key, int defaultValue)
		{
			StringBuilder temp = new StringBuilder(8192);

			if (GetPrivateProfileString(section, key, "", temp, 8192, this._filePath) == 0)
			{
				return defaultValue;
			}

			int result = 0;
			if (!int.TryParse(temp.ToString(), out result))
			{
				return defaultValue;
			}

			return result;
		}

		public double ReadDouble(string section, string key, double defaultValue)
		{
			StringBuilder temp = new StringBuilder(8192);

			if (GetPrivateProfileString(section, key, "", temp, 8192, this._filePath) == 0)
			{
				return defaultValue;
			}

			double result = 0.0;
			if (!double.TryParse(temp.ToString(), out result))
			{
				return defaultValue;
			}

			return result;
		}

		public bool SectionExist(string section)
		{
			StringBuilder temp = new StringBuilder(8192);
			return GetPrivateProfileString(section, null, "", temp, 8192, this._filePath) > 0;
		}

		public void RemoveSection(string section)
		{
			var lines = File.ReadAllLines(this._filePath).ToList();
			bool isSection = false;

			for (int i = 0; i < lines.Count; i++)
			{
				if (lines[i].StartsWith("[") && lines[i].EndsWith("]"))
				{
					isSection = lines[i].Trim('[', ']') == section;
					if (isSection && i > 0) lines.RemoveAt(i--);
				}
				else if (isSection && !lines[i].StartsWith("["))
				{
					lines.RemoveAt(i--);
				}
			}

			File.WriteAllLines(this._filePath, lines);
		}

		// using 키워드 사용하기 위해 IDispose 상속
		public void Dispose()
		{

		}

		#endregion => Method
	}
}
