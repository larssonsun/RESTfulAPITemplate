// TODO：[√b] Swagger (暂时没有在不使用时去除控制器相关的注释和特性)
// TODO：[√b] JWT
// TODO：[√b] response-cache
// TODO：[√b] in-memory-cache
// TODO：[√b] distributed caching （distributed-memory-cache, just for dev or test）
// TODO：[√b] DbInMemory/MSSql
// TODO：[√b] 使用AutoWrapper来全局处理api的返回信息（目前该中间件有缺陷，所以在之后加上我自己的FixAutoWrapperMiddleware中间件）
// TODO：[√b] FixAutoWrapperMiddleware 将CatchTheLastMiddleware中的isSwagger中的前缀设置与startUp中的UseSwaggerUI的swagger前缀设置都放到config中
// TODO：[√b] FixAutoWrapperMiddleware 将CathcTheLastMiddleware中的设置HttpStatusCode为200改为可选项作为中间件的option
// TODO：[√] dto validattion (fluent-validation)
// TODO：[√] finish CRUD, pagination, filter and sort
// TODO：[√] pagination adapter for local and distrubuted cache (use queerystring for cache key )
// TODO：[√] resouce shaping, ienumerable and single object
// TODO：pagination improve and abstruct
// TODO：NLog
// TODO：init DataSeed, AsNoTracking