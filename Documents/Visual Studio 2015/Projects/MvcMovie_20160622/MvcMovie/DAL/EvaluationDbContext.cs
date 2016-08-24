﻿
using MvcMovie.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;

namespace MvcMovie.DAL
{
    public class EvaluationDbContext : DbContext
    {

        public EvaluationDbContext() : base("EvaluationDbContext")
        {
        }

        public DbSet<Evaluation> Evaluations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public static implicit operator EvaluationDbContext(Evaluation v)
        {
            throw new NotImplementedException();
        }
    }
}