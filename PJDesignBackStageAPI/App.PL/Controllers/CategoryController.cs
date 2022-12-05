using App.BLL;
using App.Model;
using App.PL.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        /// <summary>
        /// 取得分類ByID
        /// </summary>
        [HttpGet]
        [Route("GetCategoriesByUnitId")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetCategoriesByUnitIdResponse>>> GetCategoriesByUnitId(int id)
        {
            return await _service.GetCategoriesByUnitId(id);
        }

        /// <summary>
        /// 新增分類
        /// </summary>
        [HttpPost]
        [Route("CreateCategory")]
        [JwtFilter]
        public async Task<ResponseBase<string>> CreateCategory(CreateCategoryRequest request)
        {
            return await _service.CreateCategory(request);
        }

        /// <summary>
        /// 修改多分類
        /// </summary>
        [HttpPost]
        [Route("UpdateCategories")]
        [JwtFilter]
        public async Task<ResponseBase<string>> UpdateCategories(IEnumerable<UpdateCategoriesRequest> requests)
        {
            return await _service.UpdateCategories(requests);
        }
    }
}
