using Microsoft.AspNetCore.Mvc;
using TC.Application.Interfaces;
using TC.Application.Aggregation;
using TC.Application.Pagination;
using TC.Application.Entities;

namespace TC.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketsCatalogController : ControllerBase
    {
        private readonly ILogger<TicketsCatalogController> _logger;
        private readonly ITicketsCatalogService _service;

        public TicketsCatalogController(ILogger<TicketsCatalogController> logger, ITicketsCatalogService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{catalogId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TicketsCatalog>> GetCatalogById(int catalogId, int userId)
        {
            var catalog = await _service.GetCatalogByIdAsync(catalogId, userId);
            if (catalog == null)
                return NotFound(new { Message = $"Catalog with ID {catalogId} not found." });

            return Ok(catalog);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TicketsCatalog>>> GetAllCatalogs(
            [FromQuery] int userId,
            [FromQuery] int currentPage = 1,
            [FromQuery] int pageSize = 50)
        {
            var catalogs = await _service.GetAllCatalogsAsync(userId);
            if (catalogs == null || catalogs.Count == 0)
                return NotFound(new { Message = "No catalogs found for the specified user." });

            var agregatedCatalogs = AggregationHelper.AggregateCatalogs(catalogs);

            var result = PaginationHelper.Paginate<AggregationResult>(agregatedCatalogs, currentPage, pageSize);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateCatalog([FromBody] TicketsCatalog catalog, int userId)
        {
            if (catalog == null)
                return BadRequest(new { Message = "Catalog data is invalid." });

            await _service.CreateCatalogAsync(catalog, userId);

            return CreatedAtAction(nameof(GetCatalogById), new { catalogId = catalog.Id, userId }, catalog);
        }

        [HttpPut("{catalogId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateCatalog(int catalogId, [FromBody] TicketsCatalog catalog, int userId)
        {
            if (catalog == null)
                return BadRequest(new { Message = "Catalog data is invalid." });

            var existingCatalog = await _service.GetCatalogByIdAsync(catalogId, userId);
            if (existingCatalog == null)
                return NotFound(new { Message = $"Catalog with ID {catalogId} not found." });

            catalog.Id = catalogId;
            await _service.UpdateCatalogAsync(catalog, userId);

            return NoContent();
        }

        [HttpDelete("{catalogId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCatalog(int catalogId, int userId)
        {
            var existingCatalog = await _service.GetCatalogByIdAsync(catalogId, userId);
            if (existingCatalog == null)
                return NotFound(new { Message = $"Catalog with ID {catalogId} not found." });

            await _service.DeleteCatalogAsync(catalogId, userId);

            return NoContent();
        }

        [HttpPost("{catalogId}/users/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddUserToCatalog(int catalogId, int userId, [FromBody] User userToAdd)
        {
            if (userToAdd == null)
                return BadRequest(new { Message = "User data is invalid." });

            var catalog = await _service.GetCatalogByIdAsync(catalogId, userId);
            if (catalog == null)
                return NotFound(new { Message = $"Catalog with ID {catalogId} not found." });

            await _service.AddUserToCatalogAsync(catalogId, userId, userToAdd);

            return NoContent();
        }

        [HttpGet("{catalogId}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<int>>> GetUsersByCatalogId(int catalogId, int userId)
        {
            var users = await _service.GetAllUsersByCatalogIdAsync(catalogId, userId);
            if (users == null || users.Count == 0)
                return NotFound(new { Message = $"No users found for catalog ID {catalogId}." });

            return Ok(users);
        }

        [HttpDelete("{catalogId}/users/{userToRemoveId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUserFromCatalog(int catalogId, int userId, int userToRemoveId)
        {
            var catalog = await _service.GetCatalogByIdAsync(catalogId, userId);
            if (catalog == null)
                return NotFound(new { Message = $"Catalog with ID {catalogId} not found." });

            await _service.DeleteUserFromCatalogAsync(catalogId, userId, userToRemoveId);

            return NoContent();
        }
    }
}
