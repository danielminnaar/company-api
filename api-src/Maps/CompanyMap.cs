using api_src.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace api_src.Maps{
       public class CompanyMap
    {
        public CompanyMap(EntityTypeBuilder<Company> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.ToTable("companies");

            entityBuilder.Property(x => x.Id).HasColumnName("id");
            entityBuilder.Property(x => x.Name).HasColumnName("name");
            entityBuilder.Property(x => x.Exchange).HasColumnName("exchange");
            entityBuilder.Property(x => x.ISIN).HasColumnName("isin");
            entityBuilder.Property(x => x.Ticker).HasColumnName("ticker");
            entityBuilder.Property(x => x.Website).HasColumnName("website");
        }
    }
}