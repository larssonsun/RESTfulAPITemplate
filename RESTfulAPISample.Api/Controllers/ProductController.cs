using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
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
using Microsoft.Extensions.Logging;
using RESTfulAPISample.Api.Resource;
using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.Interface;

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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _repository;

#if (LOCALMEMORYCACHE)

        public ProductController(ILogger<ProductController> logger, IMemoryCache cache, IMapper mapper, IUnitOfWork unitOfWork, IProductRepository repository)
        {
            _logger = logger;
            _cache = cache;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

#elif (DISTRIBUTEDCACHE)

        public ProductController(ILogger<ProductController> logger, IDistributedCache cache, IMapper mapper, IUnitOfWork unitOfWork, IProductRepository repository)
        {
            _logger = logger;
            _cache = cache;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
#else

        public ProductController(ILogger<ProductController> logger, IMapper mapper, IUnitOfWork unitOfWork, IProductRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

#endif

        #region snippet_Get
        /// <summary>
        /// Get Products
        /// </summary>
        /// <returns>products</returns>
        /// <response code="200">Returns the target products</response>
        /// <response code="401">If authorization verification is not passed</response>
        /// <response code="404">If you don't get any products</response>
        [HttpGet]

#if (ENABLEJWTAUTHENTICATION)

        [AllowAnonymous]
        
#endif

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IEnumerable<ProductResource>> Get()
        {

#if (LOCALMEMORYCACHE)
            Console.WriteLine("-------------------------mem");
            return await _cache.GetOrCreateAsync<IEnumerable<ProductResource>>("products-resource", async entry =>
            {
                entry.Size = 1;
                entry.SetSlidingExpiration(TimeSpan.FromSeconds(10));
                return _mapper.Map<IEnumerable<ProductResource>>(await _repository.GetProducts());
            });

#elif (DISTRIBUTEDCACHE)
            Console.WriteLine("-------------------------dis");
            IEnumerable<ProductResource> productsResource = null;
            var productsResourceBytes = await _cache.GetAsync("products-resource");
            if (productsResourceBytes != null)
                productsResource = MessagePackSerializer.Deserialize<IEnumerable<ProductResource>>(productsResourceBytes);

            if (productsResourceBytes == null)
            {
                var products = await _repository.GetProducts();
                productsResource = _mapper.Map<IEnumerable<ProductResource>>(products);
                var bytes = MessagePackSerializer.Serialize<IEnumerable<ProductResource>>(productsResource);
                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(10));
                await _cache.SetAsync("products-resource", bytes, options);
            }
            return productsResource;
#else

            return _mapper.Map<IEnumerable<ProductResource>>(await _repository.GetProducts());

#endif


        }

        #endregion

        #region snippet_GetById   
        /// <summary>
        /// Get product by productId
        /// </summary>
        /// <param name="id">productId</param>
        /// <returns>product</returns>
        /// <response code="200">Returns the target product</response>
        /// <response code="401">If authorization verification is not passed</response>
        /// <response code="404">If you don't get any product</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResource>> GetById(Guid id)
        {
            var result = await _repository.TryGetProduct(id);
            if (!result.hasProduct)
            {
                return NotFound();
            }

            return _mapper.Map<ProductResource>(result.product);
        }
        #endregion

        #region snippet_GetOnSaleProductsAsync
        /// <summary>
        /// Get products asynchronously by product id
        /// </summary>
        /// <returns>products</returns>
        /// <response code="200">Returns the target product</response>
        /// <response code="401">If authorization verification is not passed</response>
        /// <response code="404">If you don't get any product</response>
        [HttpGet("asyncsale")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async IAsyncEnumerable<ProductResource> GetOnSaleProductsAsync() // larsson：IAsyncEnumerable 是 net core 3 中 c#8.0 的新特性
        {
            var products = _repository.GetProductsAsync();

            await foreach (var product in products)
            {
                if (product.IsOnSale)
                {
                    yield return _mapper.Map<ProductResource>(product);
                }
            }
        }
        #endregion

        #region snippet_CreateAsync
        /// <summary>
        /// Create a product
        /// </summary>
        /// <param name="productDTO">The product to be created</param>
        /// <returns>The created new product</returns>
        /// <response code="201">Returns the newly created product</response>
        /// <response code="400">If the product to be created is null</response>
        /// <response code="401">If authorization verification is not passed</response>
        /// <response code="412">The source of the product has changed 这个暂时放在这里，应该放在put中</response> 
        /// <response code="422">DTO ProductResource failed to pass the model validation</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)] // 格式数据请求使用[Consumes]，若没有该属性，则直接识别请求头中的Content-Type，也就是[Consumes]可以省略，只要Content-Type为你需要的就能进行数据的模型绑定 // larsson：实际上asp.net core 默认就是json
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<ProductResource>> CreateAsync([FromBody]ProductAddResource productDTO)
        {
            if (productDTO == null)
            {
                return BadRequest();
            }

            // larsson：这里必须startup中设置禁用自动400响应，SuppressModelStateInvalidFilter = true。否则Model验证失败后这里的ProductResource永远是null

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState); // larsson：如果要自定义422之外的响应则需要新建一个类继承UnprocessableEntityObjectResult
            }

            var product = _mapper.Map<Product>(productDTO);
            _repository.AddProduct(product);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        #endregion
    }
}