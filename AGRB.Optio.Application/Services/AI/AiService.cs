using AGRB.Optio.Application.Interfaces.AI;
using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Models.RequestModels;
using AGRB.Optio.Application.StaticFiles;
using AGRB.Optio.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Transaction = AGRB.Optio.Domain.Entities.Transaction;

namespace AGRB.Optio.Application.Services.AI
{
    public class AiService : AbstractService<AiService>, IAiServices
    {
        public AiService(IUniteOfWork work, IMapper map, ILogger<AiService> log) : base(work, map, log)
        {
        }

        #region ClearFraudulentStatus
        public async Task<bool> ClearFraudulentStatus(IEnumerable<TransactionModel> transactions)
        {
            if (!transactions.Any()) throw new ArgumentException(ErrorKeys.BadRequest);
            var mapped = mapper.Map<IEnumerable<Transaction>>(transactions)
                ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            var res = await work.AiRepository.ClearFraudulentStatus(mapped);
            return res;
        }
        #endregion

        #region GetFraudulentTransactionsByCategory
        public async Task<IDictionary<string, IEnumerable<TransactionModel>>> GetFraudulentTransactionsByCategory(DateRangeRequestModel date)
        {
            if (date.End < date.Start) throw new ArgumentException(ErrorKeys.BadRequest);
            var res = await work.AiRepository.GetFraudulentTransactionsByCategory(date.Start, date.End);
            if (!res.Any()) throw new InvalidOperationException(ErrorKeys.BadRequest);
            var mapped = mapper.Map<IDictionary<string, IEnumerable<TransactionModel>>>(res)
                ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            return mapped;
        }
        #endregion

        #region GetSuspiciousTransactions
        public async Task<IEnumerable<TransactionModel>> GetSuspiciousTransactions(DateRangeRequestModel date)
        {
            if (date.End < date.Start) throw new ArgumentException(ErrorKeys.BadRequest);
            var res=await work.AiRepository.GetSuspiciousTransactions(date.Start, date.End);
            if (!res.Any()) throw new InvalidOperationException(ErrorKeys.BadRequest);
            var mapped = mapper.Map<IEnumerable<TransactionModel>>(res)
               ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            return mapped;
        }
        #endregion

        #region GetTransactionsForReview
        public async Task<IEnumerable<TransactionModel>> GetTransactionsForReview(DateRangeRequestModel date)
        {
            if (date.End < date.Start) throw new ArgumentException(ErrorKeys.BadRequest);
            var res = await work.AiRepository.GetTransactionsForReview(date.Start, date.End);
            if (!res.Any()) throw new InvalidOperationException(ErrorKeys.BadRequest);
            var mapped = mapper.Map<IEnumerable<TransactionModel>>(res)
               ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            return mapped;
        }
        #endregion

        #region MakePredictionAboutFraudulence
        public async Task<double> MakePredictionAboutFraudulence(TransactionModel transaction)
        {
            ArgumentNullException.ThrowIfNull(transaction, nameof(transaction));
            var mapped = mapper.Map<Transaction>(transaction)
                ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            var res = await work.AiRepository.MakePredictionAboutFraudulence(mapped);
            return res;
        }
        #endregion

        #region TrainModel
        public async Task TrainModel()
        {
           await work.AiRepository.TrainModel();
        }
        #endregion

        #region UpdateTransactionPrediction

        public async Task<bool> UpdateTransactionPrediction(TransactionModel transaction, bool isFraudulent)
        {
            ArgumentNullException.ThrowIfNull(transaction, nameof(transaction));
            var mapped = mapper.Map<Transaction>(transaction)
               ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            var res=await work.AiRepository.UpdateTransactionPrediction(mapped, isFraudulent);
            return res;
        }

        #endregion
    }
}
