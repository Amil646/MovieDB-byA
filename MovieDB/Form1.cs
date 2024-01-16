using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb; // <- for database methods

namespace MovieDB
{
    public partial class Form1 : Form
    {
        public OleDbConnection database;
        DataGridViewButtonColumn editButton;
        DataGridViewButtonColumn deleteButton;
        int movieIDInt;

        #region Form1 constructor
        public Form1()
        {

            InitializeComponent();
            // iniciate DB connection
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=moviedb.mdb";
            try
            {

                database = new OleDbConnection(connectionString);
                database.Open();
                //SQL query to list movies
                string queryString = "SELECT movieID, Title, Director, Actors, Ratings, Type FROM movie;
                loadDataGrid(queryString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        #endregion

        #region Load dataGrid
        public void loadDataGrid(string sqlQueryString) {

            OleDbCommand SQLQuery = new OleDbCommand();
            DataTable data = null;
            dataGridView1.DataSource = null;
            SQLQuery.Connection = null;
            OleDbDataAdapter dataAdapter = null;
            dataGridView1.Columns.Clear(); // <-- clear columns
            //---------------------------------
            SQLQuery.CommandText = sqlQueryString;
            SQLQuery.Connection = database;
            data = new DataTable();
            dataAdapter = new OleDbDataAdapter(SQLQuery);
            dataAdapter.Fill(data);
            dataGridView1.DataSource = data;
            dataGridView1.AllowUserToAddRows = false; // remove the null line
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 340;
            dataGridView1.Columns[3].Width = 55;
            dataGridView1.Columns[4].Width = 50;
            dataGridView1.Columns[5].Width = 80;
            // insert edit button into datagridview
            editButton = new DataGridViewButtonColumn();
            editButton.HeaderText = "Edit";
            editButton.Text = "Edit";
            editButton.UseColumnTextForButtonValue = true;
            editButton.Width = 80;
            dataGridView1.Columns.Add(editButton);
            // insert delete button to datagridview
            deleteButton = new DataGridViewButtonColumn();
            deleteButton.HeaderText = "Delete";
            deleteButton.Text = "Delete";
            deleteButton.UseColumnTextForButtonValue = true;
            deleteButton.Width = 80;
            dataGridView1.Columns.Add(deleteButton);
        }
        #endregion

        private void izlazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        #region Close database connection
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            database.Close();
        }
        #endregion

        #region refresh button
        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
            string queryString = "SELECT movieID, Title, Director, Actors, Ratings,  FROM movie,movieType WHERE movietype.typeID = movie.typeID";
            loadDataGrid(queryString);
        }
        #endregion

        #region Input
        private void button6_Click(object sender, EventArgs e)
        {
            string typeString;
            // error handling
            try
            {
                typeString = comboBox1.SelectedItem.ToString();
            }
            catch (Exception ex) {
                MessageBox.Show("You must enter movie actors\nError: " + ex.Message + "");
                return;
            }
            int yr = 0;
            string name = textBox1.Text.ToString();
            string director = textBox2.Text.ToString();
            string ratings = textBox4.Text.ToString();
            string actors = textBox3.Text.ToString();

            

                string SQLString ="";
     
                    if (ratings == "")
                    {
                        SQLString = "INSERT INTO movie(Title, Director, Actors) VALUES('" + name.Replace("'", "''") + "','" + director + "','" + actors + "'," + yr + ");";
                    }
                    else
                    {
                        MessageBox.Show(yr.ToString());
                        SQLString = "INSERT INTO movie(Title, Director, Actors,Ratings) VALUES('" + name.Replace("'", "''") + "','" + director + "','" + actors + "'," + yr + ",;";
                    }


                OleDbCommand SQLCommand = new OleDbCommand();
                SQLCommand.CommandText = SQLString;
                SQLCommand.Connection = database;
                int response = -1;
                try
                {
                    response = SQLCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (response >= 1) MessageBox.Show("Movie is added to database","Successful",MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                comboBox1.ResetText();
            }
            else
            {
                MessageBox.Show("The rating format is not correct!\n", "Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Clear();
                textBox3.Focus();
            }
        }

     

        #endregion

        #region Delete/Edit button handling
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            string queryString = "SELECT movieID, Title, Director, Actors, Ratings FROM movie;

            int currentRow = int.Parse(e.RowIndex.ToString());
            try
            {
                string movieIDString = dataGridView1[0, currentRow].Value.ToString();
                movieIDInt = int.Parse(movieIDString);
            }
            catch (Exception ex) { }
            // edit button
            if (dataGridView1.Columns[e.ColumnIndex] == editButton && currentRow >= 0)
            {
                string title = dataGridView1[1, currentRow].Value.ToString();
                string director = dataGridView1[2, currentRow].Value.ToString();
                string actors = dataGridView1[3, currentRow].Value.ToString();
                string ratings = dataGridView1[4, currentRow].Value.ToString();
                //runs form 2 for editing    
                Form2 f2 = new Form2();
                f2.title = title;
        f2.director = director;
        f2.actors = actors;
                f2.ratings = ratings;
                f2.movieID = movieIDInt;
                f2.Show();
                dataGridView1.Update();
              
            }
            // delete button
            else if (dataGridView1.Columns[e.ColumnIndex] == deleteButton && currentRow >= 0)
            {
                // delete sql query
                string queryDeleteString = "DELETE FROM movie where movieID = "+movieIDInt+"";
                OleDbCommand sqlDelete = new OleDbCommand();
                sqlDelete.CommandText = queryDeleteString;
                sqlDelete.Connection = database;
                sqlDelete.ExecuteNonQuery();
                loadDataGrid(queryString);
            }
             
         }
        #endregion
         
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region search by title
        private void button1_Click(object sender, EventArgs e)
        {
            string title = textBox1.Text.ToString();
            if (title != "")
            {
                string queryString = "SELECT movieID,  Title, Director, Actors, Ratings  FROM movie, WHERE  movie.title LIKE '" + title + "%'";
                loadDataGrid(queryString);
            }
            else
            {
                MessageBox.Show("You muste enter movie title","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
#endregion

#region search by director
private void button1_Click(object sender, EventArgs e)
{
    string director = textBox2.Text.ToString();
    if (director != "")
    {
        string queryString = "SELECT movieID,  Title, Director, Actors, Ratings  FROM movie, WHERE  movie.title LIKE '" + director + "%'";
        loadDataGrid(queryString);
    }
    else
    {
        MessageBox.Show("You muste enter movie title", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
#endregion 
#region search by actors
private void button1_Click(object sender, EventArgs e)
{
    string actors = textBox3.Text.ToString();
    if (actors != "")
    {
        string queryString = "SELECT movieID,  Title, Director, Actors, Ratings  FROM movie, WHERE  movie.title LIKE '" + actors + "%'";
        loadDataGrid(queryString);
    }
    else
    {
        MessageBox.Show("You muste enter movie title", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
#endregion 

#region search by ratings
private void button4_Click(object sender, EventArgs e)
        {
            string rt = textBox4.Text.ToString();
            int rat = CheckYear(rt);
            if ((rat != rt)
            {
                string queryString = "SELECT movieID, Title, Director, Actors, Ratings  FROM movie WHERE movie.MovieRatings;
                loadDataGrid(queryString);
            }
            else
            {
                MessageBox.Show("The year format isn't correct, pleas check again.","Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox5.Clear();
                textBox5.Focus();
                textBox6.Clear();
            }
        }
        #endregion

       
        #endregion

        private void button6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button6_Click(null, null);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string queryString = "SELECT movieID, Title, Director, Actors, Ratings FROM movie;
            loadDataGrid(queryString);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
