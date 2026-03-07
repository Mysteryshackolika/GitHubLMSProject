using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMSProject.Models.ViewModels
{
    public class CourseSearchViewModel
    {
        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SortBy { get; set; }
        public List<Course> Courses { get; set; }
        public List<Category> Categories { get; set; }
        public SelectList CategoryList => new SelectList(Categories, "Id", "Name", CategoryId);

        public List<SelectListItem> SortOptions => new List<SelectListItem>
        {
            new SelectListItem { Value = "newest", Text = "Ən yeni" },
            new SelectListItem { Value = "popular", Text = "Ən populyar" },
            new SelectListItem { Value = "price_asc", Text = "Ucuzdan bahaya" },
            new SelectListItem { Value = "price_desc", Text = "Bahadan ucuza" }
        };

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public int TotalPages { get; set; }
    }
}