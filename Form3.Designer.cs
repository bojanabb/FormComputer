namespace ProjekatZavrsni
{
	partial class Form3
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dataGridViewSpisak = new System.Windows.Forms.DataGridView();
			this.btnNazad = new System.Windows.Forms.Button();
			this.btnDodajDok = new System.Windows.Forms.Button();
			this.btnBrisiDok = new System.Windows.Forms.Button();
			this.btnDalje = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewSpisak)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridViewSpisak
			// 
			this.dataGridViewSpisak.AllowUserToAddRows = false;
			this.dataGridViewSpisak.AllowUserToDeleteRows = false;
			this.dataGridViewSpisak.AllowUserToResizeColumns = false;
			this.dataGridViewSpisak.AllowUserToResizeRows = false;
			this.dataGridViewSpisak.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridViewSpisak.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewSpisak.Location = new System.Drawing.Point(1, 1);
			this.dataGridViewSpisak.Name = "dataGridViewSpisak";
			this.dataGridViewSpisak.Size = new System.Drawing.Size(798, 307);
			this.dataGridViewSpisak.TabIndex = 0;
			// 
			// btnNazad
			// 
			this.btnNazad.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnNazad.Font = new System.Drawing.Font("Palatino Linotype", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.btnNazad.Location = new System.Drawing.Point(62, 363);
			this.btnNazad.Name = "btnNazad";
			this.btnNazad.Size = new System.Drawing.Size(121, 41);
			this.btnNazad.TabIndex = 1;
			this.btnNazad.Text = "Nazad";
			this.btnNazad.UseVisualStyleBackColor = true;
			this.btnNazad.Click += new System.EventHandler(this.btnNazad_Click);
			// 
			// btnDodajDok
			// 
			this.btnDodajDok.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnDodajDok.Font = new System.Drawing.Font("Palatino Linotype", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.btnDodajDok.Location = new System.Drawing.Point(235, 363);
			this.btnDodajDok.Name = "btnDodajDok";
			this.btnDodajDok.Size = new System.Drawing.Size(121, 41);
			this.btnDodajDok.TabIndex = 2;
			this.btnDodajDok.Text = "Dodaj dokumenta";
			this.btnDodajDok.UseVisualStyleBackColor = true;
			this.btnDodajDok.Click += new System.EventHandler(this.btnDodajDok_Click);
			// 
			// btnBrisiDok
			// 
			this.btnBrisiDok.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnBrisiDok.Font = new System.Drawing.Font("Palatino Linotype", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.btnBrisiDok.Location = new System.Drawing.Point(412, 363);
			this.btnBrisiDok.Name = "btnBrisiDok";
			this.btnBrisiDok.Size = new System.Drawing.Size(121, 41);
			this.btnBrisiDok.TabIndex = 3;
			this.btnBrisiDok.Text = "Brisi dokumenta";
			this.btnBrisiDok.UseVisualStyleBackColor = true;
			this.btnBrisiDok.Click += new System.EventHandler(this.btnBrisiDok_Click);
			// 
			// btnDalje
			// 
			this.btnDalje.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnDalje.Font = new System.Drawing.Font("Palatino Linotype", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.btnDalje.Location = new System.Drawing.Point(582, 363);
			this.btnDalje.Name = "btnDalje";
			this.btnDalje.Size = new System.Drawing.Size(121, 41);
			this.btnDalje.TabIndex = 4;
			this.btnDalje.Text = "Dalje";
			this.btnDalje.UseVisualStyleBackColor = true;
			this.btnDalje.Click += new System.EventHandler(this.btnDalje_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Multiselect = true;
			// 
			// Form3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.btnDalje);
			this.Controls.Add(this.btnBrisiDok);
			this.Controls.Add(this.btnDodajDok);
			this.Controls.Add(this.btnNazad);
			this.Controls.Add(this.dataGridViewSpisak);
			this.Name = "Form3";
			this.Text = "Form3";
			this.Load += new System.EventHandler(this.Form3_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewSpisak)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridViewSpisak;
		private System.Windows.Forms.Button btnNazad;
		private System.Windows.Forms.Button btnDodajDok;
		private System.Windows.Forms.Button btnBrisiDok;
		private System.Windows.Forms.Button btnDalje;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
	}
}