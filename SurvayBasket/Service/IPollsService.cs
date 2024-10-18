using SurvayBasket.Models;

namespace SurvayBasket.Service
{
    public interface IPollsService
    {
        public IEnumerable<Poll> GetAll();
        public Poll? GetPollById(int id);

        public Poll Add(Poll Entity);

        public bool Update(int id, Poll Entity);
        public bool Delete(int id);

    }
}
