﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineEdu.Business.Abstract;
using OnlineEdu.DTO.DTOs.AboutDtos;
using OnlineEdu.Entity.Entities;
using OnlineEdu.DTO.DTOs.BlogCategoryDtos;
using Microsoft.AspNetCore.Authorization;

namespace OnlineEdu.API.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCategoriesController(IBlogCategoryService _blogCategoryService, IMapper _mapper) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var values = _blogCategoryService.TGetCategoriesWithBlogs();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var value = _blogCategoryService.TGetById(id);
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _blogCategoryService.TDelete(id);
            return Ok("Blog Category Alanı Silindi");
        }

        [HttpPost]
        public IActionResult Create(CreateBlogCategoryDto createBlogCategoryDto)
        {
            var newValue = _mapper.Map<BlogCategory>(createBlogCategoryDto);
            _blogCategoryService.TCreate(newValue);
            return Ok("Yeni Blog Category Alanı Oluşturuldu");
        }

        [HttpPut]
        public IActionResult Update(UpdateBlogCategoryDto updateBlogCategoryDto)
        {
            var value = _mapper.Map<BlogCategory>(updateBlogCategoryDto);
            _blogCategoryService.TUpdate(value);
            return Ok("Blog Category Alanı Güncellendi");
        }
    }
}
