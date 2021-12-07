using SurveyOnline.Application.Interfaces;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using SurveyOnline.Shared.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyOnline.Application
{
    public class CategoryService: ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        private IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository CategoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = CategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(CategoryCreateRequest request)
        {
            Category category = new Category();
            category.Name = request.Name;
            category.Description = request.Description;
            await _categoryRepository.Add(category);
            var result = await _categoryRepository.Commit();
            return result;
        }

        public async Task<int> Delete(Category category)
        {
            _categoryRepository.Delete(category);
            var result=await _categoryRepository.Commit();
            return result;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<IEnumerable<Category>> GetAllPaging(string keyword)
        {
            var query = await _categoryRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.ToLower().IndexOf(keyword) >= 0);
            
            return query;
        }

        public async Task<Category> GetById(int id)
        {
            return await _categoryRepository.GetById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public async Task<int> Update(Category request)
        {  
            await _categoryRepository.Update(request);
            var result=await _categoryRepository.Commit();
            return result;
        }
    }
}
