using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows.Forms;
using Ionic.Zip;


namespace ProjekatZavrsni
{
	public partial class Form1 : Form
	{
		#region PODACI
		MySqlConnection konekcija;
		String konStr = "Server = localhost; Database = probaprojekta; uid = root; password = brusnica;";
		Upiti upiti = new Upiti();
		RadSaSlKol slozena = new RadSaSlKol();

		#endregion
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				konekcija = new MySqlConnection(konStr);
				konekcija.Open();
			}
			catch (Exception err)
			{
				MessageBox.Show("Konektovanje na bazu nije uspjesno!\n" + err);
			}
		}

		private void buttonLogIn_Click(object sender, EventArgs e)
		{
			String username = textBoxUsername.Text;
			String password = textBoxPassword.Text;
			if (username == "" || password == "")
			{
				MessageBox.Show("Polja ne mogu biti prazna");
				return;
			}
			else
			{
				//ako je user and pass ok provjeriti da li se loguje admin ili obicni korisnik
				if (upiti.Logovanje(username, password, konekcija) == 1)
				{
					panelAdmin.Visible = true;
					panelKorisnik.Visible = false; 
					panelZahtjevi.Visible = false;
					panelPocetni.Visible = false;
				}
				else if (upiti.Logovanje(username, password, konekcija) == 0)
				{
					panelKorisnik.Visible = true;
					panelAdmin.Visible = false;
					panelZahtjevi.Visible = false;
					panelPocetni.Visible = false;
					lblActiveUser.Text = username;
					upiti.PregledKolekcija(username, txtImeCOL, konekcija);//popuni combo box
				}
				textBoxUsername.Text = "";
				textBoxPassword.Text = "";
			}
			
		}

		private void btnIzlistajZahtjeve_Click(object sender, EventArgs e)
		{
			panelZahtjevi.Visible = true;
			upiti.PrikaziZahtjeve(gridZahtjevi, konekcija);
		}

		private void buttonMakeAccount_Click(object sender, EventArgs e)
		{
			if (formaIme.Text == "" || formaPrezime.Text == "" || formaJMBG.Text == "" || formaUsername.Text == "" || formaPassword.Text == "")
			{
				MessageBox.Show("Sva polja moraju biti popunjena!\n");
				return;
			}
			if (formaJMBG.Text.Length != 13)
			{
				MessageBox.Show("JMBG mora imati 13 karaktera!\n");
				return;
			}
			int postojiUser = upiti.ProvjeriUsername(textBoxUsername.Text, konekcija);
			//MessageBox.Show(postojiUser.ToString());
			if (postojiUser == 2)
			{
				return;
			}
			if (postojiUser == 1)
			{
				MessageBox.Show("Vec postoji korisnik sa ovim username-om");
				return;
			}
			upiti.SacuvajZahtjev(formaJMBG.Text, formaIme.Text, formaPrezime.Text, formaUsername.Text, formaPassword.Text, konekcija);

			formaIme.Text = "";
			formaPrezime.Text = "";
			formaJMBG.Text = "";
			formaUsername.Text = "";
			formaPassword.Text = "";
		}

		private void btnDodajKorisnika_Click(object sender, EventArgs e)
		{
			if (gridZahtjevi.SelectedRows.Count == 0)
			{
				MessageBox.Show("Potrebno je da izaberete red");
				return;
			}
			for (int i = 0; i < gridZahtjevi.SelectedRows.Count; i++)
			{
				try
				{
				String jmbg = gridZahtjevi.SelectedRows[i].Cells["JMBG"].Value.ToString();
				String ime = gridZahtjevi.SelectedRows[i].Cells["ime"].Value.ToString();
				String prezime = gridZahtjevi.SelectedRows[i].Cells["prezime"].Value.ToString();
				String username = gridZahtjevi.SelectedRows[i].Cells["username"].Value.ToString();
				String password = gridZahtjevi.SelectedRows[i].Cells["password"].Value.ToString();

				upiti.UnesiUBazu(jmbg, ime, prezime, username, password, konekcija);
				upiti.IzbrisiZahtjev(jmbg, konekcija);
				
				kreiranjefoldera(username);
				}
				catch(Exception err)
				{
					MessageBox.Show("Null pointer exception!\n" + err);
				}

			}
			upiti.PrikaziZahtjeve(gridZahtjevi, konekcija);

		}

		public void kreiranjefoldera(String username)
		{
			DirectoryInfo di = new DirectoryInfo("../../Korisnici/" + username);
			if (di.Exists)
			{
				MessageBox.Show("Folder sa ovim imenom vec postoji!");
				return;
			}
			di.Create();

		}

		private void btnLogOut_Click(object sender, EventArgs e)
		{
			panelPocetni.Visible = true;
			panelAdmin.Visible = false;
		}

		private void btnOdbijKorisnika_Click(object sender, EventArgs e)
		{
			if (gridZahtjevi.SelectedRows.Count == 0)
			{
				MessageBox.Show("Potrebno je da izaberete red");
				return;
			}
			for (int i = 0; i < gridZahtjevi.SelectedRows.Count; i++)
			{
				try
				{
					String jmbg = gridZahtjevi.SelectedRows[i].Cells["JMBG"].Value.ToString();
					String ime = gridZahtjevi.SelectedRows[i].Cells["ime"].Value.ToString();
					String prezime = gridZahtjevi.SelectedRows[i].Cells["prezime"].Value.ToString();
					String username = gridZahtjevi.SelectedRows[i].Cells["username"].Value.ToString();
					String password = gridZahtjevi.SelectedRows[i].Cells["password"].Value.ToString();

					upiti.IzbrisiZahtjev(jmbg, konekcija);
					
				}
				catch (Exception err)
				{
					MessageBox.Show("Null pointer exception!\n" + err);
				}

			}
			upiti.PrikaziZahtjeve(gridZahtjevi, konekcija);
		}

		private void btnBack_Click(object sender, EventArgs e)
		{
			panelPocetni.Visible = true;
			panelKorisnik.Visible = false;
			panelPregledKol.Visible = false;
			txtImeCOL.Text = "";
			comboBoxTipCol.SelectedIndex = -1;
			if(txtImeCOL.Items.Count>0)
			{
				txtImeCOL.Items.Clear();
			}
		}

		private void btnCreateCollection_Click(object sender, EventArgs e)
		{
			if (txtImeCOL.Text == "")
			{
				MessageBox.Show("Navedite ime kolekcije!");
				return;
			}
			DirectoryInfo di = new DirectoryInfo("../../Korisnici/" + lblActiveUser.Text + "/" + txtImeCOL.Text);
			if (di.Exists)
			{
				MessageBox.Show("Kolekcija sa ovim imenom vec postoji!");
				return;
			}
			if(comboBoxTipCol.Text == "")
			{
				MessageBox.Show("Odaberite tip kolekcije koju kreirate!");
				return;
			}
			di.Create();
			DateTime datKreiranja = DateTime.Now;
			upiti.UnesiUKolekciju(txtImeCOL.Text ,lblActiveUser.Text, datKreiranja, comboBoxTipCol.Text, konekcija);
			txtImeCOL.Text = "";
			comboBoxTipCol.SelectedIndex = -1;
			if (txtImeCOL.Items.Count > 0)
			{
				txtImeCOL.Items.Clear();
			}
			upiti.PregledKolekcija(lblActiveUser.Text, txtImeCOL, konekcija);
		}

		private void btnPregledKol_Click(object sender, EventArgs e)
		{
			int id = upiti.NadjiIDKolekcije(txtImeCOL.Text, lblActiveUser.Text, konekcija);
			int child = upiti.NadjiChildKolekcije(id, konekcija);
			if (child == 0)
			{
				btnSpoji.Visible = false;
				btnDijeli.Visible = false;
				btnNapraviSlozenu.Visible = false;
				btnKreiraj.Visible = true;
				txtNazivUzorka.Visible = true;

				panelPregledKol.Visible = true;
				upiti.PrikaziDokumenta(tabelaDokumenta, txtImeCOL.Text, konekcija);
			}
			else
			{
				Form3 forma3 = new Form3(lblActiveUser.Text, txtImeCOL.Text, konekcija);
				forma3.ShowDialog();
			}
			
		}

		private void btnDodajDoc_Click(object sender, EventArgs e)
		{
			if(txtImeCOL.Text == "")
			{
				MessageBox.Show("Selektujte kolekciju!");
				return;
			}

			btnSpoji.Visible = false;
			btnDijeli.Visible = false;
			btnKreiraj.Visible = false;
			txtNazivUzorka.Visible = false;

			int idKol = upiti.NadjiIDKolekcije(txtImeCOL.Text, lblActiveUser.Text, konekcija);

			if (upiti.ProvjeriLocked(idKol, konekcija) == 0)
			{

				openFileDialog1.FileName = "";
				openFileDialog1.Filter = "| *" + comboBoxTipCol.Text;
				if(comboBoxTipCol.Text== "")
				{
					MessageBox.Show("Nema zadate ekstenzije!");
					return;
				}
				openFileDialog1.Title = "Odaberite dokumenta za kolekciju";
				

				if (openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					String[] filePaths = openFileDialog1.FileNames; // citava putanja do fajla
					String[] fileNames = openFileDialog1.SafeFileNames; //imena fajlova sa ekstenzijom
					int len = fileNames.Length;
					for (int i = 0; i < len; i++)
					{
						String collectionPath = "../../Korisnici/" + lblActiveUser.Text + "/" + txtImeCOL.Text + "/" + fileNames[i];
						FileInfo fi = new FileInfo(filePaths[i]);
						//fi.Length - velicina fajla u bajtima - znaci treba da dijelimo sa 1024

						if (File.Exists(collectionPath))
						{
							DialogResult dr = MessageBox.Show("Fajl sa ovim imenom vec postoji u kolekciji " + txtImeCOL.Text + "!\n Zelite li da zamijenite" +
								"postojeci fajl novim?", "Upozorenje!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
							if(dr == DialogResult.Yes)
							{
								upiti.BrisiDokument(fileNames[i], idKol, konekcija);
								FileInfo newFI = new FileInfo(collectionPath);
								newFI.Delete();
								fi.CopyTo(collectionPath);
								int kolID = upiti.NadjiIDKolekcije(txtImeCOL.Text, lblActiveUser.Text, konekcija);
								upiti.UnesiUDokument(fileNames[i], kolID, konekcija);
							}
						}
						else
						{
							fi.CopyTo(collectionPath);
							int kolID = upiti.NadjiIDKolekcije(txtImeCOL.Text, lblActiveUser.Text, konekcija);
							upiti.UnesiUDokument(fileNames[i], kolID, konekcija);
						}
						
					}
					String path2 = "../../Korisnici/" + lblActiveUser.Text + "/" + txtImeCOL.Text + "/";
					String[] imenaSvihFajlova = Directory.GetFiles(path2);
					int brDoc = imenaSvihFajlova.Length;
					double velicina = 0;
					foreach (String doc in imenaSvihFajlova)
					{
						FileInfo fi3 = new FileInfo(doc);
						velicina += (double)(fi3.Length / 1024.0);
					}
					DateTime datModif = DateTime.Now;
					upiti.UpdateKolekcije(txtImeCOL.Text, lblActiveUser.Text, datModif, brDoc, velicina, konekcija);
					upiti.PrikaziDokumenta(tabelaDokumenta, txtImeCOL.Text, konekcija);
				}
			}
			else
			{
				MessageBox.Show("Nemate pravo modifikacije zakljucane kolekcije!\n");
			}
			

		}

		private void btnBrisiDoc_Click(object sender, EventArgs e)
		{
			btnSpoji.Visible = false;
			btnDijeli.Visible = false;
			btnKreiraj.Visible = false;
			txtNazivUzorka.Visible = false;

			
			int idKol = upiti.NadjiIDKolekcije(txtImeCOL.Text, lblActiveUser.Text, konekcija);
			if (upiti.ProvjeriLocked(idKol, konekcija) == 0)
			{
				if (tabelaDokumenta.SelectedRows.Count == 0)
				{
					MessageBox.Show("Potrebno je da izaberete dokument koji zelite da brisete!");
					return;
				}
				for (int i = 0; i < tabelaDokumenta.SelectedRows.Count; i++)
				{
					try
					{
						String imeDok = tabelaDokumenta.SelectedRows[i].Cells["ime1"].Value.ToString();
						String ime = tabelaDokumenta.SelectedRows[i].Cells["ime"].Value.ToString();
						String userID = tabelaDokumenta.SelectedRows[i].Cells["userID"].Value.ToString();
						
						FileInfo fi = new FileInfo("../../Korisnici/" + lblActiveUser.Text + "/" + txtImeCOL.Text + "/" + imeDok); //putanja do fajla
						fi.Delete();

						upiti.BrisiDokument(imeDok, idKol, konekcija);
					}
					catch (Exception err)
					{
						MessageBox.Show("Null pointer exception!\n" + err);
					}
				}
				String path2 = "../../Korisnici/" + lblActiveUser.Text + "/" + txtImeCOL.Text + "/";
				String[] imenaSvihFajlova = Directory.GetFiles(path2);
				int brDoc = imenaSvihFajlova.Length;
				double velicina = 0;
				foreach (String doc in imenaSvihFajlova)
				{
					FileInfo fi3 = new FileInfo(doc);
					velicina += (double)(fi3.Length / 1024.0);
				}
				DateTime datModif = DateTime.Now;
				upiti.UpdateKolekcije(txtImeCOL.Text, lblActiveUser.Text, datModif, brDoc, velicina, konekcija);
				upiti.PrikaziDokumenta(tabelaDokumenta, txtImeCOL.Text, konekcija);
			}
			else
			{
				MessageBox.Show("Nemate pravo modifikacije kolekcije!\n");
			}
			
		}

		private void btnLock_Click(object sender, EventArgs e)
		{
			if(txtImeCOL.Text == "")
			{
				MessageBox.Show("Selektujte kolekciju  koju zelite da zakljucate!");
				return;
			}
			int id = upiti.NadjiIDKolekcije(txtImeCOL.Text, lblActiveUser.Text, konekcija);
			upiti.ZakljucajKolekciju(id, 1, konekcija);
		}

		private void txtImeCOL_SelectedIndexChanged(object sender, EventArgs e)
		{
			upiti.TipKolekcije(lblActiveUser.Text, txtImeCOL.Text, comboBoxTipCol, konekcija);
		}

		private void btnSpojiKol_Click(object sender, EventArgs e)
		{	
			btnSpoji.Visible = true;
			btnDijeli.Visible = true;
			txtNazivUzorka.Visible = true;
			panelPregledKol.Visible = true;
			btnKreiraj.Visible = false;
			btnNapraviSlozenu.Visible = true;
			upiti.KorisnikoveKolekcije(lblActiveUser.Text, tabelaDokumenta, konekcija);
		}

		private void btnSpoji_Click(object sender, EventArgs e)
		{
			if (tabelaDokumenta.SelectedRows.Count != 2)
			{
				MessageBox.Show("Potrebno je selektovati tacno dvije kolekcije!");
				return;
			}

			String prva = tabelaDokumenta.SelectedRows[0].Cells["ime"].Value.ToString();
			String druga = tabelaDokumenta.SelectedRows[1].Cells["ime"].Value.ToString();
			String novoIme = prva + "_" + druga;

			DirectoryInfo di = new DirectoryInfo("../../Korisnici/" + lblActiveUser.Text + "/" + novoIme);
			di.Create();
			upiti.UnesiUKolekciju(novoIme, lblActiveUser.Text, DateTime.Now, "xxx", konekcija);

			String collectionPath = "../../Korisnici/" + lblActiveUser.Text + "/" + novoIme + "/"; //do spojenog foldera
			String PathPrva = "../../Korisnici/" + lblActiveUser.Text + "/" + prva + "/"; //do prvog
			String PathDruga = "../../Korisnici/" + lblActiveUser.Text + "/" + druga + "/"; //do drugog

			String[] imena = Directory.GetFiles(PathPrva); //prva selektovana kolekcija
			foreach (String doc in imena)
			{
				FileInfo fi = new FileInfo(doc);
				fi.CopyTo(collectionPath + fi.Name.ToString());
			}

			imena = Directory.GetFiles(PathDruga);  //druga selektovana kolekcija
			foreach (String doc in imena)
			{
				FileInfo fi = new FileInfo(doc);
				if (!File.Exists(collectionPath + fi.Name.ToString()))
				{
					fi.CopyTo(collectionPath + fi.Name.ToString());
				}
			}

			String path2 = "../../Korisnici/" + lblActiveUser.Text + "/" + novoIme + "/";
			String[] imenaSvihFajlova = Directory.GetFiles(path2);
			int brDoc = imenaSvihFajlova.Length;
			double velicina = 0;
			foreach (String doc in imenaSvihFajlova)
			{
				FileInfo fi3 = new FileInfo(doc);
				velicina += (double)(fi3.Length / 1024.0);
			}
			DateTime datModif = DateTime.Now;
			upiti.UpdateKolekcije(novoIme, lblActiveUser.Text, datModif, brDoc, velicina, konekcija);

			upiti.KorisnikoveKolekcije(lblActiveUser.Text, tabelaDokumenta, konekcija);
			txtImeCOL.Items.Clear();
			upiti.PregledKolekcija(lblActiveUser.Text, txtImeCOL, konekcija);
		}

		private void btnKreiraj_Click(object sender, EventArgs e)
		{
			if(tabelaDokumenta.SelectedRows.Count == 0)
			{
				MessageBox.Show("Morate selektovati redove");
				return;
			}
			if(txtNazivUzorka.Text == "")
			{
				MessageBox.Show("Potrebno je da unesete ime uzorka!");
				return;
			}
			
			DirectoryInfo di = new DirectoryInfo("../../Korisnici/" + lblActiveUser.Text + "/" + txtNazivUzorka.Text);
			di.Create();
			upiti.UnesiUKolekciju(txtNazivUzorka.Text, lblActiveUser.Text, DateTime.Now, "xxx", konekcija);

			int idKol = upiti.NadjiIDKolekcije(txtNazivUzorka.Text, lblActiveUser.Text, konekcija);
			for(int i = 0; i < tabelaDokumenta.SelectedRows.Count; i++)
			{
				String imeFajla = tabelaDokumenta.Rows[i].Cells["ime1"].Value.ToString();
				String path1 = "../../Korisnici/" + lblActiveUser.Text + "/" + txtImeCOL.Text + "/" + imeFajla;
				String destPath = "../../Korisnici/" + lblActiveUser.Text + "/" + txtNazivUzorka.Text + "/" + txtNazivUzorka.Text + imeFajla;
				FileInfo fi = new FileInfo(path1);
				fi.CopyTo(destPath);
				upiti.UnesiUDokument(txtNazivUzorka.Text + imeFajla, idKol, konekcija);
			}

			String path2 = "../../Korisnici/" + lblActiveUser.Text + "/" + txtNazivUzorka.Text + "/";
			String[] imenaSvihFajlova = Directory.GetFiles(path2);
			int brDoc = imenaSvihFajlova.Length;
			double velicina = 0;
			foreach (String doc in imenaSvihFajlova)
			{
				FileInfo fi3 = new FileInfo(doc);
				velicina += (double)(fi3.Length / 1024.0);
			}
			DateTime datModif = DateTime.Now;
			upiti.UpdateKolekcije(txtNazivUzorka.Text, lblActiveUser.Text, datModif, brDoc, velicina, konekcija);
		}

		private void btnEXP_Click(object sender, EventArgs e)
		{
			if (txtImeCOL.Text == "")
			{
				MessageBox.Show("Selektujte kolekciju koju zelite da eksportujete!");
				return;
			}
			
			String folderPath = "../../Korisnici/" + lblActiveUser.Text + "/" + txtImeCOL.Text;
			using (ZipFile zf = new ZipFile())
			{
				zf.AddDirectory(folderPath);
				saveFileDialog1.FileName = txtImeCOL.Text;
				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					zf.Save(saveFileDialog1.FileName + ".ZIP");
				}

			}
		}

		private void btnImp_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				String path = openFileDialog1.FileName; //citava putanja do foldera sa eksenzijom
				String[] parts = openFileDialog1.SafeFileName.Split('.');
				String collectionPath = "../../Korisnici/" + lblActiveUser.Text + "/" + parts[0] + "/";

				using (ZipFile zf = ZipFile.Read(path))
				{
					foreach (ZipEntry ze in zf)
					{
						ze.Extract(collectionPath);
					}

				}

				upiti.UnesiUKolekciju(parts[0], lblActiveUser.Text, DateTime.Now, "xxx", konekcija);
				String path2 = "../../Korisnici/" + lblActiveUser.Text + "/" + parts[0] + "/";
				String[] imenaSvihFajlova = Directory.GetFiles(path2);
				int brDoc = imenaSvihFajlova.Length;
				double velicina = 0;
				foreach (String doc in imenaSvihFajlova)
				{
					FileInfo fi3 = new FileInfo(doc);
					velicina += (double)(fi3.Length / 1024.0);
				}
				DateTime datModif = DateTime.Now;
				upiti.UpdateKolekcije(parts[0], lblActiveUser.Text, datModif, brDoc, velicina, konekcija);
			}
			
		}

		private void btnDijeli_Click(object sender, EventArgs e)
		{
			Form2 forma = new Form2(lblActiveUser.Text, tabelaDokumenta.SelectedRows[0].Cells["ime"].Value.ToString(),
									tabelaDokumenta.SelectedRows[0].Cells["tip"].Value.ToString(), konekcija);
			forma.ShowDialog();
			upiti.KorisnikoveKolekcije(lblActiveUser.Text, tabelaDokumenta, konekcija);
			txtImeCOL.Items.Clear();
			upiti.PregledKolekcija(lblActiveUser.Text, txtImeCOL, konekcija);
		}

		private void btnNapraviSlozenu_Click(object sender, EventArgs e)
		{
			if (tabelaDokumenta.SelectedRows.Count < 1)
			{
				MessageBox.Show("Selektujte bar jednu kolekciju!");
				return;
			}
			if (txtNazivUzorka.Text == "")
			{
				MessageBox.Show("Navedite ime slozene kolekcije!");
				return;
			}

			String path = "../../Korisnici/" + lblActiveUser.Text + "/" + txtNazivUzorka.Text;
			DirectoryInfo di = new DirectoryInfo(path);
			if (di.Exists)
			{
				MessageBox.Show("Folder sa imenom koje ste zadali vec postoji!");
				return;
			}
			di.Create();
			upiti.UnesiUSlozenuKolekciju(txtNazivUzorka.Text, lblActiveUser.Text, DateTime.Now, "xxx", konekcija);
			
			String path3 = "../../Korisnici/" + lblActiveUser.Text + "/" + txtNazivUzorka.Text + "/";
			for (int i = 0; i < tabelaDokumenta.SelectedRows.Count; i++)
			{
				String path1 = "../../Korisnici/" + lblActiveUser.Text + "/" + tabelaDokumenta.SelectedRows[i].Cells["ime"].Value.ToString();
				String path2 = "../../Korisnici/" + lblActiveUser.Text + "/";
				slozena.kopirajFolder(path1, path2, path3, tabelaDokumenta.SelectedRows[i].Cells["ime"].Value.ToString());
			}
						
		}
	}
}
