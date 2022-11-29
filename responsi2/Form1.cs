using Npgsql;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;
using System.Windows.Forms;

namespace responsi2
{
    public partial class Form1 : Form
    {
        public string sql = null;
        private DataGridViewRow r;
        public DataTable dt;
        private NpgsqlConnection conn;
        public static NpgsqlCommand cmd;
        string connstring = "Host=Localhost;Port=2022;Username=postgres;Password=informatika;Database=responsiJuniorProject";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
            conn.Open();
            btnLoad.PerformClick();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                /*                sql = @"INSERT INTO karyawan(nama, id_dep) VALUES (:_nama, :_id_dep);";
                                cmd = new NpgsqlCommand(sql, conn);
                                cmd.Parameters.AddWithValue("_nama", txtNamaKaryawan.Text);
                                //bikin switch
                                int kodeDep = 0;
                                switch (txtDepartemen.Text)
                                {
                                    case "HR":
                                        kodeDep = 1;
                                        break;
                                    case "ENG":
                                        kodeDep = 2;
                                        break;
                                    case "DEV":
                                        kodeDep = 3;
                                        break;
                                    case "PM":
                                        kodeDep = 4;
                                        break;
                                    case "FIN":
                                        kodeDep = 5;
                                        break;
                                    default:
                                        break;
                                }
                                cmd.Parameters.AddWithValue("_id_dep", kodeDep);*/
                //id karyawan ganti integer terus dibuat increment

                sql = @"SELECT * FROM karyawan_insert(:_nama,:_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_nama", txtNamaKaryawan.Text);
                cmd.Parameters.AddWithValue("_id_dep", txtDepartemen.Text);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data user berhasil diinputkan", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //conn.Close();
                    btnLoad.PerformClick();
                    txtNamaKaryawan.Text = txtDepartemen.Text = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            dgvData.DataSource = null;
            sql = "SELECT * FROM karyawan_select()";
            cmd = new NpgsqlCommand(sql, conn);
            dt = new DataTable();
            NpgsqlDataReader rd = cmd.ExecuteReader();
            dt.Load(rd);
            dgvData.DataSource = dt;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Silahkan pilih data yang ingin diedit", "Good!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                //conn.Open();
                sql = @"SELECT * FROM karyawan_update(:_id_karyawan,:_nama,:_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value); //ganti r.Cells["_id"]
                cmd.Parameters.AddWithValue("_nama", txtNamaKaryawan.Text);
                cmd.Parameters.AddWithValue("_id_dep", txtDepartemen.Text);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data user berhasil diupdate", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //conn.Close();
                    btnLoad.PerformClick();
                    txtNamaKaryawan.Text = txtDepartemen.Text = null;
                    r = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Update FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                r = dgvData.Rows[e.RowIndex];
                txtNamaKaryawan.Text = r.Cells["_nama"].Value.ToString();
                txtDepartemen.Text = r.Cells["_id_dep"].Value.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Silahakn pilih data yang ingin dihapus", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Apakah Anda yakin ingin menghapus data " + r.Cells["_nama"].Value.ToString() + " ?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                try
                {
                    //conn.Open();
                    sql = @"SELECT * FROM karyawan_delete(:_id_karyawan)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value);
                    if ((int)cmd.ExecuteScalar() == 1)
                    {
                        MessageBox.Show("Data user berhasil dihapus", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //conn.Close();
                        btnLoad.PerformClick();
                        txtNamaKaryawan.Text = txtDepartemen.Text = null;
                        r = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Delete FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}