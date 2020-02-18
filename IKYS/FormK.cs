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
    public partial class FormK : Form
    {
        //DB BAĞLANTI
        readonly IKYSEntities db = new IKYSEntities();
        private int dtCtrl = 0, dtDilCtrl = 0, dtAppCtrl = 0, depCtrl = 0;

        public FormK()
        {
            InitializeComponent();
            this.CenterToScreen();
            dtBas.ValueChanged += dtBas_ValueChanged;
            dtBit.ValueChanged += dtBit_ValueChanged;
            dtDil.ValueChanged += dtDil_ValueChanged;
            dtApp.ValueChanged += dtApp_ValueChanged;

            dtBas.CustomFormat = "dd/MM/yyyy";
            dtBit.CustomFormat = "dd/MM/yyyy";
            dtDil.CustomFormat = "dd/MM/yyyy";

            cbAppED.Text = "Eğitim Durumu Seçin";
            cbAppED.Items.Add("Yüksek Lisans");
            cbAppED.Items.Add("Lisans");
            cbAppED.Items.Add("Lise");
            cbAppED.Items.Add("OrtaOkul");
            cbAppED.Items.Add("İlkOkul");
        }

        //FORM LOAD
        private void FormK_Load(object sender, EventArgs e)
        {
            List<Izinliler> izinliler = db.Izinlilers.ToList();
            foreach(Izinliler izinli in izinliler)
            {
                if(izinli.izinBit == DateTime.Now.ToShortDateString())
                {
                    Calisanlar cal = new Calisanlar()
                    {
                        id = izinli.id,
                        departman = izinli.departman,
                        ad = izinli.ad,
                        soyad = izinli.soyad,
                        telNo = izinli.telNo,
                        sgNo = izinli.sgNo,
                        adres = izinli.adres,
                        cinsiyet = izinli.cinsiyet,
                        mail = izinli.mail
                    };
                    db.Calisanlars.Add(cal);
                    db.Izinlilers.Remove(izinli);
                    db.SaveChanges();
                }
            }

            pbProfile.Image = Image.FromFile(Form1.photo);
            lblName.Text = Form1.ad + " " + Form1.soyad;

            gbFilterW.Visible = false;
            gbFilterVac.Visible = false;
            gbFilterDil.Visible = false;
            gbFilterApp.Visible = false;
            gbDepartments.Visible = false;
            gbUpdCal.Visible = false;
        }
        //BTN PROFİL
        private void btnProfile_Click(object sender, EventArgs e)
        {
            FKProfile fk = new FKProfile();
            fk.Show();
        }
        //BTN EXİT
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1.id = 0; Form1.ad = ""; Form1.soyad = ""; Form1.photo = "";
            Form1 f = new Form1();
            f.Show();
            this.Close();
        }


        //ÇALIŞANLAR
        List<Calisanlar> kaynakC = new List<Calisanlar>();
        private void btnWork_Click(object sender, EventArgs e)
        {
            grid.Columns.Clear();
            kaynakC = db.Calisanlars.ToList();
            grid.DataSource = kaynakC;
            grid.Columns.RemoveAt(0);
            FormKFunctions.gridHeaderChange(grid, 1);

            gbFilterW.Visible = true;
            gbFilterVac.Visible = false;
            gbFilterDil.Visible = false;
            gbFilterApp.Visible = false;
            gbDepartments.Visible = true;
            btnIzin.Visible = true;
            btnDilekce.Visible = false;
            depCtrl = 1;
            label19.Visible = true;
        }
        //ÇALIŞANLAR FİLTRE
        private void btnFilter_Click(object sender, EventArgs e)
        {
            filterControl(1);
            kaynakC = FormKFunctions.calisanlarFilter(checkK, checkE, txtSearch, kaynakC);
            if(kaynakC.Count() == 0)
                MessageBox.Show("Aradığınız Kriterlere Uygun Çalışan Bulunamadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                grid.DataSource = kaynakC;
                grid.Columns.RemoveAt(0);
            }
        }


        //İZİNLİLER
        private void btnVac_Click(object sender, EventArgs e)
        {
            kaynakI = db.Izinlilers.ToList();
            grid.Columns.Clear();
            grid.DataSource = db.Izinlilers.ToList();
            grid.Columns.RemoveAt(0);
            FormKFunctions.gridHeaderChange(grid, 2);

            gbFilterW.Visible = false;
            gbFilterVac.Visible = true;
            gbFilterDil.Visible = false;
            gbFilterApp.Visible = false;
            gbDepartments.Visible = true;
            btnIzin.Visible = false;
            btnDilekce.Visible = false;
            depCtrl = 2;
            label19.Visible = true;
        }
        //İZİNLİLER FİLTRE
        List<Izinliler> kaynakI = new List<Izinliler>();
        private void btnFilterVac_Click(object sender, EventArgs e)
        {
            filterControl(1);
            kaynakI = FormKFunctions.izinlilerFilter(checkKVac, checkEVac, txtSearchVac, dtBas, dtBit, dtCtrl, kaynakI);
            if (kaynakI.Count() == 0)
                MessageBox.Show("Aradığınız Kriterlere Uygun Çalışan Bulunamadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                grid.DataSource = kaynakI;
                grid.Columns.RemoveAt(0);
            }
            dtCtrl = 0;
        }
        //DATETİMEPİCKER İZİNLİLER FİLTERS
        private void dtBas_ValueChanged(object sender, EventArgs e)
        {
            dtCtrl += 1;
        }
        private void dtBit_ValueChanged(object sender, EventArgs e)
        {
            dtCtrl += 2;
        }


        //DİLEKÇELER
        private void btnPeti_Click(object sender, EventArgs e)
        {
            depCtrl = 3;
            grid.Columns.Clear();
            grid.DataSource = db.Dilekcelers.ToList();
            grid.Columns.RemoveAt(0);
            FormKFunctions.gridHeaderChange(grid, 3);

            gbFilterW.Visible = false;
            gbFilterVac.Visible = false;
            gbFilterDil.Visible = true;
            gbFilterApp.Visible = false;
            gbDepartments.Visible = false;
            btnIzin.Visible = false;
            btnDilekce.Visible = true;
            label19.Visible = false;
        }
        //DİLEKÇELER FİLTER
        private void btnFilterDil_Click(object sender, EventArgs e)
        {
            List<Dilekceler> kaynak = FormKFunctions.dilekcelerFilter(txtSearchDil, dtDil, dtDilCtrl);
            if (kaynak.Count() == 0)
                MessageBox.Show("Aradığınız Kriterlerde Bir Dilekçe Bulunamadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                grid.DataSource = kaynak;
                grid.Columns.RemoveAt(0);
            }
            dtDilCtrl = 0;
        }
        //DATETİMEPİCKER DİLEKÇELER FİLTER
        private void dtDil_ValueChanged(object sender, EventArgs e)
        {
            dtDilCtrl = 1;
        }
        //BTN DİLEKÇE EKLE
        private void btnDilekce_Click(object sender, EventArgs e)
        {
            gbDilekce.Visible = true;
        }
        private void btnDilekle_Click(object sender, EventArgs e)
        {
            if(txtDiltarih.Text != "" && txtDilAd.Text != "" && txtDilMetin.Text != "")
            {
                int s = txtDilAd.Text.Split(' ').Count();
                if (s == 2)
                {
                    int dilid = db.Dilekcelers.Select(c => c.id).Max();
                    string s1 = txtDilAd.Text.Split(' ').First();
                    string s2 = txtDilAd.Text.Split(' ')[1];
                    Dilekceler dilekce = new Dilekceler()
                    {
                        id = dilid + 1,
                        tarih = txtDiltarih.Text,
                        ad = s1,
                        soyad = s2,
                        icerik = txtDilMetin.Text
                    };
                    db.Dilekcelers.Add(dilekce);
                    int logid = db.Logs.Select(c => c.logid).Max();
                    string logey = s1 + " " + s2 + " İsimli Personel İçin, " + txtDiltarih.Text + " Tarihli Dilekçeyi Oluşturdu.";
                    Log log = new Log()
                    {
                        logid = logid + 1,
                        kullid = Form1.id,
                        ad = Form1.ad,
                        soyad = Form1.soyad,
                        saat = DateTime.Now,
                        eylem = logey
                    };
                    db.Logs.Add(log);
                    db.SaveChanges();
                    gbDilekce.Visible = false;
                }
                else
                {
                    label15.Visible = true;
                    label15.Text = "Ad - Soyad Girilmesi Zorunludur !";
                }
            }
            else
            {
                label15.Visible = true;
                label15.Text = "Tüm Alanların Doldurulması Zorunludur !";
            }
        }
        private void btnDilkapa_Click(object sender, EventArgs e)
        {
            gbDilekce.Visible = false;
        }



        //BAŞVURULAR
        private void btnApply_Click(object sender, EventArgs e)
        {
            depCtrl = 4;
            grid.Columns.Clear();
            grid.DataSource = db.Basvurulars.ToList();
            grid.Columns.RemoveAt(0);
            FormKFunctions.gridHeaderChange(grid, 4);

            gbFilterW.Visible = false;
            gbFilterVac.Visible = false;
            gbFilterDil.Visible = false;
            gbFilterApp.Visible = true;
            gbDepartments.Visible = false;
            btnIzin.Visible = false;
            btnDilekce.Visible = false;
            label19.Visible = false;

            lblUni.Visible = false;
            cbAppMU.Visible = false;
            btnFilterApp.Location = new Point(6, 260);
            gbFilterApp.Height = 325;
        }
        //BAŞVURULAR FİLTER
        private void btnFilterApp_Click(object sender, EventArgs e)
        {
            List<Basvurular> kaynak = FormKFunctions.basvurularFilter(txtSearchApp, checkEApp, checkKApp, dtApp, cbAppED, cbAppMU, dtAppCtrl);
            if(kaynak.Count() == 0)
                MessageBox.Show("Aradığınız Özelliklerde Başvuru Bulunamadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                grid.DataSource = kaynak;
                grid.Columns.RemoveAt(0);
            }
            dtAppCtrl = 0;
            cbAppMU.Text = "Üniversite Seçin";
        }
        //COMBOBOX BAŞVURULAR FİLTER
        private List<string> unis = new List<string>();
        private void cbAppED_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAppED.Text == "Lisans" || cbAppED.Text == "Yüksek Lisans")
            {
                gbFilterApp.Height = 373;
                btnFilterApp.Location = new Point(6,320);
                lblUni.Visible = true;
                cbAppMU.Visible = true;
                cbAppMU.Text = "Üniversite Seçin";
                List<string> allUnis = db.Basvurulars.Select(c => c.mezunOkul).ToList();
                foreach (var uni in allUnis)
                {
                    if (!(unis.Contains(uni)) && uni != null)
                    {
                        unis.Add(uni);
                        cbAppMU.Items.Add(uni);
                    }
                }
            }
            else
            {
                lblUni.Visible = false;
                cbAppMU.Visible = false;
                btnFilterApp.Location = new Point(6, 260);
                gbFilterApp.Height = 325;
            }
        }
        //DATETİMEPİCKER BASVURULAR FİLTER
        private void dtApp_ValueChanged(object sender, EventArgs e)
        {
            dtAppCtrl = 1;
        }


        //DEPARTMAN BUTTONS
        private void btnDpt1_Click(object sender, EventArgs e)
        {
            filterControl(2);
            if (depCtrl == 1)
            {
                kaynakC = FormKFunctions.departmanFilter("Halkla İlişkiler");
                grid.DataSource = kaynakC;
                grid.Columns.RemoveAt(0);
            }
            else if(depCtrl == 2)
            {
                kaynakI = FormKFunctions.departmanFilterIz("Halkla İlişkiler");
                grid.DataSource = kaynakI;
                grid.Columns.RemoveAt(0);
            }
        }
        private void btnDpt2_Click(object sender, EventArgs e)
        {
            filterControl(2);
            if (depCtrl == 1)
            {
                kaynakC = FormKFunctions.departmanFilter("Bilgi Teknolojileri");
                grid.DataSource = kaynakC;
                grid.Columns.RemoveAt(0);
            }
            else if (depCtrl == 2)
            {
                kaynakI = FormKFunctions.departmanFilterIz("Bilgi Teknolojileri");
                grid.DataSource = kaynakI;
                grid.Columns.RemoveAt(0);
            }
        }
        private void btnDpt3_Click(object sender, EventArgs e)
        {
            filterControl(2);
            if (depCtrl == 1)
            {
                kaynakC = FormKFunctions.departmanFilter("Hukuk");
                grid.DataSource = kaynakC;
                grid.Columns.RemoveAt(0);
            }
            else if (depCtrl == 2)
            {
                kaynakI = FormKFunctions.departmanFilterIz("Hukuk");
                grid.DataSource = kaynakI;
                grid.Columns.RemoveAt(0);
            }
        }
        private void btnDpt4_Click(object sender, EventArgs e)
        {
            filterControl(2);
            if (depCtrl == 1)
            {
                kaynakC = FormKFunctions.departmanFilter("Satış");
                grid.DataSource = kaynakC;
                grid.Columns.RemoveAt(0);
            }
            else if (depCtrl == 2)
            {
                kaynakI = FormKFunctions.departmanFilterIz("Satış");
                grid.DataSource = kaynakI;
                grid.Columns.RemoveAt(0);
            }
        }


        //GRİD CELL DOUBLE CLİCK
        public static string ad, soyad;
        int cal, izi;
        public static string BKad = "", BKsoyad = "", BKtel = "", BKadres = "", BKcins = "";
        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(depCtrl == 1 || depCtrl == 2)
            {
                int b = grid.CurrentCell.RowIndex;
                label10.Visible = false;
                gbUpdCal.Visible = true;
                string kulAd = grid[1, b].Value.ToString();
                string kulSoyad = grid[2, b].Value.ToString();
                if (depCtrl == 1)
                {
                    cal = db.Calisanlars.Where(c => c.ad == kulAd && c.soyad == kulSoyad).Select(c => c.id).First();
                }
                else if (depCtrl == 2)
                {
                    izi = db.Izinlilers.Where(c => c.ad == kulAd && c.soyad == kulSoyad).Select(c => c.id).First();
                }
            }
            else if(depCtrl == 4)
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
        private void button1_Click(object sender, EventArgs e)
        {
            gbUpdCal.Visible = false;
        }
        //BTN ÇALIŞAN GÜNCELLE
        private string updLog;
        private void btnUpdCal_Click(object sender, EventArgs e)
        {
            updLog = "";
            if(depCtrl == 1)
            {
                var user = db.Calisanlars.Where(c => c.id == cal).First();
                if (txtDpt.Text != "")
                    user.departman = txtDpt.Text;
                if (txtTelno.Text != "")
                    user.telNo = txtTelno.Text;
                if (txtAdres.Text != "")
                    user.adres = txtAdres.Text;
                updLog = cal.ToString() + " ID'li, " + user.ad + user.soyad + " İsimli Çalışanın Bilgilerini Güncelledi";
            }
            else if (depCtrl == 2)
            {
                var user = db.Izinlilers.Where(c => c.id == izi).First();
                if (txtDpt.Text != "")
                    user.departman = txtDpt.Text;
                if (txtTelno.Text != "")
                    user.telNo = txtTelno.Text;
                if (txtAdres.Text != "")
                    user.adres = txtAdres.Text;
                updLog = izi.ToString() + " ID'li, " + user.ad + user.soyad + " İsimli Çalışanın Bilgilerini Güncelledi";
            }
            txtDpt.Text = "";
            txtTelno.Text = "";
            txtAdres.Text = "";
            try
            {
                db.SaveChanges();
                label10.Visible = true;
                label10.Text = "İşlem Başarıyla Gerçekleşti";
                int logid = db.Logs.Select(c => c.logid).Max();
                var log = new Log()
                {
                    logid = logid+1,
                    kullid = Form1.id,
                    ad = Form1.ad,
                    soyad = Form1.soyad,
                    saat = DateTime.Now,
                    eylem = updLog
                };
                db.Logs.Add(log);
                db.SaveChanges();
                grid.Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("Bilgileri Kontrol Edin", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //BTN İZNE ÇIKAR
        private void btnIzin_Click(object sender, EventArgs e)
        {
            gbIznecikar.Visible = true;
        }
        private void btnIzinkapa_Click(object sender, EventArgs e)
        {
            gbIznecikar.Visible = false;
        }
        private void btnIzincik_Click(object sender, EventArgs e)
        {
            if(txtIzinAd.Text != "" && txtIzinBas.Text != "" && txtIzinBit.Text != "")
            {
                int s = txtIzinAd.Text.Split(' ').Count();
                if(s == 2)
                {
                    string s1 = txtIzinAd.Text.Split(' ').First();
                    string s2 = txtIzinAd.Text.Split(' ')[1];
                    Calisanlar calisan = db.Calisanlars.Where(c => c.ad == s1 && c.soyad == s2).First();
                    Izinliler izinli = new Izinliler()
                    {
                        id = calisan.id,
                        departman = calisan.departman,
                        ad = calisan.ad,
                        soyad = calisan.soyad,
                        telNo = calisan.telNo,
                        sgNo = calisan.sgNo,
                        adres = calisan.adres,
                        cinsiyet = calisan.cinsiyet,
                        mail = calisan.mail,
                        izinBas = txtIzinBas.Text,
                        izinBit = txtIzinBit.Text
                    };
                    db.Izinlilers.Add(izinli);
                    db.Calisanlars.Remove(calisan);
                    db.SaveChanges();
                    string updLog = calisan.id + " ID'li, " + calisan.ad + " " + calisan.soyad + " İsimli Çalışanın " + txtIzinBas.Text + " ile " + txtIzinBit.Text + 
                                    " Tarihleri Arasında İznini Ayarladı.";
                    int logid = db.Logs.Select(c => c.logid).Max();
                    Log log = new Log()
                    {
                        logid = logid+1,
                        kullid = Form1.id,
                        ad = Form1.ad,
                        soyad = Form1.soyad,
                        saat = DateTime.Now,
                        eylem = updLog
                    };
                    db.Logs.Add(log);
                    db.SaveChanges();
                    grid.Refresh();
                }
                else
                {
                    label11.Visible = true;
                    label11.Text = "Ad - Soyad Girmeniz Gerekmektedir !";
                }
            }
            else
            {
                label11.Visible = true;
                label11.Text = "Tüm Alanların Doldurulması Zorunludur !";
            }
        }
        private int filterctrl;
        private void filterControl(int x)
        {
            if (x == 1)
            {
                filterctrl += 2;
                if (filterctrl >= 4 && depCtrl == 1)
                {
                    kaynakC = db.Calisanlars.ToList();
                    filterctrl = 0;
                }
                else if (filterctrl == 4 && depCtrl == 2)
                {
                    kaynakI = db.Izinlilers.ToList();
                    filterctrl = 0;
                }
            }
            else
            {
                filterctrl += 1;
                if ((filterctrl == 2 || filterctrl >= 4) && depCtrl == 1)
                {
                    kaynakC = db.Calisanlars.ToList();
                    filterctrl = 0;
                }
                else if ((filterctrl == 2 || filterctrl >= 4) && depCtrl == 2)
                {
                    filterctrl = 0;
                    kaynakI = db.Izinlilers.ToList();
                }
            }
        }
    }
}
