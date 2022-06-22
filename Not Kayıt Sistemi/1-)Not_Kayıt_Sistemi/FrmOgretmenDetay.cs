using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace _1__Not_Kayıt_Sistemi
{
    public partial class FrmOgretmenDetay : Form
    {
        public FrmOgretmenDetay()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-MKIIETU;Initial Catalog=DbNotKayıt;Integrated Security=True");
        

        private void FrmOgretmenDetay_Load(object sender, EventArgs e)
        {
            
            this.tbl_DersTableAdapter.Fill(this.dbNotKayıtDataSet.Tbl_Ders);

            LblGeçenSayisi.Text = dbNotKayıtDataSet.Tbl_Ders.Count(x => x.DURUM == true).ToString();
            LblKalanSayisi.Text = dbNotKayıtDataSet.Tbl_Ders.Count(x => x.DURUM == false).ToString();
            LblAgno.Text = dbNotKayıtDataSet.Tbl_Ders.Sum(y => y.ORTALAMA / (Convert.ToInt32(LblGeçenSayisi.Text) + Convert.ToInt32(LblKalanSayisi.Text))).ToString("#.00");
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into tbl_ders (ogrnumara,ograd,ogrsoyad) values (@p1,@p2,@p3)",baglanti);
            komut.Parameters.AddWithValue("@p1", MskNumara.Text);
            komut.Parameters.AddWithValue("@p2", TxtAd.Text);
            komut.Parameters.AddWithValue("@p3", TxtSoyad.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Öğrenci Sisteme Eklendi");
            this.tbl_DersTableAdapter.Fill(this.dbNotKayıtDataSet.Tbl_Ders);

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            MskNumara.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            TxtAd.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            TxtSoyad.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            TxtSınav1.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            TxtSınav2.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
            TxtSınav3.Text = dataGridView1.Rows[secilen].Cells[6].Value.ToString();
        }

        int gecenögrencisayisi = 0;
        int kalanogrencisayisi = 0;
        private void BtnGüncelle_Click(object sender, EventArgs e)
        {
            double ortalama, s1, s2, s3;
            string durum;
            s1 = Convert.ToDouble(TxtSınav1.Text);
            s2 = Convert.ToDouble(TxtSınav2.Text);
            s3 = Convert.ToDouble(TxtSınav3.Text);

            ortalama = (s1 + s2 + s3) / 3;
            LblOrtalama.Text = ortalama.ToString();

            if (ortalama>=50)
            {
                durum = "true";                
            }
            else
            {
                durum = "false";                
            }

            baglanti.Open();
            SqlCommand komut = new SqlCommand("update tbl_ders set ogrs1=@p1,ogrs2=@p2,ogrs3=@p3,ortalama=@p4,durum=@p5 where ogrnumara=@p6", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtSınav1.Text);
            komut.Parameters.AddWithValue("@p2", TxtSınav2.Text);
            komut.Parameters.AddWithValue("@p3", TxtSınav3.Text);
            komut.Parameters.AddWithValue("@p4", decimal.Parse(LblOrtalama.Text));
            komut.Parameters.AddWithValue("@p5", durum);
            komut.Parameters.AddWithValue("@p6", MskNumara.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Öğrenci Notları Güncellendi");
            this.tbl_DersTableAdapter.Fill(this.dbNotKayıtDataSet.Tbl_Ders);
        }
    }
}
