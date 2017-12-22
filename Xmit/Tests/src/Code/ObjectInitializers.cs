using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests {
    internal class ObjectInitializers {

        internal object[] GetProjectChargeTypes() {
        var retVal = new List<object>();
            var obj = new {
                ChargeTypeID = 1,
                ChargeTypeName = "UnitTest ProjectChargeType.ChargeTypeName",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            obj = new {
                ChargeTypeID = 2,
                ChargeTypeName = "UnitTest ProjectChargeType.ChargeTypeName",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }
        internal object[] GetProjectBidderStatuses() {
            var retVal = new List<object>();
            var obj = new {
                ProjectBidderStatusID = 1,
                StatusName = "UnitTest ProjectBidderStatus.StatusName",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            obj = new {
                ProjectBidderStatusID = 2,
                StatusName = "UnitTest ProjectBidderStatus.StatusName",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetCustomerEvents() {
            var retVal = new List<object>();
            var obj = new {
                CustomerEventID = 1,
                Private = false,
                OwnerID = 1,
                EventTypeID = 1,
                EventStartDate = DateTime.Now,
                EventEndDate = DateTime.Now,
                CompletedDate = DateTime.Now,
                Description = "UnitTest GetCompanyOrderStatuses CustomerEvent.Description",
                ContactID = 1,
                UserID = 1,
                EventFrequencyID = 1,
                ProjectID = 1,
                MasterEventID = 1,
                EventStatusID = 1,
                Subject = "UnitTest GetCompanyOrderStatuses CustomerEvent.Subject",
                ProspectID = 1,
                RecurrenceEndDate = DateTime.Now,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            obj = new  {
                CustomerEventID = 2,
                Private = false,
                OwnerID = 1,
                EventTypeID = 1,
                EventStartDate = DateTime.Now,
                EventEndDate = DateTime.Now,
                CompletedDate = DateTime.Now,
                Description = "UnitTest GetCompanyOrderStatuses CustomerEvent.Description",
                ContactID = 1,
                UserID = 1,
                EventFrequencyID = 1,
                ProjectID = 1,
                MasterEventID = 1,
                EventStatusID = 1,
                Subject = "UnitTest GetCompanyOrderStatuses CustomerEvent.Subject",
                ProspectID = 1,
                RecurrenceEndDate = DateTime.Now,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();

        }

        internal object[] GetDeviationTypes() {
            var retVal = new List<object>();
            var obj = new {
                DeviationTypeID = 1,
                DeviationTypeName = "UnitTest DeletedItem.TableName",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            obj = new {
                DeviationTypeID = 2,
                DeviationTypeName = "UnitTest DeletedItem.TableName",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetCompanies() {
            return new List<object>() {
                new {
                    CompanyID = 1
                }
            }.ToArray();
        }

        internal object[] GetUnitsOfMeasure() {
            var retVal = new List<object>();
            var obj = new  {
                Unit_Of_Measure_ID = 1,
                CompanyID = 1,
                Imperial_Description = "UnitTest Unit_Of_Measure.Imperial_Description",
                Metric_Description = "UnitTest Unit_Of_Measure.Metric_Description",
                Imperial_To_Metric_Conv_Factor = 1,
                Conversion_Factor_Type_ID = 1,
                Conversion_Factor_Value = 1,
                ConnectionCode = "UnitTest Unit_Of_Measure.ConnectionCode",
                UOM = "UnitTest GetDeletedItems Unit_Of_Measure.UOM",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetQuoteTypes() {
            var retVal = new List<object>();
            var obj = new  {
                QuoteTypeID = 1,
                CompanyID = 1,
                QuoteTypeName = "UnitTest QuoteType.QuoteTypeName",
                Section1 = "UnitTest QuoteType.Section1",
                Section2 = "UnitTest QuoteType.Section2",
                Section3 = "UnitTest QuoteType.Section3",
                ImageLocation = "UnitTest QuoteType.ImageLocation",
                Inactive = false,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetCompanyOrderStatuses() {
            var retVal = new List<object>();
            var obj = new  {
                CompanyOrderStatusID = 1,
                CompanyOrderStatusName = "UnitTest GetCompanyOrderStatuses CompanyOrderStatus.CompanyOrderStatusName",
                CompanyID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            
            return retVal.ToArray();
        }

        internal object[] GetCompanyProjectProductStatuses() {
            var retVal = new List<object>();
            var obj = new  {
                CompanyProjectProductStatusID = 1,
                CompanyProjectProductStatusName = "UnitTest GetCompanyOrderStatuses CompanyProjectProductStatus.CompanyProjectProductStatusName",
                CompanyID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetCompanyProjectStatuses() {
            var retVal = new List<object>();
            var obj = new {
                CompanyProjectStatusID = 1,
                CompanyProjectStatusName = "UnitTest GetCompanyOrderStatuses CompanyProjectStatus.CompanyProjectStatusName",
                CompanyID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetCompanyQuoteStatuses() {
            var retVal = new List<object>();
            var obj = new  {
                CompanyQuoteStatusID = 1,
                CompanyQuoteStatusName = "UnitTest GetCompanyOrderStatuses CompanyQuoteStatus.CompanyQuoteStatusName",
                CompanyID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetCustomerEventTypes() {
            var retVal = new List<object>();
            var obj = new  {
                EventTypeID = 1,
                CompanyID = 1,
                EventType = "UnitTest GetCompanyOrderStatuses CustomerEventType.EventType",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetCreditStatuses() {
            var retVal = new List<object>();
            var obj = new {
                CreditStatusID = 1,
                CompanyID = 1,
                Code = "UnitTest CreditStatus.Code",
                Description = "UnitTest CreditStatus.Description",
                ColorCode = "UnitTest CreditStatus.ColorCode",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetCustomerTypes() {
            var retVal = new List<object>();
            var obj = new {
                CustomerTypeID = 1,
                CompanyID = 1,
                CustomerTypeName = "UnitTest GetCompanyOrderStatuses CustomerType.CustomerTypeName",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetDivisions() {
            var retVal = new List<object>();
            var obj = new {
                DivisionID = 1,
                DivisionName = "UnitTest GetDeletedItems Division.DivisionName",
                CompanyID = 1,
                Address1 = "UnitTest GetDeletedItems Division.Address1",
                Address2 = "UnitTest GetDeletedItems Division.Address2",
                City = "UnitTest GetDeletedItems Division.City",
                ST = "UnitTest GetDeletedItems Division.ST",
                Zip = "UnitTest GetDeletedItems Division.Zip",
                Country = "UnitTest GetDeletedItems Division.Country",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetEventFrequency() {
            var retVal = new List<object>();
            var obj = new {
                EventFrequencyID = 1,
                CompanyID = 1,
                EventFrequency1 = "UnitTest EventFrequency.EventFrequency1",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetLinesOfBusiness() {
            var retVal = new List<object>();
            var obj = new  {
                LineOfBusinessID = 1,
                Name = "UnitTest GetDeletedItems LinesOfBusiness.Name",
                CompanyID = 1,
                LineOfBusinessCode = "UnitTest GetDeletedItems LinesOfBusiness.LineOfBusinessCode",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetLostReasons() {
            var retVal = new List<object>();
            var obj = new {
                LostReasonID = 1,
                CompanyID = 1,
                LostReason1 = "UnitTest LostReason.LostReason1",
                Inserted = DateTime.Now,
                Modified = DateTime.Now,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetPaymentTerms() {
            var retVal = new List<object>();
            var obj = new {
                Terms_Discount_ID = 1,
                CompanyID = 1,
                Description = "UnitTest GetDeletedItems PaymentTerm.Description",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProjectTypes() {
            var retVal = new List<object>();
            var obj = new  {
                ProjectTypeID = 1,
                CompanyID = 1,
                ConnectionCode = "UnitTest GetDeletedItems ProjectType.ConnectionCode",
                SANL_Code = "UnitTest GetDeletedItems ProjectType.ConnectionCode",
                ProjectTypeName = "UnitTest GetDeletedItems ProjectType.SANL_Code",
                ShortName = "UnitTest GetDeletedItems ProjectType.ShortName",
                Inserted = DateTime.Now,
                Modified = DateTime.Now,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProjectMapQtyRanges() {
            var retVal = new List<object>();
            var obj = new {
                ProjectMapQtyRangeID = 1,
                CompanyID = 1,
                Min = 1,
                Max = 1,
                DisplayText = "UnitTest ProjectChargeType.ChargeTypeName",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetSources() {
            var retVal = new List<object>();
            var obj = new  {
                SourceID = 1,
                CompanyID = 1,
                SourceName = "UnitTest GetDeletedItems Source.SourceName",
                Inserted = DateTime.Now,
                Modified = DateTime.Now,
                Inactive = false,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetTermsDiscount() {
            var retVal = new List<object>();
            var obj = new {
                Terms_Discount_ID = 1,
                CompanyID = 1,
                Discount_Days = 1,
                Discount_Amount = 1,
                Net_Days = 1,
                Description = "UnitTest GetDeletedItems Terms_Discount.Description",
                Discount_Taxes = true,
                Percent_Flag = true,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetVehicleTypes() {
            var retVal = new List<object>();
            var obj = new {
                VehicleTypeID = 1,
                CompanyID = 1,
                Description = "UnitTest GetDeletedItems VehicleType.Description",
                Abbreviation = "UnitTest GetDeletedItems VehicleType.Abbreviation",
                LocalVehicleTypeId = "UnitTest GetDeletedItems VehicleType.LocalVehicleTypeId",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetZoneCodes() {
            var retVal = new List<object>();
            var obj = new  {
                ZoneCodeID = 1,
                CompanyID = 1,
                ZoneCodeName = "UnitTest GetDeletedItems ZoneCode.ZoneCodeName",
                SystemCode = "UnitTest GetDeletedItems ZoneCode.SystemCode",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetCompanyBidStatuses() {
            var retVal = new List<object>();
            var obj = new {
                CompanyBidStatusID = 1,
                CompanyBidStatusName = "UnitTest GetDeletedItems CompanyBidStatus.CompanyBidStatusName",
                CompanyID = 1,
                SystemCode = "UnitTest GetDeletedItems CompanyBidStatus.SystemCode",
                MSStatusID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetPlants() {
            var retVal = new List<object>();
            var obj = new {
                PlantID = 1,
                Description = "UnitTest GetDeletedItems Plant.Description",
                SystechCompNbr = "UnitTest GetDeletedItems Plant.SystechCompNbr",
                CSPlantCode = "UnitTest GetDeletedItems Plant.CSPlantCode",
                CSShortName = "UnitTest GetDeletedItems Plant.CSShortName",
                CSPlantName = "UnitTest GetDeletedItems Plant.CSPlantName",
                Inserted = DateTime.Now,
                Modified = DateTime.Now,
                Inactive_Flag = false,
                DivisionID = 1,
                LineOfBusinessID = 1,
                PlantCost = 1,
                DeliveryCost = 1,
                MaterialCost = 1,
                SGACost = 1,
                Address1 = "UnitTest GetDeletedItems Plant.Address1",
                Address2 = "UnitTest GetDeletedItems Plant.Address2",
                City = "UnitTest GetDeletedItems Plant.City",
                State = "UnitTest GetDeletedItems Plant.State",
                Zip = "UnitTest GetDeletedItems Plant.Zip",
                Longitude = 0,
                Latitude = 0,
                Country = "UnitTest GetDeletedItems Plant.Country",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetPlantLinesOfBusiness() {
            var retVal = new List<object>();
            var obj = new  {
                PlantLineOfBusinessID = 1,
                LineOfBusinessID = 1,
                PlantID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProductLines() {
            var retVal = new List<object>();
            var obj = new {
                ProductLineID = 1,
                CompanyID = 1,
                ProductLineCode = "UnitTest GetDeletedItems ProductLine.ProductLineCode",
                ProductLineName = "UnitTest GetDeletedItems ProductLine.ProductLineName",
                Active = true,
                extProductLineCode = "UnitTest GetDeletedItems ProductLine.extProductLineCode",
                LineOfBusinessID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProductUsage() {
            var retVal = new List<object>();
            var obj = new {
                ProductUsageID = 1,
                CompanyID = 1,
                ProductUsageName = "UnitTest GetDeletedItems ProductUsage.ProductUsageName",
                ProductLineID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProductTypes() {
            var retVal = new List<object>();
            var obj = new  {
                ProductTypeID = 1,
                CompanyID = 1,
                ProductTypeName = "UnitTest GetDeletedItems ProductType.ProductTypeName",
                ProductTypeCode = "UnitTest GetDeletedItems ProductType.ProductTypeCode",
                ProductLineID = 1,
                ListPrice = 1,
                UOMID = 1,
                Inactive = false,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetSurcharges() {
            var retVal = new List<object>();
            var obj = new {
                SurchargeID = 1,
                CompanyID = 1,
                SurchargeName = "UnitTest GetDeletedItems Surcharge.SurchargeName",
                Rate = 1,
                UOMID = 1,
                Active = true,
                QuoteDefault = true,
                QuoteSelectable = true,
                PlantID = 1,
                LocalSurchargeID = "UnitTest GetDeletedItems Surcharge.LocalSurchargeID",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProducts() {
            var retVal = new List<object>();
            var obj = new {
                ProductID = 1,
                CompanyID = 1,
                ProductName = "UnitTest GetDeletedItems Product.extProductLineCode",
                Active = true,
                AU_ID = 1,
                UnitOfMeasureID = 1,
                ProductTypeID = 1,
                ConnectionCode = "UnitTest GetDeletedItems Product.ConnectionCode",
                ITEM_CAT = "UnitTest GetDeletedItems Product.ITEM_CAT",
                Product_Code = "UnitTest GetDeletedItems Product.Product_Code",
                Inserted = DateTime.Now,
                Modified = DateTime.Now,
                extProduct_Code = "UnitTest GetDeletedItems Product.extProduct_Code",
                IsSellable = true,
                MsModificationDate = DateTime.Now,
                ProductShortName = "UnitTest GetDeletedItems Product.ProductShortName"
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetPlantProductPrices() {
            var retVal = new List<object>();
            var obj = new  {
                PlantProductPriceID = 1,
                PlantID = 1,
                ProductID = 1,
                ListPrice = 1,
                Description = "UnitTest GetDeletedItems PlantProductPrice.Description",
                PriceListID = 1,
                MsModificationDate = DateTime.Now,
                PriceExtensionCode = "UnitTest GetDeletedItems PlantProductPrice.PriceExtensionCode"
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProjectCharges() {
            var retVal = new List<object>();
            var obj = new  {
                ChargeID = 1,
                ChargeName = "UnitTest GetDeletedItems ProjectCharge.ChargeName",
                ChargeType = 1,
                ChargeCode = "UnitTest GetDeletedItems ProjectCharge.ChargeName",
                CompanyID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetStandardClauses() {
            var retVal = new List<object>();
            var obj = new {
                StandardClauseID = 1,
                Name = "UnitTest GetDeletedItems StandardClaus.Name",
                StandardClauseText = "UnitTest GetDeletedItems StandardClaus.StandardClauseText",
                ProductLineID = 1,
                PlantID = 1,
                ProductID = 1,
                Inactive = false,
                CompanyID = 1,
                ProductUsageID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetUsers() {
            var retVal = new List<object>();
            var obj = new {
                UserID = 1,
                CompanyID = 1,
                UserName = "UnitTest GetDeletedItems User.UserName",
                FirstName = "UnitTest GetDeletedItems User.FirstName",
                LastName = "UnitTest GetDeletedItems User.LastName",
                Active = true,
                InsertDate = DateTime.Now,
                EmailAddress = "UnitTest GetDeletedItems User.EmailAddress",
                Password = new byte[] { new byte()},
                CellPhoneNumber = "UnitTest GetDeletedItems User.CellPhoneNumber",
                PhoneNumber = "UnitTest GetDeletedItems User.PhoneNumber",
                FaxNumber = "UnitTest GetDeletedItems User.FaxNumber",
                SalesmanCode = "UnitTest GetDeletedItems User.SalesmanCode",
                ReportsToID = 1,
                DeviationTypeID = 1,
                DeviationAmount = 1,
                Address1 = "UnitTest GetDeletedItems User.Address1",
                Address2 = "UnitTest GetDeletedItems User.Address2",
                City = "UnitTest GetDeletedItems User.City",
                ST = "UnitTest GetDeletedItems User.ST",
                Zip = "UnitTest GetDeletedItems User.Zip",
                Latitude = 1,
                Longitude = 1,
                UsesSAML = false,
                Country = "UnitTest GetDeletedItems User.Country",
                DefaultMailMessage = "UnitTest GetDeletedItems User.DefaultMailMessage",
                MsModificationDate = DateTime.Now,
                AdministratorID = 1
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetUserRoles() {
            var retVal = new List<object>();
            var obj = new {
                UserRoleID = 1,
                RoleID = 1,
                UserID = 1,
                MsModificationDate = DateTime.Now
            };
            return retVal.ToArray();
        }

        internal object[] GetCompanyUsers() {
            var retVal = new List<object>();
            var obj = new {
                CompanyUserID = 1,
                CompanyID = 1,
                UserID = 1,
                SalesmanCode = "UnitTest GetCompanyOrderStatuses CompanyUser.SalesmanCode",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetCompetitors() {
            var retVal = new List<object>();
            var obj = new {
                CompetitorID = 1,
                CompanyID = 1,
                CompetitorName = "UnitTest GetCompanyOrderStatuses Competitor.CompetitorName",
                Inserted = DateTime.Now,
                Modified = DateTime.Now,
                Inactive = false,
                UserID = 1,
                Address1 = "UnitTest GetCompanyOrderStatuses Competitor.Address1",
                Address2 = "UnitTest GetCompanyOrderStatuses Competitor.Address2",
                City = "UnitTest GetCompanyOrderStatuses Competitor.City",
                State = "UnitTest GetCompanyOrderStatuses Competitor.State",
                Zip = "UnitTest GetCompanyOrderStatuses Competitor.Zip",
                Longitude = 0,
                Latitude = 0,
                Country = "UnitTest GetCompanyOrderStatuses Competitor.Country",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetCompetitorPlants() {
            var retVal = new List<object>();
            var obj = new {
                CompetitorPlantID = 1,
                CompetitorID = 1,
                PlantName = "UnitTest GetCompanyOrderStatuses CompetitorPlant.PlantName",
                Inserted = DateTime.Now,
                Modified = DateTime.Now,
                Inactive_Flag = false,
                PlantCost = 0,
                DeliveryCost = 0,
                MaterialCost = 0,
                SGACost = 0,
                Address1 = "UnitTest GetCompanyOrderStatuses CompetitorPlant.Address1",
                Address2 = "UnitTest GetCompanyOrderStatuses CompetitorPlant.Address2",
                City = "UnitTest GetCompanyOrderStatuses CompetitorPlant.City",
                State = "UnitTest GetCompanyOrderStatuses CompetitorPlant.State",
                Zip = "UnitTest GetCompanyOrderStatuses CompetitorPlant.Zip",
                Longitude = 0,
                Latitude = 0,
                LineOfBusinessID = 1,
                DivisionID = 1,
                Country = "UnitTest GetCompanyOrderStatuses CompetitorPlant.Country",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetLogins() {
            var retVal = new List<object>();
            var obj = new {
                LoginID = 1,
                UserID = 1,
                LoginDate = DateTime.Now,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetManagersSalesmen() {
            var retVal = new List<object>();
            var obj = new {
                ManagerUserID = 1,
                ManagerID = 1,
                SalesmanID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProductTemplates() {
            var retVal = new List<object>();
            var obj = new {
                ProductTemplateID = 1,
                TemplateName = "UnitTest GetDeletedItems ProductTemplate.TemplateName",
                CompanyUserID = 1,
                Inactive = false,
                MsModificationDate = DateTime.Now,
                Shared = true
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetTemplatedProducts() {
            var retVal = new List<object>();
            var obj = new {
                TemplatedProductID = 1,
                ProductTemplateID = 1,
                ProductTypeID = 1,
                ProductID = 1,
                PlantID = 1,
                ProductUsageID = 1,
                LineOfBusinessID = 1,
                ProductLineID = 1,
                Qty = 1,
                Price = 1,
                Freight = 1,
                FreightPay = 1,
                Description = "UnitTest GetDeletedItems TemplatedProduct.Description",
                IncludeInForecast = true,
                Note = "UnitTest GetDeletedItems TemplatedProduct.Note",
                Category = "UnitTest GetDeletedItems TemplatedProduct.Category",
                MsModificationDate = DateTime.Now,
                UOMID = 1,
                PriceExtensionCode = "UnitTest GetDeletedItems TemplatedProduct.PriceExtensionCode",
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProspects() {
            var retVal = new List<object>();
            var obj = new {
                ProspectID = 1,
                Private = false,
                OwnerID = 1,
                CompanyID = 1,
                ProspectName = "UnitTest GetDeletedItems Prospect.ProspectName",
                Inserted = DateTime.Now,
                Modified = DateTime.Now,
                ProspectCode = "UnitTest GetDeletedItems Prospect.ProspectCode",
                ConnectionCode = "UnitTest GetDeletedItems Prospect.ConnectionCode",
                LocalCustNumber = "UnitTest GetDeletedItems Prospect.LocalCustNumber",
                LocalCustName = "UnitTest GetDeletedItems Prospect.LocalCustName",
                IsCustomer = true,
                Credit_Status_ID = 1,
                CustomerStatus = "UnitTest GetDeletedItems Prospect.CustomerStatus",
                Sales = 1,
                Margin = 1,
                MaterialMargin = 1,
                JobCount = 1,
                Address1 = "UnitTest GetDeletedItems Prospect.Address1",
                Address2 = "UnitTest GetDeletedItems Prospect.Address2",
                City = "UnitTest GetDeletedItems Prospect.City",
                State = "UnitTest GetDeletedItems Prospect.State",
                Zip = "UnitTest GetDeletedItems Prospect.Zip",
                Phone = "UnitTest GetDeletedItems Prospect.Phone",
                HighestBalance = 1,
                AvgInvoiceAmt = 1,
                AvgDaysToPay = 1,
                AvgDaysPastDue = 1,
                TotalDue = 1,
                ZeroTo30Days = 1,
                ThirtyOneTo60Days = 1,
                SixtyOneTo90Days = 1,
                NinetyOneTo120Days = 1,
                Over120Days = 1,
                CustomerTypeID = 1,
                StatementFrequency = "UnitTest GetDeletedItems Prospect.StatementFrequency",
                FinanceRate = 1,
                PaymentTermsID = 1,
                SalesmanCode = "UnitTest GetDeletedItems Prospect.SalesmanCode",
                Fax = "UnitTest GetDeletedItems Prospect.Fax",
                WebSite = "UnitTest GetDeletedItems Prospect.WebSite",
                AvgRevenue = 1,
                NumberofEmployees = "UnitTest GetDeletedItems Prospect.NumberofEmployees",
                CompanyOriginDate = "UnitTest GetDeletedItems Prospect.CompanyOriginDate",
                BusinessTypeID = 1,
                Notes = "UnitTest GetDeletedItems Prospect.Notes",
                Country = "UnitTest GetDeletedItems Prospect.Country",
                PORequired = false,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProspectNotes() {
            var retVal = new List<object>();
            var obj = new {
                ProspectNoteID = 1,
                ProspectID = 1,
                Note = "UnitTest GetDeletedItems ProspectNote.Note",
                Inserted = DateTime.Now,
                CreatedBy = 1,
                Subject = "UnitTest GetDeletedItems ProspectNote.Subject",
                Source = "UnitTest GetDeletedItems ProspectNote.Source",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetSalesPersonRegions() {
            var retVal = new List<object>();
            var obj = new {
                SalesPersonRegionID = 1,
                SalesPersonID = 1,
                PlantID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetContacts() {
            var retVal = new List<object>();
            var obj = new {
                ContactID = 1,
                Private = false,
                OwnerID = 1,
                FirstName = "UnitTest GetCompanyOrderStatuses Contact.FirstName",
                LastName = "UnitTest GetCompanyOrderStatuses Contact.LastName",
                Name = "UnitTest GetCompanyOrderStatuses Contact.Name",
                ProspectID = 1,
                Email = "UnitTest GetCompanyOrderStatuses Contact.Email",
                MobilePhone = "UnitTest GetCompanyOrderStatuses Contact.MobilePhone",
                Phone = "UnitTest GetCompanyOrderStatuses Contact.Phone",
                Fax = "UnitTest GetCompanyOrderStatuses Contact.Fax",
                Address = "UnitTest GetCompanyOrderStatuses Contact.Address",
                FullName = "UnitTest GetCompanyOrderStatuses Contact.FullName",
                Title = "UnitTest GetCompanyOrderStatuses Contact.Title",
                StartingYearatCompany = "UnitTest GetCompanyOrderStatuses Contact.StartingYearatCompany",
                Department = "UnitTest GetCompanyOrderStatuses Contact.Department",
                Address2 = "UnitTest GetCompanyOrderStatuses Contact.Address2",
                City = "UnitTest GetCompanyOrderStatuses Contact.City",
                State = "UnitTest GetCompanyOrderStatuses Contact.State",
                Zip = "UnitTest GetCompanyOrderStatuses Contact.Zip",
                Salutation = "UnitTest GetCompanyOrderStatuses Contact.Salutation",
                Notes = "UnitTest GetCompanyOrderStatuses Contact.Notes",
                Inserted = DateTime.Now,
                Modified = DateTime.Now,
                Country = "UnitTest GetCompanyOrderStatuses Contact.Country",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetContactNotes() {
            var retVal = new List<object>();
            var obj = new {
                ContactNoteID = 1,
                ContactID = 1,
                Note = "UnitTest GetCompanyOrderStatuses ContactNote.Note",
                Inserted = DateTime.Now,
                CreatedBy = 1,
                Subject = "UnitTest GetCompanyOrderStatuses ContactNote.Subject",
                Source = "UnitTest GetCompanyOrderStatuses ContactNote.Source",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProjects() {
            var retVal = new List<object>();
            var obj = new {
                ProjectID = 1,
                Private = false,
                OwnerID = 1,
                CompanyID = 1,
                ProjectName = "UnitTest GetDeletedItems Project.ProjectName",
                ProjectTypeID = 1,
                StatusID = 1,
                StatusChangeDate = DateTime.Now,
                SourceID = 1,
                Active = true,
                Address1 = "UnitTest GetDeletedItems Project.Address1",
                Address2 = "UnitTest GetDeletedItems Project.Address2",
                City = "UnitTest GetDeletedItems Project.City",
                County = "UnitTest GetDeletedItems Project.County",
                State = "UnitTest GetDeletedItems Project.State",
                ZipCode = "UnitTest GetDeletedItems Project.ZipCode",
                PlantID = 1,
                SalespersonID = 1,
                BidDate = DateTime.Now,
                LostReasonID = 1,
                LostToID = 1,
                LostNote = "UnitTest GetDeletedItems Project.ConnectionCode",
                Inserted = DateTime.Now,
                Modified = DateTime.Now,
                PrevStatus = 1,
                Exported = false,
                WinningProjectBidderID = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                TotalYards = 1,
                ConnectionCode = "UnitTest GetDeletedItems Project.ConnectionCode",
                CustCode = "UnitTest GetDeletedItems Project.CustCode",
                ProjectCode = "UnitTest GetDeletedItems Project.ProjectCode",
                Longitude = 1,
                Latitude = 1,
                GeofenceID = 1,
                Country = "UnitTest GetDeletedItems Project.Country",
                ProjectDescription = "UnitTest GetDeletedItems Project.ProjectDescription",
                ZoneCodeID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetEventStatuses() {
            var retVal = new List<object>();
            var obj = new {
                EventStatusID = 1,
                EventStatus = "UnitTest GetDeletedItems EventStatu.EventStatus",
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProjectProducts() {
            var retVal = new List<object>();
            var obj = new {
                ProjectProductID = 1,
                ProjectID = 1,
                Qty = 1,
                UOMID = 1,
                Inserted = DateTime.Now,
                Modified = DateTime.Now,
                Price = 1,
                ProductLineID = 1,
                ProductTypeID = 1,
                ProductID = 1,
                PsiID = 1,
                ProductUsageID = 1,
                Note = "UnitTest GetDeletedItems ProjectProduct.Note",
                Forecasted = true,
                PlantID = 1,
                Freight = 1,
                FreightPay = 1,
                CompetitorPrice = 1,
                Category = "UnitTest GetDeletedItems ProjectProduct.Category",
                Description = "UnitTest GetDeletedItems ProjectProduct.Description",
                LineOfBusinessID = 1,
                StatusID = 1,
                MsModificationDate = DateTime.Now,
                PriceExtensionCode = "UnitTest GetDeletedItems ProjectProduct.PriceExtensionCode",
                LostReasonID = 1,
                LostToID = 1,
                LostNote = "UnitTest GetDeletedItems ProjectProduct.LostNote",
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetSchedules() {
            var retVal = new List<object>();
            var obj = new {
                ScheduleID = 1,
                ProjectProductID = 1,
                Active = true,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetScheduleItems() {
            var retVal = new List<object>();
            var obj = new {
                ScheduleItemID = 1,
                ScheduleID = 1,
                Quantity = 1,
                Month = DateTime.Now,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetSalespersonContacts() {
            var retVal = new List<object>();
            var obj = new {
                SalesPersonContactID = 1,
                SalesPersonID = 1,
                ContactID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProjectNotes() {
            var retVal = new List<object>();
            var obj = new {
                NoteID = 1,
                ProjectID = 1,
                Note = "UnitTest GetDeletedItems ProjectNote.Note",
                Inserted = DateTime.Now,
                CreatedBy = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProjectBidders() {
            var retVal = new List<object>();
            var obj = new {
                ProjectBidderID = 1,
                ProjectID = 1,
                ProspectID = 1,
                Quoted = false,
                StatusSub = true,
                QuoteDate = DateTime.Now,
                NotLIkeLIst = true,
                StatusID = 1,
                PlantID = 1,
                ProjectCode = "UnitTest GetDeletedItems ProjectBidder.ProjectCode",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                PONumber = "UnitTest GetDeletedItems ProjectBidder.PONumber",
                MinLoadCharge = 1,
                SundryCharge = 1,
                SeasonalCharge = 1,
                UnloadCharge = 1,
                CustJobNbr = "UnitTest GetDeletedItems ProjectBidder.CustJobNbr",
                MsModificationDate = DateTime.Now,
                EffectiveDate = DateTime.Now
            };
            return retVal.ToArray();
        }

        internal object[] GetProjectBidderProducts() {
            var retVal = new List<object>();
            var obj = new {
                ProjectBidderProductID = 1,
                ProjectBidderID = 1,
                ProjectProductID = 1,
                Price = 1,
                Inserted = DateTime.Now,
                Modified = DateTime.Now,
                SameAsList = true,
                Forecasted = true,
                Freight = 1,
                FreightPay = 1,
                ListPrice = 1,
                CustomerPrice = 1,
                Exported = false,
                Quantity = 1,
                StatusID = 1,
                LineNbr = 1,
                MsModificationDate = DateTime.Now,
                PreviousPrice = 1,
                PreviousDeliveredPrice = 1
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProjectBidderNotes() {
            var retVal = new List<object>();
            var obj = new {
                NoteID = 1,
                ProjectBidderID = 1,
                Note = "UnitTest GetDeletedItems ProjectBidderNote.Note",
                Inserted = DateTime.Now,
                CreatedBy = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProjectBidderCharges() {
            var retVal = new List<object>();
            var obj = new {
                ProjectBidderChargeID = 1,
                ProjectBidderID = 1,
                ProductLineID = 1,
                ProjectChargeID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetOrders() {
            var retVal = new List<object>();
            var obj = new {
                OrderID = 1,
                CompanyID = 1,
                ProjectID = 1,
                year = DateTime.Now.Year,
                month = DateTime.Now.Month,
                YardsActual = 1,
                MaterialCost = 1,
                DollarsActual = 1,
                OrderDate = DateTime.Now,
                ShipDate = DateTime.Now,
                YardsTicketed = 1,
                OnJobTime = DateTime.Now,
                LocationID = 1,
                ProspectID = 1,
                DispatchOrderID = 1,
                DispatchOrderNumber = "UnitTest GetDeletedItems Order.DispatchOrderNumber",
                SalesmanCode = "UnitTest GetDeletedItems Order.SalesmanCode",
                Inserted = DateTime.Now,
                Modified = DateTime.Now,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetOrderDetails() {
            var retVal = new List<object>();
            var obj = new {
                OrderDetailID = 1,
                OrderID = 1,
                ProductID = 1,
                Quantity = 1,
                UnitPrice = 1,
                Mix = true,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetQuotes() {
            var retVal = new List<object>();
            var obj = new {
                QuoteID = 1,
                ProjectBidderID = 1,
                ContactID = 1,
                TermsID = 1,
                Attention = "UnitTest GetDeletedItems Quote.Attention",
                Phone = "UnitTest GetDeletedItems Quote.Phone",
                CellPhone = "UnitTest GetDeletedItems Quote.CellPhone",
                Fax = "UnitTest GetDeletedItems Quote.Fax",
                Email = "UnitTest GetDeletedItems Quote.Email",
                QuoteDate = DateTime.Now,
                FirmUntilDate = DateTime.Now,
                QuoteSequenceNumber = 1,
                QuoteTypeID = 1,
                DateCreated = DateTime.Now,
                DateLastModified = DateTime.Now,
                CustomerQuoteID = "UnitTest GetDeletedItems Quote.CustomerQuoteID",
                Exported = true,
                EscalationDate = DateTime.Now,
                QuoteNote = "UnitTest GetDeletedItems Quote.QuoteNote",
                EscalationAmount = "UnitTest GetDeletedItems Quote.EscalationAmount",
                QuoteStatusID = 1,
                Notes = "UnitTest GetDeletedItems Quote.Notes",
                CreatedByID = 1,
                OrderDate = DateTime.Now,
                StartOrderDate = DateTime.Now,
                ExpireOrderDate = DateTime.Now,
                VehicleTypeID_1 = 1,
                VehicleTypeID_2 = 1,
                VehicleTypeID_3 = 1,
                VehicleTypeID_4 = 1,
                VehicleTypeID_5 = 1,
                CustomerOrderID = "UnitTest GetDeletedItems Quote.CustomerOrderID",
                PurchaseOrder = "UnitTest GetDeletedItems Quote.PurchaseOrder",
                BeginDate = DateTime.Now,
                AllowBeforeStart = true,
                AllowAfterEnd = true,
                AllowExceedQty = true,
                AllowExceedLoads = true,
                AllowAfterComplete = true,
                MsModificationDate = DateTime.Now,
                SalespersonID = 1,
                ApexNoteID = 1
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetQuoteDetails() {
            var retVal = new List<object>();
            var obj = new {
                QuoteDetailID = 1,
                QuoteID = 1,
                ProjectProductID = 1,
                Price = 1,
                Qty = 1,
                Freight = 1,
                FreightPay = 1,
                ListPrice = 1,
                CustomerPrice = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetQuoteStandardClauses() {
            var retVal = new List<object>();
            var obj = new {
                QuoteStandardClauseID = 1,
                QuoteID = 1,
                StandardClauseID = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetQuoteSurcharges() {
            var retVal = new List<object>();
            var obj = new {
                QuoteSurchargeID = 1,
                QuoteID = 1,
                SurchargeID = 1,
                Rate = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }

        internal object[] GetProjectDistances() {
            var retVal = new List<object>();
            var obj = new {
                ProjectDistanceID = 1,
                ProjectID = 1,
                PlantID = 1,
                Miles = 1,
                Kilometers = 1,
                MsModificationDate = DateTime.Now
            };
            retVal.Add(obj);
            return retVal.ToArray();
        }
    }
}
