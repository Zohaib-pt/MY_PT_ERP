using DAL.oClasses;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using Spire.Doc;
using Spire.Doc.Documents;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ZXing.QrCode;
using ZXing;
using System.Drawing;
using System.Text;
using Spire.Doc.Fields;
using System.Security.Cryptography;
using System.Reflection;


namespace PT_ERP.Controllers
{
    public class BasicDataController : BaseController
    {

        private readonly IOBasicData _oBasic;
        private readonly IOSales _oSale;
        private IConfiguration configuration;
        static string ApprovalURL;
        static string Urls;

        public BasicDataController(IOBasicData oBasicBase, IOSales oSale, IConfiguration iConfig)
           : base(oBasicBase)
        {
            _oBasic = oBasicBase;
            _oSale = oSale;
            configuration = iConfig;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Make()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Make List";
            ViewBag.PageTitle = "Make";
            //---Make list for page
            _oBasic.makeList = _oBasic.Get_MakeList_DAL(c_ID);


            return View(_oBasic);
        }

        [HttpPost]
        public IActionResult AddMake(string Make, IFormFile MakeAttachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (String.IsNullOrEmpty(Make))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Make", "BasicData");
            }

            var newFileName = "";
            var filepath = "";
            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            #region saving file
            if (MakeAttachment != null)
            {
                if (MakeAttachment.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (MakeAttachment.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(MakeAttachment.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            newFileName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filepath))
                            {
                                MakeAttachment.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        TempData["Error"] = "Images not saved successfully!";
                    }

                }

            }


            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            //---saving Make in database
            try
            {
                message = _oBasic.Insert_Make_DAL(Make, newFileName, filepath, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Make", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("Make", "BasicData");
            }

            //return View();
        }

        [HttpGet]
        public IActionResult Model()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Model Description List";
            ViewBag.PageTitle = "Model";
            //---Make list for drop down in forms
            _oBasic.makeList = _oBasic.Get_MakeList_DAL(c_ID).ToList();
            ViewBag.MakeList = _oBasic.makeList;

            //---Make MakeCategoryLists
            ViewBag.MakeCategoryList = _oBasic.Get_VehicleCategoryList_DAL(c_ID).ToList();

            //---Make country list
            ViewBag.MakeCountryList = _oBasic.Get_MakeCountryList_DAL(c_ID);


            //---model list for page
            _oBasic.ModelDescList = _oBasic.Get_ModelList_DAL(c_ID);


            //---Get EngineType
            ViewBag.EngineType = _oBasic.Get_EngineTypeList_DAL(c_ID).ToList();


            return View(_oBasic);
        }







        //---method for deleting make
        [HttpPost]
        public IActionResult DeleteMake(int? Make_ID, string ImageName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Make_ID == null || Make_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Make", "BasicData");
            }
            string message = "";

            #region Deleting actual file from folder
            try
            {
                string filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CommonImages", "OtherImages")).Root + $@"\{ImageName}";

                if ((System.IO.File.Exists(filepath)))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Make", "BasicData");

            }
            #endregion


            try
            {
                message = _oBasic.Delete_Make_DAL(Make_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("Make", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("Make", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Make", "BasicData");

            }


        }


        //method for deleting make
        [HttpPost]
        public IActionResult UpdateMake(int? Make_ID, string Make, IFormFile MakeAttachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(Make) || Make_ID == null || Make_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Make", "BasicData");
            }

            var newFileName = "";
            var filepath = "";
            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);


            #region saving file

            if (MakeAttachment != null)
            {
                if (MakeAttachment.Length > 0)
                {
                    try
                    {
                        string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                        string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                        string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;


                        if (MakeAttachment.Length > 0)
                        {
                            //Getting FileName
                            var fileName = Path.GetFileName(MakeAttachment.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            newFileName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filepath))
                            {
                                MakeAttachment.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        TempData["Error"] = "Images not saved successfully!";
                    }

                }

            }


            #endregion

            //---saving Make in database
            try
            {
                message = _oBasic.Update_Make_DAL(Make_ID, Make, newFileName, filepath, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("Make", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("Make", "BasicData");
            }

            //return View();
        }



        //get engine type
        [HttpGet]
        public IActionResult EngineType()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Engine Type List";
            ViewBag.PageTitle = "Engine Type";

            //---Make list for page
            _oBasic.EngineTypeList = _oBasic.Get_EngineTypeList_DAL(c_ID);


            return View(_oBasic);
        }

        //---Insert Engine Type
        [HttpPost]
        public IActionResult InsertEngineType(string Engine_Power)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(Engine_Power))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("EngineType", "BasicData");
            }




            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving engine Type in database
            try
            {
                message = _oBasic.Insert_EngineType_DAL(Engine_Power, Created_by, c_ID);
                if (message == "Saved Successfully!")
                {
                    TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("EngineType", "BasicData");
                }
                else
                {
                    TempData["Error"] = "Error occured in saving the record!";
                    return RedirectToAction("EngineType", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("EngineType", "BasicData");
            }

            //return View();
        }

        //---method for deleting Engine type
        [HttpPost]
        public IActionResult DeleteEngineType(int? EngineType_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (EngineType_ID == null || EngineType_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("EngineType", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_EngineType_DAL(EngineType_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("EngineType", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("EngineType", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("EngineType", "BasicData");

            }


        }

        //---Method for Updating Engine type
        [HttpPost]
        public IActionResult UpdateEngineType(int? EngineType_ID, string Engine_Power)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (String.IsNullOrEmpty(Engine_Power) || EngineType_ID == null)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("EngineType", "BasicData");
            }




            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);


            //---saving engine Type in database
            try
            {
                message = _oBasic.Update_EngineType_DAL(EngineType_ID, Engine_Power, Modified_by);
                if (message == "Updated Successfully!")
                {
                    TempData["Success"] = "Updated Successfully!";
                    return RedirectToAction("EngineType", "BasicData");
                }
                else
                {
                    TempData["Error"] = "Error occured in saving the record!";
                    return RedirectToAction("EngineType", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("EngineType", "BasicData");
            }

            //return View();
        }



        //---get make country
        [HttpGet]
        public IActionResult MakeCountry()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Accounts";
            ViewBag.SectionSub_HeaderTitle = "Make Country";
            ViewBag.PageTitle = "Make Country";
            //---Make list for page
            _oBasic.Make_countriesList = _oBasic.Get_MakeCountryList_DAL(c_ID);


            return View(_oBasic);
        }

        //---Insert MakeCountry 
        [HttpPost]
        public IActionResult InsertMakeCountry(string CountryCode, string CountryName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(CountryName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("MakeCountry", "BasicData");
            }




            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving engine Type in database
            try
            {
                message = _oBasic.Insert_MakeCountry_DAL(CountryCode, CountryName, Created_by, c_ID);
                if (message == "Saved Successfully!")
                {
                    TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("MakeCountry", "BasicData");
                }
                else
                {
                    TempData["Error"] = "Error occured in saving the record!";
                    return RedirectToAction("MakeCountry", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("MakeCountry", "BasicData");
            }

            //return View();
        }

        //---method for deleting MakeCountry 
        [HttpPost]
        public IActionResult DeleteMakeCountry(int? MakeCountry_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (MakeCountry_ID == null || MakeCountry_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("MakeCountry", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_MakeCountry_DAL(MakeCountry_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("MakeCountry", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("MakeCountry", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("MakeCountry", "BasicData");

            }


        }

        //---Method for Updating MakeCountry 
        [HttpPost]
        public IActionResult UpdateMakeCountry(int? MakeCountry_ID, string CountryCode, string CountryName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (String.IsNullOrEmpty(CountryName) || MakeCountry_ID == null)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("MakeCountry", "BasicData");
            }




            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);


            //---saving engine Type in database
            try
            {
                message = _oBasic.Update_MakeCountry_DAL(MakeCountry_ID, CountryCode, CountryName, Modified_by);
                if (message == "Updated Successfully!")
                {
                    TempData["Success"] = "Updated Successfully!";
                    return RedirectToAction("MakeCountry", "BasicData");
                }
                else
                {
                    TempData["Error"] = "Error occured in saving the record!";
                    return RedirectToAction("MakeCountry", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("MakeCountry", "BasicData");
            }

            //return View();
        }

        [HttpPost]
        public IActionResult InsertModelDescription(int? Make_ID, string ModelDesciption, int? VehCategory_ID, int? MakeCountry_ID, string Makecode, string Door, string HS_CODE, string DRIVE, string FUEL_TYPE, string EngineType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            if (Make_ID == null || String.IsNullOrEmpty(ModelDesciption))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Model", "BasicData");
            }




            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving engine Type in database
            try
            {
                message = _oBasic.Insert_ModelDesc_DAL(Make_ID, ModelDesciption, VehCategory_ID, MakeCountry_ID, Created_by, c_ID, Makecode, Door, HS_CODE, DRIVE, FUEL_TYPE, EngineType);
                if (message == "Saved Successfully!")
                {
                    TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("Model", "BasicData");
                }
                else
                {
                    TempData["Error"] = "Error occured in saving the record!";
                    return RedirectToAction("Model", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("Model", "BasicData");
            }

            //return View();
        }

        [HttpPost]
        public IActionResult UpdateModelDescription(int? ModelDesc_ID, int? Make_ID, string ModelDesciption, int? VehCategory_ID, int? MakeCountry_ID, string Makecode, string Door, string HS_CODE, string DRIVE, string FUEL_TYPE, string EngineType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Make_ID == null || String.IsNullOrEmpty(ModelDesciption))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Model", "BasicData");
            }




            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);


            //---saving engine Type in database
            try
            {
                message = _oBasic.Update_ModelDesc_DAL(ModelDesc_ID, Make_ID, ModelDesciption, VehCategory_ID, MakeCountry_ID, Modified_by, Makecode, Door, HS_CODE, DRIVE, FUEL_TYPE, EngineType);
                if (message == "Updated Successfully!")
                {
                    TempData["Success"] = "Updated Successfully!";
                    return RedirectToAction("Model", "BasicData");
                }
                else
                {
                    TempData["Error"] = "Error occured in saving the record!";
                    return RedirectToAction("Model", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("Model", "BasicData");
            }

            //return View();
        }


        //---method for deleting model desc 
        [HttpPost]
        public IActionResult DeleteModelDescription(int? ModelDesc_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (ModelDesc_ID == null || ModelDesc_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Model", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_ModelDesc_DAL(ModelDesc_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("Model", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("Model", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Model", "BasicData");

            }


        }


        [HttpGet]
        public IActionResult CarLocations()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Location List";

            ViewBag.PageTitle = "Car Locations";
            //---CarLocation list for page
            _oBasic.CarLocationList = _oBasic.Get_CarLocation_DAL(c_ID);
            ViewBag.LocTypeList = _oBasic.Get_LocType_DAL();

            return View(_oBasic);
        }


        [HttpPost]
        public IActionResult AddCarLocations(string CarLocation, int? Loc_TypeID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(CarLocation))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("CarLocations", "BasicData");
            }

            if (Loc_TypeID == 0)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("CarLocations", "BasicData");
            }




            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            string Location_Type_Name = _oBasic.Get_LocType_DAL()
                .Where(x => x.loctype_ID == Loc_TypeID)
                .Select(x => x.LocTypeName).SingleOrDefault();



            //---saving CarLocation in database
            try
            {
                message = _oBasic.Insert_CarLocation_DAL(CarLocation, Created_by, Location_Type_Name, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("CarLocations", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("CarLocations", "BasicData");
            }

            //return View();
        }



        //---method for deleting CarLocation
        [HttpPost]
        public IActionResult DeleteCarLocations(int? CarLocation_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (CarLocation_ID == null || CarLocation_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("CarLocations", "BasicData");
            }
            string message = "";



            try
            {
                message = _oBasic.Delete_CarLocation_DAL(CarLocation_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("CarLocations", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("CarLocations", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("CarLocations", "BasicData");

            }


        }


        //method for deleting CarLocation
        [HttpPost]
        public IActionResult UpdateCarLocations(int? CarLocation_ID, string CarLocation)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(CarLocation) || CarLocation_ID == null || CarLocation_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("CarLocations", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);



            //---saving CarLocation in database
            try
            {
                message = _oBasic.Update_CarLocation_DAL(CarLocation_ID, CarLocation, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("CarLocations", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("CarLocations", "BasicData");
            }

            //return View();
        }






        [HttpGet]
        public IActionResult PortOfExit()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Port Of Exit List";

            ViewBag.PageTitle = "Port Of Exit";
            //---PortOfExit list for page
            _oBasic.PortOfExitList = _oBasic.Get_PortOfExit_DAL(c_ID);


            return View(_oBasic);
        }

        [HttpPost]
        public IActionResult AddPortOfExit(string PortOfExit)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(PortOfExit))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PortOfExit", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);




            //---saving PortOfExit in database
            try
            {
                message = _oBasic.Insert_PortOfExit_DAL(PortOfExit, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("PortOfExit", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("PortOfExit", "BasicData");
            }

            //return View();
        }

        //---method for deleting PortOfExit
        [HttpPost]
        public IActionResult DeletePortOfExit(int? PortOfExit_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (PortOfExit_ID == null || PortOfExit_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("PortOfExit", "BasicData");
            }
            string message = "";



            try
            {
                message = _oBasic.Delete_PortOfExit_DAL(PortOfExit_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("PortOfExit", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("PortOfExit", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("PortOfExit", "BasicData");

            }


        }


        //method for deleting PortOfExit
        [HttpPost]
        public IActionResult UpdatePortOfExit(int? PortOfExit_ID, string PortOfExit)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(PortOfExit) || PortOfExit_ID == null || PortOfExit_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PortOfExit", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);



            //---saving PortOfExit in database
            try
            {
                message = _oBasic.Update_PortOfExit_DAL(PortOfExit_ID, PortOfExit, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("PortOfExit", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("PortOfExit", "BasicData");
            }

            //return View();
        }





        [HttpGet]
        public IActionResult Destinations()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Destinations List";

            ViewBag.PageTitle = "Destinations";
            //---Destinations list for page
            _oBasic.DestinationsList = _oBasic.Get_Destinations_DAL(c_ID);


            return View(_oBasic);
        }

        [HttpPost]
        public IActionResult AddDestinations(string Destinations)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(Destinations))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Destinations", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);




            //---saving Destinations in database
            try
            {
                message = _oBasic.Insert_Destinations_DAL(Destinations, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Destinations", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("Destinations", "BasicData");
            }

            //return View();
        }

        //---method for deleting Destinations
        [HttpPost]
        public IActionResult DeleteDestinations(int? Destinations_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Destinations_ID == null || Destinations_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Destinations", "BasicData");
            }
            string message = "";



            try
            {
                message = _oBasic.Delete_Destinations_DAL(Destinations_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("Destinations", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("Destinations", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Destinations", "BasicData");

            }


        }


        //method for deleting Destinations
        [HttpPost]
        public IActionResult UpdateDestinations(int? Destinations_ID, string Destinations)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(Destinations) || Destinations_ID == null || Destinations_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Destinations", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);



            //---saving Destinations in database
            try
            {
                message = _oBasic.Update_Destinations_DAL(Destinations_ID, Destinations, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("Destinations", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("Destinations", "BasicData");
            }

            //return View();
        }



        //----new added method

        [HttpGet]
        public ActionResult ModelYear()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Year";
            ViewBag.PageTitle = "Model Year";

            //---Make list for page remember this to bind list Later on
            _oBasic.YearList = _oBasic.Get_Year_List_DAL(c_ID);

            return View(_oBasic);

        }
        //add model year
        public IActionResult AddModelYear(string ModelYear)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(ModelYear))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("ModelYear", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving Color in database
            try
            {
                message = _oBasic.Insert_YEAR_DAL(ModelYear, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("ModelYear", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("ModelYear", "BasicData");
            }

            //return View();
        }
        //Make Description by noor
        //update model year
        [HttpPost]
        public IActionResult UpdateModelYear(int? ModelYear_ID, string ModelYear)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(ModelYear) || ModelYear_ID == null || ModelYear_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("ModelYear", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);


            //---saving Make in database
            try
            {
                message = _oBasic.Update_Year_DAL(ModelYear_ID, ModelYear, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("ModelYear", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("ModelYear", "BasicData");
            }

            //return View();
        }
        //delete
        [HttpPost]
        public IActionResult DeleteYear(int? ModelYear_ID, string ModelYear)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (ModelYear_ID == null || ModelYear_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("ModelYear", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_Year_DAL(ModelYear_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("ModelYear", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("ModelYear", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("ModelYear", "BasicData");

            }


        }


        [HttpGet]
        public ActionResult Global_Location()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Make Description List";

            ViewBag.PageTitle = "Global Location";

            //---Make list for page remember this to bind list
            _oBasic.CountryList = _oBasic.Get_CountriesList_DAL(c_ID);
            ViewBag.LocTypeList = _oBasic.Get_LocType_DAL();
            //_oBasic watch it later
            return View(_oBasic);
        }
        public IActionResult AddGlobalLocation(string CountryName, int? Loc_TypeID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(CountryName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Global_Location", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving Color in database
            try
            {
                message = _oBasic.Insert_GlobalLocation_DAL(CountryName, Loc_TypeID, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Global_Location", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("Global_Location", "BasicData");
            }

            //return View();
        }
        [HttpPost]
        public IActionResult UpdateGlobalLocation(int? Country_ID, string CountryName, int? Loc_TypeID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(CountryName) || Country_ID == null || Country_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Global_Location", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);


            //---saving Make in database
            try
            {
                message = _oBasic.Update_Country_DAL(Country_ID, CountryName, Loc_TypeID, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("Global_Location", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("Global_Location", "BasicData");
            }

            //return View();
        }
        //Global Location Delete
        [HttpPost]
        public IActionResult DeleteGlobalLocation(int? Country_ID, string CountryName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Country_ID == null || Country_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Global_Location", "BasicData");
            }
            string message = "";

            try
            {

                message = _oBasic.Delete_GlobalLocation_DAL(Country_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("Global_Location", "BasicData");
                }
                else
                {
                    TempData["Error"] = "Some thing went wrong. Please try again!";
                    return RedirectToAction("Global_Location", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Global_Location", "BasicData");

            }


        }

        [HttpGet]
        public ActionResult Category()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Make Description Lis";
            ViewBag.PageTitle = "Category";


            //---Make list for page remember this to bind list
            _oBasic.CategoryList = _oBasic.Get_VehicleCategory_DAL(c_ID);
            return View(_oBasic);
        }
        //add category
        public IActionResult AddCategory(string VehCategoryName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(VehCategoryName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Category", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving Color in database
            try
            {
                message = _oBasic.Insert_Category_DAL(VehCategoryName, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Category", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("Category", "BasicData");
            }

            //return View();
        }
        [HttpPost]
        //update category 
        public IActionResult UpdateCategory(int? VehCategory_ID, string VehCategoryName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(VehCategoryName) || VehCategory_ID == null || VehCategory_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Category", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);


            //---saving Make in database
            try
            {
                message = _oBasic.Update_Category_DAL(VehCategory_ID, VehCategoryName, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("Category", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("Category", "BasicData");
            }

            //return View();
        }
        //delete category working here now
        [HttpPost]
        public IActionResult DeleteCategory(int? VehCategory_ID, string VehCategoryName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (VehCategory_ID == null || VehCategory_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Color", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_Category_DAL(VehCategory_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("Category", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("Category", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Category", "BasicData");

            }


        }

        [HttpGet]
        public ActionResult Color()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Make Description List";
            ViewBag.PageTitle = "Color";
            //---Make list for page
            _oBasic.ColorList = _oBasic.Get_ColorList_DAL(c_ID);

            return View(_oBasic);
        }

        //end here
        //AddColor start here
        public IActionResult AddColor(string ColorName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(ColorName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Color", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving Color in database
            try
            {
                message = _oBasic.Insert_Color_DAL(ColorName, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Color", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("Color", "BasicData");
            }

            //return View();
        }

        //UpdateColor by noor
        [HttpPost]
        public IActionResult UpdateColor(int? Color_ID, string ColorName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(ColorName) || Color_ID == null || Color_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Make", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);


            //---saving Make in database
            try
            {
                message = _oBasic.Update_Color_DAL(Color_ID, ColorName, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("Color", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("Color", "BasicData");
            }

            //return View();
        }


        //updatecolor end
        //delete work start by noor
        [HttpPost]
        public IActionResult DeleteColor(int? Color_ID, string ColorName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Color_ID == null || Color_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Color", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_Color_DAL(Color_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("Color", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("Color", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Color", "BasicData");

            }


        }


        //delete work end
        //addcolorendhere
        //end




        [HttpGet]
        public IActionResult Partners()
        {


            #region ViewBag Area
            //---Ven Category list for drop down in forms
            ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Vendor List";

            ViewBag.PageTitle = "Vendors";

            #endregion

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            //---Partners list for page
            _oBasic.PartnersList = _oBasic.Get_Partners_DAL(c_ID);


            return View(_oBasic);
        }

        [HttpPost]
        public IActionResult AddPartnersVendor(string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax,
            string EmiratesID, string TradeLicenseNo, string TRN, int? VendorCat_ID, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
 string Remarks, string SellerType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            // Saving complete path(url) of page from where this ActionResult method called.This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();

            if (String.IsNullOrEmpty(PartnerName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            string PartnerType = "VR";


            //---saving Partners in database
            try
            {
                message = _oBasic.Insert_Partners_DAL(PartnerName, ContactNumber, MobileNo, ContactName, ContactAddress, Email, Fax, EmiratesID, TradeLicenseNo, TRN, BalanceType, OpeningBalanceDate, OpeningBalance, VendorCat_ID, PartnerType, Remarks, Created_by, c_ID, SellerType);

                if (message == "Saved Successfully!")
                {
                    TempData["Success"] = "Saved Vendor Successfully!";

                    return Redirect(url);
                }
                else
                {
                    TempData["Error"] = "Please fill all required fields!";
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return Redirect(url);
            }

            //return View();
        }

        //---method for deleting Partners
        [HttpPost]
        public IActionResult DeletePartners(int? Partner_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Partner_ID == null || Partner_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Partners", "BasicData");
            }
            string message = "";



            try
            {
                message = _oBasic.Delete_Partners_DAL(Partner_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("Partners", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("Partners", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Partners", "BasicData");

            }


        }


        //method for update Partners
        [HttpPost]
        public IActionResult UpdatePartners(int? Partner_ID, string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
 int? VendorCat_ID, string Remarks, string SellerType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(PartnerName) || Partner_ID == null || Partner_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Partners", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);



            //---saving Partners in database
            try
            {
                message = _oBasic.Update_Partners_DAL(Partner_ID, PartnerName, ContactNumber, MobileNo, ContactName, ContactAddress, Email, Fax, EmiratesID, TradeLicenseNo, TRN, BalanceType, OpeningBalanceDate, OpeningBalance, VendorCat_ID, Remarks, Modified_by, SellerType);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("Partners", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("Partners", "BasicData");
            }

            //return View();
        }

        [HttpGet]
        public IActionResult PartnersGarage()
        {


            #region ViewBag Area
            //---Ven Category list for drop down in forms
            ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Garage List";

            ViewBag.PageTitle = "Garages";
            #endregion

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            //---Partners list for page
            _oBasic.PartnersGarrageList = _oBasic.Get_PartnersGarrage_DAL(c_ID);


            return View(_oBasic);
        }

        [HttpPost]
        public IActionResult AddPartnersGarrage(string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
string Remarks, string SellerType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(PartnerName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PartnersGarage", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            string PartnerType = "GR";
           int  VendorCat_ID = 0;


            //---saving Partners in database
            try
            {
                message = _oBasic.Insert_Partners_DAL(PartnerName, ContactNumber, MobileNo, ContactName, ContactAddress, Email, Fax, EmiratesID, TradeLicenseNo, TRN, BalanceType, OpeningBalanceDate, OpeningBalance, VendorCat_ID, PartnerType, Remarks, Created_by, c_ID, SellerType);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("PartnersGarage", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("PartnersGarage", "BasicData");
            }

            //return View();
        }

        //---method for deleting Partners
        [HttpPost]
        public IActionResult DeletePartnersGarrage(int? Partner_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Partner_ID == null || Partner_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("PartnersGarage", "BasicData");
            }
            string message = "";



            try
            {
                message = _oBasic.Delete_Partners_DAL(Partner_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("PartnersGarage", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("PartnersGarage", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("PartnersGarage", "BasicData");

            }


        }


        //method for update Partners
        [HttpPost]
        public IActionResult UpdatePartnersGarrage(int? Partner_ID, string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
 string Remarks,string SellerType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(PartnerName) || Partner_ID == null || Partner_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PartnersGarage", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int VendorCat_ID = 0;


            //---saving Partners in database
            try
            {
                message = _oBasic.Update_Partners_DAL(Partner_ID, PartnerName, ContactNumber, MobileNo, ContactName, ContactAddress, Email, Fax, EmiratesID, TradeLicenseNo, TRN, BalanceType, OpeningBalanceDate, OpeningBalance, VendorCat_ID, Remarks, Modified_by, SellerType);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("PartnersGarage", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("PartnersGarage", "BasicData");
            }

            //return View();
        }



        [HttpGet]
        public IActionResult PartnersSeller()
        {


            #region ViewBag Area
            //---Ven Category list for drop down in forms
            ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Seller List";

            ViewBag.PageTitle = "Sellers";
            #endregion

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            //---Partners list for page
            _oBasic.PartnersSellerList = _oBasic.Get_PartnersSeller_DAL(c_ID);


            return View(_oBasic);
        }

        [HttpPost]
        public IActionResult AddPartnersSeller(string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
 int? VendorCat_ID, string Remarks,string SellerType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(PartnerName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PartnersSeller", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            string PartnerType = "SR";



            //---saving Partners in database
            try
            {
                message = _oBasic.Insert_Partners_DAL(PartnerName, ContactNumber, MobileNo, ContactName, ContactAddress, Email, Fax, EmiratesID, TradeLicenseNo, TRN, BalanceType, OpeningBalanceDate, OpeningBalance, VendorCat_ID, PartnerType, Remarks, Created_by, c_ID, SellerType);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("PartnersSeller", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("PartnersSeller", "BasicData");
            }

            //return View();
        }
        //---method for deleting Partners
        [HttpPost]
        public IActionResult DeletePartnersSeller(int? Partner_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Partner_ID == null || Partner_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("PartnersSeller", "BasicData");
            }
            string message = "";



            try
            {
                message = _oBasic.Delete_Partners_DAL(Partner_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("PartnersSeller", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("PartnersSeller", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("PartnersSeller", "BasicData");

            }


        }


        //method for update Partners
        [HttpPost]
        public IActionResult UpdatePartnersSeller(int? Partner_ID, string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
 int? VendorCat_ID, string Remarks, string SellerType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(PartnerName) || Partner_ID == null || Partner_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PartnersSeller", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);



            //---saving PartnersSeller in database
            try
            {
                message = _oBasic.Update_Partners_DAL(Partner_ID, PartnerName, ContactNumber, MobileNo, ContactName, ContactAddress, Email, Fax, EmiratesID, TradeLicenseNo, TRN, BalanceType, OpeningBalanceDate, OpeningBalance,
  VendorCat_ID, Remarks, Modified_by, SellerType);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("PartnersSeller", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("PartnersSeller", "BasicData");
            }

            //return View();
        }


        public IActionResult PartnersAgent()
        {


            #region ViewBag Area
            //---Ven Category list for drop down in forms
            ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Agent List";
            ViewBag.PageTitle = "Agents";
            #endregion

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            //---Partners list for page
            _oBasic.PartnersAgentList = _oBasic.Get_PartnersAgent_DAL(c_ID);


            return View(_oBasic);
        }

        [HttpPost]
        public IActionResult AddPartnersAgent(string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
 int? VendorCat_ID, string Remarks,string SellerType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(PartnerName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PartnersAgent", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            string PartnerType = "AR";



            //---saving Partners in database
            try
            {
                message = _oBasic.Insert_Partners_DAL(PartnerName, ContactNumber, MobileNo, ContactName, ContactAddress, Email, Fax, EmiratesID, TradeLicenseNo, TRN, BalanceType, OpeningBalanceDate, OpeningBalance,
VendorCat_ID, PartnerType, Remarks, Created_by, c_ID, SellerType);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("PartnersAgent", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("PartnersAgent", "BasicData");
            }

            //return View();
        }
        //---method for deleting Partners
        [HttpPost]
        public IActionResult DeletePartnersAgent(int? Partner_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Partner_ID == null || Partner_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("PartnersAgent", "BasicData");
            }
            string message = "";



            try
            {
                message = _oBasic.Delete_Partners_DAL(Partner_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("PartnersAgent", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("PartnersAgent", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("PartnersAgent", "BasicData");

            }


        }


        //method for update Partners
        [HttpPost]
        public IActionResult UpdatePartnersAgent(int? Partner_ID, string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
int? VendorCat_ID, string Remarks ,string SellerType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(PartnerName) || Partner_ID == null || Partner_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PartnersAgent", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);



            //---saving Partners in database
            try
            {
                message = _oBasic.Update_Partners_DAL(Partner_ID, PartnerName, ContactNumber, MobileNo, ContactName, ContactAddress, Email, Fax, EmiratesID, TradeLicenseNo, TRN, BalanceType, OpeningBalanceDate, OpeningBalance,
 VendorCat_ID, Remarks, Modified_by, SellerType);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("PartnersAgent", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("PartnersAgent", "BasicData");
            }

            //return View();
        }

        #region CUSTOMER

        [HttpGet]
        public IActionResult CustomersMaster()
        {


            #region ViewBag Area
            ViewBag.PageTitle = "Customer Master";
            //---Ven Category list for drop down in forms
            ViewBag.CustomerCategoryList = _oBasic.Get_CustomerCategory_DAL().ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;



            #endregion

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            //---CustomersMaster list for page
            _oBasic.CustomerMasterList = _oBasic.Get_CustomersMaster_DAL(c_ID);


            return View(_oBasic);
        }

        [HttpPost]
        public IActionResult AddCustomersMaster(string CustomerName, string ContactNumber, string MobileNo, string ContactName,
            string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN,
            string BalanceType, string OpeningBalanceDate, string OpeningBalance,
            int? CustomerCat_ID, string Remarks)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(CustomerName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("CustomersMaster", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            // Saving complete path(url) of page from where this ActionResult method called.This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();



            //---saving CustomersMaster in database
            try
            {
                message = _oBasic.Insert_CustomersMaster_DAL(CustomerName, ContactNumber, MobileNo, ContactName, ContactAddress, Email, Fax,
                    EmiratesID, TradeLicenseNo, TRN, OpeningBalance, OpeningBalanceDate, BalanceType, CustomerCat_ID, Remarks, Created_by, c_ID);
                if (message == "Saved Successfully!")
                {
                    TempData["Success"] = "Saved Successfully";
                    return Redirect(url);
                }
                else
                {

                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }

        //---method for deleting CustomersMaster
        [HttpPost]
        public IActionResult DeleteCustomersMaster(int? Customer_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Customer_ID == null || Customer_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("CustomersMaster", "BasicData");
            }
            string message = "";



            try
            {
                message = _oBasic.Delete_CustomersMaster_DAL(Customer_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("CustomersMaster", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("CustomersMaster", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("CustomersMaster", "BasicData");

            }


        }


        //method for update CustomersMaster
        [HttpPost]
        public IActionResult UpdateCustomersMaster(int? Customer_ID, string CustomerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN,
             string BalanceType, string OpeningBalanceDate, string OpeningBalance,
            int? CustomerCat_ID, int? Customer_Sno, string Remarks)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(CustomerName) || Customer_ID == null || Customer_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("CustomersMaster", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);



            //---saving CustomersMaster in database
            try
            {
                message = _oBasic.Update_CustomersMaster_DAL(Customer_ID, CustomerName, ContactNumber, MobileNo, ContactName,
                    ContactAddress, Email, Fax, EmiratesID, TradeLicenseNo, TRN, OpeningBalance, OpeningBalanceDate, BalanceType, CustomerCat_ID, Remarks, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("CustomersMaster", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("CustomersMaster", "BasicData");
            }

            //return View();
        }

        #endregion CUSTOMER



        [HttpGet]
        public IActionResult ShowRoom()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.PageTitle = "Show Room";
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            //---ShowRoom list for page
            _oBasic.ShowRoomList = _oBasic.Get_ShowRoom_DAL(c_ID);


            return View(_oBasic);
        }

        [HttpPost]
        public IActionResult AddShowRoom(string ShowRoomCode, string ShowRoomName, string showroom_no, string TRN)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(ShowRoomName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("ShowRoom", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);




            //---saving ShowRoom in database
            try
            {
                message = _oBasic.Insert_ShowRoom_DAL(ShowRoomCode, ShowRoomName, showroom_no, TRN, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("ShowRoom", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("ShowRoom", "BasicData");
            }

            //return View();
        }


        //method for deleting ShowRoom
        [HttpPost]
        public IActionResult UpdateShowRoom(int? ShowRoom_ID, string ShowRoomCode, string ShowRoomName, string showroom_no, string TRN)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(ShowRoomName) || ShowRoom_ID == null || ShowRoom_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("ShowRoom", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);



            //---saving ShowRoom in database
            try
            {
                message = _oBasic.Update_ShowRoom_DAL(ShowRoom_ID, ShowRoomCode, ShowRoomName, showroom_no, TRN, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("ShowRoom", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("ShowRoom", "BasicData");
            }

            //return View();
        }


        //---Get customer attachment list 
        [HttpGet]
        public IActionResult GetCustomerMaster_Attachments(int? Customer_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //--send this value hard coded from here for getting attachment for customer master
            string Type = "BasicData_CustomerMaster";

            try
            {
                _oBasic.attachmentList = _oBasic.GetCustomerMaster_Attachments_DAL(Customer_ID, Type).ToList();

                return PartialView("_CustomerMasterAttachments", _oBasic);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }
        }

        //---insert customer attachment list
        [HttpPost]
        public IActionResult InsertAttachments_CustomerMaster(int? Customer_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (Customer_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
            {

                return Json(new { message = "Please fill all required fields!" });
            }

            var newFileName = "";
            var filepath = "";
            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            #region saving file
            if (Attachment != null)
            {
                if (Attachment.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (Attachment.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(Attachment.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            newFileName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filepath))
                            {
                                Attachment.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        return Json(new { message = ErrorMessage });
                    }

                }

            }


            #endregion

            //---saving attachment in database
            try
            {

                message = _oBasic.InsertAttachments_CustomerMaster_DAL(Customer_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oBasic.attachmentList = _oBasic.GetCustomerMaster_Attachments_DAL(Customer_ID, Type).ToList();

                    return PartialView("_CustomerMasterAttachments", _oBasic);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }

        //---delete attachment of customer 
        [HttpGet]
        public IActionResult DeleteAttachmentCustomerMaster(int? Customer_ID, int? Attachment_ID, string Type, string File_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";

            if (Customer_ID == null)
            {
                //TempData["Error"] = "Please fill all required fields!";
                return Json(new { message = "Customer_ID id is null" });
            }

            if (Attachment_ID == null)
            {

                return Json(new { message = "Attachment_ID is null" });
            }
            if (String.IsNullOrEmpty(Type))
            {

                return Json(new { message = "Document type is null" });
            }


            #region Deleting actual file from folder
            try
            {
                string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                string filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{File_Name}";

                if ((System.IO.File.Exists(filepath)))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });

            }
            #endregion



            try
            {

                message = _oSale.Delete_Attachment(Attachment_ID);
                if (message == "Deleted Successfully!")
                {

                    _oBasic.attachmentList = _oBasic.GetCustomerMaster_Attachments_DAL(Customer_ID, Type).ToList();

                    return PartialView("_CustomerMasterAttachments", _oBasic);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }


        //---Get vendor attachment list 
        [HttpGet]
        public IActionResult GetVendorMaster_Attachments(int? Partner_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //--send this value hard coded from here for getting attachment for customer master
            string Type = "BasicData_Partners";

            try
            {
                _oBasic.attachmentList = _oBasic.GetVendorMaster_Attachments_DAL(Partner_ID, Type).ToList();

                return PartialView("_VendorMasterAttachments", _oBasic);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }
        }

        //---insert vendor attachment list
        [HttpPost]
        public IActionResult InsertAttachments_VendorMaster(int? Partner_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (Partner_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
            {

                return Json(new { message = "Please fill all required fields!" });
            }

            var newFileName = "";
            var filepath = "";
            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            #region saving file
            if (Attachment != null)
            {
                if (Attachment.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (Attachment.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(Attachment.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            newFileName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filepath))
                            {
                                Attachment.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        return Json(new { message = ErrorMessage });
                    }

                }

            }


            #endregion

            //---saving attachment in database
            try
            {

                message = _oBasic.InsertAttachments_VendorMaster_DAL(Partner_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oBasic.attachmentList = _oBasic.GetVendorMaster_Attachments_DAL(Partner_ID, Type).ToList();

                    return PartialView("_VendorMasterAttachments", _oBasic);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }

        //---delete attachment of vendor 
        [HttpGet]
        public IActionResult DeleteAttachmentVendorMaster(int? Partner_ID, int? Attachment_ID, string Type, string File_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";

            if (Partner_ID == null)
            {
                //TempData["Error"] = "Please fill all required fields!";
                return Json(new { message = "Partner_ID id is null" });
            }

            if (Attachment_ID == null)
            {

                return Json(new { message = "Attachment_ID is null" });
            }
            if (String.IsNullOrEmpty(Type))
            {

                return Json(new { message = "Document type is null" });
            }


            #region Deleting actual file from folder
            try
            {
                string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                string filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{File_Name}";

                if ((System.IO.File.Exists(filepath)))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });

            }
            #endregion



            try
            {

                message = _oSale.Delete_Attachment(Attachment_ID);
                if (message == "Deleted Successfully!")
                {

                    _oBasic.attachmentList = _oBasic.GetVendorMaster_Attachments_DAL(Partner_ID, Type).ToList();

                    return PartialView("_VendorMasterAttachments", _oBasic);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }


        //---Get Showroom attachment list 
        [HttpGet]
        public IActionResult GetShowroom_Attachments(int? ShowRoom_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //--send this value hard coded from here for getting attachment for showroom 
            string Type = "BasicData_ShowRoom";

            try
            {
                _oBasic.attachmentList = _oBasic.GetShowroom_Attachments_DAL(ShowRoom_ID, Type).ToList();

                return PartialView("_ShowroomAttachments", _oBasic);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }
        }

        //---insert Showroom attachment list
        [HttpPost]
        public IActionResult InsertAttachments_Showroom(int? ShowRoom_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (ShowRoom_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
            {

                return Json(new { message = "Please fill all required fields!" });
            }

            var newFileName = "";
            var filepath = "";
            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            #region saving file
            if (Attachment != null)
            {
                if (Attachment.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (Attachment.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(Attachment.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            newFileName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filepath))
                            {
                                Attachment.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        return Json(new { message = ErrorMessage });
                    }

                }

            }


            #endregion

            //---saving attachment in database
            try
            {

                message = _oBasic.InsertAttachments_Showroom_DAL(ShowRoom_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oBasic.attachmentList = _oBasic.GetShowroom_Attachments_DAL(ShowRoom_ID, Type).ToList();

                    return PartialView("_ShowroomAttachments", _oBasic);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }

        //---delete Showroom of vendor 
        [HttpGet]
        public IActionResult DeleteAttachmentShowroom(int? ShowRoom_ID, int? Attachment_ID, string Type, string File_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";

            if (ShowRoom_ID == null)
            {
                //TempData["Error"] = "Please fill all required fields!";
                return Json(new { message = "ShowRoom_ID id is null" });
            }

            if (Attachment_ID == null)
            {

                return Json(new { message = "Attachment_ID is null" });
            }
            if (String.IsNullOrEmpty(Type))
            {

                return Json(new { message = "Document type is null" });
            }


            #region Deleting actual file from folder
            try
            {
                string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                string filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{File_Name}";

                if ((System.IO.File.Exists(filepath)))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });

            }
            #endregion



            try
            {

                message = _oSale.Delete_Attachment(Attachment_ID);
                if (message == "Deleted Successfully!")
                {

                    _oBasic.attachmentList = _oBasic.GetShowroom_Attachments_DAL(ShowRoom_ID, Type).ToList();

                    return PartialView("_ShowroomAttachments", _oBasic);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }


        public IActionResult PartnersShipping()
        {


            #region ViewBag Area
            ViewBag.PageTitle = "Partner Shipping";
            //---Ven Category list for drop down in forms
            ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();
            #endregion

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            //---Partners list for page
            _oBasic.PartnersShippingList = _oBasic.Get_ShippingCompaniesList_DAL(c_ID);


            return View(_oBasic);
        }

        [HttpPost]
        public IActionResult AddPartnersShipping(string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
 int? VendorCat_ID, string Remarks, string SellerType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(PartnerName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PartnersShipping", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            string PartnerType = "SHIP";



            //---saving Partners in database
            try
            {
                message = _oBasic.Insert_Partners_DAL(PartnerName, ContactNumber, MobileNo, ContactName, ContactAddress, Email, Fax, EmiratesID, TradeLicenseNo, TRN, BalanceType, OpeningBalanceDate, OpeningBalance,
 VendorCat_ID, PartnerType, Remarks, Created_by, c_ID, SellerType);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("PartnersShipping", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("PartnersShipping", "BasicData");
            }

            //return View();
        }
        //---method for deleting Partners
        [HttpPost]
        public IActionResult DeletePartnersShipping(int? Partner_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Partner_ID == null || Partner_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("PartnersShipping", "BasicData");
            }
            string message = "";



            try
            {
                message = _oBasic.Delete_Partners_DAL(Partner_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("PartnersShipping", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("PartnersShipping", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("PartnersShipping", "BasicData");

            }


        }


        //method for update Partners
        [HttpPost]
        public IActionResult UpdatePartnersShipping(int? Partner_ID, string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
int? VendorCat_ID, string Remarks,string SellerType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(PartnerName) || Partner_ID == null || Partner_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PartnersShipping", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);



            //---saving Partners in database
            try
            {
                message = _oBasic.Update_Partners_DAL(Partner_ID, PartnerName, ContactNumber, MobileNo, ContactName, ContactAddress, Email, Fax, EmiratesID, TradeLicenseNo, TRN, BalanceType, OpeningBalanceDate, OpeningBalance,
VendorCat_ID, Remarks, Modified_by, SellerType);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("PartnersShipping", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("PartnersShipping", "BasicData");
            }

            //return View();
        }

        public IActionResult PartnersExport()
        {


            #region ViewBag Area
            ViewBag.PageTitle = "Partners Export";
            //---Ven Category list for drop down in forms
            ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();
            #endregion

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            //---Partners list for page
            _oBasic.PartnersExportList = _oBasic.Get_ExportCompaniesList_DAL(c_ID);


            return View(_oBasic);
        }

        [HttpPost]
        public IActionResult AddPartnersExport(string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
int? VendorCat_ID, string Remarks, string SellerType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(PartnerName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PartnersExport", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            string PartnerType = "EXPT";



            //---saving Partners in database
            try
            {
                message = _oBasic.Insert_Partners_DAL(PartnerName, ContactNumber, MobileNo, ContactName, ContactAddress, Email, Fax, EmiratesID, TradeLicenseNo, TRN, BalanceType, OpeningBalanceDate, OpeningBalance,
 VendorCat_ID, PartnerType, Remarks, Created_by, c_ID, SellerType);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("PartnersExport", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("PartnersExport", "BasicData");
            }

            //return View();
        }
        //---method for deleting Partners
        [HttpPost]
        public IActionResult DeletePartnersExport(int? Partner_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Partner_ID == null || Partner_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("PartnersExport", "BasicData");
            }
            string message = "";



            try
            {
                message = _oBasic.Delete_Partners_DAL(Partner_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("PartnersExport", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("PartnersExport", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("PartnersExport", "BasicData");

            }


        }


        //method for update Partners
        [HttpPost]
        public IActionResult UpdatePartnersExport(int? Partner_ID, string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
int? VendorCat_ID, string Remarks, string SellerType )
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(PartnerName) || Partner_ID == null || Partner_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PartnersExport", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);



            //---saving Partners in database
            try
            {
                message = _oBasic.Update_Partners_DAL(Partner_ID, PartnerName, ContactNumber, MobileNo, ContactName, ContactAddress, Email, Fax, EmiratesID, TradeLicenseNo, TRN, BalanceType, OpeningBalanceDate, OpeningBalance,
VendorCat_ID, Remarks, Modified_by, SellerType);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("PartnersExport", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("PartnersExport", "BasicData");
            }

            //return View();
        }

        [HttpGet]
        public IActionResult BasicBank()
        {




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.PageTitle = "Basic Bank";
            //---BasicBank list for page
            _oBasic.BasicBankList = _oBasic.Get_BasicBanksList_DAL(c_ID);


            return View(_oBasic);
        }

        [HttpPost]
        public IActionResult AddBasicBank(string BankName, string AccountName, string AccountNumber, string IBAN, string branch, string SwiftCode, string Address, string ContactNumber)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            if (String.IsNullOrEmpty(BankName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("BasicBank", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            // Saving complete path(url) of page from where this ActionResult method called.This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();



            //---saving BasicBank in database
            try
            {
                message = _oBasic.Insert_BasicBank_DAL(BankName, AccountName, AccountNumber, IBAN, branch, SwiftCode, Address, ContactNumber, Created_by, c_ID);
                if (message == "Saved Successfully!")
                {
                    TempData["Success"] = "Saved Successfully";
                    return Redirect(url);
                }
                else
                {

                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }

        //---method for deleting BasicBank
        [HttpPost]
        public IActionResult DeleteBasicBank(int? Bank_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Bank_ID == null || Bank_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("BasicBank", "BasicData");
            }
            string message = "";



            try
            {
                message = _oBasic.Delete_BasicBank_DAL(Bank_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("BasicBank", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("BasicBank", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("BasicBank", "BasicData");

            }


        }


        //method for update BasicBank
        [HttpPost]
        public IActionResult UpdateBasicBank(int? Bank_ID, string BankName, string AccountName, string AccountNumber, string IBAN, string branch, string SwiftCode, string Address, string ContactNumber)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(BankName) || Bank_ID == null || Bank_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("BasicBank", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);



            //---saving BasicBank in database
            try
            {
                message = _oBasic.Update_BasicBank_DAL(Bank_ID, BankName, AccountName, AccountNumber, IBAN, branch, SwiftCode, Address, ContactNumber, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("BasicBank", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("BasicBank", "BasicData");
            }

            //return View();
        }


        [HttpGet]
        public IActionResult CurrencyMaster()
        {




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.PageTitle = "Manage Currency";

            _oBasic.currencyMasteList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();


            return View(_oBasic);
        }



        [HttpPost]
        public IActionResult InsertCurrencyMaster(string Currency_Name, string Currency_ShortName, double? Curr_Rate, string Minor_ShortName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            // Saving complete path(url) of page from where this ActionResult method called.This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();




            if (String.IsNullOrEmpty(Currency_Name) || String.IsNullOrEmpty(Currency_ShortName) || Curr_Rate == null || String.IsNullOrEmpty(Minor_ShortName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);



            //---saving BasicBank in database
            try
            {
                message = _oBasic.InsertCurrencyMaster_DAL(Currency_Name, Currency_ShortName, Curr_Rate, Minor_ShortName, Created_by, c_ID);
                if (message == "Saved Successfully!")
                {
                    TempData["Success"] = "Saved Successfully";
                    return Redirect(url);
                }
                else
                {

                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }



        [HttpPost]
        public IActionResult UpdateCurrencyMaster(int? Currency_ID, string Currency_Name, string Currency_ShortName, double? Curr_Rate, string Minor_ShortName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            // Saving complete path(url) of page from where this ActionResult method called.This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (Currency_ID == null)
            {
                TempData["Error"] = "Currency_ID is null!";
                return Redirect(url);
            }


            if (String.IsNullOrEmpty(Currency_Name) || String.IsNullOrEmpty(Currency_ShortName) || Curr_Rate == null || String.IsNullOrEmpty(Minor_ShortName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);




            try
            {
                message = _oBasic.UpdateCurrencyMaster_DAL(Currency_ID, Currency_Name, Currency_ShortName, Curr_Rate, Minor_ShortName, Modified_by);
                if (message == "Updated Successfully!")
                {
                    TempData["Success"] = "Updated Successfully!";
                    return Redirect(url);
                }
                else
                {

                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }
        [HttpPost]
        public IActionResult DeleteCurrencyMaster(int? Currency_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            // Saving complete path(url) of page from where this ActionResult method called.This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (Currency_ID == null)
            {
                TempData["Error"] = "Currency_ID is null!";
                return Redirect(url);
            }


            string message = "";


            try
            {
                message = _oBasic.DeleteCurrencyMaster_DAL(Currency_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully!";
                    return Redirect(url);
                }
                else
                {

                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }

        [HttpGet]
        public ActionResult Inventory()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            #region viewbag area
            ViewBag.UOMList = _oBasic.Get_UOM_DAL(c_ID).ToList();
            ViewBag.ItemGroupList = _oBasic.Get_ItemGroup_DAL(c_ID).ToList();
            ViewBag.ItemCategoryList = _oBasic.Get_ItemCategory_DAL(c_ID).ToList();
            #endregion

            //---Make list for page
            _oBasic.InventryMasterList = _oBasic.Get_InventryMasterList_DAL();

            return View(_oBasic);
        }

        public IActionResult AddInventry(string ItemCode, string ItemName, string ItemDescription, string IsSerializable, string BarCode, string UnitPrice, string SalePrice, string CostMethod, string Comment, string UOM, int Group_ID, int Category_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (String.IsNullOrEmpty(ItemCode))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Inventory", "BasicData");
            }

            if (String.IsNullOrEmpty(ItemName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Inventory", "BasicData");
            }



            //if (String.IsNullOrEmpty(IsSerializable))
            //{
            //    TempData["Error"] = "Please fill all required fields!";
            //    return RedirectToAction("Inventry", "BasicData");
            //}

            string message = "";
            // int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving Inventry in database
            try
            {
                message = _oBasic.Insert_InventryMaster_DAL(ItemCode, ItemName, ItemDescription, IsSerializable, BarCode, UnitPrice, SalePrice, CostMethod, Comment, UOM, Group_ID, Category_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Inventory", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("Inventory", "BasicData");
            }

            //return View();
        }


        public IActionResult UpdateInventry(int? Item_ID, string ItemCode, string ItemName, string ItemDescription,
            string IsSerializable, string BarCode, string UnitPrice, string SalePrice, string CostMethod, string Comment,
            string UOM, int Group_ID, int Category_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (String.IsNullOrEmpty(ItemCode))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Inventory", "BasicData");
            }

            if (String.IsNullOrEmpty(ItemName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Inventory", "BasicData");
            }



            //if (String.IsNullOrEmpty(IsSerializable))
            //{
            //    TempData["Error"] = "Please fill all required fields!";
            //    return RedirectToAction("Inventry", "BasicData");
            //}

            string message = "";
            // int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving Color in database
            try
            {
                message = _oBasic.Update_InventryMaster_DAL(Item_ID, ItemCode, ItemName, ItemDescription, IsSerializable, BarCode, UnitPrice, SalePrice, CostMethod, Comment, UOM, Group_ID, Category_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Inventory", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("Inventory", "BasicData");
            }

            //return View();
        }

        [HttpPost]
        public IActionResult DeleteInventry(int Item_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Item_ID == 0 || Item_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Inventory", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_Inventory_DAL(Item_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("Inventory", "BasicData");
                }
                else
                {
                    TempData["Error"] = "Some thing went wrong. Please try again!";
                    return RedirectToAction("Inventory", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Inventory", "BasicData");

            }


        }

        //---Get vendor attachment list 


        [HttpGet]
        public IActionResult GetShippingMaster_Attachments(int? Partner_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //--send this value hard coded from here for getting attachment for customer master
            string Type = "BasicData_Shipping";

            try
            {
                _oBasic.attachmentList = _oBasic.GetVendorMaster_Attachments_DAL(Partner_ID, Type).ToList();

                return PartialView("_ShippingMasterAttachments", _oBasic);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }
        }


        public IActionResult GetAgent_Attachments(int? Partner_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //--send this value hard coded from here for getting attachment for customer master
            string Type = "BasicData_Agent";

            try
            {
                _oBasic.attachmentList = _oBasic.GetVendorMaster_Attachments_DAL(Partner_ID, Type).ToList();

                return PartialView("_PartnerAgentAttachments", _oBasic);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }
        }



        //---insert vendor attachment list
        [HttpPost]
        public IActionResult InsertAttachments_ShippingMaster(int? Partner_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (Partner_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
            {

                return Json(new { message = "Please fill all required fields!" });
            }

            var newFileName = "";
            var filepath = "";
            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            #region saving file
            if (Attachment != null)
            {
                if (Attachment.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (Attachment.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(Attachment.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            newFileName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filepath))
                            {
                                Attachment.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        return Json(new { message = ErrorMessage });
                    }

                }

            }


            #endregion

            //---saving attachment in database
            try
            {

                message = _oBasic.InsertAttachments_VendorMaster_DAL(Partner_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oBasic.attachmentList = _oBasic.GetVendorMaster_Attachments_DAL(Partner_ID, Type).ToList();

                    return PartialView("_ShippingMasterAttachments", _oBasic);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }

        [HttpPost]
        public IActionResult InsertAttachments_Agents(int? Partner_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (Partner_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
            {

                return Json(new { message = "Please fill all required fields!" });
            }

            var newFileName = "";
            var filepath = "";
            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            #region saving file
            if (Attachment != null)
            {
                if (Attachment.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (Attachment.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(Attachment.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            newFileName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filepath))
                            {
                                Attachment.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        return Json(new { message = ErrorMessage });
                    }

                }

            }


            #endregion

            //---saving attachment in database
            try
            {

                message = _oBasic.InsertAttachments_VendorMaster_DAL(Partner_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oBasic.attachmentList = _oBasic.GetVendorMaster_Attachments_DAL(Partner_ID, Type).ToList();

                    return PartialView("_PartnerAgentAttachments", _oBasic);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }


        [HttpGet]
        public IActionResult DeleteAttachmentShippingMaster(int? Partner_ID, int? Attachment_ID, string Type, string File_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";

            if (Partner_ID == null)
            {
                //TempData["Error"] = "Please fill all required fields!";
                return Json(new { message = "Partner_ID id is null" });
            }

            if (Attachment_ID == null)
            {

                return Json(new { message = "Attachment_ID is null" });
            }
            if (String.IsNullOrEmpty(Type))
            {

                return Json(new { message = "Document type is null" });
            }


            #region Deleting actual file from folder
            try
            {
                string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                string filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{File_Name}";

                if ((System.IO.File.Exists(filepath)))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });

            }
            #endregion



            try
            {

                message = _oSale.Delete_Attachment(Attachment_ID);
                if (message == "Deleted Successfully!")
                {

                    _oBasic.attachmentList = _oBasic.GetVendorMaster_Attachments_DAL(Partner_ID, Type).ToList();

                    return PartialView("_ShippingMasterAttachments", _oBasic);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }
        [HttpGet]
        public ActionResult Unit_OF_Measurement()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Unit OF Measurement List";
            ViewBag.PageTitle = "Unit OF Measurement";
            //---Make list for page
            _oBasic.UOMList = _oBasic.Get_UOMList_DAL(c_ID);

            return View(_oBasic);
        }

        //end here
        //AddUOM start here
        public IActionResult AddUOM(string UOM)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(UOM))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Unit_OF_Measurement", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving UOM in database
            try
            {
                message = _oBasic.Insert_UOM_DAL(UOM, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Unit_OF_Measurement", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("Unit_OF_Measurement", "BasicData");
            }

            //return View();
        }

        //UpdateUOM by noor
        [HttpPost]
        public IActionResult UpdateUOM(int? UOM_ID, string UOM)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(UOM) || UOM_ID == null || UOM_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Make", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);


            //---saving Make in database
            try
            {
                message = _oBasic.Update_UOM_DAL(UOM_ID, UOM, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("Unit_OF_Measurement", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("Unit_OF_Measurement", "BasicData");
            }

            //return View();
        }


        //updateUOM end
        //delete work start by noor
        [HttpPost]
        public IActionResult DeleteUOM(int? UOM_ID, string UOM)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (UOM_ID == null || UOM_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Unit_OF_Measurement", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_UOM_DAL(UOM_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("Unit_OF_Measurement", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("Unit_OF_Measurement", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("Unit_OF_Measurement", "BasicData");

            }


        }
        [HttpGet]
        public ActionResult ItemGroup()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Make Description List";
            ViewBag.PageTitle = "ItemGroup";
            //---Make list for page
            _oBasic.ItemGroupList = _oBasic.Get_ItemGroupList_DAL(c_ID);

            return View(_oBasic);
        }

        //end here
        //AddItemGroup start here
        public IActionResult AddItemGroup(string Group_Code, string Group_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(Group_Name))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("ItemGroup", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving ItemGroup in database
            try
            {
                message = _oBasic.Insert_ItemGroup_DAL(Group_Code, Group_Name, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("ItemGroup", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("ItemGroup", "BasicData");
            }

            //return View();
        }

        //UpdateItemGroup by muneeb
        [HttpPost]
        public IActionResult UpdateItemGroup(int? Group_ID, string Group_Code, string Group_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(Group_Name) || Group_ID == null || Group_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Make", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);


            //---saving Make in database
            try
            {
                message = _oBasic.Update_ItemGroup_DAL(Group_ID, Group_Code, Group_Name, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("ItemGroup", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("ItemGroup", "BasicData");
            }

            //return View();
        }


        //updateItemGroup end
        //delete work start by muneeb
        [HttpPost]
        public IActionResult DeleteItemGroup(int? Group_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Group_ID == null || Group_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("ItemGroup", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_ItemGroup_DAL(Group_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("ItemGroup", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("ItemGroup", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("ItemGroup", "BasicData");

            }


        }

        [HttpGet]
        public ActionResult ItemCategory()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Make Description List";
            ViewBag.PageTitle = "ItemCategory";
            //---Make list for page
            _oBasic.ItemCategoryList = _oBasic.Get_ItemCategoryList_DAL(c_ID);

            return View(_oBasic);
        }

        //end here
        //AddItemCategory start here
        [HttpPost]
        public IActionResult AddItemCategory(string Category_Code, string Category_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(Category_Name))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("ItemCategory", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving ItemCategory in database
            try
            {
                message = _oBasic.Insert_ItemCategory_DAL(Category_Code, Category_Name, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("ItemCategory", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("ItemCategory", "BasicData");
            }

            //return View();
        }

        //UpdateItemCategory by muneeb
        [HttpPost]
        public IActionResult UpdateItemCategory(int? Category_ID, string Category_Code, string Category_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(Category_Name) || Category_ID == null || Category_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Make", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);


            //---saving Make in database
            try
            {
                message = _oBasic.Update_ItemCategory_DAL(Category_ID, Category_Code, Category_Name, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("ItemCategory", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("ItemCategory", "BasicData");
            }

            //return View();
        }


        //updateItemCategory end
        //delete work start by muneeb
        [HttpPost]
        public IActionResult DeleteItemCategory(int? Category_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Category_ID == null || Category_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("ItemCategory", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_ItemCategory_DAL(Category_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("ItemCategory", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("ItemCategory", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("ItemCategory", "BasicData");

            }


        }



        public JsonResult Get_MultipleMake(int Item_ID)
        {
            List<int> make_ID = new List<int>();

            _oBasic.MultipleMakeList = _oBasic.Get_MultipleMakeList_DAL(Item_ID).ToList();
            foreach (var item in _oBasic.MultipleMakeList)
            {
                make_ID.Add(item.make_ID);
            }
            return Json(new { make_ID });

        }
        public JsonResult Get_MultipleYear(int Item_ID)
        {
            List<string> Year = new List<string>();

            _oBasic.MultipleYearList = _oBasic.Get_MultipleYearList_DAL(Item_ID).ToList();
            foreach (var item in _oBasic.MultipleYearList)
            {
                Year.Add(item.Year);
            }
            return Json(new { Year });

        }

        public JsonResult Get_MultipleEngineSpecsCode(int Item_ID)
        {
            List<int> enginespecscode = new List<int>();

            _oBasic.MultipleEngineSpecsCodeList = _oBasic.Get_MultipleEngineSpecsCodeList_DAL(Item_ID).ToList();
            foreach (var item in _oBasic.MultipleEngineSpecsCodeList)
            {
                enginespecscode.Add(item.enginespecscode_ID);
            }
            return Json(new { enginespecscode });

        }

        [HttpGet]
        public ActionResult InventoryParts()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            #region viewbag area
            ViewBag.UOMList = _oBasic.Get_UOM_DAL(c_ID).ToList();
            ViewBag.ItemGroupList = _oBasic.Get_ItemGroup_DAL(c_ID).ToList();
            ViewBag.ItemCategoryList = _oBasic.Get_ItemCategory_DAL(c_ID).ToList();

            ViewBag.MakeList = _oBasic.Get_MakeModelList_DAL(c_ID).ToList();

            ViewBag.EngineSpecsCodeList = _oBasic.Get_EngineSpecsCodeList_DAL(c_ID).ToList();
            ViewBag.PageTitle = "Inventory Parts";
            

            #endregion

            //---Make list for page
            _oBasic.InventryMasterList = _oBasic.Get_InventryMasterList_DAL();

            return View(_oBasic);
        }

        public IActionResult AddInventryParts(string ItemCode, string ItemName, string ItemDescription, string IsSerializable, string BarCode, string UnitPrice, string OpeningBal
            , string SalePrice, string CostMethod, string Comment, string UOM, int Group_ID
            , int Category_ID, string Traditional, string FUEL_TYPE, string Transmission, string Drive,
            int[] make_ID, string StartYear, string EndYear, int[] EngineSpecsCode_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(ItemCode))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("InventoryParts", "BasicData");
            }


            string ItemID = "";
            string message = "";


            //---saving Inventry in database
            try
            {
                ItemID = _oBasic.Insert_InventryPartsMaster_DAL(ItemCode, ItemName, ItemDescription, IsSerializable, BarCode,
                    UnitPrice, SalePrice, CostMethod, Comment, UOM, Group_ID, Category_ID, Traditional, FUEL_TYPE, Transmission, Drive, StartYear, EndYear, OpeningBal);
                if (!string.IsNullOrEmpty(ItemID))
                {
                    if (make_ID.Count() > 0)
                    {
                        foreach (var item in make_ID)
                        {
                            message = _oBasic.Insert_InventryPartsMultipleMake_DAL(Convert.ToInt32(ItemID), item);
                        }
                    }


                    if (EngineSpecsCode_ID.Count() > 0)
                    {
                        foreach (var item in EngineSpecsCode_ID)
                        {
                            message = _oBasic.Insert_MultipleEngineSpecsCode_DAL(Convert.ToInt32(ItemID), item);
                        }
                    }

                }
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("InventoryParts", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("InventoryParts", "BasicData");
            }

            //return View();
        }

        public IActionResult UpdateInventryParts(int? Item_ID, string ItemCode, string ItemName, string ItemDescription,
            string IsSerializable, string BarCode, string UnitPrice, string OpeningBal, string SalePrice, string CostMethod, string Comment,
            string UOM, int Group_ID, int Category_ID, string Traditional, string FUEL_TYPE,
            string Transmission, string Drive, int[] make_ID, string StartYear, string EndYear, int[] EngineSpecsCode_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (String.IsNullOrEmpty(ItemCode))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("InventoryParts", "BasicData");
            }

            if (String.IsNullOrEmpty(ItemName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("InventoryParts", "BasicData");
            }



            //if (String.IsNullOrEmpty(IsSerializable))
            //{
            //    TempData["Error"] = "Please fill all required fields!";
            //    return RedirectToAction("Inventry", "BasicData");
            //}

            string message = "";
            // int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving Color in database
            try
            {
                message = _oBasic.Update_InventryPartsMaster_DAL(Item_ID, ItemCode, ItemName, ItemDescription, IsSerializable, BarCode, UnitPrice, SalePrice, CostMethod, Comment, UOM, Group_ID, Category_ID, Traditional, FUEL_TYPE, Transmission, Drive, StartYear, EndYear, OpeningBal);
                if (Item_ID > 0 && Item_ID != null)
                {
                    if (make_ID.Count() > 0)
                    {
                        message = _oBasic.Delete_MultipleMake_DAL(Item_ID);
                        foreach (var item in make_ID)
                        {
                            message = _oBasic.Insert_InventryPartsMultipleMake_DAL(Convert.ToInt32(Item_ID), item);
                        }
                    }


                    if (EngineSpecsCode_ID.Count() > 0)
                    {
                        message = _oBasic.Delete_MultipleEngineSpecsCode_DAL(Convert.ToInt32(Item_ID));
                        foreach (var item in EngineSpecsCode_ID)
                        {
                            message = _oBasic.Insert_MultipleEngineSpecsCode_DAL(Convert.ToInt32(Item_ID), item);
                        }
                    }


                }


                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("InventoryParts", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("InventoryParts", "BasicData");
            }

            //return View();
        }

        [HttpPost]
        public IActionResult DeleteInventryParts(int Item_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Item_ID == 0 || Item_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("InventoryParts", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_Inventory_DAL(Item_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("InventoryParts", "BasicData");
                }
                else
                {
                    TempData["Error"] = "Some thing went wrong. Please try again!";
                    return RedirectToAction("InventoryParts", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("InventoryParts", "BasicData");

            }


        }


        [HttpGet]
        public ActionResult ItemLocationMaster()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "ItemLocationMaster List";
            ViewBag.PageTitle = "ItemLocationMaster";
            //---Make list for page
            _oBasic.ItemLocationList = _oBasic.Get_ItemLocationList_DAL(c_ID);

            return View(_oBasic);
        }

        //end here
        //AddItemLocation start here
        public IActionResult AddItemLocation(string ItemLocationName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(ItemLocationName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("ItemLocationMaster", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving ItemLocation in database
            try
            {
                message = _oBasic.Insert_ItemLocation_DAL(ItemLocationName, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("ItemLocationMaster", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("ItemLocationMaster", "BasicData");
            }

            //return View();
        }

        //UpdateItemLocation by noor
        [HttpPost]
        public IActionResult UpdateItemLocation(int? ItemLocation_ID, string ItemLocationName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(ItemLocationName) || ItemLocation_ID == null || ItemLocation_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("Make", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);


            //---saving Make in database
            try
            {
                message = _oBasic.Update_ItemLocation_DAL(ItemLocation_ID, ItemLocationName, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("ItemLocationMaster", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("ItemLocationMaster", "BasicData");
            }

            //return View();
        }


        //updateItemLocation end
        //delete work start by noor
        [HttpPost]
        public IActionResult DeleteItemLocation(int? ItemLocation_ID, string ItemLocationName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (ItemLocation_ID == null || ItemLocation_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("ItemLocationMaster", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_ItemLocation_DAL(ItemLocation_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("ItemLocationMaster", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("ItemLocationMaster", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("ItemLocationMaster", "BasicData");

            }


        }
        [HttpGet]
        public ActionResult CustomerCategory()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "CustomerCategory List";
            ViewBag.PageTitle = "CustomerCategory";
            //---Make list for page
            _oBasic.CustomerCatList = _oBasic.Get_CustomerCatList_DAL(c_ID);

            return View(_oBasic);
        }

        //end here
        //AddCustomerCat start here
        public IActionResult AddCustomerCat(string CustomerCatName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(CustomerCatName))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("CustomerCategory", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving CustomerCat in database
            try
            {
                message = _oBasic.Insert_CustomerCat_DAL(CustomerCatName, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("CustomerCategory", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("CustomerCategory", "BasicData");
            }

            //return View();
        }

        //UpdateCustomerCat by noor
        [HttpPost]
        public IActionResult UpdateCustomerCat(int? CustomerCat_ID, string CustomerCatName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(CustomerCatName) || CustomerCat_ID == null || CustomerCat_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("CustomerCategory", "BasicData");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);


            //---saving Make in database
            try
            {
                message = _oBasic.Update_CustomerCat_DAL(CustomerCat_ID, CustomerCatName, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("CustomerCategory", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("CustomerCategory", "BasicData");
            }

            //return View();
        }


        //updateCustomerCat end
        //delete work start by noor
        [HttpPost]
        public IActionResult DeleteCustomerCat(int? CustomerCat_ID, string CustomerCatName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (CustomerCat_ID == null || CustomerCat_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("CustomerCategory", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_CustomerCat_DAL(CustomerCat_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("CustomerCategory", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("CustomerCategory", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("CustomerCategory", "BasicData");

            }


        }



        #region EngineSpecsCode

        //get EngineSpecsCode
        [HttpGet]
        public IActionResult EngineSpecsCode()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Engine Spec Code List";
            ViewBag.PageTitle = "Engine Spec Code";

            //---Make list for page
            _oBasic.EngineSpecsCodeList = _oBasic.Get_EngineSpecsCodeList_DAL(c_ID);


            return View(_oBasic);
        }

        //---Insert EngineSpecsCode
        [HttpPost]
        public IActionResult Insert_EngineSpecsCode(string EngineSpecsCode, string SpecsDescription)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            if (String.IsNullOrEmpty(EngineSpecsCode))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("EngineSpecsCode", "BasicData");
            }




            string message = "";
            int Created_by = Convert.ToInt32(UserID_Admin);


            //---saving engine Type in database
            try
            {
                message = _oBasic.Insert_EngineSpecsCode_DAL(EngineSpecsCode, SpecsDescription, Created_by, c_ID);
                if (message == "Saved Successfully!")
                {
                    TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("EngineSpecsCode", "BasicData");
                }
                else
                {
                    TempData["Error"] = "Error occured in saving the record!";
                    return RedirectToAction("EngineSpecsCode", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("EngineSpecsCode", "BasicData");
            }

            //return View();
        }

        //---method for deleting EngineSpecsCode
        [HttpPost]
        public IActionResult Delete_EngineSpecsCode(int? EngineSpecsCode_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (EngineSpecsCode_ID == null || EngineSpecsCode_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("EngineSpecsCode", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_EngineSpecs_DAL(EngineSpecsCode_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("EngineSpecsCode", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("EngineSpecsCode", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("EngineType", "BasicData");

            }


        }

        //---Method for Updating EngineSpecsCode
        [HttpPost]
        public IActionResult Update_EngineSpecsCode(int EngineSpecsCode_ID, string EngineSpecsCode, string SpecsDescription)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (String.IsNullOrEmpty(EngineSpecsCode) || EngineSpecsCode_ID == 0)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("EngineSpecsCode", "BasicData");
            }




            string message = "";
            int Modified_by = Convert.ToInt32(UserID_Admin);


            //---saving engine Type in database
            try
            {
                message = _oBasic.Update_EngineSpecsCode_DAL(EngineSpecsCode_ID, EngineSpecsCode, SpecsDescription, Modified_by);
                if (message == "Updated Successfully!")
                {
                    TempData["Success"] = "Updated Successfully!";
                    return RedirectToAction("EngineSpecsCode", "BasicData");
                }
                else
                {
                    TempData["Error"] = "Error occured in saving the record!";
                    return RedirectToAction("EngineSpecsCode", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("EngineSpecsCode", "BasicData");
            }

            //return View();
        }






        #endregion




        #region CityRegistrations
        //---method for CityRegistrations
        [HttpGet]
        public IActionResult CityRegistrations()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "City Registrations";

            ViewBag.PageTitle = "City Of Exit";

            _oBasic.CustomerRegistrationList = _oBasic.Get_CityRegistration_DAL_List();

            return View(_oBasic);
        }

        //---method for Insert_CityRegistrations
        [HttpPost]
        public IActionResult Insert_CityReg_DAL(string CityName, string CityNameEnglish)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);

            try
            {
                message = _oBasic.Insert_CityReg_DAL(CityName, CityNameEnglish);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("CityRegistrations", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("CityRegistrations", "BasicData");
            }

            //return View();
        }

        //---method for DeleteCityRegExit
        [HttpPost]
        public IActionResult DeleteCityRegExit(int? ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (ID == null || ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("CityRegistrations", "BasicData");
            }
            string message = "";



            try
            {
                message = _oBasic.DeleteCityRegExit(ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("CityRegistrations", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("CityRegistrations", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("CityRegistrations", "BasicData");

            }
        }

        //method for Update_CityReg_DAL
        [HttpPost]
        public IActionResult Update_CityReg_DAL(int? ID, string CityName, string CityNameEnglish)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion



            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);

            try
            {

                message = _oBasic.Update_CityReg_DAL(ID, CityName, CityNameEnglish);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("CityRegistrations", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("CityRegistrations", "BasicData");
            }

            //return View();
        }

        #endregion

        #region WordsRegistrations

        //---method for WordsRegistrations
        [HttpGet]
        public IActionResult WordsRegistrations()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Words Registrations";

            ViewBag.PageTitle = "Words Registrations Of Exit";

            _oBasic.WordsRegistrationList = _oBasic.Get_WordsDAL_List();

            return View(_oBasic);
        }

        //---method for InsertWordsDAL
        [HttpPost]
        public IActionResult InsertWordsDAL(string Words)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);

            try
            {

                message = _oBasic.InsertWordsDAL(Words);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("WordsRegistrations", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("WordsRegistrations", "BasicData");
            }

            return View();
        }

        //---method for DeleteWordsExit
        [HttpPost]
        public IActionResult DeleteWordsExit(int? ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (ID == null || ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("WordsRegistrations", "BasicData");
            }
            string message = "";



            try
            {
                message = _oBasic.DeleteWordsExit(ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("WordsRegistrations", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("WordsRegistrations", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("WordsRegistrations", "BasicData");

            }
        }

        //method for UpdateWordsDAL
        [HttpPost]
        public IActionResult UpdateWordsDAL(int? ID, string Words)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);

            try
            {

                message = _oBasic.UpdateWordsDAL(ID, Words);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("WordsRegistrations", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("WordsRegistrations", "BasicData");
            }
        }

        #endregion

        #region wordupload
        [HttpGet]
        public IActionResult WordDocuments(int WordDocument_ID, int Vendor_ID, int Customer_ID, string WordTitle)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            WordTitle = (String.IsNullOrEmpty(WordTitle) || String.IsNullOrWhiteSpace(WordTitle) ? "" : WordTitle);
            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_ID).ToList();
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();
            ViewBag.Vendor_ID = Vendor_ID;
            ViewBag.Customer_ID = Customer_ID;
            ViewBag.WordTitle = WordTitle;
            ViewBag.PageTitle = "Ledger";
            Urls = null;

            ApprovalURL = HttpContext.Request.Headers["Referer"];
            Urls = ApprovalURL;

            _oBasic.WordDocumentList = _oBasic.Get_WordDocument_List_DAL(c_ID, WordDocument_ID, Vendor_ID, Customer_ID, WordTitle).ToList();
            _oBasic.WordRef = _oBasic.WordLoadRef();

            return View(_oBasic);


        }
        [HttpPost]
        public IActionResult Add_WordDocuments(string WordRef, IFormFile UserAttachment, int Vendor_ID, int Customer_ID, string WordTitle)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            int Create_by = Convert.ToInt32(UserID_Admin);
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            string message = "";
            var newFileName = "";
            var filepath = "";



            #region saving file
            if (UserAttachment != null)
            {
                if (UserAttachment.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (UserAttachment.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(UserAttachment.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            newFileName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filepath))
                            {

                                UserAttachment.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        TempData["Error"] = "Images not saved successfully!";
                    }

                }

            }


            #endregion

            message = _oBasic.Insert_WordDocument_DAL(WordRef, c_ID, newFileName, filepath, Vendor_ID, Customer_ID, WordTitle, Create_by);

            if (message == "Saved Successfully!")
            {
                TempData["Success"] = "Document added successfully";
                return RedirectToAction("WordDocuments", "BasicData");
            }
            else
            {
                TempData["Error"] = "Error in saving the record";
                return RedirectToAction("WordDocuments", "BasicData");
            }



        }
        [HttpPost]
        public IActionResult DeleteWordDocument(int? WordDocument_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";


            if (WordDocument_ID == null)
            {
                //return new HttpResponse("");
            }



            message = _oBasic.Delete_WordDocument_DAL(WordDocument_ID);
            if (message == "Deleted sucessfully")
            {
                TempData["Success"] = message;

                return RedirectToAction("WordDocuments", "BasicData");
            }
            else
            {
                TempData["Error"] = "Error in deleting the record";
                return RedirectToAction("WordDocuments", "BasicData");
            }




        }

        public static void ReplaceTextInWordDoc(string filepaths, string WordRef)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filepaths, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                Regex regexText = new Regex("SREF");
                docText = regexText.Replace(docText, WordRef);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
        }

        [HttpGet]
        public string PrintPDF(string FileName)
        {


            var newFileName = "";
            var filepath = "";



            #region saving file
            if (FileName != null)
            {

                try
                {
                    string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                    string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                    string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;
                    //Load Document
                    Document document = new Document();
                    document.LoadFromFile(FileName);

                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                    newFileName = String.Concat(myUniqueFileName, ".pdf");
                    filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

                    //Convert Word to PDF

                    document.SaveToFile(filepath, FileFormat.PDF);
                }
                catch (Exception ex)
                {
                    string ErrorMessage = ex.Message;
                    TempData["Error"] = "Images not saved successfully!";
                }
            }


            #endregion


            return filepath;

        }
        [HttpPost]
        public IActionResult ChangeWordMasterStatus(int WordDocument_ID, int? Status_ID, string Trans_Type, string FileName)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion

            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int c_ID = Convert.ToInt32(company_ID);

            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();


            if (WordDocument_ID == 0 || Status_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {


                string QRName = QRGenerator(WordDocument_ID, c_ID.ToString());

                message = _oBasic.ChangeMasterStatus_DAL(WordDocument_ID, Status_ID, c_ID, Trans_Type, Modified_by, "", "");
                _oBasic.WordListById = _oBasic.Get_WordList_I(c_ID, WordDocument_ID);
                if (_oBasic.WordListById.imgname == null || _oBasic.WordListById.imgname == "")
                {
                    ReplaceTextInWordDocIB("", FileName, "wwwroot/CommonImages/OtherImages/" + QRName, _oBasic.WordListById.WordRef);

                }
                else
                {
                    ReplaceTextInWordDocIB("wwwroot/CommonImages/OtherImages/" + _oBasic.WordListById.imgname, FileName, "wwwroot/CommonImages/OtherImages/" + QRName, _oBasic.WordListById.WordRef);

                }
                //ReplaceTextInWordDocI("wwwroot/CommonImages/OtherImages/" + QRName, FileName);
                string PDFName = PrintPDF(FileName);
                message = _oBasic.ChangeMasterStatus_DAL(WordDocument_ID, Status_ID, c_ID, Trans_Type, Modified_by, QRName, PDFName);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = message;
                    return Redirect(Urls);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }

        #endregion








        #region NEW
        //[HttpPost]
        //public FileResult Convert(HttpPostedFileBase postedFile)
        //{
        //    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(postedFile.FileName);
        //    string filePath = Server.MapPath("~/Files/") + Path.GetFileName(postedFile.FileName);
        //    postedFile.SaveAs(filePath);
        //    string input = filePath;
        //    string output = Server.MapPath("~/Files/") + fileNameWithoutExtension + ".pdf";
        //    ConvertWordToSpecifiedFormat(input, output, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
        //    return File(output, "application/pdf", fileNameWithoutExtension + ".pdf");
        //}

        //private static void ConvertWordToSpecifiedFormat(object input, object output, object format)
        //{
        //    Microsoft.Office.Interop.Word._Application application = new Microsoft.Office.Interop.Word.Application();
        //    application.Visible = false;
        //    object missing = Missing.Value;
        //    object isVisible = true;
        //    object readOnly = false;
        //    Microsoft.Office.Interop.Word._Document document = application.Documents.Open(ref input, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing,
        //                            ref missing, ref missing, ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

        //    document.Activate();
        //    document.SaveAs(ref output, ref format, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
        //                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
        //    application.Quit(ref missing, ref missing, ref missing);
        //}

        #endregion






        #region QRGENERATOR
        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
        public string QRGenerator(int? WordDocument_ID, string c_ID)
        {
            string url = configuration.GetSection("AppSettings").GetSection("WordURL").Value;
            WordDocument_ID = (WordDocument_ID == null ? 0 : WordDocument_ID);
            var writer = new QRCodeWriter();
            //generate QR Code
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            var encryptedString = EncryptString(key, WordDocument_ID.ToString());
            var resultBit = writer.encode(url + encryptedString + "&c_id=" + c_ID, BarcodeFormat.QR_CODE, 75, 75);
            //get Bitmatrix result
            var matrix = resultBit;

            //convert bitmatrix into image 
            int scale = 2;

            Bitmap result = new Bitmap(matrix.Width * scale, matrix.Height * scale);
            for (int x = 0; x < matrix.Height; x++)
                for (int y = 0; y < matrix.Width; y++)
                {
                    Color pixel = matrix[x, y] ? System.Drawing.Color.Black : System.Drawing.Color.White;
                    for (int i = 0; i < scale; i++)
                        for (int j = 0; j < scale; j++)
                            result.SetPixel(x * scale + i, y * scale + j, pixel);
                }
            string filepath = "";
            string imgname = "";
            #region
            int length = 4;
            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }

            //appending str_build text to image name
            imgname = str_build.ToString() + "_" + WordDocument_ID + "QR.png";
            #endregion

            //get wwwroot folder location
            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;


            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{imgname}";

            //save result as png inside 'Images' folder
            result.Save(filepath);
            return imgname;
        }
        public static void ReplaceTextInWordDocI(string filepaths, string DOCFILE)
        {
            //Open a blank word document as template

            Document document = new Document(DOCFILE);

            Section section = document.Sections[0];
            Paragraph paragraph2 = section.AddParagraph();

            //Append Picture and Text for Footer Paragraph
            DocPicture footerimage = paragraph2.AppendPicture(Image.FromFile(filepaths));
            document.MailMerge.HideEmptyParagraphs = true;
            footerimage.Width = 50;
            footerimage.Height = 50;
            paragraph2.Format.BeforeSpacing = section.PageSetup.PageSize.Height / 2 - 1 * section.PageSetup.Margins.Bottom;
            paragraph2.Format.HorizontalAlignment = HorizontalAlignment.Left;
            document.SaveToFile(DOCFILE, FileFormat.Docx);

        }
        public static void ReplaceTextInWordDocIB(string filepaths, string DOCFILE, string QRname, string WordREF)
        {
            //Open a blank word document as template
            Document document = new Document();
            document.LoadFromFile(DOCFILE);

            HeaderFooter section1 = document.Sections[0].HeadersFooters.Header;
            Paragraph paragraph3 = section1.AddParagraph();
            TextRange HText = paragraph3.AppendText(WordREF);
            HText.CharacterFormat.FontSize = 15;
            HText.CharacterFormat.TextColor = System.Drawing.Color.Black;


            Section section = document.Sections[0];
            Paragraph paragraph2 = section.AddParagraph();
            //Append Picture and Text for Footer Paragraph
            DocPicture footerimage2 = paragraph2.AppendPicture(Image.FromFile(QRname));

            if (filepaths != "")
            {
                DocPicture footerimage = paragraph2.AppendPicture(Image.FromFile(filepaths));

                footerimage.Width = 192F;

                footerimage.Height = 84F;
            }

            footerimage2.Width = 80;
            footerimage2.Height = 80;
            paragraph3.Format.HorizontalAlignment = HorizontalAlignment.Right;

            paragraph2.Format.HorizontalAlignment = HorizontalAlignment.Right;






            document.SaveToFile(DOCFILE, FileFormat.Docx);

        }

        #endregion




        #region Commision Rates

        [HttpGet]
        public ActionResult CommisionRates()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.SectionHeaderTitle = "Basic Data";
            ViewBag.SectionSub_HeaderTitle = "Make Description List";
            ViewBag.PageTitle = "CommisionRates";

            ViewBag.ItemCategoryList = _oBasic.Get_ItemCategory_DAL(c_ID).ToList();
            //---Make list for page
            _oBasic.CommisionRateobjList = _oBasic.Get_CommisionRatesList_DAL();

            return View(_oBasic);
        }

      
        [HttpPost]
        public IActionResult AddCommisionRate(int Category_ID, decimal Rate ,int Status )
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (Category_ID == 0)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("CommisionRates", "BasicData");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            //---saving ItemCategory in database
            try
            {
                message = _oBasic.Insert_CommisionRates_DAL(Category_ID, Rate, Status, Created_by);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("CommisionRates", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("CommisionRates", "BasicData");
            }

            //return View();
        }


        [HttpPost]
        public IActionResult UpdateCommisionRate(int? CommissionRates_ID, int Category_ID, decimal Rate, int Status)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);

            if (CommissionRates_ID == null || CommissionRates_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("CommisionRates", "BasicData");
            }



            //---saving Make in database
            try
            {
                message = _oBasic.Update_CommisionRates_DAL(CommissionRates_ID ,Category_ID, Rate, Status, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("CommisionRates", "BasicData");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("CommisionRates", "BasicData");
            }

            //return View();
        }


    
        [HttpPost]
        public IActionResult DeleteCommisionRate(int? CommissionRates_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (CommissionRates_ID == null || CommissionRates_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("CommisionRates", "BasicData");
            }
            string message = "";

            try
            {
                message = _oBasic.Delete_CommisionRates_DAL(CommissionRates_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("CommisionRates", "BasicData");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("CommisionRates", "BasicData");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("CommisionRates", "BasicData");

            }


        }

        #endregion

    }
}
