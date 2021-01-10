using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BD_lab7
{
    public partial class Form1 : Form
    {
        MySqlConnection mysqlConnection;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);

            await mysqlConnection.OpenAsync();

            Combo();
            Combo1();
            Combo2();
            Combo3();
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void Add_book_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            if (Error_add_book.Visible)
            {
                Error_add_book.Visible = false;
            }

            if (!string.IsNullOrEmpty(textBox_name_book.Text) && !string.IsNullOrWhiteSpace(textBox_name_book.Text) &&
                !string.IsNullOrEmpty(textBox_price_book.Text) && !string.IsNullOrWhiteSpace(textBox_price_book.Text) &&
                !string.IsNullOrEmpty(textBox_author_book.Text) && !string.IsNullOrWhiteSpace(textBox_author_book.Text) &&
                !string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MySqlCommand command1 = new MySqlCommand("SELECT id FROM database.genre WHERE name = @combo", mysqlConnection);

                command1.Parameters.AddWithValue("combo", comboBox1.Text);
                string combo_id = command1.ExecuteScalar().ToString();

                MySqlCommand command = new MySqlCommand("INSERT INTO database.book (name, price, author, genre_id)VALUES(@name, @price, @author, @combo_genre_id)", mysqlConnection);

                command.Parameters.AddWithValue("name", textBox_name_book.Text);
                command.Parameters.AddWithValue("price", textBox_price_book.Text);
                command.Parameters.AddWithValue("author", textBox_author_book.Text);
                command.Parameters.AddWithValue("combo_genre_id", combo_id);

                await command.ExecuteNonQueryAsync();

            }
            else
            {
                Error_add_book.Visible = true;
                Error_add_book.Text = "Заполните поля корректно!!!";
            }
            Rich();
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            dataGridView1.Rows.Clear();
            textBox_name_book.Clear();
            textBox_price_book.Clear();
            textBox_author_book.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            MySqlDataReader sqlReader = null;

            MySqlCommand command = new MySqlCommand("SELECT id, name, price, author FROM database.book", mysqlConnection);

            try
            {
                sqlReader = command.ExecuteReader();

                List<string[]> data = new List<string[]>();

                while (await sqlReader.ReadAsync())
                {
                    data.Add(new string[4]);

                    data[data.Count - 1][0] = sqlReader[0].ToString();
                    data[data.Count - 1][1] = sqlReader[1].ToString();
                    data[data.Count - 1][2] = sqlReader[2].ToString();
                    data[data.Count - 1][3] = sqlReader[3].ToString();
                }

                foreach (string[] s in data)
                    dataGridView1.Rows.Add(s);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            Rich();
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void Update_book_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            if (Error_update_book.Visible)
            {
                Error_update_book.Visible = false;
            }

            if (!string.IsNullOrEmpty(textBox_name_book1.Text) && !string.IsNullOrWhiteSpace(textBox_name_book1.Text) &&
                !string.IsNullOrEmpty(textBox_price_book1.Text) && !string.IsNullOrWhiteSpace(textBox_price_book1.Text) &&
                !string.IsNullOrEmpty(textBox_author_book1.Text) && !string.IsNullOrWhiteSpace(textBox_author_book1.Text) &&
                !string.IsNullOrEmpty(comboBox2.Text) && !string.IsNullOrWhiteSpace(comboBox2.Text))
            {

                MySqlCommand command1 = new MySqlCommand("SELECT id FROM database.genre WHERE name = @combo", mysqlConnection);

                command1.Parameters.AddWithValue("combo", comboBox2.Text);
                string combo_id = command1.ExecuteScalar().ToString();

                MySqlCommand command = new MySqlCommand("UPDATE database.book SET name=@name,  price=@price, author=@author, genre_id = @combo_genre_id WHERE id=@id", mysqlConnection);

                command.Parameters.AddWithValue("id", dataGridView1.CurrentRow.Cells[0].Value);
                command.Parameters.AddWithValue("name", textBox_name_book1.Text);
                command.Parameters.AddWithValue("price", textBox_price_book1.Text);
                command.Parameters.AddWithValue("author", textBox_author_book1.Text);
                command.Parameters.AddWithValue("combo_genre_id", combo_id);

                await command.ExecuteNonQueryAsync();
            }
            else
            {
                Error_update_book.Visible = true;
                Error_update_book.Text = "Заполните поля корректно!!!";
            }
            Rich();
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void Delete_book_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            if (Error_delete_book.Visible)
            {
                Error_delete_book.Visible = false;
            }
            MySqlCommand command = new MySqlCommand("DELETE FROM database.book WHERE id=@id", mysqlConnection);

            command.Parameters.AddWithValue("id", dataGridView1.CurrentRow.Cells[0].Value);

            await command.ExecuteNonQueryAsync();

            Rich();
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            if (Error_delete_book.Visible)
            {
                Error_delete_book.Visible = false;
            }

            if (!string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox4.Text))
            {
                dataGridView1.Rows.Clear();

                MySqlDataReader sqlReader = null;

                MySqlCommand command = new MySqlCommand("SELECT id, name, price, author FROM book WHERE name LIKE @name_book", mysqlConnection);

                command.Parameters.AddWithValue("name_book", textBox4.Text);

                try
                {
                    sqlReader =  command.ExecuteReader();

                List<string[]> data = new List<string[]>();
                
                while (await sqlReader.ReadAsync())
                {
                    data.Add(new string[4]);

                    data[data.Count - 1][0] = sqlReader[0].ToString();
                    data[data.Count - 1][1] = sqlReader[1].ToString();
                    data[data.Count - 1][2] = sqlReader[2].ToString();
                    data[data.Count - 1][3] = sqlReader[3].ToString();
                }

                foreach (string[] s in data)
                    dataGridView1.Rows.Add(s);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (sqlReader != null)
                        sqlReader.Close();
                }
            }
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void Btn_seller_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            dataGridView2.Rows.Clear();
            textBox_fcs_seller.Clear();
            textBox_email_seller.Clear();
            textBox_phone_seller.Clear();
            textBox_experience_seller.Clear();

            MySqlDataReader sqlReader = null;

            MySqlCommand command = new MySqlCommand("SELECT * FROM database.seller", mysqlConnection);

            try
            {
                sqlReader = command.ExecuteReader();

                List<string[]> data = new List<string[]>();

                while (await sqlReader.ReadAsync())
                {
                    data.Add(new string[5]);

                    data[data.Count - 1][0] = sqlReader[0].ToString();
                    data[data.Count - 1][1] = sqlReader[1].ToString();
                    data[data.Count - 1][2] = sqlReader[2].ToString();
                    data[data.Count - 1][3] = sqlReader[3].ToString();
                    data[data.Count - 1][4] = sqlReader[4].ToString();
                }

                foreach (string[] s in data)
                    dataGridView2.Rows.Add(s);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            Experience();
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            if (Error_add_seller.Visible)
            {
                Error_add_seller.Visible = false;
            }

            if (!string.IsNullOrEmpty(textBox_fcs_seller.Text) && !string.IsNullOrWhiteSpace(textBox_fcs_seller.Text) &&
                !string.IsNullOrEmpty(textBox_email_seller.Text) && !string.IsNullOrWhiteSpace(textBox_email_seller.Text) &&
                !string.IsNullOrEmpty(textBox_phone_seller.Text) && !string.IsNullOrWhiteSpace(textBox_phone_seller.Text) &&
                !string.IsNullOrEmpty(textBox_experience_seller.Text) && !string.IsNullOrWhiteSpace(textBox_experience_seller.Text) &&
                !string.IsNullOrEmpty(comboBox3.Text) && !string.IsNullOrWhiteSpace(comboBox3.Text))
            {
                MySqlCommand command1 = new MySqlCommand("SELECT id FROM database.shop WHERE address = @combo", mysqlConnection);

                command1.Parameters.AddWithValue("combo", comboBox3.Text);
                string combo_id = command1.ExecuteScalar().ToString();

                MySqlCommand command = new MySqlCommand("INSERT INTO database.seller (fcs, email, phone, experience, shop_id)VALUES(@fcs, @email, @phone, @experience, @combo_shop_id)", mysqlConnection);

                command.Parameters.AddWithValue("fcs", textBox_fcs_seller.Text);
                command.Parameters.AddWithValue("email", textBox_email_seller.Text);
                command.Parameters.AddWithValue("phone", textBox_phone_seller.Text);
                command.Parameters.AddWithValue("experience", textBox_experience_seller.Text);
                command.Parameters.AddWithValue("combo_shop_id", combo_id);

                await command.ExecuteNonQueryAsync();
            }
            else
            {
                Error_add_seller.Visible = true;
                Error_add_seller.Text = "Заполните поля корректно!!!";
            }
            Experience();
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            if (Error_update_seller.Visible)
            {
                Error_update_seller.Visible = false;
            }

            if (!string.IsNullOrEmpty(textBox_fcs_seller1.Text) && !string.IsNullOrWhiteSpace(textBox_fcs_seller1.Text) &&
                !string.IsNullOrEmpty(textBox_email_seller1.Text) && !string.IsNullOrWhiteSpace(textBox_email_seller1.Text) &&
                !string.IsNullOrEmpty(textBox_phone_seller1.Text) && !string.IsNullOrWhiteSpace(textBox_phone_seller1.Text) &&
                !string.IsNullOrEmpty(textBox_experience_seller1.Text) && !string.IsNullOrWhiteSpace(textBox_experience_seller1.Text) &&
                !string.IsNullOrEmpty(comboBox4.Text) && !string.IsNullOrWhiteSpace(comboBox4.Text))
            {
                MySqlCommand command1 = new MySqlCommand("SELECT id FROM database.shop WHERE address = @combo", mysqlConnection);

                command1.Parameters.AddWithValue("combo", comboBox4.Text);
                string combo_id = command1.ExecuteScalar().ToString();

                MySqlCommand command = new MySqlCommand("UPDATE database.seller SET fcs=@fcs,  email=@email, phone=@phone, experience=@experience, shop_id=@combo_shop_id WHERE id=@id", mysqlConnection);

                command.Parameters.AddWithValue("id", dataGridView2.CurrentRow.Cells[0].Value);
                command.Parameters.AddWithValue("fcs", textBox_fcs_seller1.Text);
                command.Parameters.AddWithValue("email", textBox_email_seller1.Text);
                command.Parameters.AddWithValue("phone", textBox_phone_seller1.Text);
                command.Parameters.AddWithValue("experience", textBox_experience_seller1.Text);
                command.Parameters.AddWithValue("combo_shop_id", combo_id);

                await command.ExecuteNonQueryAsync();
            }
            else
            {
                Error_update_seller.Visible = true;
                Error_update_seller.Text = "Заполните поля корректно!!!";
            }
            Experience();
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            if (Error_delete_seller.Visible)
            {
                Error_delete_seller.Visible = false;
            }
            MySqlCommand command = new MySqlCommand("DELETE FROM database.seller WHERE id=@id", mysqlConnection);

            command.Parameters.AddWithValue("id", dataGridView2.CurrentRow.Cells[0].Value);

            await command.ExecuteNonQueryAsync();

            Experience();
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            if (Error_delete_book.Visible)
            {
                Error_delete_book.Visible = false;
            }

            if (!string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text))
            {
                dataGridView2.Rows.Clear();

                MySqlDataReader sqlReader = null;

                MySqlCommand command = new MySqlCommand("SELECT * FROM seller WHERE fcs LIKE @name_seller", mysqlConnection);

                command.Parameters.AddWithValue("name_seller", textBox5.Text);

                try
                {
                    sqlReader = command.ExecuteReader();

                    List<string[]> data = new List<string[]>();

                    while (await sqlReader.ReadAsync())
                    {
                        data.Add(new string[5]);

                        data[data.Count - 1][0] = sqlReader[0].ToString();
                        data[data.Count - 1][1] = sqlReader[1].ToString();
                        data[data.Count - 1][2] = sqlReader[2].ToString();
                        data[data.Count - 1][3] = sqlReader[3].ToString();
                        data[data.Count - 1][4] = sqlReader[4].ToString();
                    }

                    foreach (string[] s in data)
                        dataGridView2.Rows.Add(s);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (sqlReader != null)
                        sqlReader.Close();
                }
            }
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                string value1 = row.Cells[1].Value.ToString();
                string value2 = row.Cells[2].Value.ToString();
                string value3 = row.Cells[3].Value.ToString();

                textBox_name_book1.Text = value1;
                textBox_price_book1.Text = value2;
                textBox_author_book1.Text = value3;
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                string value1 = row.Cells[1].Value.ToString();
                string value2 = row.Cells[2].Value.ToString();
                string value3 = row.Cells[3].Value.ToString();
                string value4 = row.Cells[4].Value.ToString();

                textBox_fcs_seller1.Text = value1;
                textBox_email_seller1.Text = value2;
                textBox_phone_seller1.Text = value3;
                textBox_experience_seller1.Text = value4;
            }
        }
        private async void Rich()
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            string sql = "SELECT name FROM book WHERE price = (SELECT MAX(price) FROM book)";
            MySqlCommand command = new MySqlCommand(sql, mysqlConnection);
            string name = command.ExecuteScalar().ToString();
            label10.Text = name;

            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void Experience()
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            string sql = "SELECT fcs FROM seller WHERE experience = (SELECT MAX(experience) FROM seller)";
            MySqlCommand command = new MySqlCommand(sql, mysqlConnection);
            string name1 = command.ExecuteScalar().ToString();
            label13.Text = name1;

            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void Combo()
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            MySqlCommand command1 = new MySqlCommand("SELECT name FROM database.genre", mysqlConnection);

            MySqlDataReader comboReader = null;

            try
            {
                comboReader = command1.ExecuteReader();

                List<string[]> data = new List<string[]>();

                while (await comboReader.ReadAsync())
                {
                    string sName = comboReader.GetString("name");
                    comboBox1.Items.Add(sName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (comboReader != null)
                    comboReader.Close();
            }
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }
        private async void Combo1()
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            MySqlCommand command1 = new MySqlCommand("SELECT name FROM database.genre", mysqlConnection);

            MySqlDataReader comboReader = null;

            try
            {
                comboReader = command1.ExecuteReader();

                List<string[]> data = new List<string[]>();

                while (await comboReader.ReadAsync())
                {
                    string sName = comboReader.GetString("name");
                    comboBox2.Items.Add(sName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (comboReader != null)
                    comboReader.Close();
            }
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void Combo2()
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            MySqlCommand command1 = new MySqlCommand("SELECT address FROM database.shop", mysqlConnection);

            MySqlDataReader comboReader = null;

            try
            {
                comboReader = command1.ExecuteReader();

                List<string[]> data = new List<string[]>();

                while (await comboReader.ReadAsync())
                {
                    string sName = comboReader.GetString("address");
                    comboBox3.Items.Add(sName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (comboReader != null)
                    comboReader.Close();
            }
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void Combo3()
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            MySqlCommand command1 = new MySqlCommand("SELECT address FROM database.shop", mysqlConnection);

            MySqlDataReader comboReader = null;

            try
            {
                comboReader = command1.ExecuteReader();

                List<string[]> data = new List<string[]>();

                while (await comboReader.ReadAsync())
                {
                    string sName = comboReader.GetString("address");
                    comboBox4.Items.Add(sName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (comboReader != null)
                    comboReader.Close();
            }
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=database;password=14tahtarov;";
            mysqlConnection = new MySqlConnection(connectionString);
            await mysqlConnection.OpenAsync();

            if (Error_delete_book.Visible)
            {
                Error_delete_book.Visible = false;
            }

            if (!string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox4.Text))
            {
                dataGridView1.Rows.Clear();

                MySqlDataReader sqlReader = null;

                MySqlCommand command = new MySqlCommand("SELECT id, name, price, author FROM book WHERE name LIKE @name_book", mysqlConnection);

                command.Parameters.AddWithValue("name_book", textBox4.Text);

                try
                {
                    sqlReader = command.ExecuteReader();

                    List<string[]> data = new List<string[]>();

                    while (await sqlReader.ReadAsync())
                    {
                        data.Add(new string[4]);

                        data[data.Count - 1][0] = sqlReader[0].ToString();
                        data[data.Count - 1][1] = sqlReader[1].ToString();
                        data[data.Count - 1][2] = sqlReader[2].ToString();
                        data[data.Count - 1][3] = sqlReader[3].ToString();
                    }

                    foreach (string[] s in data)
                        dataGridView1.Rows.Add(s);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (sqlReader != null)
                        sqlReader.Close();
                }
            }
            if (mysqlConnection != null && mysqlConnection.State != ConnectionState.Closed)
                mysqlConnection.Close();
        }
    }
}
