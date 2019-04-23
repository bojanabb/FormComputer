using Ionic.Zip;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjekatZavrsni
{
	class RadSaSlKol
	{

		#region FAJLOVI

		public void kopirajFolder(String putanja1, String putanja2, String putanja3, String naziv)
		{
			using (ZipFile zf = new ZipFile())
			{
				zf.AddDirectory(putanja1);
				zf.Save(putanja2 + naziv + ".zip");
			}
			using (ZipFile zf = ZipFile.Read(putanja2 + naziv + ".zip"))
			{
				foreach (ZipEntry ze in zf)
				{
					ze.Extract(putanja3 + naziv);
				}
			}
			FileInfo fi = new FileInfo(putanja1 + ".zip");
			fi.Delete();
		}

		public int brojFajlova(DirectoryInfo di)
		{
			int ukupno = 0;
			FileInfo[] fi = di.GetFiles();
			ukupno += fi.Length;
			DirectoryInfo[] dirs = di.GetDirectories();
			foreach (DirectoryInfo dir in dirs)
			{
				ukupno += brojFajlova(dir);
			}
			return ukupno;
		}

		public String[] spisakFajlova(String putanja)
		{
			String[] curr = Directory.GetFiles(putanja);
			String[] konacno = new String[curr.Length];
			String[] temp;
			for (int i = 0; i < curr.Length; i++)
			{
				temp = curr[i].Split('/');
				konacno[i] = temp[temp.Length - 1];
			}
			return konacno;
		}

		public String[] spisakFoldera(String putanja)
		{
			String[] curr = Directory.GetDirectories(putanja);
			String[] konacno = new String[curr.Length];
			String[] temp;
			for (int i = 0; i < curr.Length; i++)
			{
				temp = curr[i].Split('/');
				konacno[i] = temp[temp.Length - 1];
			}
			return konacno;
		}

		public String novaPutanja(String putanja)
		{
			//nazad i naprijed u slozenim kolekcijama
			int pos = putanja.LastIndexOf('/');
			String temp = putanja.Substring(0, pos);
			pos = temp.LastIndexOf('/');
			String konacno = temp.Substring(0, pos + 1);
			return konacno;
		}

		public int izbrisiFajl(String putanja)
		{
			FileInfo fi = new FileInfo(putanja);
			try
			{
				fi.Delete();
				return 0;
			}
			catch
			{
				return 1;
			}
		}

		public void izbrisiKolekciju(DirectoryInfo di)
		{
			FileInfo[] fi = di.GetFiles();
			foreach (FileInfo file in fi)
			{
				file.Delete();
			}
			DirectoryInfo[] dirs = di.GetDirectories();
			foreach (DirectoryInfo dir in dirs)
			{
				izbrisiKolekciju(dir);
				dir.Delete();
			}
		}

		public void kopirajFajl(String putanja1, String putanja2, String naziv)
		{
			FileInfo fi = new FileInfo(putanja1);
			if (File.Exists(putanja2 + naziv))
			{
				DialogResult dr = MessageBox.Show("Postoji fajl. Zelite li da zamjenite fajlove?", "Question", 
								  MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (dr == DialogResult.Yes)
				{
					File.Delete(putanja2 + naziv);
					fi.CopyTo(putanja2 + naziv);
				}
			}
			else
			{
				fi.CopyTo(putanja2 + naziv);
			}
		}

		#endregion

		#region METODE

		public void kreirajGridView(DataGridView dgv, String[] fajlovi, String[] folderi, String[] kolone, String putanja)
		{
			DataTable table = new DataTable();
			for (int i = 0; i < kolone.Length; i++)
			{
				DataColumn kolona = new DataColumn(kolone[i]);
				table.Columns.Add(kolona);
			}
			for (int i = 0; i < folderi.Length; i++)
			{
				DataRow red = table.NewRow();
				DirectoryInfo di = new DirectoryInfo(putanja + folderi[i] + "/");
				int broj = brojFajlova(di);
				
				red[0] = folderi[i];
				red[1] = broj;
				red[2] = "Folder";
				table.Rows.Add(red);
			}
			
			for (int i = 0; i < fajlovi.Length; i++)
			{
				DataRow red = table.NewRow();
				red[0] = fajlovi[i];
				red[1] = 0;
				red[2] = "Fajl";
				table.Rows.Add(red);
			}
			dgv.DataSource = table;
		}

		#endregion
	}
}
