using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IKYS
{
    public partial class FormY : Form
    {
        public FormY()
        {
            InitializeComponent();
            this.CenterToScreen();

            dtpizBas.ValueChanged += DtpizBas_ValueChanged;
            dtpizBit.ValueChanged += DtpizBit_ValueChanged;
            dtpDilekcetarih.ValueChanged += DtpDilekcetarih_ValueChanged;
            dtpBasvuru.ValueChanged += DtpBasvuru_ValueChanged;
        }


        readonly IKYSEntities db = new IKYSEntities();
        private int dtpController = 0, calisanController = 0, dtpDilekcectrl = 0, dtpBasvuructrl = 0;
        private void FormY_Load(object sender, EventArgs e)
        {
            lblName.Text = Form1.Yad + " " + Form1.Ysoyad;
            pbProfile.Image = Image.FromFile(Form1.Yfoto);

            cbCalisanlar.Text = "Kaynak Seçin";
            cbCalisanlar.Items.Add("Çalışanlar");
            cbCalisanlar.Items.Add("İzinliler");

            comboBoxDpt.Items.Add("Satış");
            comboBoxDpt.Items.Add("Hukuk");
            comboBoxDpt.Items.Add("Halkla İlişkiler");
            comboBoxDpt.Items.Add("Bilgi Teknolojileri");

            comboDptiz.Items.Add("Satış");
            comboDptiz.Items.Add("Hukuk");
            comboDptiz.Items.Add("Halkla İlişkiler");
            comboDptiz.Items.Add("Bilgi Teknolojileri");

            cbBasvuruEg.Text = "Eğitim Durumu Seçin";
            cbBasvuruEg.Items.Add("Yüksek Lisans");
            cbBasvuruEg.Items.Add("Lisans");
            cbBasvuruEg.Items.Add("Lise");
            cbBasvuruEg.Items.Add("OrtaOkul");
            cbBasvuruEg.Items.Add("İlkOkul");
        }

        //BTN EXİT
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1.Yad = ""; Form1.Ysoyad = ""; Form1.Yfoto = ""; Form1.Yid = 0;
            Form1 f = new Form1();
            f.Show();
            this.Close();
        }
        //BTN PROFİLE
        private void btnProfile_Click(object sender, EventArgs e)
        {
            YoneticiProfile yp = new YoneticiProfile();
            yp.Show();
        }


        //COMBOBOX SEÇ
        private void cbCalisanlar_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid.Visible = true;
            grid.Columns.Clear();
            if (cbCalisanlar.Text == "Çalışanlar")
            {
                var query = (from cal in db.Calisanlars join acal in db.ACalisans on cal.id equals acal.id select 
                             new { cal.departman, cal.ad, cal.soyad, acal.maas, acal.tcNo, cal.telNo, cal.sgNo, cal.adres, cal.cinsiyet, cal.mail }).ToList();
                grid.DataSource = query;
                FormYonFuncs.gridHeaderChange(grid, 1);

                gbCalisanlarFilter.Visible = true;
                gbIzinlilerFiltre.Visible = false;
                calisanController = 1;
            }
            else if (cbCalisanlar.Text == "İzinliler")
            {
                var query = (from izin in db.Izinlilers join acal in db.ACalisans on izin.id equals acal.id select
                             new{ izin.departman, izin.ad, izin.soyad, acal.maas, acal.tcNo, izin.telNo, izin.sgNo, izin.adres, izin.cinsiyet, izin.mail, izin.izinBas, 
                                 izin.izinBit }).ToList();
                grid.DataSource = query;
                FormYonFuncs.gridHeaderChange(grid, 2);

                gbCalisanlarFilter.Visible = false;
                gbIzinlilerFiltre.Visible = true;
                calisanController = 2;
            }
            labelNot.Visible = true;
            lblDilnot.Visible = false;
            lblBasnot.Visible = false;
            gbDilekcelerFiltre.Visible = false;
            gbBasvurularFilter.Visible = false;
            btnDuzenle.Visible = false;
            gbKullidsec.Visible = false;
            gbKullDuzenle.Visible = false;
            btnCalistencikar.Visible = false;
        }

        //KULLANICILAR FİLTRE BTN
        private void btnKullFilter_Click(object sender, EventArgs e)
        {
            List<Calisanlar> kaynak = FormYonFuncs.calisanlarFilter(comboBoxDpt, textBoxAdSoyad, checkBoxErkek, checkBoxKadin, lblFilterError);
            if(kaynak.Count() == 0)
                MessageBox.Show("Aradığınız Kriterlere Uygun Çalışan Bulunamadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                grid.DataSource = kaynak;
                grid.Columns.RemoveAt(0);
            }
            comboBoxDpt.Text = "Departman Seçin";
        }

        //İZİNLİLER FİLTRE BTN
        private void btnIzFilter_Click(object sender, EventArgs e)
        {
            List<Izinliler> kaynak = FormYonFuncs.izinlilerFiltre(comboDptiz, txtAdiz, checkEiz, checkKiz, dtpizBas, dtpizBit, lblErroriz, dtpController); 
            if (kaynak.Count() == 0)
                MessageBox.Show("Aradığınız Kriterlere Uygun Çalışan Bulunamadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                grid.DataSource = kaynak;
                grid.Columns.RemoveAt(0);
            }
            dtpController = 0;
            comboDptiz.Text = "Departman Seçin";
        }
        private void DtpizBit_ValueChanged(object sender, EventArgs e)
        {
            dtpController += 2;
        }
        private void DtpizBas_ValueChanged(object sender, EventArgs e)
        {
            dtpController += 1;
        }


        //GRİD DOUBLE CLİCK
        int cal, izi;
        public static string BKad = "", BKsoyad = "", BKtel = "", BKadres = "", BKcins = "";
        private void grid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (calisanController == 1 || calisanController == 2)
            {
                int b = grid.CurrentCell.RowIndex;
                gbCalisanGuncelle.Visible = true;
                gbCalisanGuncelle.BringToFront();
                string kulAd = grid[1, b].Value.ToString();
                string kulSoyad = grid[2, b].Value.ToString();
                if (calisanController == 1)
                {
                    cal = db.Calisanlars.Where(c => c.ad == kulAd && c.soyad == kulSoyad).Select(c => c.id).First();
                }
                else if (calisanController == 2)
                {
                    izi = db.Izinlilers.Where(c => c.ad == kulAd && c.soyad == kulSoyad).Select(c => c.id).First();
                }
            }
            else if(calisanController == 3)
            {
                int b = grid.CurrentCell.RowIndex;
                string dilTarih = grid[0, b].Value.ToString();
                string dilAd = grid[1, b].Value.ToString();
                string dilSoyad = grid[2, b].Value.ToString();
                Dilekceler dilekce = db.Dilekcelers.Where(c => c.tarih == dilTarih && c.ad == dilAd && c.soyad == dilSoyad).First();
                DialogResult result = MessageBox.Show(dilTarih + " Tarihli Dilekçeyi Gerçekten Kaldırmak İstiyor Musunuz ?", "Dilekçe Kaldırma", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    db.Dilekcelers.Remove(dilekce);
                    db.SaveChanges();
                    grid.Refresh();
                    FormYonFuncs.LogFonk(Form1.Yid, Form1.Yad, Form1.Ysoyad, dilTarih + " Tarihli, " + dilAd + " " + dilSoyad + " Tarafından Yazılan Dilekçeyi Kaldırdı");
                }
            }
            else if (calisanController == 4)
            {
                int b = grid.CurrentCell.RowIndex;
                BKad = grid[1, b].Value.ToString();
                BKsoyad = grid[2, b].Value.ToString();
                BKtel = grid[3, b].Value.ToString();
                BKadres = grid[5, b].Value.ToString();
                BKcins = grid[9, b].Value.ToString();
                BasvuruKabul BK = new BasvuruKabul();
                BK.Show();
            }
        }
        private void btnCloseUpdt_Click(object sender, EventArgs e)
        {
            gbCalisanGuncelle.Visible = false;
        }
        private void btnGuncelleCal_Click(object sender, EventArgs e)
        {
            FormYonFuncs.calisanGuncelle(calisanController, cal, izi, txtGuncelleDpt, txtGuncelleTelno, txtGuncelleAdres, txtGuncelleMaas, grid);
            gbCalisanGuncelle.Visible = false;
        }


        //BTN DİLEKÇELER
        private void btnDilekce_Click(object sender, EventArgs e)
        {
            lblDilnot.Visible = true;
            labelNot.Visible = false;
            lblBasnot.Visible = false;
            gbKullidsec.Visible = false;
            gbKullDuzenle.Visible = false;
            btnCalistencikar.Visible = false;

            grid.Columns.Clear();
            grid.Visible = true;
            calisanController = 3;
            List<Dilekceler> kaynak = db.Dilekcelers.ToList();
            grid.DataSource = kaynak;
            grid.Columns.RemoveAt(0);
            FormYonFuncs.gridHeaderChange(grid, 3);

            gbCalisanlarFilter.Visible = false;
            gbIzinlilerFiltre.Visible = false;
            gbDilekcelerFiltre.Visible = true;
            gbBasvurularFilter.Visible = false;
            btnDuzenle.Visible = false;
        }


        //BTN DİLEKÇELER FİLTRE
        private void button1_Click(object sender, EventArgs e)
        {
            List<Dilekceler> kaynak = db.Dilekcelers.ToList();
            kaynak = FormYonFuncs.dilekceFiltre(dtpDilekcetarih, txtDilAd, dtpDilekcectrl, label12);
            if (kaynak.Count() == 0)
                MessageBox.Show("Aradığınız Kriterlerde Bir Dilekçe Bulunamadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                grid.DataSource = kaynak;
                grid.Columns.RemoveAt(0);
            }
            dtpDilekcectrl = 0;
        }
        private void DtpDilekcetarih_ValueChanged(object sender, EventArgs e)
        {
            dtpDilekcectrl = 1;
        }


        //BTN BAŞVURULAR
        private void btnBasvurular_Click(object sender, EventArgs e)
        {
            btnDuzenle.Visible = false;
            btnCalistencikar.Visible = false;
            lblDilnot.Visible = false;
            labelNot.Visible = false;
            lblBasnot.Visible = true;
            gbKullidsec.Visible = false;
            gbKullDuzenle.Visible = false;
            grid.Columns.Clear();
            grid.Visible = true;
            calisanController = 4;
            List<Basvurular> kaynak = db.Basvurulars.ToList();
            grid.DataSource = kaynak;
            grid.Columns.RemoveAt(0);
            FormYonFuncs.gridHeaderChange(grid, 4);

            gbCalisanlarFilter.Visible = false;
            gbIzinlilerFiltre.Visible = false;
            gbDilekcelerFiltre.Visible = false;
            gbBasvurularFilter.Visible = true;

            lblUni.Visible = false;
            cbBasvuruMez.Visible = false;
            btnBasvuruFilter.Location = new Point(6, 260);
            gbBasvurularFilter.Height = 325;
            lblBaserror.Location = new Point(3, 298);
        }
        private void btnBasvuruFilter_Click(object sender, EventArgs e)
        {
            List<Basvurular> kaynak = FormYonFuncs.basvuruFiltre(txtBasvuruAd, checkEbas, checkKbas, dtpBasvuru, cbBasvuruEg, cbBasvuruMez, dtpBasvuructrl, lblBaserror);
            if (kaynak.Count() == 0)
                MessageBox.Show("Aradığınız Özelliklerde Başvuru Bulunamadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                grid.DataSource = kaynak;
                grid.Columns.RemoveAt(0);
            }
            dtpBasvuructrl = 0;
            cbBasvuruMez.Text = "Üniversite Seçin";
            cbBasvuruEg.Text = "Eğitim Durumu Seçin";
        }
        private void DtpBasvuru_ValueChanged(object sender, EventArgs e)
        {
            dtpBasvuructrl = 1;
        }
        List<string> unis = new List<string>();
        private void cbBasvuruEg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBasvuruEg.Text == "Lisans" || cbBasvuruEg.Text == "Yüksek Lisans")
            {
                gbBasvurularFilter.Height = 375;
                btnBasvuruFilter.Location = new Point(6, 310);
                lblBaserror.Location = new Point(3, 348);
                lblUni.Visible = true;
                cbBasvuruMez.Visible = true;
                cbBasvuruMez.Text = "Üniversite Seçin";
                List<string> allUnis = db.Basvurulars.Select(c => c.mezunOkul).ToList();
                foreach (var uni in allUnis)
                {
                    if (!(unis.Contains(uni)) && uni != null)
                    {
                        unis.Add(uni);
                        cbBasvuruMez.Items.Add(uni);
                    }
                }
            }
            else
            {
                lblUni.Visible = false;
                cbBasvuruMez.Visible = false;
                btnBasvuruFilter.Location = new Point(6, 260);
                gbBasvurularFilter.Height = 325;
            }
        }

        //BTN LOGS
        private void btnLogkayit_Click(object sender, EventArgs e)
        {
            btnDuzenle.Visible = false;
            lblDilnot.Visible = false;
            labelNot.Visible = false;
            lblBasnot.Visible = false;
            gbKullidsec.Visible = false;
            gbKullDuzenle.Visible = false;
            btnCalistencikar.Visible = false;
            calisanController = 5;

            grid.Columns.Clear();
            grid.Visible = true;
            List<Log> loglar = db.Logs.ToList();
            grid.DataSource = loglar;
            grid.Columns.RemoveAt(0);
            FormYonFuncs.gridHeaderChange(grid, 5);

            gbCalisanlarFilter.Visible = false;
            gbIzinlilerFiltre.Visible = false;
            gbDilekcelerFiltre.Visible = false;
            gbBasvurularFilter.Visible = false;
        }


        //BTN KULLANICI DÜZENLE
        private void btnSetUser_Click(object sender, EventArgs e)
        {
            btnDuzenle.Visible = true;
            lblDilnot.Visible = false;
            labelNot.Visible = false;
            lblBasnot.Visible = false;
            gbKullidsec.Visible = false;
            gbKullDuzenle.Visible = false;
            btnCalistencikar.Visible = false;
            calisanController = 6;

            grid.Columns.Clear();
            grid.Visible = true;
            List<Kullanici> kullanicis = db.Kullanicis.ToList();
            grid.DataSource = kullanicis;
            FormYonFuncs.gridHeaderChange(grid, 6);

            gbCalisanlarFilter.Visible = false;
            gbIzinlilerFiltre.Visible = false;
            gbDilekcelerFiltre.Visible = false;
            gbBasvurularFilter.Visible = false;

        }
        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            gbKullidsec.Visible = true;
        }
        private int id;
        private void btnIdDuzenle_Click(object sender, EventArgs e)
        {
            id = Convert.ToInt32(txtId.Text);
            List<Kullanici> kull = db.Kullanicis.Where(c => c.id == id).ToList();
            if(txtId.Text != "" && kull.Count() != 0)
            {
                gbKullDuzenle.Visible = true;
                gbKullidsec.Visible = false;
            }
            else
            {
                lbliderror.Visible = true;
                lbliderror.Text = "Bu ID'de Bir Kullanıcı Yok";
            }
        }
        private void btnGbDuzenle_Click(object sender, EventArgs e)
        {
            lbliderror.Visible = false;
            Kullanici kull = db.Kullanicis.Where(c => c.id == id).First();
            string updLog = id + " ID'li, " + kull.ad + " " + kull.soyad + " İsimli Kullanıcının Bilgilerini Güncelledi";
            if (txtSifre.Text != "")
            {
                kull.sifre = txtSifre.Text;
                updLog = id + " ID'li, " + kull.ad + " " + kull.soyad + " İsimli Kullanıcının Bilgilerini ve Şifresini Güncelledi !";
            }
            if (txtAd.Text != "")
                kull.ad = txtAd.Text;
            if (txtSoyad.Text != "")
                kull.soyad = txtSoyad.Text;
            if (txtTelno.Text != "")
                kull.telNo = txtTelno.Text;
            if (txtMaas.Text != "")
                kull.maas = Convert.ToInt32(txtMaas.Text);
            if (txtAdres.Text != "")
                kull.adres = txtAdres.Text;
            if (txtSifre.Text == "" && txtAd.Text == "" && txtSoyad.Text == "" && txtTelno.Text == "" && txtMaas.Text == "" && txtAdres.Text == "")
            {
                lblkullduzerror.Visible = true;
                lblkullduzerror.Text = "En Az Bir Alan Doldurulmalıdır !";
            }
            else
                lblkullduzerror.Visible = false;
            db.SaveChanges();
            FormYonFuncs.LogFonk(Form1.Yid, Form1.Yad, Form1.Ysoyad, updLog);
            gbKullDuzenle.Visible = false;
            txtSifre.Text = ""; txtAd.Text = ""; txtSoyad.Text = ""; txtTelno.Text = ""; txtMaas.Text = ""; txtAdres.Text = ""; txtId.Text = "";
            MessageBox.Show("İşlem Başarıyla Gerçekleşti", "Onay", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //KULLANICI İŞTEN ÇIKAR
        private void btnKullcikar_Click(object sender, EventArgs e)
        {
            id = Convert.ToInt32(txtId.Text);
            List<Kullanici> kull = db.Kullanicis.Where(c => c.id == id).ToList();
            if (txtId.Text != "" && kull.Count() != 0)
            {
                DialogResult res = MessageBox.Show("Bu Kullanıcıyı İşten Çıkarmak İstediğinize Emin Misiniz ?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    Kullanici kullanici = db.Kullanicis.Where(c => c.id == id).First();
                    db.Kullanicis.Remove(kullanici);
                    db.SaveChanges();
                    FormYonFuncs.LogFonk(Form1.Yid, Form1.Yad, Form1.Ysoyad, id + " ID'li Kullanıcı İşten Çıkarıldı !");
                    MessageBox.Show(id + " ID'li Kullanıcı İşten Çıkarıldı !", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                lbliderror.Visible = true;
                lbliderror.Text = "Bu ID'de Bir Kullanıcı Yok";
            }
        }
        private void btnidkapa_Click(object sender, EventArgs e)
        {
            gbKullidsec.Visible = false;
        }


        //BTN İŞTEN ÇIKARMA
        private void btnFire_Click(object sender, EventArgs e)
        {
            gbCalisanlarFilter.Visible = false;
            gbIzinlilerFiltre.Visible = false;
            gbDilekcelerFiltre.Visible = false;
            gbBasvurularFilter.Visible = false;
            btnDuzenle.Visible = false;
            lblDilnot.Visible = false;
            labelNot.Visible = false;
            lblBasnot.Visible = false;
            gbKullidsec.Visible = false;
            gbKullDuzenle.Visible = false;
            btnCalistencikar.Visible = true;
            calisanController = 7;

            grid.Columns.Clear();
            grid.Visible = true;

            var query = (from cal in db.Calisanlars join acal in db.ACalisans on cal.id equals acal.id select
                         new { cal.id, cal.departman, cal.ad, cal.soyad, acal.maas, acal.tcNo, cal.telNo, cal.sgNo, cal.adres, cal.cinsiyet, cal.mail }).ToList();
            grid.DataSource = query;
            FormYonFuncs.gridHeaderChange(grid, 7);
        }
        //ÇALIŞAN İŞTEN ÇIKAR
        private void btnCalkapa_Click(object sender, EventArgs e)
        {
            gbCalcikar.Visible = false;
        }
        private void btnCalcikar_Click(object sender, EventArgs e)
        {
            lblcalerror.Visible = false;
            int calid = Convert.ToInt32(txtkullid.Text);
            List<Calisanlar> cal = db.Calisanlars.Where(c => c.id == calid).ToList();
            if (txtkullid.Text != "" && cal.Count() != 0)
            {
                DialogResult res = MessageBox.Show("Bu Çalışanı İşten Çıkarmak İstediğinize Emin Misiniz ?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    Calisanlar calisan = db.Calisanlars.Where(c => c.id == calid).First();
                    ACalisan acalisan = db.ACalisans.Where(c => c.id == calid).First();
                    db.Calisanlars.Remove(calisan);
                    db.ACalisans.Remove(acalisan);
                    db.SaveChanges();
                    FormYonFuncs.LogFonk(Form1.Yid, Form1.Yad, Form1.Ysoyad, calid + " ID'li, " + calisan.ad + " " + calisan.soyad + " İsimli Kullanıcı İşten Çıkarıldı !");
                    MessageBox.Show(calid + " ID'li Kullanıcı İşten Çıkarıldı !", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtkullid.Text = "";
                }
            }
            else
            {
                lblcalerror.Visible = true;
                lblcalerror.Text = "Bu ID'de Bir Kullanıcı Yok";
            }
        }
        private void btnCalistencikar_Click(object sender, EventArgs e)
        {
            gbCalcikar.Visible = true;
        }


    }
}
