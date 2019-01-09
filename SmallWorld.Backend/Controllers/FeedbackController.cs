using System;
using Microsoft.AspNetCore.Mvc;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Filters;

namespace SmallWorld.Controllers
{
    [Route("pairs/{id}/feedback")]
    public class FeedbackController : MyController
    {
        private readonly IPairRepository pairs;

        public FeedbackController(IPairRepository pairs)
        {
            this.pairs = pairs;
        }

        [HttpPost]
        [DatabaseUpdate]
        public IActionResult SubmitFeedback(Guid id, [FromBody] FeedbackRequest request)
        {
            if (!pairs.Find(id, out var pair))
                return NotFound();

            switch (request.Content)
            {
                case "yes": pair.Outcome = PairOutcome.Success; break;
                case "no": pair.Outcome = PairOutcome.Failure; break;
            }

            pairs.Update(pair);

            return Ok();
        }

        public class FeedbackRequest
        {
            public string Content { get; set; }
        }
    }
}