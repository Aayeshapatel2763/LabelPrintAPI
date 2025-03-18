

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Data;
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
                    cmd.CommandTimeout = 1000;
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
   
}
