using MenuService.Handlers.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MenuService.DTO.Category;

namespace MenuService.Controllers
{
    [ApiController]
    [Route("api/menu/categories")]
    //[Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryHandler categoryHandler;
        public CategoryController(ICategoryHandler categoryHandler)
        { 
            this.categoryHandler = categoryHandler;
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CreateCategoryDTO createCategoryDTO)
        {
            var categoryDTO = await categoryHandler.CreateCategory(createCategoryDTO);
            return Ok(categoryDTO);
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            List<CategoryDTO> categories = await categoryHandler.GetCategories();
            return Ok(categories);
        }

        [HttpGet("{idCategory}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById([FromRoute] int idCategory)
        {
            CategoryDTO category = await categoryHandler.GetCategoryById(idCategory);
            return Ok(category);
        }

        [HttpPut("{idCategory}")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory([FromRoute] int idCategory, [FromBody] UpdateCategoryDTO updateCategoryDTO)
        {
            CategoryDTO category = await categoryHandler.UpdateCategory(updateCategoryDTO, idCategory);
            return Ok(category);
        }

        [HttpDelete("{idCategory}")]
        public async Task<ActionResult<bool>> DeleteCategory([FromRoute] int idCategory)
        {
            bool isDeleted = await categoryHandler.DeleteCategory(idCategory);
            return Ok(isDeleted);
        }
    }
}
