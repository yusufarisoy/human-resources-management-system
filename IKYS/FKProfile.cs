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
    public partial class FKProfile : Form
    {
        public FKProfile()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        IKYSEntities db = new IKYSEntities();
        private void FKProfile_Load(object sender, EventArgs e)
        {
            var user = db.Kullanicis.Where(c => c.id == Form1.id).First();
            this.Text = Form1.ad + " " + Form1.soyad;
            pbProfile.Image = Image.FromFile(Form1.photo);
            label1.Text = Form1.ad + " " + Form1.soyad;
            label2.Text = "Telefon : " + user.telNo;
            label5.Text = "Adres : " + user.adres;
            label6.Text = "Şifre :";

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            label3.Visible = false;
            label4.Visible = false;
        }
        private int ctrl = 0;
        private string updLog;
        private void button1_Click(object sender, EventArgs e)
        {
            updLog = "";
            if(ctrl == 0)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                button1.Text = "Kaydet";
                ctrl = 1; 
                label3.Visible = true;
                label4.Visible = true;
            }
            else
            {

                updLog = "Kendi Bilgilerini Güncelledi";
                int id = Form1.id;
                var user = db.Kullanicis.Where(c => c.id == id).First();
                if(textBox1.Text != "")
                {
                    user.telNo = textBox1.Text;
                    label2.Text = "Telefon : " + textBox1.Text;
                }
                if (textBox2.Text != "")
                {
                    user.adres = textBox2.Text;
                    label5.Text = "Adres : " + textBox2.Text;
                }
                if (textBox3.Text != "" && textBox3.Text == user.sifre)
                {
                    user.sifre = textBox4.Text;
                    updLog = "Kendi Şifresini Güncelledi !";
                }
                else if(textBox3.Text != "" && textBox3.Text != user.sifre)
                    MessageBox.Show("Mevcut Şifrenizi Yanlış Girdiniz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                try
                {
                    db.SaveChanges();
                    MessageBox.Show("İşlem Başarıyla Gerçekleşti", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                }
                catch (ArgumentNullException)
                {
                    MessageBox.Show("Hiçbir Değer Girmediniz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                ctrl = 0;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                label3.Visible = false;
                label4.Visible = false;
                button1.Text = "Bilgileri Güncelle";
            }
        }

    }
}
