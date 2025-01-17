﻿using OnlineEdu.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineEdu;
using OnlineEdu.Business.Concrete;
using OnlineEdu.DataAccess.Abstract;
using OnlineEdu.Business.Abstract;


namespace OnlineEdu.DataAccess.Concrete
{
    public class BlogManager : GenericManager<Blog>, IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogManager(IRepository<Blog> _repository, IBlogRepository blogRepository) : base(_repository)
        {
            _blogRepository = blogRepository;
        }

        public List<Blog> TGetBlogsByCategoryId(int id)
        {
            return _blogRepository.GetBlogsByCategoryId(id);
        }

        public List<Blog> TGetBlogsWithCategories()
        {
            return _blogRepository.GetBlogsWithCategories();
        }

        public List<Blog> TGetBlogsWithCategoriesByWriterId(int id)
        {
            return _blogRepository.GetBlogsWithCategoriesByWriterId(id);
        }

        public Blog TGetBlogWithCategory(int id)
        {
            return _blogRepository.GetBlogWithCategory(id);
        }

        public List<Blog> TGetLast4BlogsWithCategories()
        {
            return _blogRepository.GetLast4BlogsWithCategories();
        }
    }
}
