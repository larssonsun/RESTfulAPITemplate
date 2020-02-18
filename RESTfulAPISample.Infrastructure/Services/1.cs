// using System;
// using RESTfulAPISample.Core.DomainModel;

// namespace RESTfulAPISample.Infrastructure.Service
// {
//     public class CountryPropertyMapping : PropertyMapping<ProductDTO, Country>
//     {
//         public CountryPropertyMapping() : base(new Dictionary<string, List<MappedProperty>>
//             (StringComparer.OrdinalIgnoreCase)
//         {
//             [nameof(CountryResource.EnglishName)] = new List<MappedProperty>
//             {
//                 new MappedProperty{ Name = nameof(Country.EnglishName), Revert = false}
//             },
//             [nameof(CountryResource.ChineseName)] = new List<MappedProperty>
//             {
//                 new MappedProperty{ Name = nameof(Country.ChineseName), Revert = false}
//             },
//             [nameof(CountryResource.Abbreviation)] = new List<MappedProperty>
//             {
//                 new MappedProperty{ Name = nameof(Country.Abbreviation), Revert = false}
//             }
//         })
//         {
//         }
//     }
// }