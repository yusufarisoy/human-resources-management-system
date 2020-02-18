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
    public partial class BasvuruKabul : Form
    {
        public BasvuruKabul()
        {
            InitializeComponent();
            this.CenterToScreen();
        }
        string adres, cinsiyet;
        private void BasvuruKabul_Load(object sender, EventArgs e)
        {
            if(FormK.BKad != "")
            {
                label1.Text = FormK.BKad;
                label2.Text = FormK.BKsoyad;
                label3.Text = FormK.BKtel;
                adres = FormK.BKadres;
                cinsiyet = FormK.BKcins;
            }
            else if(FormY.BKad != "")
            {
                label1.Text = FormY.BKad;
                label2.Text = FormY.BKsoyad;
                label3.Text = FormY.BKtel;
                adres = FormY.BKadres;
                cinsiyet = FormY.BKcins;
                txtMaas.Visible = true;
                txtTC.Visible = true;
                label11.Visible = true;
                label12.Visible = true;
            }
            label10.Visible = false;
        }
        private string updLog;
        private void btnApply_Click(object sender, EventArgs e)
        {
            using(IKYSEntities db = new IKYSEntities())
            {
                if (txtDpt.Text != "" && txtMail.Text != "" && txtSgk.Text != "")
                {
                    int id1 = db.Calisanlars.Select(c => c.id).Max();
                    int id2 = db.Izinlilers.Select(c => c.id).Max();
                    int id;
                    if (id1 > id2)
                        id = id1;
                    else
                        id = id2;
                    Calisanlar calis = new Calisanlar()
                    {
                        id = id+1,
                        departman = txtDpt.Text,
                        ad = label1.Text,
                        soyad = label2.Text,
                        telNo = label3.Text,
                        sgNo = txtSgk.Text,
                        adres = adres,
                        cinsiyet = cinsiyet,
                        mail = txtMail.Text
                    };
                    db.Calisanlars.Add(calis);
                    if (FormY.BKad != "")
                    {
                        ACalisan calisan = new ACalisan()
                        {
                            id = id + 1,
                            maas = Convert.ToInt32(txtMaas.Text),
                            tcNo = txtTC.Text
                        };
                        db.ACalisans.Add(calisan);
                    }
                    else
                    {
                        ACalisan calisan = new ACalisan()
                        {
                            id = id + 1,
                            maas = null,
                            tcNo = null
                        };
                        db.ACalisans.Add(calisan);
                    }
                    Basvurular bas = db.Basvurulars.Where(c => c.basAd == label1.Text && c.basSoyad == label2.Text).First();
                    db.Basvurulars.Remove(bas);
                    db.SaveChanges();
                    updLog = label1.Text + " " + label2.Text + " Adlı Kişinin İş Başvurusunu Kabul Etti ve " + txtDpt.Text + " Departmanında Göreve Aldı";
                    if (FormK.BKad != "")
                    {
                        FormKFunctions.LogFonk(Form1.id, Form1.ad, Form1.soyad, updLog);
                    }
                    else if (FormY.BKad != "")
                    {
                        FormYonFuncs.LogFonk(Form1.Yid, Form1.Yad, Form1.Ysoyad, updLog);
                    }
                    this.Close();
                }
                else
                {
                    label10.Visible = true;
                    label10.Text = "Tüm Alanların Doldurulması Zorunludur !";
                }
            }
        }
    }
}
