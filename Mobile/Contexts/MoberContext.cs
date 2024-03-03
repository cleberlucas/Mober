
using Microsoft.EntityFrameworkCore;
using Mobile.Mappers;
using Mobile.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mobile.Repositories
{
    class MoberContext : DbContext
    {
        private readonly string _connectionString = "";

        public DbSet<User> Users { get; set; }

        public MoberContext(DbContextOptions<MoberContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapper());
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var serverVersion = new MySqlServerVersion(new Version(5, 7, 23));
            optionsBuilder
                .UseMySQL(_connectionString, mySqlOptions =>
                {
                    mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: System.TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
                });

            //.UseMySql(_connectionString, serverVersion, mySqlOptions =>
            //{
            //    mySqlOptions.EnableRetryOnFailure(
            //    maxRetryCount: 10,
            //    maxRetryDelay: System.TimeSpan.FromSeconds(10),
            //    errorNumbersToAdd: null);
            //});
        }
    }
}

//CREATE TABLE `xxx`.`usuario` (
//  `id` INT NOT NULL AUTO_INCREMENT,
//  `nome` VARCHAR(45) NULL,
//  `telefone` VARCHAR(20) NULL,
//  `longitude` DECIMAL(18,15)  NULL,
//  `latitude` DECIMAL(18,15)  NULL,
//  `servico` VARCHAR(45) NULL,
//  `prestador` TINYINT NULL,
//  `atualizado` DATETIME NULL,
//  PRIMARY KEY (`id`));
