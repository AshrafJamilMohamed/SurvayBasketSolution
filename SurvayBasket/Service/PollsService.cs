using SurvayBasket.Models;

namespace SurvayBasket.Service
{
    public class PollsService : IPollsService
    {
        private static readonly List<Poll> polls = new List<Poll>()
        {
            new Poll()
            {
                Id = 1,
                Title = "Poll 1",
                Description = "this is poll one"
            }

        };

        public Poll Add(Poll Entity)
        {
            polls.Add(Entity);
            return Entity;

        }

        public bool Delete(int id)
        {
            var Poll = GetPollById(id);
            return Poll is not null ? polls.Remove(Poll) : false;


        }

        public IEnumerable<Poll> GetAll() => polls;


        public Poll? GetPollById(int id)
        {
            var poll = polls.FirstOrDefault(x => x.Id == id);
            return poll;
        }

        public bool Update(int id, Poll Entity)
        {
            var CurrentPoll = GetPollById(id);
            if (CurrentPoll is null)
                return false;

            CurrentPoll.Title = Entity.Title;
            CurrentPoll.Description = Entity.Description;
            return true;

        }
    }
}
