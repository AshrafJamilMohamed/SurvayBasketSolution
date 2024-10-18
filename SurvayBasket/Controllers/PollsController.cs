using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurvayBasket.Models;
using SurvayBasket.Service;

namespace SurvayBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly IPollsService pollsService;

        public PollsController(IPollsService _pollsService)
        {
            pollsService = _pollsService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(pollsService.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var poll = pollsService.GetPollById(id);
            return poll is not null ? Ok(poll) : NotFound();
        }

        [HttpPost]
        public IActionResult Add(Poll poll)
        {
            var NewPoll = pollsService.Add(poll);
            return CreatedAtAction(nameof(GetById), new { id = NewPoll.Id }, NewPoll);
        }

        [HttpPut]
        public IActionResult Update(int id, Poll poll)
        {
            var Result = pollsService.Update(id, poll);
            return Result is true ? Ok(Result) : NotFound();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var Result = pollsService.Delete(id);
            return Result is true ? Ok(Result) : NotFound();
        }
    }
}
