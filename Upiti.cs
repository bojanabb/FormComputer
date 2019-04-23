using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;
using Ionic.Zip;

namespace ProjekatZavrsni
{
	class Upiti
	{
		public int Logovanje(String username, String password, MySqlConnection conn)
		{
			int postoji = ProvjeriUsername(username, conn);
			if (postoji == 0)
			{
				MessageBox.Show("Nalog ne postoji");
				return -1;
			}
			if (postoji == 1)
			{
				String upit = "select password,admin from Korisnik where username=@userParam";
				MySqlCommand komanda = new MySqlCommand();
				try
				{
					komanda.CommandText = upit;
					komanda.Connection = conn;
					komanda.Parameters.Add("userParam", MySqlDbType.VarChar);
					komanda.Parameters["userParam"].Value = username;
					MySqlDataReader reader = komanda.ExecuteReader();
					reader.Read();
					String pass = reader["password"].ToString();
					int admin = Int32.Parse(reader["admin"].ToString());
					reader.Close();
					if (pass != password)
					{
						MessageBox.Show("Neispravan password");
						return -1;
					}
					return admin;
				}
				catch (Exception err)
				{
					MessageBox.Show("neispravan upit " + err);
					return -2;
				}
			}
			return -3;
		}

		public int ProvjeriUsername(String username, MySqlConnection conn)
		{
			String upit = "select count(*) as broj from Korisnik where username=@userParam";
			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;
				komanda.Parameters.Add("userParam", MySqlDbType.VarChar);
				komanda.Parameters["userParam"].Value = username;
				MySqlDataReader reader = komanda.ExecuteReader();
				reader.Read();
				int podatak = Int32.Parse(reader["broj"].ToString());
				reader.Close();
				return podatak; // ili 1 ili 0
			}
			catch (Exception err)
			{
				MessageBox.Show("Greska " + err);
				return 2; //ako pukne program
			}
		}

		public void PrikaziZahtjeve(DataGridView dgv, MySqlConnection conn)
		{
			String upit = "select * from zahtjevi";
			MySqlCommand komanda = new MySqlCommand();
			DataTable tabela = new DataTable();

			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				MySqlDataAdapter adapter = new MySqlDataAdapter();
				adapter.SelectCommand = komanda;
				adapter.Fill(tabela);
				dgv.DataSource = tabela;
				dgv.Columns["ID"].Visible = false;
				dgv.Columns["password"].Visible = false;
			}
			catch
			{

			}
		}

		public void SacuvajZahtjev(String jmbg, String ime, String prezime, String username, String password, MySqlConnection conn)
		{
			String upit = "insert into zahtjevi (`JMBG`,`Ime`,`Prezime`,`Username`,`Password`) values " +
				"(@jmbgParam, @imeParam, @prezimeParam,@userParam, @passParam)";
			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;
				komanda.Parameters.Add("jmbgParam", MySqlDbType.VarChar);
				komanda.Parameters.Add("imeParam", MySqlDbType.VarChar);
				komanda.Parameters.Add("prezimeParam", MySqlDbType.VarChar);
				komanda.Parameters.Add("userParam", MySqlDbType.VarChar);
				komanda.Parameters.Add("passParam", MySqlDbType.VarChar);

				komanda.Parameters["jmbgParam"].Value = jmbg;
				komanda.Parameters["imeParam"].Value = ime;
				komanda.Parameters["prezimeParam"].Value = prezime;
				komanda.Parameters["userParam"].Value = username;
				komanda.Parameters["passParam"].Value = password;

				komanda.ExecuteNonQuery();
				MessageBox.Show("Uspjesno slanje zahtjeva");
			}
			catch (Exception err)
			{
				MessageBox.Show("Greska pri upisu zahtjeva " + err);
			}
		}

		public void UnesiUBazu(String jmbg, String ime, String prezime, String username, String password, MySqlConnection conn)
		{
			String upit = "insert into korisnik (`JMBG`,`Ime`,`Prezime`,`Admin`,`Username`,`Password`) values " +
				"(@jmbgParam, @imeParam, @prezimeParam, 0, @userParam, @passParam)";
			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("jmbgParam", MySqlDbType.VarChar);
				komanda.Parameters.Add("imeParam", MySqlDbType.VarChar);
				komanda.Parameters.Add("prezimeParam", MySqlDbType.VarChar);
				komanda.Parameters.Add("userParam", MySqlDbType.VarChar);
				komanda.Parameters.Add("passParam", MySqlDbType.VarChar);

				komanda.Parameters["jmbgParam"].Value = jmbg;
				komanda.Parameters["imeParam"].Value = ime;
				komanda.Parameters["prezimeParam"].Value = prezime;
				komanda.Parameters["userParam"].Value = username;
				komanda.Parameters["passParam"].Value = password;

				komanda.ExecuteNonQuery();
				MessageBox.Show("Uspjesan unos u bazu");
			}
			catch (Exception err)
			{
				MessageBox.Show("Greska pri unosu korisnika " + err);
			}
		}

		public void IzbrisiZahtjev(String jmbg, MySqlConnection conn)
		{
			String upit = "SET SQL_SAFE_UPDATES=0; delete from zahtjevi where zahtjevi.JMBG = @jmbgParam; SET SQL_SAFE_UPDATES = 1; ";
			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("jmbgParam", MySqlDbType.VarChar);
				komanda.Parameters["jmbgParam"].Value = jmbg;

				komanda.ExecuteNonQuery();
				MessageBox.Show("Uspjesno brisanje iz baze");
			}
			catch (Exception err)
			{
				MessageBox.Show("Korisnik ne postoji u tabeli zahtjevi\n" + err);
				return;
			}
		}

		public void UnesiUKolekciju(String ime, String userID, DateTime datumKreiranja, String tip, MySqlConnection conn)
		{
			String upit = "insert into kolekcija (`ime`, `userID`,`datumKreiranja`,`tip`) values " +
				"(@imeParam ,@userIDParam, @datumKreiranjaParam, @tipParam)";
			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("imeParam", MySqlDbType.VarChar);
				komanda.Parameters.Add("userIDParam", MySqlDbType.VarChar);
				komanda.Parameters.Add("datumKreiranjaParam", MySqlDbType.DateTime);
				komanda.Parameters.Add("tipParam", MySqlDbType.VarChar);

				komanda.Parameters["imeParam"].Value = ime;
				komanda.Parameters["userIDParam"].Value = userID;
				komanda.Parameters["datumKreiranjaParam"].Value = datumKreiranja;
				komanda.Parameters["tipParam"].Value = tip;
				
				komanda.ExecuteNonQuery();
				MessageBox.Show("Uspjesan unos u tabelu kolekcija");
			}
			catch (Exception err)
			{
				MessageBox.Show("Greska pri unosu u kolekciju " + err);
			}
		}

		public void UnesiUSlozenuKolekciju(String ime, String userID, DateTime datumKreiranja, String tip, MySqlConnection conn)
		{
			String upit = "insert into kolekcija (`ime`, `userID`,`datumKreiranja`,`tip`, `djeca`) values " +
				"(@imeParam ,@userIDParam, @datumKreiranjaParam, @tipParam, 1)";
			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("imeParam", MySqlDbType.VarChar);
				komanda.Parameters.Add("userIDParam", MySqlDbType.VarChar);
				komanda.Parameters.Add("datumKreiranjaParam", MySqlDbType.DateTime);
				komanda.Parameters.Add("tipParam", MySqlDbType.VarChar);

				komanda.Parameters["imeParam"].Value = ime;
				komanda.Parameters["userIDParam"].Value = userID;
				komanda.Parameters["datumKreiranjaParam"].Value = datumKreiranja;
				komanda.Parameters["tipParam"].Value = tip;

				komanda.ExecuteNonQuery();
				MessageBox.Show("Uspjesan unos u tabelu kolekcija");
			}
			catch (Exception err)
			{
				MessageBox.Show("Greska pri unosu u kolekciju " + err);
			}
		}

		public void PrikaziDokumenta(DataGridView dgv, String imeKolekcije, MySqlConnection conn)
		{
			String upit = "select * from KOLEKCIJA, DOKUMENT where " +
						  "kolekcija.ID = dokument.kolekcijaID and kolekcija.ime=@imeParam";
			MySqlCommand komanda = new MySqlCommand();
			DataTable tabela = new DataTable();

			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("imeParam", MySqlDbType.VarChar);
				komanda.Parameters["imeParam"].Value = imeKolekcije;

				MySqlDataAdapter adapter = new MySqlDataAdapter();
				adapter.SelectCommand = komanda;
				adapter.Fill(tabela);
				dgv.DataSource = tabela;
				dgv.Columns["ID"].Visible = false;
				dgv.Columns["kolekcijaID"].Visible = false;
			}
			catch
			{

			}
		}

		public void UpdateKolekcije(String ime, String userID, DateTime datMod, Int32 br, Double velicina, MySqlConnection conn)
		{
			int kolID = NadjiIDKolekcije(ime, userID, conn);
			String upit = "update KOLEKCIJA set datumModifikovanja = @datModParam, brDokumenata = @brParam, velicina = @velicinaParam" +
						  " where ID = @kolIDParam";

			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("datModParam", MySqlDbType.DateTime);
				komanda.Parameters.Add("brParam", MySqlDbType.Int32);
				komanda.Parameters.Add("velicinaParam", MySqlDbType.Double);
				komanda.Parameters.Add("kolIDParam", MySqlDbType.Int32);

				komanda.Parameters["datModParam"].Value = datMod;
				komanda.Parameters["brParam"].Value = br;
				komanda.Parameters["velicinaParam"].Value = velicina;
				komanda.Parameters["kolIDParam"].Value = kolID;

				komanda.ExecuteNonQuery();
				MessageBox.Show("Uspjesan unos u bazu");
			}
			catch (Exception err)
			{
				MessageBox.Show("Greska pri unosu u tabelu kolekcija " + err);
			}
		}

		public void UnesiUDokument(String ime, int kolekcijaID, MySqlConnection conn)
		{
			String upit = "insert into dokument (`ime`, `kolekcijaID`) values " +
							"(@imeParam ,@kolekcijaIDParam)";
			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("imeParam", MySqlDbType.VarChar);
				komanda.Parameters.Add("kolekcijaIDParam", MySqlDbType.Int32);

				komanda.Parameters["imeParam"].Value = ime;
				komanda.Parameters["kolekcijaIDParam"].Value = kolekcijaID;

				komanda.ExecuteNonQuery();
				MessageBox.Show("Uspjesan unos u tabelu dokument");
			}
			catch (Exception err)
			{
				MessageBox.Show("Greska pri unosu u dooooookument " + err);
			}
		}

		public int NadjiIDKolekcije(String ime, String userID, MySqlConnection conn)
		{
			String upit = "select id from kolekcija where ime = @imeParam and userID = @userParam";
			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("imeParam", MySqlDbType.VarChar);
				komanda.Parameters["imeParam"].Value = ime;
				komanda.Parameters.Add("userParam", MySqlDbType.VarChar);
				komanda.Parameters["userParam"].Value = userID;

				MySqlDataReader reader = komanda.ExecuteReader();
				reader.Read();
				int podatak = Int32.Parse(reader["id"].ToString());
				reader.Close();

				return podatak;
			}
			catch (Exception err)
			{
				MessageBox.Show("Greska " + err);
				return 2; //ako pukne program
			}
		}

		public int NadjiChildKolekcije(int ID, MySqlConnection conn)
		{
			String upit = "select djeca from kolekcija where ID = @IDParam";
			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("IDParam", MySqlDbType.Int32);
				komanda.Parameters["IDParam"].Value = ID;

				MySqlDataReader reader = komanda.ExecuteReader();
				reader.Read();
				int podatak = Int32.Parse(reader["djeca"].ToString());
				reader.Close();

				return podatak;
			}
			catch (Exception err)
			{
				MessageBox.Show("Greska " + err);
				return 2; //ako pukne program
			}
		}

		public double NadjiVelicinu(int kolekcijaID, MySqlConnection conn)
		{
			String upit = "select velicina from kolekcija where id = @idParam";
			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("idParam", MySqlDbType.Int32);
				komanda.Parameters["idParam"].Value = kolekcijaID;

				MySqlDataReader reader = komanda.ExecuteReader();
				reader.Read();
				double podatak = Double.Parse(reader["velicina"].ToString());
				reader.Close();

				return podatak;
			}
			catch (Exception err)
			{
				MessageBox.Show("Greska " + err);
				return 2; //ako pukne program
			}
		}

		public void BrisiDokument(String ime, int kolekcijaID, MySqlConnection conn)
		{
			String upit = "SET SQL_SAFE_UPDATES=0; delete from dokument where ime = @imeParam and kolekcijaID = @idParam; SET SQL_SAFE_UPDATES = 1; ";
			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("imeParam", MySqlDbType.VarChar);
				komanda.Parameters["imeParam"].Value = ime;
				komanda.Parameters.Add("idParam", MySqlDbType.VarChar);
				komanda.Parameters["idParam"].Value = kolekcijaID;

				komanda.ExecuteNonQuery();
				MessageBox.Show("Uspjesno brisanje iz tabele dokument");
			}
			catch (Exception err)
			{
				MessageBox.Show("Greska u brisanju" +
					"\n" + err);
				return;
			}
		}

		public int ProvjeriLocked(int kolekcijaID, MySqlConnection conn)
		{
			String upit = "select zakljucana from kolekcija where id = @idParam";
			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("idParam", MySqlDbType.Int32);
				komanda.Parameters["idParam"].Value = kolekcijaID;

				MySqlDataReader reader = komanda.ExecuteReader();
				reader.Read();
				int podatak = Int32.Parse(reader["zakljucana"].ToString());
				reader.Close();

				return podatak;

			}
			catch (Exception err)
			{
				MessageBox.Show("Greska " + err);
				return 2; //ako pukne program
			}
		}

		public void ZakljucajKolekciju(int id, int locked, MySqlConnection conn)
		{
			String upit = "update KOLEKCIJA set zakljucana = @lockedParam where ID = @idParam";

			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("idParam", MySqlDbType.Int32);
				komanda.Parameters.Add("lockedParam", MySqlDbType.Int32);

				komanda.Parameters["idParam"].Value = id;
				komanda.Parameters["lockedParam"].Value = locked;

				komanda.ExecuteNonQuery();
				MessageBox.Show("Uspjesan update");
			}
			catch (Exception err)
			{
				MessageBox.Show("Greska pri updateovanju " + err);
			}
		}

		public void PregledKolekcija(String userID, ComboBox cb, MySqlConnection conn)
		{
			String upit = "select ime from kolekcija where userID = @userIDParam";
			MySqlCommand komanda = new MySqlCommand();
			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("userIDParam", MySqlDbType.VarChar);
				komanda.Parameters["userIDParam"].Value = userID;

				MySqlDataReader reader = komanda.ExecuteReader();

				while (reader.Read())
				{
					cb.Items.Add(reader["ime"].ToString());
				}
				
				reader.Close();

			}
			catch (Exception err)
			{
				MessageBox.Show("Greska " + err);
			}
		}

		public void TipKolekcije(String userID, String ime, ComboBox cb, MySqlConnection conn)
		{
			int id = NadjiIDKolekcije(ime, userID, conn);
			int child = NadjiChildKolekcije(id, conn);
			if (child == 0)
			{
				String upit = "select tip from kolekcija where userID = @userIDParam and ime = @imeParam";
				MySqlCommand komanda = new MySqlCommand();
				try
				{
					komanda.CommandText = upit;
					komanda.Connection = conn;

					komanda.Parameters.Add("userIDParam", MySqlDbType.VarChar);
					komanda.Parameters.Add("imeParam", MySqlDbType.VarChar);

					komanda.Parameters["userIDParam"].Value = userID;
					komanda.Parameters["imeParam"].Value = ime;

					MySqlDataReader reader = komanda.ExecuteReader();

					reader.Read();
					cb.Text = reader["tip"].ToString();
					reader.Close();

				}
				catch (Exception err)
				{
					MessageBox.Show("Greska " + err);
				}
			}
			else
			{
				cb.SelectedIndex = -1;
			}
			
		}

		public void KorisnikoveKolekcije(String uID, DataGridView dgv, MySqlConnection conn)
		{
			String upit = "select ime, tip from KOLEKCIJA where " +
						  "userID = @uIDParam";
			MySqlCommand komanda = new MySqlCommand();
			DataTable tabela = new DataTable();

			try
			{
				komanda.CommandText = upit;
				komanda.Connection = conn;

				komanda.Parameters.Add("uIDParam", MySqlDbType.VarChar);
				komanda.Parameters["uIDParam"].Value = uID;

				MySqlDataAdapter adapter = new MySqlDataAdapter();
				adapter.SelectCommand = komanda;
				adapter.Fill(tabela);
				dgv.DataSource = tabela;
				dgv.Columns["tip"].Visible = false;
			}
			catch
			{

			}
		}

	}
}
