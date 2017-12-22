using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web.Http;
using mcTransmitter;
using System;
using OracleFirewall.DTOs;
using OracleFirewall.Interfaces;
using HostForTransmitter.Code;
namespace OracleFirewall.Controllers {
    public class OracleController : ApiController, IOracleController {

        #region Constructors



        #endregion



        #region Destructors



        #endregion

        #region Private Members



        #endregion

        #region Private Methods

        private void ProcessEntityValidationError(DbEntityValidationResult eve, ref StringBuilder sb, IDto dto) {
            var xSb = new StringBuilder();
            sb.AppendLine(string.Format("{0}{1}{2}{3}{4}", "Entity of type ", eve.Entry.Entity.GetType().Name,
                    " in state ", eve.Entry.State, " has the following validation errors:"));
            xSb.AppendLine(string.Format("{0}{1}{2}{3}{4}", "Entity of type ", eve.Entry.Entity.GetType().Name,
            " in state ", eve.Entry.State, " has the following validation errors:"));
            foreach (var ve in eve.ValidationErrors) {
                sb.AppendLine(string.Format("{0}{1}{2}{3}", " - Property: ", ve.PropertyName, ", Error: ",
                                ve.ErrorMessage));
                xSb.AppendLine(string.Format("{0}{1}{2}{3}", " - Property: ", ve.PropertyName, ", Error: ",
                                ve.ErrorMessage));
            }
            dto.Message = xSb.ToString();
        }

        private void ProcessExceptionMessage(Exception ex, ref StringBuilder sb, IDto obj) {
            sb.AppendLine(ex.Message);
            obj.Message = ex.Message;
        }


        private void DeleteProjectProducts (List<DeletedItem> pp, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteProjectProducts");
            obj.RecordCount = pp.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in pp) {
                        var del = ctx.ProjectProducts.SingleOrDefault(x => x.ProjectProductID == item.KeyID);
                        if (del != null) {
                            ctx.ProjectProducts.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteProjectBidders (List<DeletedItem> pb, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteProjectBidders");
            obj.RecordCount = pb.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in pb) {
                        var del = ctx.ProjectBidders.SingleOrDefault(x => x.ProjectBidderID == item.KeyID);
                        if (del != null) {
                            obj.RecordsProcessed = ctx.SaveChanges();
                        }
                    }
                    ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteQuoteSections (List<DeletedItem> qSec, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteQuoteSections");
            obj.RecordCount = qSec.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in qSec) {
                        var del = ctx.QuoteSections.SingleOrDefault(x => x.QuoteSectionID == item.KeyID);
                        if (del != null) {
                            ctx.QuoteSections.Remove(del);                            
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteSchedules (List<DeletedItem> s, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteSchedules");
            obj.RecordCount = s.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in s) {
                        var del = ctx.Schedules.SingleOrDefault(x => x.ScheduleID == item.KeyID);
                        if (del != null) {
                            ctx.Schedules.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteUserRoles (List<DeletedItem> ur, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteUserRoles");
            obj.RecordCount = ur.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in ur) {
                        var del = ctx.UserRoles.SingleOrDefault(x => x.UserRoleID == item.KeyID);
                        if (del != null) {
                            ctx.UserRoles.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteTemplatedProducts (List<DeletedItem> tp, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteTemplatedProducts");
            obj.RecordCount = tp.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in tp) {
                        var del = ctx.TemplatedProducts.SingleOrDefault(x => x.TemplatedProductID == item.KeyID);
                        if (del != null) {
                            ctx.TemplatedProducts.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteScheduleItems (List<DeletedItem> si, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteScheduleItems");
            obj.RecordCount = si.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in si) {
                        var del = ctx.ScheduleItems.SingleOrDefault(x => x.ScheduleItemID == item.KeyID);
                        if (del != null) {
                            obj.RecordsProcessed = ctx.SaveChanges();
                        }
                    }
                    ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteSalesPersonRegions (List<DeletedItem> sr, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteSalesPersonRegions");
            obj.RecordCount = sr.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in sr) {
                        var del = ctx.SalesPersonRegions.SingleOrDefault(x => x.SalesPersonRegionID == item.KeyID);
                        if (del != null) {
                            ctx.SalesPersonRegions.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteSalespersonContacts (List<DeletedItem> sc, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteSalespersonContacts");
            obj.RecordCount = sc.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in sc) {
                        var del = ctx.SalespersonContacts.SingleOrDefault(x => x.SalesPersonContactID == item.KeyID);
                        if (del != null) {
                            ctx.SalespersonContacts.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteQuoteSurcharges (List<DeletedItem> qs, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteQuoteSurcharges");
            obj.RecordCount = qs.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in qs) {
                        var del = ctx.QuoteSurcharges.SingleOrDefault(x => x.QuoteSurchargeID == item.KeyID);
                        if (del != null) {
                            ctx.QuoteSurcharges.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteQuoteStandardClauses (List<DeletedItem> qsc, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteQuoteStandardClauses");
            obj.RecordCount = qsc.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in qsc) {
                        var del = ctx.QuoteStandardClauses.SingleOrDefault(x => x.QuoteStandardClauseID == item.KeyID);
                        if (del != null) {
                            ctx.QuoteStandardClauses.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteContacts (List<DeletedItem> c, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteContacts");
            obj.RecordCount = c.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in c) {
                        var del = ctx.Contacts.SingleOrDefault(x => x.ContactID == item.KeyID);
                        if (del != null) {
                            ctx.Contacts.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }


        private void DeleteProspects (List<DeletedItem> p, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteProspects");
            obj.RecordCount = p.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in p) {
                        var del = ctx.Prospects.SingleOrDefault(x => x.ProspectID == item.KeyID);
                        if (del != null) {
                            ctx.Prospects.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }


        private void DeleteQuotes (List<DeletedItem> q, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteQuotes");
            obj.RecordCount = q.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in q) {
                        var del = ctx.Quotes.SingleOrDefault(x => x.QuoteID == item.KeyID);
                        if (del != null) {
                            ctx.Quotes.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }



        private void DeleteQuoteDetails (List<DeletedItem> qd, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteQuoteDetails");
            obj.RecordCount = qd.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in qd) {
                        var del = ctx.QuoteDetails.SingleOrDefault(x => x.QuoteDetailID == item.KeyID);
                        if (del != null) {
                            ctx.QuoteDetails.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteProjectBidderProducts (List<DeletedItem> pbp, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteProjectBidderProducts");
            obj.RecordCount = pbp.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in pbp) {
                        var del = ctx.ProjectBidderProducts.SingleOrDefault(x => x.ProjectBidderID == item.KeyID);
                        if (del != null) {
                            ctx.ProjectBidderProducts.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteManagersSalesmen (List<DeletedItem> ms, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteManagersSalesmen");
            obj.RecordCount = ms.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in ms) {
                        var del = ctx.ManagersSalesmen.SingleOrDefault(x => x.ManagerUserID == item.KeyID);
                        if (del != null) {
                            ctx.ManagersSalesmen.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteProjectBidderCharges (List<DeletedItem> pbc, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteProjectBidderCharges");
            obj.RecordCount = pbc.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in pbc) {
                        var del = ctx.ProjectBidderCharges.SingleOrDefault(x => x.ProjectBidderChargeID == item.KeyID);
                        if (del != null) {
                            ctx.ProjectBidderCharges.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        private void DeleteCompanyUsers (List<DeletedItem> cu, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeleteCompanyUsers");
            obj.RecordCount = cu.Count;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in cu) {
                        var del = ctx.CompanyUsers.SingleOrDefault(x => x.CompanyUserID == item.KeyID);
                        if (del != null) {
                            ctx.CompanyUsers.Remove(del);
                        }
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        



        #endregion



        #region public Methods


        public int DeleteItems (List<DeletedItem> deleteItems, ref StringBuilder sb, ref ResultDto dto) {
            var pbc = deleteItems.Where(x => x.TableName == "ProjectBidderCharges").ToList();
            var pbp = deleteItems.Where(x => x.TableName == "ProjectBidderProducts").ToList();
            var si = deleteItems.Where(x => x.TableName == "ScheduleItems").ToList();
            var pb = deleteItems.Where(x => x.TableName == "ProjectBidders").ToList();
            var qd = deleteItems.Where(x => x.TableName == "QuoteDetails").ToList();
            var cu = deleteItems.Where(x => x.TableName == "CompanyUsers").ToList();
            var ms = deleteItems.Where(x => x.TableName == "ManagersSalesmen").ToList();
            var qs = deleteItems.Where(x => x.TableName == "QuoteSurcharges").ToList();
            var qsc = deleteItems.Where(x => x.TableName == "QuoteStandardClauses").ToList();
            var sc = deleteItems.Where(x => x.TableName == "SalespersonContacts").ToList();
            var sr = deleteItems.Where(x => x.TableName == "SalesPersonRegions").ToList();
            var tp = deleteItems.Where(x => x.TableName == "TemplatedProducts").ToList();
            var ur = deleteItems.Where(x => x.TableName == "UserRoles").ToList();
            var s = deleteItems.Where(x => x.TableName == "Schedules").ToList();
            var qSec = deleteItems.Where(x => x.TableName == "QuoteSections").ToList();
            var pp = deleteItems.Where(x => x.TableName == "ProjectProducts").ToList();
            var c = deleteItems.Where(x => x.TableName == "Contacts"
                                        || x.TableName == "Contact").ToList();
            var p = deleteItems.Where(x => x.TableName == "Prospects").ToList();
            var q = deleteItems.Where(x => x.TableName == "Quotes").ToList();
            try {
                if (cu.Any()) {
                    DeleteCompanyUsers(cu, ref sb, ref dto);
                }
                if (pbc.Any()) {
                    DeleteProjectBidderCharges(pbc, ref sb, ref dto);
                }
                if (ms.Any()) {
                    DeleteManagersSalesmen(ms, ref sb, ref dto);
                }
                if (pbp.Any()) {
                    DeleteProjectBidderProducts(pbp, ref sb, ref dto);
                }
                if (qd.Any()) {
                    DeleteQuoteDetails(qd, ref sb, ref dto);
                }
                if (qs.Any()) {
                    DeleteQuoteSurcharges(qs, ref sb, ref dto);
                }
                if (qsc.Any()) {
                    DeleteQuoteStandardClauses(qsc, ref sb, ref dto);
                }
                if (c.Any()) {
                    DeleteContacts(c, ref sb, ref dto);
                }
                if (p.Any()) {
                    DeleteProspects(p, ref sb, ref dto);
                }
                if (sc.Any()) {
                    DeleteSalespersonContacts(sc, ref sb, ref dto);
                }
                if (sr.Any()) {
                    DeleteSalesPersonRegions(sr, ref sb, ref dto);
                }
                if (si.Any()) {
                    DeleteScheduleItems(si, ref sb, ref dto);
                }
                if (tp.Any()) {
                    DeleteTemplatedProducts(tp, ref sb, ref dto);
                }
                if (ur.Any()) {
                    DeleteUserRoles(ur, ref sb, ref dto);
                }
                if (s.Any()) {
                    DeleteSchedules(s, ref sb, ref dto);
                }
                if (qSec.Any()) {
                    DeleteQuoteSections(qSec, ref sb, ref dto);
                }
                if (pb.Any()) {
                    DeleteProjectBidders(pb, ref sb, ref dto);
                }
                if (pp.Any()) {
                    DeleteProjectProducts(pp, ref sb, ref dto);
                }
                if (q.Any()) {
                    DeleteQuotes(q, ref sb, ref dto);
                }
                return 1;
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, dto);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, dto);
                throw;
            }

        }




        public string ReturnSomethingFromDB () {
            try {
                using (var ctx = new Entities()) {
                    if (ctx.SourceSystems.Where(x => x.SourceSystemID == 999).Count() == 0) {
                        return "I think I connected";
                    } else {
                        return "Oh dang";
                    }
                }
            }
            catch (Exception e) {
                return e.Message;
            }

        }

        public void ProcessSourceSystems(List<SourceSystem> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "SourceSystems");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.SourceSystems.SingleOrDefault(x => x.SourceSystemID == item.SourceSystemID);
                        if (del != null) {
                            ctx.SourceSystems.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.SourceSystems.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessRoles(List<Role> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Roles");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Roles.SingleOrDefault(x => x.RoleID == item.RoleID);
                        if (del != null) {
                            ctx.Roles.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Roles.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessStatuses(List<Status> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Statuses");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Statuses.SingleOrDefault(x => x.StatusID == item.StatusID);
                        if (del != null) {
                            ctx.Statuses.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Statuses.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessQuoteTypes(List<QuoteType> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "QuoteTypes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.QuoteTypes.SingleOrDefault(x => x.QuoteTypeID == item.QuoteTypeID);
                        if (del != null) {
                            ctx.QuoteTypes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.QuoteTypes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessQuoteStatuses(List<QuoteStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "QuoteStatuses");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.QuoteStatuses.SingleOrDefault(x => x.QuoteStatusID == item.QuoteStatusID);
                        if (del != null) {
                            ctx.QuoteStatuses.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.QuoteStatuses.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessQuoteSections(List<QuoteSection> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "QuoteSections");
            decimal id = 0;
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.QuoteSections.SingleOrDefault(x => x.QuoteSectionID == item.QuoteSectionID);
                        if (del != null) {
                            ctx.QuoteSections.Remove(del);
                        }
                        ctx.SaveChanges();
                    }
                    var count = 0;
                    foreach (var item in list) {
                        id = item.QuoteSectionID;
                        ctx.QuoteSections.Add(item);
                        count += ctx.SaveChanges();
                    }
                    obj.RecordsProcessed = count;
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProjectChargeTypes(List<ProjectChargeType> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProjectChargeTypes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProjectChargeTypes.SingleOrDefault(x => x.ChargeTypeID == item.ChargeTypeID);
                        if (del != null) {
                            ctx.ProjectChargeTypes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProjectChargeTypes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProjectBidderStatuses(List<ProjectBidderStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProjectBidderStatuses");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProjectBidderStatuses.SingleOrDefault(x => x.ProjectBidderStatusID == item.ProjectBidderStatusID);
                        if (del != null) {
                            ctx.ProjectBidderStatuses.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProjectBidderStatuses.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessEventStatus(List<EventStatu> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "EventStatus");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.EventStatus.SingleOrDefault(x => x.EventStatusID == item.EventStatusID);
                        if (del != null) {
                            ctx.EventStatus.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.EventStatus.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessDeviationTypes(List<DeviationType> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "DeviationTypes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.DeviationTypes.SingleOrDefault(x => x.DeviationTypeID == item.DeviationTypeID);
                        if (del != null) {
                            ctx.DeviationTypes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.DeviationTypes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessUnit_Of_Measure(List<Unit_Of_Measure> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Unit_Of_Measure");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Unit_Of_Measure.SingleOrDefault(x => x.Unit_Of_Measure_ID == item.Unit_Of_Measure_ID);
                        if (del != null) {
                            ctx.Unit_Of_Measure.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Unit_Of_Measure.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessCompanies(List<Company> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Companies");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Companies.SingleOrDefault(x => x.CompanyID == item.CompanyID);
                        if (del != null) {
                            ctx.Companies.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Companies.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessCompanyOrderStatuses(List<CompanyOrderStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "CompanyOrderStatuses");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.CompanyOrderStatuses.SingleOrDefault(x => x.CompanyOrderStatusID == item.CompanyOrderStatusID);
                        if (del != null) {
                            ctx.CompanyOrderStatuses.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.CompanyOrderStatuses.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessCompanyProjectProductStatuses(List<CompanyProjectProductStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "CompanyProjectProductStatuses");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.CompanyProjectProductStatuses.SingleOrDefault(x => x.CompanyProjectProductStatusID == item.CompanyProjectProductStatusID);
                        if (del != null) {
                            ctx.CompanyProjectProductStatuses.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.CompanyProjectProductStatuses.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessCompanyProjectStatuses(List<CompanyProjectStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "CompanyProjectStatuses");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.CompanyProjectStatuses.SingleOrDefault(x => x.CompanyProjectStatusID == item.CompanyProjectStatusID);
                        if (del != null) {
                            ctx.CompanyProjectStatuses.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.CompanyProjectStatuses.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessCompanyQuoteStatuses(List<CompanyQuoteStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "CompanyQuoteStatuses");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.CompanyQuoteStatuses.SingleOrDefault(x => x.CompanyQuoteStatusID == item.CompanyQuoteStatusID);
                        if (del != null) {
                            ctx.CompanyQuoteStatuses.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.CompanyQuoteStatuses.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessCreditStatuses(List<CreditStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "CreditStatuses");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.CreditStatuses.SingleOrDefault(x => x.CreditStatusID == item.CreditStatusID);
                        if (del != null) {
                            ctx.CreditStatuses.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.CreditStatuses.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessCustomerEventTypes(List<CustomerEventType> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "CustomerEventTypes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.CustomerEventTypes.SingleOrDefault(x => x.EventTypeID == item.EventTypeID);
                        if (del != null) {
                            ctx.CustomerEventTypes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.CustomerEventTypes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessCustomerTypes(List<CustomerType> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "CustomerTypes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.CustomerTypes.SingleOrDefault(x => x.CustomerTypeID == item.CustomerTypeID);
                        if (del != null) {
                            ctx.CustomerTypes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.CustomerTypes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessDivisions(List<Division> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Divisions");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Divisions.SingleOrDefault(x => x.DivisionID == item.DivisionID);
                        if (del != null) {
                            ctx.Divisions.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Divisions.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessEventFrequency(List<EventFrequency> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "EventFrequency");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.EventFrequencies.SingleOrDefault(x => x.EventFrequencyID == item.EventFrequencyID);
                        if (del != null) {
                            ctx.EventFrequencies.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.EventFrequencies.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessLinesOfBusiness(List<LinesOfBusiness> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "LinesOfBusiness");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.LinesOfBusinesses.SingleOrDefault(x => x.LineOfBusinessID == item.LineOfBusinessID);
                        if (del != null) {
                            ctx.LinesOfBusinesses.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.LinesOfBusinesses.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessLostReasons(List<LostReason> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "LostReasons");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.LostReasons.SingleOrDefault(x => x.LostReasonID == item.LostReasonID);
                        if (del != null) {
                            ctx.LostReasons.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.LostReasons.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessPaymentTerms(List<PaymentTerm> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "PaymentTerms");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.PaymentTerms.SingleOrDefault(x => x.Terms_Discount_ID == item.Terms_Discount_ID);
                        if (del != null) {
                            ctx.PaymentTerms.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.PaymentTerms.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProjectMapQtyRanges(List<ProjectMapQtyRanx> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProjectMapQtyRanges");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProjectMapQtyRanges.SingleOrDefault(x => x.ProjectMapQtyRangeID == item.ProjectMapQtyRangeID);
                        if (del != null) {
                            ctx.ProjectMapQtyRanges.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProjectMapQtyRanges.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProjectTypes(List<ProjectType> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProjectTypes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProjectTypes.SingleOrDefault(x => x.ProjectTypeID == item.ProjectTypeID);
                        if (del != null) {
                            ctx.ProjectTypes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProjectTypes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges(); ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessSources(List<Source> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Sources");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Sources.SingleOrDefault(x => x.SourceID == item.SourceID);
                        if (del != null) {
                            ctx.Sources.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Sources.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges(); ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessTerms_Discount(List<Terms_Discount> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Terms_Discount");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Terms_Discount.SingleOrDefault(x => x.Terms_Discount_ID == item.Terms_Discount_ID);
                        if (del != null) {
                            ctx.Terms_Discount.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Terms_Discount.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges(); ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessVehicleTypes(List<VehicleType> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "VehicleTypes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.VehicleTypes.SingleOrDefault(x => x.VehicleTypeID == item.VehicleTypeID);
                        if (del != null) {
                            ctx.VehicleTypes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.VehicleTypes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessZoneCodes(List<ZoneCode> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ZoneCodes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ZoneCodes.SingleOrDefault(x => x.ZoneCodeID == item.ZoneCodeID);
                        if (del != null) {
                            ctx.ZoneCodes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ZoneCodes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessCompanyBidStatuses(List<CompanyBidStatus> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "CompanyBidStatuses");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.CompanyBidStatuses.SingleOrDefault(x => x.CompanyBidStatusID == item.CompanyBidStatusID);
                        if (del != null) {
                            ctx.CompanyBidStatuses.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.CompanyBidStatuses.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessPlants(List<Plant> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Plants");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Plants.SingleOrDefault(x => x.PlantID == item.PlantID);
                        if (del != null) {
                            ctx.Plants.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Plants.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessPlantLinesOfBusiness(List<PlantLinesOfBusiness> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "PlantLinesOfBusiness");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.PlantLinesOfBusinesses.SingleOrDefault(x => x.PlantLineOfBusinessID == item.PlantLineOfBusinessID);
                        if (del != null) {
                            ctx.PlantLinesOfBusinesses.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.PlantLinesOfBusinesses.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProductLines(List<ProductLine> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProductLines");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProductLines.SingleOrDefault(x => x.ProductLineID == item.ProductLineID);
                        if (del != null) {
                            ctx.ProductLines.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProductLines.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProductTypes(List<ProductType> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProductTypes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProductTypes.SingleOrDefault(x => x.ProductTypeID == item.ProductTypeID);
                        if (del != null) {
                            ctx.ProductTypes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProductTypes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProductUsages(List<ProductUsage> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProductUsages");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProductUsages.SingleOrDefault(x => x.ProductUsageID == item.ProductUsageID);
                        if (del != null) {
                            ctx.ProductUsages.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProductUsages.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessSurcharges(List<Surcharge> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Surcharges");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Surcharges.SingleOrDefault(x => x.SurchargeID == item.SurchargeID);
                        if (del != null) {
                            ctx.Surcharges.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Surcharges.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProducts(List<Product> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Products");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Products.SingleOrDefault(x => x.ProductID == item.ProductID);
                        if (del != null) {
                            ctx.Products.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Products.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessPlantProductPrices(List<PlantProductPrice> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "PlantProductPrices");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.PlantProductPrices.SingleOrDefault(x => x.PlantProductPriceID == item.PlantProductPriceID);
                        if (del != null) {
                            ctx.PlantProductPrices.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.PlantProductPrices.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProjectCharges(List<ProjectCharge> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProjectCharges");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProjectCharges.SingleOrDefault(x => x.ChargeID == item.ChargeID);
                        if (del != null) {
                            ctx.ProjectCharges.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProjectCharges.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessStandardClauses(List<StandardClaus> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "StandardClauses");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.StandardClauses.SingleOrDefault(x => x.StandardClauseID == item.StandardClauseID);
                        if (del != null) {
                            ctx.StandardClauses.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.StandardClauses.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessUsers(List<User> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Users");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Users.SingleOrDefault(x => x.UserID == item.UserID);
                        if (del != null) {
                            ctx.Users.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Users.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessUserRoles(List<UserRole> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "UserRoles");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.UserRoles.SingleOrDefault(x => x.UserRoleID == item.UserRoleID);
                        if (del != null) {
                            ctx.UserRoles.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.UserRoles.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessCompanyUsers(List<CompanyUser> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "CompanyUsers");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.CompanyUsers.SingleOrDefault(x => x.CompanyUserID == item.CompanyUserID);
                        if (del != null) {
                            ctx.CompanyUsers.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.CompanyUsers.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessCompetitors(List<Competitor> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Competitors");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Competitors.SingleOrDefault(x => x.CompetitorID == item.CompetitorID);
                        if (del != null) {
                            ctx.Competitors.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Competitors.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessCompetitorPlants(List<CompetitorPlant> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "CompetitorPlants");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.CompetitorPlants.SingleOrDefault(x => x.CompetitorPlantID == item.CompetitorPlantID);
                        if (del != null) {
                            ctx.CompetitorPlants.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.CompetitorPlants.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessLogins(List<Login> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Logins");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Logins.SingleOrDefault(x => x.LoginID == item.LoginID);
                        if (del != null) {
                            ctx.Logins.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Logins.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessManagersSalesmen(List<ManagersSalesman> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ManagersSalesmen");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ManagersSalesmen.SingleOrDefault(x => x.ManagerUserID == item.ManagerUserID);
                        if (del != null) {
                            ctx.ManagersSalesmen.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ManagersSalesmen.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProductTemplates(List<ProductTemplate> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProductTemplates");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProductTemplates.SingleOrDefault(x => x.ProductTemplateID == item.ProductTemplateID);
                        if (del != null) {
                            ctx.ProductTemplates.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProductTemplates.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessTemplatedProducts(List<TemplatedProduct> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "TemplatedProducts");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.TemplatedProducts.SingleOrDefault(x => x.TemplatedProductID == item.TemplatedProductID);
                        if (del != null) {
                            ctx.TemplatedProducts.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.TemplatedProducts.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProspects(List<Prospect> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Prospects");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Prospects.SingleOrDefault(x => x.ProspectID == item.ProspectID);
                        if (del != null) {
                            ctx.Prospects.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Prospects.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProspectNotes(List<ProspectNote> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProspectNotes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProspectNotes.SingleOrDefault(x => x.ProspectNoteID == item.ProspectNoteID);
                        if (del != null) {
                            ctx.ProspectNotes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProspectNotes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessSalesPersonRegions(List<SalesPersonRegion> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "SalesPersonRegions");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.SalesPersonRegions.SingleOrDefault(x => x.SalesPersonRegionID == item.SalesPersonRegionID);
                        if (del != null) {
                            ctx.SalesPersonRegions.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.SalesPersonRegions.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessContacts(List<Contact> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Contacts");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Contacts.SingleOrDefault(x => x.ContactID == item.ContactID);
                        if (del != null) {
                            ctx.Contacts.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Contacts.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessContactNotes(List<ContactNote> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ContactNotes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ContactNotes.SingleOrDefault(x => x.ContactNoteID == item.ContactNoteID);
                        if (del != null) {
                            ctx.ContactNotes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ContactNotes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProjects(List<Project> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Projects");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Projects.SingleOrDefault(x => x.ProjectID == item.ProjectID);
                        if (del != null) {
                            ctx.Projects.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Projects.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessCustomerEvents(List<CustomerEvent> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "CustomerEvents");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.CustomerEvents.SingleOrDefault(x => x.CustomerEventID == item.CustomerEventID);
                        if (del != null) {
                            ctx.CustomerEvents.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.CustomerEvents.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProjectProducts(List<ProjectProduct> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProjectProducts");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProjectProducts.SingleOrDefault(x => x.ProjectProductID == item.ProjectProductID);
                        if (del != null) {
                            ctx.ProjectProducts.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProjectProducts.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessSchedules(List<Schedule> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Schedules");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Schedules.SingleOrDefault(x => x.ScheduleID == item.ScheduleID);
                        if (del != null) {
                            ctx.Schedules.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Schedules.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessScheduleItems(List<ScheduleItem> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ScheduleItems");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ScheduleItems.SingleOrDefault(x => x.ScheduleItemID == item.ScheduleItemID);
                        if (del != null) {
                            ctx.ScheduleItems.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ScheduleItems.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessSalespersonContacts(List<SalespersonContact> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "SalespersonContacts");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.SalespersonContacts.SingleOrDefault(x => x.SalesPersonContactID == item.SalesPersonContactID);
                        if (del != null) {
                            ctx.SalespersonContacts.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.SalespersonContacts.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProjectNotes(List<ProjectNote> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProjectNotes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProjectNotes.SingleOrDefault(x => x.NoteID == item.NoteID);
                        if (del != null) {
                            ctx.ProjectNotes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProjectNotes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProjectBidders(List<ProjectBidder> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProjectBidders");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProjectBidders.SingleOrDefault(x => x.ProjectBidderID == item.ProjectBidderID);
                        if (del != null) {
                            ctx.ProjectBidders.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProjectBidders.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProjectBidderProducts(List<ProjectBidderProduct> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProjectBidderProducts");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProjectBidderProducts.SingleOrDefault(x => x.ProjectBidderProductID == item.ProjectBidderProductID);
                        if (del != null) {
                            ctx.ProjectBidderProducts.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProjectBidderProducts.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProjectBidderNotes(List<ProjectBidderNote> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProjectBidderNotes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProjectBidderNotes.SingleOrDefault(x => x.NoteID == item.NoteID);
                        if (del != null) {
                            ctx.ProjectBidderNotes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProjectBidderNotes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProjectBidderCharges(List<ProjectBidderCharge> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProjectBidderCharges");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProjectBidderCharges.SingleOrDefault(x => x.ProjectBidderChargeID == item.ProjectBidderChargeID);
                        if (del != null) {
                            ctx.ProjectBidderCharges.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProjectBidderCharges.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessOrders(List<Order> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Orders");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Orders.SingleOrDefault(x => x.OrderID == item.OrderID);
                        if (del != null) {
                            ctx.Orders.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Orders.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessOrderDetails(List<OrderDetail> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "OrderDetails");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.OrderDetails.SingleOrDefault(x => x.OrderDetailID == item.OrderDetailID);
                        if (del != null) {
                            ctx.OrderDetails.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.OrderDetails.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessQuotes(List<Quote> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "Quotes");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.Quotes.SingleOrDefault(x => x.QuoteID == item.QuoteID);
                        if (del != null) {
                            ctx.Quotes.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.Quotes.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessQuoteDetails(List<QuoteDetail> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "QuoteDetails");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.QuoteDetails.SingleOrDefault(x => x.QuoteDetailID == item.QuoteDetailID);
                        if (del != null) {
                            ctx.QuoteDetails.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.QuoteDetails.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessQuoteStandardClauses(List<QuoteStandardClaus> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "QuoteStandardClauses");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.QuoteStandardClauses.SingleOrDefault(x => x.QuoteStandardClauseID == item.QuoteStandardClauseID);
                        if (del != null) {
                            ctx.QuoteStandardClauses.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.QuoteStandardClauses.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessQuoteSurcharges(List<QuoteSurcharge> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "QuoteSurcharges");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.QuoteSurcharges.SingleOrDefault(x => x.QuoteSurchargeID == item.QuoteSurchargeID);
                        if (del != null) {
                            ctx.QuoteSurcharges.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.QuoteSurcharges.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        public void ProcessProjectDistances(List<ProjectDistance> list, ref StringBuilder sb, ref ResultDto dto) {
            var obj = dto.Records.Single(x => x.TableName == "ProjectDistances");
            try {
                using (var ctx = new Entities()) {
                    foreach (var item in list) {
                        var del = ctx.ProjectDistances.SingleOrDefault(x => x.ProjectDistanceID == item.ProjectDistanceID);
                        if (del != null) {
                            ctx.ProjectDistances.Remove(del);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var item in list) {
                        ctx.ProjectDistances.Add(item);
                    }
                    obj.RecordsProcessed = ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    ProcessEntityValidationError(eve, ref sb, obj);
                }
                throw;
            }
            catch (Exception ex) {
                ProcessExceptionMessage(ex, ref sb, obj);
                throw;
            }
        }

        #endregion
    }
}
