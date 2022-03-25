

using Newtonsoft.Json;

namespace MDA.Models
{

    public class Price
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class Images
    {
        public List<string> packShots { get; set; }
        public List<string> modelShots { get; set; }
        public List<string> washingInstructions { get; set; }
        public List<string> sizeCharts { get; set; }
    }

    public class Categories
    {
        public string main_category { get; set; }
        public List<string> sub_categories { get; set; }
    }

    public class Result
    {
        public string name { get; set; }
        public string sku { get; set; }
        public string gtin { get; set; }
        public string product_number { get; set; }
        public string product_name { get; set; }
        public string color_code { get; set; }
        public string color { get; set; }
        public string pantone { get; set; }
        public string hex { get; set; }
        public string size_code { get; set; }
        public string size { get; set; }
        public Price price { get; set; }
        public int stock { get; set; }
        public double weight { get; set; }
        public string tariff_number { get; set; }
        public string country_of_origin { get; set; }
        public Images images { get; set; }
        public Categories categories { get; set; }
        public string description { get; set; }
    }

    public class Pagination
    {
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int totalPages { get; set; }
        public int totalResults { get; set; }
    }

    public class Root
    {
        public List<Result> results { get; set; }
        public Pagination pagination { get; set; }
    }

}
