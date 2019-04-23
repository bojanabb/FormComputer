using MySql.Data.MySqlClient;
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

namespace ProjekatZavrsni
{
	public partial class Form3 : Form
	{
		RadSaSlKol rad = new RadSaSlKol();
		Upiti upiti = new Upiti();
		String[] kolone = { "Ime", "BrojDokumenata", "Tip" };

		String username;
		String nazivKolekcije;
		String trenutnaPutanja = "";

		MySqlConnection konekcija;

		public Form3(String username, String nazivKolekcije, MySqlConnection conn)
		{
			InitializeComponent();
			this.username = username;
			this.nazivKolekcije = nazivKolekcije;
			this.konekcija = conn;
		}

		public String GetPath()
		{
			return this.trenutnaPutanja;
		}

		public void SetPath(String s)
		{
			this.trenutnaPutanja += s;
		}

		private void Form3_Load(object sender, EventArgs e)
		{
			String putanja = "../../Korisnici/" + this.username + "/" + this.nazivKolekcije + "/";
			String[] spisakFajlova = rad.spisakFajlova(putanja);
			String[] spisakFoldera = rad.spisakFoldera(putanja);
			rad.kreirajGridView(dataGridViewSpisak, spisakFajlova, spisakFoldera, this.kolone, putanja);
		}

		private void btnNazad_Click(object sender, EventArgs e)
		{
			String temp = GetPath();
			if (temp == "")
			{
				return;
			}
			String s = rad.novaPutanja(temp);
			this.trenutnaPutanja = "";
			SetPath(s);
			String curr = "../../Korisnici/" + this.username + "/" + this.nazivKolekcije + "/" + temp;
			String putanja = rad.novaPutanja(curr);
			String[] spisakFajlova = rad.spisakFajlova(putanja);
			String[] spisakFoldera = rad.spisakFoldera(putanja);
			rad.kreirajGridView(dataGridViewSpisak, spisakFajlova, spisakFoldera, this.kolone, putanja);
		}

		private void btnDodajDok_Click(object sender, EventArgs e)
		{
			int id = upiti.NadjiIDKolekcije(nazivKolekcije, username, konekcija);
			int zakljucana = upiti.ProvjeriLocked(id, konekcija);
			if (zakljucana == 1)
			{
				MessageBox.Show("Ne mozete modifikovati zakljucanu kolekciju!");
				return;
			}
			if (dataGridViewSpisak.SelectedRows.Count > 1)
			{
				MessageBox.Show("Mozete izabrati najvise jedan red");
				return;
			}
			if (dataGridViewSpisak.SelectedRows.Count == 0)
			{
				openFileDialog1.Filter = "PDF(*.pdf)|*.pdf|Text files(*.txt)|*.txt|All files(*.*)|*.*";
				if (openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					String[] putanje = openFileDialog1.FileNames;
					String[] nazivi = openFileDialog1.SafeFileNames;
					String destPath = "../../Korisnici/" + this.username + "/" + this.nazivKolekcije + "/" + GetPath();
					for (int i = 0; i < putanje.Length; i++)
					{
						rad.kopirajFajl(putanje[i], destPath, nazivi[i]);
					}
				}
			}
			if (dataGridViewSpisak.SelectedRows.Count == 1)
			{
				String tip = dataGridViewSpisak.SelectedRows[0].Cells["Tip"].Value.ToString();
				if (tip == "Fajl")
				{
					MessageBox.Show("Izaberite folder");
					return;
				}
				String ime = dataGridViewSpisak.SelectedRows[0].Cells["Ime"].Value.ToString();
				openFileDialog1.Filter = "PDF(*.pdf)|*.pdf|Text files(*.txt)|*.txt|All files(*.*)|*.*";
				if (openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					String[] putanje = openFileDialog1.FileNames;
					String[] nazivi = openFileDialog1.SafeFileNames;
					String destPath = "../../Korisnici/" + this.username + "/" + this.nazivKolekcije + "/" + GetPath() + ime + "/";
					for (int i = 0; i < putanje.Length; i++)
					{
						rad.kopirajFajl(putanje[i], destPath, nazivi[i]);
					}
				}
			}
			String putanja2 = "../../Korisnici/" + this.username + "/" + this.nazivKolekcije + "/" + GetPath();
			String[] spisakFajlova = rad.spisakFajlova(putanja2);
			String[] spisakFoldera = rad.spisakFoldera(putanja2);
			rad.kreirajGridView(dataGridViewSpisak, spisakFajlova, spisakFoldera, this.kolone, putanja2);
		}

		private void btnBrisiDok_Click(object sender, EventArgs e)
		{
			int id = upiti.NadjiIDKolekcije(nazivKolekcije, username, konekcija);
			int zakljucana = upiti.ProvjeriLocked(id, konekcija);
			if (zakljucana == 1)
			{
				MessageBox.Show("Ne mozete modifikovati zakljucanu kolekciju!");
				return;
			}
			if (dataGridViewSpisak.SelectedRows.Count < 1)
			{
				MessageBox.Show("Potrebno je da izaberete kolekcije koje brisete");
				return;
			}
			for (int i = 0; i < dataGridViewSpisak.SelectedRows.Count; i++)
			{
				String tip = dataGridViewSpisak.SelectedRows[i].Cells["Tip"].Value.ToString(); //tip=Fajl ili tip=Folder
				String naziv = dataGridViewSpisak.SelectedRows[i].Cells["Ime"].Value.ToString(); //ime = ili ime fajla ili ime foldera
				String putanja = "../../Korisnici/" + this.username + "/" + this.nazivKolekcije + "/" + GetPath() + naziv;
				
				if (tip == "Fajl")
				{
					int rez = rad.izbrisiFajl(putanja);
					if (rez == 1)
					{
						MessageBox.Show("Nije moguce izbrisati fajl " + naziv);
					}
				}
				else
				{
					DirectoryInfo di = new DirectoryInfo(putanja + "/");
					rad.izbrisiKolekciju(di);
					di.Delete();
				}
			}
			String putanja2 = "../../Korisnici/" + this.username + "/" + this.nazivKolekcije + "/" + GetPath();
			String[] spisakFajlova = rad.spisakFajlova(putanja2);
			String[] spisakFoldera = rad.spisakFoldera(putanja2);
			rad.kreirajGridView(dataGridViewSpisak, spisakFajlova, spisakFoldera, this.kolone, putanja2);
		}

		private void btnDalje_Click(object sender, EventArgs e)
		{
			if (dataGridViewSpisak.SelectedRows.Count != 1)
			{
				MessageBox.Show("Potrebno je da izaberete jedan podatak");
				return;
			}
			String tip = dataGridViewSpisak.SelectedRows[0].Cells["Tip"].Value.ToString();
			if (tip == "Fajl")
			{
				MessageBox.Show("Izabrali ste fajl. Nije moguce prikazati strukturu fajla");
				return;
			}
			String imeFoldera = dataGridViewSpisak.SelectedRows[0].Cells["Ime"].Value.ToString();
			String pocetnaPutanja = "../../Korisnici/" + this.username + "/" + this.nazivKolekcije + "/";
			String path = imeFoldera + "/";
			SetPath(path);
			String temp = GetPath();
			String putanja = pocetnaPutanja + temp;
			String[] spisakFajlova = rad.spisakFajlova(putanja);
			String[] spisakFoldera = rad.spisakFoldera(putanja);
			rad.kreirajGridView(dataGridViewSpisak, spisakFajlova, spisakFoldera, this.kolone, putanja);
		}
	}
}
