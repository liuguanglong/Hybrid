using Hybrid.Service.Shared.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using WorkflowCore.Interface;
using WorkflowCore.Services.DefinitionStorage;

namespace Hybrid.Server.Controllers
{

    [Authorize(Roles = "Manager", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    [ApiController]
    public class WorkflowController : Controller
    {
        private readonly IDefinitionLoader definitionLoader;
        private readonly IWorkflowRegistry workflowRegistry;

        public WorkflowController(
            IDefinitionLoader definitionLoader,
            IWorkflowRegistry registry
            )
        {
            this.definitionLoader = definitionLoader;
            workflowRegistry = registry;
        }

        [HttpPost("Deploy")]
        public async Task DeployWorkflow([FromBody]JsonObject definition)
        {
            var wf = definitionLoader.LoadDefinition(definition.ToJsonString(), Deserializers.Json);
            workflowRegistry.RegisterWorkflow(wf);

        }
    }
}
