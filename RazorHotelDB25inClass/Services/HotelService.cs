using Microsoft.Data.SqlClient;
using RazorHotelDB25inClass.Exceptions;
using RazorHotelDB25inClass.Interfaces;
using RazorHotelDB25inClass.Models;
using System.Data;

namespace RazorHotelDB25inClass.Services
{
    public class HotelService : IHotelServiceAsync
    {
        private string connectionString = Secret.ConnectionString;
        
        private string queryString = "SELECT Hotel_No, Name, Address FROM Hotel";
        private string findHotelByIDSql = "select Hotel_No, Name, Address FROM Hotel WHERE Hotel_No = @ID";
        private string findHotelByNameSql = "select Hotel_No, Name, Address FROM Hotel WHERE Name LIKE @Name";
        private string insertSql = "Insert INTO Hotel Values(@ID, @Navn, @Adresse)";
        private string updateSql = "UPDATE Hotel SET Name = @Name, Address = @Address WHERE Hotel_No = @ID";
        private string deleteSql = "DELETE FROM Hotel WHERE Hotel_No = @ID";
        public async Task<bool> CreateHotelAsync(Hotel hotel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(insertSql, connection);

                try
                {
                    command.Parameters.AddWithValue("@ID", hotel.HotelNr);
                    command.Parameters.AddWithValue("@Navn", hotel.Navn);
                    command.Parameters.AddWithValue("@Adresse", hotel.Adresse);

                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();


                    return true;
                }
                catch (SqlException sqlEx)
                {
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return false;
        }

        public async Task<Hotel> DeleteHotelAsync(int hotelNr)
        {
            Hotel? hotel = GetHotelFromIdAsync(hotelNr).Result;
            if (hotel == null)
                return null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(deleteSql, connection);
                    cmd.Parameters.AddWithValue("@ID", hotelNr);
                    await connection.OpenAsync();
                    int noOfRows = cmd.ExecuteNonQueryAsync().Result;
                    if (noOfRows > 0)
                        return hotel;
                    else
                        return null;
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine(sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
                return hotel;
            }
        }

        public async Task<List<Hotel>> GetAllHotelAsync()
        {
            List<Hotel> hoteller = new List<Hotel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    Thread.Sleep(1000);
                    while (await reader.ReadAsync())
                    {
                        int hotelNr = reader.GetInt32("Hotel_No");
                        string hotelNavn = reader.GetString("Name");
                        string hotelAdr = reader.GetString("Address");
                        Hotel hotel = new Hotel(hotelNr, hotelNavn, hotelAdr);
                        hoteller.Add(hotel);
                    }
                    reader.Close();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error" + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
                finally
                {

                }
            }
            return hoteller;

        }

        public async Task<Hotel> GetHotelFromIdAsync(int hotelNr)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Hotel hotel = null;
                try
                {
                    SqlCommand command = new SqlCommand(findHotelByIDSql, connection);
                    command.Parameters.AddWithValue("@ID", hotelNr);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        int hNr = reader.GetInt32("Hotel_No");
                        string hotelNavn = reader.GetString("Name");
                        string hotelAdr = reader.GetString("Address");
                        hotel = new Hotel(hNr, hotelNavn, hotelAdr);
                    }
                    reader.Close();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error" + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
                finally
                {

                }
                return hotel;
            }
        }

        //public Task<List<Hotel>> GetHotelsByNameAsync(string name)
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        Hotel hotel = null;
        //        List<Hotel> hotelName = new List<Hotel>();
        //        try
        //        {
        //            SqlCommand command = new SqlCommand(findHotelByNameSql, connection);
        //            command.Parameters.AddWithValue("@Name", "%" + name + "%");
        //            command.Connection.Open();
        //            SqlDataReader reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                int hNr = reader.GetInt32("Hotel_No");
        //                string hotelNavn = reader.GetString("Name");
        //                string hotelAdr = reader.GetString("Address");
        //                hotel = new Hotel(hNr, hotelNavn, hotelAdr);
        //                hotelName.Add(hotel);
        //            }
        //            reader.Close();
        //            //return hotel;
        //        }
        //        catch (SqlException sqlExp)
        //        {
        //            Console.WriteLine("Database error" + sqlExp.Message);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Generel fejl: " + ex.Message);
        //        }
        //        finally
        //        {

        //        }
        //        return hotelName;
        //    }
        //}

        public async Task<bool> UpdateHotelAsync(Hotel hotel, int hotelNr)
        {
            bool temp = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(updateSql, connection);
                    command.Parameters.AddWithValue("@ID", hotel.HotelNr);
                    command.Parameters.AddWithValue("@Name", hotel.Navn);
                    command.Parameters.AddWithValue("@Address", hotel.Adresse);
                    command.Connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    reader.Close();
                    temp = true;
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error" + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
                finally
                {

                }
            }
            return temp;
        }
        //public Task<IEnumerable<Hotel>> NameSearch(string str)
        //{
        //    List<Hotel> nameSearch = new List<Hotel>();
        //    foreach (Hotel hotel in _items)
        //    {
        //        if (string.IsNullOrEmpty(str) || hotel.Navn.ToLower().Contains(str.ToLower()))
        //        {
        //            nameSearch.Add(hotel);
        //        }
        //    }

        //    return nameSearch;
        //}


        public async Task<List<Hotel>> GetHotelsByNameAsync(string name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<Hotel> hoteller = new List<Hotel>();
                try
                {
                    SqlCommand command = new SqlCommand(queryString + " where Name like @Search", connection);
                    command.Parameters.AddWithValue("@Search", "%" + name + "%");
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync()) // reads from data not from console
                    {
                        int hotelNr = reader.GetInt32("Hotel_No");
                        string hotelNavn = reader.GetString("Name");
                        string hotelAdr = reader.GetString("Address");
                        Hotel hotel = new Hotel(hotelNr, hotelNavn, hotelAdr);
                        hoteller.Add(hotel);
                    }
                    reader.Close();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
                finally { }
                return hoteller;
            }
        }
    }
}
