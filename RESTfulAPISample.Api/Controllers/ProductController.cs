using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
#if (ENABLEJWTAUTHENTICATION)
using Microsoft.AspNetCore.Authorization;
#endif
#if (LOCALMEMORYCACHE)
using Microsoft.Extensions.Caching.Memory;
#elif (DISTRIBUTEDCACHE)
using Microsoft.Extensions.Caching.Distributed;
using MessagePack;
#endif
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using RESTfulAPISample.Core.DTO;
using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.Entity;
using RESTfulAPISample.Core.Interface;
using Larsson.RESTfulAPIHelper.Interface;
using Larsson.RESTfulAPIHelper.Pagination;
using Larsson.RESTfulAPIHelper.Shaping;

namespace RESTfulAPISample.Api.Controller
{
    /// <summary>
    /// Product
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]

#if (ENABLEJWTAUTHENTICATION)

    [Authorize]

#endif
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

#if (LOCALMEMORYCACHE)    

        private readonly IMemoryCache _cache;

#elif (DISTRIBUTEDCACHE)

        private readonly IDistributedCache _cache;

#endif

        private readonly IMapper _mapper;
        private readonly LinkGenerator _generator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _repository;
        private readonly IPropertyMappingContainer _propertyMappingContainer;
        private readonly ITypeHelperService _typeHelperService;

#if (LOCALMEMORYCACHE)

        public ProductController(ILogger<ProductController> logger, IMemoryCache cache, IMapper mapper, LinkGenerator generator,
            IUnitOfWork unitOfWork, IProductRepository repository, IPropertyMappingContainer propertyMappingContainer,
            ITypeHelperService typeHelperService)
        {
            _logger = logger;
            _cache = cache;
            _mapper = mapper;
            _generator = generator;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _propertyMappingContainer = propertyMappingContainer;
            _typeHelperService = typeHelperService;
        }

#elif (DISTRIBUTEDCACHE)

        public ProductController(ILogger<ProductController> logger, IDistributedCache cache, IMapper mapper, LinkGenerator generator,
            IUnitOfWork unitOfWork, IProductRepository repository, IPropertyMappingContainer propertyMappingContainer,
            ITypeHelperService typeHelperService)
        {
            _logger = logger;
            _cache = cache;
            _mapper = mapper;
            _generator = generator;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _propertyMappingContainer = propertyMappingContainer;
            _typeHelperService = typeHelperService;
        }
#else

        public ProductController(ILogger<ProductController> logger, IMapper mapper, LinkGenerator generator, IUnitOfWork unitOfWork,
        IProductRepository repository, IPropertyMappingContainer propertyMappingContainer, ITypeHelperService typeHelperService)
        {
            _logger = logger;
            _mapper = mapper;
            _generator = generator;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _propertyMappingContainer = propertyMappingContainer;
            _typeHelperService = typeHelperService;
        }

#endif

        #region snippet_GetProductsAsync
        /// <summary>
        /// Get Products
        /// </summary>
        /// <param name="queryStrParams">The params for filter and page products</param>
        /// <returns>products</returns>
        /// <response code="200">Returns the target products</response>
        /// <response code="304">Server-side data is not modified</response>
        /// <response code="401">Authorization verification is not passed</response>
        /// <response code="404">Did not get any product</response>
        /// <response code="406">Server does not support the media-type specified in the request</response>
        [HttpGet]

#if (ENABLEJWTAUTHENTICATION)

        [AllowAnonymous]
        
#endif

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<ProductDTO>> GetProductsAsync([FromQuery] ProductDTOParameters queryStrParams)
        {
            if (!_propertyMappingContainer.ValidMappingExistsFor<ProductDTO, Product>(queryStrParams.OrderBy))
            {
                return BadRequest("Can't find the fields for sorting.");
            }

            if (!_typeHelperService.TypeHasProperties<ProductDTO>(queryStrParams.Fields))
            {
                return BadRequest("Can't find the fields on DTO.");
            }

            PagedListBase<Product> pagedProducts;

#if (LOCALMEMORYCACHE)

            var cacheKey = Request.QueryString.Value;
            pagedProducts = await _cache.GetOrCreateAsync<PagedListBase<Product>>(cacheKey, async entry =>
            {
                Console.WriteLine("--------------------not from cache-----------------------");
                entry.Size = 2;
                entry.SetSlidingExpiration(TimeSpan.FromSeconds(15));
                return await _repository.GetProducts(queryStrParams);
            });

#elif (DISTRIBUTEDCACHE)

            var cacheKey = Request.QueryString.Value;
            var pagedProductsBytes = await _cache.GetAsync(cacheKey);
            if (pagedProductsBytes != null)
            {
                pagedProducts = MessagePackSerializer.Deserialize<PagedListBase<Product>>(pagedProductsBytes);
            }
            else
            {
                Console.WriteLine("--------------------not from cache-----------------------");
                pagedProducts = await _repository.GetProducts(queryStrParams);
                var productsResourceBytes = MessagePackSerializer.Serialize<PagedListBase<Product>>(pagedProducts);
                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(15));            
                await _cache.SetAsync(cacheKey, productsResourceBytes, options);
            }

#else

             pagedProducts = await _repository.GetProducts(queryStrParams);

#endif

            var filterProps = new Dictionary<string, object>();
            filterProps.Add("name", queryStrParams.Name);
            filterProps.Add("description", queryStrParams.Description);

            Response.SetPaginationHead(pagedProducts, queryStrParams, filterProps,
                values => _generator.GetUriByAction(HttpContext, controller: "Product", action: "GetProducts", values: values),
                meta => JsonSerializer.Serialize(meta, new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                })
            );
            var mappedProducts = _mapper.Map<IEnumerable<ProductDTO>>(pagedProducts);
            return Ok(mappedProducts.ToDynamicIEnumerable(queryStrParams.Fields));
        }

        #endregion

        #region snippet_GetProductAsync   
        /// <summary>
        /// Get product by productId
        /// </summary>
        /// <param name="id">productId</param>
        /// <param name="fields">fields to keep</param>
        /// <returns>product</returns>
        /// <response code="200">Returns the target product</response>
        /// <response code="304">Client-side data is up to date</response>
        /// <response code="401">Authorization verification is not passed</response>
        /// <response code="404">Did not get any product</response>
        /// <response code="406">Server does not support the media-type specified in the request</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<ActionResult<ProductDTO>> GetProductAsync(Guid id, string fields)
        {
            var result = await _repository.TryGetProduct(id);
            if (!result.hasProduct)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductDTO>(result.product).ToDynamic(fields));
        }
        #endregion

        #region snippet_GetProductsEachAsync
        /// <summary>
        /// Get products asynchronously by product id
        /// </summary>
        /// <returns>products</returns>
        /// <response code="200">Returns the target product</response>
        /// <response code="304">server-side cached data has not changed</response>
        /// <response code="401">Authorization verification is not passed</response>
        /// <response code="404">Did not get any product</response>
        /// <response code="406">Server does not support the media-type specified in the request</response>
        [HttpGet("async")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async IAsyncEnumerable<ProductDTO> GetProductsEachAsync() // larsson：IAsyncEnumerable 是 net core 3 中 c#8.0 的新特性
        {
            var products = _repository.GetProductsEachAsync();

            await foreach (var product in products)
            {
                if (product.IsOnSale)
                {
                    yield return _mapper.Map<ProductDTO>(product);
                }
            }
        }
        #endregion

        #region snippet_CreateProductAsync
        /// <summary>
        /// Create a product
        /// </summary>
        /// <param name="productCreateDTO">The product to be created</param>
        /// <returns>The created new product</returns>
        /// <response code="201">Create the product succeed and returns the newly created product</response>
        /// <response code="400">The product to be created is null</response>
        /// <response code="401">Authorization verification is not passed</response>
        /// <response code="406">Server does not support the media-type specified in the request</response>
        /// <response code="415">Server cannot accept the media-type of the incoming data.</response>
        /// <response code="422">DTO productCreateDTO failed to pass the model validation</response>
        /// <response code="500">Adding product service side failed</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)] // 格式数据请求使用[Consumes]，若没有该属性，则直接识别请求头中的Content-Type，也就是[Consumes]可以省略，只要Content-Type为你需要的就能进行数据的模型绑定 // larsson：实际上asp.net core 默认就是json
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDTO>> CreateProductAsync([FromBody]ProductCreateOrUpdateDTO productCreateDTO)
        {
            // larsson：这里必须startup中设置禁用自动400响应，SuppressModelStateInvalidFilter = true。否则Model验证失败后这里的ProductResource永远是null而无法返回422

            if (productCreateDTO == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState); // larsson：如果要自定义422之外的响应则需要新建一个类继承UnprocessableEntityObjectResult
            }

            var product = _mapper.Map<Product>(productCreateDTO);
            _repository.AddProduct(product);
            {
                if (!await _unitOfWork.SaveAsync())
                {
                    return StatusCode(500, "Adding product failed.");
                }

                var productDTO = _mapper.Map<ProductDTO>(product);

                // larsson：CreatedAtRoute在response中加入一个locaton头，包含GetProduct的调用
                return CreatedAtRoute(nameof(GetProductAsync), new
                {
                    id = product.Id
                }, productDTO);
            }
        }
        #endregion

        #region snippet_DeleteProductAsync
        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="productId">The id of product whitch to be deleting</param>
        /// <returns>Results of the product deleting</returns>
        /// <response code="204">Product successfully deleted</response>
        /// <response code="401">Authorization verification is not passed</response>
        /// <response code="404">Did not get any product</response>
        /// <response code="406">Server does not support the media-type specified in the request</response>
        /// <response code="500">Deleting product service side failed</response>
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProductAsync(Guid productId)
        {
            var result = await _repository.TryGetProduct(productId);
            if (!result.hasProduct)
            {
                return NotFound();
            }

            _repository.DeleteProduct(result.product);
            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "Delere product failed.");
            }

            return NoContent();
        }

        #endregion

        #region snippet_UpdateProductAsync
        /// <summary>
        /// Update a product
        /// </summary>
        /// <param name="productUpdateDTO">The product to be updated</param>
        /// <param name="productId">The id of the product to be updated</param>
        /// <returns>The update a product</returns>
        /// <response code="204">Updating product succeed</response>
        /// <response code="400">The product to be updated is null</response>
        /// <response code="401">Authorization verification is not passed</response>
        /// <response code="406">Server does not support the media-type specified in the request</response>
        /// <response code="412">Source of the product has changed</response> 
        /// <response code="415">Server cannot accept the media-type of the incoming data.</response>
        /// <response code="422">DTO productUpdateDTO failed to pass the model validation</response>
        /// <response code="500">Updating product service side failed</response>
        [HttpPut("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProductAsync(Guid productId, [FromBody]ProductUpdateDTO productUpdateDTO)
        {
            if (productUpdateDTO == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState); // larsson：如果要自定义422之外的响应则需要新建一个类继承UnprocessableEntityObjectResult
            }

            var result = await _repository.TryGetProduct(productId);
            if (!result.hasProduct)
            {
                return NotFound();
            }

            _mapper.Map(productUpdateDTO, result.product);
            _repository.UpdateProduct(result.product);

            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "Updating product field.");
            }

            return NoContent();
        }
        #endregion

        #region snippet_PartiallyUpdateProductAsync
        /// <summary>
        /// Partially Update a product
        /// </summary>
        /// <param name="patchDoc">The JsonPatchDocument for the product partially updating</param>
        /// <param name="productId">The id of the product to be partially updated</param>
        /// <returns>The partially update a product</returns>
        /// <response code="204">Partially updating product succeed</response>
        /// <response code="400">The product to be partially updated is null</response>
        /// <response code="401">Authorization verification is not passed</response>
        /// <response code="406">Server does not support the media-type specified in the request</response>
        /// <response code="412">Source of the product has changed</response> 
        /// <response code="415">Server cannot accept the media-type of the incoming data.</response>
        /// <response code="422">DTO productUpdateDTO failed to pass the model validation</response>
        /// <response code="500">Partially updating product service side failed</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch("{productId}")]
        public async Task<IActionResult> PartiallyUpdateProductAsync(Guid productId, [FromBody] Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ProductUpdateDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var result = await _repository.TryGetProduct(productId);
            if (!result.hasProduct)
            {
                return NotFound();
            }

            var productUpdateDTO = _mapper.Map<ProductUpdateDTO>(result.product);
            patchDoc.ApplyTo(productUpdateDTO, ModelState);

            TryValidateModel(productUpdateDTO);
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            _mapper.Map(productUpdateDTO, result.product);
            _repository.UpdateProduct(result.product);

            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "Patching product field.");
            }

            return NoContent();
        }
        #endregion


        // private string SetPaginationHead<TPaged, TQueryParams>(PaginatedList<TPaged> pagedEntity, TQueryParams queryParams,
        //     Dictionary<string, object> filterProps, string controllerName, string actionName)
        //     where TPaged : class
        //     where TQueryParams : PaginationBase
        // {
        //     var previousPageLink = pagedEntity.HasPrevious ? CreateProductsUri(queryParams, PaginationResourceUriType.PreviousPage, filterProps,
        //         (values) => _generator.GetUriByAction(HttpContext, actionName, controllerName, values)) : null;

        //     var nextPageLink = pagedEntity.HasNext ? CreateProductsUri(queryParams, PaginationResourceUriType.NextPage, filterProps,
        //         (values) => _generator.GetUriByAction(HttpContext, actionName, controllerName, values)) : null;

        //     var meta = new
        //     {
        //         pagedEntity.TotalItemsCount,
        //         pagedEntity.PaginationBase.PageSize,
        //         pagedEntity.PaginationBase.PageIndex,
        //         pagedEntity.PageCount,
        //         previousPageLink,
        //         nextPageLink
        //     };

        //     return JsonSerializer.Serialize(meta, new JsonSerializerOptions
        //     {
        //         Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        //         PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        //     });
        // }
        // private string CreateProductsUri<T>(T parameters, PaginationResourceUriType uriType, Dictionary<string, object> filterProps, Func<object, string> linkGenerator)
        //     where T : PaginationBase
        // {
        //     dynamic paginationParms = new System.Dynamic.ExpandoObject();

        //     paginationParms.pageIndex = parameters.PageIndex + (int)uriType;
        //     paginationParms.pageSize = parameters.PageSize;
        //     paginationParms.orderBy = parameters.OrderBy;
        //     paginationParms.fields = parameters.Fields;

        //     var dict = (paginationParms as IDictionary<string, object>);

        //     filterProps.Any(x =>
        //     {
        //         dict[x.Key] = x.Value;
        //         return false;
        //     });

        //     return linkGenerator(paginationParms);
        // }
    }
}