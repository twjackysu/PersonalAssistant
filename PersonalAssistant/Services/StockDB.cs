using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PersonalAssistant.DTO;
using StockLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalAssistant.Services
{
    public class StockDB : IStockDB
    {
        private readonly string connectionString;
        private readonly ILogger<StockDB> logger;
        public StockDB(IConfiguration configuration, ILogger<StockDB> logger)
        {
            connectionString = configuration.GetConnectionString("StockConnection");
            this.logger = logger;
        }
        public async Task<Dictionary<string, DTO.StockHistory>> GetLatestStocksPrice(params string[] stockNo)
        {
            var result = new Dictionary<string, DTO.StockHistory>();
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var selectCommand = connection.CreateCommand())
                {
                    selectCommand.CommandText = $@"with cteRowNumber as (
                                                        select StockHistoryID,No,Type,Name,Date,ClosingPrice, row_number() over(partition by [No] order by [Date] desc) as RowNum
                                                            from [StockInfoDB].[dbo].[StockHistory]
		                                                    WHERE [No] IN ('{string.Join("','", stockNo)}')
                                                    )
                                                    select *
                                                        from cteRowNumber
                                                        where RowNum = 1";
                    selectCommand.CommandTimeout = connection.ConnectionTimeout;
                    try
                    {
                        using (var rdr = await selectCommand.ExecuteReaderAsync())
                        {

                            if (rdr.HasRows)
                            {
                                var StockHistoryID = rdr.GetOrdinal("StockHistoryID");
                                var No = rdr.GetOrdinal("No");
                                var Type = rdr.GetOrdinal("Type");
                                var Name = rdr.GetOrdinal("Name");
                                var Date = rdr.GetOrdinal("Date");
                                var ClosingPrice = rdr.GetOrdinal("ClosingPrice");
                                while (await rdr.ReadAsync())
                                {
                                    var line = new DTO.StockHistory();
                                    line.StockHistoryID = Convert.ToUInt32(rdr[StockHistoryID]);
                                    line.No = rdr[No].ToString();
                                    line.Type = Enum.Parse<StockType>(rdr[Type].ToString());
                                    line.Name = rdr[Name].ToString();
                                    line.Date = Convert.ToDateTime(rdr[Date]);
                                    line.ClosingPrice = Convert.ToInt32(rdr[ClosingPrice]);
                                    result.Add(line.No, line);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Error when GetLatestStocksPrice({string.Join(",", stockNo)})");
                    }
                }
                return result;
            }
        }

        public async Task<Dictionary<string, DTO.StockHistory>> GetDateStocksPrice(DateTime date, params string[] stockNo)
        {
            var result = new Dictionary<string, DTO.StockHistory>();
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var selectCommand = connection.CreateCommand())
                {
                    selectCommand.CommandText = $@"with cteRowNumber as (
                                                        select StockHistoryID,No,Type,Name,Date,ClosingPrice, row_number() over(partition by [No] order by [Date] desc) as RowNum
                                                            from [StockInfoDB].[dbo].[StockHistory]
		                                                    WHERE [No] IN ('{string.Join("','", stockNo)}') AND Date <= '{date:yyyy-MM-dd hh:mm:ss}'
                                                    )
                                                    select *
                                                        from cteRowNumber
                                                        where RowNum = 1";
                    selectCommand.CommandTimeout = connection.ConnectionTimeout;
                    try
                    {
                        using (var rdr = await selectCommand.ExecuteReaderAsync())
                        {

                            if (rdr.HasRows)
                            {
                                var StockHistoryID = rdr.GetOrdinal("StockHistoryID");
                                var No = rdr.GetOrdinal("No");
                                var Type = rdr.GetOrdinal("Type");
                                var Name = rdr.GetOrdinal("Name");
                                var Date = rdr.GetOrdinal("Date");
                                var ClosingPrice = rdr.GetOrdinal("ClosingPrice");
                                while (await rdr.ReadAsync())
                                {
                                    var line = new DTO.StockHistory();
                                    line.StockHistoryID = Convert.ToUInt32(rdr[StockHistoryID]);
                                    line.No = rdr[No].ToString();
                                    line.Type = Enum.Parse<StockType>(rdr[Type].ToString());
                                    line.Name = rdr[Name].ToString();
                                    line.Date = Convert.ToDateTime(rdr[Date]);
                                    line.ClosingPrice = Convert.ToInt32(rdr[ClosingPrice]);
                                    result.Add(line.No, line);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Error when GetLatestStocksPrice({string.Join(",", stockNo)})");
                    }
                }
                return result;
            }
        }
    }
}
