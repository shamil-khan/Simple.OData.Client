using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.OData;
using Microsoft.Data.Edm;
using Xunit;
using Moq;
using EdmSchemaElementKind = Microsoft.Data.Edm.EdmSchemaElementKind;
using IEdmModel = Microsoft.Data.Edm.IEdmModel;

namespace Simple.OData.Client.Tests
{
    public class ResponseReaderTests : TestBase
    {
        private const int productProperties = 10;
        private const int categoryProperties = 4;

        [Fact]
        public async Task GetSingleProduct()
        {
            var response = SetUpResourceMock("SingleProduct.xml");
            var responseReader = new ResponseReaderV3(await _client.GetMetadataAsync<IEdmModel>());
            var result = (await responseReader.GetResponseAsync(response)).Entry;
            Assert.Equal(productProperties, result.Count);
        }

        [Fact]
        public async Task GetMultipleProducts()
        {
            var response = SetUpResourceMock("MultipleProducts.xml");
            var responseReader = new ResponseReaderV3(await _client.GetMetadataAsync<IEdmModel>());
            var result = (await responseReader.GetResponseAsync(response)).Entries;
            Assert.Equal(20, result.Count());
            Assert.Equal(productProperties, result.First().Count);
        }

        [Fact]
        public async Task GetSingleProductWithCategory()
        {
            var response = SetUpResourceMock("SingleProductWithCategory.xml");
            var responseReader = new ResponseReaderV3(await _client.GetMetadataAsync<IEdmModel>());
            var result = (await responseReader.GetResponseAsync(response)).Entry;
            Assert.Equal(productProperties + 1, result.Count);
            Assert.Equal(categoryProperties, (result["Category"] as IDictionary<string, object>).Count);
        }

        [Fact]
        public async Task GetMultipleProductsWithCategory()
        {
            var response = SetUpResourceMock("MultipleProductsWithCategory.xml");
            var responseReader = new ResponseReaderV3(await _client.GetMetadataAsync<IEdmModel>());
            var result = (await responseReader.GetResponseAsync(response)).Entries;
            Assert.Equal(20, result.Count());
            Assert.Equal(productProperties + 1, result.First().Count);
            Assert.Equal(categoryProperties, (result.First()["Category"] as IDictionary<string, object>).Count);
        }

        [Fact]
        public async Task GetSingleCategoryWithProducts()
        {
            var response = SetUpResourceMock("SingleCategoryWithProducts.xml");
            var responseReader = new ResponseReaderV3(await _client.GetMetadataAsync<IEdmModel>());
            var result = (await responseReader.GetResponseAsync(response)).Entry;
            Assert.Equal(categoryProperties + 1, result.Count);
            Assert.Equal(12, (result["Products"] as IEnumerable<IDictionary<string, object>>).Count());
            Assert.Equal(productProperties, (result["Products"] as IEnumerable<IDictionary<string, object>>).First().Count);
        }

        [Fact]
        public async Task GetMultipleCategoriesWithProducts()
        {
            var response = SetUpResourceMock("MultipleCategoriesWithProducts.xml");
            var responseReader = new ResponseReaderV3(await _client.GetMetadataAsync<IEdmModel>());
            var result = (await responseReader.GetResponseAsync(response)).Entries;
            Assert.Equal(8, result.Count());
            Assert.Equal(categoryProperties + 1, result.First().Count);
            Assert.Equal(12, (result.First()["Products"] as IEnumerable<IDictionary<string, object>>).Count());
            Assert.Equal(productProperties, (result.First()["Products"] as IEnumerable<IDictionary<string, object>>).First().Count);
        }

        [Fact]
        public async Task GetSingleProductWithComplexProperty()
        {
            var response = SetUpResourceMock("SingleProductWithComplexProperty.xml");
            var responseReader = new ResponseReaderV3(null);
            var result = (await responseReader.GetResponseAsync(response)).Entry;
            Assert.Equal(productProperties + 1, result.Count);
            var quantity = result["Quantity"] as IDictionary<string, object>;
            Assert.NotNull(quantity);
            Assert.Equal(10d, quantity["Value"]);
            Assert.Equal("bags", quantity["Units"]);
        }

        [Fact]
        public async Task GetSingleProductWithCollectionOfPrimitiveProperties()
        {
            var response = SetUpResourceMock("SingleProductWithCollectionOfPrimitiveProperties.xml");
            var responseReader = new ResponseReaderV3(null);
            var result = (await responseReader.GetResponseAsync(response)).Entry;
            Assert.Equal(productProperties + 2, result.Count);
            var tags = result["Tags"] as IList<dynamic>;
            Assert.Equal(2, tags.Count);
            Assert.Equal("Bakery", tags[0]);
            Assert.Equal("Food", tags[1]);
            var ids = result["Ids"] as IList<dynamic>;
            Assert.Equal(2, ids.Count);
            Assert.Equal(1, ids[0]);
            Assert.Equal(2, ids[1]);
        }

        [Fact]
        public async Task GetSingleProductWithCollectionOfComplexProperties()
        {
            var response = SetUpResourceMock("SingleProductWithCollectionOfComplexProperties.xml");
            var responseReader = new ResponseReaderV3(null);
            var result = (await responseReader.GetResponseAsync(response)).Entry;
            Assert.Equal(productProperties + 1, result.Count);
            var tags = result["Tags"] as IList<dynamic>;
            Assert.Equal(2, tags.Count);
            Assert.Equal("Food", tags[0]["group"]);
            Assert.Equal("Bakery", tags[0]["value"]);
            Assert.Equal("Food", tags[1]["group"]);
            Assert.Equal("Meat", tags[1]["value"]);
        }

        [Fact]
        public async Task GetSingleProductWithEmptyCollectionOfComplexProperties()
        {
            var response = SetUpResourceMock("SingleProductWithEmptyCollectionOfComplexProperties.xml");
            var responseReader = new ResponseReaderV3(null);
            var result = (await responseReader.GetResponseAsync(response)).Entry;
            Assert.Equal(productProperties + 1, result.Count);
            var tags = result["Tags"] as IList<dynamic>;
            Assert.Equal(0, tags.Count);
        }

        // TODO: enums
        //[Fact]
        //public async Task GetSingleCustomerWithAddress()
        //{
        //    var response = SetUpResourceMock("SingleCustomerWithAddress.xml");
        //    var responseReader = new ResponseReaderV3(await _client.GetMetadataAsync<IEdmModel>());
        //    var result = (await responseReader.GetResponseAsync(response)).Entry;
        //    Assert.Equal(3, result.Count);
        //    Assert.Equal(5, (result["Address"] as IEnumerable<KeyValuePair<string, object>>).Count());
        //    Assert.Equal("Private", ((result["Address"] as IEnumerable<KeyValuePair<string, object>>)).First().Value);
        //}

        //[Fact]
        //public async Task GetNorthwindSchemaTableAssociations()
        //{
        //    var response = SetUpResourceMock("Northwind.edmx");
        //    var schema = Schema.FromMetadata(document);
        //    var EntitySet = schema.FindEntitySet("Product");
        //    //var association = EntitySet.FindAssociation("OrderDetails");
        //    //Assert.NotNull(association);
        //}

        //[Fact]
        //public async Task GetArtifactsSchemaTableAssociations()
        //{
        //    var response = SetUpResourceMock("Artifacts.edmx");
        //    var schema = Schema.FromMetadata(document);
        //    var EntitySet = schema.FindEntitySet("Product");
        //    //var association = EntitySet.FindAssociation("Artifacts");
        //    Assert.NotNull(association);
        //}

        [Fact]
        public async Task GetColorsSchema()
        {
            ParseSchema("Colors");
        }

        [Fact]
        public async Task GetFacebookSchema()
        {
            ParseSchema("Facebook");
        }

        [Fact]
        public async Task GetFlickrSchema()
        {
            ParseSchema("Flickr");
        }

        [Fact]
        public async Task GetGoogleMapsSchema()
        {
            ParseSchema("GoogleMaps");
        }

        [Fact]
        public async Task GetiPhoneSchema()
        {
            ParseSchema("iPhone");
        }

        [Fact]
        public async Task GetTwitterSchema()
        {
            ParseSchema("Twitter");
        }

        [Fact]
        public async Task GetYouTubeSchema()
        {
            ParseSchema("YouTube");
        }

        [Fact]
        public async Task GetNestedSchema()
        {
            ParseSchema("Nested");
        }

        [Fact]
        public async Task GetArrayOfNestedSchema()
        {
            ParseSchema("ArrayOfNested");
        }

        private void ParseSchema(string schemaName)
        {
            var document = GetResourceAsString(schemaName + ".edmx");
            var metadata = ODataClient.ParseMetadataString<IEdmModel>(document);
            var entityType = metadata.SchemaElements
                .Single(x => x.SchemaElementKind == EdmSchemaElementKind.TypeDefinition && 
                    (x as IEdmType).TypeKind == EdmTypeKind.Entity);
            Assert.Equal(schemaName, entityType.Name);
        }
    }
}
