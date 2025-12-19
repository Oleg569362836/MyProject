using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace Уч.практика_2.Учет_продаж_в_супермаркете
{
    internal class DataBase
    {
        static string connStr = "Data Source=TopMarket.db";

        SqliteConnection conn = new SqliteConnection(connStr);

        public static void CreateTableEmploye()
        {
            SqliteConnection conn = new SqliteConnection("Data Source=TopMarket.db");

            try
            {
                conn.Open();
                SqliteCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Employe (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 
                    EmloyeId INTEGER NOT NULL,
                    fullname TEXT NOT NULL,
                    login TEXT NOT NULL,
                    password TEXT NOT NULL,
                    role TEXT NOT NULL)";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Таблица работника создана!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании таблицы работника: {ex}");
            }
            finally
            {
                conn.Close();
            }
        }

        public static void CreateTableSale()
        {
            SqliteConnection conn = new SqliteConnection("Data Source=TopMarket.db");

            try
            {
                conn.Open();
                SqliteCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Sale (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                    Saletime DATETIME NOT NULL,
                    EmployeId INTEGER NOT NULL,
                    count INTEGER NOT NULL,
                    ProductId INTEGER NOT NULL)";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Таблица продаж создана!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании таблицы продаж: {ex}");
            }
            finally
            {
                conn.Close();
            }
        }

        public static void CreateTableProduct()
        {
            SqliteConnection conn = new SqliteConnection("Data Source=TopMarket.db");

            try
            {
                conn.Open();
                SqliteCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Product (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                    name TEXT NOT NULL,
                    decription TEXT,
                    CategoryId INTEGER NOT NULL,
                    prise INTEGER NOT NULL)";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Таблица продуктов создана!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании таблицы продуктов: {ex}");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}