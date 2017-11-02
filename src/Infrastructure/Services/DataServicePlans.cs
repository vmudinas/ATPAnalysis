using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Infrastructure.Entities;
using Infrastructure.Services.Abstraction;
using Infrastructure.UnitEntities;

namespace Infrastructure.Services
{
    public partial class DataService : IDataService
    {
        public IEnumerable<Plan> GetPlans(string userName)
        {
            return _repo.Get<Plan>(x => x.AccountId == GetDefaultAccountId(userName)).ToList();
        }

        public bool AddPlansFromUnit(PlanItem unitPlanItem)
        {
            var plans = GetPlansFromDb(unitPlanItem.AccountId);

            foreach (var up in unitPlanItem.Payload)
            {
                Plan pln;
                if (plans.Exists(p => p.PlanId == up.PlanId))
                {
                    pln = plans.FirstOrDefault(p => p.AccountId == unitPlanItem.AccountId && p.PlanId == up.PlanId);
                    var modifiedDateNotNullable = pln?.ModifiedDate ?? DateTime.MinValue;

                    // TBD - only latest get saved? Return list of Plan Ids that did not save (instead of "bool")?
                    if (DateTime.Compare(modifiedDateNotNullable, up.ModifiedDate) > 0) { continue; }

                    if (pln == null) continue;
                    pln.ModifiedDate = up.ModifiedDate;
                    pln.ModifiedBy = up.ModifiedBy;
                    pln.PlanName = up.PlanName;
                    pln.Quota = up.Quota;
                    pln.PreventRepeat = up.ShouldPreventRepeat;
                    pln.IsRandom = up.IsRandom;

                    pln.PlanLocations = GetPlanLocsFromUnitLocs(up);

                    if (pln.PlanUnits.All(pu => pu.UnitNo != unitPlanItem.UnitNo))
                    {
                        pln.PlanUnits = GetAppendedPlanUnitList(pln.PlanUnits, pln, unitPlanItem.UnitNo);
                    }

                    _repo.UpdateSave(pln);
                }
                else // insert new plan
                {
                    pln = GeneratePlanFromUnitPlan(unitPlanItem, up);
                    _repo.AddSave(pln);
                }
            }

            return true;
        }

        public List<UnitPlan> GetPlanPayload(Guid acctId, int unitNo)
        {
            var lstUnitPlan = new List<UnitPlan>();
            var lstPlanUnit = _repo.Get<PlanUnit>(pu => pu.UnitNo == unitNo).ToList();
            var lstPlanLocs = new List<PlanLocation>();

            if (lstPlanUnit.Count <= 0) return lstUnitPlan;
            var plansPerAccount = _repo.Get<Plan>(p => p.AccountId == acctId).ToList();

            foreach (var p in plansPerAccount)
            {
                if (!lstPlanUnit.Exists(planUnit => planUnit.PlanId == p.PlanId)) { continue; }

                var unitPlan = new UnitPlan
                {
                    IsRandom = p.IsRandom,
                    ModifiedBy = string.IsNullOrWhiteSpace(p.ModifiedBy) ? p.CreatedBy : p.ModifiedBy,
                    ModifiedDate = p.ModifiedDate.HasValue
                        ? Convert.ToDateTime(p.ModifiedDate, CultureInfo.InvariantCulture)
                        : Convert.ToDateTime(p.CreatedDate),
                    PlanId = p.PlanId,
                    PlanName = p.PlanName,
                    Quota = p.Quota,
                    ShouldPreventRepeat = p.PreventRepeat
                };
                lstPlanLocs.Clear();
                lstPlanLocs = _repo.Get<PlanLocation>(pl => pl.PlanId == p.PlanId).OrderBy(pl => pl.DisplayOrder).ToList();
                unitPlan.Locations = lstPlanLocs.Select(pl => pl.LocationId).ToList();
                unitPlan.RequiredLocations = lstPlanLocs.Where(pl => pl.IsRequired).Select(pl => pl.LocationId).ToList();

                lstUnitPlan.Add(unitPlan);
            }

            return lstUnitPlan;
        }

        public int GetPlanCountFromPlanIdList(List<Guid> planIds)
        {
            return _repo.Get<Plan>(p => planIds.Contains(p.PlanId)).ToList().Count();
        }

        public void DeletePlanList(List<Guid> planIds)
        {
            _repo.RemoveRange(_repo.Get<Plan>(p => planIds.Contains(p.PlanId)).ToList());

            _repo.Save();
        }

        public Guid GetPlanIdByIdAndModBy(Guid planId, string modifiedBy)
        {
            return _repo.Get<Plan>(p => p.PlanId == planId && p.ModifiedBy == modifiedBy).Select(pln => pln.PlanId).FirstOrDefault();
        }

        private List<Plan> GetPlansFromDb(Guid acctId)
        {
            var planIds = _repo.Get<Plan>(p => p.AccountId == acctId).ToList();

            return planIds;
        }

        private static IEnumerable<PlanUnit> GetAppendedPlanUnitList(IEnumerable<PlanUnit> lstPlanUnitIn, Plan planIn, int unitNo)
        {
            var lstPlanUnitReturn = new List<PlanUnit>();

            lstPlanUnitReturn.AddRange(lstPlanUnitIn);

            //foreach (var planUnit in lstPlanUnitIn) { lstPlanUnitReturn.Add(planUnit); }

            lstPlanUnitReturn.Add(new PlanUnit()
            {
                Id = Guid.NewGuid(),
                PlanId = planIn.PlanId,
                Quota = planIn.Quota,
                UnitNo = unitNo
            });

            return lstPlanUnitReturn;
        }

        private Plan GeneratePlanFromUnitPlan(PlanItem pi, UnitPlan up)
        {
            var retPlan = new Plan()
            {
                PlanId = up.PlanId,
                PlanName = up.PlanName,
                AccountId = pi.AccountId,
                Active = true,
                CreatedBy = up.ModifiedBy, // created by is actually unit modified by in this context (i.e. Plan does not already exist in DB)
                CreatedDate = up.ModifiedDate, // created date is actually unit modified date in this context
                IsRandom = up.IsRandom,
                ModifiedBy = string.Empty,
                ModifiedDate = null,
                PlanLocations = GetPlanLocsFromUnitLocs(up),
                PlanUnits = GetPlanUnitsFromUnit(pi.UnitNo, up),
                PreventRepeat = up.ShouldPreventRepeat,
                Quota = up.Quota
            };

            return retPlan;
        }

        private static IEnumerable<PlanLocation> GetPlanLocsFromUnitLocs(UnitPlan up)
        {
            var displayOrder = 0;
            var lstPlanLocations = new List<PlanLocation>();
            
            foreach (var ul in up.Locations)
            {
                var isRequired = up.RequiredLocations.Any(rl => rl.Equals(ul));

                lstPlanLocations.Add(new PlanLocation {
                    Id = Guid.NewGuid(),
                    IsRequired = isRequired,
                    LocationId = ul,
                    PlanId = up.PlanId,
                    DisplayOrder = displayOrder
                });

                displayOrder++;
            }

            return lstPlanLocations;
        }

        private static IEnumerable<PlanUnit> GetPlanUnitsFromUnit(int unitNo, UnitPlan up)
        {
            var lstPlanUnits = new List<PlanUnit>()
                { new PlanUnit { Id = Guid.NewGuid(), PlanId = up.PlanId, Quota = up.Quota, UnitNo = unitNo } };

            return lstPlanUnits;
        }

    }
}