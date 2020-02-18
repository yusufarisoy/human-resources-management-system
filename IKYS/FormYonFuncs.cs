using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IKYS
{
    class FormYonFuncs
    {
        //GRİD TİTLE FUNC
        public static void gridHeaderChange(DataGridView grid, int ctrl)
        {
            if (ctrl == 1)
            {
                grid.Columns[0].HeaderText = "Departman";
                grid.Columns[1].HeaderText = "İsim";
                grid.Columns[2].HeaderText = "Soyİsim";
                grid.Columns[3].HeaderText = "Maaş";
                grid.Columns[4].HeaderText = "TC No";
                grid.Columns[5].HeaderText = "Telefon Numarası";
                grid.Columns[6].HeaderText = "Sosyal Güvenlik Numarası";
                grid.Columns[7].HeaderText = "Adres";
                grid.Columns[8].HeaderText = "Cinsiyet";
                grid.Columns[9].HeaderText = "E-Mail";
            }
            else if (ctrl == 2)
            {
                grid.Columns[0].HeaderText = "Departman";
                grid.Columns[1].HeaderText = "İsim";
                grid.Columns[2].HeaderText = "Soyİsim";
                grid.Columns[3].HeaderText = "Maaş";
                grid.Columns[4].HeaderText = "TC No";
                grid.Columns[5].HeaderText = "Telefon Numarası";
                grid.Columns[6].HeaderText = "Sosyal Güvenlik Numarası";
                grid.Columns[7].HeaderText = "Adres";
                grid.Columns[8].HeaderText = "Cinsiyet";
                grid.Columns[9].HeaderText = "E-Mail";
                grid.Columns[10].HeaderText = "İzne Çıkış Tarihi";
                grid.Columns[11].HeaderText = "İzin Bitiş Tarihi";
            }
            else if (ctrl == 3)
            {
                grid.Columns[0].HeaderText = "Dilekçe Tarihi";
                grid.Columns[1].HeaderText = "İsim";
                grid.Columns[2].HeaderText = "Soyİsim";
                grid.Columns[3].HeaderText = "Dilekçe Metni";
            }
            else if (ctrl == 4)
            {
                grid.Columns[0].HeaderText = "Başvuru Tarihi";
                grid.Columns[1].HeaderText = "İsim";
                grid.Columns[2].HeaderText = "Soyİsim";
                grid.Columns[3].HeaderText = "Telefon Numarası";
                grid.Columns[4].HeaderText = "İkamet Şehri";
                grid.Columns[5].HeaderText = "Açık Adres";
                grid.Columns[6].HeaderText = "Yabancı Dilleri";
                grid.Columns[7].HeaderText = "Eğitim Durumu";
                grid.Columns[8].HeaderText = "Mezun Olduğu Okul";
                grid.Columns[9].HeaderText = "Cinsiyet";
                grid.Columns[10].HeaderText = "ÖzGeçmiş";
            }
            else if(ctrl == 5)
            {
                grid.Columns[0].HeaderText = "İşlemi Yapan Kullanıcı ID";
                grid.Columns[1].HeaderText = "İsim";
                grid.Columns[2].HeaderText = "Soyİsim";
                grid.Columns[3].HeaderText = "Yapılan İşlem";
                grid.Columns[4].HeaderText = "İşlem Saati";
            }
            else if(ctrl == 6)
            {
                grid.Columns[0].HeaderText = "ID";
                grid.Columns[1].HeaderText = "Şifre";
                grid.Columns[2].HeaderText = "Resim Link";
                grid.Columns[3].HeaderText = "İsim";
                grid.Columns[4].HeaderText = "Soyİsim";
                grid.Columns[5].HeaderText = "Telefon Numarası";
                grid.Columns[6].HeaderText = "Maaş";
                grid.Columns[7].HeaderText = "Adres";
                grid.Columns[8].HeaderText = "SGK Numarası";
            }
            else if (ctrl == 7)
            {
                grid.Columns[0].HeaderText = "ID";
                grid.Columns[1].HeaderText = "Departman";
                grid.Columns[2].HeaderText = "İsim";
                grid.Columns[3].HeaderText = "Soyİsim";
                grid.Columns[4].HeaderText = "Maaş";
                grid.Columns[5].HeaderText = "TC No";
                grid.Columns[6].HeaderText = "Telefon Numarası";
                grid.Columns[7].HeaderText = "Sosyal Güvenlik Numarası";
                grid.Columns[8].HeaderText = "Adres";
                grid.Columns[9].HeaderText = "Cinsiyet";
                grid.Columns[10].HeaderText = "E-Mail";
            }
            for (int i = 0; i <= grid.Columns.Count - 1; i++)
            {
                grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        //LOG
        public static void LogFonk(int id, string ad, string soyad, string updLog)
        {
            using(IKYSEntities db = new IKYSEntities())
            {
                int logid = db.Logs.Select(c => c.logid).Max();
                Log log = new Log()
                {
                    logid = logid + 1,
                    kullid = id,
                    ad = ad,
                    soyad = soyad,
                    saat = DateTime.Now,
                    eylem = updLog
                };
                db.Logs.Add(log);
                db.SaveChanges();
            }

        }

        //ÇALIŞANLAR FİLTRE
        public static List<Calisanlar> calisanlarFilter(ComboBox cbDpt, TextBox txtAd, CheckBox checkE, CheckBox checkK, Label lblError)
        {
            using(IKYSEntities db = new IKYSEntities())
            {
                List<Calisanlar> kaynak = db.Calisanlars.ToList();
                string checkCtrl = "E";
                if(checkK.Checked == true)
                {
                    checkE.Checked = false;
                    checkCtrl = "K";
                }
                if(cbDpt.Text != "Departman Seçin")
                {
                    kaynak = kaynak.Where(c => c.departman == cbDpt.Text).ToList();
                }
                if(txtAd.Text != "")
                {
                    int s = txtAd.Text.Split(' ').Count();
                    if(s == 2)
                    {
                        string ad = txtAd.Text.Split(' ').First();
                        string soyad = txtAd.Text.Split(' ')[1];
                        kaynak = kaynak.Where(c => c.ad == ad && c.soyad == soyad).ToList();
                    }
                    else
                    {
                        kaynak = kaynak.Where(c => (c.ad == txtAd.Text || c.soyad == txtAd.Text)).ToList();
                    }
                }
                if(checkE.Checked == true || checkK.Checked == true)
                {
                    kaynak = kaynak.Where(c => c.cinsiyet == checkCtrl).ToList();
                }
                if (cbDpt.Text == "Departman Seçin" && txtAd.Text == "" && checkE.Checked == false && checkK.Checked == false)
                {
                    lblError.Visible = true;
                    lblError.Text = "Lütfen En Az Bir Filtre Seçin !";
                }
                else
                    lblError.Visible = false;
                return kaynak;
            }
        }

        //İZİNLİLER FİLTRE
        public static List<Izinliler> izinlilerFiltre(ComboBox cbDpt, TextBox txtAd, CheckBox checkE, CheckBox checkK, DateTimePicker dtpBas, DateTimePicker dtpBit, 
                                                      Label lblerror, int dtpControl)
        {
            using (IKYSEntities db = new IKYSEntities())
            {
                List<Izinliler> kaynak = db.Izinlilers.ToList();
                string checkCtrl = "E";
                if (checkK.Checked == true)
                {
                    checkE.Checked = false;
                    checkCtrl = "K";
                }
                if(cbDpt.Text != "Departman Seçin")
                    kaynak = kaynak.Where(c => c.departman == cbDpt.Text).ToList();
                if(txtAd.Text != "")
                {
                    int s = txtAd.Text.Split(' ').Count();
                    if(s == 2)
                    {
                        string ad = txtAd.Text.Split(' ').First();
                        string soyad = txtAd.Text.Split(' ')[1];
                        kaynak = kaynak.Where(c => c.ad == ad && c.soyad == soyad).ToList();
                    }
                    else
                        kaynak = kaynak.Where(c => (c.ad == txtAd.Text || c.soyad == txtAd.Text)).ToList();
                }
                if (checkE.Checked == true || checkK.Checked == true)
                    kaynak = kaynak.Where(c => c.cinsiyet == checkCtrl).ToList();
                if(dtpControl == 1)
                    kaynak = kaynak.Where(c => c.izinBas == dtpBas.Value.ToShortDateString()).ToList();
                else if(dtpControl == 2)
                    kaynak = kaynak.Where(c => c.izinBit == dtpBit.Value.ToShortDateString()).ToList();
                else if(dtpControl > 2)
                    kaynak = kaynak.Where(c => c.izinBas == dtpBas.Value.ToShortDateString() && c.izinBit == dtpBit.Value.ToShortDateString()).ToList();
                if (cbDpt.Text == "Departman Seçin" && txtAd.Text == "" && checkE.Checked == false && checkK.Checked == false && dtpControl == 0)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Lütfen En Az Bir Filtre Seçin !";
                }
                else
                    lblerror.Visible = false;
                return kaynak;
            }
        }

        //ÇALIŞAN GÜNCELLE
        public static void calisanGuncelle(int calisanController, int cal, int izi, TextBox txtGuncelleDpt, TextBox txtGuncelleTelno, TextBox txtGuncelleAdres, 
                                           TextBox txtGuncelleMaas, DataGridView grid)
        {
            using(IKYSEntities db = new IKYSEntities())
            {
                string updLog = "";
                if (calisanController == 1)
                {
                    var user = db.Calisanlars.Where(c => c.id == cal).First();
                    var auser = db.ACalisans.Where(c => c.id == cal).First();
                    if (txtGuncelleDpt.Text != "")
                        user.departman = txtGuncelleDpt.Text;
                    if (txtGuncelleTelno.Text != "")
                        user.telNo = txtGuncelleTelno.Text;
                    if (txtGuncelleAdres.Text != "")
                        user.adres = txtGuncelleAdres.Text;
                    if (txtGuncelleMaas.Text != "")
                        auser.maas = Convert.ToInt32(txtGuncelleMaas.Text);
                    updLog = cal.ToString() + " ID'li, " + user.ad + " " + user.soyad + " İsimli Çalışanın Bilgilerini Güncelledi";
                }
                else if (calisanController == 2)
                {
                    var user = db.Izinlilers.Where(c => c.id == izi).First();
                    var auser = db.ACalisans.Where(c => c.id == izi).First();
                    if (txtGuncelleDpt.Text != "")
                        user.departman = txtGuncelleDpt.Text;
                    if (txtGuncelleTelno.Text != "")
                        user.telNo = txtGuncelleTelno.Text;
                    if (txtGuncelleAdres.Text != "")
                        user.adres = txtGuncelleAdres.Text;
                    if(txtGuncelleMaas.Text != "")
                        auser.maas = Convert.ToInt32(txtGuncelleMaas.Text);
                    updLog = izi.ToString() + " ID'li, " + user.ad + " " + user.soyad + " İsimli Çalışanın Bilgilerini Güncelledi";
                }
                txtGuncelleAdres.Text = "";
                txtGuncelleDpt.Text = "";
                txtGuncelleTelno.Text = "";
                try
                {
                    db.SaveChanges();
                    LogFonk(Form1.Yid, Form1.Yad, Form1.Ysoyad, updLog);
                    if (calisanController == 1)
                    {
                        var query = (from call in db.Calisanlars
                                     join acal in db.ACalisans on call.id equals acal.id
                                     select
                                    new { call.departman, call.ad, call.soyad, acal.maas, call.telNo, call.sgNo, call.adres, call.cinsiyet, call.mail }).ToList();
                        grid.DataSource = query;
                    }
                    else
                    {
                        var query = (from izin in db.Izinlilers
                                     join acal in db.ACalisans on izin.id equals acal.id
                                     select
                                    new { izin.departman, izin.ad, izin.soyad, acal.maas, izin.telNo, izin.sgNo, izin.adres, izin.cinsiyet, izin.mail, 
                                          izin.izinBas, izin.izinBit }).ToList();
                        grid.DataSource = query;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Bilgileri Kontrol Edin", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //DİLEKÇELER FİLTRE
        public static List<Dilekceler> dilekceFiltre(DateTimePicker dtp, TextBox adSoyad, int dtpCtrl, Label lblerror)
        {
            using(IKYSEntities db = new IKYSEntities())
            {
                List<Dilekceler> kaynak = db.Dilekcelers.ToList();
                if (dtpCtrl != 0)
                    kaynak = kaynak.Where(c => c.tarih == dtp.Value.ToShortDateString()).ToList();
                if(adSoyad.Text != "")
                {
                    int s = adSoyad.Text.Split(' ').Count();
                    if (s == 2)
                    {
                        string ad = adSoyad.Text.Split(' ').First();
                        string soyad = adSoyad.Text.Split(' ')[1];
                        kaynak = kaynak.Where(c => c.ad == ad && c.soyad == soyad).ToList();
                    }
                    else
                        kaynak = kaynak.Where(c => (c.ad == adSoyad.Text || c.soyad == adSoyad.Text)).ToList();
                }
                if(dtpCtrl == 0 && adSoyad.Text == "") 
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Lütfen En Az Bir Filtre Seçin !";
                }
                else
                    lblerror.Visible = false;
                return kaynak;

            }
        }

        //BAŞVURULAR FİLTRE
        public static List<Basvurular> basvuruFiltre(TextBox txtAd, CheckBox checkE, CheckBox checkK, DateTimePicker dtp, ComboBox cbegi, ComboBox cbuni, int dtpCtrl,
                                                     Label lbl)
        {
            using(IKYSEntities db = new IKYSEntities())
            {
                List<Basvurular> kaynak = db.Basvurulars.ToList();
                string checkCtrl = "E";
                if (checkK.Checked == true)
                {
                    checkE.Checked = false;
                    checkCtrl = "K";
                }

                if (txtAd.Text != "")
                {
                    int s = txtAd.Text.Split(' ').Count();
                    if (s == 2)
                    {
                        string ad = txtAd.Text.Split(' ').First();
                        string soyad = txtAd.Text.Split(' ')[1];
                        kaynak = kaynak.Where(c => c.basAd == ad && c.basSoyad == soyad).ToList();
                    }
                    else
                        kaynak = kaynak.Where(c => (c.basAd == txtAd.Text || c.basSoyad == txtAd.Text)).ToList();
                }
                if (checkE.Checked == true || checkK.Checked == true)
                    kaynak = kaynak.Where(c => c.cinsiyet == checkCtrl).ToList();
                if (dtpCtrl != 0)
                    kaynak = kaynak.Where(c => c.tarih == dtp.Value.ToShortDateString()).ToList();
                if (cbegi.Text != "Eğitim Durumu Seçin")
                {
                    kaynak = kaynak.Where(c => c.egitimDurum == cbegi.Text).ToList();
                    if (cbuni.Text != "Üniversite Seçin")
                    {
                        kaynak = kaynak.Where(c => c.mezunOkul == cbuni.Text).ToList();
                    }
                }
                if (txtAd.Text == "" && checkE.Checked == false && checkK.Checked == false && dtpCtrl == 0 && cbegi.Text == "Eğitim Durumu Seçin")
                {
                    lbl.Visible = true;
                    lbl.Text = "Lütfen En Az Bir Filtre Seçin !";
                }
                else
                    lbl.Visible = false;
                return kaynak;
            }
        }



    }
}
