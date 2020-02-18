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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
            txtSifre.PasswordChar = '*';
            txtSifre.MaxLength = 15;
            txtAd.MaxLength = 20;
        }
        public static string Yad, Ysoyad, Yfoto;
        public static int Yid;
        private void btnAdmin_Click(object sender, EventArgs e)
        {
            using (var db = new IKYSEntities())
            {
                var admins = db.Yoneticis.ToList();
                int control = 1; 
                if (txtAd.Text == "" || txtSifre.Text == "")
                {
                    MessageBox.Show("Alanlar Boş Bırakılamaz", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    foreach (var admin in admins)
                    {
                        if (Convert.ToInt32(txtAd.Text) == admin.id && txtSifre.Text == admin.sifre)
                        {
                            Yad = admin.ad; Ysoyad = admin.soyad; Yid = admin.id; Yfoto = admin.foto;
                            FormYonFuncs.LogFonk(admin.id, admin.ad, admin.soyad, "Sisteme Giriş Yaptı");
                            FormY f = new FormY();
                            f.Show();
                            this.Hide();
                            control = 1;
                            break;
                        }
                        else
                            control = 0;
                    }
                    if(control == 0)
                    {
                        MessageBox.Show("Kullanıcı Adı veya Şifre Yanlış", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FormYonFuncs.LogFonk(404, "UYARI", "UYARI", txtAd.Text + " ID'li Yöneticinin Hesabına, Yanlış Şifreyle Giriş Denemesi Yapıldı !");
                    }
                }
                
            }
        }

        public static string ad, soyad, photo;
        public static int id;
        private void btnGiris_Click(object sender, EventArgs e)
        {
            using (var db = new IKYSEntities())
            {
                int control = 1;
                var kullanicilar = db.Kullanicis.ToList();
                if(txtAd.Text == "" || txtSifre.Text == "")
                {
                    MessageBox.Show("Alanlar Boş Bırakılamaz", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    foreach (var kull in kullanicilar)
                    {
                        if (Convert.ToInt32(txtAd.Text) == kull.id && txtSifre.Text == kull.sifre)
                        {
                            ad = kull.ad; soyad = kull.soyad; photo = kull.resim; id = kull.id;
                            FormKFunctions.LogFonk(kull.id, kull.ad, kull.soyad, "Sisteme Giriş Yaptı");
                            FormK f = new FormK();
                            f.Show();
                            this.Hide();
                            control = 1;
                            break;
                        }
                        else
                            control = 0;
                    }
                    if(control == 0)
                    {
                        MessageBox.Show("Kullanıcı Adı veya Şifre Yanlış", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FormKFunctions.LogFonk(404, "UYARI", "UYARI", txtAd.Text + " ID'li Kullanıcının Hesabına, Yanlış Şifreyle Giriş Denemesi Yapıldı !");
                    }
                }
            }
        }
    }
}
