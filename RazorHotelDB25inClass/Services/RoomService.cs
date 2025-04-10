using Microsoft.Data.SqlClient;
using RazorHotelDB25inClass.Interfaces;
using RazorHotelDB25inClass.Models;
using System.Data;

namespace RazorHotelDB25inClass.Services
{
    public class RoomService : IRoomServiceAsync
    {
        private string connectionString = Secret.ConnectionString;
        private string roomSql = "Select Room_No, Hotel_No, Types, Price from Room where Hotel_No = @HotelNo";
        private string insertSql = "Insert into Room Values(@Room_No, @Hotel_No, @Types, @Price)";
        private string deleteSql = "Delete from Room where Hotel_No = @HotelNo and Room_No = @RoomNo";
        private string updateSql = "Update Room set Types = @Types, Price = @Price, Room_No = @RoomNo, Hotel_No = @HotelNo where Room_No = @RoomNo and Hotel_No = @HotelNo";
        public async Task<bool> CreateRoomAsync(int hotelNr, Room room)
        {
            bool isCreated = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(insertSql, connection);

                try
                {
                    command.Parameters.AddWithValue("@Room_No", room.RoomNr);
                    command.Parameters.AddWithValue("@Hotel_No", hotelNr);
                    command.Parameters.AddWithValue("@Types", room.Types);
                    command.Parameters.AddWithValue("@Price", room.Pris);

                    await command.Connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if(rowsAffected > 0)
                    {
                        isCreated = true;
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine(sqlEx.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                return isCreated;
            }
        }
        public async Task<Room> DeleteRoomAsync(int roomNr, int hotelNr)
        {
            Room? room = await GetRoomFromIdAsync(roomNr, hotelNr);
            if (room == null)
                return null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(deleteSql, connection);
                    command.Parameters.AddWithValue("@RoomNo", roomNr);
                    command.Parameters.AddWithValue("@HotelNo", hotelNr);
                    await connection.OpenAsync();
                    int noOfRows = await command.ExecuteNonQueryAsync();
                    if (noOfRows > 0)
                        return room;
                    else
                        return null;
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine(sqlEx.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return room;
            }
        }
        public async Task<List<Room>> GetAllRoomAsync(int hotelNr)
        {
            List<Room> rooms = new List<Room>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(roomSql, connection);
                    command.Parameters.AddWithValue("@HotelNo", hotelNr);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        int roomNr = reader.GetInt32("Room_No");
                        char types = reader.GetString("Types")[0];
                        double price = reader.GetDouble("Price");
                        Room room = new Room(roomNr, types, price, hotelNr);
                        rooms.Add(room);
                    }
                    reader.Close();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                }
                finally
                {

                }
            }
            return rooms;
        }

        public async Task<Room?> GetRoomFromIdAsync(int roomNr, int hotelNr)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Room? room = null;
                try
                {
                    SqlCommand command = new SqlCommand(roomSql + " and Room_No = @RoomNo", connection);
                    command.Parameters.AddWithValue("@RoomNo", roomNr);
                    command.Parameters.AddWithValue("@HotelNo", hotelNr);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        int rNr = reader.GetInt32("Room_No");
                        int hNr = reader.GetInt32("Hotel_No");
                        string types = reader.GetString("Types");
                        double price = reader.GetDouble("Price");
                        room = new Room(rNr, types[0], price, hNr);
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
                finally
                {

                }
                return room;
            }
        }
        public async Task<bool> UpdateRoomAsync(Room room, int roomNr, int hotelNr)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(updateSql, connection);
                    command.Parameters.AddWithValue("@RoomNo", room.RoomNr);
                    command.Parameters.AddWithValue("@HotelNo", room.HotelNr);
                    command.Parameters.AddWithValue("@Types", room.Types);
                    command.Parameters.AddWithValue("@Price", room.Pris);
                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    return false;
                }
                finally { }
                return true;
            }
        }
    }
}
