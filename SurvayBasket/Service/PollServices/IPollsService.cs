
using SurvayBasket.Contracts.Common;

namespace SurvayBasket.Service.PollServices
{
    public interface IPollsService
    {
        public Task<Pagination<PollResponse>> GetAll(RequestFilter requestFilter ,CancellationToken cancellationToken);
        public Task<IReadOnlyList<PollResponse>> GetCurrentAll();
        public Task<Poll?> GetPollById(int id, CancellationToken cancellationToken);

        public Task<Poll> Add(Poll Entity, CancellationToken cancellationToken);

        public Task<bool> Update(int id, Poll Entity, CancellationToken cancellationToken);
        public Task<bool> Delete(int id, CancellationToken cancellationToken);

    }
}
