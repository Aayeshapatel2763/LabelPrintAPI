
using LabelPrintAPI.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Threading.Tasks;


[Route("api/[controller]")]
[ApiController]
public class DataSyncController : ControllerBase
{
    //    private readonly string _sqlConnectionString =
    //        "Server=DESKTOP-EA7TR18;Database=UWCLablePrint;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

    //    private readonly string _sqlConnectionString1 =
    //        "Server=DESKTOP-EA7TR18;Database=NewKoblenz;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

    private readonly ConnectionStrings _connectionStrings;

    public DataSyncController(IOptions<ConnectionStrings> options)
    {
        _connectionStrings = options.Value;
    }

    [HttpPost("run-transfer-UWC")]
    public async Task<IActionResult> RunLabelTransfer()
    {
        //return await ExecuteStoredProcedure(_sqlConnectionString, "sp_lbltransfer");

        return await ExecuteStoredProcedure(_connectionStrings.UWCLablePrintDB, "sp_lbltransfer");
    }

    [HttpPost("run-transfer-Kob")]
    public async Task<IActionResult> RunLabelKobTransfer()
    {

        return await ExecuteStoredProcedure(_connectionStrings.NewKoblenzDB, "sp_lbltransfer");
        // return await ExecuteStoredProcedure(_sqlConnectionString1, "sp_lbltransfer");
    }

    private async Task<IActionResult> ExecuteStoredProcedure(string connectionString, string storedProcedureName)
    {
        try
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
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
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}

