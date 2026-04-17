using MenuService.Data;
using MenuService.Entities;
using MenuService.Handlers.MenuItem;
using MenuService.Handlers.MenuItemCategory;
using MenuService.Handlers.MenuItemIngredient;
using MenuService.Handlers.Links;

using MenuService.DTO.Ingredient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MenuService.DTO.MenuItem;
using Microsoft.EntityFrameworkCore;
using MenuService.DTO.Category;

namespace MenuService.Controllers
{
    [ApiController]
    [Route("api/")]
    //[Authorize]
    public class MenuController : ControllerBase
    {
        private readonly MenuDbContext _context;
        private readonly IMenuItemHandler menuItemHandler;
        private readonly IMenuItemCategoryHandler menuItemCategoryHandler;
        private readonly IMenuItemIngredientHandler menuItemIngredientHandler;
        private readonly IOrderLink orderLink;


        public MenuController(MenuDbContext context, 
                                IMenuItemHandler menuItemHandler,
                                IMenuItemCategoryHandler menuItemCategoryHandler,
                                IMenuItemIngredientHandler menuItemIngredientHandler,
                                IOrderLink orderLink)
        {
            _context = context;
            this.menuItemHandler = menuItemHandler;
            this.menuItemCategoryHandler = menuItemCategoryHandler;
            this.menuItemIngredientHandler = menuItemIngredientHandler;
            this.orderLink = orderLink;
        }



        //[Authorize(Roles = "ADMIN")]
        [HttpPost("menu/")]
        public async Task<ActionResult<MenuItemDTO>> CreateMenuItem(CreateMenuItemDTO createMenuItemDTO)
        {
            var menuItemDTO = await menuItemHandler.CreateMenuItem(createMenuItemDTO);
            var categoryDTOs = await menuItemCategoryHandler.CreateMenuItemCategory(menuItemDTO.IdMenuItem, createMenuItemDTO.CategoryIds);
            var ingredientsDTOs = await menuItemIngredientHandler.CreateMenuItemIngredient(menuItemDTO.IdMenuItem, createMenuItemDTO.IngredientIds);
            menuItemDTO.Categories = categoryDTOs;
            menuItemDTO.Ingredients = ingredientsDTOs;

            return Ok(menuItemDTO);
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpGet("menu/")]
        public async Task<ActionResult<List<MenuItemDTO>>> GetMenuItems()
        {
            var menuItemDTOs = await menuItemHandler.GetMenuItems();
            foreach (MenuItemDTO menuItemDTO in menuItemDTOs)
            {
                menuItemDTO.Categories = await menuItemCategoryHandler.GetMenuItemCategories(menuItemDTO.IdMenuItem);
                menuItemDTO.Ingredients = await menuItemIngredientHandler.GetMenuItemIngredients(menuItemDTO.IdMenuItem);
            }
            
            return Ok(menuItemDTOs);
        }

        [HttpGet("menu/{idMenuItem}")]
        public async Task<ActionResult<MenuItemDTO>> GetMenuItemById([FromRoute] int idMenuItem)
        {
            var menuItemDTO = await menuItemHandler.GetMenuItemById(idMenuItem);

            //await menuItemCategoryHandler.DeleteMenuItemCategories(menuItemDTO.IdMenuItem);

            menuItemDTO.Categories = await menuItemCategoryHandler.GetMenuItemCategories(menuItemDTO.IdMenuItem);
            menuItemDTO.Ingredients = await menuItemIngredientHandler.GetMenuItemIngredients(menuItemDTO.IdMenuItem);

            return Ok(menuItemDTO);
        }

        [HttpPut("menu/{idMenuItem}")]
        public async Task<ActionResult<List<MenuItemDTO>>> UpdateMenu([FromRoute] int idMenuItem, [FromBody] UpdateMenuItemDTO updateMenuItemDTO)
        {
            var menuItemDTO = await menuItemHandler.UpdateMenuItem(updateMenuItemDTO, idMenuItem);

            await menuItemCategoryHandler.DeleteMenuItemCategories(idMenuItem);

            var categoryDTOs = await menuItemCategoryHandler.CreateMenuItemCategory(menuItemDTO.IdMenuItem, updateMenuItemDTO.CategoryIds);
            var ingredientsDTOs = await menuItemIngredientHandler.CreateMenuItemIngredient(menuItemDTO.IdMenuItem, updateMenuItemDTO.IngredientIds);

            menuItemDTO.Categories = categoryDTOs;
            menuItemDTO.Ingredients = ingredientsDTOs;
            return Ok(menuItemDTO);
        }


        [HttpDelete("menu/{idMenuItem}")]
        public async Task<ActionResult<bool>> DeleteMenu([FromRoute] int idMenuItem, [FromHeader] string authorization)
        {
            try
            {
                bool? isInUse = await orderLink.IsMenuItemInUse(idMenuItem, authorization);
                if (!isInUse.Value)
                    await menuItemCategoryHandler.DeleteMenuItemCategories(idMenuItem);
                else
                {
                    return Conflict("Menu item is in use.");
                }
                    var isDeleted = await menuItemHandler.DeleteMenuItem(idMenuItem);
                return Ok(isDeleted);
            }
            catch(Exception e)
            {
                return NotFound("Menu couldn't be found");
            }
           
            
        }

       
 
    }

}

