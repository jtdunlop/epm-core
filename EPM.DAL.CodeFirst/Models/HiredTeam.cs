namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    using System;
    using System.Collections.Generic;
    using DBSoft.EPM.DAL.CodeFirst.Models;

    public class HiredTeam
    {
        public HiredTeam()
        {
            HiredTeamSpecialties = new List<HiredTeamSpecialty>();
        }

        public int ID { get; set; }
        public int TeamID { get; set; }
        public DateTime HireDate { get; set; }
        public decimal HireAmount { get; set; }
        public decimal MaterialBonusValue { get; set; }
        public decimal Salary { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string SpecialtyNames { get; set; }

        public virtual ICollection<ProductionJob> ProductionJobs { get; set; }
        public virtual ICollection<HiredTeamCost> HiredTeamCosts { get; set; }
        public virtual ICollection<HiredTeamAuction> HiredTeamAuctions { get; set; }
        public virtual ICollection<HiredTeamSpecialty> HiredTeamSpecialties { get; set; }
    }
}
