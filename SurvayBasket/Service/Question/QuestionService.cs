﻿
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SurvayBasket.Contracts.Common;
using SurvayBasket.Models;
using SurvayBasket.Service.Caching;

namespace SurvayBasket.Service.Question
{
    public sealed class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ICachingService cachingService;

        public QuestionService(ApplicationDbContext dbContext, IMapper mapper, ICachingService cachingService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.cachingService = cachingService;
        }

        private const string _cachePrefix = "Available-Questions";

        public async Task<(QuestionResponse? questionResponse, string? Message)> GetAsync(int PollId, int id, CancellationToken cancellationToken)
        {
            var key = $"{_cachePrefix}-{PollId}";

            var CachedQuestions = await cachingService.GetAsync<QuestionResponse>(key, cancellationToken);

            if (CachedQuestions is null)
            {
                var Question = await dbContext.Questions
                     .Where(q => q.Id == id && q.PollId == PollId)
                     .Include(a => a.Answer)
                     .SingleOrDefaultAsync(cancellationToken: cancellationToken);
                var Message = string.Empty;
                if (Question is null)
                {
                    Message = "Question Is Not Found";
                    return (null, Message!);
                }
                var MappedQuestion = mapper.Map<QuestionResponse>(Question);

                await cachingService.SetAsync(key, MappedQuestion, cancellationToken);
                return (MappedQuestion, Message);
            }

            return (CachedQuestions, string.Empty);
        }

        public async Task<(Pagination<QuestionResponse>? questionResponse, string? Message)> GetAllAsync(int PollId, RequestFilter requestFilter, CancellationToken cancellationToken)
        {

            // Check if Poll Is Exit or not
            var PollIsExits = await dbContext.Polls.AnyAsync(p => p.Id == PollId,cancellationToken);
           
            if (!PollIsExits)
              return (null, "Poll Is Not Found");
            

            var Questions = await dbContext.Questions
                .Where(x => x.PollId == PollId
                    &&
                    (
                    string.IsNullOrEmpty(requestFilter.SearchValue)
                    ||
                    x.Content.ToLower().Contains(requestFilter.SearchValue.ToLower()))
                    )
                .Include(a => a.Answer)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            int TotalItems = Questions.Count;

            if (!string.IsNullOrEmpty(requestFilter.OrderByAsc))
                Questions.OrderBy(x => x.Content);

            if (!string.IsNullOrEmpty(requestFilter.OrderByDesc))
                Questions.OrderByDescending(x => x.Content);


            var QuestionList = Questions.Skip((requestFilter.PageNumber - 1) * requestFilter.PageSize)
                      .Take(requestFilter.PageSize).ToList();

            var MappedQuestion = mapper.Map<List<QuestionResponse>>(QuestionList);

            var QuestionResponse = new Pagination<QuestionResponse>()
            {
                Items = MappedQuestion,
                PageNumber = requestFilter.PageNumber,
                PageSize = requestFilter.PageSize,
                Count = TotalItems
            };

            return (QuestionResponse, null);

        }

        public async Task<(QuestionResponse? questionResponse, string? Message)> AddAsync(int pollId, QuestionRequest questionRequest, ApplicationUser user, CancellationToken cancellationToken = default)
        {

            // Check if Poll Is Exit or not
            var PollIsExits = await dbContext.Polls.AnyAsync(p => p.Id == pollId, cancellationToken: cancellationToken);

            var Message = string.Empty;
            if (!PollIsExits)
            {
                Message = "Poll Is Not Found";
                return (null, Message);
            }

            // Check if this Question is Duplicated 
            var QuestionIsDuplicated = await dbContext.Questions.AnyAsync(q => q.Content == questionRequest.Content && pollId == q.PollId, cancellationToken);
            if (QuestionIsDuplicated)
                return (null, "Question Is Duplicated");

            // Mapping
            var MappedQuestion = mapper.Map<Models.Question>(questionRequest);
            MappedQuestion.PollId = pollId;
            MappedQuestion.CreatedById = user.Id;

            // Add Question To Database
            await dbContext.AddAsync(MappedQuestion, cancellationToken);
            var Result = await dbContext.SaveChangesAsync(cancellationToken);

            if (Result <= 0)
                return (null, "BadRequest");

            // Get Saved Question id
            var QuestionEntityId = await dbContext.Questions.Where(q => q.Content == MappedQuestion.Content)
                .Select(q => q.Id)
                .SingleOrDefaultAsync();

            // Add Answers To Database
            foreach (var answer in questionRequest.Answers)
            {
                var NewAnswer = new Answer()
                {
                    Content = answer.Text,
                    QuestionId = QuestionEntityId
                };
                await dbContext.Answers.AddAsync(NewAnswer, cancellationToken);
            }


            await dbContext.SaveChangesAsync(cancellationToken);

            // Map the saved Question to QuestionResponse
            var QuestionResponse = new QuestionResponse()
            {
                Id = QuestionEntityId,
                Content = questionRequest.Content,
                Answers = questionRequest.Answers

            };
            var key = $"{_cachePrefix}-{pollId}";
            await cachingService.RemoveAsync(key, cancellationToken);
            return (QuestionResponse, null);
        }

        public async Task<(bool Result, string? Message)> UpdateAsync(int pollId, int id, QuestionRequest questionRequest, ApplicationUser user, CancellationToken cancellationToken = default)
        {
            var key = $"{_cachePrefix}-{pollId}";
            // Check on Question
            var questionisexits = await dbContext.Questions
                 .SingleOrDefaultAsync(a =>
                 a.PollId == pollId
                 &&
                 a.Id != id
                 &&
                 a.Content.ToLower() == questionRequest.Content.ToLower());
            if (questionisexits is not null)
                return (false, "Duplicate Question is not allowed.");

            var Question = await dbContext.Questions
              .Include(a => a.Answer)
              .SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);

            if (Question is null)
                return (false, "Question Not Found");

            Question.Content = questionRequest.Content;
            Question.UpdatedAt = DateTime.UtcNow;
            Question.UpdatedById = user.Id;

            // Current Answers 

            var CurrentAnswers = Question.Answer.Select(x => x.Content).ToList();

            // new Answers 

            var NewAnswers = questionRequest.Answers
                .Select(a => a.Text)
                .Except(CurrentAnswers).ToList();

            // add new answers to database

            NewAnswers.ForEach(answer =>
            {
                Question.Answer.Add(new Answer() { Content = answer, QuestionId = id });
            });

            // De-Activate the Answers which are removed

            Question.Answer.ToList().ForEach(answer =>
            {
                answer.IsActive = questionRequest.Answers.Select(x => x.Text).Contains(answer.Content);
            });

            #region Old Code
            //var NewAnswersRequest = questionRequest.Answers.Select(a => a.Text).ToList();

            //var DeActivatedAnswers = new List<string>();

            //for (int i = 0; i < CurrentAnswers.Count - 1; i++)
            //{
            //    for (int j = 0; j < NewAnswersRequest.Count - 1; j++)
            //    {
            //        if (CurrentAnswers[i] != NewAnswersRequest[j])
            //        {
            //            DeActivatedAnswers.Add(CurrentAnswers[i]);
            //        }
            //    }
            //}

            //var deActivatedAnswers = dbContext.Answers
            //     .Where(q => q.QuestionId == id
            //     &&
            //     DeActivatedAnswers.Contains(q.Content));

            //foreach (var answer in deActivatedAnswers)
            //{
            //    answer.IsActive = false;
            //    dbContext.Answers.Update(answer);
            //}

            #endregion

            await dbContext.SaveChangesAsync(cancellationToken);

            await cachingService.RemoveAsync(key, cancellationToken);
            return (true, string.Empty);

        }
        public async Task<(bool Response, string? Message)> ToggleStatus(int PollId, int id, CancellationToken cancellationToken)
        {
            var question = await dbContext.Questions
                .SingleOrDefaultAsync(q => q.Id == id && q.PollId == PollId, cancellationToken);

            if (question is null)
                return (false, "Question is not Found");

            question.IsActive = !question.IsActive;

            await dbContext.SaveChangesAsync(cancellationToken);

            var key = $"{_cachePrefix}-{PollId}";
            await cachingService.RemoveAsync(key, cancellationToken);
            return (true, string.Empty);

        }

        public async Task<(IReadOnlyList<QuestionResponse>? questionResponse, string? Message)> GetAllAvailableAsync(int PollId, string UserId, CancellationToken cancellationToken)
        {
            var key = $"{_cachePrefix}-{PollId}";
            var CachedQuesions = await cachingService.GetAsync<IReadOnlyList<QuestionResponse>>(key, cancellationToken);

            if (CachedQuesions is null)
            {

                // check on Poll if exist or not and is Available for Voting

                var Poll = dbContext.Polls.Where(p =>
                p.Id == PollId
                &&
                p.IsPublished
                &&
                p.StartsAt >= DateOnly.FromDateTime(DateTime.UtcNow)
                &&
                p.EndsAt <= DateOnly.FromDateTime(DateTime.UtcNow));

                if (Poll is null)
                    return (null, "Not Avaliabel For Now");

                // Check if this user already voted before
                var Voted = await dbContext.Votes.AnyAsync(v => v.UserId == UserId && v.PollId == PollId, cancellationToken);
                if (Voted)
                    return (null, "You have voted before :)");

                // Return Availabel Questions 
                var Questions = await dbContext.Questions
                    .Where(q => q.IsActive && q.PollId == PollId)
                    .Include(a => a.Answer)
                    .Select(q => new QuestionResponse()
                    {
                        Id = q.Id,
                        Content = q.Content,
                        Answers = q.Answer.Where(x => x.IsActive).Select(ans => new AnswerResponse() { Id = ans.Id, Text = ans.Content })
                    }
                    ).AsNoTracking()
                    .ToListAsync(cancellationToken);
                await cachingService.SetAsync(key, Questions, cancellationToken);

                return (Questions, string.Empty);

            }
            return (CachedQuesions, string.Empty);


        }
    }
}
