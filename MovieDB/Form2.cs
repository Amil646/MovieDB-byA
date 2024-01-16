using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace MovieDB
{
    public partial class Form2 : Form
    {
        public string ratings, director, title, actors;
        public int movieID;

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public Form2()
        { 
            InitializeComponent();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = title;
            textBox2.Text = director;
            textBox4.Text = actors;
            textBox3.Text = ratings;
          
        }

        #region Update
        private void button6_Click(object sender, EventArgs e)
        {
            //sql query 
            Form1 f1 = new Form1();
            string typeString;
            title = textBox1.Text.ToString();
            director = textBox2.Text.ToString();
            actors = textBox4.Text.ToString();
            ratings = textBox3.Text.ToString();
            int yr = 0;
            if (ratings != "")
            {
                yr = f1.CheckYear(ratings);
            }
            try
            {
            }
            catch (Exception ex) {
                MessageBox.Show("You need to select movie type! \nError: " + ex.Message + "");
                return;
            }
           
          
                if (year == "")
                {
                    SQLUpdateString = "UPDATE movie SET Title ='" + title.Replace("'", "''") + "',, Director='" + director + "', Actors='" + actors + "' WHERE movieID=" + movieID + "";
                }
                else
                {
                    SQLUpdateString = "UPDATE movie SET Title ='" + title.Replace("'", "''") + "', Ratings=" + yr + ", Director='" + director + "', Actors='" + actors + "' WHERE movieID=" + movieID + "";
                }
                OleDbCommand SQLCommand = new OleDbCommand();
                SQLCommand.CommandText = SQLUpdateString;
                SQLCommand.Connection = f1.database;
                int response = SQLCommand.ExecuteNonQuery();
                MessageBox.Show("Update successful!","Message",MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else 
            {
                MessageBox.Show("The rating format is not correct!\n", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Clear();
                textBox3.Focus();
            }
        }
        #endregion

        private void button6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button6_Click(null, null);
            }
        }

    }
}