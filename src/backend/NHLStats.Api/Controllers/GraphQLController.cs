using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using NHLStats.Api.Models;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NHLStats.Api.Controllers
{
    [Route("[controller]")]
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ILogger _logger;
        private readonly ISchema _schema;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter, ILogger logger)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GraphQLPost([FromBody] GraphQLQuery query)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //_logger.Information($"GraphQLPost started");

            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            Inputs inputs = query.Variables.ToInputs();
            ExecutionOptions executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs
            };

            ExecutionResult result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            sw.Stop();
            //_logger.Information($"GraphQLPost finished after {sw.ElapsedMilliseconds}ms");
            _logger.Information($"GraphQLPost: {sw.ElapsedMilliseconds}ms");

            return Ok(result);
        }
    }
}

// To create an AJAX query for GraphQL, create it in GraphiQL first, then in Dev Tools go to
// the Network tab, right click on the request and select Copy -> Copy as cURL (bash).
// In Postman click Import, Raw Text and paste. The body tab will contain the request.
// From this reference but need to select Copy as cURL (bash) not (cmd):
// https://stackoverflow.com/questions/42520663/how-to-send-graphql-query-by-postman

// For example:
//query NHLStatsQuery($id: Int!)
//{
//    player(id: $id){
//        name
//    }
//}
// variables
//{
//  "id": 2
//}
//Becomes:
//    {
//	"query":"query NHLStatsQuery($id: Int!){
//		player(id: $id)
//{
//    name
//        }
//	}",
//	"variables":{
//		"id":2

//	},
//	"operationName":"NHLStatsQuery"
//}