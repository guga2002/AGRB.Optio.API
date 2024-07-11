using AGRB.Optio.Application.Interfaces.AI;
using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Models.RequestModels;
using AGRB.Optio.Application.Responses;
using AGRB.Optio.Application.StaticFiles;
using Microsoft.AspNetCore.Mvc;

namespace AGRB.Optio.API.Controllers.AI
{
    /// <summary>
    /// Controller for handling AI-based transactions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AIForTransactionController : ControllerBase
    {
        private readonly IAiServices _aiServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="AIForTransactionController"/> class.
        /// </summary>
        /// <param name="aiServices">The AI services to be used.</param>
        public AIForTransactionController(IAiServices aiServices)
        {
            this._aiServices = aiServices;
        }

        /// <summary>
        /// Makes a prediction about the fraudulence of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction model.</param>
        /// <returns>A response containing the fraudulence prediction as a double.</returns>
        [HttpPost]
        [Route(nameof(MakePredictionAboutFraudulence))]
        public async Task<Response<double>> MakePredictionAboutFraudulence(TransactionModel transaction)
        {
            if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
            var res = await _aiServices.MakePredictionAboutFraudulence(transaction);
            return Response<double>.Ok(res);
        }

        /// <summary>
        /// Gets transactions for review within a specified date range.
        /// </summary>
        /// <param name="date">The date range request model.</param>
        /// <returns>A response containing the transactions for review.</returns>
        [HttpPost]
        [Route(nameof(GetTransactionsForReview))]
        public async Task<Response<IEnumerable<TransactionModel>>> GetTransactionsForReview(DateRangeRequestModel date)
        {
            if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
            var res = await _aiServices.GetTransactionsForReview(date);
            return res.Any() ? Response<IEnumerable<TransactionModel>>.Ok(res)
                : Response<IEnumerable<TransactionModel>>.Error(ErrorKeys.BadRequest);
        }

        /// <summary>
        /// Updates the prediction of a transaction as fraudulent or not.
        /// </summary>
        /// <param name="transaction">The transaction model.</param>
        /// <param name="isFraudulent">A boolean indicating if the transaction is fraudulent.</param>
        /// <returns>A response indicating if the update was successful.</returns>
        [HttpPost]
        [Route(nameof(UpdateTransactionPrediction))]
        public async Task<Response<bool>> UpdateTransactionPrediction(TransactionModel transaction, bool isFraudulent)
        {
            if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
            var res = await _aiServices.UpdateTransactionPrediction(transaction, isFraudulent);
            return res ? Response<bool>.Ok(res)
                : Response<bool>.Error(ErrorKeys.BadRequest);
        }

        /// <summary>
        /// Gets fraudulent transactions categorized by their type within a specified date range.
        /// </summary>
        /// <param name="date">The date range request model.</param>
        /// <returns>A response containing the categorized fraudulent transactions.</returns>
        [HttpPost]
        [Route(nameof(GetFraudulentTransactionsByCategory))]
        public async Task<Response<IDictionary<string, IEnumerable<TransactionModel>>>> GetFraudulentTransactionsByCategory(DateRangeRequestModel date)
        {
            if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
            var res = await _aiServices.GetFraudulentTransactionsByCategory(date);
            return res.Any() ? Response<IDictionary<string, IEnumerable<TransactionModel>>>.Ok(res)
                : Response<IDictionary<string, IEnumerable<TransactionModel>>>.Error(ErrorKeys.BadRequest);
        }

        /// <summary>
        /// Clears the fraudulent status of the specified transactions.
        /// </summary>
        /// <param name="transactions">The list of transactions.</param>
        /// <returns>A boolean indicating if the operation was successful.</returns>
        [HttpPost]
        [Route(nameof(ClearFraudulentStatus))]
        public async Task<bool> ClearFraudulentStatus(IEnumerable<TransactionModel> transactions)
        {
            return await _aiServices.ClearFraudulentStatus(transactions);
        }

        /// <summary>
        /// Gets suspicious transactions within a specified date range.
        /// </summary>
        /// <param name="date">The date range request model.</param>
        /// <returns>A response containing the suspicious transactions.</returns>
        [HttpPost]
        [Route(nameof(GetSuspiciousTransactions))]
        public async Task<Response<IEnumerable<TransactionModel>>> GetSuspiciousTransactions(DateRangeRequestModel date)
        {
            if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
            var res = await _aiServices.GetSuspiciousTransactions(date);
            return res.Any() ? Response<IEnumerable<TransactionModel>>.Ok(res)
                : Response<IEnumerable<TransactionModel>>.Error(ErrorKeys.BadRequest);
        }

        /// <summary>
        /// Trains the AI model.
        /// </summary>
        [HttpGet]
        [Route(nameof(TrainModel))]
        public async Task TrainModel()
        {
            await _aiServices.TrainModel();
        }
    }
}
