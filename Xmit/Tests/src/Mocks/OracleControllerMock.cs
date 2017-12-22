using System.Collections.Generic;
using System.Text;
using HostForTransmitter.Code;
using mcTransmitter;
using OracleFirewall.DTOs;
using OracleFirewall.Interfaces;

namespace Tests {
    public class OracleControllerMock : IOracleController {
        public int DeleteItems(List<DeletedItem> deleteItems, ref StringBuilder sb, ref ResultDto dto) {
            return 0;
        }

        public void Dispose() {
            
        }

        public void ProcessCompanies(List<Company> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessCompanyBidStatuses(List<CompanyBidStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessCompanyOrderStatuses(List<CompanyOrderStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessCompanyProjectProductStatuses(List<CompanyProjectProductStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessCompanyProjectStatuses(List<CompanyProjectStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessCompanyQuoteStatuses(List<CompanyQuoteStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessCompanyUsers(List<CompanyUser> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessCompetitorPlants(List<CompetitorPlant> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessCompetitors(List<Competitor> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessContactNotes(List<ContactNote> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessContacts(List<Contact> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessCreditStatuses(List<CreditStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessCustomerEvents(List<CustomerEvent> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessCustomerEventTypes(List<CustomerEventType> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessCustomerTypes(List<CustomerType> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessDeviationTypes(List<DeviationType> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessDivisions(List<Division> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessEventFrequency(List<EventFrequency> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessEventStatus(List<EventStatu> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessLinesOfBusiness(List<LinesOfBusiness> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessLogins(List<Login> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessLostReasons(List<LostReason> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessManagersSalesmen(List<ManagersSalesman> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessOrderDetails(List<OrderDetail> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessOrders(List<Order> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessPaymentTerms(List<PaymentTerm> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessPlantLinesOfBusiness(List<PlantLinesOfBusiness> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessPlantProductPrices(List<PlantProductPrice> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessPlants(List<Plant> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProductLines(List<ProductLine> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProducts(List<Product> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProductTemplates(List<ProductTemplate> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProductTypes(List<ProductType> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProductUsages(List<ProductUsage> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProjectBidderCharges(List<ProjectBidderCharge> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProjectBidderNotes(List<ProjectBidderNote> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProjectBidderProducts(List<ProjectBidderProduct> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProjectBidders(List<ProjectBidder> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProjectBidderStatuses(List<ProjectBidderStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProjectCharges(List<ProjectCharge> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProjectChargeTypes(List<ProjectChargeType> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProjectDistances(List<ProjectDistance> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProjectMapQtyRanges(List<ProjectMapQtyRanx> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProjectNotes(List<ProjectNote> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProjectProducts(List<ProjectProduct> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProjects(List<Project> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProjectTypes(List<ProjectType> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProspectNotes(List<ProspectNote> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessProspects(List<Prospect> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessQuoteDetails(List<QuoteDetail> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessQuotes(List<Quote> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessQuoteSections(List<QuoteSection> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessQuoteStandardClauses(List<QuoteStandardClaus> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessQuoteStatuses(List<QuoteStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessQuoteSurcharges(List<QuoteSurcharge> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessQuoteTypes(List<QuoteType> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessRoles(List<Role> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessSalespersonContacts(List<SalespersonContact> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessSalesPersonRegions(List<SalesPersonRegion> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessScheduleItems(List<ScheduleItem> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessSchedules(List<Schedule> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessSources(List<Source> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessSourceSystems(List<SourceSystem> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessStandardClauses(List<StandardClaus> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessStatuses(List<Status> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessSurcharges(List<Surcharge> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessTemplatedProducts(List<TemplatedProduct> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessTerms_Discount(List<Terms_Discount> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessUnit_Of_Measure(List<Unit_Of_Measure> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessUserRoles(List<UserRole> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessUsers(List<User> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessVehicleTypes(List<VehicleType> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public void ProcessZoneCodes(List<ZoneCode> list, ref StringBuilder sb, ref ResultDto dto) {
            
        }

        public string ReturnSomethingFromDB() {
            return "Unit Test ReturnSomethingFromDB";
        }
    }
}
