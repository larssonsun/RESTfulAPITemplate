using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
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
        [HttpGet]
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
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // 使用 [ProducesResponseType] 特性。 此特性可针对 Swagger 等工具生成的 API 帮助页生成更多描述性响应详细信息
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
        [HttpGet("asyncsale")]
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
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)] // 格式数据请求使用[Consumes]，若没有该属性，则直接识别请求头中的Content-Type，也就是[Consumes]可以省略，只要Content-Type为你需要的就能进行数据的模型绑定 // larsson：实际上asp.net core 默认就是json
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<ProductResource>> CreateAsync([FromBody]ProductAddResource ProductResource)
        {
            if (ProductResource == null)
            {
                return BadRequest();
            }

            // larsson：这里必须startup中设置禁用自动400响应，SuppressModelStateInvalidFilter = true。否则Model验证失败后这里的ProductResource永远是null

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState); // larsson：如果要自定义422之外的响应则需要新建一个类继承UnprocessableEntityObjectResult
            }

            var product = _mapper.Map<Product>(ProductResource);
            _repository.AddProduct(product);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        #endregion
    }
}