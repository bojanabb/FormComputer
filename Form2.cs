using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace ProjekatZavrsni
{
	public partial class Form2 : Form
	{

		String activeUser;
		String colName;
		String type;
		MySqlConnection konekcija;

		Upiti upiti = new Upiti();

		public Form2(String username, String imeKolekcije, String tip, MySqlConnection conn)
		{
			InitializeComponent();
			this.activeUser = username;
			this.colName = imeKolekcije;
			this.type = tip;
			this.konekcija = conn;
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			
		}

		private void buttonPodijeli_Click(object sender, EventArgs e)
		{
			if (textBoxImeNove1.Text == ""  || textBoxImeNove2.Text == "")
			{
				MessageBox.Show("Unesite imena kolekcija!");
				return;
			}
			String ime1 = textBoxImeNove1.Text;
			String ime2 = textBoxImeNove2.Text;

			String path = "../../Korisnici/" + activeUser + "/";
			DirectoryInfo di1 = new DirectoryInfo(path + ime1);
			if (di1.Exists)
			{
				MessageBox.Show("Folder sa imenom koje ste zadali vec postoji!");
				return;
			}

			DirectoryInfo di2 = new DirectoryInfo(path + ime2);
			if (di2.Exists)
			{
				MessageBox.Show("Folder sa imenom koje ste zadali vec postoji!");
				return;
			}
			di1.Create();
			di2.Create();

			upiti.UnesiUKolekciju(ime1, activeUser, DateTime.Now, this.type, konekcija);
			upiti.UnesiUKolekciju(ime2, activeUser, DateTime.Now, this.type, konekcija);


			if (radioBtnVelicina.Checked)
			{
				if (textBoxVelicina.Text == "")
				{
					MessageBox.Show("Unesite kriterijum podjele!");
					return;
				}
				double vel = Double.Parse(textBoxVelicina.Text);

				String[] fajlovi = Directory.GetFiles(path + this.colName);
				
				for (int i = 0; i < fajlovi.Length; i++)
				{
					FileInfo fi = new FileInfo(fajlovi[i]);
					if(fi.Length <= vel)
					{
						fi.CopyTo(path + ime1 + "/" + fi.Name);
					}
					else
					{
						fi.CopyTo(path + ime2 + "/" + fi.Name);
					}
				}
			}
			else
			{
				if (textBoxIme.Text == "")
				{
					MessageBox.Show("Unesite kriterijum podjele!");
					return;
				}

				String uzorak = textBoxIme.Text;

				String[] fajlovi = Directory.GetFiles(path + this.colName);

				for (int i = 0; i < fajlovi.Length; i++)
				{
					FileInfo fi = new FileInfo(fajlovi[i]);
					if (fi.Name.Contains(uzorak))
					{
						fi.CopyTo(path + ime1 + "/" + fi.Name);
					}
					else
					{
						fi.CopyTo(path + ime2 + "/" + fi.Name);
					}
				}
			}
			this.Close();
		}
	}
}
