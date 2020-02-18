using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IKYS
{
    class FormKFunctions
    {
        //GRİD TİTLE FUNC
        public static void gridHeaderChange(DataGridView grid, int ctrl)
        {
            if (ctrl == 1)
            {
                grid.Columns[0].HeaderText = "Departman";
                grid.Columns[1].HeaderText = "İsim";
                grid.Columns[2].HeaderText = "Soyİsim";
                grid.Columns[3].HeaderText = "Telefon Numarası";
                grid.Columns[4].HeaderText = "Sosyal Güvenlik Numarası";
                grid.Columns[5].HeaderText = "Adres";
                grid.Columns[6].HeaderText = "Cinsiyet";
                grid.Columns[7].HeaderText = "E-Mail";
            }
            else if (ctrl == 2)
            {
                grid.Columns[0].HeaderText = "Departman";
                grid.Columns[1].HeaderText = "İsim";
                grid.Columns[2].HeaderText = "Soyİsim";
                grid.Columns[3].HeaderText = "Telefon Numarası";
                grid.Columns[4].HeaderText = "Sosyal Güvenlik Numarası";
                grid.Columns[5].HeaderText = "Adres";
                grid.Columns[6].HeaderText = "Cinsiyet";
                grid.Columns[7].HeaderText = "E-Mail";
                grid.Columns[8].HeaderText = "İzne Çıkış Tarihi";
                grid.Columns[9].HeaderText = "İzin Bitiş Tarihi";
            }
            else if (ctrl == 3)
            {
                grid.Columns[0].HeaderText = "Dilekçe Tarihi";
                grid.Columns[1].HeaderText = "İsim";
                grid.Columns[2].HeaderText = "Soyİsim";
                grid.Columns[3].HeaderText = "Dilekçe Metni";
            }
            else
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
            for (int i = 0; i <= grid.Columns.Count - 1; i++)
            {
                grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        //LOG FONKSİYON
        public static void LogFonk(int id, string ad, string soyad, string updLog)
        {
            using (IKYSEntities db = new IKYSEntities())
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
        public static List<Calisanlar> calisanlarFilter(CheckBox checkK, CheckBox checkE, TextBox txtSearch, List<Calisanlar> kaynak)
        {
            using (IKYSEntities db = new IKYSEntities())
            {
                string t1, t2;
                int s;
                string ctrl = "E";
                if (checkK.Checked == true)
                {
                    checkE.Checked = false;
                    ctrl = "K";
                }
                if(txtSearch.Text != "")
                {
                    s = txtSearch.Text.Split(' ').Count();
                    if(s == 2)
                    {
                        t1 = txtSearch.Text.Split(' ').First();
                        t2 = txtSearch.Text.Split(' ')[1];
                        kaynak = kaynak.Where(c => c.ad == t1 && c.soyad == t2).ToList();
                    }
                    else
                    {
                        t1 = txtSearch.Text;
                        kaynak = kaynak.Where(c => c.ad == t1 || c.soyad == t1).ToList();
                    }
                }
                if(checkE.Checked == true || checkK.Checked == true)
                {
                    kaynak = kaynak.Where(c => c.cinsiyet == ctrl).ToList();
                }
                if(txtSearch.Text == "" && checkE.Checked == false && checkK.Checked == false)
                {
                    MessageBox.Show("Lütfen En Az Bir Filtre Seçin", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return kaynak;
            }
        }

        //İZİNLİLER FİLTRE
        public static List<Izinliler> izinlilerFilter(CheckBox checkKVac, CheckBox checkEVac, TextBox txtSearchVac, DateTimePicker dtBas, DateTimePicker dtBit, 
                                                      int dtCtrl, List<Izinliler> kaynak)
        {
            using (var db = new IKYSEntities())
            {
                string t1, t2, ibas, ibit;
                int s;
                string ctrl = "E";
                if (checkKVac.Checked == true)
                {
                    checkEVac.Checked = false;
                    ctrl = "K";
                }
                if(txtSearchVac.Text != "")
                {
                    s = txtSearchVac.Text.Split(' ').Count();
                    if(s == 2)
                    {
                        t1 = txtSearchVac.Text.Split(' ').First();
                        t2 = txtSearchVac.Text.Split(' ')[1];
                        kaynak = kaynak.Where(c => c.ad == t1 && c.soyad == t2).ToList();
                    }
                    else
                    {
                        t1 = txtSearchVac.Text;
                        kaynak = kaynak.Where(c => (c.ad == t1 || c.soyad == t1)).ToList();
                    }
                }
                if(checkEVac.Checked == true || checkKVac.Checked == true)
                {
                    kaynak = kaynak.Where(c => c.cinsiyet == ctrl).ToList();
                }
                if(dtCtrl != 0)
                {
                    if(dtCtrl == 1)
                    {
                        ibas = dtBas.Value.ToShortDateString();
                        kaynak = kaynak.Where(c => c.izinBas == ibas).ToList();
                    }
                    else if(dtCtrl == 2)
                    {
                        ibit = dtBit.Value.ToShortDateString();
                        kaynak = kaynak.Where(c => c.izinBit == ibit).ToList();
                    }
                    else
                    {
                        ibas = dtBas.Value.ToShortDateString();
                        ibit = dtBit.Value.ToShortDateString();
                        kaynak = kaynak.Where(c => c.izinBas == ibas && c.izinBit == ibit).ToList();
                    }
                }
                if(txtSearchVac.Text == "" && checkEVac.Checked == false && checkKVac.Checked == false && dtCtrl == 0)
                {
                    MessageBox.Show("Lütfen En Az Bir Filtre Seçin", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return kaynak;
            }
        }

        //DİLEKÇELER FİLTRE
        public static List<Dilekceler> dilekcelerFilter(TextBox txtSearchDil, DateTimePicker dtDil, int dtDilCtrl)
        {
            using (IKYSEntities db = new IKYSEntities())
            {
                List<Dilekceler> kaynak;
                string t1, t2, tarih;
                int s;

                if (txtSearchDil.Text != "" && dtDilCtrl != 0)
                {
                    tarih = dtDil.Value.ToShortDateString();
                    s = txtSearchDil.Text.Split(' ').Count();
                    if (s == 2)
                    {
                        t1 = txtSearchDil.Text.Split(' ').First();
                        t2 = txtSearchDil.Text.Split(' ')[1];
                        kaynak = db.Dilekcelers.Where(c => c.ad == t1 && c.soyad == t2 && c.tarih == tarih).ToList();
                        return kaynak;
                    }
                    else
                    {
                        t1 = txtSearchDil.Text;
                        kaynak = db.Dilekcelers.Where(c => (c.ad == t1 || c.soyad == t1) && c.tarih == tarih).ToList();
                        return kaynak;
                    }
                }
                else if (txtSearchDil.Text != "" && dtDilCtrl == 0)
                {
                    s = txtSearchDil.Text.Split(' ').Count();
                    if (s == 2)
                    {
                        t1 = txtSearchDil.Text.Split(' ').First();
                        t2 = txtSearchDil.Text.Split(' ')[1];
                        kaynak = db.Dilekcelers.Where(c => c.ad == t1 && c.soyad == t2).ToList();
                        return kaynak;
                    }
                    else
                    {
                        t1 = txtSearchDil.Text;
                        kaynak = db.Dilekcelers.Where(c => (c.ad == t1 || c.soyad == t1)).ToList();
                        return kaynak;
                    }
                }
                else if (txtSearchDil.Text == "" && dtDilCtrl != 0)
                {
                    tarih = dtDil.Value.ToShortDateString();
                    kaynak = db.Dilekcelers.Where(c => c.tarih == tarih).ToList();
                    return kaynak;
                }
                else
                {
                    MessageBox.Show("Lütfen En Az Bir Filtre Seçin", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    kaynak = db.Dilekcelers.ToList();
                    return kaynak;
                }
            }
        }

        //BAŞVURULAR FİLTRE
        public static List<Basvurular> basvurularFilter(TextBox txtSearchApp, CheckBox checkEApp, CheckBox checkKApp, DateTimePicker dtApp, 
                                                        ComboBox cbAppED, ComboBox cbAppMU, int dtAppCtrl)
        {
            using(IKYSEntities db = new IKYSEntities())
            {
                List<Basvurular> kaynak = db.Basvurulars.ToList();
                string t1, t2, tarih;
                int s;
                string ctrl = "E";
                if (checkKApp.Checked == true)
                {
                    checkEApp.Checked = false;
                    ctrl = "K";
                }
                if (txtSearchApp.Text != "")
                {
                    s = txtSearchApp.Text.Split(' ').Count();
                    if(s == 2)
                    {
                        t1 = txtSearchApp.Text.Split(' ').First();
                        t2 = txtSearchApp.Text.Split(' ')[1];
                        kaynak = kaynak.Where(c => c.basAd == t1 && c.basSoyad == t2).ToList();
                    }
                    else
                    {
                        t1 = txtSearchApp.Text;
                        kaynak = kaynak.Where(c => (c.basAd == t1 || c.basSoyad == t1)).ToList();
                    }
                }
                if(checkEApp.Checked == true || checkKApp.Checked == true)
                {
                    kaynak = kaynak.Where(c => c.cinsiyet == ctrl).ToList();
                }
                if(dtAppCtrl != 0)
                {
                    tarih = dtApp.Value.ToShortDateString();
                    kaynak = kaynak.Where(c => c.tarih == tarih).ToList();
                }
                if(cbAppED.Text != "Eğitim Durumu Seçin")
                {
                    kaynak = kaynak.Where(c => c.egitimDurum == cbAppED.Text).ToList();
                    if(cbAppMU.Text != "Üniversite Seçin")
                    {
                        kaynak = kaynak.Where(c => c.mezunOkul == cbAppMU.Text).ToList();
                    }
                }
                if(txtSearchApp.Text == "" && checkEApp.Checked == false && checkKApp.Checked == false && dtAppCtrl == 0 && cbAppED.Text == "Eğitim Durumu Seçin")
                    MessageBox.Show("Lütfen En Az Bir Filtre Seçin", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kaynak;
            }
        }

        //DEPARTMAN BUTTONS
        public static List<Calisanlar> departmanFilter(string depName)
        {
            using (IKYSEntities db = new IKYSEntities())
            {
                List<Calisanlar> kaynakC = db.Calisanlars.Where(c => c.departman == depName).ToList();
                if (kaynakC.Count() == 0)
                {
                    MessageBox.Show("Departmana Kayıtlı Bir Çalışan Bulunamadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return db.Calisanlars.ToList();
                }
                else
                {
                    return kaynakC;
                }            
            }
        }
        public static List<Izinliler> departmanFilterIz(string depName)
        {
            using(IKYSEntities db = new IKYSEntities())
            {
                List<Izinliler> kaynakI = db.Izinlilers.Where(c => c.departman == depName).ToList();
                if (kaynakI.Count() == 0)
                {
                    MessageBox.Show("Departmana Kayıtlı Bir Çalışan Bulunamadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return db.Izinlilers.ToList();
                }
                else
                {
                    return kaynakI;
                }
            }
        }

    }
}
