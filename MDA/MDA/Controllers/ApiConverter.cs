using CsvHelper;
using MDA.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MDA.Controllers
{
    public class ApiConverter : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> InitCSVFile()
        {

            string startDate = DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss")+".csv";

            createCSV(startDate);

            int startPage = 1;
            
            Root enResult = null;
            Root dkResult = null;
            for(int i = 65; i <= 68; i++)
            {
                try
                {



                    HttpWebRequest WebReqDk = (HttpWebRequest)WebRequest.Create(string.Format($"https://www.stormtextil.dk/en/api/v1/products?page={i}"));
                //HttpWebRequest WebReqEn = (HttpWebRequest)WebRequest.Create(string.Format($"https://www.stormtextil.dk/en/api/v1/products?page={startPage}"));


                WebReqDk.Method = "GET";
                WebReqDk.Credentials = new NetworkCredential("Casper@b2bpromotion.eu", "123456");
                HttpWebResponse WebRespDk = (HttpWebResponse)WebReqDk.GetResponse();

                //WebReqEn.Method = "GET";
                //WebReqEn.Credentials = new NetworkCredential("Casper@b2bpromotion.eu", "123456");
                //HttpWebResponse WebRespEn = (HttpWebResponse)WebReqEn.GetResponse();

                string jsonStringDk;
                using (Stream stream = WebRespDk.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    jsonStringDk = reader.ReadToEnd();
                }

                    //string jsonStringEn;
                    //using (Stream stream = WebRespEn.GetResponseStream())
                    //{
                    //    StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    //    jsonStringEn = reader.ReadToEnd();
                    //}

                    //enResult = JsonConvert.DeserializeObject<Rootobject>(jsonStringEn);

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    dkResult = JsonConvert.DeserializeObject<Root>(jsonStringDk, settings);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                    //dkResult = JsonConvert.DeserializeObject<IEnumerable<Rootobject>>(jsonStringDk);




#pragma warning disable CS8602 // Dereference of a possibly null reference.

                    foreach (Result result in dkResult.results)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    {
                        //for (int j = 0; j <= enResult.results.Length; j++)
                        //{
                        //    if (enResult.results[j].sku == result.sku)
                        //    {
                        //        result.enDescription == enResult.results[j].enDescription;
                        //        break;
                        //    }


                        //}

                        addRecord(result.name, result.sku, result.gtin, result.product_number, result.description, result.product_name, result.color_code, result.color, result.pantone, result.hex, result.size_code, result.size, result.price.amount, result.price.currency, result.stock, result.weight, result.tariff_number, result.country_of_origin, result.categories.main_category, result.categories.sub_categories, result.images.packShots, result.images.modelShots, result.images.washingInstructions, startDate);
                    }

                    startPage++;

            }
                catch (Exception ex) { Console.WriteLine(ex); }

        }

            return RedirectToAction("index");

        }

        public static void createCSV(string filePath)
        {
            using (StreamWriter file = new StreamWriter(filePath, true))
            {
                file.WriteLine("name;sku;gtin;product_number;descriptionDK;product_name;color_code;color;pantone;hex;size_code;size;price\\/amount;price\\/currency;stock;weight;tariff_number;country_of_origin;main_category;sub_categories;images\\/packshots;iamges\\/modelShots;washingInstructions");

            }
        }

        public static void addRecord(string name, string sku, string gtin, string product_number, string descriptionDK, string product_name, string color_code, string color, string pantone, string hex, string size_code, string size, double priceAmount, string priceCurrency, int stock, double weight, string tariff_number, string countryOfOrigin, string mainCategory, List<string>subCategory, List<string> packShots, List<string> modelShots, List<string> washingInstructions, string filePath) 
        {
            try
            {
                using (StreamWriter file = new StreamWriter(filePath, true, Encoding.GetEncoding("iso-8859-1")))
                {

                    string totalPackShots = "";

                    if(packShots != null)
                    {
                        foreach(var packShot in packShots)
                        {
                            totalPackShots += packShot + ", ";
                        }
                    }

                    string totalModelShots = "";

                    if(modelShots != null)
                    {
                        foreach (string elm in modelShots)
                        {
                            totalModelShots += elm + ", ";
                        }
                    }

                    string totalWashingInstructions = "";
                    if(washingInstructions != null)
                    {
                        foreach (string elm in washingInstructions)
                        {
                            totalWashingInstructions += elm + ", ";
                        }
                    }


                    string totalSubCategory = "";

                    if(subCategory != null)
                    {
                        foreach(string elm in subCategory)
                        {
                            totalSubCategory += elm + ", ";
                        }

                    }

                    //Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                    //Encoding utf8 = Encoding.UTF8;
                    //byte[] utfBytes = utf8.GetBytes(descriptionDK);
                    //byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                    //string convertedDesc = iso.GetString(isoBytes);

                    descriptionDK = descriptionDK.Replace(";", "");


                    if (product_name == "")
                    {
                        product_name = "No Name";
                    }

                    file.WriteLine(name + ";" + "'" + sku + ";" + gtin + ";" + product_number + ";" + descriptionDK + ";" + product_name + ";" + color_code + ";" + color + ";" + pantone + ";" + hex + ";" + size_code + ";" + size + ";" + priceAmount + ";" + priceCurrency + ";" + stock + ";" + weight + ";" + tariff_number + ";" + countryOfOrigin + ";" + mainCategory + ";" + totalSubCategory + ";" + totalPackShots + ";" + totalModelShots + ";" + totalWashingInstructions, file.Encoding.ToString());
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("This program did an oopsie :", ex);
            }
        }
    }
}
