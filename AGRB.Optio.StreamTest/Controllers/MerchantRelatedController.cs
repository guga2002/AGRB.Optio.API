using Microsoft.AspNetCore.Mvc;
using RGBA.Optio.Stream.DecerializerClasses;
using RGBA.Optio.Stream.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;

namespace RGBA.Optio.Stream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantRelatedController : ControllerBase
    {
        private readonly IMerchantRelatedSer _merchantRelatedService;
        private readonly ITransactionRelatedSer ITransactionRelatedSer;
        public MerchantRelatedController(IMerchantRelatedSer _merchantRelatedService, ITransactionRelatedSer ITransactionRelatedSer)
        {
            this._merchantRelatedService = _merchantRelatedService;
            this.ITransactionRelatedSer = ITransactionRelatedSer;
        }


        [HttpGet]
        [Route("ValuteCourse")]
        public async Task LoadCurrencies()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/en/json");
                    response.EnsureSuccessStatusCode(); 
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var currenciesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CurrenciesResponse>>(responseBody);
                    await ITransactionRelatedSer.InsertCurrencies(currenciesResponse);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request failed: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization failed: {ex.Message}");
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("Location")]
        public async Task<IActionResult> FillDataToLocation()
        {
            await _merchantRelatedService.FillDataToLocation();
            return Ok();
        }

        [HttpGet]
        [Route("Merchant")]
        public async Task<IActionResult> FillDataMerchant()
        {
            await _merchantRelatedService.FillDataMerchant();
            return Ok();
        }
        [HttpPost]
        [Route("AssignLocationToMerchant")]
        public async Task<IActionResult> FillDataLocationToMerchant(int countNumber)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            await _merchantRelatedService.FillDataLocationToMerchant(countNumber);
            st.Stop();
            return Ok(st.ElapsedMilliseconds);
        }

        [HttpGet]
        [Route("Channel")]
        public async Task<IActionResult> fillChannel()
        {
            await ITransactionRelatedSer.fillChannel();
            return Ok();
        }
        [HttpGet]
        [Route("TypeOfTransaction")]
        public async Task<IActionResult> FillTypeOfTransaction()
        {
            await ITransactionRelatedSer.FillTypeOfTransaction();
            return Ok();
        }

        [HttpGet]
        [Route("Transaction")]
        public async Task<IActionResult> FillTransactions([FromQuery]int n)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            await ITransactionRelatedSer.FillTransactions(n);
            st.Stop();
            return Ok(st.ElapsedMilliseconds);
        }
        [HttpGet]
        [Route("TransactionFillBoolk")]
        public async Task<IActionResult> FillTransactionsboolk([FromQuery] int n)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            await ITransactionRelatedSer.FillTransactionsBulk(n);
            st.Stop();
            return Ok(st.ElapsedMilliseconds);
        }

        [HttpGet]
        [Route("AllTransactionsWithdapper")]
        public async Task<IActionResult> Getall()
        {
            return Ok(await ITransactionRelatedSer.GetAllTransactions());
        }

        [HttpGet]
        [Route("AllTransactions")]
        public async Task<IActionResult> GetallW()
        {
            return Ok(await ITransactionRelatedSer.GetAllTransactionsWithoutDapper());
        }
    }
}
