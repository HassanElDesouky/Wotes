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

namespace Wotes
{
    public partial class Wotes : Form
    {

        SqlConnection con = new SqlConnection(global::Wotes.Properties.Settings.Default.NotesDatabaseConnectionString);
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;

        public Wotes()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (titleBox.Text == "")
            {
                MessageBox.Show("Please enter a title for your note.", "Wotes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    cmd = new SqlCommand("INSERT INTO Notes(Title)VALUES('" + titleBox.Text + "')", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Note added successfully.", "Wotes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    handleDataTabelRefresh();
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            titleBox.Text = "";
            searchBox.Text = "";
        }

        private void handleDataTabelRefresh()
        {
            da = new SqlDataAdapter("SELECT Title FROM Notes ORDER BY SINo DESC ", con);
            dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            con.Open();
            handleDataTabelRefresh();
            con.Close();
        }

        int idx;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            idx = e.RowIndex;
            try
            {
                DataGridViewRow row = dataGridView1.Rows[idx];
                titleBox.Text = row.Cells[0].Value.ToString();
                searchBox.Text = row.Cells[0].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select a note.", "Wotes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (titleBox.Text == "" || searchBox.Text == "")
            {
                MessageBox.Show("Please enter a new title for your note.", "Wotes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    cmd = new SqlCommand("UPDATE Notes SET Title='" + titleBox.Text + "' WHERE Title='" + searchBox.Text + "'", con);
                    cmd.ExecuteNonQuery();
                    handleDataTabelRefresh();
                    MessageBox.Show("Note updated successfully.", "Wotes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            titleBox.Text = "";
            searchBox.Text = "";
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (titleBox.Text == "" || searchBox.Text == "")
            {
                MessageBox.Show("Please select a note to be deleted.", "Wotes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    cmd = new SqlCommand("DELETE FROM Notes WHERE Title='" + searchBox.Text + "'", con);
                    cmd.ExecuteNonQuery();
                    handleDataTabelRefresh();
                    MessageBox.Show("Note deleted successfully.", "Wotes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            titleBox.Text = "";
            searchBox.Text = "";
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView1.DataSource;
            bs.Filter = "Title like '%" + searchBox.Text + "%'";
            dataGridView1.DataSource = bs;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            searchBox.Text = "";
        }
    }
}
