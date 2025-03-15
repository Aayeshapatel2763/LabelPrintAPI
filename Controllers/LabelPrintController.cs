//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
//using System.Data;

//namespace LabelPrintAPI.Controllers
//{
//    public class LabelPrintController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Data;

//using System.Data;
//using System.Data.OleDb;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class DataSyncController : ControllerBase
{
    private readonly string _sqlConnectionString =
    "Server=DESKTOP-EA7TR18;Database=UWCLablePrint;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

    [HttpPost("run-transfer")]
    public async Task<IActionResult> RunLabelTransfer()
    {
        try
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_lbltransfer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable); // Execute stored procedure and fill DataTable
                    }
                }
            }

            // Convert DataTable to JSON
            string jsonResult = JsonConvert.SerializeObject(dataTable);

            return Ok(jsonResult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
    //[HttpPost("run-transfer")]
    //public async Task<IActionResult> RunLabelTransfer()
    //{
    //    try
    //    {
    //        using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
    //        {
    //            await conn.OpenAsync();
    //            using (SqlCommand cmd = new SqlCommand("EXEC sp_lbltransfer", conn))
    //            {
    //                await cmd.ExecuteNonQueryAsync();
    //            }
    //        }

    //        return Ok(new { message = "Label transfer completed successfully." });
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, new { error = ex.Message });
    //    }
    //}
}
//private readonly string _accessConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\LabelPrintAPI\\Label Print API\\UWC\\UWCItem.mdb;Persist Security Info=False;";

//[HttpGet("sync-data")]
//public IActionResult SyncData()
//{
//    try
//    {
//        DataTable dataTable = FetchDataFromSQL();
//        SaveDataToAccess(dataTable);
//        return Ok("Data synchronized successfully!");
//    }
//    catch (Exception ex)
//    {
//        return StatusCode(500, $"Error: {ex.Message}");
//    }
//}
//[HttpGet("sync-data")]
//public IActionResult SyncData()
//{
//    try
//    {
//        using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
//        {
//            conn.Open();

//            using (SqlCommand cmd = new SqlCommand("EXEC sp_lbltransfer", conn))
//            {
//                int rowsAffected = cmd.ExecuteNonQuery();
//                return Ok($"Data synchronized successfully! {rowsAffected} rows affected.");
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        return StatusCode(500, $"Error: {ex.Message}");
//    }
//}

//private DataTable FetchDataFromSQL()
//{
//    DataTable dataTable = new DataTable();
//    using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
//    {
//        conn.Open();
//        string query = "sp_lbltransfer";
//        using (SqlCommand cmd = new SqlCommand(query, conn))
//        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
//        {
//            adapter.Fill(dataTable);
//        }
//    }
//    return dataTable;
//}

//private void SaveDataToAccess(DataTable dataTable)
//{
//    using (OleDbConnection conn = new OleDbConnection(_accessConnectionString))
//    {
//        conn.Open();
//        foreach (DataRow row in dataTable.Rows)
//        {
//            string query = "INSERT INTO YourAccessTable (Id, Name, Age) VALUES (@Id, @Name, @Age)";
//            using (OleDbCommand cmd = new OleDbCommand(query, conn))
//            {
//                cmd.Parameters.AddWithValue("@Id", row["Id"]);
//                cmd.Parameters.AddWithValue("@Name", row["Name"]);
//                cmd.Parameters.AddWithValue("@Age", row["Age"]);
//                cmd.ExecuteNonQuery();
//            }
//        }
//    }
//}
//}
