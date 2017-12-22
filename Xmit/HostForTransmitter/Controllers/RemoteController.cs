using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using mcTransmitter;
using OracleFirewall.DTOs;
using HostForTransmitter.Code;
using OracleFirewall.Interfaces;

namespace OracleFirewall.Controllers {
    public class RemoteController : ApiController {

        #region Private Members

        private StringBuilder sb = new StringBuilder();

        #endregion

        #region Private Methods

        private bool ValidateKey(string key) {
            try {
                var cipherTextBytes = Convert.FromBase64String(key);
                var keyBytes = new Rfc2898DeriveBytes(Constants.hash, Encoding.ASCII.GetBytes(Constants.salt)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.None
                };

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(Constants.VIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                var plainTextBytes = new byte[cipherTextBytes.Length];

                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                var decryptedString = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
                var companyIndex = decryptedString.IndexOf("||");
                var companyIdFromKey = int.Parse(decryptedString.Substring(0, companyIndex));
                var companyId = int.Parse(ConfigurationManager.AppSettings["CompanyId"]);
                if (companyId != companyIdFromKey) {
                    return false;
                }
                decryptedString = decryptedString.Substring(companyIndex + 2);

                var lastPipe = decryptedString.LastIndexOf("||");
                var timeString = decryptedString.Substring(lastPipe + 2, decryptedString.Length - lastPipe - 2);
                var time = DateTime.ParseExact(timeString, Constants.SecurityTokenDateFormat, CultureInfo.InvariantCulture);

                var ts = DateTime.Now.Subtract(time);
                return ts.Minutes < 15;
            }
            catch (Exception) {
                return false;
            }
        }

        private void AddDtoToResult(ResultDto result, string tableName, int recCount) {
            result.Records.Add(new RecordDto() {
                TableName = tableName,
                RecordCount = recCount
            });
        }

        //private void ProcessDates (JObject obj, ref OracleObject oraObj) {
        private bool ProcessDates(JProperty prop, ref List<DateTime> dates) {
            try {
                //return true;
                Newtonsoft.Json.Linq.JArray o;
                o = (Newtonsoft.Json.Linq.JArray)prop.Value;
                var x = (JObject)o[0];
                var yesterday = (DateTime)x["Yesterday"];
                var tomorrow = (DateTime)x["Tomorrow"];
                var key = (string)x["Key"];
                if (ValidateKey(key)) {
                    dates = new List<DateTime> { yesterday, tomorrow };
                    //oraObj.Dates = dates;
                    return true;
                }
                return false;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        private string ProcessSourceSystem(JProperty prop, ref List<SourceSystem> srcSystems) {
            try {
                JArray o;
                o = (JArray)prop.Value;
                var x = (JObject)o[0];
                var ss = new List<SourceSystem>();
                var item = new SourceSystem {
                    SourceSystemID = (int)x["SourceSystemID"],
                    SystemName = (string)x["SystemName"],
                    MsModificationDate = (DateTime)x["MsModificationDate"]
                };
                ss.Add(item);
                srcSystems = ss;
                return "success on source system";
            }
            catch (Exception e) {
                sb.AppendLine("Failure in ProcessSourceSystem");
                sb.AppendLine(e.Message);
                sb.AppendLine(e.ToString());
                throw e;
            }
        }

        private void ProcessRoles(JProperty prop, ref List<Role> roles) {
            roles = new List<Role>();
            foreach (var item in prop.First().ToList()) {
                var role = new Role();
                role.RoleID = (int)item["RoleID"];
                role.RoleDescription = (string)item["RoleDescription"];
                role.MsModificationDate = (DateTime)item["MsModificationDate"];
                roles.Add(role);
            }
        }

        private void ProcessStatuses(JProperty prop, ref List<Status> statuses) {
            statuses = new List<Status>();
            foreach (var item in prop.First().ToList()) {
                var status = new Status();
                status.StatusID = (int)item["StatusID"];
                status.StatusName = (string)item["StatusName"];
                status.Inserted = (DateTime)item["Inserted"];
                status.Modified = (DateTime)item["Modified"];
                status.MsModificationDate = (DateTime)item["MsModificationDate"];
                statuses.Add(status);
            }
        }

        private void ProcessQuoteTypes(JProperty prop, ref List<QuoteType> qtTypes) {
            qtTypes = new List<QuoteType>();
            foreach (var item in prop.First().ToList()) {
                var qt = new QuoteType();
                qt.QuoteTypeID = (int)item["QuoteTypeID"];
                qt.CompanyID = (int)item["CompanyID"];
                qt.QuoteTypeName = (string)item["QuoteTypeName"];
                qt.Section1 = (string)item["Section1"];
                qt.Section2 = (string)item["Section2"];
                qt.Section3 = (string)item["Section3"];
                qt.ImageLocation = (string)item["ImageLocation"];
                qt.Inactive = (bool)item["Inactive"];
                qt.MsModificationDate = (DateTime)item["MsModificationDate"];
                qtTypes.Add(qt);
            }
        }

        private void ProcessQuoteStatuses(JProperty prop, ref List<QuoteStatus> qtStatuses) {
            qtStatuses = new List<QuoteStatus>();
            foreach (var item in prop.First().ToList()) {
                var qs = new QuoteStatus();
                qs.QuoteStatusID = (int)item["QuoteStatusID"];
                qs.QuoteStatusName = (string)item["QuoteStatusName"];
                qs.SortOrder = (int)item["SortOrder"];
                qs.MsModificationDate = (DateTime)item["MsModificationDate"];
                qtStatuses.Add(qs);
            }
        }

        private void ProcessQuoteSections(JProperty prop, ref List<QuoteSection> qtSections) {
            qtSections = new List<QuoteSection>();
            foreach (var item in prop.First().ToList()) {
                var qs = new QuoteSection();
                qs.QuoteSectionID = (int)item["QuoteSectionID"];
                qs.QuoteTypeID = (int)item["QuoteTypeID"];
                qs.SectionContent = (byte[])item["SectionContent"];
                qs.PositionTop = (int)item["PositionTop"];
                qs.PositionLeft = (int)item["PositionLeft"];
                qs.Height = (int)item["Height"];
                qs.Width = (int)item["Width"];
                qs.IsProducts = (bool)item["IsProducts"];
                qs.IsSurcharges = (bool)item["IsSurcharges"];
                qs.IsLogo = (bool)item["IsLogo"];
                qs.IsCompanyInfo = (bool)item["IsCompanyInfo"];
                qs.IsCustomerInfo = (bool)item["IsCustomerInfo"];
                qs.IsSignature = (bool)item["IsSignature"];
                qs.IsExpiration = (bool)item["IsExpiration"];
                qs.IsQuoteNumber = (bool)item["IsQuoteNumber"];
                qs.ProductColumns = (string)item["ProductColumns"];
                qs.MsModificationDate = (DateTime)item["MsModificationDate"];
                qtSections.Add(qs);
            }
        }

        private void ProcessProjectChargeTypes(JProperty prop, ref List<ProjectChargeType> pcTypes) {
            pcTypes = new List<ProjectChargeType>();
            foreach (var item in prop.First().ToList()) {
                var pct = new ProjectChargeType();
                pct.ChargeTypeID = (int)item["ChargeTypeID"];
                pct.ChargeTypeName = (string)item["ChargeTypeName"];
                pct.MsModificationDate = (DateTime)item["MsModificationDate"];
                pcTypes.Add(pct);
            }
        }

        private void ProcessProjectBidderStatuses(JProperty prop, ref List<ProjectBidderStatus> pbs) {
            pbs = new List<ProjectBidderStatus>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProjectBidderStatus();
                obj.ProjectBidderStatusID = (int)item["ProjectBidderStatusID"];
                obj.StatusName = (string)item["StatusName"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                pbs.Add(obj);
            }
        }

        private void ProcessEventStatuses(JProperty prop, ref List<EventStatu> objList) {
            objList = new List<EventStatu>();
            foreach (var item in prop.First().ToList()) {
                var obj = new EventStatu();
                obj.EventStatusID = (int)item["EventStatusID"];
                obj.EventStatus = (string)item["EventStatus"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessDeviationTypes(JProperty prop, ref List<DeviationType> objList) {
            objList = new List<DeviationType>();
            foreach (var item in prop.First().ToList()) {
                var obj = new DeviationType();
                obj.DeviationTypeID = (int)item["DeviationTypeID"];
                obj.DeviationTypeName = (string)item["DeviationTypeName"];
                obj.DeviationTypeName = (string)item["DeviationTypeName"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessUnitsOfMeasure(JProperty prop, ref List<Unit_Of_Measure> objList) {
            objList = new List<Unit_Of_Measure>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Unit_Of_Measure();
                obj.Unit_Of_Measure_ID = (int)item["Unit_Of_Measure_ID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.Imperial_Description = (string)item["Imperial_Description"];
                obj.Metric_Description = (string)item["Metric_Description"];
                Decimal dec = 0;
                try {
                    dec = (decimal)item["Imperial_To_Metric_Conv_Factor"];
                    obj.Imperial_To_Metric_Conv_Factor = dec;
                }
                catch (Exception) {
                    obj.Imperial_To_Metric_Conv_Factor = null;
                }
                try {
                    dec = (decimal)item["Conversion_Factor_Type_ID"];
                    obj.Conversion_Factor_Type_ID = dec;
                }
                catch (Exception) {
                    obj.Conversion_Factor_Type_ID = null;
                }
                try {
                    dec = (decimal)item["Conversion_Factor_Value"];
                    obj.Conversion_Factor_Value = dec;
                }
                catch (Exception) {
                    obj.Conversion_Factor_Value = null;
                }
                obj.ConnectionCode = (string)item["ConnectionCode"];
                obj.UOM = (string)item["UOM"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessCompanies(JProperty prop, ref List<Company> objList) {
            objList = new List<Company>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Company();
                obj.CompanyID = (int)item["CompanyID"];
                obj.CompanyName = (string)item["CompanyName"];
                obj.Address1 = (string)item["Address1"];
                obj.Address2 = (string)item["Address2"];
                obj.City = (string)item["City"];
                obj.ST = (string)item["ST"];
                obj.Zip = (string)item["Zip"];
                obj.Phone1 = (string)item["Phone1"];
                obj.Phone2 = (string)item["Phone2"];
                obj.Fax = (string)item["Fax"];
                obj.ServiceUrl = (string)item["ServiceUrl"];
                byte[] bytes;
                try {
                    bytes = (byte[])item["QuoteImage"];
                    obj.QuoteImage = bytes;
                }
                catch (Exception) {
                    obj.QuoteImage = null;
                }
                obj.SourceSystemID = (int)item["SourceSystemID"];
                obj.ChannelID = (string)item["ChannelID"];
                obj.InstanceID = (int?)item["InstanceID"];
                obj.CompanyCode = (string)item["CompanyCode"];
                obj.UsesCategories = (bool?)item["UsesCategories"];
                obj.MaxQuantity = (int)item["MaxQuantity"];
                obj.DefaultUomID = (int?)item["DefaultUomID"];
                obj.Cert509 = (string)item["Cert509"];
                obj.UsesSAML = (bool)item["UsesSAML"];
                obj.TimeoutRedirect = (string)item["TimeoutRedirect"];
                obj.ChangeProjectCode = (bool)item["ChangeProjectCode"];
                obj.UsesPriceIntegration = (bool)item["UsesPriceIntegration"];
                obj.UsesSalesOrders = (bool)item["UsesSalesOrders"];
                obj.Country = (string)item["Country"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessCompanyOrderStatuses(JProperty prop, ref List<CompanyOrderStatus> objList) {
            objList = new List<CompanyOrderStatus>();
            foreach (var item in prop.First().ToList()) {
                var obj = new CompanyOrderStatus();
                obj.CompanyOrderStatusID = (int)item["CompanyOrderStatusID"];
                obj.CompanyOrderStatusName = (string)item["CompanyOrderStatusName"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessCompanyProjectProductStatuses(JProperty prop, ref List<CompanyProjectProductStatus> objList) {
            objList = new List<CompanyProjectProductStatus>();
            foreach (var item in prop.First().ToList()) {
                var obj = new CompanyProjectProductStatus();
                obj.CompanyProjectProductStatusID = (int)item["CompanyProjectProductStatusID"];
                obj.ProjectProductStatusName = (string)item["CompanyProjectProductStatusName"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessCompanyProjectStatuses(JProperty prop, ref List<CompanyProjectStatus> objList) {
            objList = new List<CompanyProjectStatus>();
            foreach (var item in prop.First().ToList()) {
                var obj = new CompanyProjectStatus();
                obj.CompanyProjectStatusID = (int)item["CompanyProjectStatusID"];
                obj.CompanyProjectStatusName = (string)item["CompanyProjectStatusName"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessCompanyQuoteStatuses(JProperty prop, ref List<CompanyQuoteStatus> objList) {
            objList = new List<CompanyQuoteStatus>();
            foreach (var item in prop.First().ToList()) {
                var obj = new CompanyQuoteStatus();
                obj.CompanyQuoteStatusID = (int)item["CompanyQuoteStatusID"];
                obj.CompanyQuoteStatusName = (string)item["CompanyQuoteStatusName"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessCreditStatuses(JProperty prop, ref List<CreditStatus> objList) {
            objList = new List<CreditStatus>();
            foreach (var item in prop.First().ToList()) {
                var obj = new CreditStatus();
                obj.CreditStatusID = (int)item["CreditStatusID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.Code = (string)item["Code"];
                obj.Description = (string)item["Description"];
                obj.ColorCode = (string)item["ColorCode"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessCustomerEventTypes(JProperty prop, ref List<CustomerEventType> objList) {
            objList = new List<CustomerEventType>();
            foreach (var item in prop.First().ToList()) {
                var obj = new CustomerEventType();
                obj.EventTypeID = (int)item["EventTypeID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.EventType = (string)item["EventType"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessCustomerTypes(JProperty prop, ref List<CustomerType> objList) {
            objList = new List<CustomerType>();
            foreach (var item in prop.First().ToList()) {
                var obj = new CustomerType();
                obj.CustomerTypeID = (int)item["CustomerTypeID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.CustomerTypeName = (string)item["CustomerTypeName"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessDivisions(JProperty prop, ref List<Division> objList) {
            objList = new List<Division>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Division();
                obj.DivisionID = (int)item["DivisionID"];
                obj.DivisionName = (string)item["DivisionName"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.Address1 = (string)item["Address1"];
                obj.Address2 = (string)item["Address2"];
                obj.City = (string)item["City"];
                obj.ST = (string)item["ST"];
                obj.Zip = (string)item["Zip"];
                obj.Country = (string)item["Country"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessEventFrequencies(JProperty prop, ref List<EventFrequency> objList) {
            objList = new List<EventFrequency>();
            foreach (var item in prop.First().ToList()) {
                var obj = new EventFrequency();
                obj.EventFrequencyID = (int)item["EventFrequencyID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.EventFrequency1 = (string)item["EventFrequency1"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessLinesOfBusiness(JProperty prop, ref List<LinesOfBusiness> objList) {
            objList = new List<LinesOfBusiness>();
            foreach (var item in prop.First().ToList()) {
                var obj = new LinesOfBusiness();
                obj.LineOfBusinessID = (int)item["LineOfBusinessID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.Name = (string)item["Name"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                obj.LineOfBusinessCode = string.IsNullOrEmpty((string)item["LineOfBusinessCode"]) ? " " : (string)item["LineOfBusinessCode"];
                objList.Add(obj);
            }
        }

        private void ProcessLostReasons(JProperty prop, ref List<LostReason> objList) {
            objList = new List<LostReason>();
            foreach (var item in prop.First().ToList()) {
                var obj = new LostReason();
                obj.LostReasonID = (int)item["LostReasonID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.LostReason1 = (string)item["LostReason1"];
                obj.Inserted = (DateTime)item["Inserted"];
                obj.Modified = (DateTime)item["Modified"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessPaymentTerms(JProperty prop, ref List<PaymentTerm> objList) {
            objList = new List<PaymentTerm>();
            foreach (var item in prop.First().ToList()) {
                var obj = new PaymentTerm();
                obj.Terms_Discount_ID = (int)item["Terms_Discount_ID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.Description = (string)item["Description"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProjectMapQtyRanges(JProperty prop, ref List<ProjectMapQtyRanx> objList) {
            objList = new List<ProjectMapQtyRanx>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProjectMapQtyRanx();
                obj.ProjectMapQtyRangeID = (int)item["ProjectMapQtyRangeID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.Min = (int)item["Min"];
                obj.Max = (int)item["Max"];
                obj.DisplayText = (string)item["DisplayText"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProjectTypes(JProperty prop, ref List<ProjectType> objList) {
            objList = new List<ProjectType>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProjectType();
                obj.ProjectTypeID = (int)item["ProjectTypeID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.ConnectionCode = (string)item["ConnectionCode"];
                obj.SANL_Code = (string)item["SANL_Code"];
                obj.ProjectTypeName = (string)item["ProjectTypeName"];
                obj.ShortName = (string)item["ShortName"];
                obj.Inserted = (DateTime)item["Inserted"];
                obj.Modified = (DateTime)item["Modified"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessSources(JProperty prop, ref List<Source> objList) {
            objList = new List<Source>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Source();
                obj.SourceID = (int)item["SourceID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.SourceName = (string)item["SourceName"];
                obj.Inserted = (DateTime?)item["Inserted"];
                obj.Modified = (DateTime?)item["Modified"];
                obj.Inactive = (bool)item["Inactive"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }


        private void ProcessTermsDiscount(JProperty prop, ref List<Terms_Discount> objList) {
            objList = new List<Terms_Discount>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Terms_Discount();
                obj.Terms_Discount_ID = (int)item["Terms_Discount_ID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.Discount_Days = (int?)item["Discount_Days"];
                obj.Discount_Amount = (decimal)item["Discount_Amount"];
                obj.Net_Days = (int?)item["Net_Days"];
                obj.Description = (string)item["Description"];
                obj.Discount_Taxes = (bool)item["Discount_Taxes"];
                obj.Percent_Flag = (bool)item["Discount_Taxes"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessVehicleTypes(JProperty prop, ref List<VehicleType> objList) {
            objList = new List<VehicleType>();
            foreach (var item in prop.First().ToList()) {
                var obj = new VehicleType();
                obj.VehicleTypeID = (int)item["VehicleTypeID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.Description = (string)item["Description"];
                obj.Abbreviation = (string)item["Abbreviation"];
                obj.LocalVehicleTypeId = (string)item["LocalVehicleTypeId"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessZoneCodes(JProperty prop, ref List<ZoneCode> objList) {
            objList = new List<ZoneCode>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ZoneCode();
                obj.ZoneCodeID = (int)item["ZoneCodeID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.ZoneCodeName = (string)item["ZoneCodeName"];
                obj.SystemCode = (string)item["SystemCode"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessCompanyBidStatuses(JProperty prop, ref List<CompanyBidStatus> objList) {
            objList = new List<CompanyBidStatus>();
            foreach (var item in prop.First().ToList()) {
                var obj = new CompanyBidStatus();
                obj.CompanyBidStatusID = (int)item["CompanyBidStatusID"];
                obj.CompanyBidStatusName = (string)item["CompanyBidStatusName"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.SystemCode = (string)item["SystemCode"];
                obj.MSStatusID = (int)item["MSStatusID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessPlants(JProperty prop, ref List<Plant> objList) {
            objList = new List<Plant>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Plant();
                obj.PlantID = (int)item["PlantID"];
                obj.Description = (string)item["Description"];
                obj.SystechCompNbr = (string)item["SystechCompNbr"];
                obj.CSPlantCode = (string)item["CSPlantCode"];
                obj.CSShortName = (string)item["CSShortName"];
                obj.CSPlantName = (string)item["CSPlantName"];
                obj.Inserted = (DateTime)item["Inserted"];
                obj.Modified = (DateTime)item["Modified"];
                obj.Inactive_Flag = (bool)item["Inactive_Flag"];
                obj.DivisionID = (int)item["DivisionID"];
                obj.LineOfBusinessID = (int)item["LineOfBusinessID"];
                obj.PlantCost = (decimal?)item["PlantCost"];
                obj.DeliveryCost = (decimal?)item["DeliveryCost"];
                obj.MaterialCost = (decimal?)item["MaterialCost"];
                obj.SGACost = (decimal?)item["SGACost"];
                obj.Address1 = (string)item["Address1"];
                obj.Address2 = (string)item["Address2"];
                obj.City = (string)item["City"];
                obj.State = (string)item["State"];
                obj.Zip = (string)item["Zip"];
                obj.Longitude = (decimal?)item["Longitude"];
                obj.Latitude = (decimal?)item["Latitude"];
                obj.Country = (string)item["Country"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessPlantLinesOfBusiness(JProperty prop, ref List<PlantLinesOfBusiness> objList) {
            objList = new List<PlantLinesOfBusiness>();
            foreach (var item in prop.First().ToList()) {
                var obj = new PlantLinesOfBusiness();
                obj.PlantLineOfBusinessID = (int)item["PlantLineOfBusinessID"];
                obj.LineOfBusinessID = (int)item["LineOfBusinessID"];
                obj.PlantID = (int)item["PlantID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProductLines(JProperty prop, ref List<ProductLine> objList) {
            objList = new List<ProductLine>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProductLine();
                obj.ProductLineID = (int)item["ProductLineID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.ProductLineCode = (string)item["ProductLineCode"];
                obj.ProductLineName = (string)item["ProductLineName"];
                obj.Active = (bool)item["Active"];
                obj.extProductLineCode = (string)item["extProductLineCode"];
                obj.LineOfBusinessID = (int)item["LineOfBusinessID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProductTypes(JProperty prop, ref List<ProductType> objList) {
            objList = new List<ProductType>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProductType();
                obj.ProductTypeID = (int)item["ProductTypeID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.ProductTypeName = (string)item["ProductTypeName"];
                obj.ProductTypeCode = (string)item["ProductTypeCode"];
                obj.ProductLineID = (int)item["ProductLineID"];
                obj.ListPrice = (decimal)item["ListPrice"];
                obj.UOMID = (int?)item["UOMID"];
                obj.Inactive = (bool)item["Inactive"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProductUsages(JProperty prop, ref List<ProductUsage> objList) {
            objList = new List<ProductUsage>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProductUsage();
                obj.ProductUsageID = (int)item["ProductUsageID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.ProductUsageName = (string)item["ProductUsageName"];
                obj.ProductLineID = (int?)item["ProductLineID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessSurcharges(JProperty prop, ref List<Surcharge> objList) {
            objList = new List<Surcharge>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Surcharge();
                obj.SurchargeID = (int)item["SurchargeID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.SurchargeName = (string)item["SurchargeName"];
                obj.Rate = (int)item["Rate"];
                obj.UOMID = (int)item["UOMID"];
                obj.Active = (bool)item["Active"];
                obj.QuoteDefault = (bool)item["QuoteDefault"];
                obj.QuoteSelectable = (bool)item["QuoteSelectable"];
                obj.PlantID = (int?)item["PlantID"];
                obj.LocalSurchargeID = (string)item["LocalSurchargeID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProducts(JProperty prop, ref List<Product> objList) {
            objList = new List<Product>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Product();
                obj.ProductID = (int)item["ProductID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.ProductName = (string)item["ProductName"];
                obj.Active = (bool?)item["Active"];
                obj.AU_ID = (int?)item["AU_ID"];
                obj.UnitOfMeasureID = (int?)item["UnitOfMeasureID"];
                obj.ProductTypeID = (int)item["ProductTypeID"];
                obj.ConnectionCode = (string)item["ConnectionCode"];
                obj.ITEM_CAT = (string)item["ITEM_CAT"];
                obj.Product_Code = (string)item["Product_Code"];
                obj.Inserted = (DateTime?)item["Inserted"];
                obj.Modified = (DateTime?)item["Modified"];
                obj.extProduct_Code = (string)item["extProduct_Code"];
                obj.IsSellable = (bool)item["IsSellable"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                obj.ProductShortName = (string)item["ProductShortName"];
                objList.Add(obj);
            }
        }

        private void ProcessPlantProductPrices(JProperty prop, ref List<PlantProductPrice> objList) {
            objList = new List<PlantProductPrice>();
            foreach (var item in prop.First().ToList()) {
                var obj = new PlantProductPrice();
                obj.PlantProductPriceID = (int)item["PlantProductPriceID"];
                obj.PlantID = (int)item["PlantID"];
                obj.ProductID = (int)item["ProductID"];
                obj.ListPrice = (decimal)item["ListPrice"];
                obj.Description = (string)item["Description"];
                obj.PriceListID = (int)item["PriceListID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                obj.PriceExtensionCode = (string)item["PriceExtensionCode"];
                objList.Add(obj);
            }
        }

        private void ProcessProjectCharges(JProperty prop, ref List<ProjectCharge> objList) {
            objList = new List<ProjectCharge>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProjectCharge();
                obj.ChargeID = (int)item["ChargeID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.ChargeName = (string)item["ChargeName"];
                obj.ChargeType = (int)item["ChargeType"];
                obj.ChargeCode = (string)item["ChargeCode"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessStandardClauses(JProperty prop, ref List<StandardClaus> objList) {
            objList = new List<StandardClaus>();
            foreach (var item in prop.First().ToList()) {
                var obj = new StandardClaus();
                obj.StandardClauseID = (int)item["StandardClauseID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.Name = (string)item["Name"];
                obj.StandardClauseText = (string)item["StandardClauseText"];
                obj.ProductLineID = (int?)item["ProductLineID"];
                obj.PlantID = (int?)item["PlantID"];
                obj.ProductID = (int?)item["ProductID"];
                obj.Inactive = (bool)item["Inactive"];
                obj.ProductUsageID = (int?)item["ProductUsageID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessUsers(JProperty prop, ref List<User> objList) {
            objList = new List<User>();
            foreach (var item in prop.First().ToList()) {
                var obj = new User();
                obj.UserID = (int)item["UserID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.UserName = (string)item["UserName"];
                obj.FirstName = (string)item["FirstName"];
                obj.LastName = (string)item["LastName"];
                obj.Active = (bool?)item["Active"];
                obj.InsertDate = (DateTime?)item["InsertDate"];
                obj.EmailAddress = (string)item["EmailAddress"];
                obj.Password = (byte[])item["Password"];
                obj.CellPhoneNumber = (string)item["CellPhoneNumber"];
                obj.PhoneNumber = (string)item["PhoneNumber"];
                obj.FaxNumber = (string)item["FaxNumber"];
                obj.SalesmanCode = (string)item["SalesmanCode"];
                obj.ReportsToID = (int?)item["ReportsToID"];
                obj.DeviationTypeID = (int)item["DeviationTypeID"];
                obj.DeviationAmount = (decimal?)item["DeviationAmount"];
                obj.Address1 = (string)item["Address1"];
                obj.Address2 = (string)item["Address2"];
                obj.City = (string)item["City"];
                obj.ST = (string)item["ST"];
                obj.Zip = (string)item["Zip"];
                obj.Latitude = (decimal?)item["Latitude"];
                obj.Longitude = (decimal?)item["Longitude"];
                obj.UsesSAML = (bool)item["UsesSAML"];
                obj.Country = (string)item["Country"];
                obj.DefaultMailMessage = (string)item["DefaultMailMessage"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                obj.AdministratorID = (int?)item["AdministratorID"];
                objList.Add(obj);
            }
        }

        private void ProcessUserRoles(JProperty prop, ref List<UserRole> objList) {
            objList = new List<UserRole>();
            foreach (var item in prop.First().ToList()) {
                var obj = new UserRole();
                obj.UserRoleID = (int)item["UserRoleID"];
                obj.RoleID = (int)item["RoleID"];
                obj.UserID = (int)item["UserID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessCompanyUsers(JProperty prop, ref List<CompanyUser> objList) {
            objList = new List<CompanyUser>();
            foreach (var item in prop.First().ToList()) {
                var obj = new CompanyUser();
                obj.CompanyUserID = (int)item["CompanyUserID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.UserID = (int)item["UserID"];
                obj.SalesmanCode = (string)item["SalesmanCode"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessCompetitors(JProperty prop, ref List<Competitor> objList) {
            objList = new List<Competitor>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Competitor();
                obj.CompetitorID = (int)item["CompetitorID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.CompetitorName = (string)item["CompetitorName"];
                obj.Inserted = (DateTime)item["Inserted"];
                obj.Modified = (DateTime)item["Modified"];
                obj.Inactive = (bool)item["Inactive"];
                obj.UserID = (int?)item["UserID"];
                obj.Address1 = (string)item["Address1"];
                obj.Address2 = (string)item["Address2"];
                obj.City = (string)item["City"];
                obj.State = (string)item["State"];
                obj.Zip = (string)item["Zip"];
                obj.Longitude = (decimal?)item["Longitude"];
                obj.Latitude = (decimal?)item["Latitude"];
                obj.Country = (string)item["Country"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessCompetitorPlants(JProperty prop, ref List<CompetitorPlant> objList) {
            objList = new List<CompetitorPlant>();
            foreach (var item in prop.First().ToList()) {
                var obj = new CompetitorPlant();
                obj.CompetitorPlantID = (int)item["CompetitorPlantID"];
                obj.CompetitorID = (int)item["CompetitorID"];
                obj.PlantName = (string)item["PlantName"];
                obj.Inserted = (DateTime)item["Inserted"];
                obj.Modified = (DateTime)item["Modified"];
                obj.Inactive_Flag = (bool)item["Inactive_Flag"];
                obj.PlantCost = (decimal?)item["PlantCost"];
                obj.DeliveryCost = (decimal?)item["DeliveryCost"];
                obj.MaterialCost = (decimal?)item["MaterialCost"];
                obj.SGACost = (decimal?)item["SGACost"];
                obj.Address1 = (string)item["Address1"];
                obj.Address2 = (string)item["Address2"];
                obj.City = (string)item["City"];
                obj.State = (string)item["State"];
                obj.Zip = (string)item["Zip"];
                obj.Longitude = (decimal?)item["Longitude"];
                obj.Latitude = (decimal?)item["Latitude"];
                obj.LineOfBusinessID = (int)item["LineOfBusinessID"];
                obj.DivisionID = (int)item["DivisionID"];
                obj.Country = (string)item["Country"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessLogins(JProperty prop, ref List<Login> objList) {
            objList = new List<Login>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Login();
                obj.LoginID = (int)item["LoginID"];
                obj.UserID = (int)item["UserID"];
                obj.LoginDate = (DateTime)item["LoginDate"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessManagersSalesmen(JProperty prop, ref List<ManagersSalesman> objList) {
            objList = new List<ManagersSalesman>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ManagersSalesman();
                obj.ManagerUserID = (int)item["ManagerUserID"];
                obj.ManagerID = (int)item["ManagerID"];
                obj.SalesmanID = (int)item["SalesmanID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProductTemplates(JProperty prop, ref List<ProductTemplate> objList) {
            objList = new List<ProductTemplate>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProductTemplate();
                obj.ProductTemplateID = (int)item["ProductTemplateID"];
                obj.TemplateName = (string)item["TemplateName"];
                obj.CompanyUserID = (int)item["CompanyUserID"];
                obj.Inactive = (bool)item["Inactive"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                obj.Shared = (bool)item["Shared"];
                objList.Add(obj);
            }
        }

        private void ProcessTemplatedProducts(JProperty prop, ref List<TemplatedProduct> objList) {
            objList = new List<TemplatedProduct>();
            foreach (var item in prop.First().ToList()) {
                var obj = new TemplatedProduct();
                obj.TemplatedProductID = (int)item["TemplatedProductID"];
                obj.ProductTemplateID = (int)item["ProductTemplateID"];
                obj.ProductTypeID = (int?)item["ProductTypeID"];
                obj.ProductID = (int?)item["ProductID"];
                obj.PlantID = (int?)item["PlantID"];
                obj.ProductUsageID = (int?)item["ProductUsageID"];
                obj.LineOfBusinessID = (int?)item["LineOfBusinessID"];
                obj.ProductLineID = (int?)item["ProductLineID"];
                obj.Qty = (decimal?)item["Qty"];
                obj.Price = (decimal?)item["Price"];
                obj.Freight = (decimal?)item["Freight"];
                obj.FreightPay = (decimal?)item["FreightPay"];
                obj.Description = (string)item["Description"];
                obj.IncludeInForecast = (bool?)item["IncludeInForecast"];
                obj.Note = (string)item["Note"];
                obj.Category = (string)item["Category"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                obj.UOMID = (int?)item["UOMID"];
                objList.Add(obj);
            }
        }

        private void ProcessProspects(JProperty prop, ref List<Prospect> objList) {
            objList = new List<Prospect>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Prospect();
                obj.ProspectID = (int)item["ProspectID"];
                obj.Private = (bool?)item["Private"];
                obj.OwnerID = (int?)item["OwnerID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.ProspectName = (string)item["ProspectName"];
                obj.Inserted = (DateTime)item["Inserted"];
                obj.Modified = (DateTime)item["Modified"];
                obj.ProspectCode = (string)item["ProspectCode"];
                obj.ConnectionCode = (string)item["ConnectionCode"];
                obj.LocalCustNumber = (string)item["LocalCustNumber"];
                obj.LocalCustName = (string)item["LocalCustName"];
                obj.IsCustomer = (bool)item["IsCustomer"];
                obj.Credit_Status_ID = (int?)item["Credit_Status_ID"];
                obj.CustomerStatus = (string)item["CustomerStatus"];
                obj.Sales = (decimal?)item["Sales"];
                obj.Margin = (decimal?)item["Margin"];
                obj.MaterialMargin = (decimal?)item["MaterialMargin"];
                obj.JobCount = (int?)item["JobCount"];
                obj.Address1 = (string)item["Address1"];
                obj.Address2 = (string)item["Address2"];
                obj.City = (string)item["City"];
                obj.State = (string)item["State"];
                obj.Zip = (string)item["Zip"];
                obj.Phone = (string)item["Phone"];
                obj.HighestBalance = (decimal?)item["HighestBalance"];
                obj.AvgInvoiceAmt = (decimal?)item["AvgInvoiceAmt"];
                obj.AvgDaysToPay = (int?)item["AvgDaysToPay"];
                obj.AvgDaysPastDue = (int?)item["AvgDaysPastDue"];
                obj.TotalDue = (decimal?)item["TotalDue"];
                obj.ZeroTo30Days = (decimal?)item["ZeroTo30Days"];
                obj.ThirtyOneTo60Days = (decimal?)item["ThirtyOneTo60Days"];
                obj.SixtyOneTo90Days = (decimal?)item["SixtyOneTo90Days"];
                obj.NinetyOneTo120Days = (decimal?)item["NinetyOneTo120Days"];
                obj.Over120Days = (decimal?)item["Over120Days"];
                obj.CustomerTypeID = (int?)item["CustomerTypeID"];
                obj.StatementFrequency = (string)item["StatementFrequency"];
                obj.FinanceRate = (decimal?)item["FinanceRate"];
                obj.PaymentTermsID = (int?)item["PaymentTermsID"];
                obj.SalesmanCode = (string)item["SalesmanCode"];
                obj.Fax = (string)item["Fax"];
                obj.WebSite = (string)item["WebSite"];
                obj.AvgRevenue = (decimal?)item["AvgRevenue"];
                obj.NumberofEmployees = (string)item["NumberofEmployees"];
                obj.CompanyOriginDate = (string)item["CompanyOriginDate"];
                obj.BusinessTypeID = (int?)item["BusinessTypeID"];
                obj.Notes = (string)item["Notes"];
                obj.Country = (string)item["Country"];
                obj.PORequired = (bool?)item["PORequired"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProspectNotes(JProperty prop, ref List<ProspectNote> objList) {
            objList = new List<ProspectNote>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProspectNote();
                obj.ProspectNoteID = (int)item["ProspectNoteID"];
                obj.ProspectID = (int)item["ProspectID"];
                obj.Inserted = (DateTime)item["Inserted"];
                obj.CreatedBy = (int?)item["CreatedBy"];
                obj.Subject = (string)item["Subject"];
                obj.Source = (string)item["Source"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessSalesPersonRegions(JProperty prop, ref List<SalesPersonRegion> objList) {
            objList = new List<SalesPersonRegion>();
            foreach (var item in prop.First().ToList()) {
                var obj = new SalesPersonRegion();
                obj.SalesPersonRegionID = (int)item["SalesPersonRegionID"];
                obj.SalesPersonID = (int)item["SalesPersonID"];
                obj.PlantID = (int)item["PlantID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessContacts(JProperty prop, ref List<Contact> objList) {
            objList = new List<Contact>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Contact();
                obj.ContactID = (int)item["ContactID"];
                obj.Private = (bool?)item["Private"];
                obj.OwnerID = (int?)item["OwnerID"];
                obj.FirstName = (string)item["FirstName"];
                obj.LastName = (string)item["LastName"];
                obj.Name = (string)item["Name"];
                obj.ProspectID = (int)item["ProspectID"];
                obj.Email = (string)item["Email"];
                obj.MobilePhone = (string)item["MobilePhone"];
                obj.Phone = (string)item["Phone"];
                obj.Fax = (string)item["Fax"];
                obj.Address = (string)item["Address"];
                obj.FullName = (string)item["FullName"];
                obj.Title = (string)item["Title"];
                obj.StartingYearatCompany = (string)item["StartingYearatCompany"];
                obj.Department = (string)item["Department"];
                obj.Address2 = (string)item["Address2"];
                obj.City = (string)item["City"];
                obj.State = (string)item["State"];
                obj.Zip = (string)item["Zip"];
                obj.Salutation = (string)item["Salutation"];
                obj.Notes = (string)item["Notes"];
                obj.Inserted = (DateTime?)item["Inserted"];
                obj.Modified = (DateTime?)item["Modified"];
                obj.Country = (string)item["Country"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessContactNotes(JProperty prop, ref List<ContactNote> objList) {
            objList = new List<ContactNote>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ContactNote();
                obj.ContactNoteID = (int)item["ContactNoteID"];
                obj.ContactID = (int)item["ContactID"];
                obj.Note = (string)item["Note"];
                obj.Inserted = (DateTime)item["Inserted"];
                obj.CreatedBy = (int?)item["CreatedBy"];
                obj.Subject = (string)item["Subject"];
                obj.Source = (string)item["Source"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProjects(JProperty prop, ref List<Project> objList) {
            objList = new List<Project>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Project();
                obj.ProjectID = (int)item["ProjectID"];
                obj.Private = (bool?)item["Private"];
                obj.OwnerID = (int?)item["OwnerID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.ProjectName = (string)item["ProjectName"];
                obj.ProjectTypeID = (int)item["ProjectTypeID"];
                obj.StatusID = (int)item["StatusID"];
                obj.StatusChangeDate = (DateTime)item["StatusChangeDate"];
                obj.SourceID = (int)item["SourceID"];
                obj.Active = (bool)item["Active"];
                obj.Address1 = (string)item["Address1"];
                obj.Address2 = (string)item["Address2"];
                obj.City = (string)item["City"];
                obj.County = (string)item["County"];
                obj.State = (string)item["State"];
                obj.ZipCode = (string)item["ZipCode"];
                obj.PlantID = (int?)item["PlantID"];
                obj.StatusID = (int)item["StatusID"];
                obj.SalespersonID = (int)item["SalespersonID"];
                obj.BidDate = (DateTime?)item["BidDate"];
                obj.LostReasonID = (int?)item["LostReasonID"];
                obj.LostToID = (int?)item["LostToID"];
                obj.LostNote = (string)item["LostNote"];
                obj.Inserted = (DateTime)item["Inserted"];
                obj.Modified = (DateTime?)item["Modified"];
                obj.PrevStatus = (int)item["PrevStatus"];
                obj.Exported = (bool?)item["Exported"];
                obj.WinningProjectBidderID = (int?)item["WinningProjectBidderID"];
                obj.StartDate = (DateTime?)item["StartDate"];
                obj.EndDate = (DateTime?)item["EndDate"];
                obj.TotalYards = (int?)item["TotalYards"];
                obj.ConnectionCode = (string)item["ConnectionCode"];
                obj.CustCode = (string)item["CustCode"];
                obj.ProjectCode = (string)item["ProjectCode"];
                obj.Longitude = (decimal?)item["Longitude"];
                obj.Latitude = (decimal?)item["Latitude"];
                obj.GeofenceID = (int?)item["GeofenceID"];
                obj.Country = (string)item["Country"];
                obj.ProjectDescription = (string)item["ProjectDescription"];
                obj.ZoneCodeID = (int?)item["ZoneCodeID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessCustomerEvents(JProperty prop, ref List<CustomerEvent> objList) {
            objList = new List<CustomerEvent>();
            foreach (var item in prop.First().ToList()) {
                var obj = new CustomerEvent();
                obj.CustomerEventID = (int)item["CustomerEventID"];
                obj.Private = (bool?)item["Private"];
                obj.OwnerID = (int?)item["OwnerID"];
                obj.EventTypeID = (int)item["EventTypeID"];
                obj.EventStartDate = (DateTime)item["EventStartDate"];
                obj.EventEndDate = (DateTime?)item["EventEndDate"];
                obj.CompletedDate = (DateTime?)item["CompletedDate"];
                obj.Description = (string)item["Description"];
                obj.ContactID = (int?)item["ContactID"];
                obj.UserID = (int)item["UserID"];
                obj.EventFrequencyID = (int)item["EventFrequencyID"];
                obj.ProjectID = (int?)item["ProjectID"];
                obj.MasterEventID = (int?)item["MasterEventID"];
                obj.EventStatusID = (int)item["EventStatusID"];
                obj.Subject = (string)item["Subject"];
                obj.ProspectID = (int?)item["ProspectID"];
                obj.RecurrenceEndDate = (DateTime?)item["RecurrenceEndDate"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProjectProducts(JProperty prop, ref List<ProjectProduct> objList) {
            objList = new List<ProjectProduct>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProjectProduct();
                obj.ProjectProductID = (int)item["ProjectProductID"];
                obj.ProjectID = (int)item["ProjectID"];
                obj.Qty = (decimal)item["Qty"];
                obj.UOMID = (int?)item["UOMID"];
                obj.Inserted = (DateTime)item["Inserted"];
                obj.Modified = (DateTime)item["Modified"];
                obj.Price = (decimal?)item["Price"];
                obj.ProductLineID = (int?)item["ProductLineID"];
                obj.ProductTypeID = (int?)item["ProductTypeID"];
                obj.ProductID = (int?)item["ProductID"];
                obj.PsiID = (int?)item["PsiID"];
                obj.ProductUsageID = (int?)item["ProductUsageID"];
                obj.Note = (string)item["Note"];
                obj.Forecasted = (bool?)item["Forecasted"];
                obj.PlantID = (int?)item["PlantID"];
                obj.Freight = (decimal?)item["Freight"];
                obj.FreightPay = (decimal?)item["FreightPay"];
                obj.CompetitorPrice = (decimal?)item["CompetitorPrice"];
                obj.Category = (string)item["Category"];
                obj.Description = (string)item["Description"];
                obj.LineOfBusinessID = (int?)item["LineOfBusinessID"];
                obj.StatusID = (int?)item["StatusID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                obj.LostReasonID = (int?)item["LostReasonID"];
                obj.LostToID = (int?)item["LostToID"];
                obj.LostNote = (string)item["LostNote"];
                objList.Add(obj);
            }
        }

        private void ProcessSchedules(JProperty prop, ref List<Schedule> objList) {
            objList = new List<Schedule>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Schedule();
                obj.ScheduleID = (int)item["ScheduleID"];
                obj.ProjectProductID = (int)item["ProjectProductID"];
                obj.Active = (bool)item["Active"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessScheduleItems(JProperty prop, ref List<ScheduleItem> objList) {
            objList = new List<ScheduleItem>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ScheduleItem();
                obj.ScheduleItemID = (int)item["ScheduleItemID"];
                obj.ScheduleID = (int)item["ScheduleID"];
                obj.Quantity = (decimal)item["Quantity"];
                obj.Month = (DateTime)item["Month"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessSalespersonContacts(JProperty prop, ref List<SalespersonContact> objList) {
            objList = new List<SalespersonContact>();
            foreach (var item in prop.First().ToList()) {
                var obj = new SalespersonContact();
                obj.SalesPersonContactID = (int)item["SalesPersonContactID"];
                obj.SalesPersonID = (int)item["SalesPersonID"];
                obj.ContactID = (int)item["ContactID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProjectNotes(JProperty prop, ref List<ProjectNote> objList) {
            objList = new List<ProjectNote>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProjectNote();
                obj.NoteID = (int)item["NoteID"];
                obj.ProjectID = (int)item["ProjectID"];
                obj.Note = (string)item["Note"];
                obj.Inserted = (DateTime)item["Inserted"];
                obj.CreatedBy = (int?)item["CreatedBy"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProjectBidders(JProperty prop, ref List<ProjectBidder> objList) {
            objList = new List<ProjectBidder>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProjectBidder();
                obj.ProjectBidderID = (int)item["ProjectBidderID"];
                obj.ProjectID = (int)item["ProjectID"];
                obj.ProspectID = (int)item["ProspectID"];
                obj.Quoted = (bool?)item["Quoted"];
                obj.StatusSub = (bool?)item["StatusSub"];
                obj.QuoteDate = (DateTime?)item["QuoteDate"];
                obj.NotLIkeLIst = (bool?)item["NotLIkeLIst"];
                obj.StatusID = (int?)item["StatusID"];
                obj.PlantID = (int?)item["PlantID"];
                obj.ProjectCode = (string)item["ProjectCode"];
                obj.StartDate = (DateTime?)item["StartDate"];
                obj.EndDate = (DateTime?)item["EndDate"];
                obj.PONumber = (string)item["PONumber"];
                obj.MinLoadCharge = (int?)item["MinLoadCharge"];
                obj.SundryCharge = (int?)item["SundryCharge"];
                obj.SeasonalCharge = (int?)item["SeasonalCharge"];
                obj.UnloadCharge = (int?)item["UnloadCharge"];
                obj.CustJobNbr = (string)item["CustJobNbr"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                obj.EffectiveDate = (DateTime?)item["EffectiveDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProjectBidderProducts(JProperty prop, ref List<ProjectBidderProduct> objList) {
            objList = new List<ProjectBidderProduct>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProjectBidderProduct();
                obj.ProjectBidderProductID = (int)item["ProjectBidderProductID"];
                obj.ProjectBidderID = (int)item["ProjectBidderID"];
                obj.ProjectProductID = (int)item["ProjectProductID"];
                obj.Price = (decimal)item["Price"];
                obj.Inserted = (DateTime)item["Inserted"];
                obj.Modified = (DateTime)item["Modified"];
                obj.SameAsList = (bool?)item["SameAsList"];
                obj.Forecasted = (bool?)item["Forecasted"];
                obj.Freight = (decimal?)item["Freight"];
                obj.FreightPay = (decimal?)item["FreightPay"];
                obj.ListPrice = (decimal?)item["ListPrice"];
                obj.CustomerPrice = (decimal?)item["CustomerPrice"];
                obj.Exported = (bool)item["Exported"];
                obj.Quantity = (decimal?)item["Quantity"];
                obj.StatusID = (int?)item["StatusID"];
                obj.LineNbr = (int)item["LineNbr"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                obj.PreviousPrice = (decimal?)item["PreviousPrice"];
                obj.PreviousDeliveredPrice = (decimal?)item["PreviousDeliveredPrice"];
                objList.Add(obj);
            }
        }

        private void ProcessProjectBidderNotes(JProperty prop, ref List<ProjectBidderNote> objList) {
            objList = new List<ProjectBidderNote>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProjectBidderNote();
                obj.NoteID = (int)item["NoteID"];
                obj.ProjectBidderID = (int)item["ProjectBidderID"];
                obj.Note = (string)item["Note"];
                obj.Inserted = (DateTime)item["Inserted"];
                obj.CreatedBy = (int?)item["CreatedBy"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProjectBidderCharges(JProperty prop, ref List<ProjectBidderCharge> objList) {
            objList = new List<ProjectBidderCharge>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProjectBidderCharge();
                obj.ProjectBidderChargeID = (int)item["ProjectBidderChargeID"];
                obj.ProjectBidderID = (int)item["ProjectBidderID"];
                obj.ProductLineID = (int)item["ProductLineID"];
                obj.ProjectChargeID = (int)item["ProjectChargeID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessOrders(JProperty prop, ref List<Order> objList) {
            objList = new List<Order>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Order();
                obj.OrderID = (int)item["OrderID"];
                obj.CompanyID = (int)item["CompanyID"];
                obj.ProjectID = (int?)item["ProjectID"];
                obj.year = (int?)item["year"];
                obj.month = (int?)item["month"];
                obj.YardsActual = (decimal?)item["YardsActual"];
                obj.MaterialCost = (decimal?)item["MaterialCost"];
                obj.DollarsActual = (decimal?)item["DollarsActual"];
                obj.OrderDate = (DateTime?)item["OrderDate"];
                obj.ShipDate = (DateTime?)item["ShipDate"];
                obj.YardsTicketed = (decimal?)item["YardsTicketed"];
                obj.OnJobTime = (DateTime?)item["OnJobTime"];
                obj.LocationID = (int?)item["LocationID"];
                obj.ProspectID = (int?)item["ProspectID"];
                obj.DispatchOrderID = (int?)item["DispatchOrderID"];
                obj.DispatchOrderNumber = (string)item["DispatchOrderNumber"];
                obj.SalesmanCode = (string)item["SalesmanCode"];
                obj.Inserted = (DateTime?)item["Inserted"];
                obj.Modified = (DateTime?)item["Modified"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessOrderDetails(JProperty prop, ref List<OrderDetail> objList) {
            objList = new List<OrderDetail>();
            foreach (var item in prop.First().ToList()) {
                var obj = new OrderDetail();
                obj.OrderDetailID = (int)item["OrderDetailID"];
                obj.OrderID = (int)item["OrderID"];
                obj.ProductID = (int)item["ProductID"];
                obj.Quantity = (decimal)item["Quantity"];
                obj.UnitPrice = (decimal)item["UnitPrice"];
                obj.Mix = (bool)item["Mix"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessQuotes(JProperty prop, ref List<Quote> objList) {
            sb.AppendLine("Entering ProcessQuotes");
            objList = new List<Quote>();
            foreach (var item in prop.First().ToList()) {
                var obj = new Quote();
                obj.QuoteID = (int)item["QuoteID"];
                obj.ProjectBidderID = (int)item["ProjectBidderID"];
                obj.ContactID = (int?)item["ContactID"];
                obj.TermsID = (int?)item["TermsID"];
                obj.Attention = (string)item["Attention"];
                obj.Phone = (string)item["Phone"];
                obj.CellPhone = (string)item["CellPhone"];
                obj.Fax = (string)item["Fax"];
                obj.Email = (string)item["Email"];
                obj.QuoteDate = (DateTime)item["QuoteDate"];
                obj.FirmUntilDate = (DateTime)item["FirmUntilDate"];
                obj.QuoteSequenceNumber = (int)item["QuoteSequenceNumber"];
                obj.QuoteTypeID = (int)item["QuoteTypeID"];
                obj.DateCreated = (DateTime)item["DateCreated"];
                obj.DateLastModified = (DateTime)item["DateLastModified"];
                obj.CustomerQuoteID = (string)item["CustomerQuoteID"];
                obj.Exported = (bool)item["Exported"];
                obj.EscalationDate = (DateTime?)item["EscalationDate"];
                obj.QuoteNote = (string)item["QuoteNote"];
                obj.EscalationAmount = (string)item["EscalationAmount"];
                obj.QuoteStatusID = (int)item["QuoteStatusID"];
                obj.Notes = (string)item["Notes"];
                obj.CreatedByID = (int?)item["CreatedByID"];
                obj.OrderDate = (DateTime?)item["OrderDate"];
                obj.StartOrderDate = (DateTime?)item["StartOrderDate"];
                obj.ExpireOrderDate = (DateTime?)item["ExpireOrderDate"];
                obj.VehicleTypeID_1 = (int?)item["VehicleTypeID_1"];
                obj.VehicleTypeID_2 = (int?)item["VehicleTypeID_2"];
                obj.VehicleTypeID_3 = (int?)item["VehicleTypeID_3"];
                obj.VehicleTypeID_4 = (int?)item["VehicleTypeID_4"];
                obj.VehicleTypeID_5 = (int?)item["VehicleTypeID_5"];
                obj.CustomerOrderID = (string)item["CustomerOrderID"];
                obj.PurchaseOrder = (string)item["PurchaseOrder"];
                obj.BeginDate = (DateTime?)item["BeginDate"];
                obj.AllowBeforeStart = (bool?)item["AllowBeforeStart"];
                obj.AllowAfterEnd = (bool?)item["AllowAfterEnd"];
                obj.AllowExceedQty = (bool?)item["AllowExceedQty"];
                obj.AllowExceedLoads = (bool?)item["AllowExceedLoads"];
                obj.AllowAfterComplete = (bool?)item["AllowAfterComplete"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                obj.SalespersonID = (int?)item["SalespersonID"];
                obj.ApexNoteID = (bool?)item["ApexNoteID"];
                objList.Add(obj);
            }
        }

        private void ProcessQuoteDetails(JProperty prop, ref List<QuoteDetail> objList) {
            objList = new List<QuoteDetail>();
            foreach (var item in prop.First().ToList()) {
                var obj = new QuoteDetail();
                obj.QuoteDetailID = (int)item["QuoteDetailID"];
                obj.QuoteID = (int)item["QuoteID"];
                obj.ProjectProductID = (int)item["ProjectProductID"];
                obj.Price = (decimal)item["Price"];
                obj.Qty = (decimal)item["Qty"];
                obj.Freight = (decimal)item["Freight"];
                obj.FreightPay = (decimal)item["FreightPay"];
                obj.ListPrice = (decimal?)item["ListPrice"];
                obj.CustomerPrice = (decimal?)item["CustomerPrice"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessQuoteStandardClauses(JProperty prop, ref List<QuoteStandardClaus> objList) {
            objList = new List<QuoteStandardClaus>();
            foreach (var item in prop.First().ToList()) {
                var obj = new QuoteStandardClaus();
                obj.QuoteStandardClauseID = (int)item["QuoteStandardClauseID"];
                obj.QuoteID = (int)item["QuoteID"];
                obj.StandardClauseID = (int)item["StandardClauseID"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessQuoteSurcharges(JProperty prop, ref List<QuoteSurcharge> objList) {
            objList = new List<QuoteSurcharge>();
            foreach (var item in prop.First().ToList()) {
                var obj = new QuoteSurcharge();
                obj.QuoteSurchargeID = (int)item["QuoteSurchargeID"];
                obj.QuoteID = (int)item["QuoteID"];
                obj.SurchargeID = (int)item["SurchargeID"];
                obj.Rate = (decimal)item["Rate"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessProjectDistances(JProperty prop, ref List<ProjectDistance> objList) {
            objList = new List<ProjectDistance>();
            foreach (var item in prop.First().ToList()) {
                var obj = new ProjectDistance();
                obj.ProjectDistanceID = (int)item["ProjectDistanceID"];
                obj.ProjectID = (int)item["ProjectID"];
                obj.PlantID = (int)item["PlantID"];
                obj.Miles = (decimal?)item["Miles"];
                obj.Kilometers = (decimal?)item["Kilometers"];
                obj.MsModificationDate = (DateTime)item["MsModificationDate"];
                objList.Add(obj);
            }
        }

        private void ProcessDeletedItems(JProperty prop, ref List<DeletedItem> deletedItems) {
            try {
                deletedItems = new List<DeletedItem>();
                foreach (var item in prop.First().ToList()) {
                    var obj = new DeletedItem();
                    obj.KeyID = (int)item["KeyID"];
                    obj.TableName = (string)item["TableName"];
                    obj.Processed = false;
                    deletedItems.Add(obj);
                }
            }
            catch (Exception ex) {
                sb.AppendLine("Error occurred in ProcessDeletedItems");
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.ToString());
                throw ex;
            }
        }


        #endregion



        [HttpGet]
        public HttpResponseMessage ProcessJobFile1() {
            return Request.CreateResponse(HttpStatusCode.OK, "Got Here");
        }

        [HttpGet]
        public HttpResponseMessage DBTest() {
            try {
                var oc = new OracleController();
                string ssname = oc.ReturnSomethingFromDB();
                oc.Dispose();
                return Request.CreateResponse(HttpStatusCode.OK, ssname);
            }
            catch (Exception e) {
                return Request.CreateResponse(HttpStatusCode.OK, e.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage ProcessCvhFile([FromBody] object data, IOracleController ioc = null) {
            var result = new ResultDto();
            result.Records = new List<RecordDto>();
            try {
                //var oraObj = new OracleObject();
                sb.AppendLine("Entered ProcessCvhFile function");
                if (data == null) {
                    sb.AppendLine("data object is null");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "data object is null");
                }
                var jsonSource = JsonConvert.DeserializeObject(data.ToString());
                //return Request.CreateResponse(HttpStatusCode.OK, "jsonSource processed");
                sb.AppendLine("Deserialized payload into jsonSource");
                var obj = JObject.Parse(jsonSource.ToString());
                //return Request.CreateResponse(HttpStatusCode.OK, obj.ToString());
                data = new object();
                jsonSource = "";
                sb.AppendLine("Properties Count = " + obj.Properties().Count().ToString());
                //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                var dates = new List<DateTime>();
                for (int i = 0; i < obj.Properties().Count(); i++) {
                    var prop = obj.Properties().Skip(i).First();
                    switch (i) {
                        case 0:
                            sb.AppendLine("Preparing to validate key");
                            var validKey = ProcessDates(prop, ref dates);
                            if (validKey) {
                                sb.AppendLine("Key Validated");
                                break;
                            }
                            var e = new Exception();
                            e.Source = "Validation Failure";
                            throw e;
                        case 1:
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            var deletedItems = new List<DeletedItem>();
                            sb.AppendLine("Calling ProcessDeletedItems");
                            ProcessDeletedItems(prop, ref deletedItems);
                            AddDtoToResult(result, "DeleteCompanyUsers", 0);
                            AddDtoToResult(result, "DeleteProjectBidderCharges", 0);
                            AddDtoToResult(result, "DeleteManagersSalesmen", 0);
                            AddDtoToResult(result, "DeleteProjectBidderProducts", 0);
                            AddDtoToResult(result, "DeleteQuoteDetails", 0);
                            AddDtoToResult(result, "DeleteQuoteSurcharges", 0);
                            AddDtoToResult(result, "DeleteQuoteStandardClauses", 0);
                            AddDtoToResult(result, "DeleteContacts", 0);
                            AddDtoToResult(result, "DeleteProspects", 0);
                            AddDtoToResult(result, "DeleteSalespersonContacts", 0);
                            AddDtoToResult(result, "DeleteSalesPersonRegions", 0);
                            AddDtoToResult(result, "DeleteScheduleItems", 0);
                            AddDtoToResult(result, "DeleteTemplatedProducts", 0);
                            AddDtoToResult(result, "DeleteUserRoles", 0);
                            AddDtoToResult(result, "DeleteSchedules", 0);
                            AddDtoToResult(result, "DeleteQuoteSections", 0);
                            AddDtoToResult(result, "DeleteProjectBidders", 0);
                            AddDtoToResult(result, "DeleteProjectProducts", 0);
                            AddDtoToResult(result, "DeleteQuotes", 0);
                            if (deletedItems.Any()) {
                                sb.AppendLine("Calling DeleteItems - " + deletedItems.Count() + " items to be deleted");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.DeleteItems(deletedItems, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling DeleteItems");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                            }
                            break;
                        case 2:
                            sb.AppendLine("Calling ProcessSourceSystem");
                            var ss = new List<SourceSystem>();
                            ProcessSourceSystem(prop, ref ss);
                            sb.AppendLine("ProcessSourceSystem completed");
                            sb.AppendLine("# of SourceSystem records:" + ss.Count.ToString());
                            AddDtoToResult(result, "SourceSystems", ss.Count);
                            if (ss.Any()) {
                                sb.AppendLine("Preparing to call oc.ProcessSourceSystems with " + ss.Count() + " record(s)");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessSourceSystems(ss, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessSourceSystem");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 3:
                            sb.AppendLine("Calling ProcessRoles");
                            var roles = new List<Role>();
                            ProcessRoles(prop, ref roles);
                            AddDtoToResult(result, "Roles", roles.Count);
                            if (roles.Any()) {
                                sb.AppendLine("Calling oc.ProcessRoles");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessRoles(roles, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessRoles");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 4:
                            sb.AppendLine("Calling ProcessStatuses");
                            var statuses = new List<Status>();
                            ProcessStatuses(prop, ref statuses);
                            AddDtoToResult(result, "Statuses", statuses.Count);
                            if (statuses.Any()) {
                                sb.AppendLine("Calling oc.ProcessStatuses");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessStatuses(statuses, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessStatuses");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 5:
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            sb.AppendLine("Calling ProcessQuoteStatuses");
                            var qs = new List<QuoteStatus>();
                            ProcessQuoteStatuses(prop, ref qs);
                            AddDtoToResult(result, "QuoteStatuses", qs.Count);
                            if (qs.Any()) {
                                sb.AppendLine("Calling oc.ProcessQuoteStatuses");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessQuoteStatuses(qs, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessQuoteStatuses");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 6:
                            sb.AppendLine("Calling ProcessQuoteSections");
                            var qtSections = new List<QuoteSection>();
                            ProcessQuoteSections(prop, ref qtSections);
                            AddDtoToResult(result, "QuoteSections", qtSections.Count);
                            if (qtSections.Any()) {
                                sb.AppendLine("Calling oc.ProcessQuoteSections");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessQuoteSections(qtSections, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessQuoteSections");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 7:
                            sb.AppendLine("Calling ProcessProjectChargeTypes");
                            var ct = new List<ProjectChargeType>();
                            ProcessProjectChargeTypes(prop, ref ct);
                            AddDtoToResult(result, "ProjectChargeTypes", ct.Count);
                            if (ct.Any()) {
                                sb.AppendLine("Calling oc.ProcessProjectChargeTypes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProjectChargeTypes(ct, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProjectChargeTypes");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 8:
                            sb.AppendLine("Calling ProcessProjectBidderStatuses");
                            var pbs = new List<ProjectBidderStatus>();
                            ProcessProjectBidderStatuses(prop, ref pbs);
                            AddDtoToResult(result, "ProjectBidderStatuses", pbs.Count);
                            if (pbs.Any()) {
                                sb.AppendLine("Calling oc.ProcessProjectBidderStatuses");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProjectBidderStatuses(pbs, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProjectBidderStatuses");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 9:
                            sb.AppendLine("Calling ProcessEventStatuses");
                            var es = new List<EventStatu>();
                            ProcessEventStatuses(prop, ref es);
                            AddDtoToResult(result, "EventStatus", es.Count);
                            if (es.Any()) {
                                sb.AppendLine("Calling oc.ProcessEventStatus");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessEventStatus(es, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessEventStatus");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }                               
                            }
                            break;
                        case 10:
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            sb.AppendLine("Calling ProcessDeviationTypes");
                            var dt = new List<DeviationType>();
                            ProcessDeviationTypes(prop, ref dt);
                            AddDtoToResult(result, "DeviationTypes", dt.Count);
                            if (dt.Any()) {
                                sb.AppendLine("Calling oc.ProcessDeviationTypes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessDeviationTypes(dt, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessDeviationTypes");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 11:
                            sb.AppendLine("Calling ProcessCompanies");
                            var c = new List<Company>();
                            ProcessCompanies(prop, ref c);
                            AddDtoToResult(result, "Companies", c.Count);
                            if (c.Any()) {
                                sb.AppendLine("Calling oc.ProcessCompanies");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessCompanies(c, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessCompanies");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 12:
                            sb.AppendLine("Calling ProcessUnitsOfMeasure");
                            var uom = new List<Unit_Of_Measure>();
                            ProcessUnitsOfMeasure(prop, ref uom);
                            AddDtoToResult(result, "Unit_Of_Measure", uom.Count);
                            if (uom.Any()) {
                                sb.AppendLine("Calling oc.ProcessUnit_Of_Measure");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessUnit_Of_Measure(uom, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessUnit_Of_Measure");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }                                
                            }
                            break;
                        case 13:
                            sb.AppendLine("Calling quoteTypes");
                            var quoteTypes = new List<QuoteType>();
                            ProcessQuoteTypes(prop, ref quoteTypes);
                            AddDtoToResult(result, "QuoteTypes", quoteTypes.Count);
                            if (quoteTypes.Any()) {
                                sb.AppendLine("Calling oc.ProcessQuoteTypes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessQuoteTypes(quoteTypes, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessQuoteTypes");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }                                
                            }
                            break;
                        case 14:
                            sb.AppendLine("Calling ProcessCompanyOrderStatuses");
                            var cos = new List<CompanyOrderStatus>();
                            ProcessCompanyOrderStatuses(prop, ref cos);
                            AddDtoToResult(result, "CompanyOrderStatuses", cos.Count);
                            if (cos.Any()) {
                                sb.AppendLine("Calling oc.ProcessCompanyOrderStatuses");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessCompanyOrderStatuses(cos, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessCompanyOrderStatuses");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }                                
                            }
                            break;
                        case 15:
                            sb.AppendLine("Calling ProcessCompanyProjectProductStatuses");
                            var cpps = new List<CompanyProjectProductStatus>();
                            ProcessCompanyProjectProductStatuses(prop, ref cpps);
                            AddDtoToResult(result, "CompanyProjectProductStatuses", cpps.Count);
                            if (cpps.Any()) {
                                sb.AppendLine("Calling oc.ProcessCompanyProjectProductStatuses");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessCompanyProjectProductStatuses(cpps, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessCompanyProjectProductStatuses");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 16:
                            sb.AppendLine("Calling ProcessCompanyProjectStatuses");
                            var cps = new List<CompanyProjectStatus>();
                            ProcessCompanyProjectStatuses(prop, ref cps);
                            AddDtoToResult(result, "CompanyProjectStatuses", cps.Count);
                            if (cps.Any()) {
                                sb.AppendLine("Calling oc.ProcessCompanyProjectStatuses");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessCompanyProjectStatuses(cps, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessCompanyProjectStatuses");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 17:
                            sb.AppendLine("Calling ProcessCompanyQuoteStatuses");
                            var cqs = new List<CompanyQuoteStatus>();
                            ProcessCompanyQuoteStatuses(prop, ref cqs);
                            AddDtoToResult(result, "CompanyQuoteStatuses", cqs.Count);
                            if (cqs.Any()) {
                                sb.AppendLine("Calling oc.ProcessCompanyQuoteStatuses");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessCompanyQuoteStatuses(cqs, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessCompanyQuoteStatuses");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 18:
                            sb.AppendLine("Calling ProcessCreditStatuses");
                            var cs = new List<CreditStatus>();
                            ProcessCreditStatuses(prop, ref cs);
                            AddDtoToResult(result, "CreditStatuses", cs.Count);
                            if (cs.Any()) {
                                sb.AppendLine("Calling oc.ProcessCreditStatuses");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessCreditStatuses(cs, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessCreditStatuses");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }                                
                            }
                            break;
                        case 19:
                            sb.AppendLine("Calling ProcessCustomerEventTypes");
                            var cet = new List<CustomerEventType>();
                            ProcessCustomerEventTypes(prop, ref cet);
                            AddDtoToResult(result, "CustomerEventTypes", cet.Count);
                            if (cet.Any()) {
                                sb.AppendLine("Calling oc.ProcessCustomerEventTypes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessCustomerEventTypes(cet, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessCustomerEventTypes");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 20:
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            sb.AppendLine("Calling ProcessCustomerTypes");
                            var cty = new List<CustomerType>();
                            ProcessCustomerTypes(prop, ref cty);
                            AddDtoToResult(result, "CustomerTypes", cty.Count);
                            if (cty.Any()) {
                                sb.AppendLine("Calling oc.ProcessCustomerTypes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessCustomerTypes(cty, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessCustomerTypes");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 21:
                            sb.AppendLine("Calling ProcessDivisions");
                            var divs = new List<Division>();
                            ProcessDivisions(prop, ref divs);
                            AddDtoToResult(result, "Divisions", divs.Count);
                            if (divs.Any()) {
                                sb.AppendLine("Calling oc.ProcessDivisions");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessDivisions(divs, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessDivisions");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }                                
                            }
                            break;
                        case 22:
                            sb.AppendLine("Calling ProcessEventFrequencies");
                            var ef = new List<EventFrequency>();
                            ProcessEventFrequencies(prop, ref ef);
                            AddDtoToResult(result, "EventFrequency", ef.Count);
                            if (ef.Any()) {
                                sb.AppendLine("Calling oc.ProcessEventFrequency");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessEventFrequency(ef, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessEventFrequency");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 23:
                            sb.AppendLine("Calling ProcessLinesOfBusiness");
                            var lob = new List<LinesOfBusiness>();
                            ProcessLinesOfBusiness(prop, ref lob);
                            AddDtoToResult(result, "LinesOfBusiness", lob.Count);
                            if (lob.Any()) {
                                sb.AppendLine("Calling oc.ProcessLinesOfBusiness");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessLinesOfBusiness(lob, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessLinesOfBusiness");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }                                
                            }
                            break;
                        case 24:
                            sb.AppendLine("Calling ProcessLostReasons");
                            var lr = new List<LostReason>();
                            ProcessLostReasons(prop, ref lr);
                            AddDtoToResult(result, "LostReasons", lr.Count);
                            if (lr.Any()) {
                                sb.AppendLine("Calling oc.ProcessLostReasons");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessLostReasons(lr, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessLostReasons");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 25:
                            sb.AppendLine("Calling ProcessPaymentTerms");
                            var pt = new List<PaymentTerm>();
                            ProcessPaymentTerms(prop, ref pt);
                            AddDtoToResult(result, "PaymentTerms", pt.Count);
                            if (pt.Any()) {
                                sb.AppendLine("Calling oc.ProcessPaymentTerms");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessPaymentTerms(pt, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessPaymentTerms");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 26:
                            sb.AppendLine("Calling ProcessProjectMapQtyRanges");
                            var pmqr = new List<ProjectMapQtyRanx>();
                            ProcessProjectMapQtyRanges(prop, ref pmqr);
                            AddDtoToResult(result, "ProjectMapQtyRanges", pmqr.Count);
                            if (pmqr.Any()) {
                                sb.AppendLine("Calling oc.ProcessProjectMapQtyRanges");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProjectMapQtyRanges(pmqr, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProjectMapQtyRanges");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 27:
                            sb.AppendLine("Calling ProcessProjectTypes");
                            var projTypes = new List<ProjectType>();
                            ProcessProjectTypes(prop, ref projTypes);
                            AddDtoToResult(result, "ProjectTypes", projTypes.Count);
                            if (projTypes.Any()) {
                                sb.AppendLine("Calling oc.ProcessProjectTypes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProjectTypes(projTypes, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProjectTypes");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 28:
                            sb.AppendLine("Calling ProcessSources");
                            var srcs = new List<Source>();
                            ProcessSources(prop, ref srcs);
                            AddDtoToResult(result, "Sources", srcs.Count);
                            if (srcs.Any()) {
                                sb.AppendLine("Calling oc.ProcessSources");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessSources(srcs, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessSources");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 29:
                            sb.AppendLine("Calling ProcessTermsDiscount");
                            var td = new List<Terms_Discount>();
                            ProcessTermsDiscount(prop, ref td);
                            AddDtoToResult(result, "Terms_Discount", td.Count);
                            if (td.Any()) {
                                sb.AppendLine("Calling oc.ProcessTerms_Discount");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessTerms_Discount(td, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessTerms_Discount");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }                               
                            }
                            break;
                        case 30:
                            sb.AppendLine("Calling ProcessVehicleTypes");
                            var vt = new List<VehicleType>();
                            ProcessVehicleTypes(prop, ref vt);
                            AddDtoToResult(result, "VehicleTypes", vt.Count);
                            if (vt.Any()) {
                                sb.AppendLine("Calling oc.ProcessVehicleTypes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessVehicleTypes(vt, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessVehicleTypes");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }                                
                            }
                            break;
                        case 31:
                            sb.AppendLine("Calling ProcessZoneCodes");
                            var zc = new List<ZoneCode>();
                            ProcessZoneCodes(prop, ref zc);
                            AddDtoToResult(result, "ZoneCodes", zc.Count);
                            if (zc.Any()) {
                                sb.AppendLine("Calling oc.ProcessZoneCodes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessZoneCodes(zc, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessZoneCodes");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 32:
                            sb.AppendLine("Calling ProcessCompanyBidStatuses");
                            var cbs = new List<CompanyBidStatus>();
                            ProcessCompanyBidStatuses(prop, ref cbs);
                            AddDtoToResult(result, "CompanyBidStatuses", cbs.Count);
                            if (cbs.Any()) {
                                sb.AppendLine("Calling oc.ProcessCompanyBidStatuses");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessCompanyBidStatuses(cbs, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessCompanyBidStatuses");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 33:
                            sb.AppendLine("Calling ProcessPlants");
                            var p = new List<Plant>();
                            ProcessPlants(prop, ref p);
                            AddDtoToResult(result, "Plants", p.Count);
                            if (p.Any()) {
                                sb.AppendLine("Calling oc.ProcessPlants");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessPlants(p, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessPlants");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 34:
                            sb.AppendLine("Calling ProcessPlantLinesOfBusiness");
                            var plob = new List<PlantLinesOfBusiness>();
                            ProcessPlantLinesOfBusiness(prop, ref plob);
                            AddDtoToResult(result, "PlantLinesOfBusiness", plob.Count);
                            if (plob.Any()) {
                                sb.AppendLine("Calling oc.ProcessPlantLinesOfBusiness");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessPlantLinesOfBusiness(plob, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessPlantLinesOfBusiness");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 35:
                            sb.AppendLine("Calling ProcessProductLines");
                            var pl = new List<ProductLine>();
                            ProcessProductLines(prop, ref pl);
                            AddDtoToResult(result, "ProductLines", pl.Count);
                            if (pl.Any()) {
                                sb.AppendLine("Calling oc.ProcessProductLines");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProductLines(pl, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProductLines");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 36:
                            sb.AppendLine("Calling ProcessProductTypes");
                            var pty = new List<ProductType>();
                            ProcessProductTypes(prop, ref pty);
                            AddDtoToResult(result, "ProductTypes", pty.Count);
                            if (pty.Any()) {
                                sb.AppendLine("Calling oc.ProcessProductTypes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProductTypes(pty, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProductTypes");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 37:
                            sb.AppendLine("Calling ProcessProductUsages");
                            var pu = new List<ProductUsage>();
                            ProcessProductUsages(prop, ref pu);
                            AddDtoToResult(result, "ProductUsages", pu.Count);
                            if (pu.Any()) {
                                sb.AppendLine("Calling oc.ProcessProductUsages");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProductUsages(pu, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProductUsages");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 38:
                            sb.AppendLine("Calling ProcessSurcharges");
                            var su = new List<Surcharge>();
                            ProcessSurcharges(prop, ref su);
                            AddDtoToResult(result, "Surcharges", su.Count);
                            if (su.Any()) {
                                sb.AppendLine("Calling oc.ProcessSurcharges");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessSurcharges(su, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessSurcharges");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 39:
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            sb.AppendLine("Calling ProcessProducts");
                            var prods = new List<Product>();
                            ProcessProducts(prop, ref prods);
                            AddDtoToResult(result, "Products", prods.Count);
                            if (prods.Any()) {
                                sb.AppendLine("Received " + prods.Count().ToString() + " Product records");
                                sb.AppendLine("Calling oc.ProcessProducts");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProducts(prods, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProducts");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 40:
                            sb.AppendLine("Calling ProcessPlantProductPrices");
                            var ppp = new List<PlantProductPrice>();
                            ProcessPlantProductPrices(prop, ref ppp);
                            AddDtoToResult(result, "PlantProductPrices", ppp.Count);
                            if (ppp.Any()) {
                                sb.AppendLine("Calling oc.ProcessPlantProductPrices");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessPlantProductPrices(ppp, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessPlantProductPrices");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 41:
                            sb.AppendLine("Calling ProcessProjectCharges");
                            var projChgs = new List<ProjectCharge>();
                            ProcessProjectCharges(prop, ref projChgs);
                            AddDtoToResult(result, "ProjectCharges", projChgs.Count);
                            if (projChgs.Any()) {
                                sb.AppendLine("Calling oc.ProcessProjectCharges");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProjectCharges(projChgs, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProjectCharges");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 42:
                            sb.AppendLine("Calling ProcessStandardClauses");
                            var sc = new List<StandardClaus>();
                            ProcessStandardClauses(prop, ref sc);
                            AddDtoToResult(result, "StandardClauses", sc.Count);
                            if (sc.Any()) {
                                sb.AppendLine("Calling oc.ProcessStandardClauses");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessStandardClauses(sc, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessStandardClauses");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 43:
                            sb.AppendLine("Calling ProcessUsers");
                            var u = new List<User>();
                            ProcessUsers(prop, ref u);
                            AddDtoToResult(result, "Users", u.Count);
                            if (u.Any()) {
                                sb.AppendLine("Calling oc.ProcessUsers");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessUsers(u, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessUsers");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 44:
                            sb.AppendLine("Calling ProcessUserRoles");
                            var ur = new List<UserRole>();
                            ProcessUserRoles(prop, ref ur);
                            AddDtoToResult(result, "UserRoles", ur.Count);
                            if (ur.Any()) {
                                sb.AppendLine("Calling oc.ProcessUserRoles");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessUserRoles(ur, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessUserRoles");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 45:
                            sb.AppendLine("Calling ProcessCompanyUsers");
                            var cu = new List<CompanyUser>();
                            ProcessCompanyUsers(prop, ref cu);
                            AddDtoToResult(result, "CompanyUsers", cu.Count);
                            if (cu.Any()) {
                                sb.AppendLine("Calling oc.ProcessCompanyUsers");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessCompanyUsers(cu, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessCompanyUsers");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 46:
                            sb.AppendLine("Calling ProcessCompetitors");
                            var comp = new List<Competitor>();
                            ProcessCompetitors(prop, ref comp);
                            AddDtoToResult(result, "Competitors", comp.Count);
                            if (comp.Any()) {
                                sb.AppendLine("Calling oc.ProcessCompetitors");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessCompetitors(comp, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessCompetitors");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 47:
                            sb.AppendLine("Calling ProcessCompetitorPlants");
                            var cp = new List<CompetitorPlant>();
                            ProcessCompetitorPlants(prop, ref cp);
                            AddDtoToResult(result, "CompetitorPlants", cp.Count);
                            if (cp.Any()) {
                                sb.AppendLine("Calling oc.ProcessCompetitorPlants");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessCompetitorPlants(cp, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessCompetitorPlants");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 48:
                            sb.AppendLine("Calling ProcessLogins");
                            var l = new List<Login>();
                            ProcessLogins(prop, ref l);
                            AddDtoToResult(result, "Logins", l.Count);
                            if (l.Any()) {
                                sb.AppendLine("Calling oc.ProcessLogins");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessLogins(l, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessLogins");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 49:
                            sb.AppendLine("Calling ProcessManagersSalesmen");
                            var ms = new List<ManagersSalesman>();
                            ProcessManagersSalesmen(prop, ref ms);
                            AddDtoToResult(result, "ManagersSalesmen", ms.Count);
                            if (ms.Any()) {
                                sb.AppendLine("Calling oc.ProcessManagersSalesmen");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessManagersSalesmen(ms, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessManagersSalesmen");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 50:
                            sb.AppendLine("Calling ProcessProductTemplates");
                            var pte = new List<ProductTemplate>();
                            ProcessProductTemplates(prop, ref pte);
                            AddDtoToResult(result, "ProductTemplates", pte.Count);
                            if (pte.Any()) {
                                sb.AppendLine("Calling oc.ProcessProductTemplates");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProductTemplates(pte, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProductTemplates");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 51:
                            sb.AppendLine("Calling ProcessTemplatedProducts");
                            var tp = new List<TemplatedProduct>();
                            ProcessTemplatedProducts(prop, ref tp);
                            AddDtoToResult(result, "TemplatedProducts", tp.Count);
                            if (tp.Any()) {
                                sb.AppendLine("Calling oc.ProcessTemplatedProducts");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessTemplatedProducts(tp, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessTemplatedProducts");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 52:
                            sb.AppendLine("Calling ProcessProspects");
                            var prospects = new List<Prospect>();
                            ProcessProspects(prop, ref prospects);
                            AddDtoToResult(result, "Prospects", prospects.Count);
                            if (prospects.Any()) {
                                sb.AppendLine("Calling oc.ProcessProspects");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProspects(prospects, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProspects");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 53:
                            sb.AppendLine("Calling ProcessProspectNotes");
                            var pn = new List<ProspectNote>();
                            ProcessProspectNotes(prop, ref pn);
                            AddDtoToResult(result, "ProspectNotes", pn.Count);
                            if (pn.Any()) {
                                sb.AppendLine("Calling oc.ProcessProspectNotes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProspectNotes(pn, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProspectNotes");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 54:
                            sb.AppendLine("Calling ProcessSalesPersonRegions");
                            var spr = new List<SalesPersonRegion>();
                            ProcessSalesPersonRegions(prop, ref spr);
                            AddDtoToResult(result, "SalesPersonRegions", spr.Count);
                            if (spr.Any()) {
                                sb.AppendLine("Calling oc.ProcessSalesPersonRegions");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessSalesPersonRegions(spr, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessSalesPersonRegions");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 55:
                            sb.AppendLine("Calling ProcessContacts");
                            var contacts = new List<Contact>();
                            ProcessContacts(prop, ref contacts);
                            AddDtoToResult(result, "Contacts", contacts.Count);
                            if (contacts.Any()) {
                                sb.AppendLine("Calling oc.ProcessContacts");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessContacts(contacts, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessContacts");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 56:
                            sb.AppendLine("Calling ProcessContactNotes");
                            var cn = new List<ContactNote>();
                            ProcessContactNotes(prop, ref cn);
                            AddDtoToResult(result, "ContactNotes", cn.Count);
                            if (cn.Any()) {
                                sb.AppendLine("Calling oc.ProcessContactNotes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessContactNotes(cn, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessContactNotes");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 57:
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            sb.AppendLine("Calling ProcessProjects");
                            var projects = new List<Project>();
                            ProcessProjects(prop, ref projects);
                            AddDtoToResult(result, "Projects", projects.Count);
                            if (projects.Any()) {
                                sb.AppendLine("Calling oc.ProcessProjects");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProjects(projects, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProjects");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 58:
                            sb.AppendLine("Calling ProcessCustomerEvents");
                            var cevents = new List<CustomerEvent>();
                            ProcessCustomerEvents(prop, ref cevents);
                            AddDtoToResult(result, "CustomerEvents", cevents.Count);
                            if (cevents.Any()) {
                                sb.AppendLine("Calling oc.ProcessCustomerEvents");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessCustomerEvents(cevents, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessCustomerEvents");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 59:
                            sb.AppendLine("Calling ProcessProjectProducts");
                            var pp = new List<ProjectProduct>();
                            ProcessProjectProducts(prop, ref pp);
                            AddDtoToResult(result, "ProjectProducts", pp.Count);
                            if (pp.Any()) {
                                sb.AppendLine("Calling oc.ProcessProjectProducts");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProjectProducts(pp, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProjectProducts");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 60:
                            sb.AppendLine("Calling ProcessSchedules");
                            var sched = new List<Schedule>();
                            ProcessSchedules(prop, ref sched);
                            AddDtoToResult(result, "Schedules", sched.Count);
                            if (sched.Any()) {
                                sb.AppendLine("Calling oc.ProcessSchedules");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessSchedules(sched, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessSchedules");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 61:
                            sb.AppendLine("Calling ProcessScheduleItems");
                            var si = new List<ScheduleItem>();
                            ProcessScheduleItems(prop, ref si);
                            AddDtoToResult(result, "ScheduleItems", si.Count);
                            if (si.Any()) {
                                sb.AppendLine("Calling oc.ProcessScheduleItems");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessScheduleItems(si, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessScheduleItems");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 62:
                            sb.AppendLine("Calling ProcessSalespersonContacts");
                            var spc = new List<SalespersonContact>();
                            ProcessSalespersonContacts(prop, ref spc);
                            AddDtoToResult(result, "SalespersonContacts", spc.Count);
                            if (spc.Any()) {
                                sb.AppendLine("Calling oc.ProcessSalespersonContacts");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessSalespersonContacts(spc, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessSalespersonContacts");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 63:
                            sb.AppendLine("Calling ProcessProjectNotes");
                            var projNotes = new List<ProjectNote>();
                            ProcessProjectNotes(prop, ref projNotes);
                            AddDtoToResult(result, "ProjectNotes", projNotes.Count);
                            if (projNotes.Any()) {
                                sb.AppendLine("Calling oc.ProcessProjectNotes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProjectNotes(projNotes, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProjectNotes");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 64:
                            sb.AppendLine("Calling ProcessProjectBidders");
                            var pb = new List<ProjectBidder>();
                            ProcessProjectBidders(prop, ref pb);
                            AddDtoToResult(result, "ProjectBidders", pb.Count);
                            if (pb.Any()) {
                                sb.AppendLine("Calling oc.ProcessProjectBidders");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProjectBidders(pb, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProjectBidders");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 65:
                            sb.AppendLine("Calling ProcessProjectBidderProducts");
                            var pbp = new List<ProjectBidderProduct>();
                            ProcessProjectBidderProducts(prop, ref pbp);
                            AddDtoToResult(result, "ProjectBidderProducts", pbp.Count);
                            if (pbp.Any()) {
                                sb.AppendLine("Calling oc.ProcessProjectBidderProducts");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProjectBidderProducts(pbp, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProjectBidderProducts");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 66:
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            sb.AppendLine("Calling ProcessProjectBidderNotes");
                            var pbn = new List<ProjectBidderNote>();
                            ProcessProjectBidderNotes(prop, ref pbn);
                            AddDtoToResult(result, "ProjectBidderNotes", pbn.Count);
                            if (pbn.Any()) {
                                sb.AppendLine("Calling oc.ProcessProjectBidderNotes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProjectBidderNotes(pbn, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProjectBidderNotes");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 67:
                            sb.AppendLine("Calling ProcessProjectBidderCharges");
                            var pbc = new List<ProjectBidderCharge>();
                            ProcessProjectBidderCharges(prop, ref pbc);
                            AddDtoToResult(result, "ProjectBidderCharges", pbc.Count);
                            if (pbc.Any()) {
                                sb.AppendLine("Calling oc.ProcessProjectBidderCharges");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProjectBidderCharges(pbc, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProjectBidderCharges");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 68:
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            sb.AppendLine("Calling ProcessOrders");
                            var ord = new List<Order>();
                            ProcessOrders(prop, ref ord);
                            sb.AppendLine("Exited ProcessOrders");
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            AddDtoToResult(result, "Orders", ord.Count);
                            if (ord.Any()) {
                                sb.AppendLine("Calling oc.ProcessOrders");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessOrders(ord, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessOrders");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 69:
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            sb.AppendLine("Calling ProcessOrderDetails");
                            var od = new List<OrderDetail>();
                            ProcessOrderDetails(prop, ref od);
                            AddDtoToResult(result, "OrderDetails", od.Count);
                            if (od.Any()) {
                                sb.AppendLine("Calling oc.ProcessOrderDetails");
                                sb.AppendLine("Calling oc.ProcessOrders");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessOrderDetails(od, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessOrderDetails");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 70:
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            sb.AppendLine("Calling ProcessQuotes");
                            var q = new List<Quote>();
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            ProcessQuotes(prop, ref q);
                            sb.AppendLine("Exited ProcessQuotes");
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            AddDtoToResult(result, "Quotes", q.Count);
                            if (q.Any()) {
                                sb.AppendLine("Calling oc.ProcessQuotes");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessQuotes(q, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessQuotes");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 71:
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            sb.AppendLine("Calling ProcessQuoteDetails");
                            var qd = new List<QuoteDetail>();
                            ProcessQuoteDetails(prop, ref qd);
                            AddDtoToResult(result, "QuoteDetails", qd.Count);
                            if (qd.Any()) {
                                sb.AppendLine("Calling oc.ProcessQuoteDetails");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessQuoteDetails(qd, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessQuoteDetails");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 72:
                            //return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                            sb.AppendLine("Calling ProcessQuoteStandardClauses");
                            var qsc = new List<QuoteStandardClaus>();
                            ProcessQuoteStandardClauses(prop, ref qsc);
                            AddDtoToResult(result, "QuoteStandardClauses", qsc.Count);
                            if (qsc.Any()) {
                                sb.AppendLine("Calling oc.ProcessQuoteStandardClauses");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessQuoteStandardClauses(qsc, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessQuoteStandardClauses");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 73:
                            sb.AppendLine("Calling ProcessQuoteSurcharges");
                            var qsu = new List<QuoteSurcharge>();
                            ProcessQuoteSurcharges(prop, ref qsu);
                            AddDtoToResult(result, "QuoteSurcharges", qsu.Count);
                            if (qsu.Any()) {
                                sb.AppendLine("Calling oc.ProcessQuoteSurcharges");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessQuoteSurcharges(qsu, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessQuoteSurcharges");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            break;
                        case 74:
                            sb.AppendLine("Calling ProcessProjectDistances");
                            var pd = new List<ProjectDistance>();
                            ProcessProjectDistances(prop, ref pd);
                            AddDtoToResult(result, "ProjectDistances", pd.Count);
                            if (pd.Any()) {
                                sb.AppendLine("Calling oc.ProcessProjectDistances");
                                try {
                                    var oc = ioc != null ? ioc : new OracleController();
                                    oc.ProcessProjectDistances(pd, ref sb, ref result);
                                    oc.Dispose();
                                }
                                catch (Exception ex) {
                                    sb.AppendLine("An error occurred calling ProcessProjectDistances");
                                    sb.AppendLine(ex.ToString());
                                    sb.AppendLine(result.ToString());
                                    ex.Source = sb.ToString();
                                    throw ex;
                                }
                                finally {
                                    GC.Collect();
                                }
                            }
                            sb.AppendLine("Appear to have completed processing");
                            break;
                    }
                }
            }
            catch (Exception ex) {
                //var msg = e.Message;
                //throw;
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.ToString());
                var msg = sb.ToString();
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, msg);
            }
            var ret = new ReturnDto() {
                Message = sb.ToString(),
                ResultDto = result
            };
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        

        public void AppendLine(string message) {
            sb.AppendLine(message);
        }

    }    

    //internal class OracleObject {
    //    public List<DateTime> Dates { get; set; }
    //    public List<SourceSystem> SourceSystem { get; set; }
    //    public List<Role> Roles { get; set; }
    //    public List<Status> Statuses { get; set; }
    //    public List<QuoteType> QuoteTypes { get; set; }
    //    public List<QuoteStatus> QuoteStatuses { get; set; }
    //    public List<QuoteSection> QuoteSections { get; set; }
    //    public List<ProjectChargeType> ProjectChargeTypes { get; set; }
    //    public List<ProjectBidderStatus> ProjectBidderStatuses { get; set; }
    //    //public object Locations { get; set; }
    //    public List<EventStatu> EventStatus { get; set; }
    //    public List<DeviationType> DeviationTypes { get; set; }
    //    public List<Unit_Of_Measure> Unit_Of_Measure { get; set; }
    //    public List<Company> Companies { get; set; }
    //    public List<CompanyOrderStatus> CompanyOrderStatuses { get; set; }
    //    public List<CompanyProjectProductStatus> CompanyProjectProductStatuses { get; set; }
    //    public List<CompanyProjectStatus> CompanyProjectStatuses { get; set; }
    //    public List<CompanyQuoteStatus> CompanyQuoteStatuses { get; set; }
    //    public List<CreditStatus> CreditStatuses { get; set; }
    //    public List<CustomerEventType> CustomerEventTypes { get; set; }
    //    public List<CustomerType> CustomerTypes { get; set; }
    //    public List<Division> Divisions { get; set; }
    //    public List<EventFrequency> EventFrequency { get; set; }
    //    public List<LinesOfBusiness> LinesOfBusiness { get; set; }
    //    public List<LostReason> LostReasons { get; set; }
    //    public List<PaymentTerm> PaymentTerms { get; set; }
    //    public List<ProjectMapQtyRanx> ProjectMapQtyRanges { get; set; }
    //    public List<ProjectType> ProjectTypes { get; set; }
    //    public List<Source> Sources { get; set; }
    //    public List<Terms_Discount> Terms_Discount { get; set; }
    //    public List<VehicleType> VehicleTypes { get; set; }
    //    public List<ZoneCode> ZoneCodes { get; set; }
    //    public List<CompanyBidStatus> CompanyBidStatuses { get; set; }
    //    public List<Plant> Plant { get; set; }
    //    public List<PlantLinesOfBusiness> PlantLinesOfBusiness { get; set; }
    //    public List<ProductLine> ProductLines { get; set; }
    //    public List<ProductType> ProductTypes { get; set; }
    //    public List<ProductUsage> ProductUsage { get; set; }
    //    public List<Surcharge> Surcharges { get; set; }
    //    public List<Product> Product { get; set; }
    //    public List<PlantProductPrice> PlantProductPrices { get; set; }
    //    public List<ProjectCharge> ProjectCharges { get; set; }
    //    public List<StandardClaus> StandardClauses { get; set; }
    //    public List<User> Users { get; set; }
    //    public List<UserRole> UserRoles { get; set; }
    //    public List<CompanyUser> CompanyUsers { get; set; }
    //    public List<Competitor> Competitors { get; set; }
    //    public List<CompetitorPlant> CompetitorPlants { get; set; }
    //    public List<Login> Logins { get; set; }
    //    public List<ManagersSalesman> ManagersSalesmen { get; set; }
    //    public List<ProductTemplate> ProductTemplates { get; set; }
    //    public List<TemplatedProduct> TemplatedProducts { get; set; }
    //    public List<Prospect> Prospects { get; set; }
    //    public List<ProspectNote> ProspectNotes { get; set; }
    //    public List<SalesPersonRegion> SalesPersonRegions { get; set; }
    //    public List<Contact> Contact { get; set; }
    //    public List<ContactNote> ContactNotes { get; set; }
    //    public List<Project> Projects { get; set; }
    //    public List<CustomerEvent> CustomerEvents { get; set; }
    //    public List<ProjectProduct> ProjectProducts { get; set; }
    //    public List<Schedule> Schedules { get; set; }
    //    public List<ScheduleItem> ScheduleItems { get; set; }
    //    public List<SalespersonContact> SalespersonContacts { get; set; }
    //    public List<ProjectNote> ProjectNotes { get; set; }
    //    public List<ProjectBidder> ProjectBidders { get; set; }
    //    public List<ProjectBidderProduct> ProjectBidderProducts { get; set; }
    //    public List<ProjectBidderNote> ProjectBidderNotes { get; set; }
    //    public List<ProjectBidderCharge> ProjectBidderCharges { get; set; }
    //    public List<ProjectDistance> ProjectDistances { get; set; }
    //    public List<Order> Orders { get; set; }
    //    public List<OrderDetail> OrderDetails { get; set; }
    //    public List<Quote> Quotes { get; set; }
    //    public List<QuoteDetail> QuoteDetails { get; set; }
    //    public List<QuoteStandardClaus> QuoteStandardClauses { get; set; }
    //    public List<QuoteSurcharge> QuoteSurcharges { get; set; }
    //    public List<DeletedItem> DeletedItems { get; set; }
    //}
}
