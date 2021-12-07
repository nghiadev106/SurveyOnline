using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Shared.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<int> Add(CategoryCreateRequest Category);

        Task<int> Update(Category category);

        Task<int> Delete(Category category);

        Task<IEnumerable<Category>> GetAll();

        Task<IEnumerable<Category>> GetAllPaging(string keyword);

        Task<Category> GetById(int id);

        void Save();
    }
}
