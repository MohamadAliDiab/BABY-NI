using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GetDataByDateService = MiniProject.Data.Services.GetDataByDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetDataByDate : ControllerBase
    {
        private GetDataByDateService _GetDataByDate;

        public GetDataByDate()
        {
            _GetDataByDate = new GetDataByDateService();
        }

        [HttpGet("get-data-by-date/{tableName}/ {dateFrom}/ {dateTo}")]

        public IActionResult GetData(string tableName,DateTime dateFrom, DateTime dateTo)
        {
            var rows = _GetDataByDate.GetData(tableName, dateFrom, dateTo);
            return Ok(rows);
        }

    }
}
