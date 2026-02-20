using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MenuService.Data;
using MenuService.DTO.Category;
using MenuService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public CategoryController(MenuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _context.Categories
                .Select(c => new CategoryDto
                {
                    IdCategory = c.IdCategory,
                    CategoryName = c.CategoryName
                })
                .ToList();

            return Ok(categories);
        }

        [HttpPost]
        public IActionResult Create(CategoryDto dto)
        {
            var category = new Category(dto.CategoryName);
            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok(category);
        }
    }
}