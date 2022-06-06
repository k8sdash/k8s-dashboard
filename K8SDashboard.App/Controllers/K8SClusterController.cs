﻿using K8SDashboard.Models;
using K8SDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace K8SDashboard.App.Controllers
{
    public class K8SClusterController : Controller
    {
        private readonly IK8sClientService k8SClientService;
        private readonly ILogger<K8SClusterController> logger;
        private readonly AppSettings appSettings;

        public K8SClusterController(ILogger<K8SClusterController> logger, AppSettings appSettings, IK8sClientService k8SClientService)
        {
            this.logger = logger;
            this.appSettings = appSettings;
            this.k8SClientService = k8SClientService;
        }

       [HttpGet]
       [Route("lightRoutes")]
       public async Task<ActionResult<List<LightRoute>>> GetLightRoutes()
        {
            try
            {
                logger.LogDebug("controller will get lightRoutes from service...");
                var lightRoutes = await k8SClientService.ListLightRoutesWithTimeOut(appSettings.DefaultNamespace, 2);
                if(lightRoutes!= null)
                {
                    logger.LogTrace("Got {Count} Light Routes", lightRoutes.Count);
                    return Ok();
                }
                return BadRequest("Unable to get routes. Please check the logs");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was a problem getting Light Routes");
                return BadRequest(ex);
            }
        }
    }
}