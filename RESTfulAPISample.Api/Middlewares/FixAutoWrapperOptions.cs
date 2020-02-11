namespace RESTfulAPISample.Middleware
{
    public class FixAutoWrapperOptions
    {
        public bool HttpStatusForce200 { get; set; } = false;
        public string SwaggerStartsWithSegments { get; set; } = "/swagger";
    }
}