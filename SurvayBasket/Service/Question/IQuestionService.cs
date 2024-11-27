using SurvayBasket.Contracts.Question;

namespace SurvayBasket.Service.Question
{
    public interface IQuestionService
    {
        public Task<(IReadOnlyList<QuestionResponse>? questionResponse, string? Message)> GetAllAsync(int PollId, CancellationToken cancellationToken);
        public Task<(IReadOnlyList<QuestionResponse>? questionResponse, string? Message)> GetAllAvailableAsync(int PollId, string UserId, CancellationToken cancellationToken);
        public Task<(QuestionResponse? questionResponse, string? Message)> GetAsync(int PollId, int id, CancellationToken cancellationToken);
        public Task<(bool Response, string? Message)> ToggleStatus(int PollId, int id, CancellationToken cancellationToken);
        public Task<(QuestionResponse? questionResponse, string? Message)> AddAsync(int pollId, QuestionRequest questionRequest, ApplicationUser user, CancellationToken cancellationToken = default!);
        public Task<(bool Result, string? Message)> UpdateAsync(int pollId, int id, QuestionRequest questionRequest, ApplicationUser user, CancellationToken cancellationToken = default!);
    }
}
