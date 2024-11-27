using SurvayBasket.Contracts.Answer;
using SurvayBasket.Contracts.Question;
using SurvayBasket.Contracts.Vote;

namespace SurvayBasket.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Poll, CreatePollRequest>().ReverseMap();

            CreateMap<Poll, PollResponse>().ReverseMap();
            CreateMap<VoteAnswer, VoteAnswerRequest>().ReverseMap();

            CreateMap<Question, QuestionRequest>().ReverseMap();

            CreateMap<Question, QuestionResponse>()
           .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answer)).ReverseMap();

            CreateMap<Answer, AnswerResponse>()
                .ForMember(a => a.Text, o => o.MapFrom(e => e.Content))
                .ReverseMap();
        }
    }
}
