using System;
using System.Collections.Generic;

namespace Tests {
    public static class DataTestMock {
        internal static object GetData(){
            var oi = new ObjectInitializers();
            return new OracleObject() {
                Dates = new object[1]{
                    new {
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        Key = "abc123"
                    }
                },
                DeletedItems = new object[1] {
                    new {
                        KeyID = 1,
                        TableName = "UnitTest DeletedItems"
                    }
                },
                SourceSystem = new object[1] {
                    new {
                        SourceSystemID = 1, 
                        SystemName = "UnitTest SourceSystem",
                        MsModificationDate = DateTime.Now
                    }
                },
                Roles = new object[1] {
                    new {
                        RoleID = 1,
                        RoleDescription = "UnitTest Roles",
                        MsModificationDate = DateTime.Now
                    }
                },
                Statuses = new object[1] {
                    new {
                        StatusID = 1,
                        StatusName = "UnitTest Statuses",
                        Inserted = DateTime.Now,
                        Modified = DateTime.Now,
                        MsModificationDate = DateTime.Now
                    }
                },
                QuoteStatuses = new object[1] {
                    new {
                        QuoteStatusID = 1,
                        QuoteStatusName = "UnitTest QuoteStatuses",
                        SortOrder = 1,
                        Inserted = DateTime.Now,
                        Modified = DateTime.Now,
                        MsModificationDate = DateTime.Now
                    }
                },
                QuoteSections = new object[1] {
                    new {
                        QuoteSectionID = 1,
                        QuoteTypeID = 1,
                        SectionContent = new byte[1]{new byte()},
                        PositionTop = 1,
                        PositionLeft = 1,
                        Height = 1,
                        Width = 1,
                        IsProducts = true,
                        IsSurcharges = true,
                        IsLogo = true,
                        IsCompanyInfo = true,
                        IsCustomerInfo = true,
                        IsSignature = true,
                        IsExpiration = true,
                        IsQuoteNumber = true,
                        ProductColumns = "1",
                        MsModificationDate = DateTime.Now
                    }
                },
                ProjectChargeTypes = oi.GetProjectChargeTypes(),
                ProjectBidderStatuses = oi.GetProjectBidderStatuses(),
                EventStatus = oi.GetEventStatuses(),
                DeviationTypes = oi.GetDeviationTypes(),
                Companies = oi.GetCompanies(),
                Unit_Of_Measure = oi.GetUnitsOfMeasure(),
                QuoteTypes = oi.GetQuoteTypes(),
                CompanyOrderStatuses = oi.GetCompanyOrderStatuses(),
                CompanyProjectProductStatuses = oi.GetCompanyProjectProductStatuses(),
                CompanyProjectStatuses = oi.GetCompanyProjectStatuses(),
                CompanyQuoteStatuses = oi.GetCompanyQuoteStatuses(),
                CreditStatuses = oi.GetCreditStatuses(),
                CustomerEventTypes = oi.GetCustomerEventTypes(),
                CustomerTypes = oi.GetCustomerTypes(),
                Divisions = oi.GetDivisions(),
                EventFrequency = oi.GetEventFrequency(),
                LinesOfBusiness = oi.GetLinesOfBusiness(),
                LostReasons = oi.GetLostReasons(),
                PaymentTerms = oi.GetPaymentTerms(),
                ProjectMapQtyRanges = oi.GetProjectMapQtyRanges(),
                ProjectTypes = oi.GetProjectTypes(),
                Sources = oi.GetSources(),
                Terms_Discount = oi.GetTermsDiscount(),
                VehicleTypes = oi.GetVehicleTypes(),
                ZoneCodes = oi.GetZoneCodes(),
                CompanyBidStatuses = oi.GetCompanyBidStatuses(),
                Plant = oi.GetPlants(),
                PlantLinesOfBusiness = oi.GetPlantLinesOfBusiness(),
                ProductLines = oi.GetProductLines(),
                ProductTypes = oi.GetProductTypes(),
                ProductUsage = oi.GetProductUsage(),
                Surcharges = oi.GetSurcharges(),
                Product = oi.GetProducts(),
                PlantProductPrices = oi.GetPlantProductPrices(),
                ProjectCharges = oi.GetProjectCharges(),
                StandardClauses = oi.GetStandardClauses(),
                Users = oi.GetUsers(),
                UserRoles = oi.GetUserRoles(),
                CompanyUsers = oi.GetCompanyUsers(),
                Competitors = oi.GetCompetitors(),
                CompetitorPlants = oi.GetCompetitorPlants(),
                Logins = oi.GetLogins(),
                ManagersSalesmen = oi.GetManagersSalesmen(),
                ProductTemplates = oi.GetProductTemplates(),
                TemplatedProducts = oi.GetTemplatedProducts(),
                Prospects = oi.GetProspects(),
                ProspectNotes = oi.GetProspectNotes(),
                SalesPersonRegions = oi.GetSalesPersonRegions(),
                Contact = oi.GetContacts(),
                ContactNotes = oi.GetContactNotes(),
                Projects = oi.GetProjects(),
                CustomerEvents = oi.GetCustomerEvents(),
                ProjectProducts = oi.GetProjectProducts(),
                Schedules = oi.GetSchedules(),
                ScheduleItems = oi.GetScheduleItems(),
                SalespersonContacts = oi.GetSalespersonContacts(),
                ProjectNotes = oi.GetProjectNotes(),
                ProjectBidders = oi.GetProjectBidders(),
                ProjectBidderProducts = oi.GetProjectBidderProducts(),
                ProjectBidderNotes = oi.GetProjectBidderNotes(),
                ProjectBidderCharges = oi.GetProjectBidderCharges(),
                Orders = oi.GetOrders(),
                OrderDetails = oi.GetOrderDetails(),
                Quotes = oi.GetQuotes(),
                QuoteDetails = oi.GetQuoteDetails(),
                QuoteStandardClauses = oi.GetQuoteStandardClauses(),
                QuoteSurcharges = oi.GetQuoteSurcharges(),
                ProjectDistances = oi.GetProjectDistances()
            };
        }
    }

    struct OracleObject {
        public object[] Dates { get; set; }
        public object[] DeletedItems { get; set; }
        public object[] SourceSystem { get; set; }
        public object[] Roles { get; set; }
        public object[] Statuses { get; set; }
        public object[] QuoteStatuses { get; set; }
        public object[] QuoteSections { get; set; }
        public object[] ProjectChargeTypes { get; set; }
        public object[] ProjectBidderStatuses { get; set; }
        public object[] EventStatus { get; set; }
        public object[] DeviationTypes { get; set; }
        public object[] Companies { get; set; }
        public object[] Unit_Of_Measure { get; set; }
        public object[] QuoteTypes { get; set; }
        public object[] CompanyOrderStatuses { get; set; }
        public object[] CompanyProjectProductStatuses { get; set; }
        public object[] CompanyProjectStatuses { get; set; }
        public object[] CompanyQuoteStatuses { get; set; }
        public object[] CreditStatuses { get; set; }
        public object[] CustomerEventTypes { get; set; }
        public object[] CustomerTypes { get; set; }
        public object[] Divisions { get; set; }
        public object[] EventFrequency { get; set; }
        public object[] LinesOfBusiness { get; set; }
        public object[] LostReasons { get; set; }
        public object[] PaymentTerms { get; set; }
        public object[] ProjectMapQtyRanges { get; set; }
        public object[] ProjectTypes { get; set; }
        public object[] Sources { get; set; }
        public object[] Terms_Discount { get; set; }
        public object[] VehicleTypes { get; set; }
        public object[] ZoneCodes { get; set; }
        public object[] CompanyBidStatuses { get; set; }
        public object[] Plant { get; set; }
        public object[] PlantLinesOfBusiness { get; set; }
        public object[] ProductLines { get; set; }
        public object[] ProductTypes { get; set; }
        public object[] ProductUsage { get; set; }
        public object[] Surcharges { get; set; }
        public object[] Product { get; set; }
        public object[] PlantProductPrices { get; set; }
        public object[] ProjectCharges { get; set; }
        public object[] StandardClauses { get; set; }
        public object[] Users { get; set; }
        public object[] UserRoles { get; set; }
        public object[] CompanyUsers { get; set; }
        public object[] Competitors { get; set; }
        public object[] CompetitorPlants { get; set; }
        public object[] Logins { get; set; }
        public object[] ManagersSalesmen { get; set; }
        public object[] ProductTemplates { get; set; }
        public object[] TemplatedProducts { get; set; }
        public object[] Prospects { get; set; }
        public object[] ProspectNotes { get; set; }
        public object[] SalesPersonRegions { get; set; }
        public object[] Contact { get; set; }
        public object[] ContactNotes { get; set; }
        public object[] Projects { get; set; }
        public object[] CustomerEvents { get; set; }
        public object[] ProjectProducts { get; set; }
        public object[] Schedules { get; set; }
        public object[] ScheduleItems { get; set; }
        public object[] SalespersonContacts { get; set; }
        public object[] ProjectNotes { get; set; }
        public object[] ProjectBidders { get; set; }
        public object[] ProjectBidderProducts { get; set; }
        public object[] ProjectBidderNotes { get; set; }
        public object[] ProjectBidderCharges { get; set; }
        public object[] Orders { get; set; }
        public object[] OrderDetails { get; set; }
        public object[] Quotes { get; set; }
        public object[] QuoteDetails { get; set; }
        public object[] QuoteStandardClauses { get; set; }
        public object[] QuoteSurcharges { get; set; }
        public object[] ProjectDistances { get; set; }

    }
}
