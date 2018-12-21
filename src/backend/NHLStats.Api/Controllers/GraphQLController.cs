using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using NHLStats.Api.Models;
using System;
using System.Threading.Tasks;

namespace NHLStats.Api.Controllers
{
    [Route("[controller]")]
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
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