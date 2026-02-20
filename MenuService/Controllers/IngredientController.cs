using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MenuService.Data;
using MenuService.DTO.Ingredient;
using MenuService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Controllers
{
    [ApiController]
    [Route("api/ingredients")]
    public class IngredientController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public IngredientController(MenuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var ingredients = _context.Ingredients
                .Select(i => new IngredientDto
                {
                    IdIngredient = i.IdIngredient,
                    IngredientName = i.IngredientName,
                    IsAllergen = i.IsAllergen
                })
                .ToList();

            return Ok(ingredients);
        }

        [HttpPost]
        public IActionResult Create(IngredientDto dto)
        {
            var ingredient = new Ingredient(dto.IngredientName, dto.IsAllergen);
            _context.Ingredients.Add(ingredient);
            _context.SaveChanges();

            return Ok(ingredient);
        }
    }
}