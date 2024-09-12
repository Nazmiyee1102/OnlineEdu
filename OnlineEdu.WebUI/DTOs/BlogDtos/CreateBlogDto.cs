using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineEdu.WebUI.DTOs.BlogDtos;

namespace OnlineEdu.WebUI.DTOs.BlogDtos
{
    public class CreateBlogDto
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public DateTime BlogDate { get; set; }

        public int BlogCategoryId { get; set; }

    }
}
